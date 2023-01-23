using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class ItemsViewModel:NoteTemplateViewModel
    {
        public bool Issued { get; set; }
        public string IssueType { get; set; }
        public string Remarks { get; set; }
        public string IssueTo { get; set; }
        public string Department { get; set; }
        public string Employee { get; set; }
        public DateTime? IssuedOn { get; set; }
        public string ItemId { get; set; }
        public string ItemHead { get; set; }       
        public string ItemCategory { get; set; }
        public string ItemCategoryName { get; set; }
        public string ItemSubCategory { get; set; }
        public string ItemSubCategoryName { get; set; }
        public string Item { get; set; }
        public string ItemUnitName { get; set; }
        public string CurrentBalance { get; set; }
        public string RequisitionId { get; set; }
        public decimal Amount { get; set; }
        public string DirectSalesId { get; set; }
        public decimal SaleRate { get; set; }
        public decimal PurchaseRate { get; set; }
        public string ItemDescription { get; set; }
        public string ItemSpecification { get; set; }
        public decimal ItemQuantity { get; set; }
        public decimal BalanceQuantity { get; set; }
        public long IssuedQuantity { get; set; }
        public long CurrentQuantity { get; set; }
        public long ApprovedQuantity { get; set; }
        public string ItemName { get; set; }
        public string ProposalTypeName { get; set; }
        public string ProposalNo { get; set; }
        public string ItemStatus { get; set; }
        public bool Select { get; set; }
        public string ItemPrefix { get; set; }
        public string ItemUnit { get; set; }
        public string InventoryQuantity { get; set; }
        public string MinStockQuantity { get; set; }
        public string MaxStockQuantity { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemType { get; set; }
        public bool IsOpenItem { get; set; }
        public bool UpdateMappingCode { get; set; }
        public string ItemCodeMappingId { get; set; }
        public string TaxDescription { get; set; }
        public string ValidForNo { get; set; }
        public string ValidForId { get; set; }
        public BoolStatus IsSerializable { get; set; }
        public string ServiceStatusCode { get; set; }
        public decimal POQuantity { get; set; }
        public bool IsApproved { get; set; }
        public string RequisitionItemsData { get; set; }
        public string ItemStatusCode { get; set; }
        public double ReturnQuantity { get; set; }
        public string ReturnReason { get; set; }
        public string ReturnTypeId { get; set; }
        public string NtsNoteId { get; set; }
        public bool CheckFlag { get; set; }
        public string DeliveredQuantity { get; set; }
        public string ClosingQuantity { get; set; }

    }

    public class RequisitionIssueItemsViewModel : NoteTemplateViewModel
    {       
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string AdditionalInfo { get; set; }
        public string BalanceQuantity { get; set; }
        public string ReferenceHeaderItemId { get; set; }
        public string RequisitionIssueId { get; set; }
        public string AlreadyIssuedQuantity { get; set; }      
        public string IssuedQuantity { get; set; }
        public string TransactionQuantity { get; set; }
        public string ReferenceHeaderId { get; set; }
        public string WarehouseId { get; set; }
        public bool Select { get; set; }
        //public string ReferenceHeaderId { get; set; }
        public BoolStatus IsSerializable { get; set; }
        public int SNo { get; set; }
        public string ChallanNo { get; set; }
        public string IssueToTypeName { get; set; }
        public string IssueTypeName { get; set; }
    }

    public class POItemsViewModel : NoteTemplateViewModel
    {
        public bool IsTaxable { get; set; }
        public string RequisitionItemId { get; set; }
        public decimal POQuantity { get; set; }
        public decimal ExistingPOQuantity { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal PurchaseRate { get; set; }      
        public decimal ItemPurchaseRate { get; set; }      
        public decimal ItemQuantity { get; set; }
        public decimal SalesRate { get; set; }
        public decimal ApprovedQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string ItemName { get; set; }
        public string ReqCode { get; set; }
        public string POId { get; set; }
        public bool AllowMoreThanApprovedQuantity { get; set; }
        public string RequisitionIds { get; set; }
        public string POItemsData { get; set; }
        public string ServiceStatusCode { get; set; }
        public string HSNSACCode { get; set; }
        public string ItemUOM { get; set; }
        public int SNo { get; set; }
        public string DeliveredQuantity { get; set; }

    }
}
