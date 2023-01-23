using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel.Pay
{
    public  class MandatoryDeductionSlabViewModel : ViewModelBase
    {

        public string MandatoryDeductionId { get; set; }
        public decimal SlabFrom { get; set; }
        public decimal SlabTo { get; set; }
        public decimal EmployeeDefaultValue { get; set; }
        public decimal EmployerDefaultValue { get; set; }
        public decimal EmployeeSeniorCitizenValue { get; set; }
        public decimal EmployerSeniorCitizenValue { get; set; }
        public decimal EmployeeWomanValue { get; set; }
        public decimal EmployerWomanCitizenValue { get; set; }
        public DateTime? ESD { get; set; }
        public DateTime? EED { get; set; }

        public string LegalEntityId { get;set; }
    }
}
