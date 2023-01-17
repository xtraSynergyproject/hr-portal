using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
    public partial class HITS_RecordChanges : NodeBase
    {
        public long EmployeeId { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public string EmployeeStatus { get; set; }
        public string PositionOrJob { get; set; }
        public string Transaction { get; set; }
        public Nullable<DateTime> EffectiveDate { get; set; }
        public string TransactionStatus { get; set; }
        public string ChangedData { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

    }
}
