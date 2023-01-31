using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
    public partial class HITS_LoanBalance : NodeBase
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Option { get; set; }
        public float OpenningBalance { get; set; }
        public float Debit { get; set; }
        public float Credit { get; set; }
        public float EndingBalance { get; set; }

    }
}

            