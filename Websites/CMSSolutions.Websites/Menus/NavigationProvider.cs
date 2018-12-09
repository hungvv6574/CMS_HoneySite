using CMSSolutions.Localization;
using CMSSolutions.Web.UI.Navigation;
using CMSSolutions.Websites.Permissions;

namespace CMSSolutions.Websites.Menus
{
    public class NavigationProvider : INavigationProvider
    {
        public Localizer T { get; set; }

        public NavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("Home"), "0", BuildHomeMenu);
            builder.Add(T("Quản lý"), "1", BuildManager);
            builder.Add(T("Đơn hàng"), "2", BuildOrders);
        }

        private void BuildHomeMenu(NavigationItemBuilder builder)
        {
            builder.IconCssClass("fa-home").Action("Index", "Admin", new { area = "" })
                .Permission(AdminPermissions.ManagerAdmin);
        }

        private void BuildManager(NavigationItemBuilder builder)
        {
            builder.IconCssClass("fa-th");

            builder.Add(T("Chuyên mục"), "0", b => b.Action("Index", "AdminCategories", new { area = "" })
                .Permission(CategoriesPermissions.ManagerCategories));

            builder.Add(T("Banners"), "1", b => b.Action("Index", "AdminBanners", new { area = "" })
              .Permission(BannersPermissions.ManagerBanners));

            builder.Add(T("Dòng sản phẩm"), "2", b => b.Action("Index", "AdminProductsGroup", new { area = "" })
                .Permission(ProductsGroupPermissions.ManagerProductsGroup));

            builder.Add(T("Thể tích chai"), "3", b => b.Action("Index", "AdminProductVolume", new { area = "" })
                .Permission(ProductsGroupPermissions.ManagerProductsGroup));

            builder.Add(T("Sản phẩm"), "4", b => b.Action("Index", "AdminProducts", new { area = "" })
                .Permission(ProductsPermissions.ManagerProducts));

            builder.Add(T("Tin tức"), "5", b => b.Action("Index", "AdminArticles", new { area = "" })
                .Permission(ArticlesPermissions.ManagerArticles));

            builder.Add(T("Cửa hàng"), "6", b => b.Action("Index", "AdminStoreBranch", new { area = "" })
                .Permission(StoreBranchPermissions.ManagerStoreBranch));

            builder.Add(T("Liên hệ"), "7", b => b.Action("Index", "AdminEmails", new { area = "" })
               .Permission(EmailsPermissions.ManagerEmails));
            
            builder.Add(T("Cấu hình trang"), "8", b => b.Url("/admin/settings/edit/CMSSolutions.Websites.Controllers.CommonSettings"));
        }

        private void BuildOrders(NavigationItemBuilder builder)
        {
            builder.IconCssClass("fa-truck");
            builder.Add(T("Đơn đặt hàng"), "0", b => b.Action("Index", "AdminOrders", new { area = "" })
                .Permission(OrdersPermissions.ManagerOrders));
        }
    }
}