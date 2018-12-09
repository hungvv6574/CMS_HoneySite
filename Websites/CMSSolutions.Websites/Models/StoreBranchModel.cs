namespace CMSSolutions.Websites.Models
{
    using CMSSolutions.Web.UI.ControlForms;
    using CMSSolutions.Websites.Entities;

    public class StoreBranchModel
    {
        
        [ControlHidden()]
        public int Id { get; set; }
        
        [ControlText(Type=ControlText.TextBox, Required=true, MaxLength=250, ContainerCssClass=Constants.ContainerCssClassCol6, ContainerRowIndex=0)]
        public string Name { get; set; }
        
        [ControlText(Type=ControlText.TextBox, Required=true, MaxLength=500, ContainerCssClass=Constants.ContainerCssClassCol6, ContainerRowIndex=0)]
        public string Address { get; set; }
        
        [ControlText(Type=ControlText.TextBox, Required=true, MaxLength=20, ContainerCssClass=Constants.ContainerCssClassCol6, ContainerRowIndex=1)]
        public string PhoneNumber { get; set; }

        [ControlFileUpload(EnableFineUploader = true, Required = false, LabelText = "Logo", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 1, ShowThumbnail = true)]
        public string LogoUrl { get; set; }

        [ControlText(Type=ControlText.TextBox, Required=false, MaxLength=1073741823, ContainerCssClass=Constants.ContainerCssClassCol12, ContainerRowIndex=2)]
        public string Notes { get; set; }
        
        public static implicit operator StoreBranchModel(StoreBranchInfo entity)
        {
            return new StoreBranchModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Address = entity.Address,
                PhoneNumber = entity.PhoneNumber,
                Notes = entity.Notes,
                LogoUrl = entity.LogoUrl
            };
        }
    }
}
