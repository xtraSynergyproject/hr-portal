using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class TemplateStage : DataModelBase
    {
        public string Name { get; set; }
        public TemplateStageTypeEnum StageType { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ParentStageId { get; set; }
    }
    [Table("TemplateStageLog", Schema = "log")]
    public class TemplateStageLog : TemplateStage
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
