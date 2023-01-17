using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class OverrideAttendanceServiceViewModel : ViewModelBase    {

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? AttendanceDate { get; set; }
       
        public string WeekDateString { get; set; }

        [Display(Name = "From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? SearchStart { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "To")]
        public DateTime? SearchEnd { get; set; }

        public AttendanceTypeEnum? SystemAttendance { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        [Display(Name = "System OT Hours")]
        public string SystemOTHours { get; set; }
        [Display(Name = "System Deduction Hours")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public string SystemDeductionHours { get; set; }

        public AttendanceTypeEnum? OverrideAttendance { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public string OverrideOTHours { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public string OverrideDeductionHours { get; set; }
        [Display(Name = "Override OT Hours")]
        public string OverrideOTHoursText { get; set; }
        public string OverrideComments { get; set; }

        public bool Approved { get; set; }
        public string Requester { get; set; }
        public string EmployeeName { get; set; }
        public string Level1Assignee { get; set; }
        public string Level2Assignee { get; set; }
        public string Level3Assignee { get; set; }

        public string Level1Status { get; set; }
        public string Level2Status { get; set; }
        public string Level3Status { get; set; }

        public string Level1StatusCode { get; set; }
        public string Level2StatusCode { get; set; }
        public string Level3StatusCode { get; set; }

        public bool Level1Approve { get; set; }
        public bool Level2Approve { get; set; }
        public bool Level3Approve { get; set; }

        public long? Level1TaskId { get; set; }
        public long? Level2TaskId { get; set; }
        public long? Level3TaskId { get; set; }


        public string ServiceNo { get; set; }
        public long ServiceId { get; set; }
        public string ServiceStatus { get; set; }
        // public long TaskId { get; set; }
        public string ApprovalStatus { get; set; }
     
        public string RejectComments { get; set; }

        public string EmployeeComments { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        [Display(Name = "Week")]
        public string WeekDisplayName { get; set; }

    }
}
