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
    using CMSSolutions.Web.Routing;
    
    
    [Authorize()]
    [Themed(IsDashboard=true)]
    public class AdminBannersController : BaseAdminController
    {
        private readonly IBannersService service;
        public AdminBannersController(IWorkContextAccessor workContextAccessor, IBannersService service) : 
                base(workContextAccessor)
        {
            this.service = service;
            this.TableName = "tblBanners";
        }
        
        [Url("admin/banners")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý banner"), Url = "#" });
            var result = new ControlGridFormResult<BannerInfo>();
            var siteSettings = WorkContext.Resolve<SiteSettings>();

            result.Title = this.T("Quản lý banner");
            result.CssClass = "table table-bordered table-striped";
            result.UpdateActionName = "Update";
            result.IsAjaxSupported = true;
            result.DefaultPageSize = siteSettings.DefaultPageSize;
            result.EnablePaginate = true;
            result.FetchAjaxSource = this.GetBanners;
            result.GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml;
            result.GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml;
            result.ClientId = TableName;

            result.AddCustomVar(Extensions.Constants.CategoryId, "$('#" + Extensions.Constants.CategoryId + "').val();", true);

            result.AddColumn(x => x.SortOrder, T("Sắp xếp")).HasWidth(60);
            result.AddColumn(x => x.ImageUrl, T("Ảnh Banner"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsImage(y => y.ImageUrl, Extensions.Constants.CssThumbsSize);
            result.AddColumn(x => x.Title, T("Tiêu đề"));
            result.AddColumn(x => x.Url, T("Liên kết"));

            result.AddAction().HasText(this.T("Create")).HasUrl(this.Url.Action("Edit", new { id = 0 })).HasButtonStyle(ButtonStyle.Primary).HasBoxButton(false).HasCssClass(Constants.RowLeft).HasRow(true);
            result.AddRowAction().HasText(this.T("Edit")).HasUrl(x => Url.Action("Edit", new { id = x.Id })).HasButtonStyle(ButtonStyle.Default).HasButtonSize(ButtonSize.ExtraSmall);
            result.AddRowAction(true).HasText(this.T("Delete")).HasName("Delete").HasValue(x => Convert.ToString(x.Id)).HasButtonStyle(ButtonStyle.Danger).HasButtonSize(ButtonSize.ExtraSmall).HasConfirmMessage(this.T(Constants.Messages.ConfirmDeleteRecord));

            result.AddAction(new ControlFormHtmlAction(()=>BuildCategories())).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");
            return result;
        }
        
        private ControlGridAjaxData<BannerInfo> GetBanners(ControlGridFormRequest options)
        {
            int totals;
            var CategoryId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.CategoryId]))
            {
                CategoryId = Convert.ToInt32(Request.Form[Extensions.Constants.CategoryId]);
            }
            var items = this.service.GetRecords(options, out totals, x => x.CategoryId == CategoryId);
            var result = new ControlGridAjaxData<BannerInfo>(items, totals);
            return result;
        }
        
        [Url("admin/banners/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý banner"), Url = "#" });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin banner"), Url = Url.Action("Index") });
            var model = new BannersModel();
            if (id > 0)
            {
                model = this.service.GetById(id);
            }

            var result = new ControlFormResult<BannersModel>(model);
            result.Title = this.T("Thông tin banner");
            result.FormMethod = FormMethod.Post;
            result.UpdateActionName = "Update";
            result.ShowCancelButton = false;
            result.ShowBoxHeader = false;
            result.FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml;
            result.FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml;

            result.RegisterFileUploadOptions("ImageUrl.FileName", new ControlFileUploadOptions
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
        [Url("admin/banners/update")]
        public ActionResult Update(BannersModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            BannerInfo item = model.Id == 0 ? new BannerInfo() : service.GetById(model.Id);
            item.LanguageCode = "vi-VN";
            item.CategoryId = model.CategoryId;
            item.Type = 0;
            item.Title = model.Title;
            item.Caption = model.Caption;
            item.Url = model.Url;
            item.ImageUrl = model.ImageUrl;
            item.SortOrder = model.SortOrder;
            service.Save(item);

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE").Alert(T("Đã cập nhật thành công."));
        }
        
        [ActionName("Update")]
        [FormButton("Delete")]
        public ActionResult Delete(int id)
        {
            var model = service.GetById(id);
            service.Delete(model);

            return new AjaxResult().NotifyMessage("DELETE_ENTITY_COMPLETE").Alert(T("Đã xóa thành công."));
        }
    }
}
