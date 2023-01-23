using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("CandidateExperienceByCountry", Schema = "rec")]
    public class CandidateExperienceByCountry : DataModelBase
    {
        [ForeignKey("CandidateProfile")]
        public string CandidateProfileId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }
        [ForeignKey("Country")]
        public string CountryId { get; set; }
        public double? NoOfYear { get; set; }
        public bool IsLatest { get; set; }
    }
}
