using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;


namespace ERP.UI.ViewModel
{
    public class PersonProfileViewModel : DatedViewModelBase
    {
        public long PersonId { get; set; }
        public long UserId { get; set; }

        public long PositionId { get; set; }

        [Display(Name = "Person No")]
        public string PersonNo { get; set; }
       
        [Display(Name = "Title")]
        public PersonTitleEnum? Title { get; set; }

        [Display(Name = "Gender")]
        public GenderEnum? Gender { get; set; }

        [Display(Name = "Nationality")]
        public string NationalityName { get; set; }

        [Display(Name = "Marital Status")]
        public MaritalStatusEnum? MaritalStatus { get; set; }

        [Display(Name = "Religion")]
        public ReligionEnum? Religion { get; set; }

        [Display(Name = "Birth Town")]
        public string BirthTown { get; set; }

        [Display(Name = "Birth Country")]
        public string BirthCountryName { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "E-Mail")]
        [DataType(DataType.EmailAddress)]
        public string PersonalEmail { get; set; }

        [Display(Name = "Employee Name")]
        public string PersonFullName { get; set; }

        [Display(Name = "Location")]
        public string LocationName { get; set; }

        [Display(Name = "Grade")]
        public string GradeName { get; set; }

        [Display(Name = "Organization")]
        public string  OrganizationName { get; set; }

        [Display(Name = "Job")]
        public string JobName { get; set; }

        [Display(Name = "Position")]
        public string PositionName { get; set; }

        [Display(Name = "Tenture Period")]
        public string AssignmentTypeName { get; set; }

        [Display(Name = "Date Of Join")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfJoin { get; set; }

        [Display(Name = "Person Type")]
        public string PersonTypeName { get; set; }

        [Display(Name = "Photo")]
        public string PhotoName { get; set; }
        public string base64Img { get; set; }

        [Display(Name = "Assignment Status")]
        public string AssignmentStatusName { get; set; }

        public string Page { get; set; }

        public long DependentId { get; set; }

        [Display(Name = "Mobile Number")]
        public string Mobile { get; set; }

        [Required]
        [Display(Name = "Unit Number")]
        public string PresentUnitNumber { get; set; }
        [Required]
        [Display(Name = "Building Number")]
        public string PresentBuildingNumber { get; set; }
        [Required]
        [Display(Name = "Street Name")]
        public string PresentStreetName { get; set; }
        [Display(Name = "City/Town")]
        [Required]
        public string PresentCity { get; set; }
        [Display(Name = "Postal Code")]
        [Required]
        public string PresentPostalCode { get; set; }
        [Required]
        [Display(Name = "Additional Number")]
        public string PresentAdditionalNumber { get; set; }
        [Display(Name = "Country")]
        [Required]
        public long? PresentConutryId { get; set; }

        [Display(Name = "Present Country Name")]
        public string PresentConutryName { get; set; }
        [Required]
        [Display(Name = "Unit Number")]
        public string HomeUnitNumber { get; set; }
        [Display(Name = "Building Number")]
        [Required]
        public string HomeBuildingNumber { get; set; }

        [Display(Name = "Street Name")]
        [Required]
        public string HomeStreetName { get; set; }
        [Display(Name = "City/Town")]
        [Required]
        public string HomeCity { get; set; }
        [Display(Name = "Postal Code")]
        [Required]
        public string HomePostalCode { get; set; }
        [Display(Name = "Additional Number")]
        public string HomeAdditionalNumber { get; set; }
        [Display(Name = "Country")]
        [Required]
        public long? HomeConutryId { get; set; }
        [Display(Name = "Home Country Name")]
        public string HomeConutryName { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string EmergencyContactName1 { get; set; }
        [Display(Name = "Mobile Number")]
        [Required]
        public string EmergencyContactNo1 { get; set; }
        [Display(Name = "Relationship")]
        [Required]
        public RelationshipTypeEnum? Relationship1 { get; set; }
        [Display(Name = "Other Relation")]
        public string OtherRelation1 { get; set; }
        [Display(Name = "Name")]
        [Required]
        public string EmergencyContactName2 { get; set; }
        [Display(Name = "Mobile Number")]
        [Required]
        public string EmergencyContactNo2 { get; set; }
        [Display(Name = "Relationship")]
        [Required]
        public RelationshipTypeEnum? Relationship2 { get; set; }
        [Display(Name = "Other Relation")]
        public string OtherRelation2 { get; set; }
        [Display(Name = "Country")]
        [Required]
        public long? ContactConutryId { get; set; }

        [Display(Name = "Country")]
        public string ContactConutryName { get; set; }
        [Display(Name = "Code")]
        public string ContactConutryCode { get; set; }
        public string ContactConutryDialCode { get; set; }
        [Required]
        [Display(Name = "Country")]
        public long? EmergencyContactConutryId1 { get; set; }

        [Display(Name = "Emergency Contact Country Name")]
        public string EmergencyContactConutryName1 { get; set; }
        [Display(Name = "Code")]
        public string EmergencyContactConutryCode1 { get; set; }
        public string EmergencyContactCountryDialCode1 { get; set; }
        [Required]
        [Display(Name = "Country")]
        public long? EmergencyContactConutryId2 { get; set; }

        [Display(Name = "Emergency Contact Country Name")]
        public string EmergencyContactConutryName2 { get; set; }

        [Display(Name = "Code")]
        public string EmergencyContactConutryCode2 { get; set; }
        public string EmergencyContactCountryDialCode2 { get; set; }
        //[Display(Name = "Passport Number")]
        //public long? PassportNumber { get; set; }
        //[Display(Name = "Date Of Issue")]
        //public DateTime? DateOfIssue { get; set; }
        //[Display(Name = "Expiry Date")]
        //public DateTime? PassportExpiryDate { get; set; }
        //[Display(Name = "Place Of Issue")]
        //public string PlaceOfIssue { get; set; }

        //[Display(Name = "ID Type")]
        //public IDTypeEnum? IDType { get; set; }
        //[Required]
        //[Display(Name = "Iqamah No/National ID/UAE ID")]
        //public string SponsorshipNo { get; set; }
        //[Display(Name = "Expiry Date")]
        //public DateTime? IDExpiryDate { get; set; }
        //[Display(Name = "Iqamah Job Title")]
        //public string IqamahJobTitle { get; set; }
        //public string HousingSalary { get; set; }
        //public string BasicSalary { get; set; }
        //public string TotalSalary { get; set; }
        //public string TransportationSalary { get; set; }

        //public long? PassportDocumentId { get; set; }
    }
}
