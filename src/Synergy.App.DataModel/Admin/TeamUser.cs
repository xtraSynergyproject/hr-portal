using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Synergy.App.DataModel
{
    public class TeamUser : DataModelBase
    {
        [ForeignKey("Team")]
        public string TeamId { get; set; }
        public Team Team { get; set; }
       
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        public bool IsTeamOwner { get; set; }

    }
    [Table("TeamUserLog", Schema = "log")]
    public class TeamUserLog : TeamUser
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
       public DateTime LogEndDateTime { get; set; } 
        public bool IsDatedLatest { get; set; } 
        public bool IsVersionLatest { get; set; }
    }
}
