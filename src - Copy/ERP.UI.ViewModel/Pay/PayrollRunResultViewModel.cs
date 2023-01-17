using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PayrollRunResultViewModel : ViewModelBase
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

        public long PayrollRunId { get; set; }
        public long? PayrollId { get; set; }
        public long PersonId { get; set; }
        public long UserId { get; set; }
        public long? PayrollGroupId { get; set; }
        public long? PayrollCalendarId { get; set; }

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
