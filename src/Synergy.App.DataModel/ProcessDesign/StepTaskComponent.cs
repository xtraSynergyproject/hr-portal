using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class StepTaskComponent : DataModelBase
    {

        public string Subject { get; set; }
        public string NotificationSubject { get; set; }
        public string Description { get; set; }

        [ForeignKey("TaskTemplate")]
        public string TaskTemplateId { get; set; }
        public TaskTemplate TaskTemplate { get; set; }


        /// <summary>
        /// Template Id of Task
        /// </summary>
        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }


        /// <summary>
        /// Corresponding Note Template Id
        /// </summary>
        [ForeignKey("UdfTemplate")]
        public string UdfTemplateId { get; set; }
        public Template UdfTemplate { get; set; }
        /// <summary>
        /// Corresponding Note Table Metadata Id
        /// </summary>

        [ForeignKey("UdfTableMetadata")]
        public string UdfTableMetadataId { get; set; }
        public TableMetadata UdfTableMetadata { get; set; }

        public TemplateCreateTypeEnum TemplateCreateType { get; set; }

        [ForeignKey("ReturnStepTask")]
        public string ReturnStepTaskId { get; set; }
        public TaskTemplate ReturnStepTask { get; set; }


        [ForeignKey("AssignedToType")]
        public string AssignedToTypeId { get; set; }
        public LOV AssignedToType { get; set; }

        [ForeignKey("AssignedToUser")]
        public string AssignedToUserId { get; set; }
        public User AssignedToUser { get; set; }

        [ForeignKey("AssignedToTeam")]
        public string AssignedToTeamId { get; set; }
        public WorkAssignmentTypeEnum TeamAssignmentType { get; set; }
        public Team AssignedToTeam { get; set; }

        [ForeignKey("AssignedToHierarchyMaster")]
        public string AssignedToHierarchyMasterId { get; set; }
        public HierarchyMaster AssignedToHierarchyMaster { get; set; }

        public int AssignedToHierarchyMasterLevelId { get; set; }

        [ForeignKey("Component")]
        public string ComponentId { get; set; }
        public Component Component { get; set; }

        public TimeSpan? SLA { get; set; }
        public bool AllowSLAChange { get; set; }

        [ForeignKey("Priority")]
        public string PriorityId { get; set; }
        public LOV Priority { get; set; }


        public string IconFileId { get; set; }
        public string BannerFileId { get; set; }
        public string BackgroundFileId { get; set; }


        public string SubjectText { get; set; }
        public string DescriptionText { get; set; }
        public string OwnerUserText { get; set; }
        public string RequestedByUserText { get; set; }
        public string AssignedToUserText { get; set; }
        public DateTime? StartDate { get; set; }
        public bool EnableChangingNextTaskAssignee { get; set; }
        public string ChangingNextTaskAssigneeTitle { get; set; }
        public bool EnableReturnTask { get; set; }
        public string ReturnTaskTitle { get; set; }
        public string ReturnTaskButtonText { get; set; }
        public bool EnableNextTaskAttachment { get; set; }
        public bool DisableNextTaskTeamChange { get; set; }
        public string WorkflowStatusName { get; set; }
        public bool EnableDynamicStepTaskSelection { get; set; }
        public bool EnableServiceComplete { get; set; }
        public bool EnablePlanning { get; set; }
        public WorkflowVisibilityEnum WorkflowVisibility { get; set; }
        public bool OpenSameTaskOnServiceReSubmit { get; set; }
        public bool EnableFifo { get; set; }
        public string DraftWorkflowStatus { get; set; }
        public string InprogressWorkflowStatus { get; set; }
        public string OverdueWorkflowStatus { get; set; }
        public string CanceledWorkflowStatus { get; set; }
        public string RejectedWorkflowStatus { get; set; }


        public bool IsRuntimeComponent { get; set; }
        public bool EnableRuntimeWorkflow { get; set; }
        public string RuntimeAssigneeUserListUrl { get; set; }
        public string RuntimeAssigneeTeamListUrl { get; set; }

        public string RuntimeWorkflowButtonText { get; set; }
        public bool RuntimeWorkflowMandatory { get; set; }
    }
    [Table("StepTaskComponentLog", Schema = "log")]
    public class StepTaskComponentLog : StepTaskComponent
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
