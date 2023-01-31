using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class VisaRequestViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }


        [Display(Name = "Visa Owner")]
        public string OwnerName { get; set; }


        public string Errors { get; set; }

        [Display(Name = "Visa Owner")]
        [Required]
        public int? OwnerId { get; set; }

        public string Mode { get; set; }


        [Display(Name = "Place of Issue")]
        [Required]
        public string PlaceofIssue { get; set; }

        [Required]
        [Display(Name = "Visa U.I.D No")]
        public string PermitNumber { get; set; }

        [Required]
        [Display(Name = "File Number")]
        public string FileNumber { get; set; }

        [Display(Name = "Visa Type")]
        public string VisaType { get; set; }

        [Display(Name = "Visa Type")]
        [Required]
        public int? VisaTypeId { get; set; }

        public string VisaTypeName { get; set; }

        [Required]
        [Display(Name = "Issue Date")]
        [DisplayFormat(DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime? IssueDate { get; set; }

        [Required]
        [Display(Name = "Expiry Date")]
        [DisplayFormat(DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime? ExpiryDate { get; set; }

  
        [Display(Name = "Immigration Job")]
        public string ImmigrationJob { get; set; }

        [Display(Name = "Immigration Education")]
        public string ImmigrationEducation { get; set; }

    }
}
