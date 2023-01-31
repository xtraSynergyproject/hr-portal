using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NtsTaskCommentUser : DataModelBase
    {  
        [ForeignKey("CommentToUser")]
        public string CommentToUserId { get; set; }
        public User CommentToUser { get; set; }

        [ForeignKey("NtsTaskComment")]
        public string NtsTaskCommentId { get; set; }
        public NtsTaskComment NtsTaskComment { get; set; }
    }
   
}
