using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
    {
    public class OwnerViewModel : ViewModelBase
    {
        public long OwnerId { get; set; }
        [Required]
        //public int id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

       
        [Display(Name = " Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Tax Registration")]
        public string TaxRegist  { get; set; }
        [Display(Name = "Passport No.")]
        public string PassportNo { get; set; }

        [Display(Name = "Passport Expiry")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime PassportExpiry { get; set; }

        [Display(Name = "Visa Expiry")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime VisaExpiry { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Required]
        [Display(Name = "Bank Address")]
        public string Address { get; set; }

        [Required]
        public string IBAN { get; set; }

        [Required]
        public string SWIFT { get; set; }
        public string Notes { get; set; }

        [Required]
        [Display(Name = "Send Push Notifications")]
        public Boolean SendPushNotifications { get; set; }

        [Required]
        [Display(Name = "Send Email Notifications")]
        public Boolean SendEmailNotifications { get; set; }

        [Required]    
        public string OwnerAttachmentId { get; set; }
        [Display(Name = "Attchment")]
        public FileViewModel OwnerSelectedFile { get; set; }
        public IList<FileViewModel> OwnerDocuments { get; set; }
        public string properties { get; set; }
        public string Options { get; set; }
        [Display(Name = "Attachment Description")]
        public string AttachmentDescription { get; set; }
        public string TotalPorfolioAmount { get; set; }
    }
}
