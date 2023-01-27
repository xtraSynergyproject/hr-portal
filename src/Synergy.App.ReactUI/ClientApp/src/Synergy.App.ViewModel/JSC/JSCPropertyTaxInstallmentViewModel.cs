using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCPropertyTaxInstallmentViewModel : ViewModelBase
    {
        public string ddnNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentFrom { get; set; }
        public DateTime PaymentTo { get; set; }

        public string AssessmentId { get; set; }
        public double Amount { get; set; }
        public string Year { get; set; }
        public string TaxAmountId { get; set; }
        public string RemainingAmount { get; set; }
        public double PaidAmount { get; set; }


    }
}
