using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class GoodsReceiptViewModel:ServiceTemplateViewModel
    {
        public string WarehouseId { get; set; }
        public string POItemsData { get; set; }

        public string GoodsReceiptReferenceId { get; set; }
        public ImsReceiptTypeEnum ReceiptType { get; set; }
        public string Remark { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime ReceiveDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceFile { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime InvoiceDate { get; set; }
        public string ChallonFile { get; set; }
        public string ChallonNo { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime ChallanDate { get; set; }      
        public string ItemHeadId { get; set; }
        public string PONo { get; set; }
        public string ReceiptStatus { get; set; }
        public string POId { get; set; }
        public string GoodsReceiptId { get; set; }
        public string StockTransferId { get; set; }
        public int SNo { get; set; }
        public string ItemName { get; set; }
        public decimal POQuantity { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public string VendorName { get; set; }
        public string ReceivedByName { get; set; }
        public string ToWarehouseId { get; set; }
    }
}

