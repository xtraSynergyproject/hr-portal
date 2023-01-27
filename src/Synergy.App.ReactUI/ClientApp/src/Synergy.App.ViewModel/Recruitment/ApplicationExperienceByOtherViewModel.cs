using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class ApplicationExperienceByOtherViewModel : ApplicationExperienceByOther
    {      
        public string CandidateProfileId { get; set; }       
       
       
        public string OtherTypeName { get; set; }
      
    }
}
