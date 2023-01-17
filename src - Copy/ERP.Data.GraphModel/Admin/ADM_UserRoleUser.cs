using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.Model
{
    public partial class ADM_UserRoleUser : AdminBase
    {
        public ulong UserRoleId { get; set; }
        public ADM_UserRole UserRole { get; set; }
        public ulong UserId { get; set; }
        public ADM_User User { get; set; }
    }
}
