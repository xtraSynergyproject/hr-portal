using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("CandidateLanguageProficiency", Schema = "rec")]
    public class CandidateLanguageProficiency : DataModelBase
    {
        [ForeignKey("CandidateProfile")]
        public string CandidateProfileId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }
        [ForeignKey("ListOfValue")]
        public string Language { get; set; }
        [ForeignKey("ListOfValue")]
        public string ProficiencyLevel { get; set; }
        public bool IsLatest { get; set; }
    }
}
