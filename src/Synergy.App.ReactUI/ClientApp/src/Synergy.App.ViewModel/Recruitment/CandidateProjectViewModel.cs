using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;


namespace Synergy.App.ViewModel
{
    public class CandidateProjectViewModel : CandidateProject
    {
        public string CandidateId { get; set; }
        public string CandidateProfileId { get; set; }

        public string Currency { get; set; }
        public double? Value { get; set; }
        public string Client { get; set; }
        public string Consultant { get; set; }
        public DateTime? ConstructionPeriodFrom { get; set; }
        public DateTime? ConstructionPeriodTo { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public bool IsLatest { get; set; }
        public string NoteId { get; set; }
        public string Json{ get; set; }

    }
}
