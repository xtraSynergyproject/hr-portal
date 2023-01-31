using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class PropertyTaxPaymentViewModel
    {
        public string TaskNo { get; set; }
        public string FinancialYear { get; set; }
        public string FinancialYearName { get; set; }
        public string PropertyTaxForTheYear { get; set; }
        public string PropertyTaxForTheYearName { get; set; }
        public string ParcelId { get; set; }
        public string WardNo { get; set; }
        public string OwnerName { get; set; }
        public string Address { get; set; }
        public string PropertyName { get; set; }
        public string BillingYear { get; set; }
        public string BillDate { get; set; }
        public string BillNo { get; set; }
        public string TenamentNo { get; set; }
        public string TotalAmount { get; set; }
        public string TotalArea { get; set; }
        public string RateOfTax { get; set; }
        public string PropertyType { get; set; }
        public string LocationFarctorRate { get; set; }
        public string UsageCode { get; set; }
        public string UsageTypeRate { get; set; }
        public string AgeFact { get; set; }
        public string FloorRate { get; set; }
        public string FloorArea { get; set; }
        public DateTime? PreviousArrearPeriodFrom { get; set; }
        public DateTime? PreviousArrearPeriodTo { get; set; }
        public DateTime? AmountDueDate { get; set; }


    }
}
