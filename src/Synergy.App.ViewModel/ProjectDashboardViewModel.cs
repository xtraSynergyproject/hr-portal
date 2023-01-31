
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Synergy.App.Common;

namespace Synergy.App.ViewModel
{
    public class ProjectDashboardViewModel
    {
        public string SuperServiceId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long ProjectCount { get; set; }
        public long ClosedTaskCount { get; set; }
        public long AllTaskCount { get; set; }
        public long InProgreessCount { get; set; }
        public long DraftCount { get; set; }
        public long CompletedCount { get; set; }
        public long OverDueCount { get; set; }
        public long Percentage { get; set; }
        public long UserCount { get; set; }
        public long AllTime { get; set; }
        public long FileCount { get; set; }
        public string ProjectName { get; set; }
        public string ProjectStatus { get; set; }
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string CreatedBy { get; set; }
      //  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime StartDate { get; set; }
       // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime DueDate { get; set; }
     //   [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime CreatedOn { get; set; }
     //   [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime UpdatedOn { get; set; }
        public NotePriorityEnum? Priority { get; set; }
        public string Mode { get; set; }
        public string PriorityClass
        {
            get
            {
                if (Priority != null)
                {
                    switch (Priority.ToString())
                    {
                        case "Low":
                            return "label-warning";
                        case "Medium":
                            return "label-success";
                        case "High":
                            return "label-danger";
                        default:
                            return "label-default";
                    }
                }
                return "label-default";
            }
        }
        public List<IdNameViewModel> ProjectList { get; set; }
        public NtsUserTypeEnum? TemplateUserType { get; set; }
        public double ProgressPercentage { get; set; }
        public long TaskCount { get; set; }
        public TimeSpan? TaskTimeEntry { get; set; }
        public double TaskTimeEntryHours
        {
            get
            {
                return TaskTimeEntry == null ? 0.0 : TaskTimeEntry.Value.TotalHours;
            }
        }
        public long ActivityCount { get; set; }
        public double ProjectEstimatedHours { get; set; }
        public double TaskEstimatedHours { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ModuleCode { get; set; }
        public string CategoryCode { get; set; }
        public string TemplateCode { get; set; }
    }
}
