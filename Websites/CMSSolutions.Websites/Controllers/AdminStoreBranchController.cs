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
    public class AdminStoreBranchController : BaseController
    {
        private readonly IStoreBranchService service;
        
        public AdminStoreBranchController(IWorkContextAccessor workContextAccessor, IStoreBranchService service) : 
                base(workContextAccessor)
        {
            this.service = service;
            this.TableName = "tblStoreBranch";
        }
        
        [Url("admin/storebranches")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý cửa hàng phân phối"), Url = "#" });
            var result = new ControlGridFormResult<StoreBranchInfo>();
            var siteSettings = WorkContext.Resolve<SiteSettings>();

            result.Title = this.T("Quản lý cửa hàng phân phối");
            result.CssClass = "table table-bordered table-striped";
            result.UpdateActionName = "Update";
            result.IsAjaxSupported = true;
            result.DefaultPageSize = siteSettings.DefaultPageSize;
            result.EnablePaginate = true;
            result.FetchAjaxSource = this.GetStoreBranch;
            result.GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml;
            result.GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml;
            result.ClientId = TableName;
            result.ActionsColumnWidth = 180;

            result.AddColumn(x => x.Id, T("ID")).AlignCenter().HasWidth(100);
            result.AddColumn(x => x.Name, T("Tên cửa hàng"));
            result.AddColumn(x => x.Address, T("Địa chỉ"));

            result.AddAction().HasText(this.T("Create")).HasUrl(this.Url.Action("Edit", new { id = 0 })).HasButtonStyle(ButtonStyle.Primary).HasBoxButton(false).HasCssClass(Constants.RowLeft).HasRow(true);
            result.AddRowAction().HasText(this.T("Edit")).HasUrl(x => Url.Action("Edit", new { id = x.Id })).HasButtonStyle(ButtonStyle.Default).HasButtonSize(ButtonSize.ExtraSmall);
            result.AddRowAction(true).HasText(this.T("Delete")).HasName("Delete").HasValue(x => Convert.ToString(x.Id)).HasButtonStyle(ButtonStyle.Danger).HasButtonSize(ButtonSize.ExtraSmall).HasConfirmMessage(this.T(Constants.Messages.ConfirmDeleteRecord));
            
            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");
            return result;
        }
        
        private ControlGridAjaxData<StoreBranchInfo> GetStoreBranch(ControlGridFormRequest options)
        {
            int totals;
            var items = this.service.GetRecords(options, out totals);
            var result = new ControlGridAjaxData<StoreBranchInfo>(items, totals);
            return result;
        }
        
        [Url("admin/storebranches/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý cửa hàng phân phối"), Url = "#" });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin cửa hàng phân phối"), Url = Url.Action("Index") });
            var model = new StoreBranchModel();
            if (id > 0)
            {
                model = this.service.GetById(id);
            }

            var result = new ControlFormResult<StoreBranchModel>(model);
            result.Title = this.T("Thông tin cửa hàng phân phối");
            result.FormMethod = FormMethod.Post;
            result.UpdateActionName = "Update";
            result.ShowCancelButton = false;
            result.ShowBoxHeader = false;
            result.FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml;
            result.FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml;

            result.RegisterFileUploadOptions("LogoUrl.FileName", new ControlFileUploadOptions
            {
                AllowedExtensions = "jpg,jpeg,png,gif"
            });

            result.AddAction().HasText(this.T("Clear")).HasUrl(this.Url.Action("Edit", RouteData.Values.Merge(new { id = 0 }))).HasButtonStyle(ButtonStyle.Success);
            result.AddAction().HasText(this.T("Back")).HasUrl(this.Url.Action("Index")).HasButtonStyle(ButtonStyle.Danger);
            return result;
        }
        
        [HttpPost()]
        [FormButton("Save")]
        [ValidateInput(false)]
        [Url("admin/storebranches/update")]
        public ActionResult Update(StoreBranchModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            StoreBranchInfo item = model.Id == 0 ? new StoreBranchInfo() : service.GetById(model.Id);
            item.Name = model.Name;
            item.Address = model.Address;
            item.PhoneNumber = model.PhoneNumber;
            item.Notes = model.Notes;
            item.LogoUrl = model.LogoUrl;
            service.Save(item);

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE").Alert(T("Cập nhật thành công."));
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
