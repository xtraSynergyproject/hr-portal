using System;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class MailMessageViewModel:ViewModelBase
    {
        public string MessageId { get; set; }
        public string From { get; set; }
        public string SenderName { get; set; }

        public string To { get; set; }
        public string CC { get; set; }

        public string BCC { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

       // public NotificationStatusEnum EmailStatus { get; set; }

        //public string SmtpHost { get; set; }

        //public int SmtpPort { get; set; }

        //public string SmtpUserId { get; set; }

        public Guid? EmailUniqueId { get; set; }
        public long? FromUserId { get; set; }
        public bool? ShowOriginalSender { get; set; }
        public NotificationStatusEnum? EmailStatus { get; set; }
        public string Error { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUserId { get; set; }

        public long? ProjectId { get; set; }
        public long? TaskId { get; set; }
    }
}