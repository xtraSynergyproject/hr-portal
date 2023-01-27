using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class CpmReportViewModel : ViewModelBase
    {

       // [Required]
        [Display(Name = "Property")]
        public long? UnitId { get; set; }
        public long? PaymentId { get; set; }
        public string Unit { get; set; } 
        public string UnitNumber { get; set; }
        [Display(Name = "Unit Status")]
        public string UnitStatus { get; set; }
        [Display(Name = "Unit Category")]
        public string UnitCategory { get; set; }

        [Display(Name = "Contract Status")]
        public CpmContractStatusEnum? ContractStatus { get; set; }
        [Display(Name = "Contract No")]
        public string ContractNo { get; set; }

        [Display(Name = "Beds")]
        public string NumberOfBedrooms { get; set; }
        public string UnitType { get; set; }

        [Display(Name = "Work Order")]
        public string WorkOrder { get; set; }

        [Display(Name = "Project")]
        public long? ProjectId { get; set; }
        [Display(Name = "Project")]
        public string Project { get; set; }

        [Display(Name = "Project")]
        public string PropertyName { get; set; }
        public long? PropertyId { get; set; }
        public int MaintenanceNumber { get; set; }
        public string Vendor { get; set; }
        public long? VendorId { get; set; }
        [Display(Name = "Maintenance Category")]
        public string MaintenanceCategory { get; set; }
        public string MaintenanceCategoryName { get; set; }
        [Display(Name = "Title Name")]
        public string TitleName { get; set; }
        public DateTime? AvailabilityDate { get; set; }
        public DateTime? StatusChangeDate { get; set; }
        public string ServiceNo { get; set; }


        public long TenantId { get; set; }
        public string Tenant { get; set; }
        public string PaymentMode { get; set; }
        public string TotalIncome { get; set; }
        public string TotalExpense { get; set; }
        public string NetIncome { get; set; }

        public long? AttachmentId { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }



        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }

        [Required]
        
        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? CompletionDate { get; set; }
        public string TotalDays { get; set; }

        [Display(Name = "Rent")]
        public string Rent { get; set; }
        [Display(Name = "Total Rent")]
        public string TotalRent { get; set; }

        [Display(Name = "Contract Terms")]
        public string ContractTerms { get; set; }

        [Display(Name = "Money Held By")]
        public MoneyHeldByTypeEnum? MoneyHeldBy { get; set; }

        [Display(Name = "Invoice Schedule")]
        public InvoiceScheduleTypeEnum? InvoiceSchedule { get; set; }

        [Display(Name = "Contract Payment Type")]
        public ContractPaymentTypeEnum? ContractPaymentType { get; set; }
        public long? PaymentAttachmentId { get; set; }
        public FileViewModel PaymentAttachmentSelectedFile { get; set; }
        [Display(Name = "Attachment Description")]
        public string PaymentAttachmentDescription { get; set; }
        public string Cheques { get; set; }

        [Display(Name = "Days Left")]
        public string DaysLeft { get; set; }


        [Display(Name = "Due Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DueDate { get; set; }
        public DateTime? DueDate { get; set; }


        public string PaymentInfo { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name (In Arabic)")]
        public string FullNameLocal { get; set; }
        private string _FullName { get; set; }

        [Display(Name = "Person Full Name")]
        public string FullName
        {
            set { _FullName = value; }
            get
            {
                return _FullName.IsNullOrEmpty() ?
                 string.Concat(FirstName, " ", MiddleName, MiddleName.IsNullOrEmptyOrWhiteSpace() ? "" : " "
                    , LastName) : _FullName;
            }
        }



        public string ContractId { get; set; }
        [Display(Name = "Owner")]
        public long? OwnerId { get; set; }

        [Display(Name = "Paid Via")]
        public ContractPaymentTypeEnum? PaidVia { get; set; }
        public ContractPaymentStatusEnum? PaymentStatus { get; set; }
        public PaymenTypeEnum? PaymentType { get; set; }

        [Display(Name = "Payment Category")]
        public string PaymentCategory { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Display(Name = "Cheque No")]
        public string ChequeNo { get; set; }

        [Display(Name = "Paid Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? PaidDate { get; set; }
        [Display(Name = "Refund Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? RefundDate { get; set; }
        public string Remark { get; set; }
        [Display(Name = "Refund Issued")]
        public string RefundIssued { get; set; }
        public string RefundAmount { get; set; }
        public string MaintenanceCost { get; set; }
        public string WorkOrderNo { get; set; }


        [Display(Name = "Deposite Slip Description")]
        public string DepositeSlipDescription { get; set; }
        [Display(Name = "Deposite Slip")]
        public string DepositeSlipId { get; set; }
        [Display(Name = "Notes")]
        public string Note { get; set; }
        
        [Display(Name = "Tenant")]
        public string TenantName { get; set; }

        [Display(Name = "Landloard")]
        public string OwnerName { get; set; }

        [Display(Name = "Receipt No.")]
        public string ReceiptNo { get; set; }

        public string coment { get; set; }

        public string Bedroom { get; set; }
        public CpmIntervalEnum? Interval { get; set; }
        public TimeSlotTypeEnum? TimeSlot { get; set; }

        public string Tax { get; set; }    

        #region Property Income Statement related properties

        [Display(Name = "Payment Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "Payment Type")]
        public string PaymentType1 { get; set; }

        public string Details { get; set; }

        public string Amount { get; set; }

        public string Deposit { get; set; }

        [Display(Name = "Maintenance Status")]
        public MaintenanceStatusEnum? MaintenanceStatus { get; set; }

        [Display(Name = "Move Out Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? MoveOutDate { get; set; }





        [Display(Name = "Address")]
        public string Address { get; set; }


        [Display(Name = "Unit ")]
        public string UnitWithApartmentType { get; set; }

        [Display(Name = "Unit Type")]
        public string UnitTypeName { get; set; }

   
        [Display(Name = "Size sqft")]
        public string SizeSqft { get; set; }

        [Display(Name = "Floor")]
        public string FloorNumber { get; set; }

        [Display(Name = "Unit Area")]
        public string UnitArea { get; set; }

        [Display(Name = "Beds")]
        public string NumberOfBedroomsName { get; set; }

        [Display(Name = "Baths")]
        public string NumberOfBaths { get; set; }

      
        [Display(Name = "Electricity No")]
        public string ElectricityNumber { get; set; }

        [Display(Name = "Municipality No")]
        public string MunicipalityNumber { get; set; }

        [Display(Name = "Compound No")]
        public string CompoundNumber { get; set; }

        [Display(Name = "Number of Parkings")]
        public string NumberOfParking { get; set; }

        [Display(Name = "Parking No")]
        public string ParkingNumber { get; set; }


        [Display(Name = "Management Fee type ")]
        public ManagementFeeTypeEnum? ManagementFeeType { get; set; }

       
        [Display(Name = "Furnished")]
        public bool Furnished { get; set; }

        [Display(Name = "SmokingAllowed")]
        public bool SmokingAllowed { get; set; }

        [Display(Name = "Electricity")]
        public bool Electricity { get; set; }

        [Display(Name = "Internet")]
        public bool Internet { get; set; }

        [Display(Name = "Water")]
        public bool Water { get; set; }

        [Display(Name = "Air Conditioning")]
        public bool AirConditioning { get; set; }

        [Display(Name = "Carpet")]
        public bool Carpet { get; set; }

        [Display(Name = "Fireplace")]
        public bool FirePlace { get; set; }

        [Display(Name = "Management Fee")]
        public string ManagementFee { get; set; }

        [Display(Name = "Total Area")]
        public string TotalArea { get; set; }

        [Display(Name = "Balcony Area")]
        public string BalconyArea { get; set; }

        [Display(Name = "Parking")]
        public int? NoOfParking { get; set; }

        [Display(Name = "Marketing Title")]
        public string MarketingTitle { get; set; }

        [Display(Name = "Marketing Description")]
        public string MarketingDescription { get; set; }
        
        [Display(Name = "Expiry Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Unit Pictures ")]
        public string PropertyAttachmentId { get; set; }

        [Display(Name = "Property Photo")]
        public FileViewModel PropertyAttachmentSelectedFile { get; set; }

        [Display(Name = "Listing Attachment")]
        public string ListingAttachmentId { get; set; }

        [Display(Name = "Listing Images")]
        public FileViewModel ListingAttachmentSelectedFile { get; set; }

        [Display(Name = "Document Attachment")]
        public string DocumentsAttachmentId { get; set; }

        [Display(Name = "Documents")]
        public FileViewModel DocumentsSelectedFile { get; set; }
        public IList<FileViewModel> UnitDocuments { get; set; }
        public IList<FileViewModel> UnitListDocuments { get; set; }

        [Display(Name = " Attachment Description")]
        public string DocumentDescription { get; set; }

        [Display(Name = "Attachment Description")]
        public string PictureDescription { get; set; }
        [Display(Name = "Property Management Fee")]
        public string PropertyManagementFee { get; set; }
        [Display(Name = "Owner Distribution")]
        public string OwnerDistribution { get; set; }
        [Display(Name = "Total Service Charges Expense")]
        public string TotalServiceCharge { get; set; }

        [Display(Name = "Contract No.")]
        public string ContractNumber { get; set; }

        public string TotalMaintenanceExpense { get; set; }
        public string[] IncomeElements { get; set; }
        public string[] ExpenseElements { get; set; }

        public string ServiceChargesQ1 { get; set; }
        public string ServiceChargesQ2 { get; set; }
        public string ServiceChargesQ3 { get; set; }
        public string ServiceChargesQ4 { get; set; }

        public string IncomeElement1 { get; set; }
        public string IncomeElement2 { get; set; }
        public string IncomeElement3 { get; set; }
        public string IncomeElement4 { get; set; }
        public string IncomeElement5 { get; set; }
        public string IncomeElement6 { get; set; }
        public string IncomeElement7 { get; set; }
        public string IncomeElement8 { get; set; }
        public string IncomeElement9 { get; set; }
        public string IncomeElement10 { get; set; }
        public string IncomeElement11 { get; set; }
        public string IncomeElement12 { get; set; }
        public string ExpenseElement1 { get; set; }
        public string ExpenseElement2 { get; set; }
        public string ExpenseElement3 { get; set; }
        public string ExpenseElement4 { get; set; }
        public string ExpenseElement5 { get; set; }
        public string ExpenseElement6 { get; set; }
        public string ExpenseElement7 { get; set; }
        public string ExpenseElement8 { get; set; }
        public string ExpenseElement9 { get; set; }
        public string ExpenseElement10 { get; set; }
        public string ExpenseElement11 { get; set; }
        public string ExpenseElement12 { get; set; }
        #endregion
    }
}





