using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class UserReportEmployeeSubmit
    {
        public  List<UserReportEmployeeViewModel> Created { get; set; }
        public  List<UserReportEmployeeViewModel> Updated { get; set; }
        public  List<UserReportEmployeeViewModel> Destroyed { get; set; }
    }
}
