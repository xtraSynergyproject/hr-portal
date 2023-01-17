using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class InventoryViewModel : ViewModelBase
    {
        public string Code { get; set; }
        public string InventoryNo { get; set; }
        public string InventoryName { get; set; }
        public long? SupplierId { get; set; }
        public long? ContactId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierNameAr { get; set; }      
        public string ContactName { get; set; }
        public string ContactNameAr { get; set; }
        //public string CustomerType { get; set; }
        public string UserCode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Discount { get; set; }
        public string Location { get; set; }
        public DateTime? InventoryDate { get; set; }
        public string Website { get; set; }
        public string Note { get; set; }

        public double? Total { get; set; }
        public DiscountTypeEnum? DiscountType { get; set; }
        public double? DiscountValue { get; set; }
        public double? DiscountAmount { get; set; }
        public double? TotalAfterDiscount { get; set; }
        public double? InstallationAmount { get; set; }
        public double? AccessoriesAamount { get; set; }
        public double? AdditionalWarrantyAmount { get; set; }

        public string Installation { get; set; }
        public string Accessories { get; set; }
        public double? AdditionalWarranty { get; set; }
        public double? TotalBeforeNet { get; set; }
        public VatTypeEnum? VatType { get; set; }
        public double? VatValue { get; set; }
        public double? VatAmount { get; set; }
        public double? NetAmountAfterVat { get; set; }
        public NtsActionEnum? TemplateAction { get; set; }
      
        public long? OwnerUserId { get; set; }
      
        public string ProjectName { get; set; }
        public long? ProjectId { get; set; }
        public string InventoryTypeName { get; set; }
        public string ServiceStatusName { get; set; }
        public string ServiceStatusCode { get; set; }
        public string TemplateMasterCode { get; set; }
 
        public string WarrantyType { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public string GrnServiceNo { get; set; }
        public long? GrnServiceId { get; set; }
        public string GrnNo { get; set; }
        public DateTime? GrnDate { get; set; }
        public NtsActionEnum? GrnTemplateAction { get; set; }
        public string GrnServiceStatusName { get; set; }

        public string VersionNo { get; set; }




        public DateTime? CommissioningDate { get; set; }
        public string TechnicianInCharge { get; set; }
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
    }

}
