using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class DirectSalesSearchViewModel
    {
        public string Customer { get; set; }      
        public string WorkflowStatus { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }       
        public string ProposalSource { get; set; }
      
    }
}
