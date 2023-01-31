using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCBillPaymentReportViewModel
    {
        public string ServiceId { get; set; }
        public string ServiceNo { get; set; }
        public string AssetName { get; set; }
        public string OwnerName { get; set; }
        public string ConsumerNo { get; set; }
        public DateTime? DueDate { get; set; }
        public string DueDateText { get; set; }
        public string BillAmount { get; set; }
        public string ReferenceNo { get; set; }
        public string PaymentStatus { get; set; }
        public string LogoId { get; set; }
        public string Logo { get; set; }
        
    }
}
