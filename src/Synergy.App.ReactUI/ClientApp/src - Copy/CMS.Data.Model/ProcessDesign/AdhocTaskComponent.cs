using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class AdhocTaskComponent : DataModelBase
    {
        public TaskAssignedToTypeEnum AssigneeType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AssignedToUserId { get; set; }
        public string AssignedToTeamId { get; set; }
        public string RequestedByUserId { get; set; }
        public string OwnerUserId { get; set; }
        public string Subject { get; set; }
        public string TaskDescription { get; set; }

        public string ComponentId { get; set; }
        public Component Component { get; set; }

        public string NtsTaskId { get; set; }
        public NtsTask NtsTask { get; set; }
    }
    [Table("AdhocTaskComponentLog", Schema = "log")]
    public class AdhocTaskComponentLog : AdhocTaskComponent
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
        public DateTime LogEndDateTime { get; set; }
        public bool IsDatedLatest { get; set; }
        public bool IsVersionLatest { get; set; }
    }
}
