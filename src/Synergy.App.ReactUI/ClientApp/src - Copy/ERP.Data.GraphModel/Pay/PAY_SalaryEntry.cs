
using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{
  
    public partial class PAY_SalaryEntry : NodeBase
    {
        public string SalaryName { get; set; }
        public DateTime PayrollRunDate { get; set; }
        public DateTime PayrollStartDate { get; set; }
        public DateTime PayrollEndDate { get; set; }
        public int? YearMonth { get; set; }
        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetAmount { get; set; }
        public DocumentStatusEnum PublishStatus { get; set; }

        public string BankAccountName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIBanNo { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public PaymentModeEnum PaymentMode { get; set; }

        public long? PayrollId { get; set; }
        public long? PayrollRunId { get; set; }

        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public string Error { get; set; }

        public long TotalProcessed { get; set; }
        public long TotalSucceeded { get; set; }


        public long? JobId { get; set; }
        public string JobName { get; set; }
        public long? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public long? PositionId { get; set; }
        public string PositionName { get; set; }
        public long? GradeId { get; set; }
        public string GradeName { get; set; }
        public long? LocationId { get; set; }
        public string LocationName { get; set; }

        public double ActualWorkingDays { get; set; }
        public double EmployeeWorkingDays { get; set; }
        public double AnnualLeaveDays { get; set; }
        public double AnnualLeaveBalance { get; set; }
        public double SickLeaveDays { get; set; }
        public double UnpaidLeaveDays { get; set; }
        public double OtherLeaveDays { get; set; }
        public TimeSpan? UnderTime { get; set; }
        public TimeSpan? OverTime { get; set; }
    }
    public class R_SalaryEntry_PayrollRun : RelationshipBase
    {

    }
    public class R_SalaryEntry_PersonRoot : RelationshipBase
    {

    }
}
