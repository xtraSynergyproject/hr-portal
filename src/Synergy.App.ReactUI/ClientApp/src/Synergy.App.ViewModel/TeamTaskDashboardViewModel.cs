
using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class TeamTaskDashboardViewModel
    {
        public string UserName { get; set; }
        public long TaskCount { get; set; }
        public long OverdueCount { get; set; }
        public string Billable { get; set; }
        public string NonBillable { get; set; }

    }
}
