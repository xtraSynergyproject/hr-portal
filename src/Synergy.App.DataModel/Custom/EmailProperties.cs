using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;

namespace Synergy.App.DataModel
{
    public class EmailProperties
    {
        public string FromEmailId { get; set; }
        public string SenderName { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUserId { get; set; }
        public string SmtpPassword { get; set; }
        public SmtpClient SmtpClient { get; set; }
        public long Count { get; set; }

    }
}
