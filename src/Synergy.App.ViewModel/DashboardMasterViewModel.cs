using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class DashboardMasterViewModel : NoteTemplateViewModel
    {
        public string layoutMetadata { get; set; }       
        public bool gridStack { get; set; }       
        public bool isReportDashboard { get; set; }       

    }
}
