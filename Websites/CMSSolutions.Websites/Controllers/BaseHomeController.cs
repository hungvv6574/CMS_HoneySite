using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using CMSSolutions.Net.Mail;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    public class BaseHomeController : BaseController
    {
        #region Paged
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        #endregion

        public BaseHomeController(IWorkContextAccessor workContextAccessor)
            : base(workContextAccessor)
        {

        }

        #region Send Emails
        public void SendEmail(string subject, string body, string toEmailReceive, string ccEmail)
        {
            var service = WorkContext.Resolve<IEmailSender>();
            var mailMessage = new MailMessage
            {
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                Body = body,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            mailMessage.Sender = new MailAddress(toEmailReceive);
            mailMessage.To.Add(toEmailReceive);
            if (!string.IsNullOrEmpty(ccEmail))
            {
                mailMessage.CC.Add(ccEmail);
            }
            mailMessage.Bcc.Add("qhoney.contact@gmail.com");

            service.Send(mailMessage);
        }

        public void SendOrder(int orderId)
        {
            var serviceOrder = WorkContext.Resolve<IOrdersService>();
            var serviceOrderDetails = WorkContext.Resolve<IOrderDetailsService>();
            var order = serviceOrder.GetById(orderId);
            var list = serviceOrderDetails.GetRecords(x => x.OrderId == orderId).ToList();
            if (order != null && list.Count > 0)
            {
                var result = new DataViewerModel();
                var htmlBuilder = new StringBuilder();
                var total = 0;
                foreach (var orderDetailsInfo in list)
                {
                    htmlBuilder.Append("<tr>");
                    htmlBuilder.AppendFormat("<td>{0}</td>", orderDetailsInfo.ProductName);
                    htmlBuilder.AppendFormat("<td>{0}</td>", orderDetailsInfo.Capacity);
                    htmlBuilder.AppendFormat("<td>{0}</td>", orderDetailsInfo.Quantity);
                    htmlBuilder.AppendFormat("<td>{0}</td>", Utilities.GetCurrency(orderDetailsInfo.Price.ToString()));
                    htmlBuilder.AppendFormat("<td>{0}</td>", Utilities.GetCurrency(orderDetailsInfo.Total.ToString()));
                    htmlBuilder.Append("</tr>");
                    total += orderDetailsInfo.Total;
                }
                string html = System.IO.File.ReadAllText(Server.MapPath("~/Media/Default/EmailTemplates/ShoppingCartInfo.html"));
                html = html.Replace("[FULLNAME]", order.FullName);
                html = html.Replace("[PHONENUMBER]", order.PhoneNumber);
                html = html.Replace("[EMAILADDRESS]", order.Email);
                html = html.Replace("[PAYMENTTYPE]", CMSSolutions.Extensions.EnumExtensions.GetDisplayName((PaymentType)order.PaymentType));
                html = html.Replace("[DELIVERYTIME]", order.DeliveryTime);
                html = html.Replace("[DELIVERYADDRESS]", order.DeliveryPlace);
                html = html.Replace("[ORDERCODE]", order.OrderCode);
                html = html.Replace("[PRODUCTBODY]", htmlBuilder.ToString());
                html = html.Replace("[TOTALMONEY]", Utilities.GetCurrency(total.ToString()));
                try
                {
                    SendEmail(T("Thông tin đơn hàng mật ong QHoney"), html, order.Email, string.Empty);
                    result.Status = true;
                    result.Data = T("Gửi thành công.");
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Data = ex.Message;
                }
            }
        }
        #endregion

        #region Receive Email
        [HttpPost, ValidateInput(false)]
        [Url("trang-chu/nhan-email")]
        public ActionResult ReceiveEmail()
        {
            var emailAddress = Request.Form["txtEmailAddress"];
            var model = new DataViewerModel();
            if (string.IsNullOrEmpty(emailAddress))
            {
                model.Status = false;
                model.Data = T("Địa chỉ email không để trống.");
                return Json(model);
            }

            var service = WorkContext.Resolve<IEmailsService>();
            var status = service.CheckEmailExist(emailAddress);
            if (!status)
            {
                var email = new EmailInfo
                {
                    Email = emailAddress,
                    FullName = string.Empty,
                    IsBlocked = false,
                    Notes = "Yêu cầu nhận gửi thông tin qua email."
                };
                service.Save(email);
            }

            model.Status = true;
            model.Data = T("Đã lưu lại địa chỉ email của bạn thành công.");

            return Json(model);
        }
        #endregion

        #region Cart
        internal int UpdateShoppingCart()
        {
            var result = -1;
            var listId = Request.Form["txtProductIds"];
            if (!string.IsNullOrEmpty(listId))
            {
                var serviceCap = WorkContext.Resolve<IProductVolumeService>();
                var seviceProduct = WorkContext.Resolve<IProductsService>();
                var list = listId.Split(',');
                var listProducts = new List<CartInfo>();
                if (WorkContext.HttpContext.Session[Extensions.Constants.KeyDataCart] != null)
                {
                    listProducts = (List<CartInfo>) WorkContext.HttpContext.Session[Extensions.Constants.KeyDataCart];
                }
                for (int i = 0; i < list.Length; i++)
                {
                    var id = list[i];
                    var product = seviceProduct.GetById(Convert.ToInt32(id));
                    var quantity = Request.Form["txtQuantity_" + id];
                    if (string.IsNullOrEmpty(quantity) || quantity.Equals("0"))
                    {
                        result += 1;
                        continue;
                    }
                    if (product != null)
                    {
                        var item = listProducts.FirstOrDefault(x => x.ProductId == product.Id);
                        if (item != null && item.ProductId > 0)
                        {
                            listProducts.Remove(item);
                        }
                        var type = serviceCap.GetById(product.Type);
                        item = new CartInfo();
                        item.ProductId = product.Id;
                        item.ProductName = product.Name;
                        item.Capacity = type.Name;
                        item.Quantity = Convert.ToInt32(quantity);
                        item.Price = product.Price;
                        item.Total = product.Price * Convert.ToInt32(quantity);
                        listProducts.Add(item);
                    }
                }

                WorkContext.HttpContext.Session[Extensions.Constants.KeyDataCart] = listProducts;
                if (result == -1 || result < 2)
                {
                    result = 0;
                }
                return result;
            }

            return result;
        }
        #endregion
    }
}
