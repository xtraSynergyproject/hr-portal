
using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
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
