using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NtsTaskStatusTrack : DataModelBase
    {

        [ForeignKey("NtsTask")]
        public string NtsTaskId { get; set; }
        public NtsTask NtsTask { get; set; }

        [ForeignKey("TaskStatus")]
        public string TaskStatusId { get; set; }
        public LOV TaskStatus { get; set; }


        [ForeignKey("TaskAction")]
        public string TaskActionId { get; set; }
        public LOV TaskAction { get; set; }

        public DateTime StatusChangedDate { get; set; }
        public string StatusChangedByUserId { get; set; }

    }
    [Table("NtsTaskStatusTrackLog", Schema = "log")]
    public class NtsTaskStatusTrackLog : NtsTaskStatusTrack
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
