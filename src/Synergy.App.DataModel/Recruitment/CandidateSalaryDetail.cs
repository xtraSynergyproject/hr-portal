using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("CandidateSalaryDetail", Schema = "rec")]
    public class CandidateSalaryDetail : DataModelBase
    {
        [ForeignKey("CandidateProfile")]
        public string CandidateProfileId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }      
        public double? OverseasSalary { get; set; }
        public double? IndianSalary { get; set; }
        public double? NetSalary { get; set; }
        public double? OtherAllowances { get; set; }
        public bool IsLatest { get; set; }
    }
}
