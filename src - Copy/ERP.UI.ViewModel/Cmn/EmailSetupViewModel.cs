using System;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class EmailSetupViewModel :ViewModelBase
    {

        public string UserName { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpFromId { get; set; }
        public string SmtpUserId { get; set; }
        public string SmtpPassword { get; set; }
        public string ProjectName { get; set; }
        public long? ProjectId { get; set; }
        public long? UserId { get; set; }
        public long? Count { get; set; }
    }
}