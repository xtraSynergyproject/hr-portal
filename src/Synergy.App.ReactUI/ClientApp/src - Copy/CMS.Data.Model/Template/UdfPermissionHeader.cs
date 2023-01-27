using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class UdfPermissionHeader : DataModelBase
    {
        public AssignedTypeEnum UserType { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Team")]
        public string TeamId { get; set; }
        public Team Team { get; set; }

        [ForeignKey("Page")]
        public string PageId { get; set; }
        public Page Page { get; set; }
        public NtsTypeEnum NtsType { get; set; }
        [ForeignKey("PrimaryTemplate")]
        public string PrimaryTemplateId { get; set; }

     
        public Template PrimaryTemplate { get; set; }
        public string[] CategoryCodes { get; set; }
        public string[] TemplateCodes { get; set; }
    }
    [Table("UdfPermissionHeaderLog", Schema = "log")]
    public class UdfPermissionHeaderLog : UdfPermissionHeader
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
