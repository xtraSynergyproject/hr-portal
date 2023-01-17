using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Net.Mail;

namespace Synergy.App.ViewModel
{
    public class MessageEmailViewModel
    {

        public string Subject { get; set; }
        public string Body { get; set; }
        public string Bcc { get; set; }
        public string Cc { get; set; }
        public string To { get;set; }
        public string From { get; set; }
        public string MessageId { get; set; }
        public string ToUserId { get; set; }
        public string FromUserId { get; set; }
        public string TaskId { get; set; }
        public bool IsTaskCreated { get; set; }
        public string EmailType { get; set; }

        public string AttachmentIds { get; set; }
        public int Total { get; set; }

    }
}
