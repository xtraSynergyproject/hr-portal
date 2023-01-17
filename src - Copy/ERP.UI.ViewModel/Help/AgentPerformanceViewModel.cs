using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class AgentPerformanceViewModel : ViewModelBase
    {
        public long TaskId { get; set; }
        public long? UserId { get; set; }
        public long? TeamId { get; set; }
        [Display(Name = "Department Name")]
        public string TeamName { get; set; }
        [Display(Name = "Agent Name")]
        public string AgentName { get; set; }

        [Display(Name = "TaskNo", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string TaskNo { get; set; }
        public string TaskStatusName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]

        public DateTime? EndDate { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Closed Before Time")]
        public DateTime DueDate { get; set; }
        [Display(Name = "Average Performance Rank in %")]
        public double? AverageRating { get; set; }

        [Display(Name = "Performance Rating")]
        public double? Rank { get; set; }
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
        public string Remark { get; set; }
        [Display(Name = "Total Assigned Tickets")]
        public long? TotalTicket { get; set; }

        [Display(Name = "Total Closed Tickets")]
        public long? TotalClosedTicket { get; set; }
        public long? OverDue { get; set; }
        [Display(Name = "Closed On-Time")]
        public long? CloseOnTime { get; set; }
        [Display(Name = "Pending Tickets")]
        public long? PendingTickets { get; set; }
        public long? Delayed { get; set; }

    }
    public class AgentPerformanceRatingViewModel : ViewModelBase
    {
        public double Weightage { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }
        public long TaskId { get; set; }
    }
}
