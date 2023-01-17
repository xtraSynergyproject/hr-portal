using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class UserRoleHierarchyPermissionViewModel : UserRoleHierarchyPermission
    {
        public string HierarchyName { get; set; }
        public string UserRoleName { get; set; }        
        public string ReferenceType { get; set; }
        public string Name { get; set; }
        public string HybridHierarchyId { get; set; }
       


    }
}
