using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Synergy.App.DataModel
{
    public class UserRolePreference : DataModelBase
    {
        [ForeignKey("UserRole")]
        public string UserRoleId { get; set; }
        public UserRole UserRole { get; set; }

        [ForeignKey("PreferencePortal")]
        public string PreferencePortalId { get; set; }
        public Portal PreferencePortal { get; set; }

        [ForeignKey("DefaultLandingPage")]
        public string DefaultLandingPageId { get; set; }
        public Page DefaultLandingPage { get; set; }

    }
    [Table("UserRolePreferenceLog", Schema = "log")]
    public class UserRolePreferenceLog : UserRolePreference
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
