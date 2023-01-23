using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NtsNoteLog : NtsNote
    {
        [ForeignKey("NtsNote")]
        public string NtsNoteId { get; set; }
        public NtsNote NtsNote { get; set; }
        public DateTime LogDate { get; set; }
        public string LogByUserId { get; set; }
    }

}
