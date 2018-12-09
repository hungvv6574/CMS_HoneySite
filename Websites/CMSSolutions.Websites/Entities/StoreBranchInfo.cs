namespace CMSSolutions.Websites.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using CMSSolutions.Data;
    using CMSSolutions.Data.Entity;
    using System.Runtime.Serialization;
    
    
    [DataContract()]
    public class StoreBranchInfo : BaseEntity<int>
    {
        
        [DataMember()]
        [DisplayName("Name")]
        public string Name { get; set; }
        
        [DataMember()]
        [DisplayName("Address")]
        public string Address { get; set; }
        
        [DataMember()]
        [DisplayName("PhoneNumber")]
        public string PhoneNumber { get; set; }
        
        [DataMember()]
        [DisplayName("Notes")]
        public string Notes { get; set; }
        
        [DataMember()]
        [DisplayName("LogoUrl")]
        public string LogoUrl { get; set; }
    }
    
    public class StoreBranchMapping : EntityTypeConfiguration<StoreBranchInfo>, IEntityTypeConfiguration
    {
        
        public StoreBranchMapping()
        {
            this.ToTable("Modules_StoreBranch");
            this.HasKey(m => m.Id);
            this.Property(m => m.Name).IsRequired().HasMaxLength(250);
            this.Property(m => m.Address).IsRequired().HasMaxLength(500);
            this.Property(m => m.PhoneNumber).IsRequired().HasMaxLength(20);
            this.Property(m => m.LogoUrl).HasMaxLength(500);
        }
    }
}
