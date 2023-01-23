using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("CandidateExperienceBySector", Schema = "rec")]
    public class CandidateExperienceBySector : DataModelBase
    {
        [ForeignKey("CandidateProfile")]
        public string CandidateProfileId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }
        [ForeignKey("ListOfValues")]
        public string Sector { get; set; }
        [ForeignKey("ListOfValues")]
        public string Industry { get; set; }
        [ForeignKey("ListOfValues")]
        public string Category { get; set; }
        public double? NoOfYear { get; set; }
        public bool IsLatest { get; set; }
    }
}
