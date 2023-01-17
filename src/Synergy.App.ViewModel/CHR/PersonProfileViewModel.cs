using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class PersonProfileViewModel : ViewModelBase
    {
        public string PersonId { get; set; }
        public string PersonNoteId { get; set; }
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public string PositionId { get; set; }

        [Display(Name = "Person No")]
        public string PersonNo { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Gender")]
        public GenderEnum? Gender { get; set; }

        [Display(Name = "Nationality")]
        public string NationalityName { get; set; }

        [Display(Name = "Marital Status")]
        public MaritalStatusEnum? MaritalStatus { get; set; }

        [Display(Name = "Religion")]
        public string Religion { get; set; }

        [Display(Name = "Birth Town")]
        public string BirthTown { get; set; }

        [Display(Name = "Birth Country")]
        public string BirthCountryName { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
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
        public string OrganizationName { get; set; }

        [Display(Name = "Job")]
        public string JobName { get; set; }

        [Display(Name = "Position")]
        public string PositionName { get; set; }

        [Display(Name = "Tenture Period")]
        public string AssignmentTypeName { get; set; }

        [Display(Name = "Date Of Join")]
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? DateOfJoin { get; set; }

        [Display(Name = "Person Type")]
        public string PersonTypeName { get; set; }

        [Display(Name = "Photo")]
        public string PhotoName { get; set; }
        public string base64Img { get; set; }

        [Display(Name = "Assignment Status")]
        public string AssignmentStatusName { get; set; }

        public string Page { get; set; }

        public string DependentId { get; set; }

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
        public string PresentConutryId { get; set; }

        [Display(Name = "Present Country Name")]
        public string PresentCountryName { get; set; }
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
        public string HomeConutryId { get; set; }
        [Display(Name = "Home Country Name")]
        public string HomeCountryName { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string EmergencyContactName1 { get; set; }
        [Display(Name = "Mobile Number")]
        [Required]
        public string EmergencyContactNo1 { get; set; }
        [Display(Name = "Relationship")]
        [Required]
        public string Relationship1 { get; set; }
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
        public string Relationship2 { get; set; }
        [Display(Name = "Other Relation")]
        public string OtherRelation2 { get; set; }
        [Display(Name = "Country")]
        [Required]
        public string ContactConutryId { get; set; }

        [Display(Name = "Country")]
        public string ContactCountryName { get; set; }
        [Display(Name = "Code")]
        public string ContactCountryCode { get; set; }
        public string ContactCountryDialCode { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string EmergencyContactConutryId1 { get; set; }

        [Display(Name = "Emergency Contact Country Name")]
        public string EmergencyContactCountryName1 { get; set; }
        [Display(Name = "Code")]
        public string EmergencyContactConutryCode1 { get; set; }
        public string EmergencyContactCountryDialCode1 { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string EmergencyContactConutryId2 { get; set; }

        [Display(Name = "Emergency Contact Country Name")]
        public string EmergencyContactCountryName2 { get; set; }

        [Display(Name = "Code")]
        public string EmergencyContactConutryCode2 { get; set; }
        public string EmergencyContactCountryDialCode2 { get; set; }

        public IList<NoteIndexPageTemplateViewModel> NoteTableRows { get; set; }

        public string TemplateId { get; set; }
        public string[] UserRoleCodes { get; set; }

    }
}
