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
    public class RegisterViewModel : ViewModelBase
    {
       
        [Display(Name = "Candidate No")]
        public string CandidateNo { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Display(Name = "Gender")]
        public GenderEnum? Gender { get; set; }
       
        [Display(Name = "Marital Status")]
        public MaritalStatusEnum? MaritalStatus { get; set; }
        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }
     
        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }
      
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }
       
        public string DisplayName { get { return string.Concat(FirstName, " ", LastName); } }

        [Display(Name = "Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
      
        [Display(Name = "Application Id")]
        public long? ApplicationId { get; set; }
        [Display(Name = "Nationality Id")]
        public long? NationalityId { get; set; }

        [Display(Name = "Application No")]
        public string ApplicationNo { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public long? InterviewAssessmentId { get; set; }
        public long? JobOfferId { get; set; }
        public long? RejectionId { get; set; }
        public long? CollectDocumentId { get; set; }
        public long? ContractId { get; set; }
        public long? OnBoardId { get; set; }
        public long? WelcomePackId { get; set; }
    }

}


