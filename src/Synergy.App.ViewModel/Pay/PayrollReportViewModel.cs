using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class PayrollReportViewModel : ViewModelBase
    {

        public int? YearMonth { get; set; }
        public MonthEnum? Month { get; set; }
        public int? Year { get; set; }
        [Display(Name = "Person Name")]
        public string PersonId { get; set; }
        public string UserId { get; set; }
        public string PayrollRunId { get; set; }
        public string PayrollId { get; set; }
        public DateTime? EnrollDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }



        public string PersonName { get; set; }

        [Display(Name = "Payment Mode")]
        public PaymentModeEnum? PaymentMode { get; set; }

        [Display(Name = "Is Eligible For OT")]
        public bool IsEligibleForOT { get; set; }
        [Display(Name = "Is Eligible For Self Ticket")]
        public bool? IsEligibleForAirTicketForSelf { get; set; }

        [Display(Name = "Is Eligible For Dependant Ticket")]
        public bool? IsEligibleForAirTicketForDependant { get; set; }
        [Display(Name = "Iqamah No")]
        public string SponsorshipNo { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string JobName { get; set; }
        public string[] Elements { get; set; }
        [Display(Name = "Element Name")]
        public string ElementName { get; set; }
        [Display(Name = "Element Name")]
        public string ElementId { get; set; }
        public string ElementCode { get; set; }

        public string SponsorId { get; set; }
        public double Amount { get; set; }
        public double AmountOld { get; set; }
        public string BankName { get; set; }

        public string BankAccountNo { get; set; }

        public string BankIBanNo { get; set; }

        public bool? SalaryTransferLetterProvided { get; set; }

        public bool? IsTransactionClosed { get; set; }

        public string ElementType { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EffectiveDate { get; set; }

        public double? OpeningBalance { get; set; }
        public double? ClosingBalance { get; set; }
        public double? AccuralAmount { get; set; }

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


        public double EarningAmount { get; set; }
        public double DeductionAmount { get; set; }


        public double? Quantity { get; set; }
        public double? EarningQuantity { get; set; }
        public double? DeductionQuantity { get; set; }
        [Display(Name = "Opening Balance")]
        public double? OpeningQuantity { get; set; }
        [Display(Name = "Closing Balance")]
        public double? ClosingQuantity { get; set; }

        [DataType(DataType.Date)]
        public Nullable<DateTime> PayrollDate
        {
            get
            {
                if (YearMonth != null && YearMonth != 0)
                {
                    return new DateTime(YearMonth.ToString().Substring(0, 4).ToSafeInt(), YearMonth.ToString().Substring(YearMonth.ToString().Length - 2, 2).ToSafeInt(), 1);
                }
                else
                {
                    return null;
                }
            }
        }
        public string PersonNo { get; set; }

        public string RosterText { get; set; }
        public DateTime? AttDate { get; set; }
        public GenderEnum? Gender { get; set; }
        public DependantRelationshipTypeEnum? RelationshipType { get; set; }


        public double? HolidayClosingBalance { get; set; }
        public double? HolidayOpeningBalance { get; set; }
        public double? HolidayMonthlyDueDays { get; set; }
        public double? HolidayLeaveTakenDays { get; set; }

        public double? DayOffMonthlyDueDays { get; set; }
        public double? DayOffLeaveTakenDays { get; set; }
        public double? DaysOffClosingBalance { get; set; }
        public double? DaysOffOpeningBalance { get; set; }
        // public double? TotalSalMonthlyDueAmount { get; set; }
        // public double? TotalSalLeaveTakenAmount { get; set; }
        public double? HolidayMonthlyDueAmount { get; set; }
        public double? HolidayLeaveTakenAmount { get; set; }
        public double? DaysOffMonthlyDueAmount { get; set; }
        public double? DaysOffLeaveTakenAmount { get; set; }
        public DateTime? AttendanceStartDate { get; set; }
        public DateTime? AttendanceEndDate { get; set; }

        public double? AnnualLeaveDays { get; set; }
        public double? SickLeaveDays { get; set; }
        public double? UnpaidLeaveDays { get; set; }
        public double? TotalUnpaidLeaveDays { get; set; }
        public double? PlannedUnpaidLeaveDays { get; set; }
        public double? OtherLeaveDays { get; set; }
        public double? NonWorkingDays { get; set; }

        public double? AnnualLeaveAmount { get; set; }
        public double? SickLeaveAmount { get; set; }
        public double? UnpaidLeaveAmount { get; set; }
        public double? PlannedUnpaidLeaveAmount { get; set; }
        public double? OtherLeaveAmount { get; set; }
        public double? NonWorkingAmount { get; set; }
        public bool DisableSearch { get; set; }



    }
}
