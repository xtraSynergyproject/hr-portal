using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class SalaryInfoViewModel : DatedViewModelBase    {

        public long SalaryInfoId { get; set; }
        [Display(Name = "Bank Account Number")]
        public string BankAccountNo { get; set; }
        [Display(Name = "Bank IBan Number")]
        public string BankIBanNo { get; set; }
        [Display(Name = "Is Eligible for Salary Transfer Letter")]
        public bool? SalaryTransferLetterProvided { get; set; }
        [Display(Name = "Is Validate Dependent Document For Benefit")]
        public bool? ValidateDependentDocumentForBenefit { get; set; }
        [Display(Name = "Person Name")]
        [Required]
        public long? PersonId { get; set; }
        [Display(Name = "Bank Branch")]
        public long? BankBranchId { get; set; }

        public string PersonName { get; set; }
        [Display(Name = "Payment Mode")]
        [Required]
        public PaymentModeEnum? PaymentMode { get; set; }

        [Display(Name = "Use Time and Attendance Module")]
        public bool TakeAttendanceFromTAA { get; set; }
        [Display(Name = "Is employee eligible for Overtime")]
        public bool IsEligibleForOT { get; set; }
        [Display(Name = "Overtime Payment Type")]
        //[Required]
        public OTPaymentTypeEnum? OTPaymentType { get; set; }

        [Display(Name = "Pay Group")]
        [Required]
        public long? PayGroupId { get; set; }

  
        [Display(Name = "Pay Calendar")]
        [Required]
        public long? PayCalendarId { get; set; }

        public long? UserId { get; set; }

        [Display(Name = "Total Salary")]
        public double? TotalSalary { get; set; }
        [Display(Name = "Is employee eligible for Flight tickets for self")]
        public bool? IsEligibleForAirTicketForSelf { get; set; }

        [Display(Name = "Is employee eligible for Flight tickets for Dependants")]
        public bool? IsEligibleForAirTicketForDependant { get; set; }

        public string PersonNo { get; set; }
        public string SponsorshipNo { get; set; }

        [Display(Name = "Flight ticket frequency")]
        public FlightTicketFrequentEnum? FlightTicketFrquency { get; set; }
        public int AirTicketInterval { get; set; }
        [Display(Name = "Is employee eligible for End of Service")]
        public bool? IsEligibleForEOS { get; set; }

        [Display(Name = "Disable Flight Ticket Processing in Payroll")]
        public bool? DisableProcessingTicket { get; set; }
        [Display(Name = "Unpaid Leaves Not In System")]
        public double? UnpaidLeavesNotInSystem { get; set; }
    }

}

