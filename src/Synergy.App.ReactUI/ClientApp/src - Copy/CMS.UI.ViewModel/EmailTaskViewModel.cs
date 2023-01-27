using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class EmailTaskViewModel : TaskTemplateViewModel
    {
        public string CC { get; set; }
        public string BCC { get; set; }
        public string To { get; set; }
        public string SentDate { get; set; }
        public string MessageId { get; set; }
        public string From { get; set; }
        public string PhotoId { get; set; }

        public string StatusCode { get; set; }

        public string key { get; set; }
        public string title { get; set; }
        public bool lazy { get; set; }
    }
}
