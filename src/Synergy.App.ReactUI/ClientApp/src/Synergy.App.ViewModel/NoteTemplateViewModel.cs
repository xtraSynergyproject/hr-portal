using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class NoteTemplateViewModel : NoteTemplate
    {

        public string JsonCopy { get; set; }
        public string Json { get; set; }
        public string DataJson { get; set; }
        public PageViewModel Page { get; set; }
        public string PageId { get; set; }
        public string RecordId { get; set; }
        public string PortalName { get; set; }
        public bool SetUdfValue { get; set; }
        public Dictionary<string, string> Prms { get; set; }
        public Dictionary<string, string> Udfs { get; set; }
        public Dictionary<string, bool> ReadoOnlyUdfs { get; set; }
        public Dictionary<string, bool> HiddenUdfs { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateDisplayName { get; set; }
        public long NoteCount { get; set; }
        public string NoteEventId { get; set; }
        public NoteTemplateViewModel NoteTemplateVM { get; set; }
        public TemplateViewModel TemplateViewModel { get; set; }
        public DataTable NoteTable { get; set; }
        public List<ColumnMetadataViewModel> ColumnList { get; set; }
        public string ParentTemplateId { get; set; }

        public string ActiveUserId { get; set; }
        public string NoteNo { get; set; }
        public string NoteId { get; set; }
        public string UdfNoteTableId { get; set; }


        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? StartDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? ExpiryDate { get; set; }
        //public TimeSpan? NoteSLA { get; set; }
        // public double? NoteSLASeconds { get; set; }
        public double? SLASeconds { get; set; }
        public TimeSpan ActualSLA { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? ReminderDate { get; set; }


        public string NoteStatusBgCss
        {
            get
            {
                switch (NoteStatusCode)
                {
                    case "NOTE_STATUS_DRAFT":
                        return "bg-draft";
                    case "NOTE_STATUS_INPROGRESS":
                        return "bg-inprogress";
                    case "NOTE_STATUS_OVERDUE":
                        return "bg-overdue";
                    case "NOTE_STATUS_COMPLETE":
                        return "bg-completed";

                    case "NOTE_STATUS_CANCEL":
                        return "bg-canceled";
                    default:
                        return "bg-default";
                }
            }
        }
        public string NoteStatusFontCss
        {
            get
            {
                switch (NoteStatusCode)
                {
                    case "NOTE_STATUS_DRAFT":
                        return "text-draft";
                    case "NOTE_STATUS_INPROGRESS":
                        return "text-inprogress";
                    case "NOTE_STATUS_OVERDUE":
                        return "text-overdue";
                    case "NOTE_STATUS_COMPLETE":
                        return "text-completed";

                    case "NOTE_STATUS_CANCEL":
                        return "text-canceled";
                    default:
                        return "text-default";
                }
            }
        }


        public string NoteStatusId { get; set; }
        public string TableMetadataId { get; set; }
        public string NoteStatusCode { get; set; }
        public string NoteStatusName { get; set; }

        public string NoteActionId { get; set; }
        public string NoteActionCode { get; set; }
        public string NoteActionName { get; set; }


        public string NotePriorityId { get; set; }
        public string NotePriorityCode { get; set; }
        public string NotePriorityName { get; set; }


        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? SubmittedDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? CompletedDate { get; set; }
        //[DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        //public DateTime? RejectedDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? CanceledDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? ReturnedDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? ReopenedDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime? ClosedDate { get; set; }
        public string CloseComment { get; set; }
        public double? UserRating { get; set; }
        //public string RejectionReason { get; set; }
        //public string ReturnReason { get; set; }
        // public string ReopenReason { get; set; }
        public string CancelReason { get; set; }

        public string CompleteReason { get; set; }
        //public string DelegateReason { get; set; }
        public string RequestedByUserId { get; set; }
        public string RequestedByUserName { get; set; }
        public string RequestedByUserEmail { get; set; }
        public string RequestedByUserPhotoId { get; set; }

        public string OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }
        public string OwnerUserEmail { get; set; }
        public string OwnerUserPhotoId { get; set; }

        public string NoteOwnerTypeId { get; set; }


        public NtsActiveUserTypeEnum ActiveUserType { get; set; }
        // public string AssignedToTypeId { get; set; }
        //public string AssignedToTypeCode { get; set; }
        //public string AssignedToTypeName { get; set; }

        //public string AssignedToUserId { get; set; }
        public string SharedByUserId { get; set; }
        public string SharedWithUserId { get; set; }
        //public string AssignedToUserName { get; set; }
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



        //public string AssignedToTeamId { get; set; }
        //public string AssignedToTeamName { get; set; }

        //public string AssignedToTeamUserId { get; set; }
        //public string AssignedToTeamUserName { get; set; }

        //public string AssignedToHierarchyMasterId { get; set; }
        //public string AssignedToHierarchyMasterName { get; set; }

        //public int? AssignedToHierarchyMasterLevelId { get; set; }
        //public string AssignedToHierarchyMasterLevelName { get; set; }


        //public bool IsStepNoteAutoCompleteIfSameAssignee { get; set; }

        // public string LockStatusId { get; set; }
        public LOV LockStatus { get; set; }


        public string ParentNoteId { get; set; }

        public bool IncludeReadonlyData { get; set; }
        public bool IsDraftButtonVisible
        {
            get
            {
                return EnableSaveAsDraft && !IsLogRecord &&
                    NoteStatusCode == "NOTE_STATUS_DRAFT" &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester)
                    && (DataAction != DataActionEnum.View && DataAction != DataActionEnum.None);
            }
        }
        public bool IsAcceptButtonVisible
        {
            get
            {
                return (NoteStatusCode == "NOTE_STATUS_INPROGRESS" || NoteStatusCode == "NOTE_STATUS_OVERDUE") &&
                    (NoteStatusCode == "NOTE_ACTION_AWAIT_ASSIGNEE_ACCEPT") && !IsLogRecord;
            }
        }
        public bool IsSubmitButtonVisible
        {
            get
            {
                return (NoteStatusCode == "NOTE_STATUS_DRAFT") && !IsLogRecord
                    && (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester)
                    && (DataAction != DataActionEnum.View && DataAction != DataActionEnum.None)
                    && IsVersioning == false;
            }
        }
        public bool IsExpireButtonVisible
        {
            get
            {
                return NoteStatusCode == "NOTE_STATUS_INPROGRESS" && !IsLogRecord &&
                    (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester);
            }
        }
        public bool IsInEditMode
        {
            get
            {
                return IsSubmitButtonVisible && !IsLogRecord;
            }
        }
        public bool IsVersioningButtonVisible
        {
            get
            {
                return ((NoteStatusCode == "NOTE_STATUS_INPROGRESS" || NoteStatusCode == "NOTE_STATUS_OVERDUE")
                    && (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester)
                    && !IsLogRecord) ||((NoteStatusCode == "NOTE_STATUS_INPROGRESS" || NoteStatusCode == "NOTE_STATUS_OVERDUE")
                    && IsPermittedUser && !IsLogRecord);
                // && IsVersioning && !IsLogRecord;
            }
        }
        public bool IsReplyButtonVisible
        {
            get
            {
                return NoteStatusCode != "NOTE_STATUS_CLOSE" && !IsLogRecord &&
                    (DataAction == DataActionEnum.View);
            }
        }
        public bool IsBackButtonVisibile
        {
            get
            {
                return EnableBackButton;
            }
        }
        public bool IsPermittedUser { get; set; }
        public bool IsAddCommentEnabled
        {
            get
            {
                return ( (NoteStatusCode != "NOTE_STATUS_CLOSE") && !IsLogRecord &&
                    (ActiveUserType != NtsActiveUserTypeEnum.None)) ||
                    ((NoteStatusCode != "NOTE_STATUS_CLOSE") && !IsLogRecord && IsPermittedUser);
            }
        }

        public bool IsAddAttachmentEnabled
        {
            get
            {
                return (NoteStatusCode != "NOTE_STATUS_CLOSE") && !IsLogRecord &&
                    (ActiveUserType != NtsActiveUserTypeEnum.None);
            }
        }
        public bool IsSharingEnabled
        {
            get
            {
                return (NoteStatusCode != "NOTE_STATUS_CLOSE") && !IsLogRecord &&
                    (ActiveUserType != NtsActiveUserTypeEnum.None);
            }
        }

        public bool IsCompleteButtonVisible
        {
            get
            {
                return (NoteStatusCode == "NOTE_STATUS_INPROGRESS" || NoteStatusCode == "NOTE_STATUS_OVERDUE")
                    && !IsLogRecord;
            }
        }

        public bool IsCloseButtonVisible
        {
            get
            {
                return (NoteStatusCode == "NOTE_STATUS_COMPLETE") && !IsLogRecord &&
                        (ActiveUserType == NtsActiveUserTypeEnum.Owner || ActiveUserType == NtsActiveUserTypeEnum.Requester);
            }
        }

        public long SharedCount { get; set; }
        public long AttachmentCount { get; set; }
        public long NotificationCount { get; set; }
        public long CommentCount { get; set; }

        public bool CanChangeOwner { get; set; }
        public long? NoteSequenceOrder { get; set; }
        public string NoteDescription { get; set; }
        public string NoteSubject { get; set; }
        public bool IncludeSharedList { get; set; }
        public string CustomUrl { get; set; }
        public string ReturnUrl { get; set; }
        public LayoutModeEnum? LayoutMode { get; set; }
        public string PopupCallbackMethod { get; set; }
        public NoteIndexPageTemplateViewModel NoteIndexPageTemplateDetails { get; set; }
        public NotificationTemplateViewModel NotificationTemplate { get; set; }
        public bool IsVersioning { get; set; }
        public bool IsLogRecord { get; set; }
        public string ChartItems { get; set; }
        public Dictionary<string, string> Resource { get; set; }

        public bool IgnorePermission { get; set; }
        public string PreviousParentId { get; set; }
        public string UploadedContent { get; set; }
        public string FileIds { get; set; }
        public string LogId { get; set; }

        public string ReferenceId { get; set; }

        public ReferenceTypeEnum ReferenceType { get; set; }
        public string PostComment { get; set; }
        public string ServicePlusId { get; set; }
        public string NotePlusId { get; set; }
        public string TaskPlusId { get; set; }
        public string ParentTaskId { get; set; }
        public string ParentServiceId { get; set; }

        public NtsViewTypeEnum? ViewType { get; set; }
        public NtsViewTypeEnum? ViewMode { get; set; }
        public List<NtsViewModel> BookItems { get; set; }
        public string AttachmentIds { get; set; }
        public string TableName { get; set; }
        public bool AllReadOnly { get; set; }
        public string FileName { get; set; }
        public string FolderType { get; set; }
        public bool IsUserGuide { get; set; }
    }
}
