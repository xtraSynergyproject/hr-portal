using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class UserListOfValue
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PersonId { get; set; }
        public string PositionId { get; set; }
        public string JobId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationId { get; set; }
        public string GradeId { get; set; }
        public string LocationId { get; set; }
        public string CostCenterId { get; set; }
        public string AssignmentId { get; set; }
        public string ContractId { get; set; }
        public string SponsorId { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string LegalEntityId { get; set; }
        public string SponsorshipNo { get; set; }
        public string JobName { get; set; }
        public string PositionName { get; set; }
        public string GradeName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string CostCenter { get; set; }
        public string PersonNo { get; set; }
        public string PhotoId { get; set; }
        public string PhotoVersion { get; set; }
        public bool IsSelected { get; set; }
        public double? AnnualLeaveEntitlement { get; set; }
        public string BiometricId { get; set; }

        public string PhotoName { get; set; }
        public string LocationName { get; set; }
        public string Contract { get; set; }
        public string Sponsor { get; set; }
        public string Assignment { get; set; }
        public string Dependents { get; set; }

        public DateTime? DateOfJoin { get; set; }
        public string ParentName { get; set; }
        public string PersonStatus { get; set; }
    }
}
