using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserRoleSubModule : AdminBase
    { 
        public ulong UserRoleSubModuleId { get; set; }
        public ulong UserRoleId { get; set; }
        public ulong ModuleId  {  get; set; }
        public ulong SubModuleId { get; set; }      
    }
}
