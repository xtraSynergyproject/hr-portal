using ERP.Data.Model;
using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class NotificationViewModel : ViewModelBase
    {
        public long NotificationId { get; set; }
        public NodeEnum? ReferenceNode { get; set; }
        public ReferenceTypeEnum? ReferenceTypeCode { get; set; }
        [Display(Name = "Reference Type")]
        public string ReferenceTypeName { get; set; }
        [Display(Name = "Reference Id")]
        public long? ReferenceId { get; set; }
        public long? ToUserId { get; set; }
        private long? _fromUserId;
        public DateTime? NotificationDateTime { get; set; }
        public long? FromUserId
        {
            get
            {
                if (_fromUserId == null)
                {
                    return Constant.WebAppUserId;
                }
                else
                {
                    return _fromUserId.Value;
                }
            }
            set
            {
                _fromUserId = value;
            }
        }
        public ReadStatusEnum ReadStatus { get; set; }
        public string ReadStatusString { get { return ReadStatus.ToString(); } }
        public bool NotifyByEmail { get; set; }
        public bool NotifyBySms { get; set; }

        public string From { get; set; }
        public bool ShowOriginalSender { get; set; }
        public string SenderName { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        [Display(Name = "Description")]
        public string Body { get; set; }
        public string Url { get; set; }

        public string MobileNo { get; set; }

        [Display(Name = "From")]
        public string FromUserDisplay { get; set; }
        [Display(Name = "To")]
        public string ToUserDisplay { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Notification Date")]
        public string Date { get { return CreatedDate.ToYYY_MM_DD_HHMMSS(); } }
        public bool DisableDefaultEmailTemplate { get; set; }
        public NotificationStatusEnum NotificationStatus { get; set; }
        public ModuleEnum? ModuleName { get; set; }
        public NotificationTypeEnum? NotificationType { get; set; }
        public dynamic DynamicObject { get; set; }
        public string ModuleUrl { get; set; }
        public string SmsText { get; set; }
        public bool SendAlways { get; set; }
        public ActiveUserViewModel Recipient { get; set; }

        public long? ToCandidateId { get; set; }
        public long? FromCandidateId { get; set; }
        public long? NotificationTemplateMasterId { get; set; }
        public string ReferenceTypeNo { get; set; }
        public string TemplateCategory { get; set; }
        public long? WorkflowReferenceId { get; set; }
        public bool? SendAsync { get; set; }
        public ReferenceTypeEnum? WorkflowReferenceTypeCode { get; set; }
    }
}
