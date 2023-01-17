
using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{
  
    public partial class PAY_PayrollRunResult : NodeBase
    {
        public string Name { get; set; }
        public DateTime? PayrollRunDate { get; set; }
        public DateTime PayrollStartDate { get; set; }
        public DateTime PayrollEndDate { get; set; }
        public int? YearMonth { get; set; }
        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetAmount { get; set; }

        public string BankAccountNo { get; set; }
        public string BankIBanNo { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }

        public PaymentModeEnum PaymentMode { get; set; }
        public PayrollIntervalEnum? PayrollInterval { get; set; }


        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public string Error { get; set; }

        public long? PayrollId { get; set; }
        public long? PayrollRunId { get; set; }

        public long TotalProcessed { get; set; }
        public long TotalSucceeded { get; set; }

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
    public class R_PayrollRunResult_PayrollRun : RelationshipBase
    {

    }
    public class R_PayrollRunResult_PersonRoot : RelationshipBase
    {

    }
    public class R_PayrollRunResult_PayrollGroup : RelationshipBase
    {

    }

    public class R_PayrollRunResult_PayCalendar : RelationshipBase
    {

    }

    //public class R_PayrollRunResult_ExecutedBy_User : RelationshipBase
    //{

    //}

}
