using System.Collections.Generic;
using System.Linq;
using CMSSolutions.Web.Routing;
using CMSSolutions.Websites.Extensions;

namespace CMSSolutions.Websites.Controllers
{
    using System;
    using System.Web.Mvc;
    using CMSSolutions;
    using CMSSolutions.Web.Mvc;
    using CMSSolutions.Web.Themes;
    using CMSSolutions.Web.UI.ControlForms;
    using CMSSolutions.Websites.Entities;
    using CMSSolutions.Websites.Models;
    using CMSSolutions.Websites.Services;
    using CMSSolutions.Web;
    using CMSSolutions.Web.UI.Navigation;

    [Authorize]
    [Themed(IsDashboard=true)]
    public class AdminCategoriesController : BaseAdminController
    {
        public AdminCategoriesController(IWorkContextAccessor workContextAccessor) : 
                base(workContextAccessor)
        {
            this.TableName = "tblCategories";
        }
        
        [Url("admin/categories")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý chuyên mục"), Url = "#" });
            var result = new ControlGridFormResult<CategoryInfo>();
            var siteSettings = WorkContext.Resolve<SiteSettings>();

            result.Title = this.T("Quản lý chuyên mục");
            result.CssClass = "table table-bordered table-striped";
            result.UpdateActionName = "Update";
            result.IsAjaxSupported = true;
            result.DefaultPageSize = siteSettings.DefaultPageSize;
            result.EnablePaginate = true;
            result.FetchAjaxSource = GetCategories;
            result.GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml;
            result.GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml;
            result.ClientId = TableName;
            result.ActionsColumnWidth = 180;

            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.StatusId + "').val();", true);

            result.AddColumn(x => x.Id, T("Mã"))
                .AlignCenter()
                .HasWidth(60);
            result.AddColumn(x => x.ParentName, T("Chuyên mục cha")).HasWidth(150);
            result.AddColumn(x => x.ShortName, T("Tên chuyên mục")).HasWidth(150);
            result.AddColumn(x => x.Url, T("Đường dẫn Url"));
            result.AddColumn(x => x.IsActived)
                .HasHeaderText(T("Hiển thị"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage();

            result.AddAction().HasText(this.T("Create")).HasUrl(this.Url.Action("Edit", new { id = 0 })).HasButtonStyle(ButtonStyle.Primary).HasBoxButton(false).HasCssClass(Constants.RowLeft).HasRow(true);
            result.AddRowAction().HasText(this.T("Edit")).HasUrl(x => Url.Action("Edit", new { id = x.Id })).HasButtonStyle(ButtonStyle.Default).HasButtonSize(ButtonSize.ExtraSmall);
            //result.AddRowAction(true).HasText(this.T("Delete")).HasName("Delete").HasValue(x => Convert.ToString(x.Id)).HasButtonStyle(ButtonStyle.Danger).HasButtonSize(ButtonSize.ExtraSmall).HasConfirmMessage(this.T(Constants.Messages.ConfirmDeleteRecord));

            result.AddAction(new ControlFormHtmlAction(BuildStatus)).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");
            return result;
        }

        private ControlGridAjaxData<CategoryInfo> GetCategories(ControlGridFormRequest options)
        {
            int totals;
            var service = WorkContext.Resolve<ICategoriesService>();
            var languageCode = WorkContext.CurrentCulture;

            bool status = false;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.StatusId]))
            {
                var statusId = Convert.ToInt32(Request.Form[Extensions.Constants.StatusId]);
                switch ((Status)statusId)
                {
                    case Status.Deleted:
                        status = true;
                        break;
                }
            }

            service.LanguageCode = languageCode;
            var list = service.GetPaged(status, options.PageIndex, options.PageSize, out totals);
            return new ControlGridAjaxData<CategoryInfo>(list, totals);
        }
        
        [Url("admin/categories/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý chuyên mục"), Url = "#" });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin chuyên mục"), Url = Url.Action("Index") });
            var model = new CategoriesModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<ICategoriesService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<CategoriesModel>(model);
            result.Title = this.T("Thông tin chuyên mục");
            result.FormMethod = FormMethod.Post;
            result.UpdateActionName = "Update";
            result.ShowCancelButton = false;
            result.ShowBoxHeader = false;
            result.FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml;
            result.FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml;

            result.RegisterExternalDataSource(x => x.ParentId, y => BindCategoriesParent());

            result.AddAction().HasText(this.T("Clear")).HasUrl(this.Url.Action("Edit", RouteData.Values.Merge(new { id = 0 }))).HasButtonStyle(ButtonStyle.Success);
            result.AddAction().HasText(this.T("Back")).HasUrl(this.Url.Action("Index")).HasButtonStyle(ButtonStyle.Danger);
            
            return result;
        }

        [Url("admin/category/get-categories-parent")]
        public ActionResult GetCategoriesParent()
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }

            var service = WorkContext.Resolve<ICategoriesService>();
            service.LanguageCode = languageCode;
            var items = service.GetTree();
            var result = new List<SelectListItem>();

            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.ChildenName,
                Value = item.Id.ToString()
            }));

            result.Insert(0, new SelectListItem { Text = T("--- Không chọn ---"), Value = "0" });

            return Json(result);
        }

        [HttpPost()]
        [FormButton("Save")]
        [ValidateInput(false)]
        [Url("admin/categories/update")]
        public ActionResult Update(CategoriesModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var service = WorkContext.Resolve<ICategoriesService>();
            service.LanguageCode = WorkContext.CurrentCulture;
            CategoryInfo item = model.Id == 0 ? new CategoryInfo() : service.GetById(model.Id);
            var alias = Utilities.GetAlias(model.ShortName);
            alias = GetAlias(item.Id, alias, alias, WorkContext.CurrentCulture);
            var refId = model.RefId;
            if (string.IsNullOrEmpty(refId))
            {
                refId = DateTime.Now.ToString(Extensions.Constants.DateTimeFomatFullNone);
            }
            var url = "/" + alias;
            if (model.IsHome)
            {
                url = "/";
            }
            item.LanguageCode = WorkContext.CurrentCulture;
            item.ParentId = model.ParentId;
            item.ShortName = model.ShortName;
            item.Name = model.Name;
            item.Alias = alias;
            item.IsHome = model.IsHome;
            item.HasChilden = model.HasChilden;
            item.CreateDate = DateTime.Now.Date;
            item.Notes = model.Notes;
            item.Description = model.Description;
            item.Tags = model.Tags;
            item.Url = url;
            item.IsActived = true;
            item.OrderBy = 0;
            item.MenuOrderBy = model.MenuOrderBy;
            item.IsDisplayMenu = true;
            item.IsDisplayFooter = false;
            item.IsDeleted = model.IsDeleted;
            item.RefId = refId;
            service.Save(item);

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE").Alert(T("Đã cập nhật thành công."));
        }

        private int iIndex = 1;
        private string GetAlias(int id, string alias, string aliasSource, string languageCode)
        {
            var service = WorkContext.Resolve<ICategoriesService>();
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

        //[ActionName("Update")]
        //[FormButton("Delete")]
        //public ActionResult Delete(int id)
        //{
        //    var service = WorkContext.Resolve<ICategoriesService>();
        //    var model = service.GetById(id);
        //    service.LanguageCode = model.LanguageCode;
        //    model.IsDeleted = true;
        //    service.Update(model);

        //    return new AjaxResult().NotifyMessage("DELETE_ENTITY_COMPLETE").Alert(T("Đã xóa tạm thời chuyên mục."));
        //}
    }
}
