using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class PayrollRunResultViewModel : NoteTemplateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PayrollRunDate { get; set; }
        public DateTime PayrollStartDate { get; set; }
        public DateTime PayrollEndDate { get; set; }
        public int? YearMonth { get; set; }
        public MonthEnum? Month { get; set; }
        public int? Year { get; set; }
        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetAmount { get; set; }

        public string PayrollRunId { get; set; }
        public string PayrollId { get; set; }
        public string PersonId { get; set; }
        public string UserId { get; set; }
        public string PayrollGroupId { get; set; }
        public string PayrollCalendarId { get; set; }
        public string CalendarId { get; set; }
        public string BankAccountNo { get; set; }
        [Display(Name = "Iban No")]
        public string BankIBanNo { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string BankCode { get; set; }
        public PaymentModeEnum? PaymentMode { get; set; }
        public PayrollIntervalEnum? PayrollInterval { get; set; }
        public DocumentStatusEnum PublishStatus { get; set; }
        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public ElementCategoryEnum? ElementCategory { get; set; }
        public string Error { get; set; }

        public string[] Elements { get; set; }
        public string[] SalaryElements { get; set; }

        public double ActualWorkingDays { get; set; }
        [Display(Name = "Working Days")]
        public double EmployeeWorkingDays { get; set; }
        public double AnnualLeaveBalance { get; set; }
        public double AnnualLeaveDays { get; set; }
        public double SickLeaveDays { get; set; }
        public double UnpaidLeaveDays { get; set; }
        public double PlannedUnpaidLeaveDays { get; set; }
        public double OtherLeaveDays { get; set; }
        public TimeSpan? UnderTime { get; set; }
        public TimeSpan? OverTime { get; set; }

    }
}
