using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
   public class TimeinTimeoutDetailsViewModel
    {

        public string Id { get; set; }
        public string UserId { get; set; }
        public string PersonId { get; set; }
        public DateTime Date { get; set; }
        public string Roster { get; set; }
        public string Actual { get; set; }
        public string Duty2Roster { get; set; }
        public string Duty3Roster { get; set; }
        public string Duty2Actual { get; set; }
        public string Duty3Actual { get; set; }
        public string EmployeeComments { get; set; }
        public string OverrideComments { get; set; }

        public DateTime SearchMonth { get; set; }
        public DateTime? SearchStart { get; set; }
        public DateTime? SearchEnd { get; set; }
        public string SearchType { get; set; }
    }
}
