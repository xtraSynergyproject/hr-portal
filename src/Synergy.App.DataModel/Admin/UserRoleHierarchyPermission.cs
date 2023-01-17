using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Synergy.App.DataModel
{
    public class UserRoleHierarchyPermission : DataModelBase
    {
        [ForeignKey("UserRole")]
        public string UserRoleId { get; set; }
        public UserRole UserRole { get; set; }

        [ForeignKey("HierarchyMaster")]
        public string HierarchyId { get; set; }
        public HierarchyMaster HierarchyMaster { get; set; }

        public HierarchyPermissionEnum HierarchyPermission { get; set; }

        public string CustomRootId { get; set; }

    }
    [Table("UserRoleHierarchyPermissionLog", Schema = "log")]
    public class UserRoleHierarchyPermissionLog : UserRoleHierarchyPermission
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
