
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class REC_ApplicationWorkExperience : REC_CandidateWorkExperience
    {
        public long WorkExperienceId { get; set; }
        public DateTime SnapshotDate { get; set; }
    }
    public class R_ApplicationWorkExperience_Application : RelationshipBase
    {

    }


}
