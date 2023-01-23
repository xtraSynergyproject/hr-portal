using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class PersonViewModel:ViewModelBase
    {
        public string PersonId { get; set; }
        //[Required]
        [Display(Name = "Person Type")]
        public string PersonTypeCode { get; set; }

        [Display(Name = "Person Type")]
        public string PersonTypeName { get; set; }

        [Display(Name = "Person No")]
        public string PersonNo { get; set; }
      
        public PersonTitleEnum? Title { get; set; }
       
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
        public string PersonFullName
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
            get { return _FullNameWithPersonNo.IsNullOrEmpty() ? string.Concat(PersonFullName, PersonNo.IsNullOrEmptyOrWhiteSpace() ? "" : "-", PersonNo) : _FullNameWithPersonNo; }
        }
        private string _DisplayNameWithPersonNo;
        public string NtsNoteId { get; set; }
        public string DisplayNameWithPersonNo
        {
            set { _DisplayNameWithPersonNo = value; }
            get
            {
                return _DisplayNameWithPersonNo.IsNullOrEmpty() ? string.Concat(DisplayName,
              PersonNo.IsNullOrEmptyOrWhiteSpace() ? "" : "-", PersonNo) : _DisplayNameWithPersonNo;
            }
        }







        //[Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
       // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime DateOfBirth { get; set; }

        public string AgeGroup
        {
            get
            {
                var d = "";
                if (0 <= Age && Age < 20)
                {
                    d = "0-20";
                }
                if (20 <= Age && Age < 30)
                {
                    d = "20-30";
                }
                if (30 <= Age && Age < 40)
                {
                    d = "30-40";
                }
                if (40 < Age && Age < 50)
                {
                    d = "40-50";
                }
                if (50 < Age && Age < 60)
                {
                    d = "50-60";
                }
                if (60 <= Age && Age < 70)
                {
                    d = "60-70";
                }
                if (70 <= Age && Age < 80)
                {
                    d = "70-80";
                }
                if (80 <= Age && Age < 90)
                {
                    d = "80-90";
                }
                if (90 <= Age && Age < 100)
                {
                    d = "90-100";
                }
                return d;
            }
            
        }
        public int Age
        {
            get 
            {
                if (DateOfBirth.IsNotNull()) 
                {
                    var d= (DateTime.Now.Year - DateOfBirth.Year);
                    
                    return d;
                }
                return 0;
            }
        }
        public double Salary { get; set; }
        public string SalaryRange 
        {
            get 
            {
                var d = "";
                if (0 < Salary && Salary < 10000)
                {
                    d = "0-10,000";
                }
                if (10000 <= Salary && Salary < 20000)
                {
                    d = "10,000-20,000";
                }
                if (20000 <= Salary && Salary < 30000)
                {
                    d = "20,000-30,000";
                }
                if (30000 <= Salary && Salary < 40000)
                {
                    d = "30,000-40,000";
                }
                if (40000 <= Salary && Salary < 50000)
                {
                    d = "40,000-50,000";
                }
                if (50000 < Salary && Salary < 60000)
                {
                    d = "50,000-60,000";
                }
                if (60000 <= Salary && Salary < 70000)
                {
                    d = "60,000-70,000";
                }
                if (70000 < Salary && Salary < 80000)
                {
                    d = "70,000-80,000";
                }
                if (80000 <= Salary && Salary < 90000)
                {
                    d = "80,000-90,000";
                }
                if (90000 <= Salary && Salary < 100000)
                {
                    d = "90,000-1,00,000";
                }
                return d;
            }
        }
       
       // [Required]
        public string GenderId { get; set; }
        [Display(Name = "Gender")]
        public string Gender{ get; set; }


        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }



       
        [Display(Name = "Nationality")]
        public string NationalityId { get; set; }

        [Display(Name = "Nationality")]
        public string NationalityName { get; set; }

      
        [Display(Name = "Religion")]
        public string Religion { get; set; }


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

        public string ContactPersonalEmail { get; set; }

        [Display(Name = "Birth Town")]
        public string BirthTown { get; set; }

        [Display(Name = "Birth Country")]
        public string BirthCountryId { get; set; }

        [Display(Name = "Birth Country")]
        public string BirthCountryName { get; set; }

        [Display(Name = "Pension No")]
        public string PensionNo { get; set; }

        [Display(Name = "National Identity Card No")]
        public string NationalIdentityCardNo { get; set; }
        [Display(Name = "Health Card No")]
        public string HealthCardNo { get; set; }



        [Display(Name = "Photo")]
        public string PhotoId { get; set; }
        public FileViewModel SelectedFile { get; set; }

        public string ContractId { get; set; }
        public string AssignmentId { get; set; }

        public string UserId { get; set; }
        public string UserRole { get; set; }

        public double? AnnualLeaveEntitlement { get; set; }


        [Display(Name = "Biometric Id")]
        public string BiometricId { get; set; }

        public string PersonPositionId { get; set; }




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
        public string PresentConutryId { get; set; }

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
        public string HomeConutryId { get; set; }
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


       // public RelativeTypeEnum? Relationship1 { get; set; }
        // [Required]
        //[Display(Name = "Relationship")]
       // public RelationshipTypeEnum? RelationshipType1 { get; set; }
        //[Display(Name = "Other Relation")]
        //public string OtherRelation1 { get; set; }

        [Display(Name = "Name")]
        // [Required]
        public string EmergencyContactName2 { get; set; }
        [Display(Name = "Mobile Number")]
        // [Required]
        public string EmergencyContactNo2 { get; set; }

       // public RelativeTypeEnum? Relationship2 { get; set; }

        //[Required]
       // [Display(Name = "Relationship")]
        //public RelationshipTypeEnum? RelationshipType2 { get; set; }
        [Display(Name = "Other Relation")]
        public string OtherRelation2 { get; set; }

        [Display(Name = "Country")]
        // [Required]
        public string ContactConutryId { get; set; }
        public string ContactConutryName { get; set; }
        [Display(Name = "Code")]
        public string ContactConutryCode { get; set; }
        public string ContactConutryDialCode { get; set; }
        // [Required]
        [Display(Name = "Country")]
        public string EmergencyContactConutryId1 { get; set; }
        public string EmergencyContactConutryName1 { get; set; }
        [Display(Name = "Code")]
        public string EmergencyContactConutryCode1 { get; set; }
        public string EmergencyContactCountryDialCode1 { get; set; }
        // [Required]
        [Display(Name = "Country")]
        public string EmergencyContactConutryId2 { get; set; }
        public string EmergencyContactConutryName2 { get; set; }

        [Display(Name = "Code")]
        public string EmergencyContactConutryCode2 { get; set; }
        public string EmergencyContactCountryDialCode2 { get; set; }
        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }
        [Display(Name = "Date Of Issue")]
        public DateTime? DateOfIssue { get; set; }
        [Display(Name = "Expiry Date")]
        public DateTime? PassportExpiryDate { get; set; }
        [Display(Name = "Place Of Issue")]
        public string PlaceOfIssue { get; set; }

       // [Display(Name = "ID Type")]
        //public IDTypeEnum? IDType { get; set; }
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

        public string PassportDocumentId { get; set; }
        [Display(Name = "Legal Entity")]
        public string LegalEntityId { get; set; }

        public string LegalEntityName { get; set; }
        [Display(Name = "Personal Status")]
        public EmployeeStatusEnum? EmployeeStatus { get; set; }

        public string SalaryInfoId { get; set; }

        public string HierarchyId { get; set; }
        public string FatherName { get; set; }
        public string PreferedName { get; set; }
        public string IdCardName { get; set; }
       // [Required]
       // [Display(Name = "Date Of Join")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime PersonDateOfJoin { get; set; }
        public string TitleId { get; set; }
        public string MaritalStatusId { get; set; }
        public string ReligionId { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public string LineManagerId { get; set; }
        public string BusinessHierarchyParentId { get; set; }
        public string BusinessHierarchyReferenceType { get; set; }

        public string PresentAddressBuildingNumber { get; set; }
        public string PresentAddressStreetName { get; set; }
        public string PresentAddressCityOrTown { get; set; }
        public string PresentAddressCountryId { get; set; }
        public string PermanentAddressBuildingNumber { get; set; }
        public string PermanentAddressStreetName { get; set; }
        public string PermanentAddressCityOrTown { get; set; }
        public string PermanentAddressCountryId { get; set; }
        public string MobileNumber { get; set; }

    }
}
