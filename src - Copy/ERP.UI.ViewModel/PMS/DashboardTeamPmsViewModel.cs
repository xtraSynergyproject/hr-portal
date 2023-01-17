using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class DashboardTeamPmsViewModel
    {
        public long S_RequestedByMe { get; set; }
        public long S_RequestOverdue { get; set; }
        public long S_RequestCompleted { get; set; }
        public long S_RequestDraft { get; set; }
        public long S_RequestPending { get; set; }
        public long S_RequestCancel { get; set; }
        public long S_SharedWithMe { get; set; }
        public long S_ShareWithPending { get; set; }
        public long S_ShareWithCompleted { get; set; }
        public long S_ShareWithOverdue { get; set; }
        public long S_ShareWithCancel { get; set; }

        public long T_AssignedToMe { get; set; }
        public long T_AssignPending { get; set; }
        public long T_AssignCompleted { get; set; }
        public long T_AssignOverdue { get; set; }
        public long T_AssignReject { get; set; }
        public long T_SharedWithMe { get; set; }
        public long T_ShareWithPending { get; set; }
        public long T_ShareWithCompleted { get; set; }
        public long T_ShareWithOverdue { get; set; }
        public long T_ShareWithReject { get; set; }
    }
}
