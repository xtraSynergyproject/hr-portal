using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class MenuGroupDetails : DataModelBase
    {

        public string Details { get; set; }
        [ForeignKey("MenuGroup")]
        public string MenuGroupId { get; set; }
        public MenuGroup MenuGroup { get; set; }
    }
    [Table("MenuGroupDetailsLog", Schema = "log")]
    public class MenuGroupDetailsLog : MenuGroupDetails
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
