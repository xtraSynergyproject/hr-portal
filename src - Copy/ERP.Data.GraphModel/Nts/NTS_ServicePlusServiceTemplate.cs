using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_ServicePlusServiceTemplate : NTSBase
    {
        public string Subject { get; set; }
        public string Description { get; set; }

        public string TriggeredByScript { get; set; }
        public RatingTypeEnum RatingType { get; set; }
        public AssignToTypeEnum? OwnerType { get; set; }
        public AssignedQueryTypeEnum? OwnerQueryType { get; set; }
        public string OwnerQuery { get; set; }
        public NtsServicePlusServiceTypeEnum? ServicePlusServiceType { get; set; }
        public bool HideIfDraft { get; set; }
        public decimal? SequenceNo { get; set; }

        public DateTime? ServiceStartDate { get; set; }
        public DateTime? ServiceDueDate { get; set; }
        public TimeSpan? ServiceSLA { get; set; }
        public SLACalculationMode? ServiceSLACalculationMode { get; set; }

    }
    public class R_ServicePlusServiceTemplate_ServicePlus_Template : RelationshipBase
    {

    }
    public class R_ServicePlusServiceTemplate_Service_TemplateMaster : RelationshipBase
    {

    }
    public class R_ServicePlusServiceTemplate_TriggeredBy_ServicePlusServiceTemplate : RelationshipBase
    {

    }
    //public class R_ServiceTaskTemplate_ReturnedTo_TaskTemplate : RelationshipBase
    //{

    //}
    //public class R_ServiceTaskTemplate_RatingBy_TaskTemplate : RelationshipBase
    //{

    //}
    //public class R_ServicePlusServiceTemplate_AssignedTo_User : RelationshipBase
    //{

    //}
    public class R_ServicePlusServiceTemplate_Owner_Team : RelationshipBase
    {

    }
    public class R_ServicePlusServiceTemplate_Owner_User : RelationshipBase
    {

    }
}
