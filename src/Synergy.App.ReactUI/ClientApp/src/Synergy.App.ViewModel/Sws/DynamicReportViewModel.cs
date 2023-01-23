using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class DynamicReportViewModel
    {       
        public string MetaDeta { get; set; }        
        public string measuresField { get; set; }
        public string dimensionsField { get; set; } 
        public string timeField { get; set; }
        public string rangeType { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string limit { get; set; }
        public string filters { get; set; }
        public string orders { get; set; }        
    }    
}
