using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("ManpowerRecruitmentSummaryVersion", Schema = "rec")]
    public class ManpowerRecruitmentSummaryVersion : DataModelBase
    {
        [ForeignKey("ManpowerRecruitmentSummary")]
        public string ManpowerRecruitmentSummaryId { get; set; }
        public ManpowerRecruitmentSummary ManpowerRecruitmentSummary { get; set; }
        public string JobId { get; set; }
        public string OrganizationId { get; set; }
        public long? Requirement { get; set; }
        public long? Seperation { get; set; }
        public long? Available { get; set; }
        public long? Planning { get; set; }
        public long? Transfer { get; set; }
        public long? Balance { get; set; }
      

    }
}
