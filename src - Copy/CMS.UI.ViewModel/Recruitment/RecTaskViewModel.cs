using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class RecTaskViewModel : RecTask
    {
        public string SubjectLabelName { get; set; }
        public bool IsSubjectRequired { get; set; }
        public bool IsSubjectEditable { get; set; }
        public string DescriptionLabelName { get; set; }
        public bool IsDescriptionRequired { get; set; }
        public bool IsDescriptionEditable { get; set; }
        public bool? HideDateAndSLA { get; set; }
        public string UdfIframeSrc { get; set; }
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
        public bool SaveNewVersionButton { get; set; }
        public ModuleEnum? ModuleName { get; set; }
        public bool SaveButton { get; set; }

        public bool CanViewVersions { get; set; }
        public bool? DisplayActionButtonBelow { get; set; }
        public bool SaveChangesButton { get; set; }

        public bool CompleteButton { get; set; }
        public bool IsCompleteReasonRequired { get; set; }
        public bool RejectButton { get; set; }
        public bool NotApplicableButton { get; set; }
        public bool IsLockVisible { get; set; }
        public bool IsReleaseVisible { get; set; }
        public bool IsTaskTeamOwner { get; set; }
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

        public NtsActionEnum? TemplateAction { get; set; }

        public string ActiveUserId { get; set; }
        public string AssignedToUserId { get; set; }
        public string AssignedToTeamId { get; set; }
        public FieldDisplayModeEnum? DisplayMode { get; set; }
        public NtsUserTypeEnum? TemplateUserType { get; set; }
        public bool CanEditHeader { get; set; }

        public string CancelEditButtonText { get; set; }
        public IList<IdNameViewModel> TeamMembers { get; set; }
        public string AssigneeDisplayName { get; set; }
        public string TaskOwnerFirstLetter { get; set; }
        public string OwnerUserName { get; set; }
        public string AssigneeUserName { get; set; }
        public string StatusClass
        {
            get
            {
                switch (TaskStatusCode)
                {
                    case "DRAFT":
                        return "LightBlue";
                    case "INPROGRESS":
                        return "Orange";
                    case "COMPLETED":
                        return "Green";
                    case "CANCELLED":
                        return "Gray";
                    case "OVERDUE":
                        return "Red";
                    case "REJECTED":
                        return "Gray";
                    default:
                        return "Blue";
                }
            }
        }
        public string StepTemplateIds { get; set; }
        public string DisplayDueDate { get; set; }
        public string ServiceDetailsHeight { get; set; }
        public long? TaskVersionId { get; set; } 
        public bool IsTextBoxEdit1 { get; set; } 
        public bool IsTextBoxEdit2 { get; set; } 
        public bool IsTextBoxEdit3 { get; set; } 
        public bool IsTextBoxEdit4 { get; set; } 
        public bool IsTextBoxEdit5 { get; set; }
        public bool IsTextBoxEdit6 { get; set; }
        public bool IsTextBoxEdit7 { get; set; }
        public bool IsTextBoxEdit8 { get; set; }
        public bool IsTextBoxEdit9 { get; set; }
        public bool IsTextBoxEdit10 { get; set; }

        public bool IsDropDownEdit1 { get; set; }
        public bool IsDropDownEdit2 { get; set; }
        public bool IsDropDownEdit3 { get; set; }
        public bool IsDropDownEdit4 { get; set; }
        public bool IsDropDownEdit5 { get; set; }
        public bool IsDropDownEdit6 { get; set; }
        public bool IsDropDownEdit7 { get; set; }
        public bool IsDropDownEdit8 { get; set; }
        public bool IsDropDownEdit9 { get; set; }
        public bool IsDropDownEdit10 { get; set; }
        public string UserRole { get; set; }
        public string Position { get; set; }
        public string OrgId { get; set; }
        public string CandidateName { get; set; }
        public string JobName { get; set; }
        public string OrgUnitName { get; set; }
        public string GaecNo { get; set; }
        public string ReturnTemplateName { get; set; }
        public NtsActionEnum? GridTemplateAction { get; set; }
        public bool? GridRowLock { get; set; }
        public bool IsIncludeEmailAttachment { get; set; }
        public string EmailSettingId { get; set; }
        public string TaskListJsonString { get; set; }
        public string BatchId { get; set; }
        public string DummyColumn { get; set; }
        public long? Count { get; set; }
        public string BannerId { get; set; }
        public string BannerStyle { get; set; }
    }
}
