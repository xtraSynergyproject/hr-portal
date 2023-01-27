using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("OrganizationDocument", Schema = "rec")]
    public class OrganizationDocument : DataModelBase
    {
        [ForeignKey("Organization")]
        public string OrganizationId { get; set; }
        public string AttachmentId { get; set; }
        public long? Version { get; set; }
    }
}
