using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class Template : DataModelBase
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        [ForeignKey("TemplateCategory")]
        public string TemplateCategoryId { get; set; }
        public TemplateCategory TemplateCategory { get; set; }
        public TemplateStatusEnum TemplateStatus { get; set; }

        public TemplateTypeEnum TemplateType { get; set; }

        [ForeignKey("TableMetadata")]
        public string TableMetadataId { get; set; }
        public TableMetadata TableMetadata { get; set; }
        public TableSelectionTypeEnum TableSelectionType { get; set; }

        [ForeignKey("UdfTemplate")]
        public string UdfTemplateId { get; set; }
        public Template UdfTemplate { get; set; }

        [ForeignKey("UdfTableMetadata")]
        public string UdfTableMetadataId { get; set; }
        public TableMetadata UdfTableMetadata { get; set; }

        public string Json { get; set; }
        public string PrintJson { get; set; }

        [ForeignKey("Module")]
        public string ModuleId { get; set; }
        public Module Module { get; set; }

        [ForeignKey("Domain")]
        public string DomainId { get; set; }
        public LOV Domain { get; set; }
        [ForeignKey("SubDomain")]
        public string SubDomainId { get; set; }
        public LOV SubDomain { get; set; }
        public string[] AllowedTagCategories { get; set; }

        [ForeignKey("TemplateStage")]
        public string TemplateStageId { get; set; }
        public TemplateStage TemplateStage { get; set; }

        [ForeignKey("TemplateStep")]
        public string TemplateStepId { get; set; }
        public TemplateStage TemplateStep { get; set; }
        [ForeignKey("WorkflowTemplate")]
        public string WorkFlowTemplateId { get; set; }
        public Template WorkflowTemplate { get; set; }
        public NtsViewTypeEnum? ViewType { get; set; }
        public NtsTypeEnum? NtsType { get; set; }
        public string OtherAttachmentId { get; set; }

    }
    [Table("TemplateLog", Schema = "log")]
    public class TemplateLog : Template
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
        public string GroupCode { get; set; }
    }
}
