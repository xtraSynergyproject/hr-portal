using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class PageDetails : DataModelBase
    {
        public string Details { get; set; }
        [ForeignKey("Page")]
        public string PageId { get; set; }
        public Page Page { get; set; }
    }
    [Table("PageDetailsLog", Schema = "log")]
    public class PageDetailsLog : PageDetails
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
