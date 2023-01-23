using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class CandidateExperienceByOtherViewModel : DataModelBase
    {      
        public string CandidateId { get; set; }
        public string CandidateProfileId { get; set; }

        public double? NoOfYear { get; set; }
        public bool IsLatest { get; set; }
        public string OtherTypeId { get; set; }
        public string OtherTypeName { get; set; }
        public string NoteId { get; set; }

        public string Json { get; set; }

    }
}
