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
    public class NewsController : BaseHomeController
    {
        public NewsController(IWorkContextAccessor workContextAccessor) : base(workContextAccessor)
        {
            PageIndex = 1;
        }

        [Url("tin-tuc/{categoryAlias}")]
        public ActionResult Index(string categoryAlias)
        {
            var categoryService = WorkContext.Resolve<ICategoriesService>();
            categoryService.LanguageCode = WorkContext.CurrentCulture;
            var category = categoryService.GetByAlias(categoryAlias, WorkContext.CurrentCulture);
            if (category != null)
            {
                ViewData[Extensions.Constants.SeoTitle] = category.Name;
                ViewData[Extensions.Constants.SeoKeywords] = category.Tags;
                ViewData[Extensions.Constants.SeoDescription] = category.Description;

                var viewRenderer = new ViewRenderer { Context = ControllerContext };
                if (Request.QueryString["trang"] != null)
                {
                    PageIndex = int.Parse(Request.QueryString["trang"]);
                }
                PageSize = 3;

                #region News
                var modelNews = new DataViewerModel();
                var serviceNews = WorkContext.Resolve<IArticlesService>();
                var setting = WorkContext.Resolve<CommonSettings>();
                modelNews.Settings = setting;
                var total = 0;
                var list = serviceNews.GetRecords(PageIndex, PageSize, out total, x => x.CategoryId == category.Id && x.IsPublished).OrderBy(x => x.Id).ToList();
                foreach (var articlesInfo in list)
                {
                    articlesInfo.CategoryAlias = category.Alias;
                }
                modelNews.ListArticles = list;
                modelNews.TotalRow = total;
                modelNews.PageIndex = PageIndex;
                modelNews.PageSize = PageSize;
                var viewSectionNews = viewRenderer.RenderPartialView("~/Views/Shared/ListNews.cshtml", modelNews);
                WorkContext.Layout.SectionBlog.Add(new MvcHtmlString(viewSectionNews));
                #endregion
            }

            return View("Empty");
        }

        [Url("tin-tuc/{categoryAlias}/{alias}")]
        public ActionResult NewsDetails(string categoryAlias, string alias)
        {
            var serviceNews = WorkContext.Resolve<IArticlesService>();
            var articles = serviceNews.GetByAlias(alias, WorkContext.CurrentCulture);
            if (articles != null)
            {
                ViewData[Extensions.Constants.SeoTitle] = articles.Title;
                ViewData[Extensions.Constants.SeoKeywords] = articles.Tags;
                ViewData[Extensions.Constants.SeoDescription] = articles.Description;
            }
            
            var viewRenderer = new ViewRenderer { Context = ControllerContext };

            #region News
            var modelNews = new DataViewerModel();
            modelNews.Articles = articles;
            var viewSectionNews = viewRenderer.RenderPartialView("~/Views/Shared/NewsDetails.cshtml", modelNews);
            WorkContext.Layout.SectionBlog.Add(new MvcHtmlString(viewSectionNews));
            #endregion

            return View("Empty");
        }
    }
}
