using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class SalaryEntryViewModel : ViewModelBase
    {

        public string SalaryName { get; set; }
        public DateTime? PayrollRunDate { get; set; }
        public DateTime PayrollStartDate { get; set; }
        public DateTime PayrollEndDate { get; set; }
        public DateTime DateOfJoin { get; set; }
        public string DateOfJoinText { get { return DateOfJoin.ToDefaultDateFormat(); } }
        public int? YearMonth { get; set; }
        public MonthEnum? Month { get; set; }
        public int? Year { get; set; }
        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetAmount { get; set; }
        public double VacationBalance { get; set; }
        public double GrossSalary { get; set; }

        public string BankAccountNo { get; set; }
        public string BankIBanNo { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public PaymentModeEnum PaymentMode { get; set; }
        public PayrollIntervalEnum? PayrollInterval { get; set; }
        public DocumentStatusEnum PublishStatus { get; set; }
        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public string Error { get; set; }

        public string PayrollId { get; set; }
        public string PayrollRunId { get; set; }
        [Display(Name ="Person")]
        public string PersonId { get; set; }
        public string UserId { get; set; }
        public string PersonFullName { get; set; }
        public string SponsorshipNo { get; set; }


        public string PayrollGroupId { get; set; }
        public string PayrollCalendarId { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public string JobId { get; set; }
        public string JobName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string PositionId { get; set; }
        public string PositionName { get; set; }
        public string GradeId { get; set; }
        public string GradeName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string PersonNo { get; set; }
        public string NetAmountInWords { get; set; }

        public string[] Elements { get; set; }

        public double ActualWorkingDays { get; set; }
        public double EmployeeWorkingDays { get; set; }
        public double AnnualLeaveDays { get; set; }
        public double SickLeaveDays { get; set; }
        public double UnpaidLeaveDays { get; set; }
        public double OtherLeaveDays { get; set; }
        public TimeSpan? UnderTime { get; set; }
        public TimeSpan? OverTime { get; set; }
        public String CurrencyCode { get; set; }
        public String CompanyNameBasedOnLegalEntity { get; set; }
        public IList<SalaryElementEntryViewModel> PaySlipEarning { get; set; }
        public IList<SalaryElementEntryViewModel> PaySlipDeduction { get; set; }
    }
}
