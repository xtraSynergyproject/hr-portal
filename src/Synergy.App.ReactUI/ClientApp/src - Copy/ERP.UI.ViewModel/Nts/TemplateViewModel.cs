using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    [Serializable]
    public class TemplateViewModel : DatedViewModelBase
    {

        //public long TemplateId { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateMasterCode { get; set; }
        public string LegalEntityCode { get; set; }

        public string TemplateMasterDescription { get; set; }
        public long TemplateMasterId { get; set; }
        public long? TemplateMasterCategoryId { get; set; }
        public string TemplateMasterCategoryCode { get; set; }
        [Display(Name = "TemplateMasterName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string TemplateMasterName { get; set; }
        [Display(Name = "TemplateMasterTemplateCategoryName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string TemplateMasterTemplateCategoryName { get; set; }
        [Display(Name = "TemplateMasterTemplateCategoryNtsType", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public NtsTypeEnum? TemplateMasterTemplateCategoryNtsType { get; set; }
        public string TemplateMasterLegalEntityCode { get; set; }
        public NtsClassificationEnum? TemplateMasterTemplateCategoryNtsClassification { get; set; }

        [Display(Name = "Name", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string Name { get; set; }
        public string Description { get; set; }


        [Display(Name = "HeaderSectionName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string HeaderSectionText { get; set; }
        [Display(Name = "Header Section Message")]
        public string HeaderSectionMessage { get; set; }

        [Display(Name = "FieldSectionName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string FieldSectionText { get; set; }
        [Display(Name = "Field Section Message")]
        public string FieldSectionMessage { get; set; }

        [Display(Name = "Draft Button Text")]
        public string DraftButtonText { get; set; }
        [Display(Name = "CancelButtonText", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string CancelButtonText { get; set; }
        [Display(Name = "BackButtonText", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string BackButtonText { get; set; }
        [Display(Name = "RejectButtonText", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string RejectButtonText { get; set; }
        [Display(Name = "CompleteButtonText", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string CompleteButtonText { get; set; }
        [Display(Name = "DelegateButtonText", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string DelegateButtonText { get; set; }
        [Display(Name = "Submit Button Text")]
        public string SaveButtonText { get; set; }
        [Display(Name = "DraftButton", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool DraftButton { get; set; }
        [Display(Name = "Submit Button")]
        public bool SaveButton { get; set; }
        [Display(Name = "CancelButton", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool CancelButton { get; set; }
        [Display(Name = "BackButton", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool BackButton { get; set; }
        [Display(Name = "RejectButton", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool RejectButton { get; set; }
        [Display(Name = "DelegateButton", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool DelegateButton { get; set; }
        [Display(Name = "CompleteButton", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool CompleteButton { get; set; }
        [Display(Name = "Return Button")]
        public bool ReturnButton { get; set; }
        [Display(Name = "Return Button Text")]
        public string ReturnButtonText { get; set; }
        [Display(Name = "Nts No Label Name")]
        public string NtsNoLabelName { get; set; }



        [Display(Name = "Not Applicable")]
        public bool NotApplicableButton { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DayTimeFormat)]
        public TimeSpan? SLA { get; set; }

        [Display(Name = "SLA", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string SLAText
        {
            get
            {
                if (SLA != null)
                {
                    //TimeSpan t = TimeSpan.Parse(SLA.ToString());
                    //return string.Format("{0}.{1}:{2}", t.Days,t.Hours, t.Minutes);
                    return SLA.Value.ToString(@"d\.hh\:mm");
                }
                return "";
            }

        }


        public int? SLADay { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? SLAHour { get; set; }

        [Display(Name = "Is Delegate Reason Required")]
        public bool IsDelegateReasonRequired { get; set; }
        [Display(Name = "Is Complete Reason Required")]
        public bool IsCompleteReasonRequired { get; set; }
        [Display(Name = "Is Rejection Reason Required")]
        public bool IsRejectionReasonRequired { get; set; }
        [Display(Name = "Is Return Reason Required")]
        public bool IsReturnReasonRequired { get; set; }
        [Display(Name = "Is Cancel Reason Required")]
        public bool IsCancelReasonRequired { get; set; }
        [Display(Name = "Is System Rating")]
        public bool IsSystemRating { get; set; }
        [Display(Name = "Is Confidential")]
        public bool IsConfidential { get; set; }
        [Display(Name = "Collapse Header")]
        public bool CollapseHeader { get; set; }
        [Display(Name = "Duplicated From Id")]
        public long? DuplicatedFromId { get; set; }
        [Display(Name = "Can Edit Owner")]
        public bool CanEditOwner { get; set; }
        [Display(Name = "Create New Version Button")]
        public bool CreateNewVersionButton { get; set; }

        [Display(Name = "OwnerType", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public AssignToTypeEnum? OwnerType { get; set; }
        [Display(Name = "OwnerId", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public long? OwnerId { get; set; }
        public long? OwnerUserId { get; set; }
        public long? TeamId { get; set; }

        [Display(Name = "AssignToType", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public AssignToTypeEnum? AssignToType { get; set; }
        [Display(Name = "AssignedQueryType", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public AssignedQueryTypeEnum? AssignedQueryType { get; set; }
        [Display(Name = "AssignedTo", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public long? AssignedTo { get; set; }
        [Display(Name = "AssignedByQuery", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string AssignedByQuery { get; set; }
        [Display(Name = "LayoutColumnCount", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public int LayoutColumnCount { get; set; }
        [Display(Name = "SequenceNo", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public long SequenceNo { get; set; }
        [Display(Name = "DocumentStatus", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public DocumentStatusEnum? DocumentStatus { get; set; }
        public long Count { get; set; }

        [Display(Name = "DisableMessage", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool DisableMessage { get; set; }

        [Display(Name = "SubjectLabelName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string SubjectLabelName { get; set; }
        [Display(Name = "IsSubjectRequired", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool IsSubjectRequired { get; set; }
        [Display(Name = "IsSubjectEditable", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool IsSubjectEditable { get; set; }
        [Display(Name = "DescriptionLabelName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string DescriptionLabelName { get; set; }
        [Display(Name = "IsDescriptionRequired", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool IsDescriptionRequired { get; set; }
        [Display(Name = "IsDescriptionEditable", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool IsDescriptionEditable { get; set; }

        [Display(Name = "ClientValidationScript", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string ClientValidationScript { get; set; }
        [Display(Name = "ServerValidationScript", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string ServerValidationScript { get; set; }
        [Display(Name = "PostSubmitExecutionMethod", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string PostSubmitExecutionMethod { get; set; }
        [Display(Name = "PreSubmitExecutionMethod", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string PreSubmitExecutionMethod { get; set; }


        [Display(Name = "CanViewServiceReference", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool CanViewServiceReference { get; set; }
        [Display(Name = "ServiceReferenceText", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string ServiceReferenceText { get; set; }

        [Display(Name = "NotificationUrlPattern", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string NotificationUrlPattern { get; set; }

        [Display(Name = "ModuleName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public ModuleEnum ModuleName { get; set; }

        [Display(Name = "StatusLabelName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string StatusLabelName { get; set; }
        [Display(Name = "Enable Task Auto Complete")]
        public bool EnableTaskAutoComplete { get; set; }
        [Display(Name = "Disable Step Task")]
        public bool? DisableStepTask { get; set; }
        [Display(Name = "Enable Adhoc Task")]
        public bool EnableAdhocTask { get; set; }
        [Display(Name = "Can Add Adhoc Task")]
        public bool CanAddAdhocTask { get; set; }
        [Display(Name = "AdhocTaskHeaderText", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string AdhocTaskHeaderText { get; set; }
        [Display(Name = "AdhocTaskAddButtonText", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string AdhocTaskAddButtonText { get; set; }
        [Display(Name = "AdhocTaskHeaderMessage", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string AdhocTaskHeaderMessage { get; set; }
        [Display(Name = "Close Button")]
        public bool CloseButton { get; set; }
        [Display(Name = "CloseButtonText", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string CloseButtonText { get; set; }

        [Display(Name = "Step Task Add Button Text")]
        public string StepTaskAddButtonText { get; set; }
        [Display(Name = "Step Task Cancel Button Text")]
        public string StepTaskCancelButtonText { get; set; }

        [Display(Name = "Save As New Version Button")]
        public bool SaveAsNewVersionButton { get; set; }
        [Display(Name = "Save As New Version Button Text")]
        public string SaveAsNewVersionButtonText { get; set; }
        [Display(Name = "Save Changes Button")]
        public bool SaveChangesButton { get; set; }
        [Display(Name = "Can View Versions")]
        public bool CanViewVersions { get; set; }
        [Display(Name = "Save Changes Button Text")]
        public string SaveChangesButtonText { get; set; }


        [Display(Name = "Step Section Text")]
        public string StepSectionText { get; set; }
        [Display(Name = "Step Section Message")]
        public string StepSectionMessage { get; set; }
        [Display(Name = "Can Add Step Task")]
        public bool CanAddStepTask { get; set; }
        [Display(Name = "Step Task Creation Optional Label")]
        public string StepTaskCreationOptionalLabel { get; set; }
        [Display(Name = "Can Remove Step Task")]
        public bool CanRemoveStepTask { get; set; }
        [Display(Name = "Remove Step Task Button Text")]
        public string RemoveStepTaskButtonText { get; set; }
        [Display(Name = "Remove Step Task Confirm Text")]
        public string RemoveStepTaskConfirmText { get; set; }
        [Display(Name = "Remove Step Task Success Message")]
        public string RemoveStepTaskSuccessMessage { get; set; }
        [Display(Name = "Enable SLA Change Request")]
        public bool EnableSLAChangeRequest { get; set; }
        [Display(Name = "Allowed Step Task Master Ids")]
        public string AllowedStepTaskMasterIds { get; set; }
        public long[] AllowedStepTaskMasterId { get; set; }
        [Display(Name = "Allowed Step Service Master Ids")]
        public string AllowedStepServiceMasterIds { get; set; }
        public long[] AllowedStepServiceMasterId { get; set; }


        [Display(Name = "Can Remove Adhoc Task")]
        public bool CanRemoveAdhocTask { get; set; }
        [Display(Name = "Remove Adhoc Task Button Text")]
        public string RemoveAdhocTaskButtonText { get; set; }
        [Display(Name = "Remove Adhoc Task Confirm Text")]
        public string RemoveAdhocTaskConfirmText { get; set; }
        [Display(Name = "Remove Adhoc Task Success Message")]
        public string RemoveAdhocTaskSuccessMessage { get; set; }
        [Display(Name = "Can Add Step Service")]
        public bool CanAddStepService { get; set; }
        [Display(Name = "Step Service Creation Optional Label")]
        public string StepServiceCreationOptionalLabel { get; set; }
        [Display(Name = "Step Service Add Button Text")]
        public string StepServiceAddButtonText { get; set; }
        [Display(Name = "Step Service Cancel Button Text")]
        public string StepServiceCancelButtonText { get; set; }
        [Display(Name = "Can Remove Step Service")]
        public bool CanRemoveStepService { get; set; }
        [Display(Name = "Remove Step Service Button Text")]
        public string RemoveStepServiceButtonText { get; set; }
        [Display(Name = "Remove Step Service Confirm Text")]
        public string RemoveStepServiceConfirmText { get; set; }
        [Display(Name = "Remove Step Service Success Message")]
        public string RemoveStepServiceSuccessMessage { get; set; }
        [Display(Name = "Enable Adhoc Service")]
        public bool EnableAdhocService { get; set; }
        [Display(Name = "Adhoc Service Header Text")]
        public string AdhocServiceHeaderText { get; set; }
        [Display(Name = "Adhoc Service Add Button Text")]
        public string AdhocServiceAddButtonText { get; set; }
        [Display(Name = "Adhoc Service Header Message")]
        public string AdhocServiceHeaderMessage { get; set; }
        [Display(Name = "Can Remove Adhoc Service")]
        public bool CanRemoveAdhocService { get; set; }
        [Display(Name = "Remove Adhoc Service Button Text")]
        public string RemoveAdhocServiceButtonText { get; set; }
        [Display(Name = "Remove Adhoc Service Confirm Text")]
        public string RemoveAdhocServiceConfirmText { get; set; }
        [Display(Name = "Remove Adhoc Service Success Message")]
        public string RemoveAdhocServiceSuccessMessage { get; set; }

        [Display(Name = "HierarchyId", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public long? HierarchyId { get; set; }

        [Display(Name = "HideDateAndSLA", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool? HideDateAndSLA { get; set; }
        [Display(Name = "IsAttachmentRequired", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool? IsAttachmentRequired { get; set; }
        [Display(Name = "ChangeStatusOnStepChange", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool? ChangeStatusOnStepChange { get; set; }
        [Display(Name = "ServiceOwnerText", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string ServiceOwnerText { get; set; }
        [Display(Name = "Include Requester In Owner List")]
        public bool? IncludeRequesterInOwnerList { get; set; }

        [Display(Name = "ServiceOwnerActAsStepTaskAssignee", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool? ServiceOwnerActAsStepTaskAssignee { get; set; }
        [Display(Name = "Create In BackGround")]
        public bool? CreateInBackGround { get; set; }
        [Display(Name = "Display Action Button Below")]
        public bool DisplayActionButtonBelow { get; set; }
        [Display(Name = "Save Changes Button Visibility Method")]
        public string SaveChangesButtonVisibilityMethod { get; set; }
        [Display(Name = "Edit Button Visibility Method")]
        public string EditButtonVisibilityMethod { get; set; }
        [Display(Name = "Print Button Visibility Method")]
        public string PrintButtonVisibilityMethod { get; set; }
        [Display(Name = "Reopen Button")]
        public bool ReopenButton { get; set; }
        [Display(Name = "Is Reopen Reason Required")]
        public bool IsReopenReasonRequired { get; set; }
        [Display(Name = "Reopen Button Text")]
        public string ReopenButtonText { get; set; }
        [Display(Name = "Save New Version Button Text")]
        public string SaveNewVersionButtonText { get; set; }
        [Display(Name = "Create New Version Button Text")]
        public string CreateNewVersionButtonText { get; set; }
        [Display(Name = "Admin Can Edit Udf")]
        public bool? AdminCanEditUdf { get; set; }
        [Display(Name = "Notification Subject")]
        public string NotificationSubject { get; set; }
        [Display(Name = "Admin Can Submit And Auto Complete")]
        public bool? AdminCanSubmitAndAutoComplete { get; set; }
        [Display(Name = "Enable Team As Owner")]
        public bool? EnableTeamAsOwner { get; set; }
        [Display(Name = "Is Team As Owner Mandatory")]
        public bool? IsTeamAsOwnerMandatory { get; set; }        
        public string Layout { get; set; }
        [Display(Name = "Hide Subject")]
        public bool? HideSubject { get; set; }
        [Display(Name = "Hide Description")]
        public bool? HideDescription { get; set; }

        [Display(Name = "Print Button")]
        public bool? EnablePrintButton { get; set; }
        [Display(Name = "Print Button Text")]
        public string PrintButtonText { get; set; }
        [Display(Name = "Print Method Name")]
        public string PrintMethodName { get; set; }
        [Display(Name = "Template Master Color")]
        public string TemplateMasterColor { get; set; }
        [Display(Name = "Template Master File Id")]
        public long? TemplateMasterFileId { get; set; }
        [Display(Name = "Enable Lock")]
        public bool? EnableLock { get; set; }
        [Display(Name = "ReSubmit Button")]
        public bool? ReSubmitButton { get; set; }
        [Display(Name = "ReSubmit Button Text")]
        public string ReSubmitButtonText { get; set; }
        [Display(Name = "Enable Banner")]
        public bool? EnableBanner { get; set; }
        [Display(Name = "Edit Button Validation Method")]
        public string EditButtonValidationMethod { get; set; }
        [Display(Name = "Workflow Template")]
        public long? WorkflowTemplateId { get; set; }
        [Display(Name = "Workflow Template Name")]
        public string WorkflowTemplateName { get; set; }
        [Display(Name= "Allow Template Change")]
        public bool? AllowTemplateChange { get; set; }
        [Display(Name = "Enable Code")]
        public bool? EnableCode { get; set; }
        [Display(Name = "Is Code Required")]
        public bool? IsCodeRequired { get; set; }
        [Display(Name = "Is Code Unique In Template")]
        public bool? IsCodeUniqueInTemplate { get; set; }
        [Display(Name = "Is Code Editable")]
        public bool? IsCodeEditable { get; set; }
        [Display(Name = "Code Label Name")]
        public string CodeLabelName { get; set; }
        [Display(Name = "Enable Sequence No")]
        public bool? EnableSequenceNo { get; set; }
        [Display(Name = "Is Sequence No Required")]
        public bool? IsSequenceNoRequired { get; set; }
        [Display(Name = "Is Sequence No Unique In Template")]
        public bool? IsSequenceNoUniqueInTemplate { get; set; }
        [Display(Name = "Is Sequence No Editable")]
        public bool? IsSequenceNoEditable { get; set; }
        [Display(Name = "Sequence No Label Name")]
        public string SequenceNoLabelName { get; set; }
        [Display(Name = "WorkFlow Step Id")]
        public long? WorkFlowStepId { get; set; }
        [Display(Name = "WorkFlow Step Name")]
        public string WorkFlowStepName { get; set; }
        [Display(Name = "WorkFlow Stage Id")]
        public long? WorkFlowStageId { get; set; }
        [Display(Name = "WorkFlow Stage Name")]
        public string WorkFlowStageName { get; set; }
        [Display(Name = "Disable Sharing")]
        public bool? DisableSharing { get; set; }
        [Display(Name = "Load Execution Method")]
        public string LoadExecutionMethod { get; set; }
        [Display(Name = "Document Template")]
        public string DocumentTemplateReferenceIds { get; set; }
        [Display(Name = "Note Template Reference Ids")]
        public string NoteTemplateReferenceIds { get; set; }
        [Display(Name = "Task Template Reference Ids")]
        public string TaskTemplateReferenceIds { get; set; }
        [Display(Name = "Service Template Reference Ids")]
        public string ServiceTemplateReferenceIds { get; set; }
        public long[] NoteTemplateReferenceId { get; set; }
        public long[] TaskTemplateReferenceId { get; set; }
        [Display(Name = "Service Template Reference Ids")]
        public long[] ServiceTemplateReferenceId { get; set; }
        [Display(Name = "Document Template")]
        public long[] DocumentTemplateId { get; set; }
        [Display(Name = "Completion Percentage")]
        public double? CompletionPercentage { get; set; }
        public TemplatePackageViewModel TemplatePackageConfig { get; set; }
        public long? TemplatePackageId { get; set; }

        [Display(Name = "Document Ready Script")]
        public string DocumentReadyScript { get; set; }

        [Display(Name = "Is NTS No Manual")]
        public bool? IsNtsNoManual { get; set; }
        [Display(Name = "Post Submit Execution Code")]
        public string PostSubmitExecutionCode { get; set; }
        [Display(Name = "Run Post script In Background")]
        public bool? RunPostscriptInBackground { get; set; }
        [Display(Name = "Disable Automatic Draft")]
        public bool? DisableAutomaticDraft { get; set; }
        public string DefaultView { get; set; }
        [Display(Name = "Enable Document Change Request")]
        public bool? EnableDocumentChangeRequest { get; set; }
        [Display(Name = "Template Owner")]
        public string TemplateOwner { get; set; }
        public bool? EnableParent { get; set; }

        public string JsonForm { get; set; }
        [Display(Name = "Is Nts No Readonly")]
        public bool? IsNtsNoReadonly { get; set; }
    }
}
