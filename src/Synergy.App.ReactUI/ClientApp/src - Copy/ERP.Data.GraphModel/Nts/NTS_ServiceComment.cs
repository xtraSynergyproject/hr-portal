namespace ERP.Data.GraphModel
{
  
    public partial class NTS_ServiceComment : NodeBase
    {       
       // public long ServiceId { get; set; }
       // public long? ParentServiceCommentId { get; set; }
        public string Comment { get; set; }
        public long? ServiceVersionId { get; set; }
        public bool? IsCommentDelete { get; set; }
        //public long? CommentedByUserId { get; set; }

    }
    public class R_ServiceComment_Service : RelationshipBase
    {

    }
    public class R_ServiceComment_Parent_ServiceComment : RelationshipBase
    {

    }
    public class R_ServiceComment_CommentedBy_User : RelationshipBase
    {

    }
}
