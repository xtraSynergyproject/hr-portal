//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ERP.Utility;

namespace ERP.Data.GraphModel
{        

    public partial class ADM_Team : NodeBase
    {       
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }        
        public string GroupCode { get; set; }
        public WorkAssignmentTypeEnum TeamWorkAssignmentType { get; set; }
        public TeamTypeEnum? TeamType { get; set; }
        public string Color { get; set; }
    }
    public class R_Team_User : RelationshipBase
    {
        public bool IsTeamOwner { get; set; }
    }
    public class R_Team_OrganizationRoot : RelationshipBase
    {

    }
    public class R_Team_ParentTeam : RelationshipBase
    {


    }
    public class R_Team_SupportTicket_TemplateCategory : RelationshipBase
    {

    }

}
