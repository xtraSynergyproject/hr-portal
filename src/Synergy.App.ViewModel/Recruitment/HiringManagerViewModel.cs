using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class HiringManagerViewModel : HiringManager
    {
        public string OrganizationName { get; set; }
        public string[] OrganizationId { get; set; }
        public string OrganizationIds { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
    }
}
