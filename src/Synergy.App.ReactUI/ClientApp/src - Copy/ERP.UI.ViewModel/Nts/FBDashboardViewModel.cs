using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class FBDashboardViewModel
    {
        public long S_RequestOverdue { get; set; }
        public long S_RequestPending { get; set; }

        public long T_AssignPending { get; set; }
        public long T_AssignOverdue { get; set; }

        public long N_Active { get; set; }

        public string base64Img { get; set; } 
    }
}
