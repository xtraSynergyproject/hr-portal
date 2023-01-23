using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("CandidateComputerProficiency", Schema = "rec")]
    public class CandidateComputerProficiency : DataModelBase
    {
        [ForeignKey("CandidateProfile")]
        public string CandidateProfileId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }       
        public string Program { get; set; }
        [ForeignKey("ListOfValue")]
        public string ProficiencyLevel { get; set; }
        public bool IsLatest { get; set; }
    }
}
