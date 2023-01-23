using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class CompleteEventComponent : DataModelBase
    {
        [ForeignKey("Component")]
        public string ComponentId { get; set; }
        public Component Component { get; set; }
    }
    [Table("CompleteEventComponentLog", Schema = "log")]
    public class CompleteEventComponentLog : CompleteEventComponent
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
