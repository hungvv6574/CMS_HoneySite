using System.ComponentModel.DataAnnotations.Schema;

namespace CMSSolutions.Websites.Entities
{
    using System.ComponentModel;
    using CMSSolutions.Data;
    using CMSSolutions.Data.Entity;
    using System.Runtime.Serialization;
    
    
    [DataContract()]
    public class ProductInfo : BaseEntity<int>
    {
        
        [DataMember()]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }
        
        [DataMember()]
        [DisplayName("CategoryId")]
        public int CategoryId { get; set; }

        [NotMapped]
        [DisplayName("CategoryAlias")]
        public string CategoryAlias { get; set; }

        [NotMapped]
        [DisplayName("CategoryName")]
        public string CategoryName { get; set; }

        [DataMember()]
        [DisplayName("RefId")]
        public string RefId { get; set; }
        
        [DataMember()]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DataMember()]
        [DisplayName("GroupId")]
        public int GroupId { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string GroupName { get; set; }

        [DataMember()]
        [DisplayName("Alias")]
        public string Alias { get; set; }
        
        [DataMember()]
        [DisplayName("Type")]
        public int Type { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string CapacityName { get; set; }
        
        [DataMember()]
        [DisplayName("Price")]
        public int Price { get; set; }

        [DataMember()]
        [DisplayName("Summary")]
        public string Summary { get; set; }

        [DataMember()]
        [DisplayName("Contents")]
        public string Contents { get; set; }
        
        [DataMember()]
        [DisplayName("IsPublished")]
        public bool IsPublished { get; set; }
        
        [DataMember()]
        [DisplayName("Image")]
        public string Image { get; set; }

        [DataMember()]
        [DisplayName("Thumbnail")]
        public string Thumbnail { get; set; }
        
        [DataMember()]
        [DisplayName("Description")]
        public string Description { get; set; }
        
        [DataMember()]
        [DisplayName("Tags")]
        public string Tags { get; set; }
        
        [DataMember()]
        [DisplayName("IsDeleted")]
        public bool IsDeleted { get; set; }
    }
    
    public class ProductsMapping : EntityTypeConfiguration<ProductInfo>, IEntityTypeConfiguration
    {
        
        public ProductsMapping()
        {
            this.ToTable("Modules_Products");
            this.HasKey(m => m.Id);
            this.Property(m => m.LanguageCode).IsRequired().HasMaxLength(50);
            this.Property(m => m.CategoryId).IsRequired();
            this.Property(m => m.RefId).IsRequired().HasMaxLength(50);
            this.Property(m => m.Name).IsRequired().HasMaxLength(250);
            this.Property(m => m.Alias).IsRequired().HasMaxLength(250);
            this.Property(m => m.Type).IsRequired();
            this.Property(m => m.Price).IsRequired();
            this.Property(m => m.Summary).HasMaxLength(2000);
            this.Property(m => m.Contents).IsRequired();
            this.Property(m => m.IsPublished).IsRequired();
            this.Property(m => m.Image).HasMaxLength(500);
            this.Property(m => m.Thumbnail).HasMaxLength(500);
            this.Property(m => m.Description).HasMaxLength(500);
            this.Property(m => m.Tags).HasMaxLength(500);
            this.Property(m => m.IsDeleted).IsRequired();
            this.Property(m => m.GroupId).IsRequired();
        }
    }
}
