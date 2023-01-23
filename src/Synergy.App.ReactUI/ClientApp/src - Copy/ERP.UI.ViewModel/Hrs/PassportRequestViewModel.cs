using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class PassportRequestViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }

        public string Errors { get; set; }
        public string Mode { get; set; }


        
        public int? EmployeeId { get; set; }

        [Display(Name = "Passport Owner")]
        public string OwnerName { get; set; }

        [Display(Name = "Passport Owner")]
        [Required]
        public int? OwnerId { get; set; }

        [Display(Name = "Passport Type")]
        [Required]
        public int? PassportTypeId { get; set; }

        [Display(Name = "Passport Type")]
        public string PassportTypeName { get; set; }

        [Display(Name = "Passport Number")]
        [Required]
        public string Number { get; set; }

        [Display(Name = "Issue Country")]
        [Required]
        public string IssueCountry { get; set; }

        [Display(Name = "Issue Place")]
        [Required]
        public string IssuePlace { get; set; }

        [Display(Name = "Issue Place (Arabic)")]
        public string IssuePlaceAr { get; set; }

        [Required]
        [Display(Name = "Issue Date")]
        [DisplayFormat(DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime? IssueDate { get; set; }

        [Required]
        [Display(Name = "Expiry Date")]
        [DisplayFormat(DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime? ExpiryDate { get; set; }
         

    }
}
