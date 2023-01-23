using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("ApplicationDrivingLicense", Schema = "rec")]
    public class ApplicationDrivingLicense : DataModelBase
    {
        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        [ForeignKey("Country")]
        public string CountryId { get; set; }
        [ForeignKey("ListOfValue")]
        public string LicenseType { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ValidUpTo { get; set; }
        public bool IsLatest { get; set; }
    }
}
