using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("CandidateProject", Schema = "rec")]
    public class CandidateProject : DataModelBase
    {
        [ForeignKey("CandidateProfile")]
        public string CandidateProfileId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }
        public string Currency { get; set; }
        public double? Value { get; set; }
        public string Client { get; set; }
        public string Consultant { get; set; }
        public DateTime? ConstructionPeriodFrom { get; set; }
        public DateTime? ConstructionPeriodTo { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public bool IsLatest { get; set; }
    }
}
