using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_ServiceTaskTemplate :NTSBase
    {
        public string Subject { get; set; }
        public string NotificationSubject { get; set; }
        public string Description { get; set; }

        public string TriggeredByScript { get; set; }
        public RatingTypeEnum RatingType { get; set; }
        public AssignToTypeEnum? AssignToType { get; set; }
        public AssignedQueryTypeEnum? AssignedQueryType { get; set; }
        public string AssignedByQuery { get; set; }
      
        public NtsServiceTaskTypeEnum? ServiceTaskType { get; set; }
        public bool HideIfDraft { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public TimeSpan? TaskSLA { get; set; }
        public SLACalculationMode? TaskSLACalculationMode { get; set; }
        public int? AssignedToHierarchyLevelNo { get; set; }
        public bool IsTaskAutoCompleteIfSameAssignee { get; set; }
        public bool? IsTaskAutoCompleteIfAssigneeIsServiceRequester { get; set; }
        public bool EnableTaskAutoComplete { get; set; }
        public NtsUserTypeEnum? AssigneeTargetType { get; set; }
        public long? OldServiceTaskTemplateId { get; set; }
        public string AssignedByMethodName { get; set; }
        public double? CompletionPercentage { get; set; }
        public decimal? SequenceNo { get; set; }
        public bool? DisableDirectApproval { get; set; }
    }

    public class R_ServiceTaskTemplate_Service_Template : RelationshipBase
    {

    }
    public class R_ServiceTaskTemplate_Task_TemplateMaster : RelationshipBase
    {

    }
    public class R_ServiceTaskTemplate_TriggeredBy_TaskTemplate : RelationshipBase
    {

    }
    public class R_ServiceTaskTemplate_ReturnedTo_TaskTemplate : RelationshipBase
    {

    }
    public class R_ServiceTaskTemplate_RatingBy_TaskTemplate : RelationshipBase
    {

    }
    public class R_ServiceTaskTemplate_AssignedTo_User : RelationshipBase
    {

    }
    public class R_ServiceTaskTemplate_AssignedTo_Team : RelationshipBase
    {

    }
    public class R_ServiceTaskTemplate_Owner_User : RelationshipBase
    {

    }
    public class R_ServiceTaskTemplate_WorkflowStep_Note : RelationshipBase
    {

    }
    public class R_ServiceTaskTemplate_WorkflowStage_Note : RelationshipBase
    {

    }
    public class R_ServiceTaskTemplate_DistributionList_Note : RelationshipBase
    {

    }
}
