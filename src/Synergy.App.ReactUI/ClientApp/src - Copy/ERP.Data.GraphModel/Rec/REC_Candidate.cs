
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class REC_Candidate : NodeBase
    {
        public string CandidateNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public GenderEnum? Gender { get; set; }
        public MaritalStatusEnum? MaritalStatus { get; set; }
        public string Mobile { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public bool IsProfileCompleted { get; set; }

        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string CareerSummary { get; set; }
    }
    public class R_Candidate_Nationality : RelationshipBase
    {

    }
    
}
