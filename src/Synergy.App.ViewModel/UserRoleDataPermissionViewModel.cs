using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class UserRoleDataPermissionViewModel : UserRoleDataPermission
    {
        public List<string> UserRoleIds { get; set; }
        public string UserRoles { get; set; }
        public string UserRoleName { get; set; }
        public string PageName { get; set; }
        public string ColumnMetadataName { get; set; }
        public string ColumnMetadataName2 { get; set; }
    }
}
