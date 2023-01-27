using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class PersonExperienceDocumentViewModel : PersonDocumentViewModel
    {
        [Display(Name = "Employer Name")]
        public string EmployerName { get; set; }
        [Display(Name = "Employer Address")]
        public string EmployerAddress { get; set; }
        [Display(Name = "Current Employer")]
        public string CurrentEmployer { get; set; }
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Last Manager Name")]
        public string LastManagerName { get; set; }
        [Display(Name = "Company Address")]
        public string CompanyAddress { get; set; }
        [Display(Name = "Employee Job Title")]
        public string EmployeeJobTitle { get; set; }
        [Display(Name = "Reason For Leaving")]
        public string ReasonForLeaving { get; set; }
        [Display(Name = "Role And Responsibility")]
        public string RoleAndResponsibility { get; set; }
        public string Attachment { get; set; }

    }
}
