using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class GoodsReceiptItemViewModel : NoteTemplateViewModel
    {
        public string ReferenceHeaderItemId { get; set; }
        public bool IsTaxable { get; set; }
        public decimal ReturnedToVendorQuantity { get; set; }
        public decimal POQuantity { get; set; }
        public decimal ReceivedQuantity { get; set; }            
        public decimal ItemQuantity { get; set; }
        public decimal IssuedQuantity { get; set; }
        public decimal BalancedQuantity { get; set; }
        public string ItemName { get; set; }
        public string ItemId { get; set; }
        public string WarehouseId { get; set; }
        public string GoodsReceiptId { get; set; }
        public string AdditionalInfo { get; set; }
        public decimal PurchaseRate { get; set; }
        public string IsSerializable { get; set; }
        public int SNo { get; set; }
        public string PONumber { get; set; }
        public DateTime ReceivedOn { get; set; }
        public string ReturnedTo { get; set; }
        public DateTime ReturnedOn { get; set; }        
    }
}
