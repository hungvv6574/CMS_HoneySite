using System;
using System.Linq;
using System.Web.Mvc;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = false)]
    public class ProductController : BaseHomeController
    {
        public ProductController(IWorkContextAccessor workContextAccessor) : base(workContextAccessor)
        {

        }

        [Url("san-pham")]
        public ActionResult Index()
        {
            var categoryService = WorkContext.Resolve<ICategoriesService>();
            categoryService.LanguageCode = WorkContext.CurrentCulture;
            var category = categoryService.GetById((int)Category.Product);
            if (category != null)
            {
                ViewData[Extensions.Constants.SeoTitle] = category.Name;
                ViewData[Extensions.Constants.SeoKeywords] = category.Tags;
                ViewData[Extensions.Constants.SeoDescription] = category.Description;
            }

            var setting = WorkContext.Resolve<CommonSettings>();
            var viewRenderer = new ViewRenderer { Context = ControllerContext };

            #region SectionBannerSlider
            var modelSectionBannerSlider = new DataViewerModel();
            var service = WorkContext.Resolve<IBannersService>();
            modelSectionBannerSlider.Banner = service.GetRecords(x => x.LanguageCode == WorkContext.CurrentCulture && x.CategoryId == category.Id).FirstOrDefault();
            var viewSectionBannerSlider = viewRenderer.RenderPartialView("~/Views/Shared/ProductBanner.cshtml", modelSectionBannerSlider);
            WorkContext.Layout.SectionBanner.Add(new MvcHtmlString(viewSectionBannerSlider));
            #endregion

            #region Products
            var modelProducts = new DataViewerModel();
            var seviceProduct = WorkContext.Resolve<IProductsGroupService>();
            modelProducts.ListProductGroup = seviceProduct.GetRecords().OrderBy(x => x.GroupName).ToList();
            modelProducts.Settings = setting;
            var viewSectionProducts = viewRenderer.RenderPartialView("~/Views/Shared/PageListProducts.cshtml", modelProducts);
            WorkContext.Layout.SectionProducts.Add(new MvcHtmlString(viewSectionProducts));
            #endregion

            return View("Empty");
        }

        [Url("san-pham/{alias}")]
        public ActionResult ProductDetails(string alias)
        {
            var seviceProductGroup = WorkContext.Resolve<IProductsGroupService>();
            var group = seviceProductGroup.GetRecords(x => x.Alias == alias).FirstOrDefault();
            if (group != null)
            {
                ViewData[Extensions.Constants.SeoTitle] = group.GroupName;
                ViewData[Extensions.Constants.SeoKeywords] = group.GroupName;
                ViewData[Extensions.Constants.SeoDescription] = group.GroupName;
            }

            var setting = WorkContext.Resolve<CommonSettings>();
            var viewRenderer = new ViewRenderer { Context = ControllerContext };

            #region Products
            var seviceProduct = WorkContext.Resolve<IProductsService>();
            var seviceProductType = WorkContext.Resolve<IProductVolumeService>();
            var list = seviceProduct.GetRecords(x => x.GroupId == group.Id).OrderBy(x => x.Type).ToList();
            if (list.Count > 0 && group != null)
            {
                foreach (var productInfo in list)
                {
                    var type = seviceProductType.GetById(productInfo.Type);
                    productInfo.GroupName = group.GroupName;
                    productInfo.CapacityName = type.Name;
                }
            }
            var modelProducts = new DataViewerModel
            {
                ProductGroup = group,
                Settings = setting,
                Products = list
            };
            var viewSectionProducts = viewRenderer.RenderPartialView("~/Views/Shared/ProductDetails.cshtml", modelProducts);
            WorkContext.Layout.SectionProducts.Add(new MvcHtmlString(viewSectionProducts));
            #endregion

            return View("Empty");
        }

        [HttpPost, ValidateInput(false)]
        [Url("product/get-product-details")]
        public ActionResult GetProductDetails()
        {
            var productId = Request.Form["ProductId"];
            var model = new DataViewerModel();
            var seviceProduct = WorkContext.Resolve<IProductsService>();
            model.Product = seviceProduct.GetById(Convert.ToInt32(productId));
            return Json(model);
        }
    }
}
