using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserOrg : AdminBase
    { 
        public ulong UserId { get; set; }
        public ulong OrganizationId  {  get; set; }
    }
}
