using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PaySalaryElementViewModel : ViewModelBase
    {

        [Display(Name = "Employee Name")]
        public string PersonName { get; set; }
        public string LegalEntityName { get; set; }
        public DateTime? PayrollRunDate { get; set; }
        public DateTime PayrollStartDate { get; set; }
        public DateTime PayrollEndDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfJoin { get; set; }
        public int? YearMonth { get; set; }
        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetAmount { get; set; }
        [Display(Name = "Net Payable To Employee")]
        public double NetPayableAmount { get; set; }
        public double GrossSalary { get; set; }
        public double OTHours { get; set; }
        public double DeductionHours { get; set; }
        public double LoanRecoveryAmount { get; set; }
        public double Amount { get; set; }
        public long? SalaryEntryId { get; set; }
        public long? UserId { get; set; }

        [Display(Name = "Account No")]
        public string BankAccountNo { get; set; }
        [Display(Name = "Iban No")]
        public string BankIBanNo { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public PaymentModeEnum PaymentMode { get; set; }
        public PayrollIntervalEnum? PayrollInterval { get; set; }
        public DocumentStatusEnum PublishStatus { get; set; }
        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public string Error { get; set; }

        public long? PayrollId { get; set; }
        public long? PayrollRunId { get; set; }
        public long? PersonId { get; set; }
        public string PersonFullName { get; set; }
        [Display(Name = "Iqamah No")]
        public string SponsorshipNo { get; set; }


        public long? PayrollGroupId { get; set; }
        public long? PayrollCalendarId { get; set; }

        public string ElementName { get; set; }
        public long? ElementId { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public string PayrollOrganizationName { get; set; }
        public string JobName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        [Display(Name = "Department Code")]
        public string OrganizationCode { get; set; }
        public string ResponsibilityCenter { get; set; }
        public string CostCenter { get; set; }
        public string NavSection { get; set; }
        public string NavSectionCode { get; set; }

        [Display(Name = "Working Days")]
        public double ActualWorkingDays { get; set; }
        [Display(Name = "Working Days")]
        public double EmployeeWorkingDays { get; set; }
        public double AnnualLeaveBalance { get; set; }
        [Display(Name = "Annual Leave")]
        public double AnnualLeaveDays { get; set; }
        public double SickLeaveDays { get; set; }
        [Display(Name = "Unpaid Leave")]
        public double UnpaidLeaveDays { get; set; }
        [Display(Name = "Other Leave")]
        public double OtherLeaveDays { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? UnderTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? OverTime { get; set; }

        [Display(Name = "Over Time")]
        public string OverTimeText
        {
            get
            {
                return OverTime.ToTimeSpanString();

            }
        }
        [Display(Name = "Deduction Time")]
        public string UnderTimeText
        {
            get
            {
                return UnderTime.ToTimeSpanString();
            }
        }

        public double Element1 { get; set; }
        public double Element2 { get; set; }
        public double Element3 { get; set; }
        public double Element4 { get; set; }
        public double Element5 { get; set; }
        public double Element6 { get; set; }
        public double Element7 { get; set; }
        public double Element8 { get; set; }
        public double Element9 { get; set; }
        public double Element10 { get; set; }
        public double Element11 { get; set; }
        public double Element12 { get; set; }
        public double Element13 { get; set; }
        public double Element14 { get; set; }
        public double Element15 { get; set; }
        public double Element16 { get; set; }
        public double Element17 { get; set; }
        public double Element18 { get; set; }
        public double Element19 { get; set; }
        public double Element20 { get; set; }
        public double Element21 { get; set; }
        public double Element22 { get; set; }
        public double Element23 { get; set; }
        public double Element24 { get; set; }
        public double Element25 { get; set; }
        public double Element26 { get; set; }
        public double Element27 { get; set; }
        public double Element28 { get; set; }
        public double Element29 { get; set; }
        public double Element30 { get; set; }
        public double Element31 { get; set; }
        public double Element32 { get; set; }
        public double Element33 { get; set; }
        public double Element34 { get; set; }
        public double Element35 { get; set; }
        public double Element36 { get; set; }
        public double Element37 { get; set; }
        public double Element38 { get; set; }
        public double Element39 { get; set; }
        public double Element40 { get; set; }
        public double Element41 { get; set; }
        public double Element42 { get; set; }
        public double Element43 { get; set; }
        public double Element44 { get; set; }
        public double Element45 { get; set; }
        public double Element46 { get; set; }
        public double Element47 { get; set; }
        public double Element48 { get; set; }
        public double Element49 { get; set; }
        public double Element50 { get; set; }



        public double SalaryElement1 { get; set; }
        public double SalaryElement2 { get; set; }
        public double SalaryElement3 { get; set; }
        public double SalaryElement4 { get; set; }
        public double SalaryElement5 { get; set; }
        public double SalaryElement6 { get; set; }
        public double SalaryElement7 { get; set; }
        public double SalaryElement8 { get; set; }
        public double SalaryElement9 { get; set; }
        public double SalaryElement10 { get; set; }
        public double SalaryElement11 { get; set; }
        public double SalaryElement12 { get; set; }
        public double SalaryElement13 { get; set; }
        public double SalaryElement14 { get; set; }
        public double SalaryElement15 { get; set; }
        public double SalaryElement16 { get; set; }
        public double SalaryElement17 { get; set; }
        public double SalaryElement18 { get; set; }
        public double SalaryElement19 { get; set; }
        public double SalaryElement20 { get; set; }
        public double SalaryElement21 { get; set; }
        public double SalaryElement22 { get; set; }
        public double SalaryElement23 { get; set; }
        public double SalaryElement24 { get; set; }
        public double SalaryElement25 { get; set; }
        public double SalaryElement26 { get; set; }
        public double SalaryElement27 { get; set; }
        public double SalaryElement28 { get; set; }
        public double SalaryElement29 { get; set; }
        public double SalaryElement30 { get; set; }
        public double SalaryElement31 { get; set; }
        public double SalaryElement32 { get; set; }
        public double SalaryElement33 { get; set; }
        public double SalaryElement34 { get; set; }
        public double SalaryElement35 { get; set; }
        public double SalaryElement36 { get; set; }
        public double SalaryElement37 { get; set; }
        public double SalaryElement38 { get; set; }
        public double SalaryElement39 { get; set; }
        public double SalaryElement40 { get; set; }
        public double SalaryElement41 { get; set; }
        public double SalaryElement42 { get; set; }
        public double SalaryElement43 { get; set; }
        public double SalaryElement44 { get; set; }
        public double SalaryElement45 { get; set; }
        public double SalaryElement46 { get; set; }
        public double SalaryElement47 { get; set; }
        public double SalaryElement48 { get; set; }
        public double SalaryElement49 { get; set; }
        public double SalaryElement50 { get; set; }
        public string PersonNo { get; set; }

        public long? SponsorId { get; set; }
        public string SponsorName { get; set; }
    }
}
