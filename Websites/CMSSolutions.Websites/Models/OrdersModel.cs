namespace CMSSolutions.Websites.Models
{
    using CMSSolutions.Web.UI.ControlForms;
    using CMSSolutions.Websites.Entities;

    public class OrdersModel
    {
        [ControlHidden()]
        public int Id { get; set; }

        [ControlText(Type=ControlText.TextBox, LabelText = "Mã đơn hàng", Required=true, MaxLength=50, ContainerCssClass=Constants.ContainerCssClassCol3, ContainerRowIndex=0)]
        public string OrderCode { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Hình thức thanh toán", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int PaymentType { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Mã thanh toán", Required = false, MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public string PaymentCode { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Đã thanh toán", Required = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public bool IsPaid { get; set; }

        [ControlText(Type=ControlText.TextBox, LabelText = "Họ và Tên", Required=true, MaxLength=250, ContainerCssClass=Constants.ContainerCssClassCol3, ContainerRowIndex=1)]
        public string FullName { get; set; }
        
        [ControlText(Type=ControlText.TextBox, LabelText = "Số điện thoại", Required=true, MaxLength=50, ContainerCssClass=Constants.ContainerCssClassCol3, ContainerRowIndex=1)]
        public string PhoneNumber { get; set; }
        
        [ControlText(Type=ControlText.TextBox, LabelText = "Địa chỉ email", Required=false, MaxLength=50, ContainerCssClass=Constants.ContainerCssClassCol3, ContainerRowIndex=1)]
        public string Email { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Giao hàng thành công", Required = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public bool IsDeliverySuccessful { get; set; }

        [ControlText(Type=ControlText.TextBox, LabelText = "Thời gian giao hàng", Required=true, MaxLength=250, ContainerCssClass=Constants.ContainerCssClassCol12, ContainerRowIndex=2)]
        public string DeliveryTime { get; set; }
        
        [ControlText(Type=ControlText.TextBox, LabelText = "Địa điểm giao hàng", Required=true, MaxLength=250, ContainerCssClass=Constants.ContainerCssClassCol12, ContainerRowIndex=2)]
        public string DeliveryPlace { get; set; }

        [ControlText(Type=ControlText.TextBox, LabelText = "Ghi chú", Required=false, MaxLength=500, ContainerCssClass=Constants.ContainerCssClassCol12, ContainerRowIndex=3)]
        public string Notes { get; set; }

        public static implicit operator OrdersModel(OrderInfo entity)
        {
            return new OrdersModel
            {
                Id = entity.Id,
                OrderCode = entity.OrderCode,
                FullName = entity.FullName,
                PhoneNumber = entity.PhoneNumber,
                Email = entity.Email,
                DeliveryTime = entity.DeliveryTime,
                DeliveryPlace = entity.DeliveryPlace,
                PaymentType = entity.PaymentType,
                PaymentCode = entity.PaymentCode,
                Notes = entity.Notes,
                IsPaid = entity.IsPaid,
                IsDeliverySuccessful = entity.IsDeliverySuccessful
            };
        }
    }
}
