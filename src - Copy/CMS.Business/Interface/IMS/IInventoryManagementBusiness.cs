using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IInventoryManagementBusiness : IBusinessBase<ServiceViewModel, NtsService>
    {
        Task<IList<DirectSalesViewModel>> GetDirectSalesData(DirectSalesSearchViewModel model);
        Task<IList<ItemsViewModel>> GetDirectSaleItemsData(string directSalesId);
        Task<DirectSalesViewModel> GetDirectSalesData(string serviceId);
        Task<RequisitionViewModel> GetRequisitionData(string serviceId);
        Task<IList<RequisitionViewModel>> GetRequisitionDataByItemHead(string itemHead, string Customer, string status, string From, string To);
        Task<RequisitionViewModel> GetRequisitionDataByServiceId(string serviceId);
        Task<ItemsViewModel> GetItemsUnitDetailsByItemId(string itemId);
        Task<IList<ItemsViewModel>> GetItemsListData(ItemsSearchViewModel search);
        Task<IList<ItemsViewModel>> GetRequisistionItemsData(string requisitionId);
        Task<IList<IdNameViewModel>> GetItemCategoryByItemTypeId(string itemTypeId);
        Task<IList<IdNameViewModel>> GetItemSubCategoryByItemCategoryId(string itemCategoryId);
        Task<IList<RequisitionIssueItemsViewModel>> GetRequisistionItemsToIssue(string requisitionId);
        Task<IList<RequisitionIssueItemsViewModel>> GetRequisistionIssueItems(string issueServiceId);
        Task UpdateRequisitionServiceToIssued(string requisitionId);
        Task<IList<IssueRequisitionViewModel>> GetRequisistionIssue(string requisitionServiceId);
        Task<IList<RequisitionViewModel>> GetIssueRequisitionData(string itemHead, string From, string To);
        Task<ItemsViewModel> GetItemsDetails(string id);
        Task<CommandResult<InventoryItemViewModel>> InsertInventory(InventoryItemViewModel model);
        Task<IList<RequisitionIssueItemsViewModel>> GetRequisistionIssueItemsByRequisitionId(string requisitionServiceId);
    }
}
