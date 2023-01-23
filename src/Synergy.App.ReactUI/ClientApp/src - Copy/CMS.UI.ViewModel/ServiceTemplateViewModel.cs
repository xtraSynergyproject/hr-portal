using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace CMS.UI.ViewModel
{
    public class ServiceTemplateViewModel : ServiceTemplate
    {
        public bool DisableReopen { get; set; }
        public string JsonCopy { get; set; }
        public string Json { get; set; }
        public string DataJson { get; set; }
        public PageViewModel Page { get; set; }
        public string PageId { get; set; }
        public string RecordId { get; set; }
        public string PortalName { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateDisplayName { get; set; }
        public string ServiceEventId { get; set; }

        public TemplateViewModel TemplateViewModel { get; set; }
        public DataTable ServiceTable { get; set; }
        public List<ColumnMetadataViewModel> ColumnList { get; set; }
        public string TableMetadataId { get; set; }
        public string UdfTableMetadataId { get; set; }
        public string UdfNoteId { get; set; }
        public string UdfNoteTableId { get; set; }
        public string ServiceNo { get; set; }
        public string ServiceSubject { get; set; }
        public string ServiceDescription { get; set; }
        public string ModuleId { get; set; }

        public double? SLASeconds { get; set; }
        //        public string TemplateCode { get; set; }
        //        public string Description { get; set; }
        //        public string ServiceNo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public TimeSpan? ServiceSLA { get; set; }
        public double? ServiceSLAMinutes { get; set; }
        //public TimeSpan SLA { get; set; }
        //        public TimeSpan? ServiceSLA { get; set; }
        //        public double? ServiceSLAMinutes { get; set; }
        public TimeSpan ActualSLA { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? ReminderDate { get; set; }
        public string ServiceStatusBgCss
        {
            get
            {
                switch (ServiceStatusCode)
                {
                    case "SERVICE_STATUS_DRAFT":
                        return "bg-draft";
                    case "SERVICE_STATUS_INPROGRESS":
                        return "bg-inprogress";
                    case "SERVICE_STATUS_OVERDUE":
                        return "bg-overdue";
                    case "SERVICE_STATUS_COMPLETE":
                        return "bg-completed";
                    case "SERVICE_STATUS_REJECT":
                        return "bg-rejected";
                    case "SERVICE_STATUS_CANCEL":
                        return "bg-canceled";
                    case "SERVICE_STATUS_CLOSE":
                        return "bg-canceled";
                    default:
                        return "bg-default";
                }
            }
        }
        public string ServiceStatusFontCss
        {
            get
            {
                switch (ServiceStatusCode)
                {
                    case "SERVICE_STATUS_DRAFT":
                        return "text-draft";
                    case "SERVICE_STATUS_INPROGRESS":
                        return "text-inprogress";
                    case "SERVICE_STATUS_OVERDUE":
                        return "text-overdue";
                    case "SERVICE_STATUS_COMPLETE":
                        return "text-completed";
                    case "SERVICE_STATUS_REJECT":
                        return "text-rejected";
                    case "SERVICE_STATUS_CANCEL":
                        return "text-canceled";
                    case "SERVICE_STATUS_CLOSE":
                        return "text-canceled";
                    default:
                        return "text-default";
                }
            }
        }

        public string ServiceStatusId { get; set; }
        public string ServiceStatusCode { get; set; }
        public string ServiceTableId { get; set; }
        public string ServiceStatusName { get; set; }

        public string ServiceActionId { get; set; }
        public string ServiceActionCode { get; set; }
        public string ServiceActionName { get; set; }


        public string ServicePriorityId { get; set; }
        public string ServicePriorityCode { get; set; }
        public string ServicePriorityName { get; set; }

        public DateTime? SubmittedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? RejectedDate { get; set; }
        public DateTime? CanceledDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public DateTime? ReopenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }



        public string RejectionReason { get; set; }
        public string ReturnReason { get; set; }
        public string ReopenReason { get; set; }
        public string CancelReason { get; set; }
        public string CloseComment { get; set; }
        public double? UserRating { get; set; }
        public string CompleteReason { get; set; }
        public string DelegateReason { get; set; }
        public string RequestedByUserId { get; set; }
        public string RequestedByUserName { get; set; }
        public string RequestedByUserEmail { get; set; }
        public string RequestedByUserPhotoId { get; set; }
        public User RequestedByUser { get; set; }

        public string OwnerUserId { get; set; }
        public User OwnerUser { get; set; }
        public string OwnerUserName { get; set; }
        public string OwnerUserEmail { get; set; }
        public string OwnerUserPhotoId { get; set; }
        public string ServiceOwnerTypeId { get; set; }
        //public LOV ServiceOwnerType { get; set; }


        public NtsActiveUserTypeEnum ActiveUserType { get; set; }

        public string AssignedToHierarchyMasterId { get; set; }
        public string AssignedToHierarchyMasterName { get; set; }

        public int? AssignedToHierarchyMasterLevelId { get; set; }
        public string AssignedToHierarchyMasterLevelName { get; set; }

        public bool IsStepTaskAutoCompleteIfSameAssignee { get; set; }
        public bool SetUdfValue { get; set; }

        public bool IncludeReadonlyData { get; set; }

        public string ParentServiceId { get; set; }
        public Dictionary<string, string> Prms { get; set; }
        public Dictionary<string, string> Udfs { get; set; }
        public Dictionary<string, bool> ReadoOnlyUdfs { get; set; }
        public string ActiveUserId { get; set; }
        public string ServiceId { get; set; }

        public bool IsDraftButtonVisible
        {
            get
            {
                return EnableSaveAsDraft &&
                    ServiceStatusCode == "SERVICE_STATUS_DRAFT" &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester) && DataAction != DataActionEnum.View;
            }
        }

        public bool IsSubmitButtonVisible
        {
            get
            {
                return (ServiceStatusCode == "SERVICE_STATUS_DRAFT")
                    && (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester) && DataAction != DataActionEnum.View;
            }
        }
        public bool IsInEditMode
        {
            get
            {
                return IsSubmitButtonVisible || IsVersioning;
            }
        }
        public bool IsReopenButtonVisible
        {
            get
            {
                return (ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || ServiceStatusCode == "SERVICE_STATUS_OVERDUE" || ServiceStatusCode == "SERVICE_STATUS_COMPLETE" || ServiceStatusCode == "SERVICE_STATUS_CANCEL")
                    && (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester)
                    && IsVersioning;
            }
        }
        public bool IsVersioningButtonVisible
        {
            get
            {
                return (ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || ServiceStatusCode == "SERVICE_STATUS_OVERDUE" || ServiceStatusCode == "SERVICE_STATUS_COMPLETE" || ServiceStatusCode == "SERVICE_STATUS_CANCEL")
                    && (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester)
                    ;
            }
        }
        public bool IsEditButtonVisible
        {
            get
            {
                return (ServiceStatusCode == "SERVICE_STATUS_CANCEL")
                   && (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester);
            }
        }
        public bool IsCancelButtonVisible
        {
            get
            {
                return (ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || ServiceStatusCode == "SERVICE_STATUS_OVERDUE") &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester);
            }
        }
        public bool IsReplyButtonVisible
        {
            get
            {
                return ServiceStatusCode != "SERVICE_STATUS_CLOSE" &&
                    (DataAction == DataActionEnum.View || DataAction == DataActionEnum.Edit);
            }
        }
        public bool IsCompleteButtonVisible
        {
            get
            {
                return (ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || ServiceStatusCode == "SERVICE_STATUS_OVERDUE") &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester);
            }
        }
        public bool IsRejectButtonVisible
        {
            get
            {
                return EnableCancelButton &&
                    (ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || ServiceStatusCode == "SERVICE_STATUS_OVERDUE") &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Owner);
            }
        }
        public bool IsCloseButtonVisible
        {
            get
            {
                return (ServiceStatusCode == "SERVICE_STATUS_COMPLETE" || ServiceStatusCode == "SERVICE_STATUS_REJECT") &&
                        (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester);
            }
        }


        public long SharedCount { get; set; }
        public long AttachmentCount { get; set; }
        public long NotificationCount { get; set; }
        public long CommentCount { get; set; }
        public string ServiceTemplateId { get; set; }
        public bool IsAddCommentEnabled
        {
            get
            {
                return (ServiceStatusCode != "SERVICE_STATUS_CLOSE") &&
                    (ActiveUserType != NtsActiveUserTypeEnum.None);
            }
        }
        public bool IsAddAttachmentEnabled
        {
            get
            {
                return (ServiceStatusCode != "SERVICE_STATUS_CLOSE") &&
                    (ActiveUserType != NtsActiveUserTypeEnum.None);
            }
        }
        public bool IsSharingEnabled
        {
            get
            {
                return (ServiceStatusCode != "SERVICE_STATUS_CLOSE") &&
                    (ActiveUserType != NtsActiveUserTypeEnum.None);
            }
        }
        public bool IncludeSharedList { get; set; }
        public string SharedListText
        {
            get
            {
                if (SharedList == null || SharedList.Count <= 0)
                {
                    return string.Empty;
                }
                return string.Join(';', SharedList.Select(x => $"{x.Name}<{x.Email}>").ToList());
            }
        }
        public List<UserViewModel> SharedList { get; set; }
        public string SharedByUserId { get; set; }
        public string SharedWithUserId { get; set; }
        public long ServiceCount { get; set; }
        public string CustomUrl { get; set; }
        public string ReturnUrl { get; set; }
        public LayoutModeEnum? LayoutMode { get; set; }
        public string PopupCallbackMethod { get; set; }
        public NtsViewTypeEnum? ViewType { get; set; }
        public NtsViewTypeEnum? ViewMode { get; set; }
        public ServiceIndexPageTemplateViewModel ServiceIndexPageTemplateDetails { get; set; }
        public NotificationTemplateViewModel NotificationTemplate { get; set; }
        public ProcessDesignViewModel ProcessDesign { get; set; }
        public TableMetadata UdfTableMetadata { get; set; }
        public ExportTemplateViewModel NoteUdfTemplate { get; set; }
        public string ServiceNoTextWithDefault
        {
            get
            {
                return ServiceNoText.Coalesce("Service No");
            }
        }
        public bool HideStepTaskDetails { get; set; }
        public List<TaskViewModel> StepTasksList { get; set; }

        public bool IsVersioning { get; set; }
        public string AdhocTaskTemplateId { get; set; }
        public string ChartItems { get; set; }
        public ServiceTemplateViewModel ServiceTemplateVM { get; set; }
        public string PersonId { get; set; }
        public string StageName { get; set; }
        public string StageId { get; set; }
        public string WorkflowStepName { get; set; }
        public string WorkflowStepId { get; set; }
        public string ReferenceId { get; set; }
        public ReferenceTypeEnum ReferenceType { get; set; }
        public string LogId { get; set; }
        public bool IsLogRecord { get; set; }
        public string PostComment { get; set; }
        public List<NtsViewModel> BookItems { get; set; }

        public string ServicePlusId { get; set; }
        public string NotePlusId { get; set; }
        public string TaskPlusId { get; set; }
        public string ParentNoteId { get; set; }
        public string ParentTaskId { get; set; }
        public string TableName { get; set; }
        public bool AllReadOnly { get; set; }
        public bool IsReopened { get; set; }
        public string NextStepTaskComponentId { get; set; }
        public string PreferredLanguageId { get; set; }
        public string CurrentTopicId { get; set; }
        public DateTime? SurveyStartDate { get; set; }
        public string BillAmount { get; set; }
    }

}
