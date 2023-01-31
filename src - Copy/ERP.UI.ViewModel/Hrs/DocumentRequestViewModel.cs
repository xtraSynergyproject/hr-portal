using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.Web;

namespace ERP.UI.ViewModel
{
    public class DocumentRequestViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }

        public string Mode { get; set; }


        public string Errors { get; set; }

        [Display(Name = "Document Owner")]
        public string OwnerName { get; set; }

        [Display(Name = "Document Owner")]
        [Required]
        public int? OwnerId { get; set; }


        public int? EmployeeId { get; set; }

        [Display(Name = "Document Type")]
        public string DocumentTypeName { get; set; }

        [Display(Name = "Document Type")]
        [Required]
        public string DocumentTypeCode { get; set; }

        [Display(Name = "Document Name")]
        public string Name { get; set; }

        [Display(Name = "File")]
        public string FileName { get; set; }


        [Display(Name = "File")]
        [Required]
        public int? TemporaryDocumentId { get; set; }



        public TemporaryDocumentViewModel SelectedFile { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }



        [Display(Name = "Issue Date")]
        [DisplayFormat(DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime? IssueDate { get; set; }


        [Display(Name = "Expiry Date")]
        [DisplayFormat(DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime? ExpiryDate { get; set; }

        [Display(Name = "Issued By")]
        public string IssuedBy { get; set; }
    }
}
