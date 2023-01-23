using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsServiceSequence : DataModelBase
    {
        public long NextId { get; set; }
        
        public DateTime SequenceDate { get; set; }
        public string TemplateId { get; set; }

    }
    [Table("NtsServiceSequenceLog", Schema = "log")]
    public class NtsServiceSequenceLog : NtsServiceSequence
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
