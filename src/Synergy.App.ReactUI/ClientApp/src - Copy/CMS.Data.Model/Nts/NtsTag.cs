using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NtsTag : DataModelBase
    {
        public NtsTypeEnum NtsType { get; set; }
        public string NtsId { get; set; }
        public string TagCategoryId { get; set; }
        public string TagId { get; set; }
        public string TagSourceReferenceId { get; set; }
    }
    [Table("NtsTagLog", Schema = "log")]
    public class NtsTagLog : NtsTag
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
