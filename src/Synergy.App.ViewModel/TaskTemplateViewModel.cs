using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class TaskTemplateViewModel : TaskTemplate
    {
        public string JsonCopy { get; set; }
        public string Json { get; set; }
        public string DataJson { get; set; }
        public PageViewModel Page { get; set; }
        public string PageId { get; set; }
        public string RecordId { get; set; }
        public string PortalName { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateDisplayName { get; set; }
        public long TaskCount { get; set; }
        public bool SetUdfValue { get; set; }
        public string TaskEventId { get; set; }

        public TaskIndexPageTemplateViewModel TaskIndexPageTemplateDetails { get; set; }
        public NotificationTemplateViewModel NotificationTemplate { get; set; }
        public TableMetadata UdfTableMetadata { get; set; }
        public ExportTemplateViewModel NoteUdfTemplate { get; set; }
        public TemplateViewModel TemplateViewModel { get; set; }
        public TaskTemplateViewModel TaskTemplateVM { get; set; }
        public DataTable TaskTable { get; set; }
        public List<ColumnMetadataViewModel> ColumnList { get; set; }


        public string ActiveUserId { get; set; }
        public string TaskNo { get; set; }
        public string TaskId { get; set; }
        public string TaskTableId { get; set; }
        public string TaskNoteTableId { get; set; }


        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? StartDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? DueDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public TimeSpan? TaskSLA { get; set; }
        public double? TaskSLASeconds { get; set; }
        public double? SLASeconds { get; set; }
        public TimeSpan ActualSLA { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? ReminderDate { get; set; }
        public DateTime? PlanDate { get; set; }

        public string TaskStatusBgCss
        {
            get
            {
                switch (TaskStatusCode)
                {
                    case "TASK_STATUS_DRAFT":
                        return "bg-draft";
                    case "TASK_STATUS_INPROGRESS":
                        return "bg-inprogress";
                    case "TASK_STATUS_OVERDUE":
                        return "bg-overdue";
                    case "TASK_STATUS_COMPLETE":
                        return "bg-completed";
                    case "TASK_STATUS_REJECT":
                        return "bg-rejected";
                    case "TASK_STATUS_CANCEL":
                        return "bg-canceled";
                    default:
                        return "bg-default";
                }
            }
        }
        public string TaskStatusFontCss
        {
            get
            {
                switch (TaskStatusCode)
                {
                    case "TASK_STATUS_DRAFT":
                        return "text-draft";
                    case "TASK_STATUS_INPROGRESS":
                        return "text-inprogress";
                    case "TASK_STATUS_OVERDUE":
                        return "text-overdue";
                    case "TASK_STATUS_COMPLETE":
                        return "text-completed";
                    case "TASK_STATUS_REJECT":
                        return "text-rejected";
                    case "TASK_STATUS_CANCEL":
                        return "text-canceled";
                    default:
                        return "text-default";
                }
            }
        }


        public string TaskStatusId { get; set; }
        public string TableMetadataId { get; set; }
        public string UdfTableMetadataId { get; set; }
        public string UdfNoteId { get; set; }
        public string UdfNoteTableId { get; set; }
        public string TaskStatusCode { get; set; }
        public string TaskStatusName { get; set; }

        public string TaskActionId { get; set; }
        public string TaskActionCode { get; set; }
        public string TaskActionName { get; set; }


        public string TaskPriorityId { get; set; }
        public string TaskPriorityCode { get; set; }
        public string TaskPriorityName { get; set; }


        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? SubmittedDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? CompletedDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? RejectedDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? CanceledDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? ReturnedDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? ReopenedDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? ClosedDate { get; set; }
        public string CloseComment { get; set; }

        public string RejectionReason { get; set; }
        public string ReturnReason { get; set; }
        public string ReopenReason { get; set; }
        public string CancelReason { get; set; }

        public string CompleteReason { get; set; }
        public string DelegateReason { get; set; }
        public string RequestedByUserId { get; set; }
        public string RequestedByUserName { get; set; }
        public string RequestedByUserEmail { get; set; }
        public string RequestedByUserPhotoId { get; set; }

        public string OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }
        public string OwnerUserEmail { get; set; }
        public string OwnerUserPhotoId { get; set; }

        public string TaskOwnerTypeId { get; set; }


        public NtsActiveUserTypeEnum ActiveUserType { get; set; }
        public string AssignedToTypeId { get; set; }
        public string AssignedToTypeCode { get; set; }
        public string AssignedToTypeName { get; set; }

        public string AssignedToUserId { get; set; }
        public string SharedByUserId { get; set; }
        public string SharedWithUserId { get; set; }
        public string AssignedToUserName { get; set; }
        public string AssignedToEmail { get; set; }
        public string SharedListText
        {
            get
            {
                if (SharedList == null || SharedList.Count <= 0)
                {
                    return string.Empty;
                }
                return string.Join(';', SharedList.Select(x => $"{x.UserName}<{x.Email}>").ToList());
            }
        }
        public List<UserViewModel> SharedList { get; set; }
        public string AssignedToUserEmail { get; set; }



        public string AssignedToTeamId { get; set; }
        public string AssignedToTeamName { get; set; }

        public string AssignedToTeamUserId { get; set; }
        public string AssignedToTeamUserName { get; set; }

        public string AssignedToHierarchyMasterId { get; set; }
        public string AssignedToHierarchyMasterName { get; set; }

        public int? AssignedToHierarchyMasterLevelId { get; set; }
        public string AssignedToHierarchyMasterLevelName { get; set; }


        public bool IsStepTaskAutoCompleteIfSameAssignee { get; set; }

        public string LockStatusId { get; set; }
        public LOV LockStatus { get; set; }


        public string ParentTaskId { get; set; }

        public string ParentServiceId { get; set; }
        public ServiceViewModel ParentService { get; set; }
        public NtsViewTypeEnum? ServiceViewType { get; set; }
        public TemplateViewModel ServiceBaseTemplate { get; set; }
        public bool EnableRuntimeWorkflow { get; set; }
        public string RuntimeWorkflowButtonText { get; set; }




        public bool IncludeReadonlyData { get; set; }
        public bool IsDraftButtonVisible
        {
            get
            {
                return EnableSaveAsDraft &&
                    TaskStatusCode == "TASK_STATUS_DRAFT" &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester);
            }
        }
        public bool IsRutimeWorkflowButtonVisible
        {
            get
            {
                return EnableRuntimeWorkflow &&
                    TaskStatusCode == "TASK_STATUS_DRAFT" &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester);
            }
        }
        public bool IsAcceptButtonVisible
        {
            get
            {
                return (TaskStatusCode == "TASK_STATUS_INPROGRESS" || TaskStatusCode == "TASK_STATUS_OVERDUE") &&
                    (TaskActionCode == "TASK_ACTION_AWAIT_ASSIGNEE_ACCEPT") &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Assignee);
            }
        }
        public bool IsSubmitButtonVisible
        {
            get
            {
                return TaskStatusCode == "TASK_STATUS_DRAFT" &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester
                    || OwnerUserId == AssignedToUserId || RequestedByUserId == AssignedToUserId);
            }
        }
        public bool IsInEditMode
        {
            get
            {
                return IsSubmitButtonVisible || IsVersioning;
            }
        }
        //public bool CanEdit
        //{
        //    get
        //    {

        //        return (TaskStatusCode == "TASK_STATUS_DRAFT" &&
        //            (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester
        //            || OwnerUserId == AssignedToUserId || RequestedByUserId == AssignedToUserId)) || IsVersioning;
        //    }
        //}
        public bool IsVersioningButtonVisible
        {
            get
            {
                return (TaskStatusCode == "TASK_STATUS_INPROGRESS" || TaskStatusCode == "TASK_STATUS_OVERDUE" || TaskStatusCode == "TASK_STATUS_COMPLETE")
                   && (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester || (AssignedToUserId == OwnerUserId))
                   && IsVersioning;
            }
        }
        public bool IsReplyButtonVisible
        {
            get
            {
                return TaskStatusCode != "TASK_STATUS_DRAFT" && TaskStatusCode != "TASK_STATUS_CLOSE" &&
                    (DataAction == DataActionEnum.View || DataAction == DataActionEnum.Edit);
            }
        }
        public bool IsAddCommentEnabled
        {
            get
            {
                return (TaskStatusCode != "TASK_STATUS_CLOSE") &&
                    (ActiveUserType != NtsActiveUserTypeEnum.None);
            }
        }
        public bool IsAddAttachmentEnabled
        {
            get
            {
                return (TaskStatusCode != "TASK_STATUS_CLOSE") &&
                    (ActiveUserType != NtsActiveUserTypeEnum.None);
            }
        }
        public bool IsSharingEnabled
        {
            get
            {
                return (TaskStatusCode != "TASK_STATUS_CLOSE") &&
                    (ActiveUserType != NtsActiveUserTypeEnum.None);
            }
        }

        public bool IsCompleteButtonVisible
        {
            get
            {
                return (TaskStatusCode == "TASK_STATUS_INPROGRESS" || TaskStatusCode == "TASK_STATUS_OVERDUE") &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Assignee);
            }
        }
        public bool CanComplete
        {
            get
            {
                return (TaskStatusCode == "TASK_STATUS_INPROGRESS" || TaskStatusCode == "TASK_STATUS_OVERDUE") &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Assignee);
            }
        }
        public bool IsSaveChangesVisible
        {
            get
            {
                return (TaskStatusCode == "TASK_STATUS_INPROGRESS" || TaskStatusCode == "TASK_STATUS_OVERDUE") &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Assignee);
            }
        }
        public bool IsReOpenButtonVisible
        {
            get
            {
                return EnableReOpenButton &&
                    (TaskStatusCode == "TASK_STATUS_COMPLETE" || TaskStatusCode == "TASK_STATUS_REJECT") &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester);
            }
        }
        public bool IsTimeEntryButtonVisible
        {
            get
            {
                return EnableTimeEntry &&
                      (TaskStatusCode == "TASK_STATUS_INPROGRESS" || TaskStatusCode == "TASK_STATUS_OVERDUE") &&
                      (ActiveUserType == NtsActiveUserTypeEnum.Assignee);
            }
        }
        public bool IsRejectButtonVisible
        {
            get
            {
                return EnableRejectButton &&
                    (TaskStatusCode == "TASK_STATUS_INPROGRESS" || TaskStatusCode == "TASK_STATUS_OVERDUE") &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Assignee);
            }
        }
        public bool IsReturnButtonVisible
        {
            get
            {
                return EnableReturnTask &&
                    (TaskStatusCode == "TASK_STATUS_INPROGRESS" || TaskStatusCode == "TASK_STATUS_OVERDUE") &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Assignee);
            }
        }
        public bool IsCloseButtonVisible
        {
            get
            {
                return (TaskStatusCode == "TASK_STATUS_COMPLETE" || TaskStatusCode == "TASK_STATUS_REJECT") &&
                        (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester);
            }
        }
        public bool EnableStartButton
        {
            get
            {
                return (TaskStatusCode == "TASK_STATUS_PLANNED" || TaskStatusCode == "TASK_STATUS_PLANNED_OVERDUE") &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Assignee) && EnablePlanning && ActualStartDate == null;
            }
        }
        public bool IsLockVisible { get; set; }
        public bool IsReleaseVisible { get; set; }
        public bool IsTaskTeamOwner { get; set; }
        public List<TeamViewModel> TeamMembers { get; set; }
        public long SharedCount { get; set; }
        public long AttachmentCount { get; set; }
        public long NotificationCount { get; set; }
        public long CommentCount { get; set; }
        public Dictionary<string, string> Prms { get; set; }
        public Dictionary<string, string> Udfs { get; set; }
        public Dictionary<string, bool> ReadoOnlyUdfs { get; set; }
        public Dictionary<string, bool> HiddenUdfs { get; set; }
        public bool CanChangeOwner { get; set; }

        public long? TaskSequenceOrder { get; set; }
        public string TaskDescription { get; set; }
        public string TaskSubject { get; set; }
        public bool IncludeSharedList { get; set; }
        public string CustomUrl { get; set; }
        public string ReturnUrl { get; set; }
        public LayoutModeEnum? LayoutMode { get; set; }
        public string PopupCallbackMethod { get; set; }
        public List<TaskViewModel> StepTasksList { get; set; }
        public bool HideStepTaskDetails { get; set; }
        public bool IsVersioning { get; set; }
        public string StepTaskComponentId { get; set; }
        public string ChartItems { get; set; }
        public string ReferenceId { get; set; }
        public ReferenceTypeEnum ReferenceType { get; set; }
        public string LogId { get; set; }
        public bool IsLogRecord { get; set; }

        public string PostComment { get; set; }
        public NtsViewTypeEnum? ViewType { get; set; }
        public NtsViewTypeEnum? ViewMode { get; set; }
        public string ServicePlusId { get; set; }
        public string NotePlusId { get; set; }
        public string TaskPlusId { get; set; }
        public string ParentNoteId { get; set; }

        public bool EnableChangingNextTaskAssignee { get; set; }
        public string ChangingNextTaskAssigneeTitle { get; set; }
        public bool EnableReturnTask { get; set; }
        public string ReturnTaskTitle { get; set; }
        public string ReturnTaskButtonText { get; set; }
        public string NextTaskAttachmentId { get; set; }
        public bool EnableNextTaskAttachment { get; set; }
        public bool DisableNextTaskTeamChange { get; set; }

        public string NextTaskAssignedToTypeId { get; set; }
        public string NextTaskAssignedToTypeCode { get; set; }
        public string NextTaskAssignedToUserId { get; set; }
        public string NextTaskAssignedToTeamId { get; set; }
        public string NextTaskAssignedToTeamUserId { get; set; }
        public string NextTaskAssignedToHierarchyMasterId { get; set; }
        public int? NextTaskAssignedToHierarchyMasterLevelId { get; set; }

        public bool IsReturned { get; set; }
        public bool IsReopened { get; set; }
        public bool EnableDynamicStepTaskSelection { get; set; }
        public string NextStepTaskComponentId { get; set; }
        public string TableName { get; set; }
        public bool AllReadOnly { get; set; }
        public string ServiceStatusCode { get; set; }
        public string ServiceTemplateCodes { get; set; }
        public string PortalIds { get; set; }
        public string TriggeredByReferenceId { get; set; }
        public ReferenceTypeEnum? TriggeredByReferenceType { get; set; }


    }
}
