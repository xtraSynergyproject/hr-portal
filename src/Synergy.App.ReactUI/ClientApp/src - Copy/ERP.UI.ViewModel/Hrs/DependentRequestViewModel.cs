using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class DependentRequestViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }

        public string Mode { get; set; }
        public string Errors { get; set; }

        public int? EmployeeId { get; set; }
        public int? EmployeeTypeId { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Personal Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Title")]
        public int TitleId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Arabic Name")]
        public string FullNameAr { get; set; }



        [DataType(DataType.Date)]
        [DateRange]
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime? DateOfBirth { get; set; }


        [Display(Name = "Birth Country")]
        public string BirthCountry { get; set; }

        [Display(Name = "Birth Town")]
        public string BirthTown { get; set; }

        [Display(Name = "Health Card No")]
        public string HealthCardNo { get; set; }

        [Display(Name = "Pension No")]
        public string PensionNo { get; set; }


        [Display(Name = "Nationality")]
        public string NationalityName { get; set; }


        //[Display(Name = "Nationality")]
        //public int NationalityId { get; set; }

        [Display(Name = "Dependent Type")]
        public int? DependentTypeId { get; set; }

        public string DependentTypeName { get; set; }
        public string RelationshipName { get; set; }

        [Display(Name = "Relationship")]
        [Required]
        public int? RelationshipId { get; set; }


        [Display(Name = "Nationality")]
        [Required]
        public int? NationalityId { get; set; }

        [Display(Name = "Gender")]
        public string GenderName { get; set; }


        //[Display(Name = "Gender")]
        //public int GenderId { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public int? GenderId { get; set; }

        [Display(Name = "Marital Status")]
        public string MaritalStatusName { get; set; }




        [Required]
        [Display(Name = "Marital Status")]
        public int? MaritalStatusId { get; set; }

        [DisplayFormat(DataFormatString = AnnotationDeclaration.DateFormat)]
        [Display(Name = "Date of Marriage")]
        public DateTime? DateofMarriage { get; set; }

        //[DisplayFormat(DataFormatString = AnnotationDeclaration.DateFormat)]
        //[Display(Name = "Date of Marriage")]
        //public DateTime? MarriageDate { get; set; }

        //[Display(Name = "Religion")]
        //public string Religion { get; set; }





        [Display(Name = "Religion")]
        public int? ReligionId { get; set; }

        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }


        [Display(Name = "Mobile Phone")]
        public string MobilePhone { get; set; }

        [Display(Name = "Current Address")]
        public string CurrentAddress { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Home Address")]
        public string HomeAddress { get; set; }


        [Display(Name = "Emirates Id")]
        public string EmiratesIdNo { get; set; }

        public TemporaryDocumentViewModel SelectedFile { get; set; }

        [Display(Name = "Photo")]
        public int? PhotoId { get; set; }


        public int? CandidateId { get; set; }

        public int? RequisitionId { get; set; }


        [DataType(DataType.Date)]
        [DateRange]
        [Display(Name = AnnotationDeclaration.Labels.EffectiveFromDate)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = AnnotationDeclaration.DateFormat)]
        public override DateTime? EffectiveFromDate { get; set; }

    }
}
