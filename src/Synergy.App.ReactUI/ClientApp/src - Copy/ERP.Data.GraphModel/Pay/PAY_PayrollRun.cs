
using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{
    public partial class PAY_PayrollRun : NodeBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PayRollNo { get; set; }
        public DateTime? PayrollRunDate { get; set; }
        public DateTime? ExecutionStartDate { get; set; }
        public DateTime? ExecutionEndDate { get; set; }

        public PayrollExecutionStatusEnum ExecutionStatus { get; set; }

        public int? YearMonth { get; set; }
        public PayrollStateEnum PayrollStateStart { get; set; }
        public PayrollStateEnum PayrollStateEnd { get; set; }

        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetAmount { get; set; }
        public bool IsExecuteAllEmployee { get; set; }

        public int TotalProcessed { get; set; }
        public int TotalSucceeded { get; set; }

        //public bool IsFreeze { get; set; }



        public int ExecutePayrollTotal { get; set; }
        public int ExecutePayrollError { get; set; }

        public int PayslipTotal { get; set; }
        public int PayslipError { get; set; }

        public int BankLetterTotal { get; set; }
        public int BankLetterError { get; set; }
        public PayrollExecutionStatusEnum? VacationAccrual { get; set; }
        public PayrollExecutionStatusEnum? FlightTicketAccrual { get; set; }
        public PayrollExecutionStatusEnum? EOSAccrual { get; set; }
        public PayrollExecutionStatusEnum? LoanAccrual { get; set; }
        public PayrollExecutionStatusEnum? SickLeaveAccrual { get; set; }
    }
    public class R_PayrollRun_Payroll : RelationshipBase
    {

    }
    public class R_PayrollRun_ExecutedBy_User : RelationshipBase
    {

    }
    public class R_PayrollRun_PersonRoot : RelationshipBase
    {

    }

}
