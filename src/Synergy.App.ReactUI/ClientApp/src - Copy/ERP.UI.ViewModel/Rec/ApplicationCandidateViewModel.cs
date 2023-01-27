using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using static ERP.Utility.Constant;

using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class ApplicationCandidateViewModel : ViewModelBase
    {
      
        [Display(Name = "Candidate Id")]
        public long? CandidateId { get; set; }
        [Display(Name = "Snapshot Date")]
        public DateTime? SnapshotDate { get; set; }
        

        [Display(Name = "Candidate No")]
        public string CandidateNo { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Gender")]
        public CareerGenderEnum? Gender { get; set; }

        [Display(Name = "Marital Status")]
        public CareerMaritalStatusEnum? MaritalStatus { get; set; }
        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }

        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }
        [Required]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        public string DisplayName { get { return string.Concat(FirstName, " ", LastName); } }
        [Required]
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
        public bool IsProfileCompleted { get; set; }
        public long? InterviewAssessmentId { get; set; }
        public long? JobOfferId { get; set; }
        public long? RejectionId { get; set; }
        public long? CollectDocumentId { get; set; }
        public long? ContractId { get; set; }
        public long? OnBoardId { get; set; }
        public long? WelcomePackId { get; set; }
        public string CareerSummary { get; set; }
        [Display(Name = "Resume")]
        public long? ResumeId { get; set; }
        public FileViewModel SelectedFile { get; set; }
        [Display(Name = "Photo")]
        public long? PhotoId { get; set; }
        public FileViewModel SelectedPhoto { get; set; }
    }

}


