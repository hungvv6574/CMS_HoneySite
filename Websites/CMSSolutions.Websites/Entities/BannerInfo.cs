namespace CMSSolutions.Websites.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using CMSSolutions.Data;
    using CMSSolutions.Data.Entity;
    using System.Runtime.Serialization;
    
    
    [DataContract()]
    public class BannerInfo : BaseEntity<int>
    {
        
        [DataMember()]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }
        
        [DataMember()]
        [DisplayName("CategoryId")]
        public int CategoryId { get; set; }
        
        [DataMember()]
        [DisplayName("Type")]
        public int Type { get; set; }

        [DataMember()]
        [DisplayName("Title")]
        public string Title { get; set; }

        [DataMember()]
        [DisplayName("Caption")]
        public string Caption { get; set; }
        
        [DataMember()]
        [DisplayName("Url")]
        public string Url { get; set; }
        
        [DataMember()]
        [DisplayName("ImageUrl")]
        public string ImageUrl { get; set; }
        
        [DataMember()]
        [DisplayName("SortOrder")]
        public int SortOrder { get; set; }
    }
    
    public class BannersMapping : EntityTypeConfiguration<BannerInfo>, IEntityTypeConfiguration
    {
        
        public BannersMapping()
        {
            this.ToTable("Modules_Banners");
            this.HasKey(m => m.Id);
            this.Property(m => m.LanguageCode).IsRequired().HasMaxLength(50);
            this.Property(m => m.CategoryId).IsRequired();
            this.Property(m => m.Type).IsRequired();
            this.Property(m => m.Title).HasMaxLength(250);
            this.Property(m => m.Caption).HasMaxLength(4000);
            this.Property(m => m.Url).HasMaxLength(500);
            this.Property(m => m.ImageUrl).HasMaxLength(500);
            this.Property(m => m.SortOrder).IsRequired();
        }
    }
}
