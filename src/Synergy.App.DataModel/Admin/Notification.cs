using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class Notification : DataModelBase
    {
        public ReferenceTypeEnum ReferenceType { get; set; }
        public string ReferenceTypeId { get; set; }
        public ReadStatusEnum ReadStatus { get; set; }
        public NotificationStatusEnum NotificationStatus { get; set; }
        public string From { get; set; }

        public string To { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }

        public string SenderName { get; set; }

        public string CC { get; set; }

        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SmsText { get; set; }
        public DateTime? NotificationDateTime { get; set; }
        public string Url { get; set; }
        public bool ShowOriginalSender { get; set; }
        public bool DisableDefaultEmailTemplate { get; set; }
        public ModuleEnum? ModuleName { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public bool NotifyByEmail { get; set; }
        public bool NotifyBySms { get; set; }
        public bool SendAlways { get; set; }
        public string ReferenceTypeNo { get; set; }
        public string[] AttachmentIds { get; set; }
        public string AttachmentBase64 { get; set; }
        public string AttachmentName { get; set; }
        public string TemplateCode { get; set; }
        public bool IsArchived { get; set; }
        public bool IsAutoArchive { get; set; }

        [ForeignKey("NotificationTemplate")]
        public string NotificationTemplateId { get; set; }

        public NotificationTemplate NotificationTemplate { get; set; }
        public NotificationActionStatusEnum ActionStatus { get; set; }
        public bool IsStarred { get; set; }
        public ReferenceTypeEnum TriggeredByReferenceType { get; set; }
        public string TriggeredByReferenceTypeId { get; set; }

    }
    [Table("NotificationLog", Schema = "log")]
    public class NotificationLog : Notification
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
        public DateTime LogEndDateTime { get; set; }
        public bool IsDatedLatest { get; set; }
        public bool IsVersionLatest { get; set; }
    }
}
