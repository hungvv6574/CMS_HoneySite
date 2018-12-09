using CMSSolutions.Configuration;
using CMSSolutions.Web.UI.ControlForms;

namespace CMSSolutions.Websites.Controllers
{
    public class CommonSettings : ISettings
    {
        public CommonSettings()
        {
            HomeTitleProduct = "Store";
            HomeButtonProduct = "DETAIL";
            TextReadMore = "Read more";
            TextOurBlog = "Our blog";
            TextExplore = "EXPLORE";
            
            GoogleMapLatitude = "10.84902";
            GoogleMapLongitude = "106.64085";
            GoogleMapCompanyName = "CÔNG TY CỔ PHẦN THỰC PHẨM DINH DƯỠNG SANUCO";
            GoogleMapAddress = "Số 75, Đường 51, Phường 14, Q. Gò Vấp, TP. HCM";
            GoogleMapPhoneNumber = "(08) 3831 0567";
            GoogleMapEmail = "info@saruco.com.vn";
            TitleFromContact = "SEND US AN EMAIL";
            ContactFullName = "First and Last Name";
            ContactEmail = "Email";
            ContactPhoneNumber = "Phone Number";
            ContactMessages = "Messages";
            ContactButtonSend = "SEND";
            TitleFromStore = "HỆ THỐNG PHÂN PHỐI";
            ButtonAddToCart = "Add to cart";
            ButtonBuy = "Buy";
        }

        public bool Hidden { get { return true; } }
        public string Name { get { return "Cấu hình các trang"; } }

        [ControlText(Required = true, LabelText = "EXPLORE", MaxLength = 255, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public string TextExplore { get; set; }

        [ControlText(Required = true, LabelText = "Tiêu đề sản phẩm trang chủ", MaxLength = 255, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public string HomeTitleProduct { get; set; }

        [ControlText(Required = true, LabelText = "Tiêu đề button sản phẩm", MaxLength = 255, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public string HomeButtonProduct { get; set; }

        [ControlText(Required = true, LabelText = "Our blog", MaxLength = 255, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public string TextOurBlog { get; set; }

        [ControlText(Required = true, LabelText = "Read more", MaxLength = 255, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public string TextReadMore { get; set; }

        [ControlText(Required = true, LabelText = "Google Map Latitude", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string GoogleMapLatitude { get; set; }

        [ControlText(Required = true, LabelText = "Google Map Longitude", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string GoogleMapLongitude { get; set; }

        [ControlText(Required = true, LabelText = "Company Name", MaxLength = 255, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 2)]
        public string GoogleMapCompanyName { get; set; }

        [ControlText(Required = true, LabelText = "Title form contact", MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string TitleFromContact { get; set; }

        [ControlText(Required = true, LabelText = "Title list stores", MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string TitleFromStore { get; set; }

        [ControlText(Required = true, LabelText = "Company Address", MaxLength = 255, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string GoogleMapAddress { get; set; }

        [ControlText(Required = true, LabelText = "Company Phone Number", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string GoogleMapPhoneNumber { get; set; }

        [ControlText(Required = true, LabelText = "Company Email", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string GoogleMapEmail { get; set; }

        [ControlText(Required = true, LabelText = "Title fullname", MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string ContactFullName { get; set; }

        [ControlText(Required = true, LabelText = "Title Email", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string ContactEmail { get; set; }

        [ControlText(Required = true, LabelText = "Title Phone Number", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string ContactPhoneNumber { get; set; }

        [ControlText(Required = true, LabelText = "Title Messages", MaxLength = 2000, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string ContactMessages { get; set; }

        [ControlText(Required = true, LabelText = "Button Send", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string ContactButtonSend { get; set; }

        [ControlText(Required = true, LabelText = "Button add to cart", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string ButtonAddToCart { get; set; }

        [ControlText(Required = true, LabelText = "Button Buy", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string ButtonBuy { get; set; }

        public void OnEditing(ControlFormResult<ISettings> controlForm, WorkContext workContext)
        {
            controlForm.ShowCancelButton = false;
            controlForm.CancelButtonUrl = "/admin";
        }
    }
}