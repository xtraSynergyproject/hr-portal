using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class SalesReturnViewModel:ServiceTemplateViewModel
    {
        public string DirectSaleId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContactId { get; set; }
        public string ContactPersonName { get; set; }
        public DateTime? ReturnDate { get; set; } 
        public string SalesReturnId { get; set; }
        public string DirectSaleItemId { get; set; }
        public string Item { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public double SaleQuantity { get; set; }
        public string ReturnQuantity { get; set; }
        public string ReturnTypeId { get; set; }
        public string ReturnComment { get; set; }
        public string ReturnItems { get; set; }
        public double ItemQuantity { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string NtsNoteId { get; set; }
        public string DirectSaleServiceId { get; set; }
        public string GoodsReceiptId { get; set; }
        public string GoodsReceiptReferenceId { get; set; }
        public string GoodsReceiptStatus { get; set; }
        public string GoodsReceiptServiceId { get; set; }
        public string WarehouseId { get; set; }

    }
}
