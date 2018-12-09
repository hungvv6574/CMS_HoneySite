namespace CMSSolutions.Websites.Models
{
    using CMSSolutions.Web.UI.ControlForms;
    using CMSSolutions.Websites.Entities;
    
    
    public class CategoriesModel
    {
        [ControlHidden]
        public int Id { get; set; }

        [ControlHidden]
        public string RefId { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Chuyên mục gốc", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int ParentId { get; set; }

        [ControlNumeric(LabelText = "Vị trí trên menu", Required = true, MaxLength = 3, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int MenuOrderBy { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Làm trang chủ", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public bool IsHome { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Có chuyên mục con", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public bool HasChilden { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Xóa tạm thời", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public bool IsDeleted { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tên rút gọn", PlaceHolder = "Tối đa 250 ký tự", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 2)]
        public string ShortName { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tên đầy đủ", PlaceHolder = "Tối đa 400 ký tự", Required = true, MaxLength = 400, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 2)]
        public string Name { get; set; }

        [ControlText(LabelText = "Giới thiệu chung", Required = false, Type = ControlText.RichText, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 3)]
        public string Notes { get; set; }

        [ControlText(LabelText = "Mô tả SEO", PlaceHolder = "Tối đa 2000 ký tự", Type = ControlText.MultiText, Required = true, Rows = 2, MaxLength = 2000, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 6)]
        public string Description { get; set; }

        [ControlText(LabelText = "Tags SEO", PlaceHolder = "Tối đa 2000 ký tự", Type = ControlText.MultiText, Required = true, Rows = 2, MaxLength = 2000, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 6)]
        public string Tags { get; set; }
        
        public static implicit operator CategoriesModel(CategoryInfo entity)
        {
            return new CategoriesModel
            {
                Id = entity.Id,
                ParentId = entity.ParentId,
                ShortName = entity.ShortName,
                Name = entity.Name,
                IsHome = entity.IsHome,
                HasChilden = entity.HasChilden,
                Description = entity.Description,
                Tags = entity.Tags,
                Notes = entity.Notes,
                MenuOrderBy = entity.MenuOrderBy,
                IsDeleted = entity.IsDeleted,
                RefId = entity.RefId
            };
        }
    }
}
