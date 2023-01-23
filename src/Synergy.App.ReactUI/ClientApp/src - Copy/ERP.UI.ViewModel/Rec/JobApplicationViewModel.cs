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
    public class JobApplicationViewModel : ViewModelBase
    {
        public ApplicationViewModel Application { get; set; }
        public ApplicationCandidateViewModel Candidate { get; set; }
        public JobRequestViewModel Job { get; set; }
        public List<ApplicationQualificationViewModel> Qualification { get; set; }
        public List<ApplicationTrainingViewModel> Training { get; set; }
        public List<ApplicationWorkExperienceViewModel> WorkExperience { get; set; }



    }

}


