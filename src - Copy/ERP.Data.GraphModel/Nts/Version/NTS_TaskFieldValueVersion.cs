using System.Linq;
using System;

namespace ERP.Data.GraphModel
{

  
    public partial class NTS_TaskFieldValueVersion : NTS_TaskFieldValue
    {
        public long VersionedByUserId { get; set; }
        public DateTime VersionedDate { get; set; }
    }
    public class R_TaskFieldValueVersion_TaskVersion : RelationshipBase
    {

    }
    public class R_TaskFieldValueVersion_TemplateField : RelationshipBase
    {

    }
}
