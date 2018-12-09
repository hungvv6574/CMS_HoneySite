using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CMSSolutions.DisplayManagement;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = false)]
    public class HomeController : BaseHomeController
    {
        private readonly dynamic shapeFactory;
        public HomeController(IWorkContextAccessor workContextAccessor, IShapeFactory shapeFactory) 
            : base(workContextAccessor)
        {
            this.shapeFactory = shapeFactory;
        }

        [Url("", Priority = 10)]
        public ActionResult LandingPage()
        {
            var categoryService = WorkContext.Resolve<ICategoriesService>();
            categoryService.LanguageCode = WorkContext.CurrentCulture;
            var category = categoryService.GetHomePage();
            if (category != null)
            {
                ViewData[Extensions.Constants.SeoTitle] = category.Name;
                ViewData[Extensions.Constants.SeoKeywords] = category.Tags;
                ViewData[Extensions.Constants.SeoDescription] = category.Description;
            }

            return View("LandingPage");
        }

        [Url("trang-chu")]
        public ActionResult Index()
        {
            var categoryService = WorkContext.Resolve<ICategoriesService>();
            categoryService.LanguageCode = WorkContext.CurrentCulture;
            var category = categoryService.GetHomePage();
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
            modelSectionBannerSlider.ListBannerImages = service.GetRecords(x => x.LanguageCode == WorkContext.CurrentCulture && x.CategoryId == category.Id).OrderBy(x => x.SortOrder).ToList();
            modelSectionBannerSlider.Settings = setting;
            var viewSectionBannerSlider = viewRenderer.RenderPartialView("~/Views/Shared/SliderBanners.cshtml", modelSectionBannerSlider);
            WorkContext.Layout.SectionBanner.Add(new MvcHtmlString(viewSectionBannerSlider));
            #endregion

            #region Products
            var modelProducts = new DataViewerModel();
            var seviceProductGroup = WorkContext.Resolve<IProductsGroupService>();
            modelProducts.ListProductGroup = seviceProductGroup.GetRecords().OrderBy(x => x.GroupName).ToList();
            modelProducts.Settings = setting;
            var viewSectionProducts = viewRenderer.RenderPartialView("~/Views/Shared/PageProducts.cshtml", modelProducts);
            WorkContext.Layout.SectionProducts.Add(new MvcHtmlString(viewSectionProducts));
            #endregion

            #region News
            var modelNews = new DataViewerModel();
            var serviceNews = WorkContext.Resolve<IArticlesService>();
            var list = serviceNews.GetRecords(x => x.IsPublished).OrderBy(x => x.Id).ToList();
            modelNews.Data = BuildHtml(list, setting);
            var viewSectionNews = viewRenderer.RenderPartialView("~/Views/Shared/PageBlog.cshtml", modelNews);
            WorkContext.Layout.SectionBlog.Add(new MvcHtmlString(viewSectionNews));
            #endregion

            return View("Empty");
        }

        private string BuildHtml(List<ArticlesInfo> list, CommonSettings settings)
        {
            var categoryService = WorkContext.Resolve<ICategoriesService>();
            categoryService.LanguageCode = WorkContext.CurrentCulture;
            var html = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                var cate = categoryService.GetById(item.CategoryId);
                var url = Url.Action("NewsDetails", "News", new { categoryAlias = cate.Alias, alias = item.Alias });
                if (i == 0)
                {
                    html.Append("<div class=\"col-md-6 item-news\"><div class=\"zoom-hidden\">");
                    html.AppendFormat("<a href=\"{0}\">", url);
                    html.AppendFormat("<img class=\"img-responsive height-img image-center grow\" src=\"{0}\" />", item.Image);
                    html.Append("</a></div>");
                    html.Append("<div class=\"box-caption\">");
                    html.AppendFormat("<a href=\"{0}\">", url);
                    html.AppendFormat("<h3>{0}</h3>", item.Title);
                    html.Append("</a>");
                    html.AppendFormat("<p>{0}</p>", item.Summary);
                    html.Append("</div>");
                    html.Append("</div>");
                }
                if (i == 1)
                {
                    html.Append("<div class=\"col-md-6\">");
                    html.Append("<div class=\"row\">");
                    html.Append("<div class=\"col-md-4 item-news\"><div class=\"zoom-hidden\">");
                    html.AppendFormat("<a href=\"{0}\">", url);
                    html.AppendFormat("<img class=\"cover grow\" src=\"{0}\" />", item.Image);
                    html.Append("</a></div>");
                    html.Append("<div class=\"box-caption\">");
                    html.AppendFormat("<a href=\"{0}\">", url);
                    html.AppendFormat("<h3 class=\"text-right\">{0}</h3>", item.Title);
                    html.Append("</a>");
                    html.Append("</div>");
                    html.Append("</div>");
                }

                if (i == 2)
                {
                    html.Append("<div class=\"col-md-4 item-news\"><div class=\"zoom-hidden\">");
                    html.AppendFormat("<a href=\"{0}\">", url);
                    html.AppendFormat("<img class=\"cover grow\" src=\"{0}\" />", item.Image);
                    html.Append("</a>");
                    html.Append("<div class=\"box-caption\">");
                    html.AppendFormat("<a href=\"{0}\">", url);
                    html.AppendFormat("<h3 class=\"text-right\">{0}</h3>", item.Title);
                    html.Append("</a></div>");
                    html.Append("</div>");
                    html.Append("</div>");
                }

                if (i == 3)
                {
                    html.Append("<div class=\"col-md-4 item-news\"><div class=\"zoom-hidden\">");
                    html.AppendFormat("<a href=\"{0}\">", url);
                    html.AppendFormat("<img class=\"cover grow\" src=\"{0}\" />", item.Image);
                    html.Append("</a>");
                    html.Append("<div class=\"box-caption\">");
                    html.AppendFormat("<a href=\"{0}\">", url);
                    html.AppendFormat("<h3 class=\"text-right\">{0}</h3>", item.Title);
                    html.Append("</a></div>");
                    html.Append("</div>");
                    html.Append("</div>");
                    html.Append("</div>");
                }
            }

            CategoryInfo category = categoryService.GetById((int) Category.News);
            CategoryInfo category2 = categoryService.GetById((int)Category.Blog);
            var url2 = Url.Action("Index", "News", new { categoryAlias = category2.Alias });
            html.Append("<div class=\"row\">");
            html.Append("<div class=\"col-md-12\">");
            html.Append("<div class=\"our-blog\">");
            html.AppendFormat("<h2>{0}</h2>", settings.TextOurBlog);
            html.AppendFormat("<p>{0}</p>", category.Notes);
            html.Append("<div class=\"btn-news-readmore float-fix\">");
            html.AppendFormat("<a href=\"{0}\">{1}</a>", url2, settings.TextReadMore);
            html.Append("</div>");
            html.Append("<div class=\"clearfix\"></div>");
            html.Append("</div>");
            html.Append("</div>");
            html.Append("</div>");
            html.Append("</div>");

            return html.ToString();
        }
    }
}
