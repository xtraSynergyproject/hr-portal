using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class Member : DataModelBase
    {
        public string UserName { get; set; }
        public long SynergyUserId { get; set; }
        public string[] MemberGroups { get; set; }
        public string[] MemberPortals { get; set; }
    }
    [Table("MemberLog", Schema = "log")]
    public class MemberLog : Member
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
