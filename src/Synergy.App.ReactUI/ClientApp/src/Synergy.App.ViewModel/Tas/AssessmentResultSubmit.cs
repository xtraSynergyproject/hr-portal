using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class AssessmentResultSubmit
    {
        public List<UserAssessmentResultViewModel> Created { get; set; }
        public List<UserAssessmentResultViewModel> Updated { get; set; }
        public List<UserAssessmentResultViewModel> Destroyed { get; set; }
    }
}
