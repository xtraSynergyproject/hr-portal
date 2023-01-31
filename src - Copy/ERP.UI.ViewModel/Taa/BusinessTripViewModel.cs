
using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class BusinessTripViewModel : ViewModelBase
    {

        public long UserId { get; set; }
        public long? ServiceId { get; set; }
        public string ServiceNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? AppliedDate { get; set; }
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? BusinessTripStartDate { get; set; }
        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? BusinessTripEndDate { get; set; }
        public string BusinessTripLocation {get;set;}
        [Display(Name = "Purpose")]
        public string BusinessTripPurpose { get; set; }
        [Display(Name = "Business Trip Status")]
        public string BusinessTripStatus { get; set; }

        [Display(Name = "Business Trip Claim")]
        public long? ClaimServiceId { get; set; }
        public string ClaimServiceNo { get; set; }

        //public NtsActionEnum? LeaveStatusAction { get; set; }
        //public string LeaveType { get; set; }
        //public string LeaveTypeCode { get; set; }
        //public AddDeductEnum? AddDeduct { get; set; }
        //public double? Adjustment { get; set; }

        //public DateTime? ReturnToWork { get; set; }
        //[Display(Name = "Return To Work Request")]
        //public long? ReturnToWorkServiceId { get; set; }
        //public string ReturnServiceNo { get; set; }

        //[Display(Name = "Cancel Leave")]
        //public long? CancelServiceId { get; set; }
        //public string CancelServiceNo { get; set; }

        //[Display(Name = "Duration (Days)")]
        //public double? Duration { get; set; }
        //[Display(Name = "Duration (Days)")]
        //public string DurationText { get; set; }
        
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

        //[Display(Name = "Annual Leave Yearly Entitlement")]
        //public double? YearlyEntitlement { get; set; }
        //[Display(Name = "Annual Leave Balance As Of Today")]
        //public double? LeaveBalance { get; set; }
        //[Display(Name = "Leave Status")]
        //public string LeaveStatus { get; set; }
        //public String FormattedStartDate
        //{

        //    get
        //    {
        //        DateTime startDate = StartDate ?? DateTime.Now.Date;
        //        if (startDate.Year == DateTime.Now.Year)
        //        {
        //            return startDate.ToString("MMM dd");
        //        }
        //        else
        //        {
        //            return startDate.ToString("MMM dd yyyy");
        //        }

        //    }

        //}

        //public String FormattedEndDate
        //{

        //    get
        //    {
        //        DateTime startDate = EndDate ?? DateTime.Now.Date;
        //        if (startDate.Year == DateTime.Now.Year)
        //        {
        //            return startDate.ToString("MMM dd");
        //        }
        //        else
        //        {
        //            return startDate.ToString("MMM dd yyyy");
        //        }

        //    }

        //}

        //public String FormattedAppliedDate
        //{

        //    get
        //    {
        //        DateTime startDate = AppliedDate ?? DateTime.Now.Date;
        //        if (startDate.Year == DateTime.Now.Year)
        //        {
        //            return startDate.ToString("MMM dd");
        //        }
        //        else
        //        {
        //            return startDate.ToString("MMM dd yyyy");
        //        }

        //    }

        //}
        //public String FormattedrtwDate
        //{

        //    get
        //    {
        //        DateTime startDate = ReturnToWork ?? DateTime.Now.Date;
        //        if (startDate.Year == DateTime.Now.Year)
        //        {
        //            return startDate.ToString("MMM dd");
        //        }
        //        else
        //        {
        //            return startDate.ToString("MMM dd yyyy");
        //        }

        //    }

        //}

        //public string FromtoEnd { get { return FormattedStartDate + " - " + FormattedEndDate; } }

    }
}
