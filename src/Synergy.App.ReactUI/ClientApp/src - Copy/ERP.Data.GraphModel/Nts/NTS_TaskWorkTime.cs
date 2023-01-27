using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{

    public partial class NTS_TaskWorkTime : NTSBase
    {
        //public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan Duration { get; set; }
        public string WorkComment { get; set; }
        public bool? IsBillable { get; set; }
    }

    public class R_TaskWorkTime_Task : RelationshipBase
    {

    }
    public class R_TaskWorkTime_User : RelationshipBase
    {

    } 

}
