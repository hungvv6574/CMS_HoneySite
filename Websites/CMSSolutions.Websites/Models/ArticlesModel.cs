namespace CMSSolutions.Websites.Models
{
    using CMSSolutions.Web.UI.ControlForms;
    using CMSSolutions.Websites.Entities;

    public class ArticlesModel
    {
        public ArticlesModel()
        {
            IsPublished = true;
            CategoryId = 4;
        }

        [ControlHidden]
        public int Id { get; set; }

        [ControlHidden]
        public int ViewCount { get; set; }

        [ControlHidden]
        public string RefId { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Chuyên mục hiển thị", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int CategoryId { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Đăng tin", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public bool IsPublished { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Không sử dụng", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public bool IsDeleted { get; set; }

        [ControlText(Type = ControlText.TextBox, PlaceHolder = "Vui lòng nhập tiêu đề bài viết. Tối đa 200 ký tự.", LabelText = "Tiêu đề", Required = true, MaxLength = 200, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 1)]
        public string Title { get; set; }

        [ControlFileUpload(EnableFineUploader = true, Required = true, LabelText = "Ảnh banner", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 2, ShowThumbnail = true)]
        public string BannerUrl { get; set; }

        [ControlFileUpload(EnableFineUploader = true, Required = true, LabelText = "Ảnh đại diện", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 3, ShowThumbnail = true)]
        public string Image { get; set; }

        [ControlText(LabelText = "Tóm tắt nội dung", Required = true, PlaceHolder = "Nhập tối đa 400 ký tự.", Type = ControlText.MultiText, Rows = 2, MaxLength = 400, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 4)]
        public string Summary { get; set; }

        [ControlText(LabelText = "Nội dung bài viết", Required = false, Type = ControlText.RichText, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 6)]
        public string Contents { get; set; }

        [ControlText(LabelText = "Từ khóa SEO", Required = true, PlaceHolder = "Vui lòng nhập từ khóa SEO cho bài viết. Tối đa 400 ký tự.", Type = ControlText.MultiText, Rows = 2, MaxLength = 400, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 7)]
        public string Description { get; set; }

        [ControlText(LabelText = "Tags SEO", Required = true, PlaceHolder = "Nhập từ khóa SEO cách nhau bởi dấu, Tối đa 500 ký tự.", Type = ControlText.MultiText, Rows = 2, MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 7)]
        public string Tags { get; set; }

        public static implicit operator ArticlesModel(ArticlesInfo entity)
        {
            return new ArticlesModel
            {
                Id = entity.Id,
                CategoryId = entity.CategoryId,
                Title = entity.Title,
                Summary = entity.Summary,
                Contents = entity.Contents,
                IsPublished = entity.IsPublished,
                RefId = entity.RefId,
                BannerUrl = entity.BannerUrl,
                Image = entity.Image,
                ViewCount = entity.ViewCount,
                Description = entity.Description,
                Tags = entity.Tags,
                IsDeleted = entity.IsDeleted
            };
        }
    }
}
