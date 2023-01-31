using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class ItemStockViewModel:NoteTemplateViewModel
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemTypeName { get; set; }

        public string ItemUnitId { get; set; }
        public string ItemUnit { get; set; }
        public double BalanceQuantity { get; set; } //hidden current 
        public string WarehouseId { get; set; }//opening balance---additional info
        public string AdditionalInfo { get; set; }
        public string WarehouseName { get; set; }
        public double ClosingQuantity { get; set; } //hidden
        public double UnitRate { get; set; }
        public double MinimumQuantity { get; set; }
        public double MaximumQuantity { get; set; }
        public double OpeningQuantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double CurrentQuantity { get; set; }
        public int SNo { get; set; }
        public double IssuedQuantity { get; set; }
        public string ItemStatus { get; set; }
        public string ReferenceHeaderId { get; set; }
        public double SerialNoCount { get; set; }
        public string NtsNoteId { get; set; }
        public string IsSerializable { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string InOutType { get; set; }
        public double ItemQuantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string TransactionQuantity { get; set; }
    }
}
