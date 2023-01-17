using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class RuntimeWorkflow : DataModelBase
    {
        [ForeignKey("RuntimeWorkflowSourceTemplate")]
        public string RuntimeWorkflowSourceTemplateId { get; set; }
        public Template RuntimeWorkflowSourceTemplate { get; set; }
        public string SourceServiceId { get; set; }
        public string SourceTaskId { get; set; }

        public WorkflowExecutionModeEnum RuntimeWorkflowExecutionMode { get; set; }




        [ForeignKey("TriggeringStepTaskComponent")]
        public string TriggeringStepTaskComponentId { get; set; }
        public StepTaskComponent TriggeringStepTaskComponent { get; set; }

        [ForeignKey("TriggeringTemplate")]
        public string TriggeringTemplateId { get; set; }
        public Template TriggeringTemplate { get; set; }

        [ForeignKey("TriggeringComponent")]
        public string TriggeringComponentId { get; set; }
        public Component TriggeringComponent { get; set; }

    }
    [Table("RuntimeWorkflowLog", Schema = "log")]
    public class RuntimeWorkflowLog : RuntimeWorkflow
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
