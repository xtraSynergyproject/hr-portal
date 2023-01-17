using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class TaskEmailSummaryViewModel 
    {  
        public string InprogressCount { get; set; }
        public string OverdueCount { get; set; }
        public string CompletedCount { get; set; }
        public string DraftCount { get; set; }
    }
}
