using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class SynergyTemplateViewModel
    {
        public long TemplateId { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }

        public string JsonForm { get; set; }
        public bool IsJson { get; set; }
        public NtsTypeEnum TemplateMasterTemplateCategoryNtsType { get; set; }

        public DataActionEnum DataAction { get; set; }
        public DataOperation? Operation { get; set; }

        public List<SynergyTemplateFieldViewModel> Groups { get; set; }
        public List<SynergyTemplateFieldViewModel> Columns { get; set; }
        public List<SynergyTemplateFieldViewModel> Cells { get; set; }
        public List<SynergyTemplateFieldViewModel> Components { get; set; }

        //Synergy Properties
        public bool CollapseHeader { get; set; }
        public string NtsNoLabelName { get; set; }
        public string SubjectLabelName { get; set; }
        public bool IsSubjectRequired { get; set; }
        public bool IsSubjectEditable { get; set; }
        public bool? HideSubject { get; set; }
        public string DescriptionLabelName { get; set; }
        public bool IsDescriptionRequired { get; set; }
        public bool IsDescriptionEditable { get; set; }
        public bool? HideDescription { get; set; }
        public bool? HideDateAndSLA { get; set; }
        public bool? IsAttachmentRequired { get; set; }
        public string HeaderSectionText { get; set; }
        public string HeaderSectionMessage { get; set; }
        public string FieldSectionText { get; set; }
        public string FieldSectionMessage { get; set; }        
       
        public int LayoutColumnCount { get; set; }

        public bool DraftButton { get; set; }
        public string DraftButtonText { get; set; }
        public bool CancelButton { get; set; }
        public string CancelButtonText { get; set; }
        public bool IsCancelReasonRequired { get; set; }
        public bool BackButton { get; set; }
        public string BackButtonText { get; set; }
        public bool RejectButton { get; set; }
        public string RejectButtonText { get; set; }
        public bool IsRejectionReasonRequired { get; set; }

        public bool CompleteButton { get; set; }
        public string CompleteButtonText { get; set; }
        public bool IsCompleteReasonRequired { get; set; }

        public bool SaveButton { get; set; }
        public string SaveButtonText { get; set; }
      
        public bool ReturnButton { get; set; }
        public string ReturnButtonText { get; set; }
        public bool IsReturnReasonRequired { get; set; }

        public bool CloseButton { get; set; }
        public string CloseButtonText { get; set; }
        public bool SaveChangesButton { get; set; }
        public string SaveChangesButtonText { get; set; }
        public bool ReopenButton { get; set; }
        public bool IsReopenReasonRequired { get; set; }
        public string ReopenButtonText { get; set; }
        public bool? ReSubmitButton { get; set; }
        public string ReSubmitButtonText { get; set; }
        public bool? EnablePrintButton { get; set; }
        public string PrintButtonText { get; set; }
        public string PrintMethodName { get; set; }
        public string SaveNewVersionButtonText { get; set; }
        public string CreateNewVersionButtonText { get; set; }
        public bool? AdminCanEditUdf { get; set; }
        public bool NotApplicableButton { get; set; }

        public bool CanViewVersions { get; set; }

        public TimeSpan? SLA { get; set; }
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
        public TimeSpan? SLAHour { get; set; }
       
       
        
        public bool CanEditOwner { get; set; }
        public bool CreateNewVersionButton { get; set; }
        public AssignToTypeEnum? OwnerType { get; set; }
        public long? OwnerId { get; set; }
        public long? OwnerUserId { get; set; }
        public long? TeamId { get; set; }
        public AssignToTypeEnum? AssignToType { get; set; }
        public AssignedQueryTypeEnum? AssignedQueryType { get; set; }
        public long? AssignedTo { get; set; }
        public string AssignedByQuery { get; set; }
        
        public DocumentStatusEnum? DocumentStatus { get; set; }
        public bool DisableMessage { get; set; }
      
        public string ClientValidationScript { get; set; }
        public string ServerValidationScript { get; set; }
        public string PostSubmitExecutionMethod { get; set; }
        public string PreSubmitExecutionMethod { get; set; }
        public bool CanViewServiceReference { get; set; }
        public string ServiceReferenceText { get; set; }
        public string NotificationUrlPattern { get; set; }
        public ModuleEnum ModuleName { get; set; }
        public string StatusLabelName { get; set; }
        public bool EnableTaskAutoComplete { get; set; }
        public bool? DisableStepTask { get; set; }
        public bool EnableAdhocTask { get; set; }
        public bool CanAddAdhocTask { get; set; }
        public string AdhocTaskHeaderText { get; set; }
        public string AdhocTaskAddButtonText { get; set; }
        public string AdhocTaskHeaderMessage { get; set; }
       
        public string StepTaskAddButtonText { get; set; }
        public string StepTaskCancelButtonText { get; set; }
        public bool SaveAsNewVersionButton { get; set; }
        public string SaveAsNewVersionButtonText { get; set; }
        

        public string StepSectionText { get; set; }
        public string StepSectionMessage { get; set; }
        public bool CanAddStepTask { get; set; }
        public string StepTaskCreationOptionalLabel { get; set; }
        public bool CanRemoveStepTask { get; set; }
        public string RemoveStepTaskButtonText { get; set; }
        public string RemoveStepTaskConfirmText { get; set; }
        public string RemoveStepTaskSuccessMessage { get; set; }
        public string AllowedStepTaskMasterIds { get; set; }
        public long[] AllowedStepTaskMasterId { get; set; }
        public string AllowedStepServiceMasterIds { get; set; }
        public long[] AllowedStepServiceMasterId { get; set; }
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
        public long? HierarchyId { get; set; }
      
        public bool? ChangeStatusOnStepChange { get; set; }
        public string ServiceOwnerText { get; set; }
        public bool? IncludeRequesterInOwnerList { get; set; }
        public bool? ServiceOwnerActAsStepTaskAssignee { get; set; }
        public bool? CreateInBackGround { get; set; }
        public bool DisplayActionButtonBelow { get; set; }
        public string SaveChangesButtonVisibilityMethod { get; set; }
        public string EditButtonVisibilityMethod { get; set; }
        public string PrintButtonVisibilityMethod { get; set; }
      
        public string NotificationSubject { get; set; }
        public bool? AdminCanSubmitAndAutoComplete { get; set; }
        public bool? EnableTeamAsOwner { get; set; }
        public bool? IsTeamAsOwnerMandatory { get; set; }
        public string Layout { get; set; }
      
       
        public string TemplateMasterColor { get; set; }
        public long? TemplateMasterFileId { get; set; }
        public bool? EnableLock { get; set; }
      
        public bool? EnableBanner { get; set; }
        public string EditButtonValidationMethod { get; set; }
        public long? WorkflowTemplateId { get; set; }
        public string WorkflowTemplateName { get; set; }
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
        public long? WorkFlowStepId { get; set; }
        public string WorkFlowStepName { get; set; }
        public long? WorkFlowStageId { get; set; }
        public string WorkFlowStageName { get; set; }
        public bool? DisableSharing { get; set; }
        public string LoadExecutionMethod { get; set; }
        public string DocumentTemplateReferenceIds { get; set; }
        public string NoteTemplateReferenceIds { get; set; }
        public string TaskTemplateReferenceIds { get; set; }
        public string ServiceTemplateReferenceIds { get; set; }
        public long[] NoteTemplateReferenceId { get; set; }
        public long[] TaskTemplateReferenceId { get; set; }
        public long[] ServiceTemplateReferenceId { get; set; }
        public long[] DocumentTemplateId { get; set; }
        public double? CompletionPercentage { get; set; }
        public string DocumentReadyScript { get; set; }
        public bool? IsNtsNoManual { get; set; }
        public string PostSubmitExecutionCode { get; set; }
        public bool? RunPostscriptInBackground { get; set; }
        public bool? DisableAutomaticDraft { get; set; }
        public string DefaultView { get; set; }
        public bool? EnableDocumentChangeRequest { get; set; }
        public string TemplateOwner { get; set; }
        public bool? EnableParent { get; set; }

    }
}
