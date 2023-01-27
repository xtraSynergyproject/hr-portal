using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Synergy.App.DataModel
{
    [Table("HiringManagerOrganization", Schema = "rec")]
    public class HiringManagerOrganization : DataModelBase
    {
        [ForeignKey("HiringManager")]
        public string HiringManagerId { get; set; }
        public HiringManager HiringManager { get; set; }

        [ForeignKey("Organization")]
        public string OrganizationId { get; set; }
    }
}
