using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.Model
{
    public partial class ADM_UserRoleAction : AdminBase
    {
        public ulong UserRoleActionId { get; set; }
        public ulong UserRoleId { get; set; }
        public ulong ActionId { get; set; }
        public ulong ScreenId { get; set; }
    }
}
