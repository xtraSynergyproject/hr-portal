using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{ 
    public class TaskWorkTimeViewModel 
    {
        
        public DateTime StartDate { get; set; }
        public TimeSpan SLA { get; set; }
        public DateTime EndDate { get; set; }

       // [Required]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }

        [Display(Name = "Task Group")]
        public string TaskGroup { get; set; }

        [Display(Name = "Task Name")]
        public string TaskName { get; set; }

        [Display(Name = "Assignee Name")]
        public string AssigneeName { get; set; }
        public string TaskStatusName { get; set; }

        public string AssigneeId { get; set; }

        [Required]
        [Display(Name = "Task Start Date")]
       
        public DateTime TaskStartDate { get; set; }

        [Required]
        [Display(Name = "Task Due Date")]
        
        public DateTime TaskDueDate { get; set; }

        public TimeSpan? Duration { get; set; }
       
        public string DurationText
        {
            get
            {
                if (Duration != null)
                {
                    return Duration.Value.ToString(@"d\.hh\:mm");
                }
                return "";
            }

        }
        [Required]
        [Display(Name = "Work Comment")]
        public string WorkComment { get; set; }
        [Display(Name = "Owner User")]
        public string OwnerUserId { get; set; }
        [Display(Name = "Owner User")]
        public string OwnerUserName { get; set; }
        [Display(Name = "Task")]
        public string TaskId { get; set; }
        [Display(Name = "Service")]
        public string ServiceId { get; set; }
        [Display(Name = "Task No")]
        public string TaskNo { get; set; }
        [Display(Name = "Time Log Date")]
       
        public DateTime? TimeLogDate { get; set; }
        public bool? IsBillable { get; set; }

        [Display(Name = "Work Start Time")]

        public DateTime? WSDate { get; set; }

        [Display(Name = "Work End Time")]
       
        public DateTime? WEDate { get; set; }

    }

}


