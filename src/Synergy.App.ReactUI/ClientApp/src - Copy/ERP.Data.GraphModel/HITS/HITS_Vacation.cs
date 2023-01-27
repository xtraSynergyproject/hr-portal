using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{

   
    public partial class HITS_Vacation : NodeBase
    {
        public long EmpId { get; set; }
        public string Name { get; set; }
        public string Vacation { get; set; }
        public Nullable<DateTime> From { get; set; }
        public Nullable<DateTime> To { get; set; }
        public string EnterBy { get; set; }
        public Nullable<DateTime> EnterDate { get; set; }
        public string ChangeBy { get; set; }
        public Nullable<DateTime> ChangeDate { get; set; }
    }
}
