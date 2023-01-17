using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class AlertRemarkViewModel
    {
        public string id { get; set; }
        public string parentId { get; set; }
        public string remark { get; set; }
        public string remarkBy { get; set; }       
        public string remark_type { get; set; }
        public DateTime remark_datetime { get; set; }
        public string remark_datetimeDisplay
        {
            get
            {
                var d = Humanizer.DateHumanizeExtensions.Humanize(remark_datetime.ToLocalTime());
                return d;
            }
        }
        public bool isFalseEvent { get; set; }  

    }  
}
