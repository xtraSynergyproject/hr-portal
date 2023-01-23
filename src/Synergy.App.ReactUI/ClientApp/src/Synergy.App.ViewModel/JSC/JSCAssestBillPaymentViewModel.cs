using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCAssestBillPaymentViewModel
    {
        public string Id { get; set; }
        public string NtsNoteId { get; set; }
        public string Email { get; set; }
        public string AllotmentFromDate { get; set; }
        public string AllotmentToDate { get; set; }
        public string Mobile { get; set; }
        public string IdProofImageId { get; set; }
        public string FeeTypeId { get; set; }
        public string ConsumerId { get; set; }
        public string AssetId { get; set; }
        public string PaymentDay { get; set; }
        public string BillGenerationDay { get; set; }
        public string PaymentDueDays { get; set; }
        public double FeeAmount { get; set; }
        public string PaymentTypeCode { get; set; }
        public string FeeEndDate { get; set; }
        public string FeeStartDate { get; set; }

    }
}
