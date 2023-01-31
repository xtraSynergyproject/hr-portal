using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel.Sal
{
    public class GoalViewModel:ViewModelBase
    {
        [Required]
        [Display(Name = "Target Description")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Time Period")]
        public SalGoalIntervalEnum Interval { get; set; }

        [Display(Name = "Goal Type")]
        public SalGoalTypeEnum GoalType { get; set; }

        [Display(Name = "Goal Period")]
        public long GoalPeriod { get; set; }
        [Required]
        [Display(Name = "Date From")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Date End")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Property Consultant")]
        [Required]
        public long? UsersId { get; set; }
        [Display(Name = "Team")]
        public long? TeamId { get; set; }
        [Display(Name = "Target Type")]
        public SalGoalMetricEnum Metric { get; set; }

        public long Target { get; set; }
        [Display(Name = "Property Consultant")]
        public string User { get; set; }
        public string Team { get; set; }
        public string IntervalValue { get; set; }

    }
}
