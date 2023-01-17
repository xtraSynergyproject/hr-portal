using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NtsTaskAttachment : DataModelBase
    {
        [ForeignKey("NtsTask")]
        public string NtsTaskId { get; set; }
        public NtsTask NtsTask { get; set; }

        public string AttachmentId { get; set; }

    }
    [Table("NtsTaskAttachmentLog", Schema = "log")]
    public class NtsTaskAttachmentLog : NtsTaskAttachment
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
