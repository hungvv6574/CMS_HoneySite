namespace CMSSolutions.Websites.Entities
{
    using System.ComponentModel;
    using CMSSolutions.Data;
    using CMSSolutions.Data.Entity;
    using System.Runtime.Serialization;

    [DataContract()]
    public class ProductsGroupInfo : BaseEntity<int>
    {
        [DataMember()]
        [DisplayName("GroupName")]
        public string GroupName { get; set; }

        [DataMember()]
        [DisplayName("Alias")]
        public string Alias { get; set; }

        [DataMember()]
        [DisplayName("ImageUrl")]
        public string ImageUrl { get; set; }
        
        [DataMember()]
        [DisplayName("HomeImageUrl")]
        public string HomeImageUrl { get; set; }
        
        [DataMember()]
        [DisplayName("BackgroundImageUrl")]
        public string BackgroundImageUrl { get; set; }
        
        [DataMember()]
        [DisplayName("Notes")]
        public string Notes { get; set; }
    }
    
    public class ProductsGroupMapping : EntityTypeConfiguration<ProductsGroupInfo>, IEntityTypeConfiguration
    {
        public ProductsGroupMapping()
        {
            this.ToTable("Modules_ProductsGroup");
            this.HasKey(m => m.Id);
            this.Property(m => m.GroupName).IsRequired().HasMaxLength(250);
            this.Property(m => m.Alias).IsRequired().HasMaxLength(250);
            this.Property(m => m.ImageUrl).HasMaxLength(500);
            this.Property(m => m.HomeImageUrl).HasMaxLength(500);
            this.Property(m => m.BackgroundImageUrl).HasMaxLength(500);
            this.Property(m => m.Notes).HasMaxLength(500);
        }
    }
}
