using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class ProcessDesignVariable : DataModelBase
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public DataColumnTypeEnum DataType { get; set; }
        public string Value { get; set; }


        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }


        [ForeignKey("ProcessDesign")]
        public string ProcessDesignId { get; set; }
        public ProcessDesign ProcessDesign { get; set; }


    }
    [Table("ProcessDesignVariableLog", Schema = "log")]
    public class ProcessDesignVariableLog : ProcessDesignVariable
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
