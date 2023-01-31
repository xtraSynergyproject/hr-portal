using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using static ERP.Utility.Constant;

namespace ERP.UI.ViewModel
{
    public class ApplicationWorkExperienceViewModel : ViewModelBase
    {
        [Display(Name = "Work Experience Id")]
        public long? WorkExperienceId { get; set; }
        [Display(Name = "Snapshot Date")]
        public DateTime? SnapshotDate { get; set; }
        public long? ApplicationId { get; set; }

        [Required]
        [Display(Name = "Employer Name")]
        public string EmployerName { get; set; }

        [Display(Name = "Employer Address")]
        public string EmployerAddress { get; set; }
        [Display(Name = "Is this your current company?")]
        public bool IsCurrentEmployer { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Last Manager Name")]
        public string LastManagerName { get; set; }
        [Display(Name = "Company Address")]
        public string CompanyAddress { get; set; }
        [Required]
        [Display(Name = "Designation")]
        public string JobTitle { get; set; }
        [Display(Name = "Reason for Leaving")]
        public string ReasonForLeaving { get; set; }
        [Display(Name = "Roles and Responsibilities")]
        public string RolesAndResponsibilities { get; set; }

        public long? CandidateId { get; set; }
        public long? CountryId { get; set; }
    }

}


