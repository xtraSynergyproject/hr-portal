using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
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
