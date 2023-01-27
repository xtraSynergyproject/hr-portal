using ERP.Data.Model;
using ERP.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class TemplateViewModelBase : ViewModelBase
    {
        public string TemplateName { get; set; }
        public long TemplateId { get; set; }
        public string TemplateMasterCode { get; set; }
        public string LegalEntityCode { get; set; }
        public long TemplateMasterId { get; set; }
        public string TemplateMasterName { get; set; }
        public string TemplateTypeCode { get; set; }
        public string TemplateCategoryCode { get; set; }
        public string TemplateClassificationCode { get; set; }
        public EnvironmentViewModel Environment { get; set; }
        public ActiveUserViewModel ActiveUser { get; set; }
        //public ActiveUserViewModel Recipient { get; set; }
        //public ActiveUserViewModel Sender { get; set; }

        public DateTime? SubmittedDate { get; set; }
        public DateTime? RejectedDate { get; set; }
        public DateTime? CanceledDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public decimal? SequenceNo { get; set; }
        public string Layout { get; set; }

        public string ReminderDescription { get; set; }
        public string RecurrenceDescription { get; set; }

        [Display(Name = "Team")]
        public long? OwnerTeamId { get; set; }
        public string OwnerTeamName { get; set; }

        public long? ActiveUserId { get; set; }

        public NtsUserTypeEnum? TemplateUserType { get; set; } //set runtime
        public NtsActionEnum? TemplateAction { get; set; }//set runtime
        public NtsActionEnum? OverrideTemplateAction { get; set; }
        public bool IsSelfTemplate { get; set; }//set runtime
        public bool? DisableStepTask { get; set; }

        public long? LegalEntityId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DayTimeFormat)]
        [Display(Name = "SLA", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public TimeSpan? SLA { get; set; }

        [Display(Name = "SLA", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string SLAText
        {
            get
            {
                if (SLA != null)
                {
                    return SLA.Value.ToString(@"d\.hh\:mm");
                }
                return "";
            }

        }
        [Display(Name = "SLACalculationMode", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public SLACalculationMode? SLACalculationMode { get; set; }

        public List<TemplateFieldValueViewModel> Controls { get; set; }

        public TemplateFieldValueViewModel Field(string fieldName)
        {
            return Controls.FirstOrDefault(x => x.FieldName == fieldName);
        }

        public AssignToTypeEnum? OwnerType { get; set; }
        public long? TeamId { get; set; }
        public long? UserId { get; set; }
        public long? HolderUserId { get; set; }
        public long? RequestedByUserId { get; set; }

        public bool SkipPreAndPostScript { get; set; }
        //[Display(Name = "Requested By")]
        [Display(Name = "RequesterDisplayName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string RequesterDisplayName { get; set; }
        //[Display(Name = "Requested By")]
        [Display(Name = "RequesterUserUserName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string RequesterUserUserName { get; set; }

        private int _TemplateMaximumColumn;
        public int TemplateMaximumColumn
        {
            get { return _TemplateMaximumColumn == 0 ? Constant.Nts.DefaultTemplateMaximumColumn : _TemplateMaximumColumn; }
            set { _TemplateMaximumColumn = value; }
        }
        public int TemplateColumnCount { get { return 12 / TemplateMaximumColumn; } }


        private string _DraftButtonText;
        public string DraftButtonText
        {
            get { return _DraftButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultSaveAsDraftButtonName : _DraftButtonText; }
            set { _DraftButtonText = value; }
        }
        private string _SaveNewVersionButtonText;
        public string SaveNewVersionButtonText
        {
            get { return _SaveNewVersionButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultSaveNewVersionName : _SaveNewVersionButtonText; }
            set { _SaveNewVersionButtonText = value; }
        }
        private string _ReopenButtonText;
        public string ReopenButtonText
        {
            get { return _ReopenButtonText.IsNullOrEmpty() ? "Reopen" : _ReopenButtonText; }
            set { _ReopenButtonText = value; }
        }
        private string _PrintButtonText;
        public string PrintButtonText
        {
            get { return _PrintButtonText.IsNullOrEmpty() ? "Print" : _PrintButtonText; }
            set { _PrintButtonText = value; }
        }
        private string _CancelEditButtonText;
        public string CancelEditButtonText
        {
            get { return _CancelEditButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultCancelEditButtonName : _CancelEditButtonText; }
            set { _CancelEditButtonText = value; }
        }
        private string _CreateNewVersionButtonText;
        public string CreateNewVersionButtonText
        {
            get { return _CreateNewVersionButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultCreateNewVersionName : _CreateNewVersionButtonText; }
            set { _CreateNewVersionButtonText = value; }
        }
        private string _SaveButtonText;
        public string SaveButtonText
        {
            get { return _SaveButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultSubmitButtonName : _SaveButtonText; }
            set { _SaveButtonText = value; }
        }
        private string _SaveChangesButtonText;
        public string SaveChangesButtonText
        {
            get { return _SaveChangesButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultSaveChangesButtonName : _SaveChangesButtonText; }
            set { _SaveChangesButtonText = value; }
        }

        private string _ReturnButtonText;
        public string ReturnButtonText
        {
            get { return _ReturnButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultReturnButtonName : _ReturnButtonText; }
            set { _ReturnButtonText = value; }
        }
        private string _RejectButtonText;
        public string RejectButtonText
        {
            get { return _RejectButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultRejectButtonName : _RejectButtonText; }
            set { _RejectButtonText = value; }
        }
        private string _CompleteButtonText;
        public string CompleteButtonText
        {
            get { return _CompleteButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultCompleteButtonName : _CompleteButtonText; }
            set { _CompleteButtonText = value; }
        }

        private string _BackButtonText;
        public string BackButtonText
        {
            get { return _BackButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultBackButtonName : _BackButtonText; }
            set { _BackButtonText = value; }
        }
        private string _CloseButtonText;
        public string CloseButtonText
        {
            get { return _CloseButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultCloseButtonName : _CloseButtonText; }
            set { _CloseButtonText = value; }
        }
        private string _DelegateButtonText;
        public string DelegateButtonText
        {
            get { return _DelegateButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultDelegateButtonName : _DelegateButtonText; }
            set { _DelegateButtonText = value; }
        }
        private string _HeaderSectionText;
        public string HeaderSectionText
        {
            get { return _HeaderSectionText.IsNullOrEmpty() ? Constant.Nts.DefaultHeaderSectionName : _HeaderSectionText; }
            set { _HeaderSectionText = value; }
        }
        private string _SharedSectionText;
        public string SharedSectionText
        {
            get { return _SharedSectionText.IsNullOrEmpty() ? Constant.Nts.DefaultSharedSectionName : _SharedSectionText; }
            set { _SharedSectionText = value; }
        }
        public string HeaderSectionMessage { get; set; }

        private string _FieldSectionText;
        public string FieldSectionText
        {
            get { return _FieldSectionText.IsNullOrEmpty() ? Constant.Nts.DefaultFieldSectionName : _FieldSectionText; }
            set { _FieldSectionText = value; }
        }
        public string FieldSectionMessage { get; set; }


        private string _StepSectionName;
        public string StepSectionName
        {
            get { return _StepSectionName.IsNullOrEmpty() ? Constant.Nts.DefaultStepSectionName : _StepSectionName; }
            set { _StepSectionName = value; }
        }

        private string _ServicePlusStepSectionName;
        public string ServicePlusStepSectionName
        {
            get { return _ServicePlusStepSectionName.IsNullOrEmpty() ? ERP.Translation.Nts.Master.Services : _ServicePlusStepSectionName; }
            set { _ServicePlusStepSectionName = value; }
        }

        private string _StatusLabelName;
        public string StatusLabelName
        {
            get { return _StatusLabelName.IsNullOrEmpty() ? Constant.Nts.DefaultStatusLabelText : _StatusLabelName; }
            set { _StatusLabelName = value; }
        }

        public string StepSectionMessage { get; set; }
        public string ServicePlusStepSectionMessage { get; set; }
        public bool CanEditOwner { get; set; }
        public bool CanEditHeader { get; set; }

        public bool DraftButton { get; set; }

        public bool SaveNewVersionButton { get; set; }
        public bool CreateNewVersionButton { get; set; }

        public bool SaveButton { get; set; }
        public bool SaveChangesButton { get; set; }
        //public bool SubmitButton { get; set; }
        public bool CompleteButton { get; set; }
        public bool? PrintButton { get; set; }
        public bool RejectButton { get; set; }
        public bool ReturnButton { get; set; }
        public bool CancelButton { get; set; }
        public bool DelegateButton { get; set; }
        public bool CloseButton { get; set; }
        public bool BackButton { get; set; }
        public bool CollapseHeader { get; set; }
        public bool NotApplicableButton { get; set; }

        public bool? ReopenButton { get; set; }
        public bool? IsReopenReasonRequired { get; set; }


        public bool IsRejectionReasonRequired { get; set; }
        public string RejectionReason { get; set; }
        public bool IsReturnReasonRequired { get; set; }
        public string ReturnReason { get; set; }
        public bool IsCompleteReasonRequired { get; set; }
        public string CompleteReason { get; set; }
        public bool IsDelegateReasonRequired { get; set; }
        public string DelegateReason { get; set; }
        public bool IsCancelReasonRequired { get; set; }
        public string CancelReason { get; set; }
        public string Comment { get; set; }


        public bool IsHierarchyAdmin { get; set; }
        public long? HierarchyId { get; set; }
        public bool IsSubjectRequired { get; set; }
        public bool IsSubjectEditable { get; set; }

        public bool IsDescriptionRequired { get; set; }
        public bool IsDescriptionEditable { get; set; }
        public bool DisableMessage { get; set; }


        public string ClientValidationScript { get; set; }
        public string ServerValidationScript { get; set; }
        public string PostSubmitExecutionMethod { get; set; }
        public string PreSubmitExecutionMethod { get; set; }

        public string LoadExecutionMethod { get; set; }

        public string PrintMethodName { get; set; }

        public long? ReferenceId { get; set; }
        public ReferenceTypeEnum? ReferenceTypeCode { get; set; }
        public NodeEnum? ReferenceNode { get; set; }

        [Display(Name = "NotificationUrlPattern", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string NotificationUrlPattern { get; set; }
        [Display(Name = "ModuleNotificationUrlPattern", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string ModuleNotificationUrlPattern { get; set; }
        [Display(Name = "ModuleName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public ModuleEnum? ModuleName { get; set; }
        public string Url { get; set; }
        public bool CanViewVersions { get; set; }


        public long? VersionNo { get; set; }

        public bool CanRemoveStepTask { get; set; }


        private string _RemoveStepTaskConfirmText;
        public string RemoveStepTaskConfirmText
        {
            get { return _RemoveStepTaskConfirmText.IsNullOrEmpty() ? ERP.Translation.Nts.Template.RemoveStepTaskConfirmMessage : _RemoveStepTaskConfirmText; }
            set { _RemoveStepTaskConfirmText = value; }
        }

        private string _DescriptionLabelName;
        public string DescriptionLabelName
        {
            get { return _DescriptionLabelName.IsNullOrEmpty() ? ERP.Translation.Nts.Template.Description : _DescriptionLabelName; }
            set { _DescriptionLabelName = value; }
        }
        private string _SubjectLabelName;
        public string SubjectLabelName
        {
            get { return _SubjectLabelName.IsNullOrEmpty() ? ERP.Translation.Nts.Template.Subject : _SubjectLabelName; }
            set { _SubjectLabelName = value; }
        }

        private string _RemoveStepTaskButtonText;
        public string RemoveStepTaskButtonText
        {
            get { return _RemoveStepTaskButtonText.IsNullOrEmpty() ? ERP.Translation.General.Delete : _RemoveStepTaskButtonText; }
            set { _RemoveStepTaskButtonText = value; }
        }

        private string _SLAChangeRequestButtonText;
        public string SLAChangeRequestButtonText
        {
            get { return _SLAChangeRequestButtonText.IsNullOrEmpty() ? ERP.Translation.Nts.Template.SLAChangeRequestButton : _SLAChangeRequestButtonText; }
            set { _SLAChangeRequestButtonText = value; }
        }

        private string _ApproveSLAChangeRequestButtonText;
        public string ApproveSLAChangeRequestButtonText
        {
            get { return _ApproveSLAChangeRequestButtonText.IsNullOrEmpty() ? ERP.Translation.Nts.Template.ApproveSLAChangeRequestButton : _ApproveSLAChangeRequestButtonText; }
            set { _ApproveSLAChangeRequestButtonText = value; }
        }

        public bool EnableSLAChangeRequest { get; set; }
        public bool HasAnyPendingSLAChangeRequest { get; set; }
        public bool ApproveSLAChangeRequest { get; set; }

        [Display(Name = "OwnerEmployeeNo", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string OwnerEmployeeNo { get; set; }
        [Display(Name = "OwnerSponsorhipNo", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string OwnerSponsorhipNo { get; set; }
        [Display(Name = "OwnerJobTitle", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string OwnerJobTitle { get; set; }
        [Display(Name = "OwnerDepartmentName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string OwnerDepartmentName { get; set; }
        [Display(Name = "OwnerLocationtName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string OwnerLocationtName { get; set; }


        //[Display(Name = "Hide Date AndSLA")]
        [Display(Name = "HideDateAndSLA", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool? HideDateAndSLA { get; set; }
        //[Display(Name = "Is Attachment Required")]
        [Display(Name = "IsAttachmentRequired", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public bool? IsAttachmentRequired { get; set; }

        public bool? IncludeRequesterInOwnerList { get; set; }

        public bool? IsRecurringItem { get; set; }
        public long? RecurringItemParentId { get; set; }

        public string UdfName1 { get; set; }
        public string UdfCode1 { get; set; }
        public string UdfValue1 { get; set; }
        public string UdfName2 { get; set; }
        public string UdfCode2 { get; set; }
        public string UdfValue2 { get; set; }
        public string UdfName3 { get; set; }
        public string UdfCode3 { get; set; }
        public string UdfValue3 { get; set; }
        public string UdfName4 { get; set; }
        public string UdfCode4 { get; set; }
        public string UdfValue4 { get; set; }

        public string UdfName5 { get; set; }
        public string UdfCode5 { get; set; }
        public string UdfValue5 { get; set; }

        public bool IsAllowed { get; set; }

        public DataSourceEnum? DataSource { get; set; }

        public bool? AllowOnePerTagTo { get; set; }

        public string SaveChangesButtonVisibilityMethod { get; set; }
        public string EditButtonVisibilityMethod { get; set; }
        public bool? AdminCanEditUdf { get; set; }
        public string NotificationSubject { get; set; }
        public bool? AdminCanSubmitAndAutoComplete { get; set; }

        public bool? EnableTeamAsOwner { get; set; }
        public bool? IsTeamAsOwnerMandatory { get; set; }
        public string NtsNoLabelName { get; set; }

        public bool? IsArchived { get; set; }
        public bool? HideSubject { get; set; }
        public bool? HideDescription { get; set; }

        public long? TemplateMasterFileId { get; set; }

        private string _TemplateMasterColor;
        public string TemplateMasterColor
        {
            get
            {
                if (_TemplateMasterColor.IsNullOrEmpty())
                {
                    return Helper.RandomColor();
                }
                return _TemplateMasterColor;
            }
            set { _TemplateMasterColor = value; }
        }


        public bool? EnablePrintButton { get; set; }
        public bool? EnableLock { get; set; }

        public bool CheckinButton { get; set; }
        public bool CheckoutButton { get; set; }

        public long? LockedbyUserId { get; set; }

        public bool? ReSubmitButton { get; set; }
        private string _ReSubmitButtonText;
        public string ReSubmitButtonText
        {
            get { return _ReSubmitButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultResubmitButtonName : _ReSubmitButtonText; }
            set { _ReSubmitButtonText = value; }
        }

        public NtsTypeEnum? TemplateMasterNtsType { get; set; }

        public string EditButtonValidationMethod { get; set; }
        public bool? AllowTemplateChange { get; set; }
        public long? OldId { get; set; }

        public string Code { get; set; }

        public bool? EnableCode { get; set; }
        public bool? IsCodeRequired { get; set; }
        public bool? IsCodeUniqueInTemplate { get; set; }
        public bool? IsCodeEditable { get; set; }

        private string _CodeLabelName;
        public string CodeLabelName
        {
            get { return _CodeLabelName.IsNullOrEmpty() ? Constant.Nts.DefaultCodeLabelText : _CodeLabelName; }
            set { _CodeLabelName = value; }
        }


        public bool? EnableSequenceNo { get; set; }
        public bool? IsSequenceNoRequired { get; set; }
        public bool? IsSequenceNoUniqueInTemplate { get; set; }
        public bool? IsSequenceNoEditable { get; set; }

        private string _SequenceNoLabelName;
        public string SequenceNoLabelName
        {
            get { return _SequenceNoLabelName.IsNullOrEmpty() ? Constant.Nts.DefaultSequenceLabelText : _SequenceNoLabelName; }
            set { _SequenceNoLabelName = value; }
        }
        public string WorkFlowStep { get; set; }
        public string WorkFlowStage { get; set; }

        public string TagIds { get; set; }
        public NtsPriorityEnum? Priority { get; set; }




        /// <summary>
        /// Forecasted Start Date
        /// </summary>

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        [Display(Name = "Forecasted Start Date")]
        public DateTime? PlanDate { get; set; }
        /// <summary>
        /// Forecasted Due Date
        /// </summary>

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        [Display(Name = "Forecasted Due Date")]
        public DateTime? PlanDueDate { get; set; }

        /// <summary>
        /// Planned Start Date
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        [Display(Name = "Planned Start Date")]
        public DateTime? ProposedDate { get; set; }

        /// <summary>
        /// Planned Due date
        /// </summary>

        [Display(Name = "Planned Due Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? ProposedDueDate { get; set; }
        public string DocumentReadyScript { get; set; }

        public bool? IsNtsNoManual { get; set; }

        public string DefaultView { get; set; }
        public bool? DisableDirectApproval { get; set; }
        public bool? EnableParent { get; set; }
        public NtsModifiedStatusEnum? ModifiedStatus { get; set; }
        public List<NoteFieldValueViewModel> SyncFields { get; set; }
        public List<ServiceFieldValueViewModel> SyncServiceFields { get; set; }
        public List<TaskFieldValueViewModel> SyncTaskFields { get; set; }
        public bool? IsNtsNoReadonly { get; set; }
    }

}


