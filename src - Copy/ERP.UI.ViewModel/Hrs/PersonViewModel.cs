using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PersonViewModel : DatedViewModelBase
    {
        public long PersonId { get; set; }
        //[Required]
        [Display(Name = "Person Type")]
        public string PersonTypeCode { get; set; }

        [Display(Name = "Person Type")]
        public string PersonTypeName { get; set; }

        [Display(Name = "Person No")]
        public string PersonNo { get; set; }
        [Required]
        public PersonTitleEnum? Title { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

       // [Required]
        [Display(Name = "Mobile Number")]
        public string Mobile { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

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
                    , LastName, "-", PersonNo) : _FullName;
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
              PersonNo.IsNullOrEmptyOrWhiteSpace() ? "" : "-", PersonNo) : _DisplayNameWithPersonNo;
            }
        }







        [Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime DateOfBirth { get; set; }



        [Display(Name = "Gender")]
        [Required]
        public GenderEnum? Gender { get; set; }

        [Required]
        [Display(Name = "Marital Status")]
        public MaritalStatusEnum? MaritalStatus { get; set; }



        [Required]
        [Display(Name = "Nationality")]
        public long? NationalityId { get; set; }

        [Display(Name = "Nationality")]
        public string NationalityName { get; set; }

        [Required]
        [Display(Name = "Religion")]
        public ReligionEnum? Religion { get; set; }


        [Display(Name = "Employee Status")]
        public int? EmployeeStatusId { get; set; }

        [Display(Name = "Employee Status")]
        public string EmployeeStatusName { get; set; }


        [Display(Name = "Work Location")]
        public int? WorkLocationId { get; set; }

        [Display(Name = "Work Location")]
        public string WorkLocationName { get; set; }

        [Display(Name = "Personal Email")]
        [DataType(DataType.EmailAddress)]
        public string PersonalEmail { get; set; }

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

        public double? AnnualLeaveEntitlement { get; set; }


        [Display(Name = "Biometric Id")]
        public string BiometricId { get; set; }

        public long? PersonPositionId { get; set; }




        //[Required]
        [Display(Name = "Unit Number")]
        public string PresentUnitNumber { get; set; }
        // [Required]
        [Display(Name = "Building Number")]
        public string PresentBuildingNumber { get; set; }
        //  [Required]
        [Display(Name = "Street Name")]
        public string PresentStreetName { get; set; }
        [Display(Name = "City/Town")]
        //  [Required]
        public string PresentCity { get; set; }
        [Display(Name = "Postal Code")]
        // [Required]
        public string PresentPostalCode { get; set; }
        //  [Required]
        [Display(Name = "Additional Number")]
        public string PresentAdditionalNumber { get; set; }
        [Display(Name = "Country")]
        //[Required]
        public long? PresentConutryId { get; set; }

        public string PresentConutryName { get; set; }
        // [Required]
        [Display(Name = "Neighborhood Name")]
        public string PresentNeighborhoodName { get; set; }

        //  [Required]
        [Display(Name = "Unit Number")]
        public string HomeUnitNumber { get; set; }
        [Display(Name = "Building Number")]
        //  [Required]
        public string HomeBuildingNumber { get; set; }

        [Display(Name = "Street Name")]
        //  [Required]
        public string HomeStreetName { get; set; }
        [Display(Name = "City/Town")]
        //  [Required]
        public string HomeCity { get; set; }
        [Display(Name = "Postal Code")]
       // [Required]
        public string HomePostalCode { get; set; }
        [Display(Name = "Additional Number")]
        public string HomeAdditionalNumber { get; set; }
        [Display(Name = "Country")]
        //  [Required]
        public long? HomeConutryId { get; set; }
        public string HomeConutryName { get; set; }
        [Display(Name = "Neighborhood Name")]
        //   [Required]
        public string HomeNeighborhoodName { get; set; }

        [Display(Name = "Name")]
        //  [Required]
        public string EmergencyContactName1 { get; set; }
        [Display(Name = "Mobile Number")]
        // [Required]
        public string EmergencyContactNo1 { get; set; }


        public RelativeTypeEnum? Relationship1 { get; set; }
        // [Required]
        [Display(Name = "Relationship")]
        public RelationshipTypeEnum? RelationshipType1 { get; set; }
        [Display(Name = "Other Relation")]
        public string OtherRelation1 { get; set; }

        [Display(Name = "Name")]
        // [Required]
        public string EmergencyContactName2 { get; set; }
        [Display(Name = "Mobile Number")]
        // [Required]
        public string EmergencyContactNo2 { get; set; }

        public RelativeTypeEnum? Relationship2 { get; set; }

        //[Required]
        [Display(Name = "Relationship")]
        public RelationshipTypeEnum? RelationshipType2 { get; set; }
        [Display(Name = "Other Relation")]
        public string OtherRelation2 { get; set; }

        [Display(Name = "Country")]
        // [Required]
        public long? ContactConutryId { get; set; }
        public string ContactConutryName { get; set; }
        [Display(Name = "Code")]
        public string ContactConutryCode { get; set; }
        public string ContactConutryDialCode { get; set; }
        // [Required]
        [Display(Name = "Country")]
        public long? EmergencyContactConutryId1 { get; set; }
        public string EmergencyContactConutryName1 { get; set; }
        [Display(Name = "Code")]
        public string EmergencyContactConutryCode1 { get; set; }
        public string EmergencyContactCountryDialCode1 { get; set; }
        // [Required]
        [Display(Name = "Country")]
        public long? EmergencyContactConutryId2 { get; set; }
        public string EmergencyContactConutryName2 { get; set; }

        [Display(Name = "Code")]
        public string EmergencyContactConutryCode2 { get; set; }
        public string EmergencyContactCountryDialCode2 { get; set; }
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
        [Display(Name = "Iqamah No/National ID")]
        public string SponsorshipNo { get; set; }
        [Display(Name = "Expiry Date")]
        public DateTime? IDExpiryDate { get; set; }
        [Display(Name = "Iqamah Job Title")]
        public string IqamahJobTitle { get; set; }
        public string HousingSalary { get; set; }
        public string BasicSalary { get; set; }
        public string TotalSalary { get; set; }
        public string TransportationSalary { get; set; }

        public long? PassportDocumentId { get; set; }
        [Display(Name = "Legal Entity")]
        public long? LegalEntityId { get; set; }

        public string LegalEntityName { get; set; }
        [Display(Name = "Personal Status")]
        public EmployeeStatusEnum? EmployeeStatus { get; set; }

        public long? SalaryInfoId { get; set; }

        public long? HierarchyId { get; set; }
        public string FatherName { get; set; }
        public string PreferedName { get; set; }
        public string IdCardName { get; set; }
        [Required]
        [Display(Name = "Date Of Join")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime PersonDateOfJoin { get; set; }
    }
}
