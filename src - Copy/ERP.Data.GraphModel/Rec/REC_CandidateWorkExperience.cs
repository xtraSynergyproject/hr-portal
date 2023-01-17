
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class REC_CandidateWorkExperience : NodeBase
    {
        public string EmployerName { get; set; }
        public string EmployerAddress { get; set; }
        public bool IsCurrentEmployer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string LastManagerName { get; set; }
        public string CompanyAddress { get; set; }
        public string JobTitle { get; set; }
        public string ReasonForLeaving { get; set; }
        public string RolesAndResponsibilities { get; set; }
    }
    public class R_CandidateWorkExperience_Candidate : RelationshipBase
    {

    }
    public class R_CandidateWorkExperience_Country : RelationshipBase
    {

    }

}
