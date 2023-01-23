using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Synergy.App.DataModel
{
    public class UserRolePermission : DataModelBase
    {
        [ForeignKey("UserRole")]
        public string UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
        [ForeignKey("Page")]
        public string PageId { get; set; }
        public Page Page { get; set; }
        public string[] Permissions { get; set; }
    }
    [Table("UserRolePermissionLog", Schema = "log")]
    public class UserRolePermissionLog : UserRolePermission
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
