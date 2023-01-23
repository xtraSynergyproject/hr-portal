
using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class LeaveDetailViewModel : ViewModelBase
    {

        public long? ServiceId { get; set; }
        public long UserId { get; set; }
        public long? PersonId { get; set; }
        public string TemplateAction { get; set; }
        public string ServiceNo { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string LeaveType { get; set; }
        public string LeaveTypeCode { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        public NtsActionEnum? LeaveStatusAction { get; set; }
        public AddDeductEnum? AddDeduct { get; set; }
        public double? Adjustment { get; set; }

        public DateTime? ReturnToWork { get; set; }
        [Display(Name = "Return To Work Request")]
        public long? ReturnToWorkServiceId { get; set; }
        public string ReturnServiceNo { get; set; }

        [Display(Name = "Handover Service")]
        public long? HandoverServiceId { get; set; }
        public string HandoverServiceNo { get; set; }

        [Display(Name = "Cancel Leave")]
        public long? CancelServiceId { get; set; }
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

        //[Display(Name = "Duration (Days)")]
        [Display(Name = "Calendar Days")]
        public string DurationText { get; set; }

        [Display(Name = "Working Days")]
        public double? WorkingDuration { get; set; }
        //    get
        //    {
        //        if (StartDate != null && EndDate != null)
        //        {
        //            DateTime sd = DateTime.Parse(StartDate.ToString());
        //            DateTime ed = DateTime.Parse(EndDate.ToString());
        //            return (ed - sd).Days + 1;
        //        }
        //        return 0;
        //    }

        //}
        //public decimal DurationHour
        //{
        //    get
        //    {
        //        if (StartDate != null && EndDate != null)
        //        {
        //            DateTime sd = DateTime.Parse(StartDate.ToString());
        //            DateTime ed = DateTime.Parse(EndDate.ToString());
        //            return (9 * (ed - sd).Days + 1);
        //        }
        //        return 0.0M;
        //    }

        //}

        [Display(Name = "Annual Leave Yearly Entitlement")]
        public double? YearlyEntitlement { get; set; }
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

        public string FromtoEnd { get { return FormattedStartDate + " - " + FormattedEndDate; } }

    }
}
