using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class AttendanceViewModel : ViewModelBase
    {

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime AttendanceDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? SearchDate { get; set; }
        public RosterDutyTypeEnum? AttendanceDutyType { get; set; }
        public bool Duty1Enabled { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty1StartTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty1EndTime { get; set; }
        public bool? Duty1FallsPreviousDay { get; set; }
        public bool Duty1FallsNextDay { get; set; }
        public bool Duty2Enabled { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty2StartTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty2EndTime { get; set; }
        public bool? Duty2FallsPreviousDay { get; set; }
        public bool Duty2FallsNextDay { get; set; }
        public bool Duty3Enabled { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty3StartTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty3EndTime { get; set; }
        public bool? Duty3FallsPreviousDay { get; set; }
        public bool Duty3FallsNextDay { get; set; }

        public TimeSpan? TotalHours { get; set; }

        public AttendanceTypeEnum? SystemAttendance { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? SystemOTHours { get; set; }
        [Display(Name = "System OT Hours")]
        public string SystemOTHoursText
        {
            get
            {
                return SystemOTHours.ToHHMMFormat();
            }
        }
        [Display(Name = "System OT Hours")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? PermittedOTHours { get; set; }
        public string PermittedOTHoursText
        {
            get
            {
                return PermittedOTHours.ToHHMMFormat();
            }
        }
        [Display(Name = "Calc. OT Hours")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? CalculatedOTHours { get; set; }
        public string CalculatedOTHoursText
        {
            get
            {
                return CalculatedOTHours.ToHHMMFormat();
            }
        }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? SystemDeductionHours { get; set; }


        [Display(Name = "System Deduction Hours")]
        public string SystemDeductionHoursText
        {
            get
            {
                return SystemDeductionHours.ToHHMMFormat();
            }
        }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? PermittedDeductionHours { get; set; }
        public string PermittedDeductionHoursText
        {
            get
            {
                return PermittedDeductionHours.ToHHMMFormat();
            }
        }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? CalculatedDeductionHours { get; set; }
        public string CalculatedDeductionHoursText
        {
            get
            {
                return CalculatedDeductionHours.ToHHMMFormat();
            }
        }
        public string SystemAttendanceText { get; set; }

        public EmployeeStatusEnum? EmployeeStatus { get; set; }


        public AttendanceTypeEnum? OverrideAttendance { get; set; }
        // [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? OverrideOTHours { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? OverrideDeductionHours { get; set; }
        [Display(Name = "Override OT Hours")]
        public string OverrideOTHoursText { get; set; }
        [Display(Name = "Override Deduction Hours")]
        public string OverrideDeductionHoursText { get; set; }
        public string OverrideAttendanceText { get; set; }
        public bool IsOverridden { get; set; }


        public bool Approved { get; set; }

        public string EmployeeNo { get; set; }
        public string BiometricId { get; set; }
        public string EmployeeName { get; set; }
        public string UserNameWithEmail { get; set; }
        public string Email { get; set; }
        public string IqamahNo { get; set; }
        public string DisplayName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        [Display(Name = "Job Title")]
        public string JobName { get; set; }
        public string GradeName { get; set; }
        public string Nationality { get; set; }

        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        [UIHint("TypeEditor1")]
        public long? SectionId { get; set; }
        [UIHint("TypeEditor1")]
        [Display(Name = "Section")]
        public string SectionName { get; set; }
        [Display(Name = "Sub Section")]
        public string SubSectionName { get; set; }
        public long? UserId { get; set; }
        public long? AssignmentId { get; set; }

        public string UserIds { get; set; }
        public string RosterDates { get; set; }

        public DateTime? Sunday { get; set; }
        public DateTime? Monday { get; set; }
        public DateTime? Tuesday { get; set; }
        public DateTime? Wednesday { get; set; }
        public DateTime? Thursday { get; set; }
        public DateTime? Friday { get; set; }
        public DateTime? Saturday { get; set; }

        [Display(Name = "Roster Total Hours")]
        public string RosterHours { get; set; }
        [Display(Name = "Actual Total Hours")]
        public string ActualHours { get; set; }
        public string RosterText { get; set; }
        public string ActualText { get; set; }

        public string OverrideComments { get; set; }

        public string EmployeeComments { get; set; }

        [Display(Name = "Contract End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ContractEndDate { get; set; }
        [Display(Name = "Contract Renewed")]
        public string ContractRenewable { get; set; }
        [Display(Name = "Sponsor")]
        public string Sponsor { get; set; }

        [Display(Name = "Month")]
        public int? SearchMonth { get; set; }
        public int? Year { get; set; }
        public DateTime? SearchStart { get; set; }
        public DateTime? SearchEnd { get; set; }
        public ScheduleTypeEnum? SearchType { get; set; }
        [Display(Name = "Payroll Group")]
        public long? PayrollGroupId { get; set; }

        public long? ServiceId { get; set; }

        public ApprovalStatusEnum? ApprovalStatus { get; set; }
        [Display(Name = "Employee")]
        public long? PersonId { get; set; }
        public TimeSpan? OTHours { get; set; }
        public TimeSpan? DeductionHours { get; set; }
        public double? BasicSalary { get; set; }
        public string NationalityCode { get; set; }
        public string AttendanceFlag { get; set; }
        public PayrollPostedStatusEnum? PayrollPostedStatus { get; set; }
        public DateTime PayrollPostedDate { get; set; }
        public bool IsCalculated { get; set; }
        public NodeEnum? ReferenceNode { get; set; }
        public long? ReferenceId { get; set; }

        public int? Different { get; set; }
        public int[] Columns { get; set; }
        public string SponsorCode { get; set; }
        public string LeaveTypeCode { get; set; }

        public string Mode { get; set; }
        public int ReportSerialNo { get; set; }



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
        [Display(Name = "Biometric Log")]
        public string AccessLogText { get; set; }

        public DateTime? RosterStartDate { get; set; }
        public DateTime? RosterEndDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? SearchFromDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? SearchToDate { get; set; }
        [Display(Name = "Attendance Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? AttDate { get; set; }

        public AttendanceLeaveTypeEnum? AttendanceLeaveType { get; set; }

        public TimeSpan? UnderTimeHours { get; set; }
        public TimeSpan? OverrideUnderTimeHours { get; set; }
        public string PersonNo { get; set; }
        public string SponsorshipNo { get; set; }

        [Display(Name = "Sign In")]
        public TimeSpan? TimeIn { get; set; }
        [Display(Name = "Sign Out")]
        public TimeSpan? TimeOut { get; set; }
        public string ShiftPatternName { get; set; }

    }
}
