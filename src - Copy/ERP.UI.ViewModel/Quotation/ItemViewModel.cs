using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class ItemViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string ResellerPrice { get; set; }
        public string CustomerPrice { get; set; }
        public string Code { get; set; }
        public long? SequenceNo { get; set; }
        public long? ItemCategoryId { get; set; }
        public string ItemCategoryName { get; set; }

    }
    public class QuotationItemViewModel : ItemViewModel
    {
        public long? QuotationId { get; set; }
        public long? QuotationItemId { get; set; }
        public QuotationTypeEnum? QuotationType { get; set; }
        public QuotationItemTypeEnum? QuotationItemType { get; set; }
        public double? QuotationItemPrice { get; set; }

        public long? QuotationItemVersionNo { get; set; }
        public double? QuotationItemQuantity { get; set; }
        public double? QuotationItemReceivedQuantity { get; set; }
        public double? QuotationItemRejectedQuantity { get; set; }
        public string QuotationItemRejectionReason { get; set; }
        public double? QuotationItemTotal { get; set; }
        public double? QuotationItemQuantityDelivered { get; set; }
        public string QuotationItemNote { get; set; }
        public string ContactName { get; set; }
        public string ContactNameAr { get; set; }
        public CustomerTypeEnum? CustomerType { get; set; }
        public SupplierTypeEnum? SupplierType { get; set; }
        public string UserCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Discount { get; set; }
        public long? LocationId { get; set; }
        public string Location { get; set; }
        public DateTime? Date { get; set; }
        public string Website { get; set; }
        public string Note { get; set; }
        public string TemplateMasterCode { get; set; }
        public long UserId { get; set; }
        public long? CurrencyId { get; set; }
        public string Currency { get; set; }
        public bool HasAmountPermission { get; set; }
    }

}
