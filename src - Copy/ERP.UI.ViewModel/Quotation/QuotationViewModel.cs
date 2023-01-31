using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class QuotationViewModel : ViewModelBase
    {
        public string Code { get; set; }
        public string QuotationNo { get; set; }
        public string QuotationName { get; set; }
        public string SystemName { get; set; }
        public long? SystemId { get; set; }
        public long? CustomerId { get; set; }
        public long? ContactId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNameAr { get; set; }
        public string CompanyCode { get; set; }
        public string ContactName { get; set; }
        public string ContactNameAr { get; set; }
        public string CustomerType { get; set; }
        public string UserCode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Discount { get; set; }
        public string Location { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateTimeFormatOnly)]
        public DateTime? QutoationDate { get; set; }
        public string Website { get; set; }
        public string Note { get; set; }
        public string PaymentTerms { get; set; }
        public double? Total { get; set; }
        public DiscountTypeEnum? DiscountType { get; set; }
        public double? DiscountValue { get; set; }
        public double? DiscountAmount { get; set; }
        public double? TotalAfterDiscount { get; set; }
        public double? InstallationAmount { get; set; }
        public double? AccessoriesAamount { get; set; }
        public double? AccessoriesAmount { get; set; }
        public double? AdditionalWarrantyAmount { get; set; }
        public string AddressingComment { get; set; }
        public string AddrssingTo { get; set; }

        public string Installation { get; set; }
        public string Accessories { get; set; }
        public string AdditionalWarranty { get; set; }
        public double? TotalBeforeVat { get; set; }
        public VatTypeEnum? VatType { get; set; }
        public double? VatValue { get; set; }
        public double? VatAmount { get; set; }
        public double? NetAmountAfterVat { get; set; }
        public NtsActionEnum? TemplateAction { get; set; }
      
        public long? OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }
        public string ProjectName { get; set; }
        public long? ProjectId { get; set; }
        public string QuotationTypeName { get; set; }
        public string ServiceStatusName { get; set; }
        public string ServiceStatusCode { get; set; }
        public string TemplateMasterCode { get; set; }
 
        public string WarrantyType { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public string StartingFormServiceNo { get; set; }
        public long? StartingFormServiceId { get; set; }
        public string StartingFormNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateTimeFormatOnly)]
        public DateTime? StartingFormDate { get; set; }
        public NtsActionEnum? StartingFormTemplateAction { get; set; }


        public long? InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateTimeFormatOnly)]
        public DateTime? InvoiceDate { get; set; }
        public NtsActionEnum? InvoiceTemplateAction { get; set; }

        public long? PaymentReceiptId { get; set; }
        public string PaymentReceiptNo { get; set; }
        public string DeliveryNoteNo { get; set; }
        public long? DeliveryNoteId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateTimeFormatOnly)]
        public DateTime? DeliveryNoteDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateTimeFormatOnly)]
        public DateTime? PaymentReceiptDate { get; set; }
        public NtsActionEnum? PaymentReceiptTemplateAction { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateTimeFormatOnly)]
        public DateTime? CommissioningDate { get; set; }
        public string TechnicianInCharge { get; set; }
        public string CustomerDeliveryNoteNo { get; set; }
        public long? CustomerDeliveryNoteId { get; set; }
        public string StatusClass
        {
            get
            {
                switch (TemplateAction)
                {
                    case NtsActionEnum.Draft:
                        return "label-info";
                    case NtsActionEnum.Submit:
                        return "label-warning";
                    case NtsActionEnum.Complete:
                        return "label-success";
                    case NtsActionEnum.Cancel:
                        return "label-default";
                    case NtsActionEnum.Overdue:
                        return "label-danger";
                    default:
                        return "label-default";
                }
            }
        }

        public string Description { get; set; }
        public long? ServiceFormDocumentId { get; set; }
        public long? ServiceFormWarrantyDocumentId { get; set; }
        public long? ServiceFormFeedbackDocumentId { get; set; }
        public string Designation { get; set; }
        public string VatNumber { get; set; }
        public string AmountInWords { get; set; }
        public long? ServiceVersionId { get; set; }
        public string VersionNo { get; set; }
        public string POBoxNo { get; set; }
        public string LocationName { get; set; }
        public string CancelReason { get; set; }
        public double? AmountReceived { get; set; }
        public double? InvoiceAmountReceived { get; set; }
        public double? InvoiceAmount { get; set; }
        public double? BalanceAmount { get; set; }
        public double? QuotationNetAmountAfterVat { get; set; }
        public double? InvoiceBalanceAmount { get
            {
                return NetAmountAfterVat - AmountReceived;
            }
        }
        public QuotationPaymentModeEnum? PaymentMode { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string PaymentAgainst { get; set; }
        public long? Bank { get; set; }
        public string BankName { get; set; }
        public string QuotationCode { get; set; }
        public bool HasAmountPermission { get; set; }
        public double? AdvancePaymentAmount { get; set; }
        public bool IsCustomerApprovalPO { get; set; }
        public bool IsEstimateMatchingApproval { get; set; }
        public bool IsPaymentCollection { get; set; }
        public bool IsCopyOfApprovedQuote { get; set; }
        public bool IsDrawing { get; set; }
        public bool IsManpowerForm { get; set; }
        public bool IsApprovedWorkContract { get; set; }
        public bool IsLocationMap { get; set; }
        public bool IsOthers { get; set; }
        public bool IsBOQRFQ { get; set; }
        public bool IsScheduleForm { get; set; }
        public bool IsSubContractorAssignment { get; set; }
        public long? StoreDeliveryNoteId { get; set; }
        public string StoreDeliveryNoteNo { get; set; }
        public long? AdditionalWorkReferenceId { get; set; }
        public long? MaintenanceContractReferenceId { get; set; }
        public string AdditionalWorkNo { get; set; }
        public string MaintenanceContractNo { get; set; }


    }

}
