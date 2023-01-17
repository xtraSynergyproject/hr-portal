using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
namespace ERP.UI.ViewModel
{
    public class EmailConfigSearchViewModel : SearchViewModelBase
    {
        public string Name { get; set; }
        public string SmtpSenderName { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpFromId { get; set; }
        public string SmtpUserId { get; set; }
        public string SmtpPassword { get; set; }
    }
}

