using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class AssessmentResultSubmit
    {
        public List<UserAssessmentResultViewModel> Created { get; set; }
        public List<UserAssessmentResultViewModel> Updated { get; set; }
        public List<UserAssessmentResultViewModel> Destroyed { get; set; }
    }
}
