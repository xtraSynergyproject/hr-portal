using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{
    public class NTS_TemplatePackageServiceTask : NTS_TemplatePackageServiceTaskConfig
    {
       
    }
    public class R_TemplatePackageServiceTask_TemplatePackage : RelationshipBase
    {

    }
    public class R_TemplatePackageServiceTask_ServiceTaskTemplate : RelationshipBase
    {

    }
    public class NTS_TemplatePackageServiceTaskConfig : NodeBase
    {
        public bool SubjectAllow { get; set; }
        public bool NotificationSubjectAllow { get; set; }
        public bool DescriptionAllow { get; set; }
        public bool ServiceTemplateIdAllow { get; set; }
        public bool ServiceTemplateNameAllow { get; set; }
        public bool TaskTemplateMasterIdAllow { get; set; }

        public bool EnableTaskAutoCompleteAllow { get; set; }
        public bool IsTaskAutoCompleteIfSameAssigneeAllow { get; set; }
        public bool IsTaskAutoCompleteIfAssigneeIsServiceRequesterAllow { get; set; }

        public bool TaskTemplateIdAllow { get; set; }
        public bool TaskTemplateMasterNameAllow { get; set; }
        public bool TaskTemplateMasterCodeAllow { get; set; }
        public bool TriggeredByServiceTaskTemplateIdAllow { get; set; }
        public bool TriggeredByServiceTaskTemplateSubjectAllow { get; set; }
        public bool ReturnedToServiceTaskTemplateIdAllow { get; set; }

        public bool RatingTypeAllow { get; set; }
        public bool RatingTypeCodeAllow { get; set; }
        public bool RatingTypeNameAllow { get; set; }
        public bool RatingByServiceTaskTemplateIdAllow { get; set; }
        public bool AssignToTypeAllow { get; set; }
        public bool AssigneeTargetTypeAllow { get; set; }
        public bool AssignedQueryTypeAllow { get; set; }
        public bool AssignedToAllow { get; set; }
        public bool OwnerUserIdAllow { get; set; }
        public bool AssignIdAllow { get; set; }
        public bool AssignedByQueryAllow { get; set; }
        public bool TeamIdAllow { get; set; }
        public bool SequenceNoAllow { get; set; }


        public bool AssignedToHierarchyIdAllow { get; set; }
        public bool AssignedToHierarchyLevelNoAllow { get; set; }


        public bool ServiceTaskTypeAllow { get; set; }
        public bool TriggeredByScriptAllow { get; set; }

        public bool HideIfDraftAllow { get; set; }



        public bool SLAAllow { get; set; }

        public bool SLACalculationModeAllow { get; set; }

        public bool StartDateAllow { get; set; }

        public bool DueDateAllow { get; set; }


        public bool TaskSLAAllow { get; set; }

        public bool SLADayAllow { get; set; }
        public bool SLAHourAllow { get; set; }

        public bool TaskSLACalculationModeAllow { get; set; }

        public bool TaskStartDateAllow { get; set; }

        public bool TaskDueDateAllow { get; set; }

        public bool OldServiceTaskTemplateIdAllow { get; set; }

        public bool AssignedByMethodNameAllow { get; set; }

        public bool WorkFlowStepIdAllow { get; set; }
        public bool WorkFlowStageIdAllow { get; set; }
        public bool WorkFlowStepAllow { get; set; }
        public bool WorkFlowStageAllow { get; set; }

        public bool DistributionListIdAllow { get; set; }
        public bool DistributionListAllow { get; set; }

        public bool CompletionPercentageAllow { get; set; }
        public bool GroupNameAllow { get; set; }
    }


}
