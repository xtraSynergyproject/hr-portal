using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class ProjectDashboardViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TemplateMasterName { get; set; }
        public double ProgressPercentage { get; set; }
        public long TaskCount { get; set; }
        public long TaskLeftCount { get; set; }
        public long TaskCompletedCount { get; set; }
        public long DaysLeftCount { get; set; }
        public long TaskCompletedPercentage { get; set; }

        public long ActivityCount { get; set; }
        public double ProjectEstimatedHours { get; set; }
        public double TaskEstimatedHours { get; set; }
        public List<IdNameViewModel> ProjectList { get; set; }
        public TimeSpan? TaskTimeEntry { get; set; }
        public double TaskTimeEntryHours
        {
            get
            {
                return TaskTimeEntry == null ? 0.0 : TaskTimeEntry.Value.TotalHours;
            }
        }
        public NtsPriorityEnum? PriorityDashboard { get; set; }
        public NtsPriorityEnum? Priority { get; set; }
        public NtsActionEnum TemplateAction { get; set; }
        public NtsUserTypeEnum? TemplateUserType { get; set; }
        public string ProjectStatus { get; set; }
        public string ServiceNo { get; set; }
        public string CreatedBy { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime DueDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime CreatedOn { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime UpdatedOn { get; set; }

        public long TemplateId { get; set; }

        // Filter Properties

        public long UserId { get; set; }


        public ICollection<NtsPriorityEnum?> Priorities { get; set; }

        public string StatusCode { get; set; }
        public ICollection<string> Status { get; set; }

        public ModuleEnum? Module { get; set; }
        public ICollection<ModuleEnum?> Modules { get; set; }

        public ICollection<long> Templates { get; set; }
        public ICollection<long> Users { get; set; }
        public ICollection<long> TaskRequesters { get; set; }
        public long? ServiceId { get; set; }
        public long? ServiceStepServiceId { get; set; }

        public string[] StepList { get; set; }

        public List<WBSWorkflowViewModel> WBSWorkflowList { get; set; }

        public double? CompletionPercentage { get; set; }

        public long WbsTemplateId { get; set; }
        public string ViewType { get; set; }
        //public string[] UdfList { get; set; }   
        public List<WBSItemFieldViewModel> UdfList { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name ="Cut Off Date")]
        public DateTime? CutOffDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Cut Off Date")]
        public DateTime? CutOffDate1 { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserFirstLetter { get; set; }

        public string UserIds { get; set; }
        public string TaskGroupIds { get; set; }
        public DateTime? EDate { get; set; }
        public DateTime? SDate { get; set; }
    }
}
