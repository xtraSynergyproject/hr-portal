using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_TaskFieldValue : NodeBase
    {
        public virtual long? VersionNo { get; set; }
        //public NTS_Task Task { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }  
    public class R_TaskFieldValue_TemplateField : RelationshipBase
    {

    }
    public class R_TaskFieldValue_Task : RelationshipBase
    {

    }
}
