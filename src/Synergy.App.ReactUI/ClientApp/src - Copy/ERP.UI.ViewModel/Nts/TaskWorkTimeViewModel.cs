using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class TaskWorkTimeViewModel : TemplateViewModelBase
    {
        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime EndDate { get; set; }

       // [Required]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        public long? ProjectId { get; set; }

        [Display(Name = "Task Group")]
        public string TaskGroup { get; set; }

        [Display(Name = "Task Name")]
        public string TaskName { get; set; }

        [Display(Name = "Assignee Name")]
        public string AssigneeName { get; set; }

        public long? AssigneeId { get; set; }

        [Required]
        [Display(Name = "Task Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime TaskStartDate { get; set; }

        [Required]
        [Display(Name = "Task Due Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime TaskDueDate { get; set; }

        public TimeSpan? Duration { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DayTimeFormat)]
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
        public long? OwnerUserId { get; set; }
        [Display(Name = "Owner User")]
        public string OwnerUserName { get; set; }
        [Display(Name = "Task")]
        public long? TaskId { get; set; }
        [Display(Name = "Service")]
        public long? ServiceId { get; set; }
        [Display(Name = "Task No")]
        public string TaskNo { get; set; }
        [Display(Name = "Time Log Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? TimeLogDate { get; set; }
        public bool? IsBillable { get; set; }

        [Display(Name = "Work Start Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? WSDate { get; set; }

        [Display(Name = "Work End Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? WEDate { get; set; }

    }

}


