using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PayrollExecutionViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public PayrollRunTypeEnum RunType { get; set; }
        public int? YearMonth { get; set; }
        public DateTime PayrollStartDate { get; set; }
        public DateTime PayrollEndDate { get; set; }

        public DateTime ExecutionStartDate { get; set; }
        public DateTime ExecutionEndDate { get; set; }
        public PayrollExecutionStatusEnum ExecutionStatus { get; set; }
        public PayrollStatusEnum PayrollStatus { get; set; }
        public PayrollStateEnum PayrollState { get; set; }

        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetAmount { get; set; }

        [Display(Name = "Payroll Group")]
        public long PayrollGroupId { get; set; }
        [Display(Name = "Payroll Group")]
        public string PayrollGroupName { get; set; }
        public long ExecteUserId { get; set; }
        [Display(Name = "Company")]
        public long OrganizationId { get; set; }
        [Display(Name = "Company")]
        public string OrganizationName { get; set; }

        public bool PayrollExecuted { get; set; }
        public bool PayslipGenerated { get; set; }
        public bool BankLetterPrepared { get; set; }
        public bool PayrollPublished { get; set; }
        public bool PostedToAccounting { get; set; }


        public List<PersonViewModel> EmployeeList { get; set; }
        public List<SalaryElementInfoViewModel> PersonSalaryElementInfoList { get; set; }
        public List<PayrollTransactionViewModel> UnProcessedTransactionList { get; set; }
    }
}
