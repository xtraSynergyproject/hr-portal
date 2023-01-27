using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ERP.UI.ViewModel
{
    public class EmployeeFilterViewModel
    {
        //Employee Items
        [Display(Name = "Status")]
        public StatusEnum Status { get; set; }
        public string SelectedModules { get; set; }
        //public DataTable DataList { get; set; }

        public long PersonId { get; set; }
        //
        [Display(Name = "Person Type")]
        public string PersonTypeCode { get; set; }

        [Display(Name = "Person Type")]
        public string PersonTypeName { get; set; }

        [Display(Name = "Person No")]
        public string PersonNo { get; set; }
        public bool IsPersonNoColumnRequired { get; set; }

        public PersonTitleEnum? Title { get; set; }
        public string TitleName { get; set; }
        public bool IsTitleColumnRequired { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        public bool IsFirstNameColumnRequired { get; set; }
        //  
        [Display(Name = "Mobile Number")]
        public string Mobile { get; set; }
        public bool IsMobileColumnRequired { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        public bool IsMiddleNameColumnRequired { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public bool IsLastNameColumnRequired { get; set; }
        [Display(Name = "Full Name (In Arabic)")]
        public string FullNameLocal { get; set; }
        private string _FullName { get; set; }

        [Display(Name = "Person Full Name")]
        public string FullName
        {
            set { _FullName = value; }
            get
            {
                return _FullName.IsNullOrEmpty() ?
                 string.Concat(FirstName, " ", MiddleName, MiddleName.IsNullOrEmptyOrWhiteSpace() ? "" : " "
                    , LastName, "-", SponsorshipNo) : _FullName;
            }
        }



        [Display(Name = "Employee Name")]
        public string NameAndNo { get; set; }

        private string _DisplayName;
        public string DisplayName
        {

            set { _DisplayName = value; }
            get { return _DisplayName.IsNullOrEmpty() ? string.Concat(FirstName, " ", LastName) : _DisplayName; }
        }
        private string _FullNameWithPersonNo;
        public string FullNameWithPersonNo
        {
            set { _FullNameWithPersonNo = value; }
            get { return _FullNameWithPersonNo.IsNullOrEmpty() ? string.Concat(FullName, PersonNo.IsNullOrEmptyOrWhiteSpace() ? "" : "-", PersonNo) : _FullNameWithPersonNo; }
        }
        private string _DisplayNameWithPersonNo;
        public string DisplayNameWithPersonNo
        {
            set { _DisplayNameWithPersonNo = value; }
            get
            {
                return _DisplayNameWithPersonNo.IsNullOrEmpty() ? string.Concat(DisplayName,
              PersonNo.IsNullOrEmptyOrWhiteSpace() ? "" : "-", SponsorshipNo) : _DisplayNameWithPersonNo;
            }
        }








        [Display(Name = "Date Of Birth Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfBirthFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfBirthTo { get; set; }
        public bool IsDateOfBirthColumnRequired { get; set; }


        [Display(Name = "Gender")]

        public GenderEnum? Gender { get; set; }
        public string GenderName { get; set; }
        public bool IsGenderColumnRequired { get; set; }

        [Display(Name = "Marital Status")]
        public MaritalStatusEnum? MaritalStatus { get; set; }
        public string MaritalStatusName { get; set; }
        public bool IsMaritalStatusColumnRequired { get; set; }



        [Display(Name = "Nationality")]
        public long? NationalityId { get; set; }
        public bool IsNationalityColumnRequired { get; set; }
        [Display(Name = "Nationality")]
        public string NationalityName { get; set; }


        [Display(Name = "Religion")]
        public ReligionEnum? Religion { get; set; }
        public string ReligionName { get; set; }
        public bool IsReligionColumnRequired { get; set; }

        [Display(Name = "Employee Status")]
        public int? EmployeeStatusId { get; set; }
        public bool IsStatusColumnRequired { get; set; }
        [Display(Name = "Employee Status")]
        public PersonStatusEnum? EmployeeStatusName { get; set; }
        public string StatusName { get; set; }

        [Display(Name = "Work Location")]
        public int? WorkLocationId { get; set; }

        [Display(Name = "Work Location")]
        public string WorkLocationName { get; set; }

        [Display(Name = "Personal Email")]
        [DataType(DataType.EmailAddress)]
        public string PersonalEmail { get; set; }
        public bool IsPersonalEmailColumnRequired { get; set; }
        [Display(Name = "Birth Town")]
        public string BirthTown { get; set; }

        [Display(Name = "Birth Country")]
        public long? BirthCountryId { get; set; }

        [Display(Name = "Birth Country")]
        public string BirthCountryName { get; set; }

        [Display(Name = "Pension No")]
        public string PensionNo { get; set; }

        [Display(Name = "National Identity Card No")]
        public string NationalIdentityCardNo { get; set; }
        [Display(Name = "Health Card No")]
        public string HealthCardNo { get; set; }



        [Display(Name = "Photo")]
        public long? PhotoId { get; set; }
        public FileViewModel SelectedFile { get; set; }

        public long? ContractId { get; set; }
        public long? AssignmentId { get; set; }
        public long? UserId { get; set; }




        [Display(Name = "Biometric Id")]
        public string BiometricId { get; set; }
        public bool IsBiometricIdColumnRequired { get; set; }


        //  
        [Display(Name = "Building Number")]
        public string PresentBuildingNumber { get; set; }
        public bool IsPresentBuildingNumberColumnRequired { get; set; }

        // 
        [Display(Name = "Street Name")]
        public string PresentStreetName { get; set; }
        public bool IsPresentStreetNameColumnRequired { get; set; }
        [Display(Name = "City/Town")]
        //
        public string PresentCity { get; set; }
        public bool IsPresentCityColumnRequired { get; set; }
        [Display(Name = "Postal Code")]
        //
        public string PresentPostalCode { get; set; }
        public bool IsPresentPostalCodeColumnRequired { get; set; }
        [Display(Name = "Country")]
        //
        public long? PresentCountryId { get; set; }
        public string PresentCountryName { get; set; }
        public bool IsPresentCountryColumnRequired { get; set; }
        [Display(Name = "Building Number")]
        //
        public string HomeBuildingNumber { get; set; }
        [Display(Name = "Street Name")]
        //
        public string HomeStreetName { get; set; }
        [Display(Name = "City/Town")]
        //
        public string HomeCity { get; set; }
        [Display(Name = "Postal Code")]
        //
        public string HomePostalCode { get; set; }
        [Display(Name = "Conutry")]
        // 
        public long? HomeConutryId { get; set; }

        [Display(Name = "Name1")]
        //
        public string EmergencyContactName1 { get; set; }
        public bool IsEmergencyContactName1ColumnRequired { get; set; }

        [Display(Name = "Mobile Number1")]
        //
        public string EmergencyContactNo1 { get; set; }
        public bool IsEmergencyContactNo1ColumnRequired { get; set; }

        [Display(Name = "Relationship1")]
        //
        public RelativeTypeEnum? Relationship1 { get; set; }
        public string Relationship1Name { get; set; }
        public bool IsRelationship1ColumnRequired { get; set; }

        [Display(Name = "Name2")]
        //
        public string EmergencyContactName2 { get; set; }
        public bool IsEmergencyContactName2ColumnRequired { get; set; }
        [Display(Name = "Mobile Number2")]
        //
        public string EmergencyContactNo2 { get; set; }
        public bool IsEmergencyContactNo2ColumnRequired { get; set; }
        [Display(Name = "Relationship2")]
        //
        public RelativeTypeEnum? Relationship2 { get; set; }
        public string Relationship2Name { get; set; }
        public bool IsRelationship2ColumnRequired { get; set; }

        [Display(Name = "Country")]
        //
        public long? ContactCountryId { get; set; }
        public string ContactCountryName { get; set; }
        public bool IsContactCountryIdColumnRequired { get; set; }
        [Display(Name = "Code")]
        public string ContactCountryCode { get; set; }
        public bool IsContactCountryCodeColumnRequired { get; set; }
        //
        [Display(Name = "Country1")]
        public long? EmergencyContactCountryId1 { get; set; }
        public string EmergencyContactCountry1Name { get; set; }
        public bool IsEmergencyContactCountryId1ColumnRequired { get; set; }
        [Display(Name = "Code1")]
        public string EmergencyContactCountryCode1 { get; set; }
        public bool IsEmergencyContactCountryCode1ColumnRequired { get; set; }
        //
        [Display(Name = "Country2")]
        public long? EmergencyContactCountryId2 { get; set; }
        public string EmergencyContactCountry2Name { get; set; }
        public bool IsEmergencyContactCountryId2ColumnRequired { get; set; }
        [Display(Name = "Code2")]
        public string EmergencyContactCountryCode2 { get; set; }
        public bool IsEmergencyContactCountryCode2ColumnRequired { get; set; }

        [Display(Name = "Passport Number")]
        public long? PassportNumber { get; set; }
        [Display(Name = "Date Of Issue")]
        public DateTime? DateOfIssue { get; set; }
        [Display(Name = "Expiry Date")]
        public DateTime? PassportExpiryDate { get; set; }
        [Display(Name = "Place Of Issue")]
        public string PlaceOfIssue { get; set; }

        [Display(Name = "ID Type")]
        public IDTypeEnum? IDType { get; set; }

        [Display(Name = "Iqamah No")]
        public string SponsorshipNo { get; set; }
        public bool IsSponsorshipNoColumnRequired { get; set; }
        [Display(Name = "Expiry Date")]
        public DateTime? IDExpiryDate { get; set; }
        [Display(Name = "Iqamah Job Title")]
        public string IqamahJobTitle { get; set; }
        public string HousingSalary { get; set; }
        public string BasicSalary { get; set; }
        public string TotalSalary { get; set; }
        public string TransportationSalary { get; set; }

        public long? PassportDocumentId { get; set; }

        [Display(Name = "Job Title")]
        public string PersonJob { get; set; }

        [Display(Name = "Organization")]
        public string PersonOrganization { get; set; }

        [Display(Name = "Restaurant")]
        public string PersonRestaurant { get; set; }

        public System.Data.DataTable ReportDataTable { get; set; }

        //Dependent Items
        public long DependentId { get; set; }

        [Display(Name = "Relationship Type")]
        public RelationshipTypeEnum? RelationshipType { get; set; }
        public string RelationshipTypeName { get; set; }
        public bool IsRelationshipTypeColumnRequired { get; set; }

        [Display(Name = "First Name")]
        public string DependentFirstName { get; set; }
        public bool IsDependentFirstNameColumnRequired { get; set; }
        [Display(Name = "Middle Name")]
        public string DependentMiddleName { get; set; }
        public bool IsDependentMiddleNameColumnRequired { get; set; }
        [Display(Name = "Last Name")]
        public string DependentLastName { get; set; }
        public bool IsDependentLastNameColumnRequired { get; set; }

        [Display(Name = "Gender")]
        public GenderEnum? DependentGender { get; set; }
        public string DependentGenderName { get; set; }
        public bool IsDependentGenderColumnRequired { get; set; }


        [Display(Name = "Iqamah ID/Nationality ID/UAE ID")]
        public string DependentSponsorshipNo { get; set; }
        public bool IsDependentSponsorshipNoColumnRequired { get; set; }


        [Display(Name = "Date Of Birth Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DependentDateOfBirthFrom { get; set; }
        public bool IsDependentDateOfBirthColumnRequired { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DependentDateOfBirthTo { get; set; }


        [Display(Name = "Place Of Birth")]
        public string DependentBirthTown { get; set; }
        public bool IsDependentBirthTownColumnRequired { get; set; }


        [Display(Name = "Birth Country")]
        public long? DependentBirthCountryId { get; set; }
        public string DependentBirthCountryName { get; set; }
        public bool IsDependentBirthCountryColumnRequired { get; set; }

        [Display(Name = "Sponsor Id")]
        public string DependentSponsorId { get; set; }
        public bool IsDependentSponsorIdColumnRequired { get; set; }

        //Contract Items
        [Display(Name = "Contract Type")]
        public ContractTypeEnum? ContractType { get; set; }
        public string ContractTypeName { get; set; }
        public bool IsContractTypeColumnRequired { get; set; }


        [Display(Name = "Annual Leave Entitlement")]
        public double? AnnualLeaveEntitlement { get; set; }
        public bool IsAnnualLeaveEntitlementColumnRequired { get; set; }



        [Display(Name = "Sponsor")]
        public long? SponsorId { get; set; }
        public string Sponsor { get; set; }
        public bool IsSponsorColumnRequired { get; set; }


        [Display(Name = "Contract Renewable")]
        public BoolStatus? ContractRenewable { get; set; }
        public string ContractRenewableName { get; set; }
        public bool IsContractRenewableColumnRequired { get; set; }

        [Display(Name = "Effective Start Date Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ContractEffectiveStartDateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ContractEffectiveStartDateTo { get; set; }
        public bool IsContractEffectiveStartDateColumnRequired { get; set; }

        [Display(Name = "Effective End Date Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ContractEffectiveEndDateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ContractEffectiveEndDateTo { get; set; }
        public bool IsContractEffectiveEndDateColumnRequired { get; set; }

        //Assignment Items
        [Display(Name = "Organization")]
        public long? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public bool IsOrganizationColumnRequired { get; set; }

        [Display(Name = "Job")]
        public long? JobId { get; set; }
        public string JobName { get; set; }
        public bool IsJobColumnRequired { get; set; }

        [Display(Name = "Position")]
        public long? PositionId { get; set; }
        public string PositionName { get; set; }
        public bool IsPositionColumnRequired { get; set; }

        [Display(Name = "Location")]
        public long? LocationId { get; set; }
        public string LocationName { get; set; }
        public bool IsLocationColumnRequired { get; set; }

        [Display(Name = "Grade")]
        public long? GradeId { get; set; }
        public string GradeName { get; set; }
        public bool IsGradeColumnRequired { get; set; }

        [Display(Name = "Assignment Type")]
        public string AssignmentTypeCode { get; set; }
        public string AssignmentTypeName { get; set; }
        public bool IsAssignmentTypeColumnRequired { get; set; }

        [Display(Name = "Probation Period")]
        public ProbationPeriodEnum? ProbationPeriod { get; set; }
        public string ProbationPeriodName { get; set; }
        public bool IsProbationPeriodColumnRequired { get; set; }

        [Display(Name = "Date Of Join")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfJoinFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfJoinTo { get; set; }
        public bool IsDateOfJoinColumnRequired { get; set; }

        [Display(Name = "Assignment Status")]
        public long? AssignmentStatusId { get; set; }
        public string AssignmentStatusName { get; set; }
        public bool IsAssignmentStatusColumnRequired { get; set; }

        [Display(Name = "Effective Start Date Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? AssignmentEffectiveStartDateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? AssignmentEffectiveStartDateTo { get; set; }
        public bool IsAssignmentEffectiveStartDateColumnRequired { get; set; }

        [Display(Name = "Effective End Date Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? AssignmentEffectiveEndDateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? AssignmentEffectiveEndDateTo { get; set; }
        public bool IsAssignmentEffectiveEndDateColumnRequired { get; set; }

        //User Items
        [Display(Name = "Legal Entity")]
        public long? LegalEntityId { get; set; }
        public string LegalEntityName { get; set; }
        public bool IsLegalEntityColumnRequired { get; set; }

        [Display(Name = "User Login Type")]
        public UserLoginTypeEnum? UserLoginType { get; set; }
        public string UserLoginTypeName { get; set; }
        public bool IsUserLoginTypeColumnRequired { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public bool IsUserNameColumnRequired { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
        public bool IsEmailColumnRequired { get; set; }

        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }
        public bool IsMobileNoColumnRequired { get; set; }

        [Display(Name = "Iqamah No")]
        public string IqamahNo { get; set; }
        public bool IsIqamahNoColumnRequired { get; set; }

        [Display(Name = "User Roles")]
        public string UserRoleVal { get; set; }
        public string UserRoleName { get; set; }
        public bool IsUserRoleColumnRequired { get; set; }

        [Display(Name = "User Permissions")]
        public string UserPermission { get; set; }
        public string UserPermissionName { get; set; }
        public bool IsUserPermissionColumnRequired { get; set; }

        [Display(Name = "Exclude User Permissions")]
        public string ExcludeUserPermission { get; set; }
        public string ExcludeUserPermissionName { get; set; }
        public bool IsExcludeUserPermissionColumnRequired { get; set; }

        [Display(Name = "Is Admin")]
        public BoolStatus? IsAdmin { get; set; }
        public string IsAdminName { get; set; }
        public bool IsAdminColumnRequired { get; set; }

        [Display(Name = "Status")]
        public UserStatusEnum? UserStatus { get; set; }
        public string UserStatusName { get; set; }
        public bool IsUserStatusColumnRequired { get; set; }

        //Leave Approval Hierarchy Items
        [Display(Name = "Hierarchy Admin")]
        public long? AdminUserId { get; set; }
        public string AdminUserName { get; set; }
        public bool IsAdminUserNameColumnRequired { get; set; }

        [Display(Name = "Direct Manager Option1")]
        public long? Level1ApproverOption1UserId { get; set; }
        public string Level1ApproverOption1UserName { get; set; }
        public bool IsLevel1ApproverOption1ColumnRequired { get; set; }

        [Display(Name = "Direct Manager Option2")]
        public long? Level1ApproverOption2UserId { get; set; }
        public string Level1ApproverOption2UserName { get; set; }
        public bool IsLevel1ApproverOption2ColumnRequired { get; set; }

        [Display(Name = "Direct Manager Option3")]
        public long? Level1ApproverOption3UserId { get; set; }
        public string Level1ApproverOption3UserName { get; set; }
        public bool IsLevel1ApproverOption3ColumnRequired { get; set; }

        [Display(Name = "Department Manager Option1")]
        public long? Level2ApproverOption1UserId { get; set; }
        public string Level2ApproverOption1UserName { get; set; }
        public bool IsLevel2ApproverOption1ColumnRequired { get; set; }

        [Display(Name = "Department Manager Option2")]
        public long? Level2ApproverOption2UserId { get; set; }
        public string Level2ApproverOption2UserName { get; set; }
        public bool IsLevel2ApproverOption2ColumnRequired { get; set; }

        [Display(Name = "Department Manager Option3")]
        public long? Level2ApproverOption3UserId { get; set; }
        public string Level2ApproverOption3UserName { get; set; }
        public bool IsLevel2ApproverOption3ColumnRequired { get; set; }

        [Display(Name = "HR Officer Option1")]
        public long? Level3ApproverOption1UserId { get; set; }
        public string Level3ApproverOption1UserName { get; set; }
        public bool IsLevel3ApproverOption1ColumnRequired { get; set; }

        [Display(Name = "HR Officer Option2")]
        public long? Level3ApproverOption2UserId { get; set; }
        public string Level3ApproverOption2UserName { get; set; }
        public bool IsLevel3ApproverOption2ColumnRequired { get; set; }

        [Display(Name = "HR Officer Option3")]
        public long? Level3ApproverOption3UserId { get; set; }
        public string Level3ApproverOption3UserName { get; set; }
        public bool IsLevel3ApproverOption3ColumnRequired { get; set; }

        [Display(Name = "HR Manager Option1")]
        public long? Level4ApproverOption1UserId { get; set; }
        public string Level4ApproverOption1UserName { get; set; }
        public bool IsLevel4ApproverOption1ColumnRequired { get; set; }

        [Display(Name = "HR Manager Option2")]
        public long? Level4ApproverOption2UserId { get; set; }
        public string Level4ApproverOption2UserName { get; set; }
        public bool IsLevel4ApproverOption2ColumnRequired { get; set; }

        [Display(Name = "HR Manager Option3")]
        public long? Level4ApproverOption3UserId { get; set; }
        public string Level4ApproverOption3UserName { get; set; }
        public bool IsLevel4ApproverOption3ColumnRequired { get; set; }

        [Display(Name = "Business Unit Director Option1")]
        public long? Level5ApproverOption1UserId { get; set; }
        public string Level5ApproverOption1UserName { get; set; }
        public bool IsLevel5ApproverOption1ColumnRequired { get; set; }

        [Display(Name = "Business Unit Director Option2")]
        public long? Level5ApproverOption2UserId { get; set; }
        public string Level5ApproverOption2UserName { get; set; }
        public bool IsLevel5ApproverOption2ColumnRequired { get; set; }

        [Display(Name = "Business Unit Director Option3")]
        public long? Level5ApproverOption3UserId { get; set; }
        public string Level5ApproverOption3UserName { get; set; }
        public bool IsLevel5ApproverOption3ColumnRequired { get; set; }

        //Position Hierarchy Details
        [Display(Name = "Position")]
        public long? HierarchyPositionId { get; set; }
        public string HierarchyPositionName { get; set; }
        public bool IsHierarchyPositionColumnRequired { get; set; }

        [Display(Name = "Parent Position")]
        public long? ParentPositionId { get; set; }
        public string ParentPositionName { get; set; }
        public bool IsParentPositionColumnRequired { get; set; }

        [Display(Name = "Status")]
        public PersonStatusEnum? PositionStatus { get; set; }
        public string PositionStatusName { get; set; }
        public bool IsPositionStatusColumnRequired { get; set; }

        //Organization Hierarchy Details
        [Display(Name = "Organization")]
        public long? HierarchyOrganizationId { get; set; }
        public string HierarchyOrganizationName { get; set; }
        public bool IsHierarchyOrganizationColumnRequired { get; set; }

        [Display(Name = "Parent Organization")]
        public long? ParentOrganizationId { get; set; }
        public string ParentOrganizationName { get; set; }
        public bool IsParentOrganizationColumnRequired { get; set; }

        [Display(Name = "Status")]
        public PersonStatusEnum? OrganizationStatus { get; set; }
        public string OrganizationStatusName { get; set; }
        public bool IsOrganizationStatusColumnRequired { get; set; }

        //Salary Info Details
        [Display(Name = "Payment Mode")]
        public PaymentModeEnum? PaymentMode { get; set; }
        public string PaymentModeName { get; set; }
        public bool IsPaymentModeColumnRequired { get; set; }

        [Display(Name = "Pay Group")]
        public long? PayGroupId { get; set; }
        public string PayGroupName { get; set; }
        public bool IsPayGroupColumnRequired { get; set; }

        [Display(Name = "Pay Calendar")]
        public long? PayCalendarId { get; set; }
        public string PayCalendarName { get; set; }
        public bool IsPayCalendarColumnRequired { get; set; }

        [Display(Name = "Take Attendance From TAA")]
        public BoolStatus? TakeAttendanceFromTAA { get; set; }
        public string TakeAttendanceFromTAAName { get; set; }
        public bool IsTakeAttendanceFromTAAColumnRequired { get; set; }

        [Display(Name = "Is Eligible For OT")]
        public BoolStatus? IsEligibleForOT { get; set; }
        public string IsEligibleForOTName { get; set; }
        public bool IsEligibleForOTColumnRequired { get; set; }

        [Display(Name = "OT Payment Type")]
        public OTPaymentTypeEnum? OTPaymentType { get; set; }
        public string OTPaymentTypeName { get; set; }
        public bool IsOTPaymentTypeColumnRequired { get; set; }

        [Display(Name = "Bank Branch")]
        public long? BankBranchId { get; set; }
        public string BankBranchName { get; set; }
        public bool IsBankBranchColumnRequired { get; set; }

        [Display(Name = "Bank Account No")]
        public string BankAccountNo { get; set; }
        public bool IsBankAccountNoColumnRequired { get; set; }

        [Display(Name = "Bank IBan No")]
        public string BankIBanNo { get; set; }
        public bool IsBankIBanNoColumnRequired { get; set; }

        [Display(Name = "Status")]
        public PersonStatusEnum? SalaryInfoStatus { get; set; }
        public string SalaryInfoStatusName { get; set; }
        public bool IsSalaryInfoStatusColumnRequired { get; set; }

        [Display(Name = "Salary Element")]
        public string SalaryElement1 { get; set; }
        public string SalaryElementName1 { get; set; }
        public bool IsSalaryElementColumnRequired1 { get; set; }

        [Display(Name = "Salary Element")]
        public string SalaryElement2 { get; set; }
        public string SalaryElementName2 { get; set; }
        public bool IsSalaryElementColumnRequired2 { get; set; }

        [Display(Name = "Salary Element")]
        public string SalaryElement3 { get; set; }
        public string SalaryElementName3 { get; set; }
        public bool IsSalaryElementColumnRequired3 { get; set; }

        [Display(Name = "Salary Element")]
        public string SalaryElement4 { get; set; }
        public string SalaryElementName4 { get; set; }
        public bool IsSalaryElementColumnRequired4 { get; set; }

        [Display(Name = "Salary Element")]
        public string SalaryElement5 { get; set; }
        public string SalaryElementName5 { get; set; }
        public bool IsSalaryElementColumnRequired5 { get; set; }

        [Display(Name = "Salary Element")]
        public string SalaryElement6 { get; set; }
        public string SalaryElementName6 { get; set; }
        public bool IsSalaryElementColumnRequired6 { get; set; }

        [Display(Name = "Salary Element")]
        public string SalaryElement7 { get; set; }
        public string SalaryElementName7 { get; set; }
        public bool IsSalaryElementColumnRequired7 { get; set; }

        [Display(Name = "Salary Element")]
        public string SalaryElement8 { get; set; }
        public string SalaryElementName8 { get; set; }
        public bool IsSalaryElementColumnRequired8 { get; set; }

        [Display(Name = "Salary Element")]
        public string SalaryElement9 { get; set; }
        public string SalaryElementName9 { get; set; }
        public bool IsSalaryElementColumnRequired9 { get; set; }

        [Display(Name = "Salary Element")]
        public string SalaryElement10 { get; set; }
        public string SalaryElementName10 { get; set; }
        public bool IsSalaryElementColumnRequired10 { get; set; }

        [Display(Name = "Amount Between")]
        public double? AmountFrom1 { get; set; }
        public double? AmountTo1 { get; set; }
        public bool IsAmountColumnRequired1 { get; set; }

        [Display(Name = "Amount Between")]
        public double? AmountFrom2 { get; set; }
        public double? AmountTo2 { get; set; }
        public bool IsAmountColumnRequired2 { get; set; }

        [Display(Name = "Amount Between")]
        public double? AmountFrom3 { get; set; }
        public double? AmountTo3 { get; set; }
        public bool IsAmountColumnRequired3 { get; set; }

        [Display(Name = "Amount Between")]
        public double? AmountFrom4 { get; set; }
        public double? AmountTo4 { get; set; }
        public bool IsAmountColumnRequired4 { get; set; }

        [Display(Name = "Amount Between")]
        public double? AmountFrom5 { get; set; }
        public double? AmountTo5 { get; set; }
        public bool IsAmountColumnRequired5 { get; set; }

        [Display(Name = "Amount Between")]
        public double? AmountFrom6 { get; set; }
        public double? AmountTo6 { get; set; }
        public bool IsAmountColumnRequired6 { get; set; }

        [Display(Name = "Amount Between")]
        public double? AmountFrom7 { get; set; }
        public double? AmountTo7 { get; set; }
        public bool IsAmountColumnRequired7 { get; set; }

        [Display(Name = "Amount Between")]
        public double? AmountFrom8 { get; set; }
        public double? AmountTo8 { get; set; }
        public bool IsAmountColumnRequired8 { get; set; }

        [Display(Name = "Amount Between")]
        public double? AmountFrom9 { get; set; }
        public double? AmountTo9 { get; set; }
        public bool IsAmountColumnRequired9 { get; set; }

        [Display(Name = "Amount Between")]
        public double? AmountFrom10 { get; set; }
        public double? AmountTo10 { get; set; }
        public bool IsAmountColumnRequired10 { get; set; }

        //Notifications Details
        [Display(Name = "From")]
        public long? FromUserId { get; set; }
        public string FromUserName { get; set; }
        public bool IsFromUserNameColumnRequired { get; set; }

        [Display(Name = "To")]
        public long? ToUserId { get; set; }
        public string ToUserName { get; set; }
        public bool IsToUserNameColumnRequired { get; set; }

        [Display(Name = "Notification Date Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? NotificationDateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? NotificationDateTo { get; set; }
        public bool IsNotificationDateColumnRequired { get; set; }

        [Display(Name = "Subject")]
        public string NotificationSubject { get; set; }
        public bool IsNotificationSubjectColumnRequired { get; set; }

        [Display(Name = "Description")]
        public string NotificationDescription { get; set; }
        public bool IsNotificationDescriptionColumnRequired { get; set; }


        [Display(Name = "Read Status")]
        public ReadStatusEnum? ReadStatus { get; set; }
        public string ReadStatusName { get; set; }
        public bool IsReadStatusColumnRequired { get; set; }

        //Notes Details
        [Display(Name = "Note Status")]
        public NoteStatusEnum? NoteStatus { get; set; }
        public string NoteStatusName { get; set; }
        public bool IsNoteStatusColumnRequired { get; set; }

        [Display(Name = "Tag Type")]
        public NoteReferenceTypeEnum? TagType { get; set; }
        public string TagTypeName { get; set; }
        public bool IsTagTypeColumnRequired { get; set; }

        [Display(Name = "Tag To")]
        public string TagTo { get; set; }
        public bool IsTagToColumnRequired { get; set; }

        [Display(Name = "Note Description")]
        public string NoteDescription { get; set; }
        public bool IsNoteDescriptionColumnRequired { get; set; }

        [Display(Name = "Start Date Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? NoteStartDateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? NoteStartDateTo { get; set; }
        public bool IsNoteStartDateColumnRequired { get; set; }

        [Display(Name = "Expiry Date Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? NoteExpiryDateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? NoteExpiryDateTo { get; set; }
        public bool IsNoteExpiryDateColumnRequired { get; set; }

        //Tasks Details
        [Display(Name = "Task Status")]
        public TaskSearchEnum? TaskStatus { get; set; }
        public string TaskStatusName { get; set; }
        public bool IsTaskStatusColumnRequired { get; set; }

        [Display(Name = "Owner Name")]
        public string TaskOwnerName { get; set; }
        public bool IsTaskOwnerColumnRequired { get; set; }

        [Display(Name = "Assigned To")]
        public string TaskAssigneeDisplayName { get; set; }
        public bool IsTaskAssignedToColumnRequired { get; set; }

        [Display(Name = "Due Date Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? TaskDueDateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? TaskDueDateTo { get; set; }
        public bool IsTaskDueDateColumnRequired { get; set; }

        [Display(Name = "Start Date Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? TaskStartDateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? TaskStartDateTo { get; set; }
        public bool IsTaskStartDateColumnRequired { get; set; }

        [Display(Name = "Completion Date Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? TaskCompletionDateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? TaskCompletionDateTo { get; set; }
        public bool IsTaskCompletionDateColumnRequired { get; set; }

        [Display(Name = "Description")]
        public string TaskDescription { get; set; }
        public bool IsTaskDescriptionColumnRequired { get; set; }

        [Display(Name = "Assign To Type")]
        public AssignToTypeEnum? TaskAssignToType { get; set; }
        public string TaskAssignToTypeName { get; set; }
        public bool IsTaskAssignToTypeColumnRequired { get; set; }

        [Display(Name = "SLA")]
        public string TaskSLA { get; set; }
        public bool IsTaskSLAColumnRequired { get; set; }

        [Display(Name = "Task No")]
        public string TaskNo { get; set; }
        public bool IsTaskNoColumnRequired { get; set; }

        [Display(Name = "Owner Email")]
        public string TaskOwnerEmail { get; set; }
        public bool IsTaskOwnerEmailColumnRequired { get; set; }

        [Display(Name = "Owner Iqamah No")]
        public string TaskOwnerIqamahNo { get; set; }
        public bool IsTaskOwnerIqamahNoColumnRequired { get; set; }

        [Display(Name = "Assignee Email")]
        public string TaskAssigneeEmail { get; set; }
        public bool IsTaskAssigneeEmailColumnRequired { get; set; }

        [Display(Name = "Assignee Iqamah No")]
        public string TaskAssigneeIqamahNo { get; set; }
        public bool IsTaskAssigneeIqamahNoColumnRequired { get; set; }

        //Services Details
        [Display(Name = "Template Name")]
        public long? TemplateId { get; set; }
        public string TemplateName { get; set; }
        public bool IsTemplateNameColumnRequired { get; set; }

        [Display(Name = "Template Category")]
        public long? TemplateCategoryId { get; set; }
        public string TemplateCategoryName { get; set; }
        public bool IsTemplateCategoryNameColumnRequired { get; set; }

        [Display(Name = "Service Module")]
        public ModuleEnum? ServiceModule { get; set; }
        public string ServiceModuleName { get; set; }
        public bool IsServiceModuleColumnRequired { get; set; }

        [Display(Name = "Service Filter")]
        public ServiceSearchHomeEnum? ServiceFilter { get; set; }
        public string ServiceFilterName { get; set; }
        public bool IsServiceFilterColumnRequired { get; set; }

        [Display(Name = "SLA")]
        public string ServiceSLA { get; set; }
        public bool IsServiceSLAColumnRequired { get; set; }

        [Display(Name = "Service No")]
        public string ServiceNo { get; set; }
        public bool IsServiceNoColumnRequired { get; set; }

        [Display(Name = "Description")]
        public string ServiceDescription { get; set; }
        public bool IsServiceDescriptionColumnRequired { get; set; }

        [Display(Name = "Owner Name")]
        public string ServiceOwnerName { get; set; }
        public bool IsServiceOwnerColumnRequired { get; set; }

        [Display(Name = "Owner Email")]
        public string ServiceOwnerEmail { get; set; }
        public bool IsServiceOwnerEmailColumnRequired { get; set; }

        [Display(Name = "Owner Iqamah No")]
        public string ServiceOwnerIqamahNo { get; set; }
        public bool IsServiceOwnerIqamahNoColumnRequired { get; set; }

        [Display(Name = "Due Date Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ServiceDueDateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ServiceDueDateTo { get; set; }
        public bool IsServiceDueDateColumnRequired { get; set; }

        [Display(Name = "Start Date Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ServiceStartDateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ServiceStartDateTo { get; set; }
        public bool IsServiceStartDateColumnRequired { get; set; }

        [Display(Name = "Completion Date Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ServiceCompletionDateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ServiceCompletionDateTo { get; set; }
        public bool IsServiceCompletionDateColumnRequired { get; set; }

        //UDF Values
        public Dictionary<string, string> udfList { get; set; }


    }
}
