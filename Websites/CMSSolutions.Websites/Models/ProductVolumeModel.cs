namespace CMSSolutions.Websites.Models
{
    using System;
    using CMSSolutions.Web.UI.ControlForms;
    using CMSSolutions.Websites.Entities;
    
    
    public class ProductVolumeModel
    {
        
        [ControlHidden()]
        public int Id { get; set; }
        
        [ControlText(Type=ControlText.TextBox, Required=true, MaxLength=250, ContainerCssClass=Constants.ContainerCssClassCol6, ContainerRowIndex=0)]
        public string Name { get; set; }
        
        public static implicit operator ProductVolumeModel(ProductVolumeInfo entity)
        {
            return new ProductVolumeModel
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

    }
}
