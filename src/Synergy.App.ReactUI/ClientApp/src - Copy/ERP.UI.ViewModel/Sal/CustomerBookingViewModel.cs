using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class CustomerBookingViewModel : ViewModelBase
    {
        [Display(Name = "Full Name")]
        [Required]
        public string FullName { get; set; }
        [Display(Name = "National ID")]
        public string NationalID { get; set; }
        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }
        [Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime DOB { get; set; }
        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? IssueDate { get; set; }
        [Display(Name = "Issue Place")]
        public string IssuePlace { get; set; }
        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ExpiryDate { get; set; }
        [Display(Name = "Mobile Number")]
        [Required]
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        [Display(Name = "PO Box")]
        public string POBox { get; set; }
        public string City { get; set; }
        [Display(Name = "House/Flat Number")]
        public string HouseNo { get; set; }
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        public string District { get; set; }
        //[Display(Name = "Deposit Proof")]
        //public DepositProofEnum? DepositProof { get; set; }
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Display(Name = "Bank Branch")]
        public string Branch { get; set; }


        public string IBAN { get; set; }
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }
        [Display(Name = "Booking Status")]
        public BookingStatusEnum BookingStatus { get; set; }



        [Display(Name = "Project")]

        public string Project { get; set; }

        [Display(Name = "Unit type")]

        public string UnitType { get; set; }

        [Display(Name = "Unit")]

        public string Unit { get; set; }

        [Display(Name = "Payment Plan")]

        public string PaymentPlan { get; set; }

        [Display(Name = "Nationality")]
        public long NationalityId { get; set; }
        public string Nationality { get; set; }

        // Token Form Fields

        [Required]
        [Display(Name = "Reference No")]
        public string ReferenceNo { get; set; }
        [Required]
        [Display(Name = "Booking Amount")]
        public long? TokenAmount { get; set; }
        [Required]
        [Display(Name = "Payment Mode")]
        public SalPaymentModeEnum PaymentMode { get; set; }
        //[Required]       
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Date")]
        public DateTime? PaymentDate { get; set; }
        [Required]
        [Display(Name = "Lead Name")]
        public long LeadId { get; set; }
        [Display(Name = "Lead Name")]
        public string LeadName { get; set; }
        public long? LeadPersonId { get; set; }
        public string LeadPersonName { get; set; }
        [Required]
        [Display(Name = "Project Name")]
        public long ProjectId { get; set; }
        [Display(Name = "Project Name")]

        public string ProjectName { get; set; }
        [Required]
        [Display(Name = "Unit Type Name")]
        public long UnitTypeId { get; set; }
        [Display(Name = "Unit Type Name")]

        public string UnitTypeName { get; set; }
        [Required]
        [Display(Name = "Unit Number")]
        public long UnitId { get; set; }
        [Display(Name = "Unit Number")]
        public long[] UnitIdData { get; set; }
        [Display(Name = "Unit Number")]
        public string UnitName { get; set; }

        public long? ServiceId { get; set; }


        public FileViewModel OtherDocumentsSelectedFile { get; set; }
        [Required]
        public string Attachments { get; set; }

        [Display(Name = "Currency")]
        public long CurrencyId { get; set; }
        [Display(Name = "Currency")]
        public string CurrencyName { get; set; }
        public string ConvertedTokenAmount { get; set; }

        public long? NoOfUnitsCount { get; set; }
        public string LeadStatus { get; set; }
        public long? PropertyConsultantId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? ProjectDetailId { get; set; }
        [Display(Name = "Consultant Name")]
        public string PropertyConsultantName { get; set; }
        [Display(Name = "Direct Reportee Name")]
        public string DirectReporteeName { get; set; }

        public long? PositionId { get; set; }

        [Required]
        [Display(Name = "Payment Plan Name")]
        public long PaymentPlanId { get; set; }
        [Display(Name = "Payment Plan Name")]
        public string PaymentPlanName { get; set; }
        [Display(Name = "Discount In %")]
        public float PaymentPlanDiscount { get; set; }
        [Display(Name = "Discount Amount")]
        public string DiscountAmount { get; set; }
        [Display(Name = "Bank Name")]
        public string Bank { get; set; }

        public string PreviousDiscount { get; set; }
        [Display(Name = "Total Booking Amount")]
        public string TotalTokenAmount { get; set; }
        [Display(Name = "Balanced DownPayment")]
        public string BalanceDownPayment { get; set; }
        [Display(Name = "Down Payment After Discount")]
        public string DownPaymentAfterDiscount { get; set; }
        [Display(Name = "DLD Fee On DUP")]
        public string DLDOnDiscountUnitPrice { get; set; }
        public string UserRole { get; set; }
        public string SalesAgent { get; set; }
        public string UnitPriceAfterDiscount { get; set; }

        [Display(Name = "Attachment Description")]
        public string AttachmentDescription { get; set; }
        public bool IsPropertyConsultant { get; set; }
        [Display(Name = "Other Govt. Charges")]
        public string Others { get; set; }

    }
}

