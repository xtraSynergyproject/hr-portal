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
    public class ApplicationViewModel : ViewModelBase
    {
        [Display(Name = "Application No")]
        public string ApplicationNo { get; set; }
        [Display(Name = "Applied Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime AppliedDate { get; set; }
        [Display(Name = "Application Status")]
        public ApplicationStatusEnum? ApplicationStatus { get; set; }
        [Display(Name = "Application State")]
        public ApplicationStateEnum? ApplicationState { get; set; }


        public HiringStatusEnum HiringStatus { get; set; }
        public RecruitmentEmailStatusEnum? EmailSentStaus { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? InterviewDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? InterviewTime { get; set; }
        [Display(Name = "Candidate Id")]
        public long? CandidateId { get; set; }
        [Display(Name = "Job Request Id")]
        public long? JobRequestId { get; set; }
        [Display(Name = "Job Request Criteria Id")]
        public long? JobRequestCriteriaId { get; set; }


        public List<DynamicFieldViewModel> Criteria { get; set; }
        public List<DynamicFieldViewModel> Skills { get; set; }
        public List<DynamicFieldViewModel> Others { get; set; }

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
        [Required]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        public string DisplayName { get { return string.Concat(FirstName, " ", LastName); } }
        [Required]
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfBirth { get; set; }
        //[Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
      
        [Display(Name = "Nationality Id")]
        public long? NationalityId { get; set; }

        [Display(Name = "Religion")]
        public ReligionEnum? Religion { get; set; }

        [Display(Name = "Resume")]
        public long? ResumeId { get; set; }

        [Display(Name = "Photo")]
        public long? PhotoId { get; set; }

        public long? InterviewAssessmentId { get; set; }
        public long? JobOfferId { get; set; }
        public long? RejectionId { get; set; }
        public long? CollectDocumentId { get; set; }
        public long? ContractId { get; set; }
        public long? OnBoardId { get; set; }
        public long? WelcomePackId { get; set; }

        public FileViewModel SelectedFile { get; set; }
        public FileViewModel SelectedPhoto { get; set; }

        public long? UserId { get; set; }

        public double? Score { get; set; }

        [Display(Name = "Job Name")]
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        public JobTypeEnum? JobType { get; set; }
        public string JobLocation { get; set; }
        public string Experience { get; set; }

        public NtsActionEnum TemplateAction { get; set; }
        [Display(Name = "Nationality")]
        public string Nationality { get; set; }
        [Display(Name = "Department")]
        public string Department { get; set; }
        [Display(Name = "Grade")]
        public string Grade { get; set; }
        [Display(Name = "Line Manager")]
        public string LineManager { get; set; }
        [Display(Name = "Date of Join")]
        public string JoinDate { get; set; }
        [Display(Name = "Job Offer Subject")]
        public string Subject { get; set; }
        [Display(Name = "Job Offer Body")]
        public string Body { get; set; }
        [Display(Name = "Filename")]
        public string FileName { get; set; }
        [Display(Name = "Candidate Name")]
        public string CandidateName { get; set; }
        public string FileId { get; set; }
        public long? JoingChecklistId { get; set; }
        public long? PreJoingFormalitiesId { get; set; }

        public long? AssessmentId { get; set; }

        public long? AssessmentSlotId { get; set; }
    }

}


