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
    public class PreJoiningFormalitiesViewModel : ViewModelBase
    {
        public string PreJoiningFormalitiesNo { get; set; }
        public string JobTitle { get; set; }
        public string CandidateName { get; set; }
        public ApplicationStatusEnum? PreJoinFormalStatus { get; set; }

        public long? ApplicationId { get; set; }
        [Display(Name = "Application No")]
        public string ApplicationNo { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Gender")]
        public GenderEnum? Gender { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }
        public double? Score { get; set; }
        [Display(Name = "Resume")]
        public long? ResumeId { get; set; }
        [Display(Name = "Photo")]
        public long? PhotoId { get; set; }
        public ApplicationStateEnum? ApplicationState { get; set; }

    }

}


