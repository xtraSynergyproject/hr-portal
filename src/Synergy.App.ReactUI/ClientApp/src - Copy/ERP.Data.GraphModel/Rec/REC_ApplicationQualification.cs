
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class REC_ApplicationQualification : REC_CandidateQualification
    {
        public long QualificationId { get; set; }
        public DateTime SnapshotDate { get; set; }
    }
    public class R_ApplicationQualification_Application : RelationshipBase
    {

    }
}
