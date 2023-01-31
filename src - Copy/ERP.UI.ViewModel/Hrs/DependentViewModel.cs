using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class DependentViewModel : DatedViewModelBase
    {
        public long DependentId { get; set; }
        [Display(Name = "Employee Name")]
        public long PersonId { get; set; }
        [Required]
        public DependantRelationshipTypeEnum? RelationshipType { get; set; }       
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

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
        [Required]
        public GenderEnum? Gender { get; set; }
        [Required]
        [Display(Name = "Iqamah ID/Nationality ID")]
        public string SponsorshipNo { get; set; }
        [Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfBirth { get; set; }
        [Display(Name = "Place Of Birth")]
        public string BirthTown { get; set; }

        [Required]
        [Display(Name = "Birth Country")]
        public long? BirthCountryId { get; set; }

        [Display(Name = "Birth Country Name")]
        public string BirthCountryName { get; set; }

        [Display(Name = "DocumentAttach")]
        public long? DocumentId { get; set; }
        public FileViewModel SelectedFile { get; set; }

        public string PersonFullName { get; set; }
        [Display(Name = "Sponsor Id")]
        public string SponsorId { get; set; }

        [Display(Name = "Photo")]
        public long? PhotoId { get; set; }
        public long? NationalityId { get; set; }
        public string PersonNo { get; set; }

        public string NationalityName { get; set; }
    }
}
