using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel.IMS
{
    public class POInvoiceViewModel : ServiceTemplateViewModel
    {
        public string POInvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PoId { get; set; }
        public decimal InvoiceAmount { get; set; }
    }
}
