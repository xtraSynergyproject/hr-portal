using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserScreen : AdminBase
    { 
        public ulong UserscreenId { get; set; }
        public ulong UserId { get; set; }
        public ulong SubModuleId  {  get; set; }
        public ulong ScreenId { get; set; }     
       
    }
}
