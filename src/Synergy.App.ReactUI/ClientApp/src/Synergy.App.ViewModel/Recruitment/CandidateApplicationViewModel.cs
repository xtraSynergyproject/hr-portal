using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class CandidateApplicationViewModel : DataModelBase
    {
        public List<ApplicationViewModel> ApplicationList { get; set; }
        public List<ApplicationViewModel> BookmarkList { get; set; }

        public List<ApplicationViewModel> ConfidentialList { get; set; }

        public bool IsCandidateDetailsFilled { get; set; }
        public string CandidateId { get; set; }

    }
}
