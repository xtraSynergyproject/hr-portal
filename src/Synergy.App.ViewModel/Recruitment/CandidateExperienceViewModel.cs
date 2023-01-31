using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class CandidateExperienceViewModel : CandidateExperience
    {
        public string AttachmentName { get; set; }
        public double? TotalDuration { get; set; }
        public string NoteId { get; set; }
        public string Json { get; set; }

        public string CandidateProfileId { get; set; }
        public string CandidateId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }
        public string Employer { get; set; }
        public string LocationId { get; set; }
        public string JobTitle { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public double? Duration { get; set; }
        public string Responsibilities { get; set; }
        public string AttachmentId { get; set; }
        public bool IsLatest { get; set; }
        public string Comment { get; set; }
    }
}
