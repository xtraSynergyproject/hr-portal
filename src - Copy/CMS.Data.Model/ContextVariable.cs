using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class ContextVariable : DataModelBase
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string FullyQualifiedName { get; set; }

        [ForeignKey("Parent")]
        public string ParentId { get; set; }
        public ContextVariable Parent { get; set; }
        public DataColumnTypeEnum DataType { get; set; }
    }
    [Table("ContextVariableLog", Schema = "log")]
    public class ContextVariableLog : ContextVariable
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
