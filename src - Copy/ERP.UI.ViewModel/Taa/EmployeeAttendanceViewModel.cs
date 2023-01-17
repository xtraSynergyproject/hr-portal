using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class EmployeeAttendanceViewModel : ViewModelBase    {

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? AttendanceDate { get; set; }
        public DateTime? RosterDate { get; set; }

        public RosterDutyTypeEnum? AttendanceDutyType { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty1StartTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty1EndTime { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty2StartTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty2EndTime { get; set; }
 
      
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty3StartTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty3EndTime { get; set; }
  

        public TimeSpan? TotalHours { get; set; }

        public AttendanceTypeEnum? SystemAttendance { get; set; }
        public TimeSpan? SystemOTHours { get; set; }
        public string SystemOTHoursText { get; set; }


        public AttendanceTypeEnum? OverrideAttendance { get; set; }
        public TimeSpan? OverrideOTHours { get; set; }
        public string OverrideOTHoursText { get; set; }

        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        public long UserId { get; set; }    


        public string RosterHours { get; set; }
        public string ActualHours { get; set; }
        public string RosterText { get; set; }
        public string ActualText { get; set; }

        public string OverrideComments { get; set; }

        public string EmployeeComments { get; set; }

        public DateTime? SearchMonth { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? SearchStart { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? SearchEnd { get; set; }
        public ScheduleTypeEnum? SearchType { get; set; }

    }
}
