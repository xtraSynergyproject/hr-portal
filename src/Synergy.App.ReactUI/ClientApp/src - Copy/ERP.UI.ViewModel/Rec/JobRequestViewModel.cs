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
    public class JobRequestViewModel : ViewModelBase
    {
       
        public DateTime chekin_on_after { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
        [Required]
        public long? JobId { get; set; }
        public long? ManpowerRequestId { get; set; }       
        public string ManpowerRequestNo { get; set; }
        public int NoOfPositions { get; set; }
        [Required]
        public string JobDescription { get; set; }
        public string SkillSet { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? PostedDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Required]
        public DateTime? ExpiryDate { get; set; }
        public string Experience { get; set; }
        public string Qualification { get; set; }
        public long? NationalityId { get; set; }
        public long? JobLocationId { get; set; }
        public string Nationality { get; set; }
        public string JobLocation { get; set; }
        public JobTypeEnum? JobType { get; set; }
        [Required]
        public JobRequestStateEnum JobRequestState { get; set; }
        public JobRequestStatusEnum JobRequestStatus { get; set; }

        [Required]
        public long? RecruiterId { get; set; }
        public string Recruiter { get; set; }
        public string MinimumExperience { get; set; }
        public string MaximumExperience { get; set; }
        public List<DynamicFieldViewModel> Criteria { get; set; }
        public List<DynamicFieldViewModel> Questionaire { get; set; }
        public List<DynamicFieldViewModel> Skills { get; set; }
        public List<DynamicFieldViewModel> Others { get; set; }

        public long? CandidateId { get; set; }

        public bool IsApplied { get; set; }

        public long? UserId { get; set; }
        public string ServiceStatus { get; set; }
        public long? TotalCandidateApplied { get; set; }
        public long? TotalCandidateShortlisted { get; set; }

        public long? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? FromDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ToDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? JoiningDate { get; set; }
        public string CandidateName { get; set; }
        public DateTime? InterviewScheduledDate { get; set; }
        public ApplicationStateEnum? ApplicationState { get; set; }

    }

}


