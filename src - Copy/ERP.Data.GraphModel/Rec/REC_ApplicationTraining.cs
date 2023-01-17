
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class REC_ApplicationTraining : REC_CandidateTraining
    {
        public long TrainingId { get; set; }
        public DateTime SnapshotDate { get; set; }
    }
    public class R_ApplicationTraining_Application : RelationshipBase
    {

    }

}
