using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.Globalization;


namespace ERP.UI.ViewModel.Manpower
{
    public class SalarySlipViewModel
    {
        //Employee Details
        public string EmployeeNo { get; set; }
        public string EmployeeFullName { get; set; }
        public string DateOfJoin { get; set; }
        public string Organization { get; set; }
        public string Grade { get; set; }
        public string Position { get; set; }
        public string MonthSalary { get; set; }

        //Earnings Details
        public string EarningsCaption { get; set; }
        public string EarningsTotal { get; set; }

        //Deductions Details
        public string DeductionsCaption { get; set; }
        public string DeductionsTotal { get; set; }

        //Common Details
        public string TotalNetSalary { get; set; }
        public string Remarks { get; set; }
        public int PageWidth { get; set; }
        public int PageHeight { get; set; }
        public string Description { get; set; }
        public string Html { get; set; }
        public string Amount { get; set; }
        public string VariablePay { get; set; }
        public string VariablePayAmount { get; set; }
        public string SalaryMonth { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public decimal LeaveBalance { get; set; }
        public string CurrentDateTime { get; set; }

        //Salary Elements List
        public List<SalaryElements> SalaryEarningsElements { get; set; }
        public List<SalaryElements> SalaryDeductionsElements { get; set; }

    }

    public class SalaryElements
    {
        public string ElementName { get; set; }
        public decimal Amount { get; set; }
        
    }
}
