
using ERP.Utility;
using System;
using System.Collections.Generic;

namespace ERP.Data.GraphModel
{
    public partial class SPS_SuccessionPlanningMaster : NodeBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SuccessionPlanningTargetEnum? DocumentMasterTargetType { get; set; }
        public SuccessionPlanningDurationEnum? DocumentDurationType { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Quarter { get; set; }
        public int? HalfYear { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public SuccessionPlanningStatusEnum? DocumentMasterStatus { get; set; }
    }
    public class R_SuccessionPlanningMaster_SuccessionPlanningService_TemplateMaster : RelationshipBase
    {

    }
    public class R_SuccessionPlanningMaster_DevelopmentPlanningService_TemplateMaster : RelationshipBase
    {

    }
}
