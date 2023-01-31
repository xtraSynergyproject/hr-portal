using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{

    public partial class TAA_Attendance : NodeBase
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
        public AttendanceLeaveTypeEnum? AttendanceLeaveType { get; set; }
        public TimeSpan? SystemOTHours { get; set; }
        public TimeSpan? SystemDeductionHours { get; set; }
        public TimeSpan? UnderTimeHours { get; set; }
        public TimeSpan? OverrideUnderTimeHours { get; set; }

        //public TimeSpan? OverrideTotalHours { get; set; }
        public AttendanceTypeEnum? OverrideAttendance { get; set; }
        public TimeSpan? OverrideOTHours { get; set; }
        public TimeSpan? OverrideDeductionHours { get; set; }
        public string OverrideComments { get; set; }

        public string EmployeeComments { get; set; }
        public bool IsOverridden { get; set; }
        public ApprovalStatusEnum? ApprovalStatus { get; set; }


        public bool Approved { get; set; }
        public bool IsCalculated { get; set; }
        public PayrollPostedStatusEnum? PayrollPostedStatus { get; set; }
        public DateTime PayrollPostedDate { get; set; }
        public NodeEnum? ReferenceNode { get; set; }
        public long? ReferenceId { get; set; }

    }
    public class R_Attendance_User : RelationshipBase
    {

    }

}
