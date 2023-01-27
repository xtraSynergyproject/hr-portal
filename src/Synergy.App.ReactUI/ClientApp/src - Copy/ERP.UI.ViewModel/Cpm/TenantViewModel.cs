using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace ERP.UI.ViewModel
    {
    public class TenantViewModel : ViewModelBase
    {
        //public int id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Display(Name = " Full Name")]
        public string FullName { get; set; }


        
        [Display(Name = "Tax Registration")]
        public string TaxRegistration { get; set; }

        [Required]
        [Display(Name = "Emirates ID")]
        public string EmiratesID { get; set; }

        [Required]
        [Display(Name = "Emirates ID Expiry")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EmiratesIDExpiry { get; set; }
    
        [Display(Name = "Passport No.")]
        public string PassportNo { get; set; }
        [Display(Name = "Passport Expiry")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime PassportExpiry { get; set; }
 
        [Display(Name = "Visa State")]
        public string VisaState { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(128)]
        //[Required()]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 4)]
        public string Password { get; set; }

        [Required]
        public string Phone { get; set; }

        [Display(Name = "Additional Phone 1")]
        public string AdditionalPhone1 { get; set; }

        [Display(Name = "Additional Phone 2")]
        public string AdditionalPhone2 { get; set; }

        [Required]
        [Display(Name = " Address 1")]
        public string Address1 { get; set; }

        [Required]
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        [Display(Name = " From Country")]
        public string FromCountryName { get; set; }
        [Display(Name = " From Country")]
        public long? FromCountryId { get; set; }

        [Display(Name = " Address country")]
        public string AddressCountryName { get; set; }

        [Required]
        [Display(Name = " Address country")]
        public long? AddressCountryId { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Trade license")]
        public string TradeLicense { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DOB { get; set; }

        [Required]
        public GenderEnum? Gender { get; set; }

        [Required]
        public MaritalStatusEnum? MaritalStatus { get; set; }

        [Required]
        public string State { get; set; }

        [Display(Name = "Display as company")]
        public bool Displayascompany { get; set; }

        [Display(Name = "Passport Attachment")]
        public string PassportAttachmentId { get; set; }
        public string PassportAttachmentDescription { get; set; }
        public FileViewModel PassportSelectedFile { get; set; }
        public IList<FileViewModel> TenantDocuments { get; set; }
        public IList<FileViewModel> TenantDocuments1 { get; set; }
        public IList<FileViewModel> TenantDocuments2 { get; set; }
        public IList<FileViewModel> TenantDocuments3 { get; set; }

        [Required]
        [Display(Name = "Photo Attachment")]
        public string PhotoAttachmentId { get; set; }
        public string PhotoAttachmentDescription { get; set; }
        public FileViewModel PhotoSelectedFile { get; set; }       

        [Display(Name = "Visa Attachment")]
        public string VisaAttachmentId { get; set; }
        public string VisaAttachmentDescription { get; set; }
        public FileViewModel VisaSelectedFile { get; set; }

        [Required]
        [Display(Name = "Additional Attachment")]
        public string AdditionalDocAttachmentId { get; set; }

        public string AdditionalDocDescription { get; set; }
        public FileViewModel AdditionalDocSelectedFile { get; set; }
      
        [Required]
        public string PostCode { get; set; }

        public long? AttachmentId { get; set; }

        public string Notes { get; set; }
        [Display(Name = "Guest Type")]
        public GuestTypeEnum? GuestType { get; set; }
        [Display(Name = "Type")]
        public VehicleTypeEnum? VehicleType { get; set; }
        [Display(Name = " Model")]
        public string VehicleModel { get; set; }
        [Display(Name = " Make")]
        public string VehicleMake { get; set; }
        [Display(Name = " Color")]
        public string VehicleColor { get; set; }
        [Display(Name = " Year")]
        public string VehicleYear { get; set; }
        [Display(Name = " Plate No.")]
        public string VehiclePlateNo { get; set; }

        [Required]
        public string City { get; set; }

        [Display(Name = " Contact Number")]
        public string ContactNumber { get; set; }

        [Display(Name = " Unit Number")]
        public string UnitNumber { get; set; }

        [Display(Name = " Number of Contracts")]
        public string NumberofContracts { get; set; }

        public string CountryNameCount { get; set; }
        public int NumberofCountry { get; set; }       
        public CpmContractStatusEnum ContractStatus { get; set; }

        [Required]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Required]
        [Display(Name = "Bank Address")]
        public string BankAddress { get; set; }

        [Required]
        public string IBAN { get; set; }

        [Required]
        public string SWIFT { get; set; }

    }
}
