using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class TeamSummaryViewModel : ViewModelBase    {

        public string UserName { get; set; }
        public long NotStarted { get; set; }
        public long Started { get; set; }
        public long Completed { get; set; }
        public long Cancelled { get; set; }
        public long Overdue { get; set; }
        public long Pending { get; set; } 
        public long Rejected { get; set; }
    }
}
