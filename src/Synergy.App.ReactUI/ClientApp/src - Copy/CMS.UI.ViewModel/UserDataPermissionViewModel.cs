using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class UserDataPermissionViewModel: UserDataPermission
    {
        public List<string> UserIds { get; set; }
        public string Users { get; set; }
        public string UserName { get; set; }
        public string PageName { get; set; }
        public string ColumnMetadataName { get; set; }
        public string ColumnMetadataName2 { get; set; }
    }
}
