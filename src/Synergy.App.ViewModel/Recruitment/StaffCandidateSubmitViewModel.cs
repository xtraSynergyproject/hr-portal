using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class StaffCandidateSubmitViewModel
    {
        public List<StaffCandidateViewModel> Created { get; set; }
        public List<StaffCandidateViewModel> Updated { get; set; }
        public List<StaffCandidateViewModel> Destroyed { get; set; }
    }
}
