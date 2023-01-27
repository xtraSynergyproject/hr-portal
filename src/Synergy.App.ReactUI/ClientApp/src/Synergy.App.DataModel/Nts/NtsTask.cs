using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsTask : DataModelBase
    {
        public string TaskNo { get; set; }
        public string TaskSubject { get; set; }
        public string TaskDescription { get; set; }
        public string TemplateCode { get; set; }

        public TaskTypeEnum TaskType { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public TimeSpan TaskSLA { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public TimeSpan ActualSLA { get; set; }
        public DateTime? ReminderDate { get; set; }
        public DateTime? PlanDate { get; set; }

        [ForeignKey("TaskStatus")]
        public string TaskStatusId { get; set; }
        public LOV TaskStatus { get; set; }

        [ForeignKey("TaskAction")]
        public string TaskActionId { get; set; }
        public LOV TaskAction { get; set; }

        [ForeignKey("TaskPriority")]
        public string TaskPriorityId { get; set; }
        public LOV TaskPriority { get; set; }

        public DateTime? SubmittedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? RejectedDate { get; set; }
        public DateTime? CanceledDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public DateTime? ReopenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }

        public string RejectionReason { get; set; }
        public string ReturnReason { get; set; }
        public string ReopenReason { get; set; }
        public string CancelReason { get; set; }
        public string CompleteReason { get; set; }
        public string DelegateReason { get; set; }
        public string CloseComment { get; set; }
        public double? UserRating { get; set; }


        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }

        [ForeignKey("UdfTemplate")]
        public string UdfTemplateId { get; set; }
        public Template UdfTemplate { get; set; }

        [ForeignKey("UdfNote")]
        public string UdfNoteId { get; set; }
        public NtsNote UdfNote { get; set; }
        public string UdfNoteTableId { get; set; }

        [ForeignKey("TaskTemplate")]
        public string TaskTemplateId { get; set; }
        public TaskTemplate TaskTemplate { get; set; }


        [ForeignKey("RequestedByUser")]
        public string RequestedByUserId { get; set; }
        public User RequestedByUser { get; set; }

        [ForeignKey("OwnerUser")]
        public string OwnerUserId { get; set; }
        public User OwnerUser { get; set; }

        [ForeignKey("TaskOwnerType")]
        public string TaskOwnerTypeId { get; set; }
        public LOV TaskOwnerType { get; set; }


        [ForeignKey("AssignedToType")]
        public string AssignedToTypeId { get; set; }
        public LOV AssignedToType { get; set; }

        [ForeignKey("AssignedToUser")]
        public string AssignedToUserId { get; set; }
        public User AssignedToUser { get; set; }



        [ForeignKey("AssignedToTeam")]
        public string AssignedToTeamId { get; set; }
        public Team AssignedToTeam { get; set; }
        public string AssignedToHierarchyMasterId { get; set; }
        public int? AssignedToHierarchyMasterLevelId { get; set; }


        public bool IsStepTaskAutoCompleteIfSameAssignee { get; set; }

        [ForeignKey("LockStatus")]
        public string LockStatusId { get; set; }
        public LOV LockStatus { get; set; }

        public string ParentNoteId { get; set; }
        public string ParentTaskId { get; set; }
        public string ParentServiceId { get; set; }

       
        public bool IsVersioning { get; set; }
        public string StepTaskComponentId { get; set; }
        public string ReferenceId { get; set; }
        public ReferenceTypeEnum ReferenceType { get; set; }
        public bool IsArchived { get; set; }
        [ForeignKey("TaskEvent")]
        public string TaskEventId { get; set; }
        public LOV TaskEvent { get; set; }
        public string ServicePlusId { get; set; }
        public string NotePlusId { get; set; }
        public string TaskPlusId { get; set; }



        [ForeignKey("NextTaskAssignedToType")]
        public string NextTaskAssignedToTypeId { get; set; }
        public LOV NextTaskAssignedToType { get; set; }

        [ForeignKey("NextTaskAssignedToUser")]
        public string NextTaskAssignedToUserId { get; set; }
        public User NextTaskAssignedToUser { get; set; }


        [ForeignKey("NextTaskAssignedToTeam")]
        public string NextTaskAssignedToTeamId { get; set; }
        public Team NextTaskAssignedToTeam { get; set; }

        public string NextTaskAssignedToHierarchyMasterId { get; set; }
        public int? NextTaskAssignedToHierarchyMasterLevelId { get; set; }
        public bool IsReturned { get; set; }
        public bool IsReopened { get; set; }
        public string NextTaskAttachmentId { get; set; }
        public string NextStepTaskComponentId { get; set; }
        public bool EnableFifo { get; set; }
        public string TriggeredByReferenceId { get; set; }
        public ReferenceTypeEnum? TriggeredByReferenceType { get; set; }
        public RuntimeWorkflowData RuntimeWorkflowData { get; set; }

        [ForeignKey("RuntimeWorkflowData")]
        public string RuntimeWorkflowDataId { get; set; }

    }
    [Table("NtsTaskLog", Schema = "log")]
    public class NtsTaskLog : NtsTask
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
