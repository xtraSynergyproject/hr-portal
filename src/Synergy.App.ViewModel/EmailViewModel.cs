using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class EmailViewModel: Email
    {     
        public DataOperation Operation { get; set; }
        public string SmtpPassword { get; set; }
        public string OwnerUserId { get; set; }
        public string SenderEmail { get; set; }
        public string RecipientName { get; set; }
        public string SlotId { get; set; }
    }
}
