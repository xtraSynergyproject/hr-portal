using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PayrollRunViewModel : ViewModelBase
    {
        [Required]
        public string Name { get; set; }
        public string PayrollName { get; set; }
        public string PayRollNo { get; set; }

        public string Description { get; set; }

        [Display(Name = "Run Type")]
        public PayrollRunTypeEnum RunType { get; set; }
        public int? YearMonth { get; set; }
        public string YearMonthText
        {
            get
            {
                var year = YearMonth.ToSafeString().Substring(0, 4).ToSafeInt();
                var month = YearMonth.ToSafeString().Substring(4, 2).ToSafeInt();
                return Convert.ToDateTime(string.Concat(year, "-", month, "-", 1)).ToMMM_YYYY();
            }
        }
        [Display(Name = "Payroll Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime PayrollStartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime PayrollEndDate { get; set; }
        public DateTime PayrollRunDate { get; set; }
        [Display(Name = "Payroll End Date")]

        public DateTime AttendanceStartDate { get; set; }
        public DateTime AttendanceEndDate { get; set; }

        public bool IsExecuteAllEmployee { get; set; }


        public DateTime ExecutionStartDate { get; set; }
        public DateTime ExecutionEndDate { get; set; }
        public PayrollExecutionStatusEnum ExecutionStatus { get; set; }
        public PayrollStatusEnum PayrollStatus { get; set; }
        [Display(Name = "Payroll State")]
        public PayrollStateEnum PayrollStateStart { get; set; }
        [Display(Name = "Payroll State")]
        public PayrollStateEnum PayrollStateEnd { get; set; }

        public ElementCategoryEnum? ElementCategory { get; set; }

        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetAmount { get; set; }

        [Display(Name = "No. Of Employees Processed")]
        public int TotalProcessed { get; set; }
        [Display(Name = "No. Of Employees Succeeded")]
        public int TotalSucceeded { get; set; }
        public int TotalError { get { return TotalProcessed - TotalSucceeded; } }

        [Display(Name = "Payroll")]
        public long PayrollId { get; set; }
        [Display(Name = "Payroll Group")]
        public string PayrollGroupName { get; set; }
        public long? PayrollGroupId { get; set; }
        public long? PayrollCalendarId { get; set; }
        public long ExecuteUserId { get; set; }
        [Display(Name = "Department")]
        public long OrganizationId { get; set; }
        [Display(Name = "Department")]
        public string OrganizationName { get; set; }


        [Display(Name = "Legal Entity")]
        public long? LegalEntityId { get; set; }
        [Display(Name = "Legal Entity")]
        public string LegalEntityName { get; set; }

        [Display(Name = "Execute Payroll")]
        public bool PayrollExecuted { get; set; }
        [Display(Name = "Generate Payslip")]
        public bool PayslipGenerated { get; set; }
        [Display(Name = "Prepare Bank Letter")]
        public bool BankLetterPrepared { get; set; }
        [Display(Name = "Publish Payroll")]
        public bool PayrollPublished { get; set; }
        [Display(Name = "Post To Account")]
        public bool PostedToAccounting { get; set; }
        [Display(Name = "Close Payroll")]
        public bool ClosePayroll { get; set; }

        [Display(Name = "Re Execute Payroll")]
        public bool RePayrollExecuted { get; set; }
        [Display(Name = "Re Generate Payslip")]
        public bool RePayslipGenerated { get; set; }
        [Display(Name = "Re Prepare Bank Letter")]
        public bool ReBankLetterPrepared { get; set; }
        [Display(Name = "Re Publish Payroll")]
        public bool RePayrollPublished { get; set; }
        [Display(Name = "Re Post To Account")]
        public bool RePostedToAccounting { get; set; }
        [Display(Name = "Re Complete Payroll")]
        public bool ReCompletePayroll { get; set; }
        public long? PayrollRunServiceId { get; set; }


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

        [Display(Name = "Person")]
        public string PersonVal { get; set; }
        public string PersonsNotInList { get; set; }
        public string PersonsInList { get; set; }
        public long[] PersonData { get; set; }

        public PayrollIntervalEnum? PayrollInterval { get; set; }

        public string[] Elements { get; set; }
        public string[] RunElements { get; set; }
        public string[] SalaryElements { get; set; }
        public int StartState
        {
            get
            {
                var start = (int)PayrollStateStart;
                return start;
            }
        }
        public string ExecutionStatusText
        {
            get
            {
                return ExecutionStatus.ToString();

            }
        }
        [Display(Name = "Vacation Accrual")]
        public PayrollExecutionStatusEnum? VacationAccrual { get; set; }

        [Display(Name = "Flight Ticket Accrual")]
        public PayrollExecutionStatusEnum? FlightTicketAccrual { get; set; }
        [Display(Name = "EOS Accrual")]
        public PayrollExecutionStatusEnum? EOSAccrual { get; set; }
        [Display(Name = "Loan Accrual")]
        public PayrollExecutionStatusEnum? LoanAccrual { get; set; }
        [Display(Name = "Sick Leave Accrual")]
        public PayrollExecutionStatusEnum? SickLeaveAccrual { get; set; }



        public string VacationAccrualText { get { return Convert.ToString(VacationAccrual); } }
        public string FlightTicketAccrualText { get { return Convert.ToString(FlightTicketAccrual); } }
        public string EOSAccrualText { get { return Convert.ToString(EOSAccrual); } }
        public string LoanAccrualText { get { return Convert.ToString(LoanAccrual); } }
        public string SickLeaveAccrualText { get { return Convert.ToString(SickLeaveAccrual); } }



        public bool IsVacationAccrual { get; set; }
        public bool IsFlightTicketAccrual { get; set; }
        public bool IsEOSAccrual { get; set; }
        public bool IsLoanAccrual { get; set; }
        public bool IsSickLeaveAccrual { get; set; }
        public bool RunAccrual { get; set; }

        public string ExecutionStatusCss
        {
            get
            {
                return Helper.GetPayrollExecutionStatusCss(ExecutionStatus);
            }
        }
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

    }
}
