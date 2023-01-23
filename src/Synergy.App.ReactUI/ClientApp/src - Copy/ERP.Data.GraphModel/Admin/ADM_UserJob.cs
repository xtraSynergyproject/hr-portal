using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserJob : AdminBase
    { 
        public ulong UserId { get; set; }
        public ulong JobId  {  get; set; }
        public ulong ScreenId { get; set; }
    }
}
