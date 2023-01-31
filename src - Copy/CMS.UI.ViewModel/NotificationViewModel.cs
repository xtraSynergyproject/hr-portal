using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class NotificationViewModel: Notification
    {
        public string MobileNo { get; set; }
        public string FromUserDisplay { get; set; }
        public string ToUserDisplay { get; set; }
        public string RefStatus { get; set; }
        public dynamic DynamicObject { get; set; }
        public string ModuleUrl { get; set; }
        public string RecipientName { get; set; }
        public UserViewModel Recipient { get; set; }
        public UserViewModel Sender { get; set; }
        public long? ToCandidateId { get; set; }
        public long? FromCandidateId { get; set; }
        public long? NotificationTemplateMasterId { get; set; }
        public string TemplateCategory { get; set; }
        public string EmailUniqueId { get; set; }
        public long? WorkflowReferenceId { get; set; }
        public bool? SendAsync { get; set; }
        public ReferenceTypeEnum? WorkflowReferenceTypeCode { get; set; }
        public string ReadStatusString { get { return ReadStatus.ToString(); } }
        public string PhotoId { get; set; }
        public string ReferenceTemplateCode { get; set; }
        public bool IsIncludeAttachment { get; set; }
        public string EmailBody { get; set; }
        public string Module { get; set; }
        public string MenuGroup { get; set; }
        public string PageName { get; set; }
        public string PageType { get; set; }
        public string Template { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? WeekDate { get; set; }
        public IList<NotificationViewModel> FirstDay { get; set; }
        public IList<NotificationViewModel> SecondDay { get; set; }
        public IList<NotificationViewModel> ThirdDay { get; set; }
        public IList<NotificationViewModel> CurrentDay { get; set; }
        public IList<NotificationViewModel> FifthDay { get; set; }
        public IList<NotificationViewModel> SixthDay { get; set; }
        public IList<NotificationViewModel> SeventhDay { get; set; }
        public string FirstDayName { get; set; }
        public string SecondDayName { get; set; }
        public string ThirdDayName { get; set; }
        public string CurrentDayName { get; set; }
        public string FifthDayName { get; set; }
        public string SixthDayName { get; set; }
        public string SeventhDayName { get; set; }
       
        public IList<NotificationViewModel> Notifications { get; set; }
        public string DisplayCreatedDate
        {
            get
            {
                var d = Humanizer.DateHumanizeExtensions.Humanize(CreatedDate);
                return d;
            }
        }
        public string BookName { get; set; }
        public string BookNo { get; set; }
        public string BookTemplateCode { get; set; }
        public string BookId { get; set; }
        public NotificationActionTypeEnum? ActionType { get; set; }
        public string StatusBgCss
        {
            get
            {
                switch (RefStatus)
                {
                    case "SERVICE_STATUS_DRAFT":
                        return "#17a2b8";
                    case "SERVICE_STATUS_INPROGRESS":
                        return "#007bff";
                    case "SERVICE_STATUS_OVERDUE":
                        return "#ffc107";
                    case "SERVICE_STATUS_COMPLETE":
                        return "#28a745";
                    case "SERVICE_STATUS_REJECT":
                        return "#dc3545";
                    case "SERVICE_STATUS_CANCEL":
                        return "#343a40";
                    case "SERVICE_STATUS_CLOSE":
                        return "#343a40";
                    case "TASK_STATUS_DRAFT":
                        return "#17a2b8";
                    case "TASK_STATUS_INPROGRESS":
                        return "#007bff";
                    case "TASK_STATUS_OVERDUE":
                        return "#ffc107";
                    case "TASK_STATUS_COMPLETE":
                        return "#28a745";
                    case "TASK_STATUS_REJECT":
                        return "#dc3545";
                    case "TASK_STATUS_CANCEL":
                        return "#343a40";
                    case "NOTE_STATUS_DRAFT":
                        return "#17a2b8";
                    case "NOTE_STATUS_INPROGRESS":
                        return "#007bff";
                    case "NOTE_STATUS_OVERDUE":
                        return "#ffc107";
                    case "NOTE_STATUS_COMPLETE":
                        return "#28a745";

                    case "NOTE_STATUS_CANCEL":
                        return "#343a40";
                    default:
                        return "#383030ba";
                }
            }
        }
    }
}
