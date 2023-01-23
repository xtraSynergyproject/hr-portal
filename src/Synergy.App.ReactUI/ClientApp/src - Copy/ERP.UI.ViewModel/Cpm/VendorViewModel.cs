using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class VendorViewModel : ViewModelBase
    {
       // public long UserId { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }

        
        //[Display(Name = "Category")]
        //public VendorCategoryTypeEnum? Category { get; set; }
        [Required]
        [Display(Name = "Category")]
        public List<long> CategoryIds { get; set; }
        public string Category { get; set; }
        public List<string> CategoryNames { get; set; }
        [Display(Name = "Tax Registration")]
        public string TaxRegistration { get; set; }

        [Display(Name = "Auto Assigned")]
        public bool AutoAssigned { get; set; }
               

        //public UserTypeEnum UserType { get; set; }
                
        [Required]
        [StringLength(200)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Contact No")]
        public string ContactNumber { get; set; }

        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

                
        [Display(Name = "PostCode")]
        public string PostCode { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        public long CountryId { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "  Attachment")]
        public string VendorAttachmentId { get; set; }
     
        public FileViewModel VendorAttachmentSelectedFile { get; set; }

        public IList<FileViewModel> VendorDocuments { get; set; }

        [Display(Name = "Attachment Description")]
        public string AttachmentDescription { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Display(Name = "Swift Code")]
        public string SwiftCode { get; set; }
        public string ISBN { get; set; }

        [Display(Name = "Bank Account Type")]
        public CpmVendorAccountType? BankAccountType { get; set; }

    }


    public class PropertyViewModel : ViewModelBase
    {
        public string Property { get; set; }
        public string Owner { get; set; }
        public string Unit { get; set; }
        public string Beds { get; set; }
        public string Rent { get; set; }
        public string Deposits { get; set; }
    }
}


