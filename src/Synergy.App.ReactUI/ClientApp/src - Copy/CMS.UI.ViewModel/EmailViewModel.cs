using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
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
