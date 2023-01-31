using ERP.Utility;
using System;
using System.Dynamic;
using System.Linq;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_TaskLog
    {
        public long Id { get; set; }
        public string TaskNo { get; set; }
        public long? VersionNo { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public int? SLA { get; set; }

        public DateTime? DueDate { get; set; }
    }



}
