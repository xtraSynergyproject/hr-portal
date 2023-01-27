
using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class ActiveUserViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public long? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public PersonTitleEnum? Title { get; set; }
        public string DisplayName { get; set; }
        public string PhotoName { get; set; }
        public string EmployeeFullName { get; set; }
        public string UserRolesCSV { get; set; }
        

        public long? PersonId { get; set; }

        public string SponsorshipNo { get; set; }

        public long? LocationId { get; set; }
        public long? JobId { get; set; }
        public long? GradeId { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        public long? PositionId { get; set; }
        public long? SupervisorId { get; set; }

        public string LocationName { get; set; }
        public string JobName { get; set; }
        public string GradeName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string PositionName { get; set; }
        public string SupervisorName { get; set; }

        public GenderEnum? Gender { get; set; }
        public ReligionEnum? Religion { get; set; }
        public MaritalStatusEnum? MaritalStatus { get; set; }
        public long NationalityId { get; set; }
        public string NationalityName { get; set; }
        public string NationalityCode { get; set; }
        public string PersonNo { get; set; }
        public string EmployeeDisplayNameWithNo { get; set; }
        public string PermissionCSV { get; set; }
        public UserTypeEnum UserType { get; set; }
        public UserAuthTypeEnum UserAuthType { get; set; }
        public string EmployeeDisplayName { get; set; }

        public string Permissions { get; set; }
        public string CCHolderOrganizationMapping { get; set; }

        public string UserOrganizationMapping { get; set; }
        public string UserOrganizationMappingWithoutHierarchy { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string TimeZone { get; set; }
        public long? EmployeeRootPositionId { get; set; }
        public long? CompanyRootPositionId { get; set; }
        [Display(Name = "Employee Root Department Name")]
        public long? EmployeeRootOrganizationId { get; set; }
        [Display(Name = "Company Root Department Name")]
        public long? CompanyRootOrganizationId { get; set; }
        public long? PhotoId { get; set; }
        public string PhotoThumbNailUrl { get; set; }

        public string CompanyCode { get; set; }
        public string CultureInfo { get; set; }
        public string DateFormat { get; set; }
        public string DateTimeFormat { get; set; }

        public bool EnableEmailSummary { get; set; }
        public bool EnableRegularEmail { get; set; }

        public UserLoginTypeEnum UserLoginType { get; set; }
        public string MobileNo { get; set; }


        public DateTime? ContractEffectiveStartDate { get; set; }
        public DateTime? ContractEffectiveEndDate { get; set; }


        public DateTime? AssignmentEffectiveStartDate { get; set; }
        public DateTime? AssignmentEffectiveEndDate { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public bool? DisableWebAccess { get; set; }
        public bool? DisableDesktopAccess { get; set; }
        public bool? DisableMobileAccess { get; set; }
    }
}
