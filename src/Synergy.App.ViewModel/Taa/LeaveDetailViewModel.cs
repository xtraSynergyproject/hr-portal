using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class LeaveDetailViewModel : ViewModelBase
    {

        public string ServiceId { get; set; }
        public string UserId { get; set; }
        public string PersonId { get; set; }
        public string TemplateAction { get; set; }
        public string ServiceNo { get; set; }
        public string ServiceSubject { get; set; }
        public string ServiceDescription { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string LeaveType { get; set; }
        public string LeaveTypeCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public NtsActionEnum? LeaveStatusAction { get; set; }
        public AddDeductEnum? AddDeduct { get; set; }
        public double? Adjustment { get; set; }

        public DateTime? ReturnToWork { get; set; }
        [Display(Name = "Return To Work Request")]
        public string ReturnToWorkServiceId { get; set; }
        public string ReturnServiceNo { get; set; }

        [Display(Name = "Handover Service")]
        public string HandoverServiceId { get; set; }
        public string HandoverServiceNo { get; set; }

        [Display(Name = "Cancel Leave")]
        public string CancelServiceId { get; set; }
        public string CancelServiceNo { get; set; }

        [Display(Name = "Duration (Days)")]
        public double? Duration { get; set; }

        public double? DatedDuration { get; set; }
        public double? DatedAllDuration { get; set; }

        public double? HalfDayDuration { get; set; }

        public double? HalfDayDatedDuration { get; set; }
        public double? HalfDayDatedAllDuration { get; set; }

        public double? ThreeFourthDuration { get; set; }

        public double? ThreeFourthDatedDuration { get; set; }
        public double? ThreeFourthDatedAllDuration { get; set; }



        public double? NoPayDuration { get; set; }

        public double? NoPayDatedDuration { get; set; }
        public double? NoPayDatedAllDuration { get; set; }

        [Display(Name = "Calendar Days")]
        public string DurationText { get; set; }

        [Display(Name = "Working Days")]
        public string WorkingDuration { get; set; }
       

        [Display(Name = "Annual Leave Yearly Entitlement")]
        public string YearlyEntitlement { get; set; }
        [Display(Name = "Annual Leave Balance As Of Today")]
        public double? LeaveBalance { get; set; }
        [Display(Name = "Leave Status")]
        public string LeaveStatus { get; set; }
        public String FormattedStartDate
        {

            get
            {
                DateTime startDate = StartDate ?? DateTime.Now.Date;
                if (startDate.Year == DateTime.Now.Year)
                {
                    return startDate.ToString("MMM dd");
                }
                else
                {
                    return startDate.ToString("MMM dd yyyy");
                }

            }

        }

        public String FormattedEndDate
        {

            get
            {
                DateTime startDate = EndDate ?? DateTime.Now.Date;
                if (startDate.Year == DateTime.Now.Year)
                {
                    return startDate.ToString("MMM dd");
                }
                else
                {
                    return startDate.ToString("MMM dd yyyy");
                }

            }

        }

        public String FormattedAppliedDate
        {

            get
            {
                DateTime startDate = AppliedDate ?? DateTime.Now.Date;
                if (startDate.Year == DateTime.Now.Year)
                {
                    return startDate.ToString("MMM dd");
                }
                else
                {
                    return startDate.ToString("MMM dd yyyy");
                }

            }

        }
        public String FormattedrtwDate
        {

            get
            {
                DateTime startDate = ReturnToWork ?? DateTime.Now.Date;
                if (startDate.Year == DateTime.Now.Year)
                {
                    return startDate.ToString("MMM dd");
                }
                else
                {
                    return startDate.ToString("MMM dd yyyy");
                }

            }

        }
        public string TemplateCode { get; set; }
        public string NtsNoteId { get; set; }
        public string AnnualLeaveBalanceAsOfToday { get; set; }
        public string AnnualLeaveContract { get; set; }
        public string FromtoEnd { get { return FormattedStartDate + " - " + FormattedEndDate; }}

        public string TelephoneNumber { get; set; }
        public string AddressDetail { get; set; }
        public string OtherInformation { get; set; }
        public string ServiceStatus { get; set; }
        public string ServiceStatusCode { get; set; }

    }
}
