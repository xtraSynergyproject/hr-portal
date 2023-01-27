using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
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
