namespace CMSSolutions.Websites.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using CMSSolutions.Data;
    using CMSSolutions.Data.Entity;
    using System.Runtime.Serialization;
    
    
    [DataContract()]
    public class ProductVolumeInfo : BaseEntity<int>
    {
        
        [DataMember()]
        [DisplayName("Name")]
        public string Name { get; set; }
    }
    
    public class ProductVolumeMapping : EntityTypeConfiguration<ProductVolumeInfo>, IEntityTypeConfiguration
    {
        
        public ProductVolumeMapping()
        {
            this.ToTable("Modules_ProductVolume");
            this.HasKey(m => m.Id);
            this.Property(m => m.Name).IsRequired().HasMaxLength(250);
        }
    }
}
