using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class CandidateExperienceViewModel : CandidateExperience
    {
        public string AttachmentName { get; set; }
        public double? TotalDuration { get; set; }
    }
}
