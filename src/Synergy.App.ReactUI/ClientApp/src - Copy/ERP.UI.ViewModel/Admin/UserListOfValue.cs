
using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class UserListOfValue
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
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

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public long? LegalEntityId { get; set; }
        public string SponsorshipNo { get; set; }
        public string JobName { get; set; }
        public string PositionName { get; set; }
        public string GradeName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string CostCenter { get; set; }
        public string PersonNo { get; set; }
        public long? PhotoId { get; set; }
        public long? PhotoVersion { get; set; }
        public bool IsSelected { get; set; }
        public double? AnnualLeaveEntitlement { get; set; }
        public string BiometricId { get; set; }

        public string PhotoName { get; set; }
        public string LocationName { get; set; }
        public string Contract { get; set; }
        public string Sponsor { get; set; }
        public string Assignment { get; set; }
        public string Dependents { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfJoin { get; set; }
        public string ParentName { get; set; }
        public string PersonStatus { get; set; }
    }
}
