using CMS.Common;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class NTSMessageViewModel
    {
        public string FromId { get; set; }
        public string From { get; set; }
        public string FromEmail { get; set; }
        public DateTime? SentDate { get; set; }
        public string SentDateDisplay
        {
            get
            {
                return SentDate.Humanize();
            }
        }
        public string ToId { get; set; }
        public string To { get; set; }
        public string RequestedTo { get; set; }
        public string ToEmail { get; set; }
        public string CCId { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Type { get; set; }
    }
}
