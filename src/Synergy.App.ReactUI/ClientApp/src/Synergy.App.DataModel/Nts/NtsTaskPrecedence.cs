using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsTaskPrecedence : DataModelBase
    {
        [ForeignKey("Task")]
        public string NtsTaskId { get; set; }
        public NtsTask NtsTask { get; set; }
        public string PredecessorId { get; set; }
        public NtsTypeEnum PredecessorType { get; set; }
        public PrecedenceRelationshipTypeEnum PrecedenceRelationshipType { get; set; }

    }
    [Table("NtsTaskPrecedenceLog", Schema = "log")]
    public class NtsTaskPrecedenceLog : NtsTaskPrecedence
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
