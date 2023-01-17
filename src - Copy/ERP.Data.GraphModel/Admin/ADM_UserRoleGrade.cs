using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;


namespace ERP.Data.Model
{    
    public partial class ADM_UserRoleGrade : AdminBase
    { 
        public ulong UserRoleGradeId { get; set; }
        public ulong UserRoleId { get; set; }
        public ulong GradeId  {  get; set; }
    }
}
