using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class TemplateCategory : DataModelBase
    {
        public TemplateTypeEnum TemplateType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public TemplateCategoryTypeEnum TemplateCategoryType { get; set; }

        [ForeignKey("Module")]
        public string ModuleId { get; set; }
        public Module Module { get; set; }
        public string IconFileId { get; set; }
        public string[] AllowedPortalIds { get; set; }
    }
    [Table("TemplateCategoryLog", Schema = "log")]
    public class TemplateCategoryLog : TemplateCategory
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
