using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
    {
    public class CpmContractViewModel : ViewModelBase
    {
        [Required]
        public long UnitId { get; set; }
        public string Unit { get; set; }

        [Display(Name = "Unit Classification")]
         public CpmUnitClassification? UnitClassifications { get; set; }
         public string UnitClassification { get; set; }
        //public long? UnitClassification { get; set; }

        [Display(Name = "Project")]
        public long? ProjectId { get; set; }
        [Display(Name = "Project")]
        public string ProjectName { get; set; }

        [Required]
        [Display(Name = "Demoghraphic Type")]
        public CpmDemoghraphicTypeEnum? DemoghraphicType { get; set; }

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
       
        [Display(Name = "Move Out Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? MoveOutDate { get; set; }
        [Display(Name = "Contract Complete Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ContractCompleteDate { get; set; }

        [Display(Name = "Refund Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? RefundDate { get; set; }
        public IList<FileViewModel> ContractDocuments { get; set; }

        [Required]
        [Display(Name = "Total Rent (Annual)")]
        public string TotalRent { get; set; }
        

        [Display(Name = "Refund Notes")]
        public string RefundNotes { get; set; }
        [Display(Name = "Refund Amount")]
        public string RefundAmount { get; set; }

        public string CommissionMoney { get; set; }

        [Required]
        [Display(Name = "Deposit")]
        public string Deposit { get; set; }

        [Display(Name = "Commission (%)")]
        public float Commission { get; set; }   

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
        public CpmDueDateEnum? DueDate { get; set; }

        public CpmContractStatusEnum? ContractStatus { get; set; }
        [Display(Name = "Additional Charge Notes")]
        public string AdditionalNotes { get; set; }
        public string PaymentInfo { get; set; }
        public string Email { get; set; }
        public bool? IsRenew { get; set; }
        public List<PaymentInfo> PaymentInformation { get; set; }
        public string VersionNo { get; set; }
        [Required]
        [Display(Name = "Contract No")]
        public string ContractNo { get; set; }
        [Display(Name = "New Contract No")]
        public string NewContractNo { get; set; }
        public string Tax { get; set; }
    }

    public class PaymentInfo : ViewModelBase
    {      

        public long? ServiceId { get; set; }

        [Display(Name = "Contract")]
        [Required]
        public long ContractId { get; set; }
        [Required]
        [Display(Name = "Project")]
        public long? ProjectId { get; set; }
        [Display(Name = "Project")]
        public string Project { get; set; }
        [Display(Name = "Owner")]
        public long? OwnerId { get; set; }

        [Display(Name = "Payment Name")]
        public string PaymentName { get; set; }

        [Required]
        [Display(Name = "Tenant")]
        public long? TenantId { get; set; }
        [Required]
        [Display(Name = "Unit")]
        public long? UnitId { get; set; }

        public string Tenant { get; set; }

        public string Unit { get; set; }

        public string Amount { get; set; }
        [Display(Name = "Paid Via")]
        public ContractPaymentTypeEnum? PaidVia { get; set; }
        [Required]
        [Display(Name = "Status")]
        public ContractPaymentStatusEnum? PaymentStatus { get; set; }
        [Display(Name = "Payment Type")]
        [Required]
        public PaymenTypeEnum? PaymentType { get; set; }

        [Display(Name = "Payment Category")]
        [Required]
        public string PaymentCategoryId { get; set; }
        public string PaymentCategoryName { get; set; }
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Display(Name = "Cheque No")]
        public string ChequeNo { get; set; }
        [Display(Name = "New Cheque No")]
        public string NewChequeNo { get; set; }
        public string ChequeAmount { get; set; }
        [Required]
        [Display(Name = "Due Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DueDate { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? NewDueDate { get; set; }

        [Display(Name = "Paid Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? PaidDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? CurrentDate { get; set; }
        public string Remark { get; set; }

        [Display(Name = " Upload Payment Attachment")]
        public string PaymentAttachmentId { get; set; }

        [Display(Name = "Attachment")]
        public FileViewModel PaymentAttachmentSelectedFile { get; set; }
        [Display(Name = "Attachment Description")]
        public string PaymentAttachmentDescription { get; set; }

        public FileViewModel DepositeSlipSelectedFile { get; set; }
        [Display(Name = "Deposite Slip Description")]
        public string DepositeSlipDescription { get; set; }
        [Display(Name = "Deposite Slip")]
        public string DepositeSlipId { get; set; }
        public string Note { get; set; }
        [Display(Name = "Account")]
        public ContractAccountEnum? Account { get; set; }
        public string Tax { get; set; }
        public string Code { get; set; }
        public string TaxRegistration { get; set; }
        public string ProcessingFees { get; set; }
        public string TotalAmount { get; set; }


        [Display(Name = "Bounced Date ")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? BouncedDate { get; set; }
        [Display(Name = "Bounced Penalty Amount")]
        public string BouncedAmount { get; set; }
        [Display(Name = "Bounced Penalty Amount Per Day")]
        public string BouncedAmountPerDay { get; set; }
        [Display(Name = "Bounced Notes")]
        public string BouncedNotes { get; set; }
        public bool IsBounced { get; set; }
        public string MaintenanceCost { get; set; }

        public string PMcommsion { get; set; }

        [Display(Name = "Recieved From")]
        public string RecievedFrom { get; set; }

        [Display(Name = "Beneficiary Account")]
        public string BeneficiaryAccount { get; set; }

        public CpmPayeeBeneficiaryEnum? Payee { get; set; }
        public CpmPayeeBeneficiaryEnum? Beneficiary { get; set; }
    }
}
