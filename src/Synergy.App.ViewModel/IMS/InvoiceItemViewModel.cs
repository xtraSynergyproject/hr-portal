using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class InvoiceItemViewModel : NoteTemplateViewModel
    {
        public string POItemId { get; set; }
        public string POInvoiceId { get; set; }
        public string ItemId { get; set; }
        public string POQuantity { get; set; }
        public string ReceivedQuantity { get; set; }
        public int SNo { get; set; }
        public string ItemName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PurchaseRate { get; set; }
    }
}
