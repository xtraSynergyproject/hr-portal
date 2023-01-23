using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
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
