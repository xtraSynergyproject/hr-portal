using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class DataModelBase
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        [Key]
        public string Id { get; set; }

        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; }

        // [BsonIgnore]
        public string CreatedBy { get; set; }
        // [BsonDateTimeOptions(Kind = DateTimeKind.Local)]

        public DateTime LastUpdatedDate { get; set; }

        //  [BsonIgnore]
        public string LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public long? SequenceOrder { get; set; }

        // [BsonIgnore]
        public string CompanyId { get; set; }
        public string LegalEntityId { get; set; }

        [NotMapped]
        public DataActionEnum DataAction { get; set; }
        public StatusEnum Status { get; set; }
        public long VersionNo { get; set; }
        public string PortalId { get; set; }
    }
}