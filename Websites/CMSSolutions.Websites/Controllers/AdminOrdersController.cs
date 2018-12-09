using System.IO;
using CMSSolutions.Extensions;
using CMSSolutions.Websites.Extensions;
using GemBox.Spreadsheet;

namespace CMSSolutions.Websites.Controllers
{
    using System;
    using System.Web.Mvc;
    using CMSSolutions;
    using CMSSolutions.Web.Mvc;
    using CMSSolutions.Web.Themes;
    using CMSSolutions.Web.UI.ControlForms;
    using CMSSolutions.Websites.Entities;
    using CMSSolutions.Websites.Models;
    using CMSSolutions.Websites.Services;
    using CMSSolutions.Web;
    using CMSSolutions.Web.UI.Navigation;
    using CMSSolutions.Web.Routing;

    [Authorize()]
    [Themed(IsDashboard=true)]
    public class AdminOrdersController : BaseAdminController
    {
        
        private readonly IOrdersService service;
        
        public AdminOrdersController(IWorkContextAccessor workContextAccessor, IOrdersService service) : 
                base(workContextAccessor)
        {
            this.service = service;
            this.TableName = "tblOrders";
        }
        
        [Url("admin/orders")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý đơn hàng"), Url = "#" });
            var result = new ControlGridFormResult<OrderInfo>();
            var siteSettings = WorkContext.Resolve<SiteSettings>();

            result.Title = this.T("Quản lý đơn hàng");
            result.CssClass = "table table-bordered table-striped";
            result.UpdateActionName = "Update";
            result.IsAjaxSupported = true;
            result.DefaultPageSize = siteSettings.DefaultPageSize;
            result.EnablePaginate = true;
            result.FetchAjaxSource = this.GetOrders;
            result.GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml;
            result.GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml;
            result.ClientId = TableName;

            result.AddColumn(x => x.OrderCode, T("Mã"));
            result.AddColumn(x => x.FullName, T("Họ và Tên"));
            result.AddColumn(x => x.PhoneNumber, T("Số điện thoại"));
            result.AddColumn(x => CMSSolutions.Extensions.EnumExtensions.GetDisplayName((PaymentType)x.PaymentType), T("Hình thức thanh toán"));
            result.AddColumn(x => x.IsPaid)
                .HasHeaderText(T("Đã thanh toán"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage();
            result.AddColumn(x => x.IsDeliverySuccessful)
                .HasHeaderText(T("Đã giao hàng"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage();

            result.AddRowAction().HasText(this.T("Xem")).HasUrl(x => Url.Action("Edit", new { id = x.Id })).HasButtonStyle(ButtonStyle.Default).HasButtonSize(ButtonSize.ExtraSmall);
            //result.AddRowAction().HasText(this.T("Export")).HasUrl(x => Url.Action("ExportExcelSend")).HasButtonStyle(ButtonStyle.Default).HasButtonSize(ButtonSize.ExtraSmall);
            result.AddRowAction(true).HasText(this.T("Hủy")).HasName("Delete").HasValue(x => Convert.ToString(x.Id)).HasButtonStyle(ButtonStyle.Danger).HasButtonSize(ButtonSize.ExtraSmall).HasConfirmMessage(this.T("Bạn có chắc chắn muốn hủy đơn hàng này không?"));

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");
            return result;
        }
        
        private ControlGridAjaxData<OrderInfo> GetOrders(ControlGridFormRequest options)
        {
            int totals;
            var items = this.service.GetRecords(options, out totals);
            var result = new ControlGridAjaxData<OrderInfo>(items, totals);
            return result;
        }
        
        [Url("admin/orders/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý đơn hàng"), Url = "#" });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Xem thông tin đơn hàng"), Url = Url.Action("Index") });
            var model = new OrdersModel();
            if (id > 0)
            {
                model = this.service.GetById(id);
            }

            var result = new ControlFormResult<OrdersModel>(model)
            {
                Title = this.T("Xem thông tin đơn hàng"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                ShowCancelButton = false,
                ShowBoxHeader = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };
            result.MakeReadOnlyProperty(x => x.OrderCode);
            result.MakeReadOnlyProperty(x => x.FullName);
            result.MakeReadOnlyProperty(x => x.PhoneNumber);
            result.MakeReadOnlyProperty(x => x.Email);
            result.MakeReadOnlyProperty(x => x.PaymentType);
            result.MakeReadOnlyProperty(x => x.PaymentCode);
            result.RegisterExternalDataSource(x => x.PaymentType, y => BindPaymentTypes());

            var result2 = new ControlGridFormResult<OrderDetailsInfo>
            {
                Title = T("Danh sách sản phẩm"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetProducts,
                DefaultPageSize = WorkContext.DefaultPageSize,
                EnablePaginate = true,
                UpdateActionName = "Update",
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml,
                ClientId = TableName
            };

            result2.AddCustomVar(Extensions.Constants.OrderId, id);
            result2.AddColumn(x => x.ProductName, T("Tên sản phẩm"));
            result2.AddColumn(x => x.Capacity, T("Thể tích"));
            result2.AddColumn(x => x.Quantity, T("Số lượng"));
            result2.AddColumn(x => x.Price, T("Giá"));
            result2.AddColumn(x => (x.Price * x.Quantity), T("Thành tiền"));

            result.AddAction().HasText(this.T("Back")).HasUrl(this.Url.Action("Index")).HasButtonStyle(ButtonStyle.Danger);
            
            return new ControlFormsResult(result, result2);
        }

        private ControlGridAjaxData<OrderDetailsInfo> GetProducts(ControlGridFormRequest options)
        {
            var orderId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.OrderId]))
            {
                orderId = Convert.ToInt32(Request.Form[Extensions.Constants.OrderId]);
            }

            var totals = 0;
            var items = WorkContext.Resolve<IOrderDetailsService>().GetRecords(x => x.OrderId == orderId);
            var result = new ControlGridAjaxData<OrderDetailsInfo>(items, totals);

            return result;
        }

        [HttpPost()]
        [FormButton("Save")]
        [ValidateInput(false)]
        [Url("admin/orders/update")]
        public ActionResult Update(OrdersModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            OrderInfo item = model.Id == 0 ? new OrderInfo() : service.GetById(model.Id);
            item.DeliveryTime = model.DeliveryTime;
            item.DeliveryPlace = model.DeliveryPlace;
            item.PaymentType = model.PaymentType;
            item.Notes = model.Notes;
            item.IsPaid = model.IsPaid;
            item.IsDeliverySuccessful = model.IsDeliverySuccessful;

            service.Save(item);

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE").Alert(T("Cập nhật thành công."));
        }

        [ActionName("Update")]
        [FormButton("Delete")]
        public ActionResult Delete(int id)
        {
            var model = service.GetById(id);
            service.Delete(model);
            var serviceDetails = WorkContext.Resolve<IOrderDetailsService>();
            var list = serviceDetails.GetRecords(x => x.OrderId == id);
            serviceDetails.DeleteMany(list);

            return new AjaxResult().NotifyMessage("DELETE_ENTITY_COMPLETE").Alert(T("Đã hủy đơn hàng thành công."));
        }

        //[Url("admin/orders/export")]
        //public ActionResult ExportExcel(int orderId)
        //{
        //    var ef = new ExcelFile();
            //string fileName = Server.MapPath("/Media/Default/ExcelTemplate/HoaDonThanhToan.xls");
            //ef.LoadXls(fileName, XlsOptions.PreserveAll);
            //byte[] fileContents;
            //ExcelWorksheet ws = ef.Worksheets[0];
            //var data = WorkContext.Resolve<ITransactionBankService>().ExportExcelAtm(Utilities.DateNull(), Utilities.DateNull(), (int)TransferType.Send);
            //if (data != null && data.Count > 0)
            //{
            //    int index = 1;
            //    foreach (var item in data)
            //    {
            //        ws.Cells[index, 0].Value = index;
            //        ws.Cells[index, 1].Value = item.CustomerCode;
            //        ws.Cells[index, 2].Value = item.FullName;
            //        ws.Cells[index, 3].Value = item.Amount;
            //        ws.Cells[index, 4].Value = item.CreateDate.ToString(Extensions.Constants.DateTimeFomatFull);
            //        ws.Cells[index, 5].Value = EnumExtensions.GetDisplayName((RequestStatus)item.Status);
            //        index++;
            //        ws.Rows[index].InsertCopy(1, ws.Rows[1]);
            //    }

            //    ws.Rows[index].Delete();
            //}

        //    using (var stream = new MemoryStream())
        //    {
        //        ef.SaveXls(stream);

        //        //fileContents = stream.ToArray();
        //    }

        //    //return File(fileContents, "application/vnd.ms-excel");
        //    return null;
        //}
    }
}
