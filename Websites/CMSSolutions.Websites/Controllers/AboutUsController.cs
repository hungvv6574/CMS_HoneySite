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
    public class AboutUsController : BaseHomeController
    {
        public AboutUsController(IWorkContextAccessor workContextAccessor) : base(workContextAccessor)
        {

        }

        [Url("gioi-thieu")]
        public ActionResult Index()
        {
            var categoryService = WorkContext.Resolve<ICategoriesService>();
            categoryService.LanguageCode = WorkContext.CurrentCulture;
            var category = categoryService.GetById((int)Category.About);
            if (category != null)
            {
                ViewData[Extensions.Constants.SeoTitle] = category.Name;
                ViewData[Extensions.Constants.SeoKeywords] = category.Tags;
                ViewData[Extensions.Constants.SeoDescription] = category.Description;
            }

            var viewRenderer = new ViewRenderer { Context = ControllerContext };

            #region SectionBannerSlider
            var modelSectionBannerSlider = new DataViewerModel();
            var service = WorkContext.Resolve<IBannersService>();
            modelSectionBannerSlider.Banner = service.GetRecords(x => x.LanguageCode == WorkContext.CurrentCulture && x.CategoryId == category.Id).FirstOrDefault();
            var viewSectionBannerSlider = viewRenderer.RenderPartialView("~/Views/Shared/PageBannerImage.cshtml", modelSectionBannerSlider);
            WorkContext.Layout.SectionBanner.Add(new MvcHtmlString(viewSectionBannerSlider));
            #endregion

            #region News
            var modelNews = new DataViewerModel();
            var serviceNews = WorkContext.Resolve<IArticlesService>();
            var list = serviceNews.GetRecords(x=> x.CategoryId == category.Id).OrderBy(x => x.Id).ToList();
            modelNews.ListArticles = list;
            var viewSectionNews = viewRenderer.RenderPartialView("~/Views/Shared/PageAbout.cshtml", modelNews);
            WorkContext.Layout.SectionBlog.Add(new MvcHtmlString(viewSectionNews));
            #endregion

            return View("Empty");
        }
    }
}
