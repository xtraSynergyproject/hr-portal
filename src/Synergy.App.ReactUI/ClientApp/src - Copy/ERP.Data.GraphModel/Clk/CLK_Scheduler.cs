using ERP.Utility;
using System;
using System.Collections.Generic;

namespace ERP.Data.GraphModel
{
    public partial class CLK_Scheduler : NodeBase
    {
        public string Name { get; set; }
        public ScheduleTypeEnum ScheduleType { get; set; }
        public ClockServerSchedulerActionEnum SchedulerAction { get; set; }
 
        public TimeSpan? ScheduleTime { get; set; }
        public long? ScheduleOrder { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DelayInExecution { get; set; }
        public int ScheduleRecur { get; set; }
        public bool? EnableManualExecution { get; set; }

        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public string Param4 { get; set; }
        public string Param5 { get; set; }
        public string Param6 { get; set; }
        public string Param7 { get; set; }
        public string Param8 { get; set; }
        public string Param9 { get; set; }
        public string Param10 { get; set; }

        public DateTime? LastScheduleStartTime { get; set; }
        public DateTime? LastScheduleEndTime { get; set; }
        public DateTime? LastSuccessfulProcessTime { get; set; }

        public ScheduleExecutionStatusEnum ExecutionStatus { get; set; }
        public ScheduleExecutionStatusEnum LastScheduleStatus { get; set; }
    }
    /// <summary>
    /// One to many
    /// </summary>
    //public class R_Scheduler_Device : RelationshipBase
    //{

    //}

}
