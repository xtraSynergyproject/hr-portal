using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserRolePerson : AdminBase
    { 
        public ulong UserRoleId { get; set; }
        public ulong PersonId  {  get; set; }
    }
}
