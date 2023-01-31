using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NtsServicePrecedence : DataModelBase
    {
        [ForeignKey("Service")]
        public string ServiceId { get; set; }
        public NtsService Service { get; set; }
        public string PredecessorId { get; set; }
        public NtsTypeEnum PredecessorType { get; set; }
        public PrecedenceRelationshipTypeEnum PrecedenceRelationshipType { get; set; }

    }
    [Table("NtsServicePrecedenceLog", Schema = "log")]
    public class NtsServicePrecedenceLog : NtsServicePrecedence
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
