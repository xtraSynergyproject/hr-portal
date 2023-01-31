using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class SalaryInfoViewModel :NoteTemplateViewModel
    {
       
        //public string Id { get; set; }
        [Display(Name = "Bank Account Number")]
        public string BankAccountNumber { get; set; }
        [Display(Name = "Bank IBan Number")]
        public string BankIBanNumber { get; set; }
        [Display(Name = "Is Eligible for Salary Transfer Letter")]
        public bool? IsEligibleForSalaryTransferLetter { get; set; }
        [Display(Name = "Is Validate Dependent Document For Benefit")]
        public bool? IsValidateDependentDocumentForBenefit { get; set; }
        [Display(Name = "Person Name")]
        [Required]
        public string PersonId { get; set; }
        [Display(Name = "Bank Branch")]
        public string BankBranchId { get; set; }

        public string PersonName { get; set; }
        [Display(Name = "Payment Mode")]
        [Required]
        public PaymentModeEnum? PaymentMode { get; set; }

        [Display(Name = "Use Time and Attendance Module")]
        public bool TakeAttendanceFromTAA { get; set; }
        [Display(Name = "Is employee eligible for Overtime")]
        public bool IsEmployeeEligibleForOvertime { get; set; }
        [Display(Name = "Overtime Payment Type")]
        //[Required]
        public OTPaymentTypeEnum? OvertimePaymentType { get; set; }

        [Display(Name = "Pay Group")]
        [Required]
        public string PayGroupId { get; set; }


        [Display(Name = "Pay Calendar")]
        [Required]
        public string PayCalendarId { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Total Salary")]
        public double? TotalSalary { get; set; }
        [Display(Name = "Is employee eligible for Flight tickets for self")]
        public bool? IsEmployeeEligibleForFlightTicketsForSelf { get; set; }

        [Display(Name = "Is employee eligible for Flight tickets for Dependants")]
        public bool? IsEmployeeEligibleForFlightTicketsForDependants { get; set; }

        public string PersonNo { get; set; }
        public string SponsorshipNo { get; set; }

        [Display(Name = "Flight ticket frequency")]
        public FlightTicketFrequentEnum? FlightTicketFrequency { get; set; }
        public string AirTicketInterval { get; set; }
        [Display(Name = "Is employee eligible for End of Service")]
        public bool? IsEmployeeEligibleForEndOfService { get; set; }

        [Display(Name = "Disable Flight Ticket Processing in Payroll")]
        public bool? DisableFlightTicketProcessingInPayroll { get; set; }
        [Display(Name = "Unpaid Leaves Not In System")]
        public string UnpaidLeavesNotInSystem { get; set; }
        //public DataActionEnum DataAction { get; set; }
        public string StatusName { get; set; }
        public string PayGroupName { get; set; }
        public string PaymentModeCode { get; set; }
    }
}
