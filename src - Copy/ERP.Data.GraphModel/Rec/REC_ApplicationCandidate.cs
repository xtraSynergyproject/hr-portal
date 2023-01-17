
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class REC_ApplicationCandidate : REC_Candidate
    {
        public long CandidateId { get; set; }
        public DateTime SnapshotDate { get; set; }
        public long? ResumeId { get; set; }
        public long? PhotoId { get; set; }
    }
    public class R_ApplicationCandidate_Application : RelationshipBase
    {

    }

}
