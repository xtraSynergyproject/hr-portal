
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CMS.Common;

namespace CMS.UI.ViewModel
{
    public class ProgramDashboardViewModel
    {
        public string SuperServiceId { get; set; }
        public string OwnerUserId { get; set; }
        public string RequestedByUserId { get; set; }
        public string Id { get; set; }
        public string ServiceNo { get; set; }
        public string Description { get; set; }
        public string ObjectiveId { get; set; }
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
        public string ProjectStatusCode { get; set; }
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

        public InboxTypeEnum? InboxType { get; set; }
        public string TaskNo { get; set; }
        public string ServiceSubject { get; set; }
        public string DocumentStatus { get; set; }
        public string MasterStageName { get; set; }
        public string MasterStageId { get; set; }
        public List<StageViewModel> StageList { get; set; }
        public long PlannedCount { get; set; }
        public long PlannedOverDueCount { get; set; }

    }

    public class StageViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StageName { get; set; }
        public string StageId { get; set; }
        public string Status { get; set; }
        public long Goals { get; set; }
        public long Competency { get; set; }
        public string PerformanceDocumentId { get; set; }

    }
}
