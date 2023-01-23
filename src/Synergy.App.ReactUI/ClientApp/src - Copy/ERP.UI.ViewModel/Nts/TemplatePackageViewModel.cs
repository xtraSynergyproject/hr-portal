using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    [Serializable]
    public class TemplatePackageViewModel : TemplatePackageConfigViewModel
    {
        //public long Id { get; set; }
        [Required]
        [Display(Name = "Package Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Nts Type")]
        public NtsTypeEnum PackageNtsType { get; set; }
        [Required]
        [Display(Name = "Category")]
        public long PackageTemplateCategoryId { get; set; }
        [Required]
        [Display(Name = "Module Name")]
        public ModuleEnum PackageModuleName { get; set; }
        public TemplateViewModel Template { get; set; }
        public TemplatePackageFieldViewModel Field { get; set; }
        [Display(Name = "Legal Entities")]
        public List<long> LegalEntities { get; set; }
        public long[] LegalEntitieData { get; set; }
        //public string LegalEntityNameCSV { get; set; }
        public string[] LegalEntitieNames { get; set; }
        public string LegalEntityNameCSV
        {
            get
            {
                var str = "";
                if (LegalEntitieNames != null)
                    str = string.Join(",", LegalEntitieNames);
                return str;
            }
        }
        public string LegalEntityName { get; set; }
        [Required]
        [Display(Name = "Status")]
        public DocumentStatusEnum PackageStatus { get; set; }
        public bool IsTemplateCreatedForPacakage { get; set; }
        public long? TemplateId { get; set; }

        public TemplatePackageServiceTaskViewModel ServiceTask { get; set; }       

    }
    [Serializable]
    public class TemplatePackageConfigViewModel : ViewModelBase
    {
        //Header     
        public bool EnableBannerAllow { get; set; }
        public bool AllowTemplateChangeAllow { get; set; }

        public bool NtsNoLabelNameAllow { get; set; }
        public bool HideSubjectAllow { get; set; }
        public bool SubjectLabelNameAllow { get; set; }      
        public bool IsSubjectRequiredAllow { get; set; }
        public bool IsSubjectEditableAllow { get; set; }
        public bool HideDescriptionAllow { get; set; }
        public bool DescriptionLabelNameAllow { get; set; }
        public bool IsDescriptionRequiredAllow { get; set; }
        public bool IsDescriptionEditableAllow { get; set; }      

        public bool SLAAllow { get; set; }
        public bool SLACalculationModeAllow { get; set; }
        public bool StartDateAllow { get; set; }
        public bool DueDateAllow { get; set; }
        public bool HideDateAndSLAAllow { get; set; }
        public bool ReminderDaysPriorDueDateAllow { get; set; }

        public bool EnableCodeAllow { get; set; }
        public bool IsCodeRequiredAllow { get; set; }
        public bool IsCodeUniqueInTemplateAllow { get; set; }
        public bool IsCodeEditableAllow { get; set; }
        public bool CodeLabelNameAllow { get; set; }

        public bool EnableSequenceNoAllow { get; set; }
        public bool IsSequenceNoRequiredAllow { get; set; }
        public bool IsSequenceNoUniqueInTemplateAllow { get; set; }
        public bool IsSequenceNoEditableAllow { get; set; }
        public bool SequenceNoLabelNameAllow { get; set; }

        public bool StatusLabelNameAllow { get; set; }

     

        public bool CompletionPercentageAllow { get; set; }

        public bool HeaderSectionTextAllow { get; set; }
        public bool HeaderSectionMessageAllow { get; set; }

     
        public bool OwnerTypeAllow { get; set; }
        public bool CanEditOwnerAllow { get; set; }

        public bool ServiceOwnerTextAllow { get; set; }
        public bool IncludeRequesterInOwnerListAllow { get; set; }
        public bool ServiceOwnerActAsStepTaskAssigneeAllow { get; set; }
        public bool AssignToTypeAllow { get; set; }
        public bool AssignedQueryTypeAllow { get; set; }
        public bool AssignedByQueryAllow { get; set; }

        public bool EnableTeamAsOwnerAllow { get; set; }
        public bool IsTeamAsOwnerMandatoryAllow { get; set; }

        public long TemplateMasterIdAllow { get; set; }

        public bool FieldSectionTextAllow { get; set; }
        public bool FieldSectionMessageAllow { get; set; }
        public bool DisableSharingAllow { get; set; }
        public bool StepSectionTextAllow { get; set; }
        public bool StepSectionMessageAllow { get; set; }
        public bool CanAddStepTaskAllow { get; set; }
        public bool StepTaskCreationOptionalLabelAllow { get; set; }
        public bool StepTaskAddButtonTextAllow { get; set; }
        public bool StepTaskCancelButtonTextAllow { get; set; }
        public bool CanRemoveStepTaskAllow { get; set; }
        public bool RemoveStepTaskButtonTextAllow { get; set; }
        public bool RemoveStepTaskConfirmTextAllow { get; set; }
        public bool RemoveStepTaskSuccessMessageAllow { get; set; }



        public bool EnableAdhocTaskAllow { get; set; }
        public bool CanAddAdhocTaskAllow { get; set; }
        public bool AdhocTaskHeaderTextAllow { get; set; }
        public bool AdhocTaskAddButtonTextAllow { get; set; }
        public bool AdhocTaskHeaderMessageAllow { get; set; }

        public bool CanRemoveAdhocTaskAllow { get; set; }
        public bool RemoveAdhocTaskButtonTextAllow { get; set; }
        public bool RemoveAdhocTaskConfirmTextAllow { get; set; }
        public bool RemoveAdhocTaskSuccessMessageAllow { get; set; }
        public bool CanAddStepServiceAllow { get; set; }
        public bool StepServiceCreationOptionalLabelAllow { get; set; }
        public bool StepServiceAddButtonTextAllow { get; set; }
        public bool StepServiceCancelButtonTextAllow { get; set; }
        public bool CanRemoveStepServiceAllow { get; set; }
        public bool RemoveStepServiceButtonTextAllow { get; set; }
        public bool RemoveStepServiceConfirmTextAllow { get; set; }
        public bool RemoveStepServiceSuccessMessageAllow { get; set; }
        public bool EnableAdhocServiceAllow { get; set; }
        public bool AdhocServiceHeaderTextAllow { get; set; }
        public bool AdhocServiceAddButtonTextAllow { get; set; }
        public bool AdhocServiceHeaderMessageAllow { get; set; }
        public bool CanRemoveAdhocServiceAllow { get; set; }
        public bool RemoveAdhocServiceButtonTextAllow { get; set; }
        public bool RemoveAdhocServiceConfirmTextAllow { get; set; }
        public bool RemoveAdhocServiceSuccessMessageAllow { get; set; }




        //Button
        public bool DraftButtonTextAllow { get; set; }
        public bool SaveButtonTextAllow { get; set; }
        public bool CompleteButtonTextAllow { get; set; }
        public bool RejectButtonTextAllow { get; set; }
        public bool ReturnButtonTextAllow { get; set; }
        public bool ReopenButtonTextAllow { get; set; }
        public bool DelegateButtonTextAllow { get; set; }
        public bool CancelButtonTextAllow { get; set; }
        public bool BackButtonTextAllow { get; set; }
        public bool CreateNewVersionButtonTextAllow { get; set; }
        public bool SaveChangesButtonTextAllow { get; set; }

        public bool SaveNewVersionButtonTextAllow { get; set; }

        public bool DraftButtonAllow { get; set; }
        public bool CreateNewVersionButtonAllow { get; set; }
        public bool SaveButtonAllow { get; set; }

        public bool CanViewVersionsAllow { get; set; }
        public bool DisplayActionButtonBelowAllow { get; set; }
        public bool SaveChangesButtonAllow { get; set; }

        public bool CompleteButtonAllow { get; set; }
        public bool IsCompleteReasonRequiredAllow { get; set; }
        public bool RejectButtonAllow { get; set; }
        public bool NotApplicableButtonAllow { get; set; }
        public bool IsRejectionReasonRequiredAllow { get; set; }
        public bool ReturnButtonAllow { get; set; }
        public bool ReopenButtonAllow { get; set; }
        public bool IsReopenReasonRequiredAllow { get; set; }
        public bool IsReturnReasonRequiredAllow { get; set; }
        public bool DelegateButtonAllow { get; set; }
        public bool IsDelegateReasonRequiredAllow { get; set; }
        public bool CancelButtonAllow { get; set; }
        public bool IsCancelReasonRequiredAllow { get; set; }
        public bool BackButtonAllow { get; set; }
        public bool CloseButtonAllow { get; set; }
        public bool CloseButtonTextAllow { get; set; }
        public bool PrintButtonVisibilityMethodAllow { get; set; }
        public bool EnablePrintButtonAllow { get; set; }
        public bool PrintButtonTextAllow { get; set; }
        public bool PrintMethodNameAllow { get; set; }
        public bool EnableLockAllow { get; set; }

        public bool ReSubmitButtonAllow { get; set; }
        public bool ReSubmitButtonTextAllow { get; set; }
        public bool EnableSLAChangeRequestAllow { get; set; }





        //Main
        public bool IsSystemRatingAllow { get; set; }
        public bool IsConfidentialAllow { get; set; }
        public bool CollapseHeaderAllow { get; set; }
        public long? DuplicatedFromIdAllow { get; set; }
      
        public bool LayoutColumnCountAllow { get; set; }
        public bool DocumentStatusAllow { get; set; }


        public bool DisableMessageAllow { get; set; }

     
        public bool ClientValidationScriptAllow { get; set; }
        public bool DocumentReadyScriptAllow { get; set; }
        public bool ServerValidationScriptAllow { get; set; }
        public bool LoadExecutionMethodAllow { get; set; }
        public bool PostSubmitExecutionMethodAllow { get; set; }
        public bool PostSubmitExecutionCodeAllow { get; set; }
        public bool RunPostscriptInBackgroundAllow { get; set; }
        public bool DisableAutomaticDraftAllow { get; set; }
        public List<string> PostSubmitExecutionMethodAllowed { get; set; }
        public bool EditButtonValidationMethodAllow { get; set; }
        public bool PreSubmitExecutionMethodAllow { get; set; }
        public bool SaveChangesButtonVisibilityMethodAllow { get; set; }
        public bool EditButtonVisibilityMethodAllow { get; set; }


     
        public bool CanViewServiceReferenceAllow { get; set; }
        public bool ServiceReferenceTextAllow { get; set; }
        public bool NotificationUrlPatternAllow { get; set; }
        public bool ModuleNameAllow { get; set; }

      
        public bool EnableTaskAutoCompleteAllow { get; set; }
     
      
     
        public bool IsAttachmentRequiredAllow { get; set; }
        public bool ChangeStatusOnStepChangeAllow { get; set; }
      
        public bool CreateInBackGroundAllow { get; set; }
        public bool DisableStepTaskAllow { get; set; }
        public bool AdminCanEditUdfAllow { get; set; }
        public bool AdminCanSubmitAndAutoCompleteAllow { get; set; }
    
        public bool LayoutAllow { get; set; }
        public bool ReturnUrlAllow { get; set; }
     

     
    }


}
