using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ERP.UI.ViewModel
{
    public class UserLoginViewModel : ViewModelBase
    {

        // public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string IqamahNo { get; set; }
        public string PersonNo { get; set; }
        public long? PersonId { get; set; }
        public long? LogoFileId { get; set; }

        public string DisplayName { get; set; }

        public string EmployeeDisplayName { get; set; }
        public string EmployeeDisplayNameWithNo { get; set; }
        public string EmployeeFullName { get; set; }
        public string UserRolesCSV { get; set; }
        public string UserRoleIds { get; set; }
        //public UserTypeEnum UserType { get; set; }
        public bool IsAdmin { get; set; }
        public UserAuthTypeEnum UserAuthType { get; set; }

        public long? PositionId { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        [Display(Name ="Department Name")]
        public string OrganizationName { get; set; }
        public long? JobId { get; set; }
        public string JobName { get; set; }
        public long? GradeId { get; set; }
        public long? LocationId { get; set; }
        public long? SupervisorId { get; set; }


        public long? PositionChartId { get; set; }
        public long? OrgChartId { get; set; }
        public GenderEnum? Gender { get; set; }
        public ReligionEnum? Religion { get; set; }
        public long? NationalityId { get; set; }
        // public long CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CultureInfo { get; set; }
        public string DateFormat { get; set; }
        public string DateTimeFormat { get; set; }



        //public ItemStatus Status { get; set; }

        public string Permissions { get; set; }
        //public string ModulePermissions { get; set; }
        //public string SubModulePermissions { get; set; }
        public string CCHolderOrganizationMapping { get; set; }

        public string UserOrganizationMapping { get; set; }
        //  public string BPOrganizationMapping { get; set; }

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
        // public string BPOrganizationMappingWithoutHierarchy { get; set; }

        public string LegalEntityCode { get; set; }
        public int LegalEntityCount { get; set; }
        public long? LegalEntityId { get; set; }
        public bool? PasswordChangeRequired { get; set; }
    }
}
