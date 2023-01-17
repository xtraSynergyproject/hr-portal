using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Synergy.App.DataModel
{
    public class UserRolePortal : DataModelBase
    {
        [ForeignKey("UserRole")]
        public string UserRoleId { get; set; }
        public UserRole UserRole { get; set; }

        [ForeignKey("Portal")]
        public string PortalId { get; set; }
        public Portal Portal { get; set; }
    }
    [Table("UserRolePortalLog", Schema = "log")]
    public class UserRolePortalLog : UserRolePortal
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
