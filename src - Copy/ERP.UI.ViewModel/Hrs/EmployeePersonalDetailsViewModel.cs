using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
namespace ERP.UI.ViewModel
{
    public class EmployeePersonalDetailsViewModel
    {
        public int Id { get; set; }
        public int? PositionId { get; set; }

        public string EmployeeNumber { get; set; }
        public int EmployeeId { get; set; }

        [Display(Name = "Job Title")]
        public string PositionTitle { get; set; }
        [Display(Name = "Full Name")]
        public string EmployeeFullName { get; set; }

        [DisplayFormat(DataFormatString = AnnotationDeclaration.DateFormat)]
        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }
        public byte? Age { get; set; }
        public GenderEnum Gender { get; set; }

        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }
        [Display(Name = "Nationality Group")]
        public string NationalityGroup { get; set; }
        [Display(Name = "Nationality")]
        public string Nationality { get; set; }
        [Display(Name = "Adult")]
        public int? Adult { get; set; }
        [Display(Name = "Children")]
        public int? Minor { get; set; }
        public EmployeeStatusEnum Status { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }

        [DisplayFormat(DataFormatString = AnnotationDeclaration.DateFormat)]
        [Display(Name = "Date Of Join")]
        public DateTime DateOfJoin { get; set; }

        [Display(Name = "Years Of Service")]
        public string YearsOfService { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }


        public int? PhotoVersion { get; set; }

    }
}
