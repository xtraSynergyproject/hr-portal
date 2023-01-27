using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class TerminatePersonViewModel:ViewModelBase
    {
        [Display(Name = "Person")]
        [Required]
        public string PersonId { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Person Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        [Display(Name = "Job")]
        public string JobName { get; set; }
        [Display(Name = "Position")]
        public string PositionName { get; set; }
        [Display(Name = "Grade")]
        public string GradeName { get; set; }

        [Display(Name = "Employee Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm:ss}")]
        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = ApplicationConstatnt.Annotation.DefaultDateFormat)]
        public DateTime? PersonEffectiveStartDate { get; set; }
        [Display(Name = "Employee End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm:ss}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? PersonEffectiveEndDate { get; set; }

        [Display(Name = "Assignment Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm:ss}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? AssignmentEffectiveStartDate { get; set; }
        [Display(Name = "Assignment End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm:ss}")]
        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? AssignmentEffectiveEndDate { get; set; }

        [Display(Name = "Contract Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm:ss}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ContractEffectiveStartDate { get; set; }
        [Display(Name = "Contract End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm:ss}")]
        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ContractEffectiveEndDate { get; set; }

        [Display(Name = "User Status")]
        public string UserStatus { get; set; }

        [Display(Name = "No Of Pending Tasks")]
        public int PendingTaskCount { get; set; }
        [Display(Name = "No Of Services Owned")]
        public int OwnedServiceCount { get; set; }
        [Display(Name = "Approval Hierarchy Dependents")]
        public int UserHierarchyDependentCount { get; set; }


        [Display(Name = "Proposed Task Assignee")]
        [Required]
        public string ProposedTaskAssignee { get; set; }

        [Display(Name = "Proposed Service Owner")]
        [Required]
        public string ProposedServiceOwner { get; set; }

        [Display(Name = "User Hierarchy Replacement")]
        [Required]
        public string ProposedUserHierarchyReplacement { get; set; }

        [Display(Name = "Terminate Date")]
        [Required]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime PersonTerminateDate { get; set; }
        public bool AllowCreateUser { get; set; }
    }
}
