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

    [Authorize()]
    [Themed(IsDashboard=true)]
    public class AdminProductsController : BaseAdminController
    {
        
        private readonly IProductsService service;
        public AdminProductsController(IWorkContextAccessor workContextAccessor, IProductsService service) : 
                base(workContextAccessor)
        {
            this.service = service;
            this.TableName = "tblProducts";
        }
        
        [Url("admin/products")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý sản phẩm"), Url = "#" });

            var result = new ControlGridFormResult<ProductInfo>();
            var siteSettings = WorkContext.Resolve<SiteSettings>();
            result.Title = this.T("Quản lý sản phẩm");
            result.CssClass = "table table-bordered table-striped";
            result.UpdateActionName = "Update";
            result.IsAjaxSupported = true;
            result.DefaultPageSize = siteSettings.DefaultPageSize;
            result.EnablePaginate = true;
            result.FetchAjaxSource = this.GetProducts;
            result.GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml;
            result.GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml;
            result.ClientId = TableName;
            result.ActionsColumnWidth = 180;

            result.AddCustomVar(Extensions.Constants.SearchText, "$('#" + Extensions.Constants.SearchText + "').val();", true);
            result.AddCustomVar(Extensions.Constants.ProductGroup, "$('#" + Extensions.Constants.ProductGroup + "').val();", true);

            result.AddColumn(x => x.Id, T("ID")).AlignCenter().HasWidth(100);
            result.AddColumn(x => x.Image, T("Ảnh đại diện"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsImage(y => y.Image, Extensions.Constants.CssThumbsSize);
            result.AddColumn(x => x.Name, T("Tên sản phẩm"));
            result.AddColumn(x => GetName(x), T("Loại"));
            result.AddColumn(x => x.Price, T("Giá"));
            result.AddColumn(x => x.IsPublished)
                 .HasHeaderText(T("Đã đăng"))
                 .AlignCenter()
                 .HasWidth(100)
                 .RenderAsStatusImage();

            result.AddAction().HasText(this.T("Create")).HasUrl(this.Url.Action("Edit", new { id = 0 })).HasButtonStyle(ButtonStyle.Primary).HasBoxButton(false).HasCssClass(Constants.RowLeft).HasRow(true);
            result.AddRowAction().HasText(this.T("Edit")).HasUrl(x => Url.Action("Edit", new { id = x.Id })).HasButtonStyle(ButtonStyle.Default).HasButtonSize(ButtonSize.ExtraSmall);
            result.AddRowAction(true).HasText(this.T("Delete")).HasName("Delete").HasValue(x => Convert.ToString(x.Id)).HasButtonStyle(ButtonStyle.Danger).HasButtonSize(ButtonSize.ExtraSmall).HasConfirmMessage(this.T(Constants.Messages.ConfirmDeleteRecord));

            result.AddAction(new ControlFormHtmlAction(BuildProductGroup)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildSearchText)).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");
            return result;
        }

        private string GetName(ProductInfo obj)
        {
            var serviceType = WorkContext.Resolve<IProductVolumeService>();
            var item = serviceType.GetById(obj.Type);
            return item.Name;
        }
        
        private ControlGridAjaxData<ProductInfo> GetProducts(ControlGridFormRequest options)
        {
            var searchText = string.Empty;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SearchText]))
            {
                searchText = Request.Form[Extensions.Constants.SearchText];
            }
            var productGroup = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.ProductGroup]))
            {
                productGroup = Convert.ToInt32(Request.Form[Extensions.Constants.ProductGroup]);
            }

            var languageCode = WorkContext.CurrentCulture;
            var categoryId = 0;
            int totals;
            service.LanguageCode = languageCode;
            service.CategoryId = categoryId;
            var records = service.SearchPaged(searchText, productGroup, options.PageIndex, options.PageSize, out totals);

            return new ControlGridAjaxData<ProductInfo>(records, totals);
        }
        
        [Url("admin/products/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý sản phẩm"), Url = "#" });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin sản phẩm"), Url = Url.Action("Index") });
            var model = new ProductModel();
            if (id > 0)
            {
                model = this.service.GetById(id);
            }

            var result = new ControlFormResult<ProductModel>(model);
            result.Title = this.T("Thông tin sản phẩm");
            result.FormMethod = FormMethod.Post;
            result.UpdateActionName = "Update";
            result.ShowCancelButton = false;
            result.ShowBoxHeader = false;
            result.FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml;
            result.FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml;

            result.RegisterFileUploadOptions("Image.FileName", new ControlFileUploadOptions
            {
                AllowedExtensions = "jpg,jpeg,png,gif"
            });
            result.MakeReadOnlyProperty(x => x.CategoryId);

            result.AddAction().HasText(this.T("Clear")).HasUrl(this.Url.Action("Edit", RouteData.Values.Merge(new { id = 0 }))).HasButtonStyle(ButtonStyle.Success);
            result.AddAction().HasText(this.T("Back")).HasUrl(this.Url.Action("Index")).HasButtonStyle(ButtonStyle.Danger);

            result.RegisterExternalDataSource(x => x.CategoryId, y => BindCategories());
            result.RegisterExternalDataSource(x => x.GroupId, y => BindProductGroup());
            result.RegisterExternalDataSource(x => x.Type, y => BindProductTypes());

            return result;
        }
        
        [HttpPost()]
        [FormButton("Save")]
        [ValidateInput(false)]
        [Url("admin/products/update")]
        public ActionResult Update(ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }
            if (model.Price <= 0)
            {
                return new AjaxResult().Alert(T("Giá sản phẩm phải nhập lớn hơn 0 đồng."));
            }
            service.CategoryId = model.CategoryId;
            service.LanguageCode = WorkContext.CurrentCulture;
            ProductInfo item = model.Id == 0 ? new ProductInfo() : service.GetById(model.Id);

            var alias = Utilities.GetAlias(model.Name);
            alias = GetAlias(item.Id, alias, alias, WorkContext.CurrentCulture);
            var refId = model.RefId;
            if (string.IsNullOrEmpty(refId))
            {
                refId = DateTime.Now.ToString(Extensions.Constants.DateTimeFomatFullNone);
            }

            item.LanguageCode = WorkContext.CurrentCulture;
            item.CategoryId = model.CategoryId;
            item.RefId = refId;
            item.Name = model.Name;
            item.Alias = alias;
            item.GroupId = model.GroupId;
            item.Type = model.Type;
            item.Price = model.Price;
            item.Summary = model.Summary;
            item.Contents = model.Contents;
            item.IsPublished = model.IsPublished;
            item.Image = model.Image;
            item.Description = model.Description;
            item.Tags = model.Tags;
            item.IsDeleted = false;
            service.Save(item);

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE").Alert("Cập nhật thành công.");
        }

        private int iIndex = 1;
        private string GetAlias(int id, string alias, string aliasSource, string languageCode)
        {
            var service = WorkContext.Resolve<IProductsService>();
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
            var model = service.GetById(id);
            service.Delete(model);

            return new AjaxResult().NotifyMessage("DELETE_ENTITY_COMPLETE").Alert(T("Đã xóa thành công."));
        }
    }
}
