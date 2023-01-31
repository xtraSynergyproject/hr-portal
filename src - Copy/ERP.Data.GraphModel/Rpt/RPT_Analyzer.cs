
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class RPT_Analyzer : NodeBase
    {
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string Fields { get; set; }
        public string SearchCondition { get; set; }
        public string Query { get; set; }
        public string Udf { get; set; }
    }
    public class R_Analyzer_User : RelationshipBase
    {

    }
    public class R_Analyzer_ListOfValue : RelationshipBase
    {

    }
    public class R_Analyzer_Category_ListOfValue : RelationshipBase
    {

    }
    public class R_Analyzer_SharedTo_User : RelationshipBase
    {
        public SharingModeEnum SharingMode { get; set; }
        public long SharedByUserId { get; set; }
    }
}
