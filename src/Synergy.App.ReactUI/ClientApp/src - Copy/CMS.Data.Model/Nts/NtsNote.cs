using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NtsNote : DataModelBase
    {
        public string NoteNo { get; set; }
        public string NoteSubject { get; set; }
        public string NoteDescription { get; set; }

        public string TemplateCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        [ForeignKey("NoteStatus")]
        public string NoteStatusId { get; set; }
        public LOV NoteStatus { get; set; }


        [ForeignKey("NotePriority")]
        public string NotePriorityId { get; set; }
        public LOV NotePriority { get; set; }


        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }


        [ForeignKey("NoteTemplate")]
        public string NoteTemplateId { get; set; }
        public NoteTemplate NoteTemplate { get; set; }

        [ForeignKey("NoteOwnerType")]
        public string NoteOwnerTypeId { get; set; }
        public LOV NoteOwnerType { get; set; }


        [ForeignKey("RequestedByUser")]
        public string RequestedByUserId { get; set; }
        public User RequestedByUser { get; set; }

        [ForeignKey("OwnerUser")]
        public string OwnerUserId { get; set; }
        public User OwnerUser { get; set; }


        [ForeignKey("ParentNote")]
        public string ParentNoteId { get; set; }
        public NtsNote ParentNote { get; set; }

        [ForeignKey("ParentTask")]
        public string ParentTaskId { get; set; }
        public NtsTask ParentTask { get; set; }

        [ForeignKey("ParentService")]
        public string ParentServiceId { get; set; }
        public NtsService ParentService { get; set; }
        [ForeignKey("NoteAction")]
        public string NoteActionId { get; set; }
        public LOV NoteAction { get; set; }
        public DateTime? CanceledDate { get; set; }
        public string CancelReason { get; set; }
        public string CompleteReason { get; set; }
        public double? UserRating { get; set; }
        public string CloseComment { get; set; }
        public DateTime? ClosedDate { get; set; }
        public bool IsVersioning { get; set; }
        public string ReferenceId { get; set; }
        public ReferenceTypeEnum ReferenceType { get; set; }
        public bool IsArchived { get; set; }
        public LockStatusEnum? LockStatus { get; set; }
        public DateTime? LastLockedDate { get; set; }

        public bool? DisablePermissionInheritance { get; set; }
        [ForeignKey("NoteEvent")]
        public string NoteEventId { get; set; }
        public LOV NoteEvent { get; set; }
        
        public string ServicePlusId { get; set; }
        public string NotePlusId { get; set; }
        public string TaskPlusId { get; set; }
    }
    [Table("NtsNoteLog", Schema = "log")]
    public class NtsNoteLog : NtsNote
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
