using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
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
        public long ItemQuantity { get; set; }
        public long BalanceQuantity { get; set; }
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
    }

    public class RequisitionIssueItemsViewModel : NoteTemplateViewModel
    {       
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemSpecification { get; set; }
        public string BalanceQuantity { get; set; }
        public string RequisitionItemId { get; set; }
        public string RequisitionIssueId { get; set; }
        public string IssuedQuantity { get; set; }      
        public string CurrentIssueQuantity { get; set; }
        public string RequisitionId { get; set; }
        public string InventoryQuantity { get; set; }
        public string ItemQuantity { get; set; }
        public string ApprovedQuantity { get; set; }       
        public bool Select { get; set; }
        public decimal PurchaseRate { get; set; }
    }
}
