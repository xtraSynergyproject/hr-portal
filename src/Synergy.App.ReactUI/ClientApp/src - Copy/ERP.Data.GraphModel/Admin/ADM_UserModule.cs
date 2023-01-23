
namespace ERP.Data.Model
{
    using System;
    using System.Collections.Generic;

    public partial class ADM_UserModule : AdminBase
    {
        public ulong UserModuleId { get; set; }
        public ulong UserId { get; set; }
        public ulong ModuleId { get; set; }       
       
    }
}
