using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;

namespace ERP.Data.Model
{
    public partial class ADM_Tab : AdminBase
    {
        public ulong TabId { get; set; }
        public ulong ScreenId { get; set; }
      
        public string Name { get; set; }       
       
       // public ICollection<ADM_Block> Blocks { get; set; }


    }
}
