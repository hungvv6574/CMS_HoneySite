using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = false)]
    public class ContactController : BaseHomeController
    {
        public ContactController(IWorkContextAccessor workContextAccessor) : base(workContextAccessor)
        {

        }

        [Url("lien-he")]
        public ActionResult Index()
        {
            var categoryService = WorkContext.Resolve<ICategoriesService>();
            categoryService.LanguageCode = WorkContext.CurrentCulture;
            var category = categoryService.GetById((int)Category.Contact);
            if (category != null)
            {
                ViewData[Extensions.Constants.SeoTitle] = category.Name;
                ViewData[Extensions.Constants.SeoKeywords] = category.Tags;
                ViewData[Extensions.Constants.SeoDescription] = category.Description;
            }

            var viewRenderer = new ViewRenderer { Context = ControllerContext };

            #region SectionBannerSlider
            var modelSectionBannerSlider = new DataViewerModel();
            var setting = WorkContext.Resolve<CommonSettings>();
            modelSectionBannerSlider.Settings = setting;
            var viewSectionBannerSlider = viewRenderer.RenderPartialView("~/Views/Shared/GoogleMap.cshtml", modelSectionBannerSlider);
            WorkContext.Layout.SectionBanner.Add(new MvcHtmlString(viewSectionBannerSlider));
            #endregion

            #region Products
            var modelStores = new DataViewerModel();
            var seviceStore = WorkContext.Resolve<IStoreBranchService>();
            modelStores.Stores = seviceStore.GetRecords().OrderBy(x => x.Id).ToList();
            modelStores.Settings = setting;
            var viewSectionProducts = viewRenderer.RenderPartialView("~/Views/Shared/PageContact.cshtml", modelStores);
            WorkContext.Layout.SectionProducts.Add(new MvcHtmlString(viewSectionProducts));
            #endregion

            return View("Empty");
        }

        #region Send Emails
        [HttpPost, ValidateInput(false)]
        [Url("lien-he/gui-tin-nhan")]
        public ActionResult ContactInformations()
        {
            var email = Request.Form["txtEmail"];
            var fullName = Request.Form["txtFullName"];
            var phoneNumber = Request.Form["txtPhoneNumber"];
            var messages = Request.Form["txtMessages"];

            var result = new DataViewerModel();
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<div style=\"float:left;width:100%;\">");
            htmlBuilder.AppendFormat("<div style=\"float:left;\">{0} </div>", T("Họ và Tên:"));
            htmlBuilder.AppendFormat("<div style=\"float:left;margin-left:5px;\">{0}</div>", fullName);
            htmlBuilder.Append("</div><br>");

            htmlBuilder.Append("<div style=\"float:left;width:100%;\">");
            htmlBuilder.AppendFormat("<div style=\"float:left;\">{0} </div>", T("Số điện thoại:"));
            htmlBuilder.AppendFormat("<div style=\"float:left;margin-left:5px;\">{0}</div>", phoneNumber);
            htmlBuilder.Append("</div><br>");

            htmlBuilder.Append("<div style=\"float:left;width:100%;\">");
            htmlBuilder.AppendFormat("<div style=\"float:left;\">{0} </div>", T("Địa chỉ email:"));
            htmlBuilder.AppendFormat("<div style=\"float:left;margin-left:5px;\">{0}</div>", email);
            htmlBuilder.Append("</div><br>");

            htmlBuilder.Append("<div style=\"float:left;width:100%;\">");
            htmlBuilder.AppendFormat("<div style=\"float:left;\">{0} </div>", T("Yêu cầu của bạn:"));
            htmlBuilder.AppendFormat("<div style=\"float:left;margin-left:5px;\">{0}</div>", messages);
            htmlBuilder.Append("</div><br>");

            string html = System.IO.File.ReadAllText(Server.MapPath("~/Media/Default/EmailTemplates/ContactInfo.html"));
            html = html.Replace("[MAILBODY]", htmlBuilder.ToString());
            try
            {
                SendEmail(T("Thông tin liên hệ"), html, email, string.Empty);
                result.Status = true;
                result.Data = T("Gửi thành công.");
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Data = ex.Message;
            }

            return Json(result);
        }
        #endregion
    }
}
