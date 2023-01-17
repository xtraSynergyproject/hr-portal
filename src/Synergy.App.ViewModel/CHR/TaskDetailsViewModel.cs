using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;

namespace Synergy.App.ViewModel
{
    public class TaskDetailsViewModel
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Template { get; set; }
        public string owner { get; set; }
        public string Assignee { get; set; }
        public string Status { get; set; }

        public string TaskNo { get; set; }
        public string TaskName { get; set; }
        public string TaskStatus { get; set; }
        public string TaskSubject { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string ServiceNo {get;set;}
        public string ServiceName { get; set; }
        public TimeSpan SLA { get; set; }
        public string SLAText { get { return SLA.ToTimeSpanString(); } }


    }
}
