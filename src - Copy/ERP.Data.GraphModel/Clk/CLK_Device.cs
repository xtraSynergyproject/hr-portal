using ERP.Utility;
using System;
using System.Collections.Generic;

namespace ERP.Data.GraphModel
{
    public partial class CLK_Device : NodeBase
    {
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public int PortNo { get; set; }
        public int MachineNo { get; set; }
        public string SerialNo { get; set; }
        public CommunicationTypeEnum CommunicationType { get; set; }
        public PunchingTypeEnum PunchingType { get; set; }
        public bool EnableErrorNotification { get; set; }
        public string NotificiationRecipient { get; set; }

      //  public TimeSpan? ScheduleTime { get; set; }
        public DateTime? LastSuccessfulDownloadTime { get; set; }
        public DateTime? LastSuccessfulCleanupTime { get; set; }
        public DateTime ExecutionStartDate { get; set; }
        public DateTime? ExecutionEndDate { get; set; }
        public ScheduleExecutionStatusEnum? ExecutionStatus { get; set; }
        public string ExecutionError { get; set; }

        public TimeSpan? ProposedLogCleanupStartTime { get; set; }
        public TimeSpan? ProposedLogCleanupEndTime { get; set; }
        public bool? EnableLogCleanup { get; set; }
        public bool? ErrorNotificationSent { get; set; }
        public DeviceTypeEnum? DeviceType { get; set; }
    }
    public class R_Device_Product : RelationshipBase
    {

    }
    public class R_Device_TimeZone : RelationshipBase
    {

    }
}
