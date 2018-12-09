namespace CMSSolutions.Websites.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using CMSSolutions.Data;
    using CMSSolutions.Data.Entity;
    using System.Runtime.Serialization;
    
    
    [DataContract()]
    public class OrderInfo : BaseEntity<int>
    {
        
        [DataMember()]
        [DisplayName("OrderCode")]
        public string OrderCode { get; set; }
        
        [DataMember()]
        [DisplayName("FullName")]
        public string FullName { get; set; }
        
        [DataMember()]
        [DisplayName("PhoneNumber")]
        public string PhoneNumber { get; set; }
        
        [DataMember()]
        [DisplayName("Email")]
        public string Email { get; set; }
        
        [DataMember()]
        [DisplayName("DeliveryTime")]
        public string DeliveryTime { get; set; }
        
        [DataMember()]
        [DisplayName("DeliveryPlace")]
        public string DeliveryPlace { get; set; }
        
        [DataMember()]
        [DisplayName("PaymentType")]
        public int PaymentType { get; set; }
        
        [DataMember()]
        [DisplayName("PaymentCode")]
        public string PaymentCode { get; set; }
        
        [DataMember()]
        [DisplayName("CreateDate")]
        public System.Nullable<System.DateTime> CreateDate { get; set; }
        
        [DataMember()]
        [DisplayName("Notes")]
        public string Notes { get; set; }
        
        [DataMember()]
        [DisplayName("IsPaid")]
        public bool IsPaid { get; set; }
        
        [DataMember()]
        [DisplayName("IsDeliverySuccessful")]
        public bool IsDeliverySuccessful { get; set; }
    }
    
    public class OrdersMapping : EntityTypeConfiguration<OrderInfo>, IEntityTypeConfiguration
    {
        
        public OrdersMapping()
        {
            this.ToTable("Modules_Orders");
            this.HasKey(m => m.Id);
            this.Property(m => m.OrderCode).IsRequired().HasMaxLength(50);
            this.Property(m => m.FullName).IsRequired().HasMaxLength(250);
            this.Property(m => m.PhoneNumber).IsRequired().HasMaxLength(50);
            this.Property(m => m.Email).HasMaxLength(50);
            this.Property(m => m.DeliveryTime).IsRequired().HasMaxLength(250);
            this.Property(m => m.DeliveryPlace).IsRequired().HasMaxLength(250);
            this.Property(m => m.PaymentType).IsRequired();
            this.Property(m => m.PaymentCode).HasMaxLength(50);
            this.Property(m => m.CreateDate);
            this.Property(m => m.Notes).HasMaxLength(500);
            this.Property(m => m.IsPaid).IsRequired();
            this.Property(m => m.IsDeliverySuccessful).IsRequired();
        }
    }
}
