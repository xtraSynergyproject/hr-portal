using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class DashboardTaskViewModel
    {
        public long AssignedToMe { get; set; }
        public long AssignPending { get; set; }
        public long AssignCompleted { get; set; }
        public long AssignOverdue { get; set; }
        public long RequestedByMe { get; set; }
        public long RequestOverdue { get; set; }
        public long RequestCompleted { get; set; }
        public long RequestDraft { get; set; }
        public long RequestPending { get; set; }
        public long SharedWithMe { get; set; }
        public long SharedByMe { get; set; }
    } 
}
