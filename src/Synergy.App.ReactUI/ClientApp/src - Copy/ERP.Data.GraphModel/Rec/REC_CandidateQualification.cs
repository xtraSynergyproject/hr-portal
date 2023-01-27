
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class REC_CandidateQualification : NodeBase
    {
        public string QualificationName { get; set; }
        public bool IsHighestQualification { get; set; }
        public int? GraduationYear { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string InstituteName { get; set; }
        public string University { get; set; }
        public string Location { get; set; }
    }
    public class R_CandidateQualification_Candidate : RelationshipBase
    {

    }
    public class R_CandidateQualification_Country : RelationshipBase
    {

    }

}
