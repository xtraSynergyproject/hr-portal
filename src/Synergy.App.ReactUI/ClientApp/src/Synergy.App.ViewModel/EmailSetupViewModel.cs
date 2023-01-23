using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Synergy.App.Common;
using Synergy.App.DataModel;


namespace Synergy.App.ViewModel
{
    public class EmailSetupViewModel 
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