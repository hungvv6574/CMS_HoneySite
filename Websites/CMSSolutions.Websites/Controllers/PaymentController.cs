using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = false)]
    public class PaymentController : BaseHomeController
    {
        public PaymentController(IWorkContextAccessor workContextAccessor) : base(workContextAccessor)
        {

        }

        [Url("thanh-toan")]
        public ActionResult Index()
        {
            var categoryService = WorkContext.Resolve<ICategoriesService>();
            categoryService.LanguageCode = WorkContext.CurrentCulture;
            var category = categoryService.GetById((int)Category.Payment);
            if (category != null)
            {
                ViewData[Extensions.Constants.SeoTitle] = category.Name;
                ViewData[Extensions.Constants.SeoKeywords] = category.Tags;
                ViewData[Extensions.Constants.SeoDescription] = category.Description;
            }

            AddModules();

            return View("Empty");
        }

        private void AddModules()
        {
            var viewRenderer = new ViewRenderer { Context = ControllerContext };

            #region Product
            var modelCart = new DataViewerModel();
            var listProducts = new List<CartInfo>();
            if (WorkContext.HttpContext.Session[Extensions.Constants.KeyDataCart] != null)
            {
                listProducts = (List<CartInfo>)WorkContext.HttpContext.Session[Extensions.Constants.KeyDataCart];
            }
            modelCart.ListCart = listProducts;
            var viewSectionNews = viewRenderer.RenderPartialView("~/Views/Shared/ConfirmCart.cshtml", modelCart);
            WorkContext.Layout.SectionProducts.Add(new MvcHtmlString(viewSectionNews));
            #endregion
        }

        [HttpPost, ValidateInput(false)]
        [Url("thanh-toan/xac-nhan")]
        public ActionResult PaymentConfirm()
        {
            var model = new DataViewerModel();
            if (WorkContext.HttpContext.Session[Extensions.Constants.KeyDataCart] != null)
            {
                var txtCartFullName = Request.Form["txtCartFullName"];
                var txtCartEmail = Request.Form["txtCartEmail"];
                var txtCartSoDienThoai = Request.Form["txtCartSoDienThoai"];
                var ddlPaymentType = Request.Form["ddlPaymentType"];
                var txtCartDeliveryTime = Request.Form["txtCartDeliveryTime"];
                var txtCartDeliveryAddress = Request.Form["txtCartDeliveryAddress"];
                var orderId = 0;
                if (WorkContext.HttpContext.Session[Extensions.Constants.KeyOrderId] != null)
                {
                    orderId = Convert.ToInt32(WorkContext.HttpContext.Session[Extensions.Constants.KeyOrderId]);
                }
                var order = new OrderInfo
                {
                    Id = orderId,
                    OrderCode = "QH" + DateTime.Now.ToString(Extensions.Constants.DateTimeFomatFullNone),
                    FullName = txtCartFullName,
                    PhoneNumber = txtCartSoDienThoai,
                    Email = txtCartEmail,
                    DeliveryTime = txtCartDeliveryTime,
                    DeliveryPlace = txtCartDeliveryAddress,
                    PaymentType = Convert.ToInt32(ddlPaymentType),
                    PaymentCode = string.Empty,
                    CreateDate = DateTime.Now,
                    Notes = string.Empty,
                    IsPaid = false,
                    IsDeliverySuccessful = false
                };

                var listProducts = (List<CartInfo>)WorkContext.HttpContext.Session[Extensions.Constants.KeyDataCart];
                
                var orderService = WorkContext.Resolve<IOrdersService>();
                var orderDetailsService = WorkContext.Resolve<IOrderDetailsService>();
                orderService.Save(order);
                orderId = order.Id;
                WorkContext.HttpContext.Session[Extensions.Constants.KeyOrderId] = orderId;
                var total = 0;
                foreach (var item in listProducts)
                {
                    var info = new OrderDetailsInfo
                    {
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Capacity = item.Capacity,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Total = item.Price*item.Quantity
                    };
                    total += info.Total;
                    orderDetailsService.Save(info);
                }

                SendOrder(orderId);
                var type = (PaymentType) order.PaymentType;
                if (type == PaymentType.PaymentWhenReceive)
                {
                    model.Status = true;
                    model.PaymentType = order.PaymentType;
                    model.Data = T("Đã gửi đơn hàng của bạn tới chúng tôi thành công vui lòng kiểm tra email của bạn.");
                    model.Url = Url.Action("Index", "Product");
                }

                if (type == PaymentType.PaymentOnline)
                {
                    model.Status = true;
                    model.PaymentType = order.PaymentType;
                    model.Data = T("Đã gửi đơn hàng của bạn tới chúng tôi thành công vui lòng kiểm tra email của bạn. Để thanh toán online hãy nhấn vào nút dưới đây.");
                    model.Url = string.Format("https://www.nganluong.vn/button_payment.php?receiver={0}&product_name={1}&price={2}&return_url={3}&comments={4}", 
                        "hao.sanuco@gmail.com",
                        order.OrderCode,
                        total,
                        Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("PaymentCompleted", "Payment", new { @ordercode = order.OrderCode }),
                        order.DeliveryTime + " " + order.DeliveryPlace);
                }
            }
            else
            {
                model.Status = false;
                model.Data = T("Giỏ hàng của bạn không có sản phẩm.");
                model.Url = Url.Action("Index", "Product");
            }

            return Json(model);
        }

        [HttpGet]
        [Url("hoan-thanh-thanh-toan/ordercode={ordercode}")]
        public ActionResult PaymentCompleted(string ordercode)
        {
            var model = new DataViewerModel();
            model.Status = false;
            model.Data = "Thanh toán thất bại.";
            if (WorkContext.HttpContext.Session[Extensions.Constants.KeyOrderId] != null)
            {
                var currentOrderId = WorkContext.HttpContext.Session[Extensions.Constants.KeyOrderId].ToString();
                if (ordercode == currentOrderId)
                {
                    var orderService = WorkContext.Resolve<IOrdersService>();
                    var entity = orderService.GetByOrderCode(ordercode);
                    if (entity != null)
                    {
                        entity.IsPaid = true;
                        entity.Notes = "Đã thanh toán online thành công qua ngân lượng.";
                        orderService.Save(entity);
                        model.Status = true;
                        model.Data = "Đã thanh toán online thành công qua ngân lượng. Vui lòng chờ nhận hàng từ chúng tôi.";
                    }
                }
            }

            return View("PaymentCompleted", model);
        }

        [Url("thanh-toan/mua-ngay")]
        public ActionResult BuyNow()
        {
            var categoryService = WorkContext.Resolve<ICategoriesService>();
            categoryService.LanguageCode = WorkContext.CurrentCulture;
            var category = categoryService.GetById((int)Category.Payment);
            if (category != null)
            {
                ViewData[Extensions.Constants.SeoTitle] = category.Name;
                ViewData[Extensions.Constants.SeoKeywords] = category.Tags;
                ViewData[Extensions.Constants.SeoDescription] = category.Description;
            }

            UpdateShoppingCart();

            AddModules();

            return View("Empty");
        }
    }
}
