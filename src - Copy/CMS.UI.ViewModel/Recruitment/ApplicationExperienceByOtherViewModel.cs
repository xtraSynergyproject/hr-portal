using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class ApplicationExperienceByOtherViewModel : ApplicationExperienceByOther
    {      
        public string CandidateProfileId { get; set; }       
       
       
        public string OtherTypeName { get; set; }
      
    }
}
