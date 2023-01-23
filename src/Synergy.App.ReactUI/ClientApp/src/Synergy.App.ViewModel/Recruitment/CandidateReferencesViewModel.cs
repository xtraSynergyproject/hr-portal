using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class CandidateReferencesViewModel : CandidateReferences
    {
        public string CandidateId { get; set; }
        public string CandidateProfileId { get; set; }

        public string Name { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsLatest { get; set; }
        public string NoteId { get; set; }

        public string Json { get; set; }


    }
}
