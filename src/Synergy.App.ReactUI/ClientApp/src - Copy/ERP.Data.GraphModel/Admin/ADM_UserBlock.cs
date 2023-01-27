using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserBlock : AdminBase
    { 
        public ulong UserBlockId { get; set; }
        public ulong UserId { get; set; }
        public ulong TabId  {  get; set; }
        public ulong BlockId { get; set; }       
    }
}
