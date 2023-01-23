using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class UserRoleHierarchyPermissionViewModel : ViewModelBase
    {
        public long UserRoleHierarchyPermissionId { get; set; }
        public long UserRoleId { get; set; }
        public long HierarchyId { get; set; }
        public string HierarchyName { get; set; }
        public HierarchyPermissionEnum HierarchyPermission { get; set; }
        public string UserRoleName { get; set; }
        [Display(Name = "Custom Permission")]
        public long? CustomPermissionId { get; set; }

    }
}


