using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class RuleToActionsViewModel :  DatedViewModelBase
    {
  
        [Required]
        [Display(Name = "User Group Type")]
        public int RuleTypeId { get; set; }
        [Required]
        [Display(Name = "User Group Name")]
        public long RuleId { get; set; }

        public string ActionIds { get; set; }

        public string ActionsText { get; set; }
        public List<RuleToActionsGridViewModel> ActionsList { get; set; }


        //public List<GradeRuleActions> GradeList { get; set; }
        //public List<PositionRuleActions> PositionList { get; set; }
        //public List<OrganizationRuleActions> OrganizationList { get; set; }
        //public List<EmployeeRuleActions> EmployeeList { get; set; }
        //public List<JobRuleActions> JobList { get; set; }
    }
}


