using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ActivityViewModel : ViewModelBase
    {
        [Required]
        [Display(Name = "Activity")]
        public SalActivityTypeEnum? ActivityType { get; set; }
        public string Description { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? Date { get; set; }
        [Display(Name = "Date")]
        public DateTime? MeetingDate { get; set; }
        [Display(Name = "Lead Status")]
        public SalLeadStatusEnum? LeadStatus { get; set; }
        public string Lead { get; set; }
        [Display(Name = "Lead")]
        public long? LeadId { get; set; }
        [Required]
        [Display(Name = "Activity Outcome")]
        public SalActivityOutcomeEnum? ActivityOutcome { get; set; }
        public long? LeadIdNo { get; set; }
        public string AssignTo { get; set; }
        [Display(Name = "Activity Status")]
        public SalActivityStatusEnum? ActivityStatus { get; set; }
        public long? LeadPersonId { get; set; }

    }
}

