using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class PayrollRunViewModel : NoteTemplateViewModel
    {    
        public string PayrollName { get; set; }
        public DateTime PayrollStartDate { get; set; }
        public DateTime PayrollEndDate { get; set; }
        public DateTime PayrollRunDate { get; set; }
        public PayrollExecutionStatusEnum ExecutionStatus { get; set; }
        public int? YearMonth { get; set; }
        public PayrollStateEnum PayrollStateStart { get; set; }
        public PayrollStateEnum PayrollStateEnd { get; set; }


        [Display(Name = "Run Type")]
        public PayrollRunTypeEnum RunType { get; set; }

        public string Name { get; set; }
        public string PayRollNo { get; set; }
        public DateTime? ExecutionStartDate { get; set; }
        public DateTime? ExecutionEndDate { get; set; }       
        public string ExecutedBy { get; set; }
        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetAmount { get; set; }
        public bool IsExecuteAllEmployee { get; set; }
        public int TotalProcessed { get; set; }
        public int TotalSucceeded { get; set; }
        public double ExecutePayrollTotal { get; set; }
        public string ExecutePayrollError { get; set; }

        public double PayslipTotal { get; set; }
        public string PayslipError { get; set; }

        public int BankLetterTotal { get; set; }
        public string BankLetterError { get; set; }
        public PayrollExecutionStatusEnum? VacationAccrual { get; set; }
        public PayrollExecutionStatusEnum? FlightTicketAccrual { get; set; }
        public PayrollExecutionStatusEnum? EOSAccrual { get; set; }
        public PayrollExecutionStatusEnum? LoanAccrual { get; set; }
        public PayrollExecutionStatusEnum? SickLeaveAccrual { get; set; }
        public DateTime AttendanceStartDate { get; set; }
        public DateTime AttendanceEndDate { get; set; }
        public PayrollIntervalEnum? PayrollInterval { get; set; }


        public ElementCategoryEnum? ElementCategory { get; set; }

        public string PayrollId { get; set; }
        public string PayrollBatchId { get; set; }
        public string PayrollPersonId { get; set; }
        public string PersonsNotInList { get; set; }
        public string PersonsInList { get; set; }
        public string PayrollGroupName { get; set; }      
        public string OrganizationName { get; set; }
        public string PayrollGroupId { get; set; }
        public string PayrollRunServiceId { get; set; }
        public List<ElementViewModel> ElementList { get; set; }
        public List<PayrollPersonViewModel> EmployeeList { get; set; }
        public List<SalaryElementInfoViewModel> EmployeeSalaryElementInfoList { get; set; }
        public List<PayrollTransactionViewModel> UnProcessedTransactionList { get; set; }
        public List<PayrollRunResultViewModel> EmployeePayrollRunResult { get; set; }
        public List<PayrollElementRunResultViewModel> EmployeePayrollElementRunResult { get; set; }
        public List<PayrollElementDailyRunResultViewModel> EmployeePayrollElementDailyRunResult { get; set; }
        public List<PayrollPersonViewModel> PayrollActiveEmployeeList { get; set; }
        public List<SalaryEntryViewModel> EmployeeSalaryEntryList { get; set; }
        public List<BankLetterDetailViewModel> BankLetterDetails { get; set; }
        public List<SalaryElementEntryViewModel> EmployeeSalaryElementEntryList { get; set; }

        public string[] RunElements { get; set; }
        public string[] SalaryElements { get; set; }
        public string YearMonthText
        {
            get
            {
                if (YearMonth.IsNotNull()) 
                {
                    var year = YearMonth.ToString().Substring(0, 4).ToSafeInt();
                    var month = YearMonth.ToString().Substring(4, 2).ToSafeInt();
                    return Convert.ToDateTime(string.Concat(year, "-", month, "-", 1)).ToMMM_YYYY();
                }
                return "";
            }
        }
        public string VacationAccrualText { get { return Convert.ToString(VacationAccrual); } }
        public string FlightTicketAccrualText { get { return Convert.ToString(FlightTicketAccrual); } }
        public string EOSAccrualText { get { return Convert.ToString(EOSAccrual); } }
        public string LoanAccrualText { get { return Convert.ToString(LoanAccrual); } }
        public string SickLeaveAccrualText { get { return Convert.ToString(SickLeaveAccrual); } }
        public string VacationAccrualStatusCss
        {
            get
            {
                return Helper.GetPayrollExecutionStatusCss(VacationAccrual);
            }
        }



        public string FlightTicketAccrualStatusCss
        {
            get
            {
                return Helper.GetPayrollExecutionStatusCss(FlightTicketAccrual);
            }
        }
        public string EOSAccrualStatusCss
        {
            get
            {
                return Helper.GetPayrollExecutionStatusCss(EOSAccrual);
            }
        }
        public string LoanAccrualStatusCss
        {
            get
            {
                return Helper.GetPayrollExecutionStatusCss(LoanAccrual);
            }
        }
        public string SickLeaveAccrualStatusCss
        {
            get
            {
                return Helper.GetPayrollExecutionStatusCss(SickLeaveAccrual);
            }
        }
        public bool IsVacationAccrual { get; set; }
        public bool IsFlightTicketAccrual { get; set; }
        public bool IsEOSAccrual { get; set; }
        public bool IsLoanAccrual { get; set; }
        public bool IsSickLeaveAccrual { get; set; }
        public bool RunAccrual { get; set; }
    }
}
