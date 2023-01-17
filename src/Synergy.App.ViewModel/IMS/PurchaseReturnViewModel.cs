using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class PurchaseReturnViewModel : ServiceTemplateViewModel
    {
        public string PurchaseId { get; set; }
        public string POId { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorContactId { get; set; }
        public string VendorContactPersonName { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string PurchaseReturnId { get; set; }
        public string PurchaseItemId { get; set; }
        public string Item { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public double PurchaseQuantity { get; set; }
        public double ReturnQuantity { get; set; }
        public string ReturnTypeId { get; set; }
        public string ReturnComment { get; set; }
        public string ReturnItems { get; set; }
        public double ItemQuantity { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string NtsNoteId { get; set; }
        public string PurchaseOrderServiceId { get; set; }
        public string PurchaseOrderServiceNo { get; set; }
        public string GoodsReceiptId { get; set; }
        public string GoodsReceiptReferenceId { get; set; }
        public string GoodsReceiptStatus { get; set; }
        public string GoodsReceiptServiceId { get; set; }
        public string WarehouseId { get; set; }
        public int SNo { get; set; }
        public DateTime? GoodsReceiptDate { get; set; }
    }
}
