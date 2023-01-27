using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserRoleScreen : AdminBase
    { 
        public ulong UserRoleScreenId { get; set; }
        public ulong UserRoleId { get; set; }
        public ulong SubModuleId  {  get; set; }
        public ulong ScreenId { get; set; }      
       
    }
}
