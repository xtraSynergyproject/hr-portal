using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class UserReportEmployeeSubmit
    {
        public List<UserReportEmployeeViewModel> Created { get; set; }
        public List<UserReportEmployeeViewModel> Updated { get; set; }
        public List<UserReportEmployeeViewModel> Destroyed { get; set; }
    }
}
