using System;
using System.Linq;

namespace ERP.Data.GraphModel
{

    public partial class NTS_NoteTagView : NodeBase
    {
        public string Name { get; set; }
        public bool SetAsDefaultView { get; set; } 

    }

    public class R_NoteTagView_User : RelationshipBase
    {

    }

    public class R_NoteTagView_GroupBy_Note : RelationshipBase
    {
        public long SequenceNo { get; set; }
    }
     
}
