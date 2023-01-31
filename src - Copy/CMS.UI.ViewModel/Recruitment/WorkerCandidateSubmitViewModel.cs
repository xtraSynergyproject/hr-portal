using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class WorkerCandidateSubmitViewModel
    {
        public List<WorkerCandidateViewModel> Created { get; set; }
        public List<WorkerCandidateViewModel> Updated { get; set; }
        public List<WorkerCandidateViewModel> Destroyed { get; set; }
    }
}
