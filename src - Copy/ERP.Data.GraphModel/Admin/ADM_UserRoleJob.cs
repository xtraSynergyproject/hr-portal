using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserRoleJob : AdminBase
    { 
        public ulong UserRoleJobId { get; set; }
        public ulong UserRoleId { get; set; }
        public ulong JobId  {  get; set; }
    }
}
