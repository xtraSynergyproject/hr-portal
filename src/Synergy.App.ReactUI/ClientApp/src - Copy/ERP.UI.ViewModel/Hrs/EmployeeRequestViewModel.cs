using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class EmployeeRequestViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }

        public bool IsGridVisible { get; set; }

        public string Errors { get; set; }

        public int CandidateId { get; set; }

        public int RequisitionId { get; set; }

        public int NextTab { get; set; }

        [Display(Name = "Employee Remarks")]
        public string Remarks { get; set; }

        [Required]
        [Display(Name = "Vendor Name")]
        public int? VendorId { get; set; }

        public string VendorName { get; set; }

        [Display(Name = "Employee Number")]
        public string EmployeeNo { get; set; }


        [Display(Name = "Personal Email")]
        public string PersonalEmail { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

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

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }


        [Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Birth Country")]
        public string BirthCountry { get; set; }

        [Display(Name = "Birth Town")]
        public string BirthTown { get; set; }

        [Display(Name = "Health Card No")]
        public string HealthCardNo { get; set; }

        [Display(Name = "Pension No")]
        public string PensionNo { get; set; }


        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [Required]
        [Display(Name = "Nationality")]
        public int NationalityId { get; set; }

        [Display(Name = "Employee Type")]
        public string EmployeeTypeName { get; set; }

        [Required]
        [Display(Name = "Employee Type")]
        public int EmployeeTypeId { get; set; }


        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public int GenderId { get; set; }


        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }

        [Required]
        [Display(Name = "Marital Status")]
        public int? MaritalStatusId { get; set; }


        [DataType(DataType.DateTime)]
        [DateRange]
        [Display(Name = "Date of Marriage")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime? DateofMarriage { get; set; }


        [Display(Name = "Religion")]
        public string Religion { get; set; }

        [Required]
        [Display(Name = "Religion")]
        public int? ReligionId { get; set; }

        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }

        [Required]
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

        public TransactionMode Mode { get; set; }

        public TemporaryDocumentViewModel SelectedFile { get; set; }

        [Display(Name = "Photo")]
        public int? PhotoId { get; set; }

        [Display(Name = "Employee Effective From Date")]
        public override DateTime? EffectiveFromDate
        {
            get
            {
                return base.EffectiveFromDate;
            }

            set
            {
                base.EffectiveFromDate = value;
            }
        }
        [Display(Name = "Employee Effective To Date")]
        public override DateTime? EffectiveToDate
        {
            get
            {
                return base.EffectiveToDate;
            }

            set
            {
                base.EffectiveToDate = value;
            }
        }


    }
}
