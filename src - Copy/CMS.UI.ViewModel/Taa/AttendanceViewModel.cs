using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class AttendanceViewModel : ViewModelBase
    {

        public DateTime AttendanceDate { get; set; }
        public bool Duty1Enabled { get; set; }
        public TimeSpan? Duty1StartTime { get; set; }
        public TimeSpan? Duty1EndTime { get; set; }
        public bool? Duty1FallsPreviousDay { get; set; }
        public bool Duty1FallsNextDay { get; set; }
        public bool Duty2Enabled { get; set; }
        public TimeSpan? Duty2StartTime { get; set; }
        public TimeSpan? Duty2EndTime { get; set; }
        public bool? Duty2FallsPreviousDay { get; set; }
        public bool Duty2FallsNextDay { get; set; }
        public bool Duty3Enabled { get; set; }
        public TimeSpan? Duty3StartTime { get; set; }
        public TimeSpan? Duty3EndTime { get; set; }
        public bool? Duty3FallsPreviousDay { get; set; }
        public bool Duty3FallsNextDay { get; set; }

        public TimeSpan? TotalHours { get; set; }

        public AttendanceTypeEnum? SystemAttendance { get; set; }
        public string SystemAttendanceId { get; set; }
        public AttendanceLeaveTypeEnum? AttendanceLeaveType { get; set; }
        public string AttendanceTypeId { get; set; }
        public string AttendanceLeaveTypeId { get; set; }
        public TimeSpan? SystemOTHours { get; set; }
        public TimeSpan? SystemDeductionHours { get; set; }
        public TimeSpan? UnderTimeHours { get; set; }
        public TimeSpan? OverrideUnderTimeHours { get; set; }

        //public TimeSpan? OverrideTotalHours { get; set; }
        public AttendanceTypeEnum? OverrideAttendance { get; set; }
        public string OverrideAttendanceId { get; set; }
        public TimeSpan? OverrideOTHours { get; set; }
        public TimeSpan? OverrideDeductionHours { get; set; }
        public string OverrideComments { get; set; }

        public string EmployeeComments { get; set; }
        public bool IsOverridden { get; set; }
        //public ApprovalStatusEnum? ApprovalStatus { get; set; }


        public bool Approved { get; set; }
        public bool IsCalculated { get; set; }
        public PayrollPostedStatusEnum? PayrollPostedStatus { get; set; }
        public string PayrollPostedStatusId { get; set; }
        public DateTime PayrollPostedDate { get; set; }
        public NodeEnum? ReferenceNode { get; set; }
        public long? ReferenceId { get; set; }
        public string UserId { get; set; }
        public string NtsNoteId { get; set; }
        public string ServiceId { get; set; }
        public DateTime? SearchDate { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string EmployeeName { get; set; }
        public string PersonId { get; set; }
        public string PersonNo { get; set; }
        [Display(Name = "Job Title")]
        public string JobName { get; set; }

        [Display(Name = "Biometric Log")]
        public string AccessLogText { get; set; }
        public string ActualText { get; set; }
        [Display(Name = "Actual Total Hours")]
        public string ActualHours { get; set; }
        [Display(Name = "System OT Hours")]
        public string SystemOTHoursText
        {
            get
            {
                return SystemOTHours.ToTimeSpanString();
            }
        }
        [Display(Name = "System Deduction Hours")]
        public string SystemDeductionHoursText
        {
            get
            {
                return SystemDeductionHours.ToTimeSpanString();
            }
        }
        public TimeSpan? OTHours { get; set; }
        public TimeSpan? DeductionHours { get; set; }
        [Display(Name = "Override OT Hours")]
        public string OverrideOTHoursText { get; set; }
        [Display(Name = "Override Deduction Hours")]
        public string OverrideDeductionHoursText { get; set; }
        public string ReturnUrl { get; set; }
        public string UserIds { get; set; }
        public string Mode { get; set; }
        [Display(Name = "Payroll Group")]
        public string PayrollGroupId { get; set; }
        [Display(Name = "Month")]
        public int? SearchMonth { get; set; }
        public int? Year { get; set; }
        public DateTime? SearchStart { get; set; }
        public DateTime? SearchEnd { get; set; }
        public int? Different { get; set; }
        public int[] Columns { get; set; }
        public string RosterText { get; set; }
        [Display(Name = "Roster Total Hours")]
        public string RosterHours { get; set; }
        public bool? RosterDuty1Enabled { get; set; }
        public TimeSpan? RosterDuty1StartTime { get; set; }
        public TimeSpan? RosterDuty1EndTime { get; set; }
        public bool? RosterDuty1FallsNextDay { get; set; }
        public bool? RosterDuty2Enabled { get; set; }
        public TimeSpan? RosterDuty2StartTime { get; set; }
        public TimeSpan? RosterDuty2EndTime { get; set; }
        public bool? RosterDuty2FallsNextDay { get; set; }
        public bool? RosterDuty3Enabled { get; set; }
        public TimeSpan? RosterDuty3StartTime { get; set; }
        public TimeSpan? RosterDuty3EndTime { get; set; }
        public bool? RosterDuty3FallsNextDay { get; set; }
        public DateTime? RosterStartDate { get; set; }
        public DateTime? RosterEndDate { get; set; }

        public string SponsorCode { get; set; }
        public string NationalityCode { get; set; }
        public string AttendanceFlag { get; set; }
        public double? BasicSalary { get; set; }
        public string Message { get; set; }
        
        public DateTime SearchFromDate { get; set; }
        public DateTime SearchToDate { get; set; }

        public EmployeeStatusEnum? EmployeeStatus { get; set; }

        public string SponsorshipNo { get; set; }
        public string AttDate { get; set; }
        public string SystemAttendanceText { get; set; }

        public TimeSpan PermittedOTHoursText { get; set; }
        public TimeSpan CalculatedOTHoursText { get; set; }

        [Display(Name = "System OT Hours")]
        public TimeSpan? PermittedOTHours { get; set; }

        [Display(Name = "Calc. OT Hours")]
        public TimeSpan? CalculatedOTHours { get; set; }

        public TimeSpan PermittedDeductionHoursText { get; set; }

        public TimeSpan? CalculatedDeductionHoursText { get; set; }

        public string LeaveTypeCode { get;set; }
        public TimeSpan? PermittedDeductionHours { get; set; }


        public TimeSpan? CalculatedDeductionHours { get; set; }

    }
}
