using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class CollectorWardAssignmentViewModel : ViewModelBase
    {
        public string WardId { get; set; }
        public string CollectorId { get; set; }
        public string CollectorName { get; set; }
        public string WardName { get; set; }
    }
}
