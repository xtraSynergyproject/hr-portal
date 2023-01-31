using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("CandidateDrivingLicense", Schema = "rec")]
    public class CandidateDrivingLicense : DataModelBase
    {
        [ForeignKey("CandidateProfile")]
        public string CandidateProfileId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }
        [ForeignKey("Country")]
        public string CountryId { get; set; }
        [ForeignKey("ListOfValue")]
        public string LicenseType { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ValidUpTo { get; set; }
        public bool IsLatest { get; set; }
    }
}
