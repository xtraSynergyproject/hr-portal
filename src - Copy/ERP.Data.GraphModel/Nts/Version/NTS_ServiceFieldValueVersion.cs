using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_ServiceFieldValueVersion : NTS_ServiceFieldValue
    {
        public long VersionedByUserId { get; set; }
        public DateTime VersionedDate { get; set; }
    }
    public class R_ServiceFieldValueVersion_ServiceVersion : RelationshipBase
    {

    }
    public class R_ServiceFieldValueVersion_TemplateField : RelationshipBase
    {

    }
}
