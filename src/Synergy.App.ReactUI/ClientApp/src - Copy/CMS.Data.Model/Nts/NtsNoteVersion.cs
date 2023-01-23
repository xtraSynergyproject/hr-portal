using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NtsNoteVersion : NtsNote
    {
        [ForeignKey("NtsNote")]
        public string NtsNoteId { get; set; }
        public NtsNote NtsNote { get; set; }
        public DateTime VersionDate { get; set; }
        public string VersionByUserId { get; set; }
    }

}
