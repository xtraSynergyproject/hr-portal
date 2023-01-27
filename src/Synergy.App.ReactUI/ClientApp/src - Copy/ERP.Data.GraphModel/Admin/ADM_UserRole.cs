using ERP.Utility;

namespace ERP.Data.GraphModel
{
    public partial class ADM_UserRole : NodeBase
    {       
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

    }
    public class R_UserRole_Action : RelationshipBase
    {

    }
    public class R_UserRole_GeoLocation : RelationshipBase
    {

    }
    public class R_UserRole_Field : RelationshipBase
    {
        public bool IsEditable { get; set; }
    }
    public class R_UserRole_Permission_Hierarchy : RelationshipBase
    {
        public HierarchyPermissionEnum HierarchyPermission { get; set; }
        public long? CustomPermissionId { get; set; }
    }
    public class R_UserRole_Mapping_TemplateMaster : RelationshipBase
    {
    }
    public class R_UserRole_Mapping_OrganizationRoot : RelationshipBase
    {
        public bool ExcludeAllChild { get; set; }
    }
}
