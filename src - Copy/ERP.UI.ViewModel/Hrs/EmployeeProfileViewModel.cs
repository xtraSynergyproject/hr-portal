using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class EmployeeProfileViewModel
    {
        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? OrganizationId { get; set; }
        public int? JobId { get; set; }
        public int? GradeId { get; set; }
        public int? PositionId { get; set; }
        public int? LocationId { get; set; }

        [Display(Name = "Direct Reportee(s)")]
        public int DirectChildCount { get; set; }
        [Display(Name = "All Reportee(s)")]
        public int AllChildCount { get; set; }

        [Display(Name = "Vacant Position(s)")]
        public int VacantPositionCount { get; set; }

        public string EmployeeNo { get; set; }
        [Display(Name = "Cost Center")]
        public string CostCenterCode { get; set; }


        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "Years Of Service")]
        public string YearsOfService { get; set; }


        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string DisplayName { get; set; }
        [Display(Name = "Full Name")]
        public string EmployeeFullName { get; set; }

        [DisplayFormat(DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime DoB { get; set; }

        [Display(Name = "Date Of Join")]
        [DisplayFormat(DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime DoJ { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }

        [Display(Name = "Organization")]
        public string OrganizationName { get; set; }
        public string JobTitle { get; set; }
        [Display(Name = "Grade")]
        public string GradeName { get; set; }
        [Display(Name = "Job Title")]
        public string PositionTitle { get; set; }
        [Display(Name = "Location")]
        public string LocationName { get; set; }

        public string Group { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Section { get; set; }
        public string Unit { get; set; }



        public int? PhotoVersion { get; set; }

    }
}


