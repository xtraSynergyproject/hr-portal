using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel.Pay
{
    public class MandatoryDeductionElementViewModel : ViewModelBase
    {
        public string MandatoryDeductionId { get; set; }
        public string PayRollElementId { get; set; }
        public DateTime? ESD { get; set; }
        public DateTime? EED { get; set; }

        public string Name { get; set; }
        public string LegalEntityId { get; set; }
    }

    public class MandatoryDeductionViewModel : NoteTemplateViewModel
    {
        public string DeductionName { get; set; }
        public string DeductionCode { get; set; }
        public string DeductionValueType { get; set; }
        public string DeductionSlabType { get; set; }
        public string DeductionCalculationType { get; set; }
        public string PayrollElementId { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public double SeniorCitizenAmount { get; set; }
        public double WomanAmount { get; set; }
        public double EmployeeAmount { get; set; }
        public double TotalAmount { get; set; }
    }
}
