using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
    {
    public class PaymentReciptReportViewModel : ViewModelBase
    {
        [Required]
        public long UnitId { get; set; }
        public string Unit { get; set; }

        [Display(Name = "Project")]
        public string Project { get; set; }

        [Display(Name = "Attachment Description")]
        public string AttachmentDescription { get; set; }

        [Required]
        public long TenantId { get; set; }
        public string Tenant { get; set; }

        [Required]
        [Display(Name = " Upload Contract Attachment")]
        public string ContractAttachmentId { get; set; }

        [Display(Name = "Attachment")]
        public FileViewModel ContractAttachmentSelectedFile { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        public IList<FileViewModel> ContractDocuments { get; set; }

        [Required]
        [Display(Name = "Total Rent")]
        public string TotalRent { get; set; }

        [Required]
        [Display(Name = "Deposit")]
        public string Deposit { get; set; }

        [Display(Name = "Contract Terms")]
        public string ContractTerms { get; set; }

        [Display(Name = "Money Held By")]
        public MoneyHeldByTypeEnum? MoneyHeldBy { get; set; }

        [Display(Name = "Invoice Schedule")]
        public InvoiceScheduleTypeEnum? InvoiceSchedule { get; set; }

        [Display(Name = "Contract Payment Type")]
        public ContractPaymentTypeEnum? ContractPaymentType { get; set; }
      
        public string Cheques { get; set; }

        [Display(Name = "Days Left")]
        public string DaysLeft { get; set; }

        
        [Display(Name = "Due Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DueDate { get; set; }
        public DateTime? RefundDate { get; set; }
        public string PaymentInfo { get; set; }

        public string RefundAmount { get; set; }
        public IList<PaymentInfo> PaymentInformation { get; set; }

        public string ContractId { get; set; }
        public string Amount { get; set; }
        [Display(Name = "Paid Via")]
        public ContractPaymentTypeEnum? PaidVia { get; set; }
        [Display(Name = "Payment Category")]
        [Required]
        public string PaymentCategory { get; set; }
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Display(Name = "Cheque No")]
        public string ChequeNo { get; set; }
        [Required]
        [Display(Name = "Due Date")]

        public string Remark { get; set; }
        [Required]
        [Display(Name = " Upload Payment Attachment")]
        public string PaymentAttachmentId { get; set; }

        [Display(Name = "Attachment")]
        public FileViewModel PaymentAttachmentSelectedFile { get; set; }
        [Display(Name = "Attachment Description")]
        public string PaymentAttachmentDescription { get; set; }





        [Display(Name = "Tax Registration")]
        public string TaxRegistration { get; set; }

        [Display(Name = "Emirates ID")]
        public string EmiratesID { get; set; }

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

        [DataType(DataType.EmailAddress)]
        [StringLength(128)]
        //[Required()]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 4)]
        public string Password { get; set; }
        public string Phone { get; set; }

        [Display(Name = "Additional Phone 1")]
        public string AdditionalPhone1 { get; set; }

        [Display(Name = "Additional Phone 2")]
        public string AdditionalPhone2 { get; set; }

        [Display(Name = " Address 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        [Display(Name = " From Country")]
        public string FromCountryName { get; set; }
        [Display(Name = " From Country")]
        public long? FromCountryId { get; set; }
        [Display(Name = " Address country")]
        public string AddressCountryName { get; set; }
        [Display(Name = " Address country")]
        public long? AddressCountryId { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Trade license")]
        public string TradeLicense { get; set; }
        [Display(Name = "Date of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DOB { get; set; }
        public GenderEnum? Gender { get; set; }
        public MaritalStatusEnum? MaritalStatus { get; set; }
        public string State { get; set; }

        [Display(Name = "Display as company")]
        public bool Displayascompany { get; set; }

        [Display(Name = "Passport Attchment")]
        public string PassportAttachmentId { get; set; }


        [Display(Name = "Photo Attchment")]
        public string PhotoAttachmentId { get; set; }
        [Display(Name = "Photo Attchment")]
        public FileViewModel PhotoSelectedFile { get; set; }


        [Display(Name = "Visa Attchment")]
        public string VisaAttachmentId { get; set; }
        [Display(Name = "Visa Attchment")]
        public FileViewModel VisaSelectedFile { get; set; }


        [Display(Name = " Contact Number")]
        public string ContactNumber { get; set; }

        [Display(Name = " Unit Number")]
        public string UnitNumber { get; set; }

        [Display(Name = " Number of Contracts")]
        public string NumberofContracts { get; set; }

    }

          
  
   
}
