using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("CandidateExperience", Schema = "rec")]
    public class CandidateExperience : DataModelBase
    {
        [ForeignKey("CandidateProfile")]
        public string CandidateProfileId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }
        public string Employer { get; set; }       
        public string Location { get; set; }
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
