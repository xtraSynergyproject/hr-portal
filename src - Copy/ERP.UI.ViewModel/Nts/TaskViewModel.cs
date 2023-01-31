using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class TaskViewModel : TemplateViewModelBase
    {
        [Display(Name = "TaskNo", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string TaskNo { get; set; }

        /// <summary>
        /// This is used in versioning
        /// </summary>
        public long? TaskId { get; set; }

        //[Display(Name = "Task Owner")]
        [Display(Name = "OwnerUserId", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public long? OwnerUserId { get; set; }
        //[Display(Name = "Task Owner")]
        [Display(Name = "OwnerDisplayName", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string OwnerDisplayName { get; set; }
        //[Display(Name = "Task Owner")]
        [Display(Name = "OwnerUserUserName", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string OwnerUserUserName { get; set; }

        //[Display(Name = "HolderDisplayName", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string HolderDisplayName { get; set; }



        public string TaskName { get; set; }
        //[Display(Name = "Task Status")]
        [Display(Name = "TaskStatusCode", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string TaskStatusCode { get; set; }
        [Display(Name = "Task Status")]
        //[Display(Name = "TaskStatusName", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string TaskStatusName { get; set; }
        public ServiceViewModel ServiceViewModel { get; set; }
        private string _TaskOwnerText;
        [Display(Name = "ServiceOwnerText", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string TaskOwnerText
        {
            get { return _TaskOwnerText.IsNullOrEmpty() ? "Task Owner" : _TaskOwnerText; }
            set { _TaskOwnerText = value; }
        }
        private string _OwnerTeamText;
        public string OwnerTeamText
        {
            get { return _OwnerTeamText.IsNullOrEmpty() ? "Requesting on behalf of Team" : _OwnerTeamText; }
            set { _OwnerTeamText = value; }
        }

        //[Display(Name = "Created By")]
        [Display(Name = "OwnerName", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string OwnerName { get; set; }

        public FieldDisplayModeEnum DisplayMode { get; set; }
        public NtsServiceTaskTypeEnum? ServiceTaskType { get; set; }

        //[Display(Name = "Assign To Type")]
        [Display(Name = "AssignToType", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public AssignToTypeEnum? AssignToType { get; set; }

        public AssignedQueryTypeEnum? AssignedQueryType { get; set; }
        public string AssignedByQuery { get; set; }

        //[Display(Name = "Assign To Hierarchy")]
        [Display(Name = "AssignedToHierarchyId", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public long? AssignedToHierarchyId { get; set; }
        //[Display(Name = "Assign To Hierarchy Level")]
        [Display(Name = "AssignedToHierarchyLevelNo", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public int? AssignedToHierarchyLevelNo { get; set; }

        public long? ServiceTaskTemplateId { get; set; }

        //[Display(Name = "Assignee")]
        [Display(Name = "AssignedTo", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public long? AssignedTo { get; set; }
        public string AssignedToEmail { get; set; }
        public string CC { get; set; }
        public string CCIds { get; set; }
        public string TeamName { get; set; }
        public bool IsAssignedInTemplate { get; set; }
        public string AssigneeDisplayName { get; set; }
        [Display(Name = "Subject", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string Subject { get; set; }
        public string Description { get; set; }


        public string ServiceName { get; set; }



        public long? ServiceId { get; set; }
        public long? ServiceTemplateId { get; set; }
        public bool IsConfidential { get; set; }

        public bool CanViewServiceReference { get; set; }

        private string _ServiceReferenceText;
        public string ServiceReferenceText
        {
            get { return _ServiceReferenceText.IsNullOrEmpty() ? Constant.Nts.DefaultServiceReferenceText : _ServiceReferenceText; }
            set { _ServiceReferenceText = value; }
        }

        //[Display(Name = "Assignee To")]
        [Display(Name = "AssignedUserUserName", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string AssignedUserUserName { get; set; }

        public bool IsTaskAutoComplete { get; set; }
        public bool EnableTaskAutoComplete { get; set; }

        public bool IsTaskAutoCompleteIfSameAssignee { get; set; }

        public bool AllowPastStartDate { get; set; }

        //[Display(Name = "Assignee To")]
        [Display(Name = "AssignedUserUserNameWithEmail", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string AssignedUserUserNameWithEmail { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Start Date")]
        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public DateTime? StartDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Due Date")]
        [Display(Name = "DueDate", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public DateTime? DueDate { get; set; }

        //[Display(Name = "Reminder Date")]
        [Display(Name = "ReminderDate", ResourceType = typeof(ERP.Translation.Nts.Task))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? ReminderDate { get; set; }

        public long? ReminderId { get; set; }
        public long? RecurreceId { get; set; }

        //[Display(Name = "Completed Date")]
        [Display(Name = "CompletionDate", ResourceType = typeof(ERP.Translation.Nts.Task))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? CompletionDate { get; set; }
 
        public bool HideIfDraft { get; set; }
        public bool IsHiiden
        {
            get { return HideIfDraft && TemplateAction == NtsActionEnum.Draft; }
        }

        public bool IsLatestVersion
        {
            get { return TaskVersionId == 0; }
        }

        public long? TaskVersionId { get; set; }
        public long? ServiceVersionId { get; set; }
        public long? ParentTaskId { get; set; }

        private string _CancelButtonText;
        public string CancelButtonText
        {
            get { return _CancelButtonText.IsNullOrEmpty() ? "Cancel Task" : _CancelButtonText; }
            set { _CancelButtonText = value; }
        }

#pragma warning disable CS0108 // 'TaskViewModel.ReminderDescription' hides inherited member 'TemplateViewModelBase.ReminderDescription'. Use the new keyword if hiding was intended.
        public string ReminderDescription { get; set; }
#pragma warning restore CS0108 // 'TaskViewModel.ReminderDescription' hides inherited member 'TemplateViewModelBase.ReminderDescription'. Use the new keyword if hiding was intended.
        public string ScheduleDescription { get; set; }
        public string TaskSharedList { get; set; }

        public string PostComment { get; set; }


#pragma warning disable CS0108 // 'TaskViewModel.TeamId' hides inherited member 'TemplateViewModelBase.TeamId'. Use the new keyword if hiding was intended.
        public long? TeamId { get; set; }
#pragma warning restore CS0108 // 'TaskViewModel.TeamId' hides inherited member 'TemplateViewModelBase.TeamId'. Use the new keyword if hiding was intended.
        public string CSVFileIds { get; set; }

        public FileViewModel SelectedFile { get; set; }
        public bool? ServiceOwnerActAsStepTaskAssignee { get; set; }
        public long? ServiceOwnerUserId { get; set; }
        public long? CompletedByUserId { get; set; }
        public long? RejectedByUserId { get; set; }

        public long? ServiceReferenceId { get; set; }
        [Display(Name = "ServiceNo", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string ServiceNo { get; set; }
        [Display(Name = "ServiceOwner", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string ServiceOwner { get; set; }
        public NtsLockStatusEnum? LockStatus { get; set; }
        public bool IsLockVisible { get; set; }
        public bool SimpleCard { get; set; }
        public bool IsReleaseVisible { get; set; }
        public bool IsTaskTeamOwner { get; set; }
        public List<IdNameViewModel> TeamMembers { get; set; }
        public string EventName { get; set; }
       
        public string DueDateDisplay
        {
            get
            {
                if (TaskStatusName == "Completed" || TaskStatusName == "Canceled")
                {
                    var d = Humanizer.DateHumanizeExtensions.Humanize(TaskStatusName == "Completed" ? CompletionDate : CanceledDate);
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
            get { return (AssigneeDisplayName != null && AssigneeDisplayName != "") ? AssigneeDisplayName.First().ToString() : LoggedInUserName.First().ToString(); }
        }

        public string AvatarName
        {
            get { return (AssigneeDisplayName != null && AssigneeDisplayName != "") ? AssigneeDisplayName : LoggedInUserName; }
        }

        public string TaskOwnerFirstLetter
        {
            get { return (ServiceOwner != null && ServiceOwner != "") ? ServiceOwner.First().ToString() : ((OwnerName != null && OwnerName != "") ? OwnerName.First().ToString() : LoggedInUserName.First().ToString()); }
        }

        public string GetTaskOwner
        {
            get { return (ServiceOwner != null && ServiceOwner != "") ? ServiceOwner.ToString() : ((OwnerName != null && OwnerName != "") ? OwnerName : LoggedInUserName); }
        }

        public string TaskStatus { get; set; }
        public string TaskDueDateDisplay { get; set; }
        public string TaskStartDateDisplay { get; set; }
        public string ServiceStatusName { get; set; }
        public bool? IsSystemAutoTask { get; set; }
       

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
#pragma warning disable CS0108 // 'TaskViewModel.PlanDate' hides inherited member 'TemplateViewModelBase.PlanDate'. Use the new keyword if hiding was intended.
        public DateTime? PlanDate { get; set; }
#pragma warning restore CS0108 // 'TaskViewModel.PlanDate' hides inherited member 'TemplateViewModelBase.PlanDate'. Use the new keyword if hiding was intended.

        public string SubjectDisplay
        {
            get
            {
                return Subject.LimitTo(30);

            }
        }
        public string AssigneeDisplay
        {
            get
            {
                return AssigneeDisplayName.LimitTo(30);

            }
        }

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

        public string StatusStyle
        {
            get
            {
                switch (TemplateAction)
                {
                    case NtsActionEnum.Draft:
                        return "Color:#5bc0de";
                    case NtsActionEnum.Submit:
                        return "Color:#f0ad4e";
                    case NtsActionEnum.Complete:
                        return "Color:#5cb85c";
                    case NtsActionEnum.Cancel:
                        return "Color:#999";
                    case NtsActionEnum.Overdue:
                        return "Color:#d9534f";
                    default:
                        return "Color:#999";
                }
            }
        }

        public bool? OverrideEnableTeamAsOwner { get; set; }
        public bool? OverrideTeamAsOwnerMandatory { get; set; }
        public List<long> ParentIds { get; set; }

        public long? AttachmentCount { get; set; }
        public long? CommentCount { get; set; }
        public long? SharedCount { get; set; }
        public long? NotificationCount { get; set; }
        public long? logTimeCount { get; set; }
        public long? PrevServiceId { get; set; }
        public long? NextServiceId { get; set; }
        public long? PrevTaskId { get; set; }
        public long? NextTaskId { get; set; }
        public long? RecurrenceId { get; set; }
        public long? NtsReferenceId { get; set; }
        public NtsTypeEnum? NtsReferenceType { get; set; }

        public long? ParentReferenceId { get; set; }
        public ReferenceTypeEnum? ParentReferenceTypeCode { get; set; }
        public NodeEnum? ParentReferenceNode { get; set; }
        public NtsTypeEnum? SourceType { get; set; }
        public long? SourceId { get; set; }

        public string[] CCName { get; set; }
        public string CCDisplay
        {
            get
            {
                if (CCName!=null && CCName.Length > 0)
                    return string.Join(",", CCName);
                else
                    return "";
            }
        }
        public long? DocumentId { get; set; }
        public string DocumentName { get; set; }
        public bool? EmailNotSent { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        public long? ProjectId { get; set; }

        [Display(Name = "Task Group")]
        public string TaskGroup { get; set; }

        [Display(Name = "Assignee Name")]
        public string AssigneeName { get; set; }

        public long[] AssigneeId { get; set; }

        [Display(Name = "Work Start Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime[] WSDate { get; set; }
       // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public string ModifiedDate { get; set; }
    }

}


