using CMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class InventoryItemViewModel : NoteTemplateViewModel
    {
        public string ItemId { get; set; }
        public string ItemHeadReferenceId { get; set; }
        public string ItemHeadReferenceType { get; set; }
        public string ItemReferenceId { get; set; }
        public string ItemReferenceType { get; set; }
        public string ItemDescription { get; set; }
        public long ItemQuantity { get; set; }
        public long InventoryQuantity { get; set; }        
        public long ItemPurchaseRate { get; set; }
        public long ItemSellingRate { get; set; }
        public AddDeductEnum? InventoryAction { get; set; }
    } 
}
