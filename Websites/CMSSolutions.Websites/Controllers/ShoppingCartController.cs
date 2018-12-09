using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;
using Newtonsoft.Json;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = false)]
    public class ShoppingCartController : BaseHomeController
    {
        public ShoppingCartController(IWorkContextAccessor workContextAccessor) : base(workContextAccessor)
        {

        }

        [Url("gio-hang")]
        public ActionResult Index()
        {
            var categoryService = WorkContext.Resolve<ICategoriesService>();
            categoryService.LanguageCode = WorkContext.CurrentCulture;
            var category = categoryService.GetById((int)Category.ShoppingCart);
            if (category != null)
            {
                ViewData[Extensions.Constants.SeoTitle] = category.Name;
                ViewData[Extensions.Constants.SeoKeywords] = category.Tags;
                ViewData[Extensions.Constants.SeoDescription] = category.Description;
            }

            var viewRenderer = new ViewRenderer { Context = ControllerContext };

            #region Product
            var modelCart = new DataViewerModel();
            var listProducts = new List<CartInfo>();
            if (WorkContext.HttpContext.Session[Extensions.Constants.KeyDataCart] != null)
            {
                listProducts = (List<CartInfo>) WorkContext.HttpContext.Session[Extensions.Constants.KeyDataCart];
            }
            modelCart.ListCart = listProducts;
            var viewSectionNews = viewRenderer.RenderPartialView("~/Views/Shared/ShoppingCartDetails.cshtml", modelCart);
            WorkContext.Layout.SectionProducts.Add(new MvcHtmlString(viewSectionNews));
            #endregion

            return View("Empty");
        }

        [HttpPost, ValidateInput(false)]
        [Url("gio-hang/luu-gio-hang")]
        public ActionResult SaveCart()
        {
            var data = Request.Form["txtDataCart"];
            if (!string.IsNullOrEmpty(data))
            {
                var list = Utilities.ConvertJsonToObject<List<CartInfo>>(data);
                WorkContext.HttpContext.Session[Extensions.Constants.KeyDataCart] = list;
            }

            return Redirect(Url.Action("Index", "Payment"));
        }

        [HttpPost, ValidateInput(false)]
        [Url("gio-hang/them-vao-gio-hang")]
        public ActionResult AddToCart()
        {
            var model = new DataViewerModel();
            var status = UpdateShoppingCart();
            if (status == -1)
            {
                model.Status = false;
                model.Data = T("Thêm sản phẩm vào giỏ hàng thất bại.");
            }
            if (status == 2)
            {
                model.Status = false;
                model.Data = T("Bạn vui lòng nhập số lượng sản phẩm cần mua.");
            }
            if (status == 0)
            {
                model.Status = true;
                model.Data = T("Đã thêm sản phẩm vào giỏ hàng thành công.");
            }

            return Json(model);
        }
    }
}
