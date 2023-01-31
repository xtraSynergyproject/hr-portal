using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class Component : DataModelBase
    {
        public ProcessDesignComponentTypeEnum ComponentType { get; set; }
        //public string SourceId { get; set; }
        //public string TargetId { get; set; }

        public string Name { get; set; }
        public string ParentId { get; set; }

        [ForeignKey("ProcessDesign")]
        public string ProcessDesignId { get; set; }
        public ProcessDesign ProcessDesign { get; set; }
        public string CssClass { get; set; }


    }
    [Table("ComponentLog", Schema = "log")]
    public class ComponentLog : Component
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
