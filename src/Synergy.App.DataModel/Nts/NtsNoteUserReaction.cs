using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsNoteUserReaction : DataModelBase
    {
        public bool? LikeOrDisLike { get; set; }
        public DateTime ReactedDate { get; set; }

        [ForeignKey("ReactedByByUser")]
        public string ReactedByByUserId { get; set; }

        public User ReactedByByUser { get; set; }

        [ForeignKey("NtsNote")]
        public string NtsNoteId { get; set; }

        public NtsNote NtsNote { get; set; }
    }
    [Table("NtsNoteUserReactionLog", Schema = "log")]
    public class NtsNoteUserReactionLog : NtsNoteUserReaction
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
