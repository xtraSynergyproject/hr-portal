using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class ServiceViewModel : TemplateViewModelBase
    {
        // public long Id { get; set; }
        [Display(Name = "ServiceNo", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string ServiceNo { get; set; }
        public long? ServicePlusId { get; set; }
        public bool? OverrideUdfVisibility { get; set; }

        public string IsAdminMode { get; set; }


        /// <summary>
        /// This is used in versioning
        /// </summary>
        public long? ServiceId { get; set; }
        public long? ParentServiceId { get; set; }
        public ParentServiceTypeEnum? ParentServiceType { get; set; }


        //[Display(Name = "Service Owner")]
        [Display(Name = "OwnerUserId", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public long? OwnerUserId { get; set; }


        //[Display(Name = "Service Owner")]
        [Display(Name = "OwnerDisplayName", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string OwnerDisplayName { get; set; }
        //[Display(Name = "Service Owner")]
        [Display(Name = "OwnerUserUserName", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string OwnerUserUserName { get; set; }

        public string OwnerMobile { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerGrade { get; set; }
        public DateTime? OwnerDateofJoin { get; set; }
        // public long? ServicePlusTemplateId { get; set; }
        public long? ServiceStepTemplateId { get; set; }
        public long? StepTaskId { get; set; }
        public string StepTaskNo { get; set; }
        public long? ServiceStepServiceId { get; set; }

        public NtsServicePlusServiceTypeEnum? ServicePlusServiceType { get; set; }
        public bool AllowPastStartDate { get; set; }        
        public string ServiceName { get; set; }
        //[Display(Name = "Service Name")]
        [Display(Name = "TemplateTemplateMasterName", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string TemplateTemplateMasterName { get; set; }
        //[Display(Name = "Service Status")]
        [Display(Name = "ServiceStatusCode", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string ServiceStatusCode { get; set; }
        //[Display(Name = "Service Status")]
        [Display(Name = "ServiceStatusName", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string ServiceStatusName { get; set; }
        public FieldDisplayModeEnum DisplayMode { get; set; }
        public List<TaskViewModel> Steps { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Start Date")]
        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public DateTime? StartDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Due Date")]
        [Display(Name = "DueDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public DateTime? DueDate { get; set; }

        //[Display(Name = "Reminder Date")]
        [Display(Name = "ReminderDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? ReminderDate { get; set; }

        //[Display(Name = "Completed Date")]
        [Display(Name = "CompletionDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? CompletionDate { get; set; }
        //     [Required]



        [Display(Name = "Subject", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string Subject { get; set; }
        public string Description { get; set; }

        public string ServiceTasks { get; set; }
        public List<TaskViewModel> ServiceTasksList { get; set; }
        public string ServicePlusServices { get; set; }


        public bool CanAddStepTask { get; set; }
        public string StepTaskCreationOptionalLabel { get; set; }


        private string _StepTaskAddButtonText;
        public string StepTaskAddButtonText
        {
            get { return _StepTaskAddButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultStepTaskAddButtonText : _StepTaskAddButtonText; }
            set { _StepTaskAddButtonText = value; }
        }
        private string _StepTaskCancelButtonText;
        public string StepTaskCancelButtonText
        {
            get { return _StepTaskCancelButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultStepTaskCancelButtonText : _StepTaskCancelButtonText; }
            set { _StepTaskCancelButtonText = value; }
        }


        public bool IsConfidential { get; set; }

        public string Temporary { get; set; }
        public int ServiceTaskTemplateCount { get; set; }
        public int ServicePlusServiceTemplateCount { get; set; }

        public bool EnableAdhocTask { get; set; }
        public bool CanAddAdhocTask { get; set; }
        public string AdhocTaskHeaderMessage { get; set; }

        private string _AdhocTaskHeaderText;
        public string AdhocTaskHeaderText
        {
            get { return _AdhocTaskHeaderText.IsNullOrEmpty() ? Constant.Nts.DefaultAdhocTaskHeader : _AdhocTaskHeaderText; }
            set { _AdhocTaskHeaderText = value; }
        }

        private string _AdhocTaskAddButtonText;
        public string AdhocTaskAddButtonText
        {
            get { return _AdhocTaskAddButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultAdhocTaskAddButtonText : _AdhocTaskAddButtonText; }
            set { _AdhocTaskAddButtonText = value; }
        }

        private string _RemoveStepTaskSuccessMessage;
        public string RemoveStepTaskSuccessMessage
        {
            get { return _RemoveStepTaskSuccessMessage.IsNullOrEmpty() ? "Item has been marked as Not Applicable" : _RemoveStepTaskSuccessMessage; }
            set { _RemoveStepTaskSuccessMessage = value; }
        }



        private string _AdhocTaskCancelButtonText;
        public string AdhocTaskCancelButtonText
        {
            get { return _AdhocTaskCancelButtonText.IsNullOrEmpty() ? Constant.Nts.DefaultAdhocTaskCancelButtonText : _AdhocTaskCancelButtonText; }
            set { _AdhocTaskCancelButtonText = value; }
        }
        public bool IsLatestVersion
        {
            get { return ServiceVersionId == 0; }
        }

        public long? ServiceVersionId { get; set; }
        public long? FocusTaskId { get; set; }
        public string ServiceSharedList { get; set; }

        private string _CancelButtonText;
        public string CancelButtonText
        {
            get { return _CancelButtonText.IsNullOrEmpty() ? "Cancel Service" : _CancelButtonText; }
            set { _CancelButtonText = value; }
        }
        public string PostComment { get; set; }

        public string CSVFileIds { get; set; }

        public FileViewModel SelectedFile { get; set; }

        //[Display(Name = "Change Status OnStepChange")]
        [Display(Name = "ChangeStatusOnStepChange", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public bool? ChangeStatusOnStepChange { get; set; }

        private string _ServiceOwnerText;
        //[Display(Name = "Service Owner Text")]
        [Display(Name = "ServiceOwnerText", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string ServiceOwnerText
        {
            get { return _ServiceOwnerText.IsNullOrEmpty() ? "Requesting on behalf of" : _ServiceOwnerText; }
            set { _ServiceOwnerText = value; }
        }
        //public bool AutoCompleteOnSubmit { get; set; }

        public bool? CreateInBackGround { get; set; }
        public bool IsAssigneeNull { get; set; }
        public bool SimpleCard { get; set; }
        public bool? IsCreatingInBackGround { get; set; }
        public double? PercentageCompleted { get; set; }
        public string XmlData { get; set; }

        public long? ReminderId { get; set; }
        public long? RecurrenceId { get; set; }

        public ServiceNoPrefixEnum? ServiceNoPrefix { get; set; }
        public string Base64ImageOpCheckList { get; set; }
        public string ModulesList { get; set; }
        public string DueDateDisplay
        {
            get
            {
                if(ServiceStatusName == "Completed" || ServiceStatusName == "Canceled")
                {
                    var d = Humanizer.DateHumanizeExtensions.Humanize(ServiceStatusName == "Completed" ? CompletionDate : CanceledDate);
                    return d;
                }
                else
                {
                    var d = Humanizer.DateHumanizeExtensions.Humanize(DueDate);
                    return d;
                }

            }
        }
        public string OwnerFirstLetter
        {
            get { return OwnerUserUserName != null ? OwnerUserUserName.First().ToString() : LoggedInUserName.First().ToString(); }
        }
        public string EventName { get; set; }
        public bool? IsSystemAutoService { get; set; }
        public DiagramViewModel StepTaskSwimLane { get; set; }
        public string StatusClass
        {
            get
            {
                switch (TemplateAction)
                {
                    case NtsActionEnum.Draft:
                        return "label-info";
                    case NtsActionEnum.Submit:
                        return "label-warning";
                    case NtsActionEnum.Complete:
                        return "label-success";
                    case NtsActionEnum.Cancel:
                        return "label-default";
                    case NtsActionEnum.Overdue:
                        return "label-danger";
                    default:
                        return "label-default";
                }
            }
        }
        public string TaskStatusClass
        {
            get
            {
                switch (TaskTemplateAction)
                {
                    case NtsActionEnum.Draft:
                        return "label-info";
                    case NtsActionEnum.Submit:
                        return "label-warning";
                    case NtsActionEnum.Complete:
                        return "label-success";
                    case NtsActionEnum.Cancel:
                        return "label-default";
                    case NtsActionEnum.Overdue:
                        return "label-danger";
                    default:
                        return "label-default";
                }
            }
        }

        public long? AttachmentCount { get; set; }
        public long? CommentCount { get; set; }
        public long? SharedCount { get; set; }
        public long? NotificationCount { get; set; }

        public long? ReferenceServiceId1 { get; set; }
        public long? ReferenceServiceId2 { get; set; }

        public long? ActualFocusId { get; set; }
        public long? PrevServiceId { get; set; }
        public long? NextServiceId { get; set; }
        public long? PrevTaskId { get; set; }
        public long? NextTaskId { get; set; }

        public long? RelatedNoteCount { get; set; }
        public string AssessmentUrl { get; set; }
        public NtsActionEnum? TaskTemplateAction { get; set; }
        public long? DocumentId { get; set; }
    }


    public class UDFViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
    }
}

