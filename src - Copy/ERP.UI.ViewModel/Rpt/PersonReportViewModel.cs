using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PersonReportViewModel : DatedViewModelBase
    {
        public long PersonId { get; set; }
        //[Required]
        [Display(Name = "Person Type")]
        public string PersonTypeCode { get; set; }
        public string YearsofAnniversary { get; set; }
        [Display(Name = "Person Type")]
        public string PersonTypeName { get; set; }
        public string LegalEntityCode { get; set; }
        public long LegalEntityId { get; set; }
        [Display(Name = "Person No")]
        public string PersonNo { get; set; }
        [Required]
        public PersonTitleEnum? Title { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        public int ReportSerialNo { get; set; }
        public string NoteDescription { get; set; }
        //  [Required]
        [Display(Name = "Mobile Number")]
        public string Mobile { get; set; }
        public string PersonLegalEntityName { get; set; }
        public string ReportEmployeeName { get; set; }
        public string PersonSection { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Sponsor")]
        public string Sponsor { get; set; }
        public string ExpiryDate { get; set; }
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







        [Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime DateOfBirth { get; set; }


        public DateTime? ContractEndDate { get; set; }

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

        public long? PositionId { get; set; }

        //  [Required]
        [Display(Name = "Building Number")]
        public string PresentBuildingNumber { get; set; }
        // [Required]
        [Display(Name = "Street Name")]
        public string PresentStreetName { get; set; }
        [Display(Name = "City/Town")]
        //[Required]
        public string PresentCity { get; set; }
        [Display(Name = "Postal Code")]
        //[Required]
        public string PresentPostalCode { get; set; }
        [Display(Name = "Conutry")]
        //[Required]
        public long? PresentConutryId { get; set; }

        [Display(Name = "Building Number")]
        //[Required]
        public string HomeBuildingNumber { get; set; }
        [Display(Name = "Street Name")]
        //[Required]
        public string HomeStreetName { get; set; }
        [Display(Name = "City/Town")]
        //[Required]
        public string HomeCity { get; set; }
        [Display(Name = "Postal Code")]
        //[Required]
        public string HomePostalCode { get; set; }
        [Display(Name = "Conutry")]
        // [Required]
        public long? HomeConutryId { get; set; }

        [Display(Name = "Name")]
        //[Required]
        public string EmergencyContactName1 { get; set; }
        [Display(Name = "Mobile Number")]
        //[Required]
        public string EmergencyContactNo1 { get; set; }
        [Display(Name = "Relationship")]
        //[Required]
        public RelativeTypeEnum Relationship1 { get; set; }

        [Display(Name = "Name")]
        //[Required]
        public string EmergencyContactName2 { get; set; }
        [Display(Name = "Mobile Number")]
        //[Required]
        public string EmergencyContactNo2 { get; set; }
        [Display(Name = "Relationship")]
        //[Required]
        public RelativeTypeEnum Relationship2 { get; set; }

        [Display(Name = "Country")]
        //[Required]
        public long? ContactConutryId { get; set; }
        [Display(Name = "Code")]
        public string ContactConutryCode { get; set; }
        //[Required]
        [Display(Name = "Country")]
        public long? EmergencyContactConutryId1 { get; set; }
        [Display(Name = "Code")]
        public string EmergencyContactConutryCode1 { get; set; }
        //[Required]
        [Display(Name = "Country")]
        public long? EmergencyContactConutryId2 { get; set; }
        [Display(Name = "Code")]
        public string EmergencyContactConutryCode2 { get; set; }

        [Display(Name = "Passport Number")]
        public long? PassportNumber { get; set; }
        [Display(Name = "Date Of Issue")]
        public DateTime? DateOfIssue { get; set; }
        [Display(Name = "Expiry Date")]
        public DateTime? PassportExpiryDate { get; set; }
        [Display(Name = "Place Of Issue")]
        public string PlaceOfIssue { get; set; }
        public DateTime? NationalIdExpiryDate { get; set; }
        public DateTime? VisaExpiryDate { get; set; }
        [Display(Name = "ID Type")]
        public IDTypeEnum? IDType { get; set; }
        [Required]
        [Display(Name = "Iqamah No/National ID/UAE ID")]
        public string SponsorshipNo { get; set; }
        [Display(Name = "Expiry Date")]
        public DateTime? IDExpiryDate { get; set; }
        [Display(Name = "Iqamah Job Title")]
        public string IqamahJobTitle { get; set; }
        public string HousingSalary { get; set; }
        public string BasicSalary { get; set; }
        public string TotalSalary { get; set; }
        public string TransportationSalary { get; set; }
        public string PersonAssignmentType { get; set; }
        public long? PassportDocumentId { get; set; }

        [Display(Name = "Job Title")]
        public string PersonJob { get; set; }

        [Display(Name = "Organization")]
        public string PersonOrganization { get; set; }

        [Display(Name = "Restaurant")]
        public string PersonRestaurant { get; set; }
        public string NewEmployeeDetails { get; set; }
        public string TerminatedEmployeeDetails { get; set; }
        public string EmployeeLeaveDetails { get; set; }
        public string EmployeeAbsenceDetails { get; set; }
        [Display(Name = "Rating Users")]
        public List<long?> RatingUsers { get; set; }

        public string PersonUserName { get; set; }
        public long ServiceId { get; set; }

        [Display(Name = "Unit Number")]
        public string PresentUnitNumber { get; set; }

        [Display(Name = "Leave Type")]
        public string LeaveType { get; set; }

        [Display(Name = "Leave Start Date")]
        public DateTime? LeaveStartDate { get; set; }

        [Display(Name = "Leave End Date")]
        public DateTime? LeaveEndDate { get; set; }

        [Display(Name = "Report Start Date")]
        public DateTime? ReportStartDate { get; set; }
       
        [Display(Name = "Report End Date")]
        public DateTime? ReportEndDate { get; set; }

        [Display(Name = "Additional Number")]
        public string PresentAdditionalNumber { get; set; }


        [Display(Name = "Neighborhood Name")]
        public string PresentNeighborhoodName { get; set; }

        [Display(Name = "Unit Number")]
        public string HomeUnitNumber { get; set; }

        [Display(Name = "Additional Number")]
        public string HomeAdditionalNumber { get; set; }

        [Display(Name = "Neighborhood Name")]
        public string HomeNeighborhoodName { get; set; }


        [Display(Name = "Relationship")]
        public RelationshipTypeEnum? RelationshipType1 { get; set; }
        [Display(Name = "Other Relation")]
        public string OtherRelation1 { get; set; }

        [Display(Name = "Relationship")]
        public RelationshipTypeEnum? RelationshipType2 { get; set; }
        [Display(Name = "Other Relation")]
        public string OtherRelation2 { get; set; }

        public EmployeeStatusEnum? EmployeeStatus { get; set; }
        public string Language { get; set; }

    }
}
