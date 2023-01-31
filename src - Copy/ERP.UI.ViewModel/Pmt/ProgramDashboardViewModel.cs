using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class ProgramDashboardViewModel
    {
        public long Id { get; set; }    
        public long ProjectCount { get; set; }
        public long ClosedTaskCount { get; set; }
        public long AllTaskCount { get; set; }
        public long UserCount { get; set; }
        public long AllTime { get; set; }
        public long FileCount { get; set; }
        public string ProjectName { get; set; }
        public string ProjectStatus { get; set; }
        public string TemplateName { get; set; }
        public string CreatedBy { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime DueDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime CreatedOn { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime UpdatedOn { get; set; }
        public NtsPriorityEnum? Priority { get; set; }
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
    }
}
