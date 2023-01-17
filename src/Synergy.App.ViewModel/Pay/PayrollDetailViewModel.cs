using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class PayrollDetailViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public DateTime? PayrollExecutedDate { get; set; }
        public DateTime? SalaryFromDate { get; set; }
        public DateTime? SalaryToDate { get; set; }
        public int? YearMonth { get; set; }
        public double? TotalEarning { get; set; }
        public double? TotalDeduction { get; set; }
        public double? NetAmount { get; set; }

        public string BankAccountNo { get; set; }
        public string BankIBanNo { get; set; }
        public PaymentModeEnum? PaymentMode { get; set; }
        public PayrollIntervalEnum? PayrollInterval { get; set; }
        public DocumentStatusEnum? PublishStatus { get; set; }
        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public string Error { get; set; }

        public long? PayrollRunId { get; set; }
        public long? PayrollGroupId { get; set; }
        public long? PersonId { get; set; }
        public long? ElementId { get; set; }

        public string PayCalendar { get; set; }
        public string PayGroup { get; set; }
        public string ElementName { get; set; }
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string UserNameWithEmail { get; set; }
        public string Email { get; set; }
        [Display(Name = "Iqamah No")]
        public string SponsorshipNo { get; set; }
        public string DisplayName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string JobName { get; set; }
        public string GradeName { get; set; }
        public string Nationality { get; set; }
        public string Gender { get; set; }
        public string CostCenter { get; set; }


        [Display(Name = "Contract End Date")]
        public DateTime? ContractEndDate { get; set; }

        [Display(Name = "Contract Renewal")]
        public string ContractRenewable { get; set; }
        [Display(Name = "Sponsor")]
        public string Sponsor { get; set; }

        [Display(Name = "Date Of Join")]
        public DateTime? DateOfJoin { get; set; }

        public double Day1Amount { get; set; }
        public double Day2Amount { get; set; }
        public double Day3Amount { get; set; }
        public double Day4Amount { get; set; }
        public double Day5Amount { get; set; }
        public double Day6Amount { get; set; }
        public double Day7Amount { get; set; }
        public double Day8Amount { get; set; }
        public double Day9Amount { get; set; }
        public double Day10Amount { get; set; }
        public double Day11Amount { get; set; }
        public double Day12Amount { get; set; }
        public double Day13Amount { get; set; }
        public double Day14Amount { get; set; }
        public double Day15Amount { get; set; }
        public double Day16Amount { get; set; }
        public double Day17Amount { get; set; }
        public double Day18Amount { get; set; }
        public double Day19Amount { get; set; }
        public double Day20Amount { get; set; }
        public double Day21Amount { get; set; }
        public double Day22Amount { get; set; }
        public double Day23Amount { get; set; }
        public double Day24Amount { get; set; }
        public double Day25Amount { get; set; }
        public double Day26Amount { get; set; }
        public double Day27Amount { get; set; }
        public double Day28Amount { get; set; }
        public double Day29Amount { get; set; }
        public double Day30Amount { get; set; }
        public double Day31Amount { get; set; }
        public double TotalAmount { get; set; }

        public string PayrollElementRunResultId { get; set; }
        public string PersonNo { get; set; }


        public string ExecutionStatusText { get { return ExecutionStatus.ToString(); } }
        public string ExecutionStatusCss
        {
            get
            {
                switch (ExecutionStatus)
                {
                    case ExecutionStatusEnum.Error:
                        return "label label-danger";
                    case ExecutionStatusEnum.Success:
                        return "label label-success";
                    default:
                        return "label label-default";
                }


            }
        }
    }
}
