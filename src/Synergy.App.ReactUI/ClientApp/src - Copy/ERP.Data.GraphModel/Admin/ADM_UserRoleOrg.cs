using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserRoleOrg : AdminBase
    { 
        public ulong UserRoleId { get; set; }
        public ulong OrganizationId  {  get; set; }
    }
}
