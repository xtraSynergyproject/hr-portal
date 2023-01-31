using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class StepTaskEscalation : DataModelBase
    {
        public string Name { get; set; }

        [ForeignKey("StepTaskComponent")]
        public string StepTaskComponentId { get; set; }
        public StepTaskComponent StepTaskComponent { get; set; }

        [ForeignKey("ParentStepTaskEscalation")]
        public string ParentStepTaskEscalationId { get; set; }
        public StepTaskEscalation ParentStepTaskEscalation { get; set; }
        public StepTaskEscalationTypeEnum StepTaskEscalationType { get; set; }
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

        [ForeignKey("NewPriority")]
        public string NewPriorityId { get; set; }
        public LOV NewPriority { get; set; }


        [ForeignKey("NotificationTemplate")]
        public string NotificationTemplateId { get; set; }
        public NotificationTemplate NotificationTemplate { get; set; }


        [ForeignKey("EscalatedToNotificationTemplate")]
        public string EscalatedToNotificationTemplateId { get; set; }
        public NotificationTemplate EscalatedToNotificationTemplate { get; set; }
        public int TriggerDaysAfterOverDue { get; set; }


    }
    [Table("StepTaskEscalationLog", Schema = "log")]
    public class StepTaskEscalationLog : StepTaskEscalation
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
