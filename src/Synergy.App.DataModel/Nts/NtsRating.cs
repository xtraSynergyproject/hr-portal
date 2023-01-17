using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsRating : DataModelBase
    {
        public NtsTypeEnum NtsType { get; set; }
        public string NtsId { get; set; }
        public int Rating { get; set; }
        public string RatingComment { get; set; }
        [ForeignKey("RatedByUser")]
        public string RatedByUserId { get; set; }
        public User RatedByUser { get; set; }

    }
    [Table("NtsRatingLog", Schema = "log")]
    public class NtsRatingLog : NtsRating
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
