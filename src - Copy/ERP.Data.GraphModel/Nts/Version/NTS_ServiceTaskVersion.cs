using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    [CompanyNotRequired]
    public partial class NTS_ServiceTaskVersion : NTS_ServiceTask
    {
        public long VersionedByUserId { get; set; }
        public DateTime VersionedDate { get; set; }
    }
    public class R_ServiceTaskVersion_ServiceVersion : RelationshipBase
    {

    }
    public class R_ServiceTaskVersion_TaskVersion : RelationshipBase
    {

    }
}
