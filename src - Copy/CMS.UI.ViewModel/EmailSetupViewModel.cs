using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using CMS.Common;
using CMS.Data.Model;


namespace CMS.UI.ViewModel
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