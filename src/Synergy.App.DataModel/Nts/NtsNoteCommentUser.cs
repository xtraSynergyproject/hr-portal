using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsNoteCommentUser : DataModelBase
    {     

        [ForeignKey("CommentToUser")]
        public string CommentToUserId { get; set; }
        public User CommentToUser { get; set; }

        [ForeignKey("NtsNoteComment")]
        public string NtsNoteCommentId { get; set; }
        public NtsNoteComment NtsNoteComment { get; set; }

    }
   
  
}
