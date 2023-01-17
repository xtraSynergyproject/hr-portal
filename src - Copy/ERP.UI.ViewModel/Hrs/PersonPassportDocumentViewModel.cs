
using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PersonPassportDocumentViewModel :PersonDocumentViewModel
    {
        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }
        [Display(Name = "Issue Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? PassIssueDate { get; set; }
        [Display(Name = "Expiry Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? PassExpiryDate { get; set; }
        [Display(Name = "Place Of Issue")]
        public string PlaceOfIssue { get; set; }
        [Display(Name = "Nationality")]
        public long? NationalityId { get; set; }
        [Display(Name = "Nationality")]
        public string NationalityName { get; set; }
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfBirth { get; set; }
        [Display(Name = "Place Of Birth")]
        public string PlaceOfBirth { get; set; }
        [Display(Name = "Birth Country")]
        public long? BirthCountry { get; set; }
        [Display(Name = "Birth Country")]
        public string BirthCountryName { get; set; }
        public string PassportAttachment { get; set; }

    }
}
