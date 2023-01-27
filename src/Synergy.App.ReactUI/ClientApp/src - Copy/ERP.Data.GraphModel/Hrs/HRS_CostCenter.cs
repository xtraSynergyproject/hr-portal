

using System;
using System.Linq;

namespace ERP.Data.GraphModel
{

    public partial class HRS_CostCenter : NodeBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public partial class HRS_CostCenter_Log : HRS_CostCenter
    {
        public string CostCenterHolder { get; set; }
        public string LegalEntityName { get; set; }
    }
    public class R_CostCenter_PositionRoot : RelationshipBase
    {

    }
    public class R_CostCenter_LegalEntity_OrganizationRoot : RelationshipBase
    {
    }
}
