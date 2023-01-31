
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class REC_CandidateTraining : NodeBase
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public string InstituteName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class R_CandidateTraining_Candidate : RelationshipBase
    {

    }
    public class R_CandidateTraining_Country : RelationshipBase
    {

    }

}
