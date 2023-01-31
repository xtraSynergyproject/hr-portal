namespace ERP.Data.GraphModel
{
    using ERP.Utility;
    using System;

    public partial class ADM_ScheduleMaster : NodeBase
    {        
        public string Name { get; set; }
        public string Description { get; set; }
        public ScheduleTypeEnum ScheduleType { get; set; }
        public ServiceSchedulerActionEnum? SchedulerAction { get; set; }

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

        public string LastReferenceNo { get; set; }
        public long? LastDataProcessed { get; set; }
        public long? LastDataSucceeded { get; set; }
        public bool? EnableArchive { get; set; }
    }
}
