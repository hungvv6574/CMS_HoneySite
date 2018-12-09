namespace CMSSolutions.Websites.Models
{
    using CMSSolutions.Web.UI.ControlForms;
    using CMSSolutions.Websites.Entities;

    public class ProductModel
    {
        public ProductModel()
        {
            CategoryId = 3;
            IsPublished = true;
        }

        [ControlHidden()]
        public int Id { get; set; }

        [ControlHidden]
        public string RefId { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Chuyên mục hiển thị", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int CategoryId { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Dòng sản phẩm", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int GroupId { get; set; }
        
        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Dung tích chai", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int Type { get; set; }
        
        [ControlNumeric(LabelText="Giá sản phẩm", Required=true, ContainerCssClass=Constants.ContainerCssClassCol3, ContainerRowIndex=0)]
        public int Price { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tên sản phẩm", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol9, ContainerRowIndex = 1)]
        public string Name { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Đăng tin", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public bool IsPublished { get; set; }

        //[ControlFileUpload(EnableFineUploader = true, Required = true, LabelText = "Ảnh trang chủ", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 2, ShowThumbnail = true)]
        //public string Thumbnail { get; set; }

        [ControlFileUpload(EnableFineUploader = true, Required = true, LabelText = "Ảnh trang sản phẩm", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 2, ShowThumbnail = true)]
        public string Image { get; set; }

        [ControlText(LabelText = "Mô tả ngắn cho sản phẩm", Required = true, PlaceHolder = "Nhập tối đa 400 ký tự.", Type = ControlText.MultiText, Rows = 2, MaxLength = 400, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 4)]
        public string Summary { get; set; }

        [ControlText(LabelText = "Giới thiệu đầy đủ về sản phẩm", Required = true, Type = ControlText.RichText, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 6)]
        public string Contents { get; set; }

        [ControlText(LabelText = "Từ khóa SEO", Required = true, PlaceHolder = "Nhập từ khóa SEO cho sản phẩm. Tối đa 500 ký tự.", Type = ControlText.MultiText, Rows = 2, MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 7)]
        public string Description { get; set; }

        [ControlText(LabelText = "Tags SEO", Required = true, PlaceHolder = "Nhập tags SEO cách nhau bởi dấu ','. Tối đa 500 ký tự.", Type = ControlText.MultiText, Rows = 2, MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 7)]
        public string Tags { get; set; }
        
        public static implicit operator ProductModel(ProductInfo entity)
        {
            return new ProductModel
            {
                Id = entity.Id,
                CategoryId = entity.CategoryId,
                RefId = entity.RefId,
                Name = entity.Name,
                Type = entity.Type,
                Price = entity.Price,
                Summary = entity.Summary,
                Contents = entity.Contents,
                IsPublished = entity.IsPublished,
                Image = entity.Image,
                //Thumbnail = entity.Thumbnail,
                Description = entity.Description,
                Tags = entity.Tags,
                GroupId = entity.GroupId
            };
        }

    }
}
