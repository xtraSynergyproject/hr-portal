using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{

    public class TAA_RosterDutyTemplate : NodeBase
    {
        public string Name { get; set; }
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
    }

    public class R_RosterDutyTemplate_OrganizationRoot : RelationshipDatedBase
    {
    }

}
