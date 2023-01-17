using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class HRS_OrganizationRoot : RootNodeBase
    {

    }
  
    public partial class HRS_Organization : NodeDatedBase
    {
        [NotMapped]
        public long OrganizationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DocumentCode { get; set; }
        public string NameLocal { get; set; }

        public OrganizationCategoryEnum OrganizationCategory { get; set; }
        public RestaurantTypeEnum? RestaurantType { get; set; }
        public string Description { get; set; }
        public ApprovalStatusEnum ApprovalStatus { get; set; }
        [NotMapped]
        public override bool IsActive(DateTime? asofDate = null)
        {
            return base.IsActive(asofDate) && ApprovalStatus == ApprovalStatusEnum.Approved;
        }

        public bool? IsPayrollOrganization { get; set; }
    }
    public class R_OrganizationRoot : RelationshipDatedBase
    {

    }
    public class R_Organization_CostCenter : RelationshipBase
    {

    }
    public class R_Organization_Location : RelationshipBase
    {
    }
    public class R_Organization_OrganizationType_ListOfValue : RelationshipBase
    {
    }
    public class R_Organization_OrganizationCategory_ListOfValue : RelationshipBase
    {
    }
    public class R_Organization_Admin_User : RelationshipBase
    {
    }
    public class R_OrganizationRoot_ParentOrganizationRoot : RelationshipDatedBase
    {
        public virtual DateTime? EffectiveStartDate { get; set; }
        public virtual DateTime? EffectiveEndDate { get; set; }
        public virtual long HierarchyId { get; set; }
        public virtual bool IsLatest { get; set; }
    }
    public class R_Organization_Brand_OrganizationRoot : RelationshipBase
    {
    }
    public class R_Organization_Restaurant_OrganizationRoot : RelationshipBase
    {
    }
    public class R_Organization_LegalEntity_OrganizationRoot : RelationshipBase
    {
    }
    public class R_Organization_Payroll_OrganizationRoot : RelationshipBase
    {
    }
    public class R_Organization_ResponsibilityCenter : RelationshipBase
    {

    }
    public partial class HRS_Organization_Log : HRS_Organization
    {
        public string ParentOrganizationName { get; set; }
        public string ResponsibilityCenterName { get; set; }
        public string CostCenterName { get; set; }
        public string LocationName { get; set; }
      //  public string OrganizationTypeName { get; set; }
        public string LegalEntityName { get; set; }
        public string PayrollOrganizationName { get; set; }
      //  public bool? IsPayrollOrganization { get; set; }
        public string AdminUserName { get; set; }

    }
}
