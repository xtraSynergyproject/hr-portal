using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
    public partial class HRS_Hierarchy : NodeBase
    {
        public HierarchyTypeEnum HierarchyType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Level1Name { get; set; }
        public string Level2Name { get; set; }
        public string Level3Name { get; set; }
        public string Level4Name { get; set; }
        public string Level5Name { get; set; }
    }
    public class R_Hierarchy_RootNode_PositionRoot : RelationshipDatedBase
    {
        public virtual DateTime EffectiveStartDate { get; set; }
        public virtual DateTime EffectiveEndDate { get; set; }
    }
    public class R_Hierarchy_RootNode_OrganizationRoot : RelationshipDatedBase
    {
        public virtual DateTime EffectiveStartDate { get; set; }
        public virtual DateTime EffectiveEndDate { get; set; }
    }
    public partial class HRS_Hierarchy_Log : HRS_Hierarchy
    {
    }
    }
