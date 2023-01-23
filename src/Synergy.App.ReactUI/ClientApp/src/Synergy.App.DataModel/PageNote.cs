using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class PageNote : DataModelBase
    {
        [ForeignKey("NtsCategory")]
        public string NtsCategoryId { get; set; }
        public NtsCategory NtsCategory { get; set; }

        [ForeignKey("Page")]
        public string PageId { get; set; }
        public Page Page { get; set; }
        public int[] NtsGroupingLevel { get; set; }
    }
    [Table("PageNoteLog", Schema = "log")]
    public class PageNoteLog : PageNote
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
