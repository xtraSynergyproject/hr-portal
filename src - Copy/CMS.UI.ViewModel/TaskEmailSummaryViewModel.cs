using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class TaskEmailSummaryViewModel 
    {  
        public string InprogressCount { get; set; }
        public string OverdueCount { get; set; }
        public string CompletedCount { get; set; }
        public string DraftCount { get; set; }
    }
}
