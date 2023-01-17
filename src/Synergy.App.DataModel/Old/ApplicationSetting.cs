using System;

namespace Synergy.App.DataModel
{
    public class ApplicationSetting : DataModelBase
    {
        public string SmtpSenderName { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpFromId { get; set; }
        public string SmtpUserId { get; set; }
        public string SmtpPassword { get; set; }
    }
}
