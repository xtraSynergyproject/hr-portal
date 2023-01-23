using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class UdfPermission : DataModelBase
    {
        public string ColumnMetadataId { get; set; }

        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }

        public string[] EditableBy { get; set; }
        public string[] ViewableBy { get; set; }
        public string[] EditableContext { get; set; }
        public string[] ViewableContext { get; set; }

    
    }
    [Table("UdfPermissionLog", Schema = "log")]
    public class UdfPermissionLog : UdfPermission
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
