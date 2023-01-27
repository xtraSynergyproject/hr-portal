using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class HeadOfDepartmentViewModel : HeadOfDepartment
    {
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string OrganizationName { get; set; }
        public string[] OrganizationId { get; set; }
    }
}
