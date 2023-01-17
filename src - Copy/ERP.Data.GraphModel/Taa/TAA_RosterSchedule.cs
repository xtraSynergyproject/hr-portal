using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{

    public partial class TAA_RosterSchedule : NodeBase
    {
        public DateTime RosterDate { get; set; }
        public RosterDutyTypeEnum? RosterDutyType { get; set; }
        public bool Duty1Enabled { get; set; }
        public TimeSpan? Duty1StartTime { get; set; }
        public TimeSpan? Duty1EndTime { get; set; }
        public bool Duty1FallsNextDay { get; set; }
        public bool Duty2Enabled { get; set; }
        public TimeSpan? Duty2StartTime { get; set; }
        public TimeSpan? Duty2EndTime { get; set; }
        public bool Duty2FallsNextDay { get; set; }
        public bool Duty3Enabled { get; set; }
        public TimeSpan? Duty3StartTime { get; set; }
        public TimeSpan? Duty3EndTime { get; set; }
        public bool Duty3FallsNextDay { get; set; }
        public TimeSpan? TotalHours { get; set; }


        public RosterDutyTypeEnum? DraftRosterDutyType { get; set; }
        public bool? DraftDuty1Enabled { get; set; }
        public TimeSpan? DraftDuty1StartTime { get; set; }
        public TimeSpan? DraftDuty1EndTime { get; set; }
        public bool? DraftDuty1FallsNextDay { get; set; }
        public bool? DraftDuty2Enabled { get; set; }
        public TimeSpan? DraftDuty2StartTime { get; set; }
        public TimeSpan? DraftDuty2EndTime { get; set; }
        public bool? DraftDuty2FallsNextDay { get; set; }
        public bool? DraftDuty3Enabled { get; set; }
        public TimeSpan? DraftDuty3StartTime { get; set; }
        public TimeSpan? DraftDuty3EndTime { get; set; }
        public bool? DraftDuty3FallsNextDay { get; set; }
        public TimeSpan? DraftTotalHours { get; set; }

        public DocumentStatusEnum? PublishStatus { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool? IsDraft { get; set; }
        public bool? IsAttendanceCalculated { get; set; }
        public string ShiftPatternName { get; set; }
    }
    public class R_RosterSchedule_User : RelationshipBase
    {

    }

}
