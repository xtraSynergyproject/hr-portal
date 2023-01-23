using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class NtsTaskChartList
    {
        public string Id { get; set; }
        public string TaskSubject { get; set; }
        public string TaskNo { get; set; }
        public string AssignName { get; set; }
        public string Priority { get; set; }

        public string Status { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string TemplateName { get; set; }
    }
}
