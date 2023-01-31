using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("ApplicationSalaryDetail", Schema = "rec")]
    public class ApplicationSalaryDetail : DataModelBase
    {
        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
        public Application Application { get; set; }      
        public double? OverseasSalary { get; set; }
        public double? IndianSalary { get; set; }
        public double? NetSalary { get; set; }
        public double? OtherAllowances { get; set; }
        public bool IsLatest { get; set; }
    }
}
