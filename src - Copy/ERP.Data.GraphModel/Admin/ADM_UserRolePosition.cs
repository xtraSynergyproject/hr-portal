using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserRolePosition : AdminBase
    { 
        public ulong UserRoleId { get; set; }
        public ulong PositionId  {  get; set; }
    }
}
