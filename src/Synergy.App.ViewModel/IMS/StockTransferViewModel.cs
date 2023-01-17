using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class StockTransferViewModel:ServiceTemplateViewModel
    {
        public string WareHouseLegalEntityId { get; set; }
        public string ChallanNo { get; set; }
        public DateTime? TransferDate { get; set; }
        public string FromWarehouseId { get; set; }
        public string ToWarehouseId { get; set; }
        public string TransferReason { get; set; }    
        public string TransferItemsData { get; set; }
        public string StockTransferId { get; set; }
        public string StockTransferItemId { get; set; }
        public string ItemId { get; set; }
        public string TransferQuantity { get; set; }
        public string ItemName { get; set; }
        public string TransferredBy { get; set; }

        public double ClosingQuantity { get; set; }
        public string NtsNoteId { get; set; }
        public string FromWareHouse { get; set; }
        public string ToWareHouse { get; set; }
        public string ReceiptId { get; set; }
        public string IssueId { get; set; }
        public string BusinessUnitId { get; set; }
        public string RequisitionIssueServiceStatusCode { get; set; }
        public string GoodsReceiptServiceStatusCode { get; set; }
        public string RequisitionIssueId { get; set; }
        public string GoodsReceiptId { get; set; }
        public string GoodsReceiptServiceId { get; set; }
        public string WorkflowStatus { get; set; }
        public string IssuedQuantity { get; set; }
        public BoolStatus IsSerializable { get; set; }
        public bool AllIssued { get; set; }
        public string Issued { get; set; }
        public int SNo { get; set; }

    }
    
}
