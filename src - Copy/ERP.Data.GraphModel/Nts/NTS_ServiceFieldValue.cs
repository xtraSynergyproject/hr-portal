using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_ServiceFieldValue : NodeBase
    {
        public virtual long? VersionNo { get; set; }

        //public long ServiceId { get; set; }      

        //public long TemplateId { get; set; }

        //public long TemplateFieldId { get; set; }       

        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

    }

    public class R_ServiceFieldValue_TemplateField : RelationshipBase
    {

    }
    public class R_ServiceFieldValue_Service : RelationshipBase
    {

    }
}
