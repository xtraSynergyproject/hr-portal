using CMS.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class UserGroupUser : DataModelBase
    {
        [ForeignKey("UserGroup")]
        public string UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; } 
        public User User { get; set; }
    }
    [Table("UserGroupUserLog", Schema = "log")]
    public class UserGroupUserLog : UserGroupUser
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
