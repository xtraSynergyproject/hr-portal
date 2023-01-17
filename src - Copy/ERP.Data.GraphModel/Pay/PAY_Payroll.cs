
using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{
    public partial class PAY_Payroll : NodeBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public PayrollRunTypeEnum RunType { get; set; }

        public int? YearMonth { get; set; }
        public DateTime PayrollStartDate { get; set; }
        public DateTime PayrollEndDate { get; set; }

        public DateTime? AttendanceStartDate { get; set; }
        public DateTime? AttendanceEndDate { get; set; }


        public PayrollStatusEnum PayrollStatus { get; set; }
 

        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetAmount { get; set; }


        public int ExecutePayrollTotal { get; set; }
        public int ExecutePayrollError { get; set; }

        public int PayslipTotal { get; set; }
        public int PayslipError { get; set; }

        public int BankLetterTotal { get; set; }
        public int BankLetterError { get; set; }

    }
    public class R_Payroll_PayrollGroup : RelationshipBase
    {

    }
    public class R_Payroll_ExecutedBy_User : RelationshipBase
    {

    }
    //public class R_Payroll_Company_OrganizationRoot : RelationshipBase
    //{

    //}
    public class R_Payroll_LegalEntity_OrganizationRoot : RelationshipBase
    {

    }

}
