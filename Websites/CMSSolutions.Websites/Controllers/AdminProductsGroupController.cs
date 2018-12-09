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


    [Authorize()]
    [Themed(IsDashboard=true)]
    public class AdminProductsGroupController : BaseController
    {
        
        private readonly IProductsGroupService service;
        
        public AdminProductsGroupController(IWorkContextAccessor workContextAccessor, IProductsGroupService service) : 
                base(workContextAccessor)
        {
            this.service = service;
            this.TableName = "tblProductsGroup";
        }
        
        [Url("admin/productsgroups")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý dòng sản phẩm"), Url = "#" });
            var result = new ControlGridFormResult<ProductsGroupInfo>();
            var siteSettings = WorkContext.Resolve<SiteSettings>();
            result.Title = this.T("Quản lý dòng sản phẩm");
            result.CssClass = "table table-bordered table-striped";
            result.UpdateActionName = "Update";
            result.IsAjaxSupported = true;
            result.DefaultPageSize = siteSettings.DefaultPageSize;
            result.EnablePaginate = true;
            result.FetchAjaxSource = this.GetProductsGroup;
            result.GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml;
            result.GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml;
            result.ActionsColumnWidth = 180;
            result.ClientId = TableName;

            result.AddColumn(x => x.Id, T("ID")).AlignCenter().HasWidth(100);
            result.AddColumn(x => x.ImageUrl, T("Ảnh đại diện"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsImage(y => y.ImageUrl, Extensions.Constants.CssThumbsSize);
            result.AddColumn(x => x.GroupName, T("Tên dòng sản phẩm"));

            result.AddAction().HasText(this.T("Create")).HasUrl(this.Url.Action("Edit", new { id = 0 })).HasButtonStyle(ButtonStyle.Primary).HasBoxButton(false).HasCssClass(Constants.RowLeft).HasRow(true);
            result.AddRowAction().HasText(this.T("Edit")).HasUrl(x => Url.Action("Edit", new { id = x.Id })).HasButtonStyle(ButtonStyle.Default).HasButtonSize(ButtonSize.ExtraSmall);
            result.AddRowAction(true).HasText(this.T("Delete")).HasName("Delete").HasValue(x => Convert.ToString(x.Id)).HasButtonStyle(ButtonStyle.Danger).HasButtonSize(ButtonSize.ExtraSmall).HasConfirmMessage(this.T("Không nên xóa dữ liệu dòng sản phẩm vì có thể lỗi hệ thống, bạn có chắc chắn muốn xóa nó?"));
            
            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");
            return result;
        }
        
        private ControlGridAjaxData<ProductsGroupInfo> GetProductsGroup(ControlGridFormRequest options)
        {
            int totals;
            var items = this.service.GetRecords(options, out totals);
            var result = new ControlGridAjaxData<ProductsGroupInfo>(items, totals);
            return result;
        }

        [Url("admin/productsgroups/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin dòng sản phẩm"), Url = Url.Action("Index") });
            var model = new ProductsGroupModel();
            if (id > 0)
            {
                model = this.service.GetById(id);
            }

            var result = new ControlFormResult<ProductsGroupModel>(model);
            result.Title = this.T("Thông tin dòng sản phẩm");
            result.FormMethod = FormMethod.Post;
            result.UpdateActionName = "Update";
            result.ShowBoxHeader = false;
            result.CancelButtonText = T("Back");
            result.FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml;
            result.FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml;
            result.RegisterFileUploadOptions("ImageUrl.FileName", new ControlFileUploadOptions
            {
                AllowedExtensions = "jpg,jpeg,png,gif"
            });
            result.RegisterFileUploadOptions("HomeImageUrl.FileName", new ControlFileUploadOptions
            {
                AllowedExtensions = "jpg,jpeg,png,gif"
            });
            return result;
        }
        
        [HttpPost()]
        [FormButton("Save")]
        [ValidateInput(false)]
        [Url("admin/productsgroups/update")]
        public ActionResult Update(ProductsGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            ProductsGroupInfo item = model.Id == 0 ? new ProductsGroupInfo() : service.GetById(model.Id);
            var alias = item.Alias;
            if (string.IsNullOrEmpty(alias))
            {
                alias = Utilities.GetAlias(model.GroupName);
                alias = GetAlias(item.Id, alias, alias);
            }

            item.GroupName = model.GroupName;
            item.Alias = alias;
            item.ImageUrl = model.ImageUrl;
            item.HomeImageUrl = model.HomeImageUrl;
            item.BackgroundImageUrl = model.BackgroundImageUrl;
            item.Notes = model.Notes;
            service.Save(item);

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE").Alert(T("Cập nhật thành công."));
        }

        private int iIndex = 1;
        private string GetAlias(int id, string alias, string aliasSource)
        {
            var service = WorkContext.Resolve<IProductsGroupService>();
            if (service.CheckAlias(id, alias))
            {
                alias = aliasSource + "-" + iIndex;
                if (service.CheckAlias(id, alias))
                {
                    iIndex++;
                    alias = GetAlias(id, alias, aliasSource);
                }

                return alias;
            }

            return alias;
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
