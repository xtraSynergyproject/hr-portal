using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class CandidateExperienceBySectorViewModel : DataModelBase
    {      
        public string CandidateId { get; set; }
        public string CandidateProfileId { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
        public string Category { get; set; }
        public string NoOfYear { get; set; }
        public bool IsLatest { get; set; }
        public string SectorName { get; set; }
        public string IndustryName { get; set; }
        public string CategoryName { get; set; }
        public string NoteId { get; set; }

        public string Json { get; set; }
        public string SectorId { get; set; }
        public string IndustryId { get; set; }
        public string CategoryId { get; set; }
    }
}
