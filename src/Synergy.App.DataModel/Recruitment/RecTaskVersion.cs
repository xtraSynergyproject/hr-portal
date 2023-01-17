using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("RecTaskVersion", Schema = "public")]
    public class RecTaskVersion : DataModelBase
    {
        public string TaskNo { get; set; }
        public string TemplateCode { get; set; }
        public string ReferenceMasterId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string SLA { get; set; }

        [ForeignKey("ListOfValue")]
        public string TaskStatus { get; set; }
        public ListOfValue ListOfValue { get; set; }

        public string TaskStatusCode { get; set; }
        public string TaskStatusName { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? DelegatedDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? RejectedDate { get; set; }
        public DateTime? CanceledDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public DateTime? ReopenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string RatingComment { get; set; }
        public AssignToTypeEnum? AssignToType { get; set; }
        public string RejectionReason { get; set; }
        public string ReturnReason { get; set; }
        public string ReopenReason { get; set; }
        public string CancelReason { get; set; }
        public string CompleteReason { get; set; }
        public string DelegateReason { get; set; }
        // [ForeignKey("TaskTemplate")]
        public string TemplateId { get; set; }
        // public TaskTemplate TaskTemplate { get; set; }

        [ForeignKey("User")]
        public string OwnerUserId { get; set; }
        public User OwnerUser { get; set; }

        [ForeignKey("AssgineeUser")]
        public string AssigneeUserId { get; set; }
        public User AssgineeUser { get; set; }

        [ForeignKey("RequestedUser")]
        public string RequestedUserId { get; set; }
        public User RequestedUser { get; set; }

        [ForeignKey("Team")]
        public string AssigneeTeamId { get; set; }
        public Team Team { get; set; }
        public bool IsTaskAutoComplete { get; set; }

        public bool IsAssignedInTemplate { get; set; }
        //public decimal? SequenceNo { get; set; }
        public ReferenceTypeEnum? ReferenceTypeCode { get; set; }
        public string ReferenceTypeId { get; set; }
        public long? CompletedByUserId { get; set; }
        public long? RejectedByUserId { get; set; }

        public bool IsTaskAutoCompleteIfSameAssignee { get; set; }
        public NtsLockStatusEnum? LockStatus { get; set; }


        public long? PlanOrder { get; set; }

        public bool? IsRead { get; set; }
        public string TextValue1 { get; set; }
        public string TextDisplay1 { get; set; }
        public string DropdownValue1 { get; set; }
        public string DropdownDisplay1 { get; set; }


        public string TextValue2 { get; set; }
        public string TextDisplay2 { get; set; }
        public string DropdownValue2 { get; set; }
        public string DropdownDisplay2 { get; set; }


        public string TextValue3 { get; set; }
        public string TextDisplay3 { get; set; }
        public string DropdownValue3 { get; set; }
        public string DropdownDisplay3 { get; set; }


        public string TextValue4 { get; set; }
        public string TextDisplay4 { get; set; }
        public string DropdownValue4 { get; set; }
        public string DropdownDisplay4 { get; set; }


        public string TextValue5 { get; set; }
        public string TextDisplay5 { get; set; }
        public string DropdownValue5 { get; set; }
        public string DropdownDisplay5 { get; set; }

        public string TextValue6 { get; set; }
        public string TextDisplay6 { get; set; }
        public string DropdownValue6 { get; set; }
        public string DropdownDisplay6 { get; set; }


        public string TextValue7 { get; set; }
        public string TextDisplay7 { get; set; }
        public string DropdownValue7 { get; set; }
        public string DropdownDisplay7 { get; set; }


        public string TextValue8 { get; set; }
        public string TextDisplay8 { get; set; }
        public string DropdownValue8 { get; set; }
        public string DropdownDisplay8 { get; set; }


        public string TextValue9 { get; set; }
        public string TextDisplay9 { get; set; }
        public string DropdownValue9 { get; set; }
        public string DropdownDisplay9 { get; set; }


        public string TextValue10 { get; set; }
        public string TextDisplay10 { get; set; }
        public string DropdownValue10 { get; set; }
        public string DropdownDisplay10 { get; set; }


        public NtsTypeEnum? NtsType { get; set; }
        public string ParentVersionNo { get; set; }

        public DateTime? DatePickerValue1 { get; set; }
        public string AttachmentValue1 { get; set; }
        public string AttachmentCode1 { get; set; }

        public DateTime? DatePickerValue2 { get; set; }
        public string AttachmentValue2 { get; set; }
        public string AttachmentCode2 { get; set; }

        public DateTime? DatePickerValue3 { get; set; }
        public string AttachmentValue3 { get; set; }
        public string AttachmentCode3 { get; set; }


        public DateTime? DatePickerValue4 { get; set; }
        public string AttachmentValue4 { get; set; }
        public string AttachmentCode4 { get; set; }

        public DateTime? DatePickerValue5 { get; set; }
        public string AttachmentValue5 { get; set; }
        public string AttachmentCode5 { get; set; }

        public DateTime? DatePickerValue6 { get; set; }
        public string AttachmentValue6 { get; set; }
        public string AttachmentCode6 { get; set; }

        public DateTime? DatePickerValue7 { get; set; }
        public string AttachmentValue7 { get; set; }
        public string AttachmentCode7 { get; set; }

        public DateTime? DatePickerValue8 { get; set; }
        public string AttachmentValue8 { get; set; }
        public string AttachmentCode8 { get; set; }

        public DateTime? DatePickerValue9 { get; set; }
        public string AttachmentValue9 { get; set; }
        public string AttachmentCode9 { get; set; }

        public DateTime? DatePickerValue10 { get; set; }
        public string AttachmentValue10 { get; set; }
        public string AttachmentCode10 { get; set; }
    }

}
