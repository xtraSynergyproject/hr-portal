
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class RPT_ReportScheduler : NodeBase
    {
        public ReportFrequencyEnum ReportFrequency { get; set; }
        public TimeSpan ReportScheduleTime { get; set; }
        public string EmailIds { get; set; }
        public string EmailSubject { get; set; }
        public string EmailDescription { get; set; }
    }
    //public class R_ReportScheduler_Analyzer : RelationshipBase
    //{

    //}
    public class R_ReportScheduler_ListOfValue : RelationshipBase
    {

    }
    public class R_ReportScheduler_OrganizationRoot : RelationshipBase
    {

    }
}
