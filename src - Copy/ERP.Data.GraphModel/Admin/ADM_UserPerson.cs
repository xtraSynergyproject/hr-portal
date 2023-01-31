using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserPerson : AdminBase
    { 
        public ulong UserId { get; set; }
        public ulong PersonId  {  get; set; }
    }
}
