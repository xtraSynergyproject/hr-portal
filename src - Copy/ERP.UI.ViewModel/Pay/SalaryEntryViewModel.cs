using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
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

        public long? PayrollId { get; set; }
        public long? PayrollRunId { get; set; }
        public long? PersonId { get; set; }
        public long? UserId { get; set; }
        public string PersonFullName { get; set; }
        public string SponsorshipNo { get; set; }


        public long? PayrollGroupId { get; set; }
        public long? PayrollCalendarId { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public long? JobId { get; set; }
        public string JobName { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public long? PositionId { get; set; }
        public string PositionName { get; set; }
        public long? GradeId { get; set; }
        public string GradeName { get; set; }
        public long? LocationId { get; set; }
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
    }
}
