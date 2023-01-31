using CMS.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class Email : DataModelBase
    {
        public string From { get; set; }
        public string SenderName { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public NotificationStatusEnum EmailStatus { get; set; }
        public int RetryCount { get; set; }
        public bool IsAsync { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUserId { get; set; }
        public string Error { get; set; }
        public bool ShowOriginalSender { get; set; }
        public bool DisableDefaultEmailTemplate { get; set; }
        public bool? SendToOriginalRecipient { get; set; }
        public string Source { get; set; }
        public long? SequenceNo { get; set; }
        public ReferenceTypeEnum? ReferenceType { get; set; }
        public string ReferenceId { get; set; }
        public CalendarInvitationTypeEnum? CalendarInvitationType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ReferenceTemplateCode { get; set; }
        public bool IsIncludeAttachment { get; set; }
        public string[] AttachmentIds { get; set; }
        public string AttachmentBase64 { get; set; }
        public string AttachmentName { get; set; }
        public string EmailUniqueId { get; set; }
    }
    [Table("EmailLog", Schema = "log")]
    public class EmailLog : Email
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
