namespace CMSSolutions.Websites.Models
{
    using CMSSolutions.Web.UI.ControlForms;
    using CMSSolutions.Websites.Entities;

    public class ProductsGroupModel
    {
        [ControlHidden()]
        public int Id { get; set; }
        
        [ControlText(Type=ControlText.TextBox, Required=true, MaxLength=250, ContainerCssClass=Constants.ContainerCssClassCol6, ContainerRowIndex=0)]
        public string GroupName { get; set; }

        [ControlFileUpload(EnableFineUploader = true, Required = true, LabelText = "Ảnh đại diện", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 0, ShowThumbnail = true)]
        public string ImageUrl { get; set; }
        
        [ControlFileUpload(EnableFineUploader = true, Required = true, LabelText = "Ảnh đại diện trang chủ", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 1, ShowThumbnail = true)]
        public string HomeImageUrl { get; set; }
        
        [ControlFileUpload(EnableFineUploader = true, Required = true, LabelText = "Ảnh nền khi xem sản phẩm", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 1, ShowThumbnail = true)]
        public string BackgroundImageUrl { get; set; }

        [ControlText(LabelText = "Mô tả cho nhóm", Required = true, PlaceHolder = "Nhập tối đa 500 ký tự.", Type = ControlText.MultiText, Rows = 2, MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 2)]
        public string Notes { get; set; }
        
        public static implicit operator ProductsGroupModel(ProductsGroupInfo entity)
        {
            return new ProductsGroupModel
            {
                Id = entity.Id,
                GroupName = entity.GroupName,
                ImageUrl = entity.ImageUrl,
                HomeImageUrl = entity.HomeImageUrl,
                BackgroundImageUrl = entity.BackgroundImageUrl,
                Notes = entity.Notes
            };
        }
    }
}
