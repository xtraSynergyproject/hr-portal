using CMS.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class PageMember : DataModelBase
    {
        public string PortalId { get; set; }
        public string PageId { get; set; }
        public string MemberId { get; set; }
    }
    [Table("PageMemberLog", Schema = "log")]
    public class PageMemberLog : PageMember
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
