using System.ComponentModel.DataAnnotations;

namespace CMSSolutions.Websites.Extensions
{
    public enum PaymentType
    {
        [Display(Name = "Thanh toán khi nhận hàng")]
        PaymentWhenReceive = 1,

        [Display(Name = "Thanh toán trực tuyến")]
        PaymentOnline = 2
    }
    public enum Category
    {
        Home = 1,
        About = 2,
        Product = 3,
        News = 4,
        Contact = 5,
        ShoppingCart = 6,
        Payment = 7,
        Blog = 10,
    }

    public enum Status
    {
        [Display(Name = "Đang sử dụng")]
        Approved = 1,

        [Display(Name = "Xóa tạm thời")]
        Deleted = 2

    }
    public class Constants
    {
        public const string DateTimeFomat = "dd/MM/yyyy";
        public const string DateTimeFomat2 = "dd-MM-yyyy";
        public const string DateTimeFomatFullNone = "ddMMyyyyhhmmssfff";

        public const string IsNull = "null";
        public const string IsUndefined = "undefined";
        public const string ImageDefault = "/Media/Default/Themes/no-image.png";
        public const string CssControlCustom = "form-control-custom";
        public const string CssThumbsSize = "thumbs-size";

        public const string LanguageCode = "LanguageCode";
        public const string CategoryId = "CategoryId";
        public const string StatusId = "StatusId";
        public const string ProductTypes = "ProductTypes";
        public const string ProductGroup = "ProductGroup";
        public const string FromDate = "FromDate";
        public const string ToDate = "ToDate";
        public const string SearchText = "SearchText";
        public const string UserId = "UserId";
        public const string ParentId = "ParentId";
        public const string ArticlesId = "ArticlesId";
        public const string OrderId = "OrderId";
        public const string Id = "id";
        public const string KeyDataCart = "DATA_CART_USER";
        public const string KeyOrderId = "ORDER_CART_ID";

        public const string SeoTitle = "SeoTitle";
        public const string SeoDescription = "SeoDescription";
        public const string SeoKeywords = "SeoKeywords";

        public class CacheKeys
        {
            public const string ARTICLES_BY_CATEGORY_ID_TOP = "ARTICLES_BY_CATEGORY_ID_{0}_{1}_{2}_{3}";
            public const string ARTICLES_BY_CATEGORY_ID = "ARTICLES_BY_CATEGORY_ID_{0}_{1}_{2}";
            public const string CATEGORY_ALL_TABLE = "CATEGORY_ALL_TABLE_{0}";
        }
    }
}