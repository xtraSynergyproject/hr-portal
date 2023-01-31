using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class GoalViewModel : ViewModelBase
    {

        public long GoalId { get; set; }
        public long PerformanceDocumentId { get; set; }
        public long ServiceTemplateMasterId { get; set; }
        public long ServiceId { get; set; }
        public long? GoalownerUserId { get; set; }


        public string ServiceNo { get; set; }
        public string ServiceName { get; set; }
        [Display(Name = "Service Status")]
        public string ServiceStatusName { get; set; }

        public string ServiceStatusCode { get; set; }

        public string Subject { get; set; }
        public string Description { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Required]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Start Date")]
        public string StartDateDisplay { get { return StartDate.ToDD_MMM_YYYY(); } }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Required]
        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }
        [Display(Name = "Due Date")]
        public string DueDateDisplay { get { return DueDate.ToDD_MMM_YYYY(); } }

        [Display(Name = "Completed Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? CompletionDate { get; set; }
        [Required]
        [Display(Name = "SLA(In Days)")]
        public int? SLA { get; set; }
        public bool IsConfidential { get; set; }
        public PmsAccessType PmsAccessType { get; set; }
    }
}
