
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class REC_ApplicationCriteria : NodeBase
    {
        public string Name { get; set; }
        public double Weightage { get; set; }
    }

    public class R_REC_REC_ApplicationCriteria_JobRequestCriteria : RelationshipBase
    {

    }
}
