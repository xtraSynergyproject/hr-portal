using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class WorkboardDuplicateViewModel
    {
        public string WorkBoardName { get; set; }
        public bool IsUserData { get; set; }
        public bool IsCharts { get; set; }
        public bool IsComments { get; set; }
        public bool IsTasks { get; set; }

        public string WorkBoardId { get; set; }

    }
}
