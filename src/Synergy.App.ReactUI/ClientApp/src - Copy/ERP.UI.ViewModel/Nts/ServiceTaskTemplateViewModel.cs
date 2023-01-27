using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    [Serializable]
    public class ServiceTaskTemplateViewModel : ViewModelBase
    {
        // public long ServiceTaskTemplateId { get; set; }
        [Display(Name = "Subject", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string Subject { get; set; }
        [Display(Name = "Notification Subject")]
        public string NotificationSubject { get; set; }
        public string Description { get; set; }
        public long ServiceTemplateId { get; set; }
        public string ServiceTemplateName { get; set; }
        [Display(Name = "TaskTemplateMasterId", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public long TaskTemplateMasterId { get; set; }
        [Display(Name = "Enable Task Auto Complete")]
        public bool EnableTaskAutoComplete { get; set; }
        [Display(Name = "Is Task Auto Complete If Same Assignee")]
        public bool IsTaskAutoCompleteIfSameAssignee { get; set; }
        [Display(Name = "Is Task Auto Complete If Assignee Is Service Requester")]
        public bool IsTaskAutoCompleteIfAssigneeIsServiceRequester { get; set; }

        [Display(Name = "TaskTemplateId", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public long TaskTemplateId { get; set; }
        [Display(Name = "TaskTemplateMasterName", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string TaskTemplateMasterName { get; set; }
        public string TaskTemplateMasterCode { get; set; }
        [Display(Name = "TriggeredByServiceTaskTemplateId", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public long? TriggeredByServiceTaskTemplateId { get; set; }
        public List<long> TriggeredByServiceTaskTemplates { get; set; }
        public string TriggeredByServiceTaskTemplatesStr { get; set; }
        public long[] TriggeredByServiceTaskTemplateData { get; set; }
        [Display(Name = "TriggeredByServiceTaskTemplateSubject", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string TriggeredByServiceTaskTemplateSubject { get; set; }
        public string TriggeredByServiceTaskTemplateStr { get; set; }

        public string[] TriggeredByServiceTaskTemplateSubjects { get; set; }
        public string TriggeredByServiceTaskTemplateSubjectStr
        {
            get
            {
                var str = "";
                if (TriggeredByServiceTaskTemplateSubjects != null)
                    str = string.Join(",", TriggeredByServiceTaskTemplateSubjects);
                return str;
            }
        }


        [Display(Name = "ReturnedToServiceTaskTemplateId", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public long? ReturnedToServiceTaskTemplateId { get; set; }

        public RatingTypeEnum RatingType { get; set; }
        [Display(Name = "RatingTypeCode", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string RatingTypeCode { get; set; }
        public string RatingTypeName { get; set; }
        public long? RatingByServiceTaskTemplateId { get; set; }
        [Display(Name = "AssignToType", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public AssignToTypeEnum? AssignToType { get; set; }
        [Display(Name = "AssigneeTargetType", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public NtsUserTypeEnum? AssigneeTargetType { get; set; }
        [Display(Name = "AssignedQueryType", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public AssignedQueryTypeEnum? AssignedQueryType { get; set; }
        [Display(Name = "AssignedTo", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public long? AssignedTo { get; set; }
        public long? OwnerUserId { get; set; }
        [Display(Name = "Assigned To")]
        public long? AssignId { get; set; }
        [Display(Name = "AssignedByQuery", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string AssignedByQuery { get; set; }
        [Display(Name = "TeamId", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public long? TeamId { get; set; }
        [Display(Name = "Sequence No")]
        public decimal? SequenceNo { get; set; }


        [Display(Name = "AssignedToHierarchyId", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public long? AssignedToHierarchyId { get; set; }
        [Display(Name = "AssignedToHierarchyLevelNo", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public int? AssignedToHierarchyLevelNo { get; set; }


        public NtsServiceTaskTypeEnum? ServiceTaskType { get; set; }
        [Display(Name = "TriggeredByScript", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string TriggeredByScript { get; set; }

        [Display(Name = "HideIfDraft", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public bool HideIfDraft { get; set; }



        [Display(Name = "SLA", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public TimeSpan? SLA { get; set; }

        [Display(Name = "SLACalculationMode", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public SLACalculationMode? SLACalculationMode { get; set; }

        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public DateTime? StartDate { get; set; }

        [Display(Name = "DueDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public DateTime? DueDate { get; set; }


        [Display(Name = "TaskSLA", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public TimeSpan? TaskSLA { get; set; }

        public int? SLADay { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? SLAHour { get; set; }

        [Display(Name = "TaskSLACalculationMode", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public SLACalculationMode? TaskSLACalculationMode { get; set; }

        [Display(Name = "TaskStartDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? TaskStartDate { get; set; }

        [Display(Name = "TaskDueDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? TaskDueDate { get; set; }

        public long? OldServiceTaskTemplateId { get; set; }

        public string AssignedByMethodName { get; set; }
        [Display(Name = "WorkFlow Step")]
        public long? WorkFlowStepId { get; set; }
        [Display(Name = "WorkFlow Stage")]
        public long? WorkFlowStageId { get; set; }
        public string WorkFlowStep { get; set; }
        public string WorkFlowStage { get; set; }

        public long? DistributionListId { get; set; }
        public string DistributionList { get; set; }
        [Display(Name = "Completion Percentage")]
        public double? CompletionPercentage { get; set; }
        public string GroupName { get; set; }
        public long? TemplatePackageId { get; set; }

        public TemplatePackageServiceTaskViewModel TemplatePackageServiceTaskConfig { get; set; }
        public bool? DisableDirectApproval { get; set; }
        public string AssignToTypeValue { get; set; }
    }
}
