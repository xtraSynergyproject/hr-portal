
namespace ERP.Data.Model
{
    using System;
    using System.Collections.Generic;

    public partial class ADM_UserRoleModule : AdminBase
    {
        public ulong UserRoleModuleId { get; set; }
        public ulong UserRoleId { get; set; }
        public ulong ModuleId { get; set; } 
    }
}
