using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.Model
{
    public class EmailConfig
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
