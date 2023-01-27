using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class WorkStructureViewModel 
    {
        public long? Id { get; set; }
        public string ColorCode { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }

        public List<WorkStructureViewModel> Items { get; set; }

        //public long? PersonId { get; set; }
        //public long? PositionId { get; set; }
        public string Mode { get; set; }

        public List<HierarchyViewModel> ApprovalHierarchy { get; set; }

        public long? UserId { get; set; }
        [Display (Name="Select Person")]
        public long? PersonId { get; set; }
        public long? PositionId { get; set; }
        public long? JobId { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        public long? GradeId { get; set; }
        public long? LocationId { get; set; }
        public long? CostCenterId { get; set; }
        public long? AssignmentId { get; set; }
        public long? ContractId { get; set; }
        public long? SponsorId { get; set; }
        public long? ParentPositionId { get; set; }
        public long? SalaryInfoId { get; set; }


        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string SponsorshipNo { get; set; }
        public string JobName { get; set; }
        public string PositionName { get; set; }
        public string GradeName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string CostCenter { get; set; }
        public string PersonNo { get; set; }
        public long? PhotoId { get; set; }       
        public double? AnnualLeaveEntitlement { get; set; }
       
        public string PhotoName { get; set; }
        public string LocationName { get; set; }
        public string Contract { get; set; }
        public string Sponsor { get; set; }
        public string Assignment { get; set; }
        public string Dependents { get; set; }
        public string ParentPosition { get; set; }
        public string PaymentMode { get; set; }

        public string Status { get; set; }

        public string UserStatus { get; set; }
        public string PersonStatus { get; set; }
        public string AssignmentStatus { get; set; }
        public string PositionStatus { get; set; }
        public string JobStatus { get; set; }
        public string OrgStatus { get; set; }
        public string GradeStatus { get; set; }
        public string SalaryStatus { get; set; }
        public string CostCenterStatus { get; set; }
        public string LocationStatus { get; set; }
        public string ContractStatus { get; set; }
        public string SponsorStatus { get; set; }
        public string ParentPositionStatus { get; set; }

        public string UserLastUpdate { get; set; }
        public string PersonLastUpdate { get; set; }
        public string AssignmentLastUpdate { get; set; }
        public string PositionLastUpdate { get; set; }
        public string JobLastUpdate { get; set; }
        public string OrgLastUpdate { get; set; }
        public string GradeLastUpdate { get; set; }
        public string SalaryLastUpdate { get; set; }
        public string CostCenterLastUpdate { get; set; }
        public string LocationLastUpdate { get; set; }
        public string ContractLastUpdate { get; set; }
        public string SponsorLastUpdate { get; set; }
        public string ParentPositionLastUpdate { get; set; }

        public List<DependentViewModel> DependentList { get; set; }

    }
}
