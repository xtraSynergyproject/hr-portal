using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class PageIndexColumn : DataModelBase
    {
        [ForeignKey("PageIndex")]
        public string PageIndexId { get; set; }
        public PageIndex PageIndex { get; set; }

        [ForeignKey("ColumnMetadata")]
        public string ColumnMetadataId { get; set; }
        public ColumnMetadata ColumnMetadata { get; set; }
        public string HeaderName { get; set; }
        public bool EnableSorting { get; set; }
        public bool EnableFiltering { get; set; }
    }
    [Table("PageIndexColumnLog", Schema = "log")]
    public class PageIndexColumnLog : PageIndexColumn
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
