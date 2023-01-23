using System;
using System.Linq;

namespace ERP.Data.GraphModel
{

    public partial class NTS_NoteMultipleFieldValue : NodeBase
    {
        public virtual long? VersionNo { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public long NoteId { get; set; }
        public long? RowId { get; set; }

    }

    public class R_NoteMultipleFieldValue_NoteFieldValue : RelationshipBase
    {

    }

    public class R_NoteMultipleFieldValue_TemplateField : RelationshipBase
    {

    }
    public class R_NoteMultipleFieldValue_Note : RelationshipBase
    {

    }
}
