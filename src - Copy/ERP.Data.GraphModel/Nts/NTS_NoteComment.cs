namespace ERP.Data.GraphModel
{
  
    public partial class NTS_NoteComment : NodeBase
    {       
       // public long NoteId { get; set; }
        //public long? ParentNoteCommentId { get; set; }
        public string Comment { get; set; }
        public long? NoteVersionId { get; set; }
        public bool? IsCommentDelete { get; set; }
      
    }
    public class R_NoteComment_Note : RelationshipBase
    {

    }
    public class R_NoteComment_Parent_NoteComment : RelationshipBase
    {

    }
    public class R_NoteComment_CommentedBy_User : RelationshipBase
    {

    }
    public class R_NoteComment_CommentedTo_User : RelationshipBase
    {

    }
}
