using CMSSolutions.Web.Routing;

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


    [Authorize()]
    [Themed(IsDashboard=true)]
    public class AdminProductVolumeController : BaseController
    {
        
        private readonly IProductVolumeService service;
        
        public AdminProductVolumeController(IWorkContextAccessor workContextAccessor, IProductVolumeService service) : 
                base(workContextAccessor)
        {
            this.service = service;
            this.TableName = "tblProductVolume";
        }
        
        [Url("admin/productvolumes")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý thể tích chai"), Url = "#" });
            var result = new ControlGridFormResult<ProductVolumeInfo>();
            var siteSettings = WorkContext.Resolve<SiteSettings>();
            result.Title = this.T("Quản lý thể tích chai");
            result.CssClass = "table table-bordered table-striped";
            result.UpdateActionName = "Update";
            result.IsAjaxSupported = true;
            result.DefaultPageSize = siteSettings.DefaultPageSize;
            result.EnablePaginate = true;
            result.FetchAjaxSource = this.GetProductVolume;
            result.GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml;
            result.GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml;
            result.ClientId = TableName;
            result.ActionsColumnWidth = 180;

            result.AddColumn(x => x.Id, T("Mã")).HasWidth(100);
            result.AddColumn(x => x.Name, T("Thể tích"));

            result.AddAction().HasText(this.T("Create")).HasUrl(this.Url.Action("Edit", new { id = 0 })).HasButtonStyle(ButtonStyle.Primary).HasBoxButton(false).HasCssClass(Constants.RowLeft).HasRow(true);
            result.AddRowAction().HasText(this.T("Edit")).HasUrl(x => Url.Action("Edit", new { id = x.Id })).HasButtonStyle(ButtonStyle.Default).HasButtonSize(ButtonSize.ExtraSmall);
            result.AddRowAction(true).HasText(this.T("Delete")).HasName("Delete").HasValue(x => Convert.ToString(x.Id)).HasButtonStyle(ButtonStyle.Danger).HasButtonSize(ButtonSize.ExtraSmall).HasConfirmMessage(this.T("Không nên xóa dữ liệu thể tích chai vì có thể lỗi hệ thống, bạn có chắc chắn muốn xóa nó?"));
            
            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");
            return result;
        }
        
        private ControlGridAjaxData<ProductVolumeInfo> GetProductVolume(ControlGridFormRequest options)
        {
            int totals;
            var items = this.service.GetRecords(options, out totals);
            var result = new ControlGridAjaxData<ProductVolumeInfo>(items, totals);
            return result;
        }
        
        [Url("admin/productvolumes/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý thể tích chai"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thể tích chai"), Url = "#" });
            var model = new ProductVolumeModel();
            if (id > 0)
            {
                model = this.service.GetById(id);
            }

            var result = new ControlFormResult<ProductVolumeModel>(model);
            result.Title = this.T("Thể tích chai");
            result.FormMethod = FormMethod.Post;
            result.UpdateActionName = "Update";
            result.ShowCancelButton = false;
            result.ShowBoxHeader = false;
            result.FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml;
            result.FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml;

            result.AddAction().HasText(this.T("Clear")).HasUrl(this.Url.Action("Edit", RouteData.Values.Merge(new { id = 0 }))).HasButtonStyle(ButtonStyle.Success);
            result.AddAction().HasText(this.T("Back")).HasUrl(this.Url.Action("Index")).HasButtonStyle(ButtonStyle.Danger);
            return result;
        }
        
        [HttpPost()]
        [FormButton("Save")]
        [ValidateInput(false)]
        [Url("admin/productvolumes/update")]
        public ActionResult Update(ProductVolumeModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }
            ProductVolumeInfo item;
            if (model.Id == 0)
            {
                item = new ProductVolumeInfo();
            }
            else
            {
                item = service.GetById(model.Id);
            }
            item.Name = model.Name;
            service.Save(item);

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE").CloseModalDialog();
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
