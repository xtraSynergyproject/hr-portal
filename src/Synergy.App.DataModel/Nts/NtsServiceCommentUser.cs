using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsServiceCommentUser : DataModelBase
    {      

        [ForeignKey("CommentToUser")]
        public string CommentToUserId { get; set; }
        public User CommentToUser { get; set; }    


        [ForeignKey("NtsServiceComment")]
        public string NtsServiceCommentId { get; set; }

        public NtsServiceComment NtsServiceComment { get; set; }
    }
    
}
