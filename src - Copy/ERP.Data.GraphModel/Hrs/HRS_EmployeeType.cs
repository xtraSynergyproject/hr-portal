using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.Model
{
    public partial class HRS_EmployeeType : HRSDated
    {  
        public string Name { get; set; }

        public string Description { get; set; }

        public int SequenceNo { get; set; }

    }
}
