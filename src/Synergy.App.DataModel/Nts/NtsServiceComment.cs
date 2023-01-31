using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsServiceComment : DataModelBase
    {
        public string CommentSubject { get; set; }
        public string Comment { get; set; }
        public DateTime CommentedDate { get; set; }      

        [ForeignKey("CommentedByUser")]
        public string CommentedByUserId { get; set; }

        public User CommentedByUser { get; set; }


        [ForeignKey("NtsService")]
        public string NtsServiceId { get; set; }

        public NtsService NtsService { get; set; }


        [ForeignKey("ParentComment")]
        public string ParentCommentId { get; set; }
        public NtsServiceComment ParentComment { get; set; }

        public CommentToEnum CommentedTo { get; set; }
        public string AttachmentId { get; set; }
    }
    [Table("NtsServiceCommentLog", Schema = "log")]
    public class NtsServiceCommentLog : NtsServiceComment
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
