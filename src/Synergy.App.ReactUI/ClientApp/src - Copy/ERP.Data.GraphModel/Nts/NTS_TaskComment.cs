using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_TaskComment : NodeBase
    {
        public string Comment { get; set; }
        public long? TaskVersionId { get; set; }
        public bool? IsCommentDelete { get; set; }
    }

    public class R_TaskComment_CommentedBy_User : RelationshipBase
    {

    }
    public class R_TaskComment_Task : RelationshipBase
    {

    }
    public class R_TaskComment_Parent_TaskComment : RelationshipBase
    {

    }
}
