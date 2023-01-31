using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserTab : AdminBase
    { 
        public ulong UserTabId { get; set; }
        public ulong UserId { get; set; }
        public ulong TabId  {  get; set; }
        public ulong ScreenId { get; set; }     
       
    }
}
