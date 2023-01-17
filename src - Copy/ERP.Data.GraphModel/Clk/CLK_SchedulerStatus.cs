using ERP.Utility;
using System;
using System.Collections.Generic;

namespace ERP.Data.GraphModel
{
    public partial class CLK_SchedulerStatus : NodeBase
    {
        public TimeSpan? ScheduleTime { get; set; }
        public DateTime? LastSuccessfulScheduleTime { get; set; }
        public DateTime? LastSuccessfulScheduleTimeLocal { get; set; }
        public DateTime ExecutionStartDate { get; set; }
        public DateTime? ExecutionEndDate { get; set; }
        public ScheduleExecutionStatusEnum ExecutionStatus { get; set; }
    }
    public class R_SchedulerStatus_Device : RelationshipBase
    {

    }
}
