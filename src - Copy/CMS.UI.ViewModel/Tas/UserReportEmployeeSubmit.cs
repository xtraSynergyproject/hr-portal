using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class UserReportEmployeeSubmit
    {
        public List<UserReportEmployeeViewModel> Created { get; set; }
        public List<UserReportEmployeeViewModel> Updated { get; set; }
        public List<UserReportEmployeeViewModel> Destroyed { get; set; }
    }
}
