using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class RssFeedViewModel : NoteTemplateViewModel
    {
        public string feedName { get; set; }        
        public string feedUrl { get; set; }  

    }
    public class SchedulerSyncViewModel : NoteTemplateViewModel
    {
        public string logstashContent { get; set; }
        public DateTime trackingDate { get; set; }
        public string scheduleTemplate { get; set; }

    }
}
