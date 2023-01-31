using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("RecTaskTemplate", Schema = "public")]
    public class RecTaskTemplate : DataModelBase
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public string TemplateMasterId { get; set; }
        public string TemplateCode { get; set; }
        public string HeaderSectionText { get; set; }
        public string HeaderSectionMessage { get; set; }

        public string FieldSectionText { get; set; }
        public string FieldSectionMessage { get; set; }

        public string StepSectionText { get; set; }
        public string StepSectionMessage { get; set; }
        public bool CanAddStepTask { get; set; }
        public string StepTaskCreationOptionalLabel { get; set; }
        public string StepTaskAddButtonText { get; set; }
        public string StepTaskCancelButtonText { get; set; }
        public bool CanRemoveStepTask { get; set; }
        public string RemoveStepTaskButtonText { get; set; }
        public string RemoveStepTaskConfirmText { get; set; }
        public string RemoveStepTaskSuccessMessage { get; set; }



        public bool EnableAdhocTask { get; set; }
        public bool CanAddAdhocTask { get; set; }
        public string AdhocTaskHeaderText { get; set; }
        public string AdhocTaskAddButtonText { get; set; }
        public string AdhocTaskHeaderMessage { get; set; }






        public string DraftButtonText { get; set; }
        public string SaveButtonText { get; set; }
        public string CompleteButtonText { get; set; }
        public string RejectButtonText { get; set; }
        public string ReturnButtonText { get; set; }
        public string ReopenButtonText { get; set; }
        public string DelegateButtonText { get; set; }
        public string CancelButtonText { get; set; }
        public string BackButtonText { get; set; }
        public string CreateNewVersionButtonText { get; set; }
        public string SaveChangesButtonText { get; set; }

        public string SaveNewVersionButtonText { get; set; }

        public bool DraftButton { get; set; }
        public bool CreateNewVersionButton { get; set; }
        public bool SaveButton { get; set; }

        public bool CanViewVersions { get; set; }
        public bool? DisplayActionButtonBelow { get; set; }
        public bool SaveChangesButton { get; set; }

        public bool CompleteButton { get; set; }
        public bool IsCompleteReasonRequired { get; set; }
        public bool RejectButton { get; set; }
        public bool NotApplicableButton { get; set; }
        public bool IsRejectionReasonRequired { get; set; }
        public bool ReturnButton { get; set; }
        public bool? ReopenButton { get; set; }
        public bool? IsReopenReasonRequired { get; set; }
        public bool IsReturnReasonRequired { get; set; }
        public bool DelegateButton { get; set; }
        public bool IsDelegateReasonRequired { get; set; }
        public bool CancelButton { get; set; }
        public bool IsCancelReasonRequired { get; set; }
        public bool BackButton { get; set; }
        public bool CloseButton { get; set; }
        public string CloseButtonText { get; set; }
        public string SLA { get; set; }
        public string SLACalculationMode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? ReminderDaysPriorDueDate { get; set; }
        public bool IsSystemRating { get; set; }
        public bool IsConfidential { get; set; }
        public bool CollapseHeader { get; set; }
        public long? DuplicatedFromId { get; set; }
        public AssignToTypeEnum? AssignToType { get; set; }
        public AssignToTypeEnum? OwnerType { get; set; }
        public bool CanEditOwner { get; set; }
        public AssignedQueryTypeEnum? AssignedQueryType { get; set; }
        //public long AssignedTo { get; set; }
        public string AssignedToUserId { get; set; }
        public string AssignedToTeamId { get; set; }
        public string AssignedByQuery { get; set; }
        public int LayoutColumnCount { get; set; }
        public DocumentStatusEnum? DocumentStatus { get; set; }

        public bool DisableMessage { get; set; }

        public string NtsNoLabelName { get; set; }
        public bool? IsNtsNoManual { get; set; }
        public string SubjectLabelName { get; set; }
        public bool IsSubjectRequired { get; set; }



        public bool IsSubjectEditable { get; set; }



        public string DescriptionLabelName { get; set; }
        public bool IsDescriptionRequired { get; set; }
        public bool IsDescriptionEditable { get; set; }
        public string ClientValidationScript { get; set; }
        public string DocumentReadyScript { get; set; }
        public string ServerValidationScript { get; set; }
        public string LoadExecutionMethod { get; set; }
        public string PostSubmitExecutionMethod { get; set; }
        public string PostSubmitExecutionCode { get; set; }
        public bool? RunPostscriptInBackground { get; set; }
        public bool? DisableAutomaticDraft { get; set; }
        public string EditButtonValidationMethod { get; set; }
        public string PreSubmitExecutionMethod { get; set; }
        public string SaveChangesButtonVisibilityMethod { get; set; }
        public string EditButtonVisibilityMethod { get; set; }


        public string PrintButtonVisibilityMethod { get; set; }
        public bool CanViewServiceReference { get; set; }
        public string ServiceReferenceText { get; set; }
        public string NotificationUrlPattern { get; set; }
        public ModuleEnum ModuleName { get; set; }

        public string StatusLabelName { get; set; }
        public bool EnableTaskAutoComplete { get; set; }
        public bool EnableSLAChangeRequest { get; set; }


        public bool CanRemoveAdhocTask { get; set; }
        public string RemoveAdhocTaskButtonText { get; set; }
        public string RemoveAdhocTaskConfirmText { get; set; }
        public string RemoveAdhocTaskSuccessMessage { get; set; }
        public bool CanAddStepService { get; set; }
        public string StepServiceCreationOptionalLabel { get; set; }
        public string StepServiceAddButtonText { get; set; }
        public string StepServiceCancelButtonText { get; set; }
        public bool CanRemoveStepService { get; set; }
        public string RemoveStepServiceButtonText { get; set; }
        public string RemoveStepServiceConfirmText { get; set; }
        public string RemoveStepServiceSuccessMessage { get; set; }
        public bool EnableAdhocService { get; set; }
        public string AdhocServiceHeaderText { get; set; }
        public string AdhocServiceAddButtonText { get; set; }
        public string AdhocServiceHeaderMessage { get; set; }
        public bool CanRemoveAdhocService { get; set; }
        public string RemoveAdhocServiceButtonText { get; set; }
        public string RemoveAdhocServiceConfirmText { get; set; }
        public string RemoveAdhocServiceSuccessMessage { get; set; }

        public bool? HideDateAndSLA { get; set; }
        public bool? IsAttachmentRequired { get; set; }
        public bool? ChangeStatusOnStepChange { get; set; }
        public string ServiceOwnerText { get; set; }
        public bool? IncludeRequesterInOwnerList { get; set; }
        public bool? ServiceOwnerActAsStepTaskAssignee { get; set; }
        public bool? CreateInBackGround { get; set; }
        public bool? DisableStepTask { get; set; }
        public bool? AdminCanEditUdf { get; set; }
        public bool? AdminCanSubmitAndAutoComplete { get; set; }
        public string UdfIframeSrc { get; set; }
        public bool? EnableTeamAsOwner { get; set; }
        public bool? IsTeamAsOwnerMandatory { get; set; }
        public string Layout { get; set; }
        public string ReturnUrl { get; set; }

        public bool? HideSubject { get; set; }
        public bool? HideDescription { get; set; }


        public bool? EnablePrintButton { get; set; }
        public string PrintButtonText { get; set; }
        public string PrintMethodName { get; set; }
        public bool? EnableLock { get; set; }

        public bool? ReSubmitButton { get; set; }
        public string ReSubmitButtonText { get; set; }

        public bool? EnableBanner { get; set; }

        public bool? AllowTemplateChange { get; set; }

        public bool? EnableCode { get; set; }
        public bool? IsCodeRequired { get; set; }
        public bool? IsCodeUniqueInTemplate { get; set; }
        public bool? IsCodeEditable { get; set; }
        public string CodeLabelName { get; set; }


        public bool? EnableSequenceNo { get; set; }
        public bool? IsSequenceNoRequired { get; set; }
        public bool? IsSequenceNoUniqueInTemplate { get; set; }
        public bool? IsSequenceNoEditable { get; set; }
        public string SequenceNoLabelName { get; set; }


        public bool? DisableSharing { get; set; }
        public double? CompletionPercentage { get; set; }
        public string DefaultView { get; set; }

        public bool? EnableDocumentChangeRequest { get; set; }

        public string TemplateOwner { get; set; }

        public bool? EnableParent { get; set; }

        public string JsonForm { get; set; }

        public bool EnableIndexPage { get; set; }

        public bool EnableTaskNumberManual { get; set; }
        public bool EnableSaveAsDraft { get; set; }
        public string SaveAsDraftText { get; set; }
        public string SaveAsDraftCss { get; set; }

        public string SubmitButtonText { get; set; }
        public string SubmitButtonCss { get; set; }

        public string CompleteButtonCss { get; set; }

        public bool EnableRejectButton { get; set; }
        public string RejectButtonCss { get; set; }


        public bool EnableBackButton { get; set; }
        public string BackButtonCss { get; set; }
        public bool EnableAttachment { get; set; }
        public bool EnableComment { get; set; }

        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }

        [ForeignKey("TaskIndexPageTemplate")]
        public string TaskIndexPageTemplateId { get; set; }
        public TaskIndexPageTemplate TaskIndexPageTemplate { get; set; }

        public string TextBoxDisplay1 { get; set; }
        public string TextBoxLink1 { get; set; }
        public string TextBoxLink2 { get; set; }
        public string TextBoxLink3 { get; set; }
        public string TextBoxLink4 { get; set; }
        public string TextBoxLink5 { get; set; }
        public string TextBoxLink6 { get; set; }
        public string TextBoxLink7 { get; set; }
        public string TextBoxLink8 { get; set; }
        public string TextBoxLink9 { get; set; }
        public string TextBoxLink10 { get; set; }
        public NtsFieldType? TextBoxDisplayType1 { get; set; }
        public bool IsRequiredTextBoxDisplay1 { get; set; }
        public bool IsAssigneeAvailableTextBoxDisplay1 { get; set; }
        public string IsDropdownDisplay1 { get; set; }
        public string DropdownValueMethod1 { get; set; }
        public bool IsRequiredDropdownDisplay1 { get; set; }
        public bool IsAssigneeAvailableDropdownDisplay1 { get; set; }
        public string TextBoxDisplay2 { get; set; }
        public NtsFieldType? TextBoxDisplayType2 { get; set; }
        public bool IsRequiredTextBoxDisplay2 { get; set; }
        public bool IsAssigneeAvailableTextBoxDisplay2 { get; set; }
        public string IsDropdownDisplay2 { get; set; }
        public string DropdownValueMethod2 { get; set; }
        public bool IsRequiredDropdownDisplay2 { get; set; }
        public bool IsAssigneeAvailableDropdownDisplay2 { get; set; }
        public string TextBoxDisplay3 { get; set; }
        public NtsFieldType? TextBoxDisplayType3 { get; set; }
        public bool IsRequiredTextBoxDisplay3 { get; set; }
        public bool IsAssigneeAvailableTextBoxDisplay3 { get; set; }
        public string IsDropdownDisplay3 { get; set; }
        public string DropdownValueMethod3 { get; set; }
        public bool IsRequiredDropdownDisplay3 { get; set; }
        public bool IsAssigneeAvailableDropdownDisplay3 { get; set; }
        public string TextBoxDisplay4 { get; set; }
        public NtsFieldType? TextBoxDisplayType4 { get; set; }
        public bool IsRequiredTextBoxDisplay4 { get; set; }
        public bool IsAssigneeAvailableTextBoxDisplay4 { get; set; }
        public string IsDropdownDisplay4 { get; set; }
        public string DropdownValueMethod4 { get; set; }
        public bool IsRequiredDropdownDisplay4 { get; set; }
        public bool IsAssigneeAvailableDropdownDisplay4 { get; set; }
        public string TextBoxDisplay5 { get; set; }
        public NtsFieldType? TextBoxDisplayType5 { get; set; }
        public bool IsRequiredTextBoxDisplay5 { get; set; }
        public bool IsAssigneeAvailableTextBoxDisplay5 { get; set; }
        public string IsDropdownDisplay5 { get; set; }
        public string DropdownValueMethod5 { get; set; }
        public bool IsRequiredDropdownDisplay5 { get; set; }
        public bool IsAssigneeAvailableDropdownDisplay5 { get; set; }

        public string TextBoxDisplay6 { get; set; }
        public NtsFieldType? TextBoxDisplayType6 { get; set; }
        public bool IsRequiredTextBoxDisplay6 { get; set; }
        public bool IsAssigneeAvailableTextBoxDisplay6 { get; set; }
        public string IsDropdownDisplay6 { get; set; }
        public string DropdownValueMethod6 { get; set; }
        public bool IsRequiredDropdownDisplay6 { get; set; }
        public bool IsAssigneeAvailableDropdownDisplay6 { get; set; }
        public string TextBoxDisplay7 { get; set; }
        public NtsFieldType? TextBoxDisplayType7 { get; set; }
        public bool IsRequiredTextBoxDisplay7 { get; set; }
        public bool IsAssigneeAvailableTextBoxDisplay7 { get; set; }
        public string IsDropdownDisplay7 { get; set; }
        public string DropdownValueMethod7 { get; set; }
        public bool IsRequiredDropdownDisplay7 { get; set; }
        public bool IsAssigneeAvailableDropdownDisplay7 { get; set; }
        public string TextBoxDisplay8 { get; set; }
        public NtsFieldType? TextBoxDisplayType8 { get; set; }
        public bool IsRequiredTextBoxDisplay8 { get; set; }
        public bool IsAssigneeAvailableTextBoxDisplay8 { get; set; }
        public string IsDropdownDisplay8 { get; set; }
        public string DropdownValueMethod8 { get; set; }
        public bool IsRequiredDropdownDisplay8 { get; set; }
        public bool IsAssigneeAvailableDropdownDisplay8 { get; set; }
        public string TextBoxDisplay9 { get; set; }
        public NtsFieldType? TextBoxDisplayType9 { get; set; }
        public bool IsRequiredTextBoxDisplay9 { get; set; }
        public bool IsAssigneeAvailableTextBoxDisplay9 { get; set; }
        public string IsDropdownDisplay9 { get; set; }
        public string DropdownValueMethod9 { get; set; }
        public bool IsRequiredDropdownDisplay9 { get; set; }
        public bool IsAssigneeAvailableDropdownDisplay9 { get; set; }
        public string TextBoxDisplay10 { get; set; }
        public NtsFieldType? TextBoxDisplayType10 { get; set; }
        public bool IsRequiredTextBoxDisplay10 { get; set; }
        public bool IsAssigneeAvailableTextBoxDisplay10 { get; set; }
        public string IsDropdownDisplay10 { get; set; }
        public string DropdownValueMethod10 { get; set; }
        public bool IsRequiredDropdownDisplay10 { get; set; }
        public bool IsAssigneeAvailableDropdownDisplay10 { get; set; }

        public NtsTypeEnum? NtsType { get; set; }
        public string StepTemplateIds { get; set; }
        public string ServiceDetailsHeight { get; set; }
        public string ReturnTemplateName { get; set; }
        [ForeignKey("EmailSetting")]
        public string EmailSettingId { get; set; }
        public bool IsIncludeEmailAttachment { get; set; }
        public string BannerId { get; set; }
        public string BannerStyle { get; set; }
    }

}
