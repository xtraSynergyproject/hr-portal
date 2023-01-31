using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("ManpowerRecruitmentSummary",Schema ="rec")]
    public class ManpowerRecruitmentSummary : DataModelBase
    {
        [ForeignKey("Job")]
        public string JobId { get; set; }
        [ForeignKey("Organization")]
        public string OrganizationId { get; set; }
        public long? Requirement { get; set; }
        public long? Seperation { get; set; }
        public long? Available { get; set; }
        [Display(Name = "Sub Contract")]
        public long? Planning { get; set; }
        public long? Transfer { get; set; }
        public long? Balance { get; set; }
       // public string AttachmentId { get; set; }

    }
}
