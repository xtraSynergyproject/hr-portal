using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserRoleBlock : AdminBase
    { 
        public ulong UserRoleBlockId { get; set; }
        public ulong UserRoleId { get; set; }
        public ulong TabId  {  get; set; }
        public ulong BlockId { get; set; }     
    }
}
