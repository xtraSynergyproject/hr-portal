using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class LOV : DataModelBase
    {
        public string LOVType { get; set; }
        public string Name { get; set; }       
        public string NameHindi { get; set; }
        public string NameArabic { get; set; }
        public string NameSpanish { get; set; }
        public string NameFrench { get; set; }
        public string Code { get; set; }
        public string GroupCode { get; set; }

        [ForeignKey("Parent")]
        public string ParentId { get; set; }
        public LOV Parent { get; set; }
        public string ImageId { get; set; }
        public string IconCss { get; set; }
        public string Description { get; set; }
        public string ReferenceId { get; set; }
        public ReferenceTypeEnum ReferenceType { get; set; }
    }
    [Table("LOVLog", Schema = "log")]
    public class LOVLog : LOV
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
