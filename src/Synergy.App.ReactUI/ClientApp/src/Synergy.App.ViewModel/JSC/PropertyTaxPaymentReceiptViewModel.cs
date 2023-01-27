using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class PropertyTaxPaymentReceiptViewModel : PropertyTaxPaymentViewModel
    {

        public string Id { get; set; }  
        public string ReceiptNumber { get; set; }
        public string ReceiptAmount { get; set;}
       // public string Date { get; set;}
        public DateTime Date { get; set; }
        public string PaymentMode { get; set;} 
        public string DdnNo { get; set;}
        public string InstallmentDate { get; set; }
        public string PaymentFrom { get; set; }
        public string PaymentTo { get; set; }

        public string Amount { get; set; }
        public string Year { get; set; }
    }
}
