using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsTaskSequence : DataModelBase
    {
        public long NextId { get; set; }
        
        public DateTime SequenceDate { get; set; }
        public string TemplateId { get; set; }

    }
    [Table("NtsTaskSequenceLog", Schema = "log")]
    public class NtsTaskSequenceLog : NtsTaskSequence
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
