using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserGrade : AdminBase
    { 
        public ulong UserId { get; set; }
        public ulong GradeId  {  get; set; }
    }
}
