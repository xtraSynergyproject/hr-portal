using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class ServiceTemplate : DataModelBase
    {
        public string Subject { get; set; }
        public string NotificationSubject { get; set; }
        public string Description { get; set; }
        public bool EnableIndexPage { get; set; }

        public bool EnableServiceNumberManual { get; set; }
        public bool EnableSaveAsDraft { get; set; }
        public string SaveAsDraftText { get; set; }
        public string SaveAsDraftCss { get; set; }

        public string SubmitButtonText { get; set; }
        public string SubmitButtonCss { get; set; }

        public bool EnableCompleteButton { get; set; }
        public string CompleteButtonText { get; set; }
        public string CompleteButtonCss { get; set; }




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


        [ForeignKey("ServiceIndexPageTemplate")]
        public string ServiceIndexPageTemplateId { get; set; }
        public ServiceIndexPageTemplate ServiceIndexPageTemplate { get; set; }

        public CreateReturnTypeEnum CreateReturnType { get; set; }
        public EditReturnTypeEnum EditReturnType { get; set; }
        public string PreScript { get; set; }
        public string PostScript { get; set; }

        public TimeSpan? SLA { get; set; }
        public bool AllowSLAChange { get; set; }

        [ForeignKey("Priority")]
        public string PriorityId { get; set; }
        public LOV Priority { get; set; }
        public string IconFileId { get; set; }
        public string TemplateColor { get; set; }
        public string BannerFileId { get; set; }
        public string BackgroundFileId { get; set; }



        public string SubjectText { get; set; }

        public string OwnerUserText { get; set; }
        public string RequestedByUserText { get; set; }

        public bool EnableCancelButton { get; set; }
        public string CancelButtonText { get; set; }
        public string CancelButtonCss { get; set; }
        public bool IsCancelReasonRequired { get; set; }

        public string ServiceNoText { get; set; }
        public string DescriptionText { get; set; }
        public bool HideHeader { get; set; }
        public bool HideSubject { get; set; }
        public bool HideDescription { get; set; }
        public bool HideServiceOwner { get; set; }
        public bool HideStartDate { get; set; }
        public bool HideExpiryDate { get; set; }
        public bool HideSLA { get; set; }
        public bool HidePriority { get; set; }
        public bool IsSubjectMandatory { get; set; }
        public bool IsSubjectUnique { get; set; }
        public bool IsDescriptionMandatory { get; set; }
        public bool HideToolbar { get; set; }
        public bool HideBanner { get; set; }



        public bool AllowPastStartDate { get; set; }

        public string[] AdhocTaskTemplateIds { get; set; }


        public bool EnablePrintButton { get; set; }
        public string PrintButtonText { get; set; }
        public bool EnableDataPermission { get; set; }

        [ForeignKey("ColumnMetadata")]
        public string DataPermissionColumnId { get; set; }
        public NumberGenerationTypeEnum NumberGenerationType { get; set; }
        public bool IsNumberNotMandatory { get; set; }
        public bool EnableLegalEntityFilter { get; set; }

        [ForeignKey("DefaultServiceOwnerType")]
        public string DefaultServiceOwnerTypeId { get; set; }
        public LOV DefaultServiceOwnerType { get; set; }

        [ForeignKey("DefaultOwnerUser")]
        public string DefaultOwnerUserId { get; set; }
        public User DefaultOwnerUser { get; set; }

        [ForeignKey("DefaultOwnerTeam")]
        public string DefaultOwnerTeamId { get; set; }
        public Team DefaultOwnerTeam { get; set; }

        [ForeignKey("DefaultServiceRequesterType")]
        public string DefaultServiceRequesterTypeId { get; set; }
        public LOV DefaultServiceRequesterType { get; set; }

        [ForeignKey("DefaultRequesterUser")]
        public string DefaultRequesterUserId { get; set; }
        public User DefaultRequesterUser { get; set; }

        [ForeignKey("DefaultRequesterTeam")]
        public string DefaultRequesterTeamId { get; set; }
        public Team DefaultRequesterTeam { get; set; }


        public bool HideOwner { get; set; }
        public bool EnablePermission { get; set; }
        public bool EnableInlineComment { get; set; }
        public bool EnableAdhocTask { get; set; }
        public string[] AdhocServiceTemplateIds { get; set; }
        public string[] AdhocNoteTemplateIds { get; set; }

        public ServiceTypeEnum? ServiceTemplateType { get; set; }

        public string EmailCopyTemplateCode { get; set; }
        public FormTypeEnum FormType { get; set; }
        public bool EnableDynamicStepTaskSelection { get; set; }
        public bool EnableSequenceOrder { get; set; }



        public bool DisableAutoCompleteIfNoStepTask { get; set; }
        public bool EnableCustomMessageOnCreation { get; set; }
        public string CustomMessageOnCreation { get; set; }
        public bool EnableCustomMessageOnEdit { get; set; }
        public string CustomMessageOnEdit { get; set; }
        public bool HideServiceNumberInHeader { get; set; }
        public bool HideStatusInHeader { get; set; }
        public bool HideVersionInHeader { get; set; }
        public bool HidePriorityInHeader { get; set; }
        public bool HideDueDateInHeader { get; set; }
        public bool HideNotificationInHeader { get; set; }
        public bool HideCommentInHeader { get; set; }
        public bool HideProcessDiagramInHeader { get; set; }
        public bool HideShareInHeader { get; set; }
        public bool HideAdhocTaskInHeader { get; set; }
        public bool HideAttachmentInHeader { get; set; }
        public bool HideTahsInHeader { get; set; }
        public bool HideEmailInHeader { get; set; }
        public bool HideWorkflowInHeader { get; set; }
        public bool HideLogInHeader { get; set; }
        public string SubjectMappingUdfId { get; set; }
        public string DescriptionMappingUdfId { get; set; }
        public ActionButtonPositionEnum ActionButtonPosition { get; set; }
        public string SubjectUdfMappingColumn { get; set; }
        public string DescriptionUdfMappingColumn { get; set; }

        public WorkflowVisibilityEnum WorkflowVisibility { get; set; }

        public bool EnableIntroPage { get; set; }
        public string IntroPageArea { get; set; }
        public string IntroPageController { get; set; }
        public string IntroPageAction { get; set; }
        public string IntroPageParams { get; set; }

        public bool EnablePreviewPage { get; set; }


        [ForeignKey("DisplayColumn")]
        public string DisplayColumnId { get; set; }
        public ColumnMetadata DisplayColumn { get; set; }

        public bool EnablePostSubmitPage { get; set; }
        public string PostSubmitPageArea { get; set; }
        public string PostSubmitPageController { get; set; }
        public string PostSubmitPageAction { get; set; }
        public string PostSubmitPageParams { get; set; }

        public bool EnablePreviewAcknowledgement { get; set; }
        public string PreviewAcknowledgementText { get; set; }


        public bool EnableFifo { get; set; }
        public string DraftWorkflowStatus { get; set; }
        public string InprogressWorkflowStatus { get; set; }
        public string OverdueWorkflowStatus { get; set; }
        public string CanceledWorkflowStatus { get; set; }
        public string RejectedWorkflowStatus { get; set; }
        public string ReturnedWorkflowStatus { get; set; }
        public string ClosedWorkflowStatus { get; set; }
        public string CompletedWorkflowStatus { get; set; }
        public string FormClientScript { get; set; }
        public string ParentServiceCodes { get; set; }
        public string RootServiceCode { get; set; }

        public bool EnableRuntimeWorkflow { get; set; }

        public string RuntimeAssigneeUserListUrl { get; set; }
        public string RuntimeAssigneeTeamListUrl { get; set; }
        public string RuntimeWorkflowButtonText { get; set; }
        public bool RuntimeWorkflowMandatory { get; set; }
        public bool DisableSubmitButton { get; set; }

    }
    [Table("ServiceTemplateLog", Schema = "log")]
    public class ServiceTemplateLog : ServiceTemplate
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
