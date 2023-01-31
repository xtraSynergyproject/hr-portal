using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class StartEventComponent : DataModelBase
    {
        [ForeignKey("Component")]
        public string ComponentId { get; set; }
        public Component Component { get; set; }
        public PocecssExecutionTypeEnum ExecutionType { get; set; }
    }
    [Table("StartEventComponentLog", Schema = "log")]
    public class StartEventComponentLog : StartEventComponent
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
