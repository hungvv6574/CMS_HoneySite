namespace CMSSolutions.Websites.Models
{
    using CMSSolutions.Web.UI.ControlForms;
    using CMSSolutions.Websites.Entities;

    public class BannersModel
    {
        [ControlHidden]
        public int Id { get; set; }
        
        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Chuyên mục hiển thị", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int CategoryId { get; set; }

        [ControlNumeric(LabelText = "Sắp xếp", Required = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int SortOrder { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tiêu đề", MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 1)]
        public string Title { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Liên kết", MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 1)]
        public string Url { get; set; }

        [ControlText(Type = ControlText.MultiText, LabelText = "Mô tả",  MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 2)]
        public string Caption { get; set; }

        [ControlFileUpload(EnableFineUploader = true, Required = true, LabelText = "Ảnh banner", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 3, ShowThumbnail = true)]
        public string ImageUrl { get; set; }

        public static implicit operator BannersModel(BannerInfo entity)
        {
            return new BannersModel
            {
                Id = entity.Id,
                CategoryId = entity.CategoryId,
                Caption = entity.Caption,
                Title = entity.Title,
                Url = entity.Url,
                ImageUrl = entity.ImageUrl,
                SortOrder = entity.SortOrder
            };
        }
    }
}
