using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class SchedulerLogViewModel : NoteTemplateViewModel
    {
        public string fromDate { get; set; }
        
        public string toDate { get; set; }
        
        public string districtCode { get; set; }
        
        public string response { get; set; }
        
        public string error { get; set; }      
        public bool success { get; set; }      
        
    }
}
