using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMSSolutions.Websites.Controllers;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class DataViewerModel
    {
        public int TotalRow { get; set; }

        public int TotalPage
        {
            get
            {
                if (TotalRow <= PageSize)
                {
                    return 1;
                }

                var count = TotalRow % PageSize;
                if ((count == 0))
                {
                    return TotalRow / PageSize;
                }

                return ((TotalRow - count) / PageSize) + 1;
            }
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public bool Status { get; set; }

        public string Html { get; set; }

        public string Data { get; set; }

        public string Url { get; set; }

        public StringBuilder Breadcrumb { get; set; }

        public IList<CategoryInfo> ListCategories { get; set; }

        public List<ArticlesInfo> ListArticles { get; set; }

        public CategoryInfo CategoryInfo { get; set; }

        public ArticlesInfo Articles { get; set; }

        public int CategoryId { get; set; }

        public int ArticlesId { get; set; }

        public string Keyword { get; set; }

        public BannerInfo Banner { get; set; }

        public List<BannerInfo> ListBannerImages { get; set; }

        public List<ProductInfo> Products { get; set; }

        public List<ProductsGroupInfo> ListProductGroup { get; set; }

        public ProductInfo Product { get; set; }

        public ProductsGroupInfo ProductGroup { get; set; }

        public CommonSettings Settings { get; set; }

        public List<StoreBranchInfo> Stores { get; set; }

        public List<CartInfo> ListCart { get; set; }

        public int PaymentType { get; set; }

        public int TotalMoney
        {
            get
            {
                if (ListCart != null)
                {
                    return ListCart.Sum(x => x.Total);
                }
                return 0;
            }
        }
    }
}