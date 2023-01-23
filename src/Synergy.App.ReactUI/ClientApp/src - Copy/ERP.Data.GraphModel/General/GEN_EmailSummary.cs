using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
  
    public partial class GEN_EmailSummary : NodeBase
    {
        public DateTime SummaryDate { get; set; }
        public long UserId { get; set; }
        public NotificationStatusEnum EmailStatus { get; set; }
    }
}
