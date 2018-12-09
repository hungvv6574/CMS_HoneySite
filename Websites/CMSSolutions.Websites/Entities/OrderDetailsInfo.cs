namespace CMSSolutions.Websites.Entities
{
    using System.ComponentModel;
    using CMSSolutions.Data;
    using CMSSolutions.Data.Entity;
    using System.Runtime.Serialization;
    
    
    [DataContract()]
    public class OrderDetailsInfo : BaseEntity<int>
    {
        
        [DataMember()]
        [DisplayName("OrderId")]
        public int OrderId { get; set; }
        
        [DataMember()]
        [DisplayName("ProductId")]
        public int ProductId { get; set; }
        
        [DataMember()]
        [DisplayName("ProductName")]
        public string ProductName { get; set; }
        
        [DataMember()]
        [DisplayName("Capacity")]
        public string Capacity { get; set; }
        
        [DataMember()]
        [DisplayName("Quantity")]
        public int Quantity { get; set; }
        
        [DataMember()]
        [DisplayName("Price")]
        public int Price { get; set; }
        
        [DataMember()]
        [DisplayName("Total")]
        public int Total { get; set; }
    }
    
    public class OrderDetailsMapping : EntityTypeConfiguration<OrderDetailsInfo>, IEntityTypeConfiguration
    {
        
        public OrderDetailsMapping()
        {
            this.ToTable("Modules_OrderDetails");
            this.HasKey(m => m.Id);
            this.Property(m => m.OrderId);
            this.Property(m => m.ProductId);
            this.Property(m => m.ProductName).HasMaxLength(250);
            this.Property(m => m.Capacity).HasMaxLength(250);
            this.Property(m => m.Quantity);
            this.Property(m => m.Price);
            this.Property(m => m.Total);
        }
    }
}
