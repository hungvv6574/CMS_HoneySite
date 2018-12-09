using CMSSolutions.Web.Routing;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    using System;
    using System.Web.Mvc;
    using CMSSolutions;
    using Web.Mvc;
    using Web.Themes;
    using Web.UI.ControlForms;
    using Entities;
    using Models;
    using Web;
    using Web.UI.Navigation;

    [Authorize]
    [Themed(IsDashboard=true)]
    public class AdminArticlesController : BaseAdminController
    {
        public AdminArticlesController(IWorkContextAccessor workContextAccessor) : 
                base(workContextAccessor)
        {
            this.TableName = "tblArticles";
        }
        
        [Url("admin/articles")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý bài viết"), Url = "#" });
            var result = new ControlGridFormResult<ArticlesInfo>();
            var siteSettings = WorkContext.Resolve<SiteSettings>();

            result.Title = this.T("Quản lý bài viết");
            result.CssClass = "table table-bordered table-striped";
            result.UpdateActionName = "Update";
            result.IsAjaxSupported = true;
            result.DefaultPageSize = siteSettings.DefaultPageSize;
            result.EnablePaginate = true;
            result.FetchAjaxSource = this.GetArticles;
            result.GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml;
            result.GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml;
            result.ClientId = TableName;
            result.ActionsColumnWidth = 180;

            result.AddCustomVar(Extensions.Constants.SearchText, "$('#" + Extensions.Constants.SearchText + "').val();", true);

            result.AddColumn(x => x.Id, T("ID")).AlignCenter().HasWidth(100);
            result.AddColumn(x => x.Image, T("Ảnh đại diện"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsImage(y => y.Image, Extensions.Constants.CssThumbsSize);
            result.AddColumn(x => x.Title, T("Tiêu đề"));
            result.AddColumn(x => x.CategoryName, T("Chuyên mục"));
            result.AddColumn(x => x.CreateDate.ToString(Extensions.Constants.DateTimeFomat), T("Ngày tạo"));
            result.AddColumn(x => x.IsPublished)
                .HasHeaderText(T("Đã đăng"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage();

            result.AddAction().HasText(this.T("Create")).HasUrl(this.Url.Action("Edit", new { id = 0 })).HasButtonStyle(ButtonStyle.Primary).HasBoxButton(false).HasCssClass(Constants.RowLeft).HasRow(true);
            result.AddRowAction().HasText(this.T("Edit")).HasUrl(x => Url.Action("Edit", new { id = x.Id })).HasButtonStyle(ButtonStyle.Default).HasButtonSize(ButtonSize.ExtraSmall);
            result.AddRowAction(true).HasText(this.T("Delete")).HasName("Delete").HasValue(x => Convert.ToString(x.Id)).HasButtonStyle(ButtonStyle.Danger).HasButtonSize(ButtonSize.ExtraSmall).HasConfirmMessage(this.T(Constants.Messages.ConfirmDeleteRecord));

            result.AddAction(new ControlFormHtmlAction(BuildSearchText)).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");
            return result;
        }
        
        private ControlGridAjaxData<ArticlesInfo> GetArticles(ControlGridFormRequest options)
        {
            var searchText = string.Empty;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SearchText]))
            {
                searchText = Request.Form[Extensions.Constants.SearchText];
            }

            var languageCode = WorkContext.CurrentCulture;
            var categoryId = 0;
            
            int totals;
            var service = WorkContext.Resolve<IArticlesService>();
            service.LanguageCode = languageCode;
            service.CategoryId = categoryId;
            var records = service.SearchPaged(searchText, options.PageIndex, options.PageSize, out totals);

            return new ControlGridAjaxData<ArticlesInfo>(records, totals);
        }
        
        [Url("admin/articles/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý bài viết"), Url = "#" });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin bài viết"), Url = Url.Action("Index") });
            var model = new ArticlesModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<IArticlesService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<ArticlesModel>(model);
            result.Title = this.T("Thông tin bài viết");
            result.FormMethod = FormMethod.Post;
            result.UpdateActionName = "Update";
            result.ShowCancelButton = false;
            result.ShowBoxHeader = false;
            result.FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml;
            result.FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml;

            result.RegisterFileUploadOptions("BannerUrl.FileName", new ControlFileUploadOptions
            {
                AllowedExtensions = "jpg,jpeg,png,gif"
            });

            result.RegisterFileUploadOptions("Image.FileName", new ControlFileUploadOptions
            {
                AllowedExtensions = "jpg,jpeg,png,gif"
            });

            result.AddAction().HasText(this.T("Clear")).HasUrl(this.Url.Action("Edit", RouteData.Values.Merge(new { id = 0 }))).HasButtonStyle(ButtonStyle.Success);
            result.AddAction().HasText(this.T("Back")).HasUrl(this.Url.Action("Index")).HasButtonStyle(ButtonStyle.Danger);

            result.RegisterExternalDataSource(x => x.CategoryId, y => BindCategories());

            return result;
        }

        [HttpPost()]
        [FormButton("Save")]
        [ValidateInput(false)]
        [Url("admin/articles/update")]
        public ActionResult Update(ArticlesModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }
            var service = WorkContext.Resolve<IArticlesService>();
            service.CategoryId = model.CategoryId;
            service.LanguageCode = WorkContext.CurrentCulture;
            ArticlesInfo item = model.Id == 0 ? new ArticlesInfo() : service.GetById(model.Id);
            var alias = Utilities.GetAlias(model.Title);
            alias = GetAlias(item.Id, alias, alias, WorkContext.CurrentCulture);

            var refId = model.RefId;
            if (string.IsNullOrEmpty(refId))
            {
                refId = DateTime.Now.ToString(Extensions.Constants.DateTimeFomatFullNone);
            }
            item.LanguageCode = WorkContext.CurrentCulture;
            item.CategoryId = model.CategoryId;
            item.Title = model.Title;
            item.Alias = alias;
            item.Summary = model.Summary;
            item.Contents = model.Contents;
            item.CreateDate = DateTime.Now.Date;
            item.CreateByUser = WorkContext.CurrentUser.Id;
            item.IsPublished = model.IsPublished;
            item.PublishedDate = DateTime.Now;
            item.RefId = refId;
            item.VideoUrl = string.Empty;
            item.BannerUrl = model.BannerUrl;
            item.Image = model.Image;
            if (string.IsNullOrEmpty(model.Image) || model.Image == "#" || model.Image == "/")
            {
                item.Image = Extensions.Constants.ImageDefault;
            }
            item.ViewCount = model.ViewCount;
            item.Description = model.Description;
            item.Tags = model.Tags;
            item.IsDeleted = model.IsDeleted;
            service.Save(item);
           
            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE").Alert(T("Đã cập nhật thành công."));
        }

        private int iIndex = 1;
        private string GetAlias(int id, string alias, string aliasSource, string languageCode)
        {
            var service = WorkContext.Resolve<IArticlesService>();
            if (service.CheckAlias(id, alias, languageCode))
            {
                alias = aliasSource + "-" + iIndex;
                if (service.CheckAlias(id, alias, languageCode))
                {
                    iIndex++;
                    alias = GetAlias(id, alias, aliasSource, languageCode);
                }

                return alias;
            }

            return alias;
        }

        [ActionName("Update")]
        [FormButton("Delete")]
        public ActionResult Delete(int id)
        {
            var service = WorkContext.Resolve<IArticlesService>();
            var model = service.GetById(id);
            service.Delete(model);

            return new AjaxResult().NotifyMessage("DELETE_ENTITY_COMPLETE").Alert(T("Đã xóa bài viết thành công."));
        }
    }
}
