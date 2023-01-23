using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NtsNoteStatusTrack : DataModelBase
    {

        [ForeignKey("NtsNote")]
        public string NtsNoteId { get; set; }
        public NtsNote NtsNote { get; set; }

        [ForeignKey("NoteStatus")]
        public string NoteStatusId { get; set; }
        public LOV NoteStatus { get; set; }

        public DateTime StatusChangedDate { get; set; }
        public string StatusChangedByUserId { get; set; }

    }
    [Table("NtsNoteStatusTrackLog", Schema = "log")]
    public class NtsNoteStatusTrackLog : NtsNoteStatusTrack
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
