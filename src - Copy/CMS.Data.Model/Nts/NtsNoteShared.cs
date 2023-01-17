using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NtsNoteShared : DataModelBase
    {

        [ForeignKey("NoteSharedWithType")]
        public string NoteSharedWithTypeId { get; set; }
        public LOV NoteSharedWithType { get; set; }


        [ForeignKey("SharedWithUser")]
        public string SharedWithUserId { get; set; }
        public User SharedWithUser { get; set; }




        [ForeignKey("SharedWithTeam")]
        public string SharedWithTeamId { get; set; }
        public Team SharedWithTeam { get; set; }




        [ForeignKey("SharedBy")]
        public string SharedByUserId { get; set; }
        public User SharedBy { get; set; }


        [ForeignKey("NtsNote")]
        public string NtsNoteId { get; set; }
        public NtsNote NtsNote { get; set; }

        public DateTime SharedDate { get; set; }
        public WorkBoardContributionTypeEnum ContributionType { get; set; }

    }
    [Table("NtsNoteSharedLog", Schema = "log")]
    public class NtsNoteSharedLog : NtsNoteShared
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
