using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class ProcessDesignComponent : DataModelBase
    {
        public ProcessDesignComponentExecutionType ExecutionType { get; set; }

        [ForeignKey("Component")]
        public string ComponentId { get; set; }
        public Component Component { get; set; }


        [ForeignKey("ProcessDesign")]
        public string ProcessDesignId { get; set; }
        public ProcessDesign ProcessDesign { get; set; }
    }
    [Table("ProcessDesignComponentLog", Schema = "log")]
    public class ProcessDesignComponentLog : ProcessDesignComponent
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
