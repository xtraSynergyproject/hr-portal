using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("ApplicationExperience", Schema = "rec")]
    public class ApplicationExperience : DataModelBase
    {
        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        public string Employer { get; set; }
        [ForeignKey("Location")]
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
