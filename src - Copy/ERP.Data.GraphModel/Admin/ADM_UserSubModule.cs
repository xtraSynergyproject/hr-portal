using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserSubModule : AdminBase
    { 
        public ulong UserSubModuleId { get; set; }
        public ulong UserId { get; set; }
        public ulong ModuleId  {  get; set; }
        public ulong SubModuleId { get; set; }  
    }
}
