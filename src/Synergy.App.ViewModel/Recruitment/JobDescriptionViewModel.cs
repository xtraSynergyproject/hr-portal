using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class JobDescriptionViewModel : JobDescription
    {
        public string TaskStatus { get; set; }
        public string JobCriteria { get; set; }
        public string Skills { get; set; }
        public string OtherInformation { get; set; }
        public string JobDescriptionNoteId { get; set; }       

    }
}
