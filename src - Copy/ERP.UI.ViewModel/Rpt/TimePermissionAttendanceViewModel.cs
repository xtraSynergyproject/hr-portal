using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class TimePermissionAttendanceViewModel
    {
        public string TemplateAction { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string IqhamahNo { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string JobName { get; set; }
        public string PermissionName { get; set; }
        public string PermissionCode { get; set; }
        public DateTime? SearchDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Status { get; set; }
        public string LeaveTypeCode { get; set; }
        public long ServiceId { get; set; }
        public string OtherInformation { get; set; }
        public string RosterHours { get; set; }
        public string ActualHours { get; set; }
        public string RosterText { get; set; }
        public string ActualText { get; set; }
        public string SignInType { get; set; }
        public DateTime? LogDate { get; set; }
        public string TimePermissionTypes { get; set; }
        public bool IsDeleted { get; set; }
        public AttendanceTypeEnum? SystemAttendance { get; set; }
        public long OrgId { get; set; }

        public long? UserId { get; set; }

        [Display(Name = "Service No.")]
        public string ServiceNo { get; set; }

        [Display(Name = "Hours")]
        public string Hours { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Date")]
        public DateTime? Date { get; set; }

        public string TemplateCode { get; set; }

        public TimePermissionType? TimePermissionType { get; set; }
        public string TimePermissionReason { get; set; }

        public string TimePermissionText
        {
            get { return TimePermissionReason == null ? (TimePermissionType == null ? "" : TimePermissionType.Description()) : TimePermissionReason; }
        }
        public string ServiceOwner { get; set; }
    }
}
