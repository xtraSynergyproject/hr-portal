using ERP.Utility;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_NotePermission : NodeBase
    {
        public DmsPermissionTypeEnum PermissionType { get; set; }
        public DmsAccessEnum Access { get; set; }
        public string InheritedFrom { get; set; }
        public DmsAppliesToEnum AppliesTo { get; set; }

        public bool? Iswoner { get; set; }

    }
    public class R_NotePermission_Note : RelationshipBase
    {
        public bool IsInherited { get; set; }
    }
    public class R_NotePermission_WorkspacePermissionGroup : RelationshipBase
    {

    }
    public class R_NotePermission_User : RelationshipBase
    {

    }
    //public class R_NotePermission_InheritFrom_Note : RelationshipBase
    //{

    //}
}
