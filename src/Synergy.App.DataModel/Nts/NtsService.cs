using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsService : DataModelBase
    {
        public string ServiceNo { get; set; }
        public string ServiceSubject { get; set; }
        public string ServiceDescription { get; set; }
        public string TemplateCode { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        //public string SLA { get; set; }

        public TimeSpan ServiceSLA { get; set; }
        public TimeSpan ActualSLA { get; set; }
        public DateTime? ReminderDate { get; set; }

        [ForeignKey("ServiceStatus")]
        public string ServiceStatusId { get; set; }
        public LOV ServiceStatus { get; set; }

        [ForeignKey("ServiceAction")]
        public string ServiceActionId { get; set; }
        public LOV ServiceAction { get; set; }

        [ForeignKey("ServicePriority")]
        public string ServicePriorityId { get; set; }
        public LOV ServicePriority { get; set; }

        public DateTime? SubmittedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? RejectedDate { get; set; }
        public DateTime? CanceledDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public DateTime? ReopenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string CloseComment { get; set; }
        public double? UserRating { get; set; }


        [ForeignKey("ServiceOwnerType")]
        public string ServiceOwnerTypeId { get; set; }
        public LOV ServiceOwnerType { get; set; }

        public string RejectionReason { get; set; }
        public string ReturnReason { get; set; }
        public string ReopenReason { get; set; }
        public string CancelReason { get; set; }
        public string CompleteReason { get; set; }
        public string DelegateReason { get; set; }


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


        [ForeignKey("ServiceTemplate")]
        public string ServiceTemplateId { get; set; }
        public ServiceTemplate ServiceTemplate { get; set; }


        [ForeignKey("RequestedByUser")]
        public string RequestedByUserId { get; set; }
        public User RequestedByUser { get; set; }

        [ForeignKey("OwnerUser")]
        public string OwnerUserId { get; set; }
        public User OwnerUser { get; set; }

        [ForeignKey("OwnerTeam")]
        public string OwnerTeamId { get; set; }
        public Team OwnerTeam { get; set; }


        [ForeignKey("LockStatus")]
        public string LockStatusId { get; set; }
        public LOV LockStatus { get; set; }

        public string ParentNoteId { get; set; }
        public string ParentTaskId { get; set; }
        [ForeignKey("ParentService")]
        public string ParentServiceId { get; set; }
        public NtsService ParentService { get; set; }
        public bool IsVersioning { get; set; }
        public string ReferenceId { get; set; }
        public ReferenceTypeEnum ReferenceType { get; set; }
        public bool IsArchived { get; set; }
        [ForeignKey("ServiceEvent")]
        public string ServiceEventId { get; set; }
        public LOV ServiceEvent { get; set; }

        public string ServicePlusId { get; set; }
        public string NotePlusId { get; set; }
        public string TaskPlusId { get; set; }
        public string WorkflowStatus { get; set; }
        public bool IsReopened { get; set; }
        public string NextStepTaskComponentId { get; set; }
        public bool DisableReopen { get; set; }
        public string TriggeredByReferenceId { get; set; }
        public ReferenceTypeEnum? TriggeredByReferenceType { get; set; }

    }
    [Table("NtsServiceLog", Schema = "log")]
    public class NtsServiceLog : NtsService
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
