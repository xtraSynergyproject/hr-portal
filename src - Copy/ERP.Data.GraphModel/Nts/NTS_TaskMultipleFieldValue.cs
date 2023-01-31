using System;
using System.Linq;

namespace ERP.Data.GraphModel
{

    public partial class NTS_TaskMultipleFieldValue : NodeBase
    {
        public virtual long? VersionNo { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public long TaskId { get; set; }
        public long? RowId { get; set; }

    }

    public class R_TaskMultipleFieldValue_TaskFieldValue : RelationshipBase
    {

    }

    public class R_TaskMultipleFieldValue_TemplateField : RelationshipBase
    {

    }
    public class R_TaskMultipleFieldValue_Task : RelationshipBase
    {

    }
}
