using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{

    public class TaskTemplate : DataModelBase
    {
        public string Subject { get; set; }
        public string NotificationSubject { get; set; }
        public string Description { get; set; }
        public bool EnableIndexPage { get; set; }

        public bool EnableTaskNumberManual { get; set; }
        public bool EnableSaveAsDraft { get; set; }
        public string SaveAsDraftText { get; set; }
        public string SaveAsDraftCss { get; set; }

        public string SubmitButtonText { get; set; }
        public string SubmitButtonCss { get; set; }

        public TimeSpan? SLA { get; set; }
        public bool AllowSLAChange { get; set; }

        public string CompleteButtonText { get; set; }
        public bool IsCompleteReasonRequired { get; set; }
        public string CompleteButtonCss { get; set; }
        [ForeignKey("Priority")]
        public string PriorityId { get; set; }
        public LOV Priority { get; set; }

        public bool EnableRejectButton { get; set; }
        public bool IsRejectReasonRequired { get; set; }
        public string RejectButtonText { get; set; }
        public string RejectButtonCss { get; set; }


        public bool EnableBackButton { get; set; }
        public string BackButtonText { get; set; }
        public string BackButtonCss { get; set; }
        public bool EnableAttachment { get; set; }
        public bool EnableComment { get; set; }

        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }


        [ForeignKey("UdfTemplate")]
        public string UdfTemplateId { get; set; }
        public Template UdfTemplate { get; set; }

        [ForeignKey("TaskIndexPageTemplate")]
        public string TaskIndexPageTemplateId { get; set; }
        public TaskIndexPageTemplate TaskIndexPageTemplate { get; set; }
        public CreateReturnTypeEnum CreateReturnType { get; set; }
        public EditReturnTypeEnum EditReturnType { get; set; }
        public string PreScript { get; set; }
        public string PostScript { get; set; }

        public string IconFileId { get; set; }
        public string TemplateColor { get; set; }
        public string BannerFileId { get; set; }
        public string BackgroundFileId { get; set; }


        public string SubjectText { get; set; }


        public string OwnerUserText { get; set; }
        public string RequestedByUserText { get; set; }
        public string AssignedToUserText { get; set; }

        public bool EnableCancelButton { get; set; }
        public bool IsCancelReasonRequired { get; set; }
        public string CancelButtonText { get; set; }
        public string CancelButtonCss { get; set; }

        public bool EnableTimeEntry { get; set; }
        public TaskTypeEnum TaskTemplateType { get; set; }

        public string TaskNoText { get; set; }
        public string DescriptionText { get; set; }
        public bool HideHeader { get; set; }
        public bool HideSubject { get; set; }
        public bool HideDescription { get; set; }
        public bool HideStartDate { get; set; }
        public bool HideDueDate { get; set; }
        public bool HideSLA { get; set; }
        public bool HidePriority { get; set; }
        public bool IsSubjectMandatory { get; set; }
        public bool IsSubjectUnique { get; set; }
        public bool IsDescriptionMandatory { get; set; }
        public bool HideToolbar { get; set; }
        public bool HideBanner { get; set; }
        public bool HideOwner { get; set; }
        public bool AllowPastStartDate { get; set; }
        public bool EnablePrintButton { get; set; }
        public string PrintButtonText { get; set; }
        public bool EnableEmail { get; set; }
        public bool EnableDataPermission { get; set; }
        [ForeignKey("ColumnMetadata")]
        public string DataPermissionColumnId { get; set; }
        public NumberGenerationTypeEnum NumberGenerationType { get; set; }
        public bool IsNumberNotMandatory { get; set; }
        public bool EnableLegalEntityFilter { get; set; }

        public bool EnablePermission { get; set; }
        public bool EnableInlineComment { get; set; }
        public string EmailCopyTemplateCode { get; set; }
        public FormTypeEnum FormType { get; set; }
        public bool EnableSequenceOrder { get; set; }
        public bool EnableDirectEmailAssignee { get; set; }

        public bool EnableReOpenButton { get; set; }

        [ForeignKey("TaskAssignedToType")]
        public string TaskAssignedToTypeId { get; set; }
        public LOV TaskAssignedToType { get; set; }


        public bool EnableCustomMessageOnCreation { get; set; }
        public string CustomMessageOnCreation { get; set; }
        public bool EnableCustomMessageOnEdit { get; set; }
        public string CustomMessageOnEdit { get; set; }


        public bool HideTaskNumberInHeader { get; set; }
        public bool HideStatusInHeader { get; set; }
        public bool HideVersionInHeader { get; set; }
        public bool HidePriorityInHeader { get; set; }
        public bool HideDueDateInHeader { get; set; }
        public bool HideNotificationInHeader { get; set; }
        public bool HideCommentInHeader { get; set; }
        public bool HideProcessDiagramInHeader { get; set; }
        public bool HideShareInHeader { get; set; }
        public bool HideAttachmentInHeader { get; set; }
        public bool HideTahsInHeader { get; set; }
        public bool HideEmailInHeader { get; set; }
        public bool HideWorkflowInHeader { get; set; }
        public bool HideLogInHeader { get; set; }
        public string SubjectMappingUdfId { get; set; }
        public string DescriptionMappingUdfId { get; set; }
        public bool EnablePlanning { get; set; }
        public ActionButtonPositionEnum ActionButtonPosition { get; set; }

        public string SubjectUdfMappingColumn { get; set; }
        public string DescriptionUdfMappingColumn { get; set; }
        [ForeignKey("LocalizedColumn")]
        public string LocalizedColumnId { get; set; }
        public ColumnMetadata LocalizedColumn { get; set; }

        [ForeignKey("DisplayColumn")]
        public string DisplayColumnId { get; set; }
        public ColumnMetadata DisplayColumn { get; set; }
        public string FormClientScript { get; set; }
    }

    [Table("TaskTemplateLog", Schema = "log")]
    public class TaskTemplateLog : TaskTemplate
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
