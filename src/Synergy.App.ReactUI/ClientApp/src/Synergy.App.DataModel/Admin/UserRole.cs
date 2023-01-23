using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class UserRole : DataModelBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public User[] Users { get; set; }
        public UserRolePermission[] UserRolePermissions { get; set; }
    }
    [Table("UserRoleLog", Schema = "log")]
    public class UserRoleLog : UserRole
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
