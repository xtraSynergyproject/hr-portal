using System;
using System.Linq;

namespace ERP.Data.GraphModel
{

    public partial class NTS_ServiceMultipleFieldValue : NodeBase
    {
        public virtual long? VersionNo { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public long ServiceId { get; set; }
        public long? RowId { get; set; }

    }

    public class R_ServiceMultipleFieldValue_ServiceFieldValue : RelationshipBase
    {

    }

    public class R_ServiceMultipleFieldValue_TemplateField : RelationshipBase
    {

    }
    public class R_ServiceMultipleFieldValue_Service : RelationshipBase
    {

    }
}
