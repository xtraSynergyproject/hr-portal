using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Synergy.App.ViewModel.IMS;
using Hangfire;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Synergy.App.Business
{
    public class InventoryManagementBusiness : BusinessBase<ServiceViewModel, NtsService>, IInventoryManagementBusiness
    {
        private readonly IRepositoryQueryBase<DirectSalesViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<ProgramDashboardViewModel> _queryPDRepo;
        private readonly IRepositoryQueryBase<ProjectGanttTaskViewModel> _queryGantt;
        private readonly IRepositoryQueryBase<TeamWorkloadViewModel> _queryTWRepo;
        private readonly IRepositoryQueryBase<DashboardCalendarViewModel> _queryDCRepo;
        private readonly IRepositoryQueryBase<PerformanceDashboardViewModel> _queryProjDashRepo;
        private readonly IRepositoryQueryBase<ProjectDashboardChartViewModel> _queryProjDashChartRepo;
        private readonly IRepositoryQueryBase<TaskViewModel> _queryTaskRepo;
        private readonly IRepositoryQueryBase<MailViewModel> _queryMailTaskRepo;
        private readonly IRepositoryQueryBase<PerformanceDocumentViewModel> _queryPerDoc;
        private readonly IRepositoryQueryBase<PerformanceDocumentStageViewModel> _queryPerDocStage;
        private readonly IRepositoryQueryBase<GoalViewModel> _queryGoal;
        private readonly IRepositoryQueryBase<NoteTemplateViewModel> _queryNoteTemplate;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IUserContext _userContext;
        private readonly INtsTaskPrecedenceBusiness _ntsTaskPrecedenceBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IComponentResultBusiness _componentResultBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IStepTaskComponentBusiness _stepCompBusiness;
        private readonly IRepositoryQueryBase<PerformaceRatingViewModel> _queryPerformanceRatingRepo;
        private readonly IRepositoryQueryBase<PerformanceRatingItemViewModel> _queryPerformanceRatingitemRepo;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IRepositoryQueryBase<CompetencyViewModel> _queryComp;
        private readonly IInventoryManagementQueryBusiness _inventoryManagementQueryBusiness;

        private readonly IRepositoryQueryBase<CompetencyCategoryViewModel> _queryCompeencyCategory;
        public InventoryManagementBusiness(IRepositoryBase<ServiceViewModel, NtsService> repo, IRepositoryQueryBase<DirectSalesViewModel> queryRepo,
            IRepositoryQueryBase<ProgramDashboardViewModel> queryPDRepo,
            IRepositoryQueryBase<IdNameViewModel> queryRepo1,
            IRepositoryQueryBase<DashboardCalendarViewModel> queryDCRepo,
            IRepositoryQueryBase<ProjectGanttTaskViewModel> queryGantt,
             IRepositoryQueryBase<TeamWorkloadViewModel> queryTWRepo,
             IRepositoryQueryBase<PerformanceDashboardViewModel> queryProjDashRepo,
             IRepositoryQueryBase<ProjectDashboardChartViewModel> queryProjDashChartRepo,
             IInventoryManagementQueryBusiness inventoryManagementQueryBusiness,
             IRepositoryQueryBase<TaskViewModel> queryTaskRepo, INtsTaskPrecedenceBusiness ntsTaskPrecedenceBusiness, ITableMetadataBusiness tableMetadataBusiness,
            IMapper autoMapper, ITaskBusiness taskBusiness, INoteBusiness noteBusiness, IServiceBusiness serviceBusiness, IRepositoryQueryBase<MailViewModel> queryMailTaskRepo,
            IRepositoryQueryBase<PerformanceDocumentViewModel> queryPerDoc, IRepositoryQueryBase<GoalViewModel> queryGoal, IRepositoryQueryBase<PerformanceDocumentStageViewModel> queryPerDocStage
            , IHRCoreBusiness hrCoreBusiness, IUserContext userContext, IRepositoryQueryBase<NoteTemplateViewModel> queryNoteTemplate, IComponentResultBusiness componentResultBusiness, ILOVBusiness lovBusiness
            , ITemplateBusiness templateBusiness, IStepTaskComponentBusiness stepCompBusiness, IRepositoryQueryBase<PerformaceRatingViewModel> queryPerformaceRating,
            IRepositoryQueryBase<PerformanceRatingItemViewModel> queryPerformaceRatingitem, IRepositoryQueryBase<CompetencyViewModel> queryComp, ICmsBusiness cmsBusiness, IRepositoryQueryBase<CompetencyCategoryViewModel> queryComptetencyCategory) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _queryRepo1 = queryRepo1;
            _queryPDRepo = queryPDRepo;
            _queryDCRepo = queryDCRepo;
            _queryGantt = queryGantt;
            _queryTWRepo = queryTWRepo;
            _queryProjDashRepo = queryProjDashRepo;
            _queryProjDashChartRepo = queryProjDashChartRepo;
            _queryTaskRepo = queryTaskRepo;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
            _noteBusiness = noteBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _ntsTaskPrecedenceBusiness = ntsTaskPrecedenceBusiness;
            _queryMailTaskRepo = queryMailTaskRepo;
            _queryPerDoc = queryPerDoc;
            _queryPerDocStage = queryPerDocStage;
            _hrCoreBusiness = hrCoreBusiness;
            _componentResultBusiness = componentResultBusiness;
            _queryGoal = queryGoal;
            _userContext = userContext;
            _queryNoteTemplate = queryNoteTemplate;
            _lovBusiness = lovBusiness;
            _templateBusiness = templateBusiness;
            _stepCompBusiness = stepCompBusiness;
            _queryComp = queryComp;
            _queryPerformanceRatingRepo = queryPerformaceRating;
            _queryPerformanceRatingitemRepo = queryPerformaceRatingitem;
            _cmsBusiness = cmsBusiness;
            _queryCompeencyCategory = queryComptetencyCategory;
            _inventoryManagementQueryBusiness = inventoryManagementQueryBusiness;


        }
        public async override Task<CommandResult<ServiceViewModel>> Create(ServiceViewModel model, bool autoCommit = true)
        {

            return CommandResult<ServiceViewModel>.Instance();
        }
        public async override Task<CommandResult<ServiceViewModel>> Edit(ServiceViewModel model, bool autoCommit = true)
        {
            return CommandResult<ServiceViewModel>.Instance();
        }

        public async Task<DirectSalesViewModel> GetDirectSalesData(string serviceId)
        {

            return await _inventoryManagementQueryBusiness.GetDirectSalesData(serviceId);
        }
        public async Task<RequisitionViewModel> GetRequisitionData(string serviceId)
        {

            return await _inventoryManagementQueryBusiness.GetRequisitionData(serviceId);
        }
        public async Task<IList<DirectSalesViewModel>> GetDirectSalesData(DirectSalesSearchViewModel search)
        {

            return await _inventoryManagementQueryBusiness.GetDirectSalesData(search);
        }
        public async Task<IList<ItemsViewModel>> GetDirectSaleItemsData(string directSalesId)
        {

            return await _inventoryManagementQueryBusiness.GetDirectSaleItemsData(directSalesId);
        }

        public async Task<IList<ItemsViewModel>> GetItemsListData(ItemsSearchViewModel search)
        {
            return await _inventoryManagementQueryBusiness.GetItemsListData(search);
        }
        public async Task<ItemsViewModel> GetItemsDetails(string id)
        {

            var result = await _inventoryManagementQueryBusiness.GetItemsDetails(id);
            return result;
        }
        public async Task<IList<RequisitionViewModel>> GetRequisitionDataByItemHead(string itemHead, string Customer, string status, string From, string To, string requisitionIds = null)
        {

            return await _inventoryManagementQueryBusiness.GetRequisitionDataByItemHead(itemHead, Customer, status, From, To, requisitionIds);
        }
        public async Task<IList<RequisitionViewModel>> GetIssueRequisitionData(string itemHead, string From, string To)
        {
            return await _inventoryManagementQueryBusiness.GetIssueRequisitionData(itemHead, From, To);
        }

        public async Task<RequisitionViewModel> GetRequisitionDataByServiceId(string serviceId)
        {
            return await _inventoryManagementQueryBusiness.GetRequisitionDataByServiceId(serviceId);
        }
        public async Task<RequisitionViewModel> GetRequisitionDataById(string Id)
        {
            return await _inventoryManagementQueryBusiness.GetRequisitionDataById(Id);
        }

        public async Task<ItemsViewModel> GetItemsUnitDetailsByItemId(string itemId)
        {
            return await _inventoryManagementQueryBusiness.GetItemsUnitDetailsByItemId(itemId);
        }

        public async Task<IList<DirectSalesViewModel>> FilterDirectSalesData(DirectSalesSearchViewModel search)
        {

            return await _inventoryManagementQueryBusiness.FilterDirectSalesData(search);
        }

        public async Task<IList<IdNameViewModel>> GetItemCategoryByItemTypeId(string itemTypeId)
        {
            var result = await _inventoryManagementQueryBusiness.GetItemCategoryByItemTypeId(itemTypeId);
            return result;
        }
        public async Task<IList<IdNameViewModel>> GetItemSubCategoryByItemCategoryId(string itemCategoryId)
        {
            var result = await _inventoryManagementQueryBusiness.GetItemSubCategoryByItemCategoryId(itemCategoryId);
            return result;
        }
        public async Task<IList<IdNameViewModel>> GetItemByItemSubCategoryId(string itemSubCategoryId)
        {
            var result = await _inventoryManagementQueryBusiness.GetItemByItemSubCategoryId(itemSubCategoryId);
            return result;
        }
        public async Task<IList<ItemsViewModel>> GetRequisistionItemsData(string requisitionId)
        {
            return await _inventoryManagementQueryBusiness.GetRequisistionItemsData(requisitionId);
        }

        public async Task<IList<RequisitionIssueItemsViewModel>> GetRequisistionItemsToIssue(string requisitionId)
        {

            return await _inventoryManagementQueryBusiness.GetRequisistionItemsToIssue(requisitionId);
        }

        public async Task<IList<RequisitionIssueItemsViewModel>> GetRequisistionIssueItems(string issueServiceId, ImsIssueTypeEnum issueType)
        {
            return await _inventoryManagementQueryBusiness.GetRequisistionIssueItems(issueServiceId, issueType);
        }
        public async Task UpdateStockClosingBalance(string itemId, string warehouseId, double closingBalance)
        {
            await _inventoryManagementQueryBusiness.UpdateStockClosingBalance(itemId, warehouseId, closingBalance);
        }

        public async Task<IList<RequisitionIssueItemsViewModel>> GetRequisistionIssueItemsByRequisitionId(string requisitionId)
        {
            return await _inventoryManagementQueryBusiness.GetRequisistionIssueItemsByRequisitionId(requisitionId);
        }
        public async Task<IList<RequisitionIssueItemsViewModel>> GetRequisistionIssueItemsToDeliver(string requisitionId)
        {
            return await _inventoryManagementQueryBusiness.GetRequisistionIssueItemsToDeliver(requisitionId);
        }
        public async Task UpdateRequisitionServiceToIssued(string requisitionId)
        {

            var noteTempModel = new ServiceTemplateViewModel();
            //noteTempModel.TemplateCode = note.TemplateCode;
            noteTempModel.ServiceId = requisitionId;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _serviceBusiness.GetServiceDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            if (notemodel.IsNotNull())
            {
                var noteTempModel1 = new NoteTemplateViewModel();
                noteTempModel1.SetUdfValue = true;
                noteTempModel1.NoteId = notemodel.UdfNoteId;
                var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                if (rowData.ContainsKey("Issued"))
                {
                    rowData["Issued"] = "True";
                }
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);
            }
        }
        public async Task<IList<IssueRequisitionViewModel>> GetRequisistionIssue(string requisitionId, ImsIssueTypeEnum issuetype)
        {

            return await _inventoryManagementQueryBusiness.GetRequisistionIssue(requisitionId, issuetype);
        }
        public async Task<CommandResult<InventoryItemViewModel>> InsertInventory(InventoryItemViewModel model)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Create;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "IMS_ITEM_INVENTORY";

            var note = await _noteBusiness.GetNoteDetails(noteTempModel);

            note.StartDate = DateTime.Now;

            model.InventoryQuantity = (model.InventoryAction == AddDeductEnum.Add) ? model.ItemQuantity : (model.ItemQuantity - 1);

            note.Json = JsonConvert.SerializeObject(model);
            note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            var result = await _noteBusiness.ManageNote(note);

            return CommandResult<InventoryItemViewModel>.Instance(model);
        }
        public async Task<IList<DeliveryNoteViewModel>> GetDeliveryNoteData(string itemHead, string From, string To)
        {
            return await _inventoryManagementQueryBusiness.GetDeliveryNoteData(itemHead, From, To);
        }
        public async Task<DeliveryNoteReportViewModel> GetDeliveryNoteReportData(string deliveryNoteId)
        {
            return await _inventoryManagementQueryBusiness.GetDeliveryNoteReportData(deliveryNoteId);
        }
        public async Task<List<DeliveryItemViewModel>> GetDeliveryItemsList(string deliveryNoteId)
        {
            return await _inventoryManagementQueryBusiness.GetDeliveryItemsList(deliveryNoteId);
        }
        public async Task<PurchaseOrderReportViewModel> GetPurchaseOrderReportData(string purchaseOrderId)
        {
            return await _inventoryManagementQueryBusiness.GetPurchaseOrderReportData(purchaseOrderId);
        }
        public async Task<ReceivedNoteReportViewModel> GetReceivedNoteReportData(string receivedNoteId)
        {
            return await _inventoryManagementQueryBusiness.GetReceivedNoteReportData(receivedNoteId);
        }
        public async Task<PurchaseInvoiceReportViewModel> GetPurchaseInvoiceReportData(string purchaseInvoiceId)
        {
            return await _inventoryManagementQueryBusiness.GetPurchaseInvoiceReportData(purchaseInvoiceId);
        }
        public async Task<List<InvoiceItemViewModel>> GetPurchaseInvoiceItemsList(string purchaseInvoiceId)
        {
            return await _inventoryManagementQueryBusiness.GetPurchaseInvoiceItemsList(purchaseInvoiceId);
        }
        public async Task<List<GoodsReceiptItemViewModel>> GetGoodReceiptItemsList(string receiptId)
        {
            return await _inventoryManagementQueryBusiness.GetGoodReceiptItemsList(receiptId);
        }
        public async Task<List<IdNameViewModel>> ReadInvertoryFinancialYearIdNameList()
        {
            return await _inventoryManagementQueryBusiness.GetInvertoryFinancialYearIdNameList();
        }
        public async Task<List<PurchaseInvoiceReportViewModel>> ReadInvoiceDetailsData(DateTime fromDate, DateTime toDate)
        {
            return await _inventoryManagementQueryBusiness.GetReportInvoiceDetailsData(fromDate, toDate);
        }
        public async Task<List<RequisitionIssueItemsViewModel>> ReadIssueTypeData(DateTime fromDate, DateTime toDate, string issueTypeId, string departmentId, string employeeId, string issueToTypeId)
        {
            return await _inventoryManagementQueryBusiness.GetReportIssueTypeData(fromDate, toDate, issueTypeId, departmentId, employeeId, issueToTypeId);
        }
        public async Task<List<ItemStockViewModel>> ReadItemHistoryData(DateTime fromDate, DateTime toDate, string warehouseId, string itemTypeId, string itemCategoryId, string itemSubCategoryId, string itemId)
        {
            return await _inventoryManagementQueryBusiness.GetReportItemHistoryData(fromDate, toDate, warehouseId, itemTypeId, itemCategoryId, itemSubCategoryId, itemId);
        }
        public async Task<List<StockTransferViewModel>> ReadItemTransferData(DateTime fromDate, DateTime toDate, string warehouseId, string itemTypeId, string itemCategoryId, string itemSubCategoryId, string itemId)
        {
            return await _inventoryManagementQueryBusiness.GetReportItemTransferData(fromDate, toDate, warehouseId, itemTypeId, itemCategoryId, itemSubCategoryId, itemId);
        }
        public async Task<List<PurchaseOrderViewModel>> ReadPurchaseOrderStatusData(DateTime fromDate, DateTime toDate, string statusId)
        {
            return await _inventoryManagementQueryBusiness.GetReportPurchaseOrderStatusData(fromDate, toDate, statusId);
        }
        public async Task<List<PurchaseOrderViewModel>> ReadOrderBookData()
        {
            return await _inventoryManagementQueryBusiness.GetReportOrderBookData();
        }
        public async Task<List<OrderSummaryViewModel>> ReadOrderStatusData(string financialYearId)
        {
            return await _inventoryManagementQueryBusiness.GetReportOrderStatusData(financialYearId);
        }
        public async Task<List<RequisitionViewModel>> ReadRequisitionByStatusData(DateTime fromDate, DateTime toDate, string typeId, string customerId, string statusId)
        {
            return await _inventoryManagementQueryBusiness.GetReportRequisitionByStatusData(fromDate, toDate, typeId, customerId, statusId);
        }
        public async Task<List<RequisitionViewModel>> ReadRequisitionByDetailsData(DateTime fromDate, DateTime toDate, string typeId, string customerId, string statusId)
        {
            return await _inventoryManagementQueryBusiness.GetReportRequisitionByDetailsData(fromDate, toDate, typeId, customerId, statusId);
        }
        public async Task<List<GoodsReceiptViewModel>> ReadReceivedFromPOData(DateTime fromDate, DateTime toDate, string vendorId)
        {
            return await _inventoryManagementQueryBusiness.GetReportReceivedFromPOData(fromDate, toDate, vendorId);
        }
        public async Task<List<VendorCategoryViewModel>> ReadVendorCategoryData(string vendorId, string categoryId, string subCategoryId)
        {
            return await _inventoryManagementQueryBusiness.GetReportVendorCategoryData(vendorId, categoryId, subCategoryId);
        }
        public async Task<List<PurchaseReturnViewModel>> ReadReturnToVendorData(DateTime fromDate, DateTime toDate, string vendorId)
        {
            return await _inventoryManagementQueryBusiness.GetReportReturnToVendorData(fromDate, toDate, vendorId);
        }
        public async Task<IList<VendorViewModel>> GetVendorList(string countryId, string stateId, string cityId, string name)
        {
            return await _inventoryManagementQueryBusiness.GetVendorList(countryId, stateId, cityId, name);
        }
        public async Task<VendorViewModel> GetVendorDetailsById(string vendorId)
        {
            return await _inventoryManagementQueryBusiness.GetVendorDetailsById(vendorId);
        }
        public async Task<ItemShelfViewModel> GetItemShelfDetailsById(string itemShelfId)
        {
            return await _inventoryManagementQueryBusiness.GetItemShelfDetailsById(itemShelfId);
        }

        public async Task<List<ContactsViewModel>> ReadVendorContactsData(string vendorId)
        {
            return await _inventoryManagementQueryBusiness.ReadVendorContactsData(vendorId);
        }
        public async Task<List<VendorCategoryViewModel>> ReadVendorCategoryData(string vendorId)
        {
            return await _inventoryManagementQueryBusiness.ReadVendorCategoryData(vendorId);
        }
        public async Task<List<ItemShelfViewModel>> ReadItemShelfCategoryData(string itemShelfId)
        {
            return await _inventoryManagementQueryBusiness.ReadItemShelfCategoryData(itemShelfId);
        }
        public async Task<List<VendorCategoryViewModel>> ReadCategoryNotInVendorCategoryData(string vendorId)
        {
            return await _inventoryManagementQueryBusiness.ReadCategoryNotInVendorCategoryData(vendorId);
        }
        public async Task<List<ItemShelfViewModel>> ReadCategoryNotInItemShelfCategoryData(string itemShelfId)
        {
            return await _inventoryManagementQueryBusiness.ReadCategoryNotInItemShelfCategoryData(itemShelfId);
        }
        public async Task DeleteContacts(string id)
        {
            await _inventoryManagementQueryBusiness.DeleteContacts(id);
        }
        public async Task<List<ItemShelfViewModel>> ReadShelfList()
        {
            return await _inventoryManagementQueryBusiness.ReadShelfList();
        }
        public async Task<ItemShelfViewModel> GetItemShelfDetail(string noteId)
        {
            return await _inventoryManagementQueryBusiness.GetItemShelfDetail(noteId);
        }
        public async Task DeleteVendorCategories(string id)
        {
            await _inventoryManagementQueryBusiness.DeleteVendorCategories(id);
        }
        public async Task DeleteItemShelfCategories(string id)
        {
            await _inventoryManagementQueryBusiness.DeleteItemShelfCategories(id);
        }
        public async Task<List<IdNameViewModel>> GetItemCodeMappingList()
        {
            return await _inventoryManagementQueryBusiness.GetItemCodeMappingList();
        }
        public async Task<List<ItemStockViewModel>> ReadItemStockData(string itemTypeId, string itemCategory, string itemSubCategory, string warehouseId)
        {
            return await _inventoryManagementQueryBusiness.ReadItemStockData(itemTypeId, itemCategory, itemSubCategory, warehouseId);
        }
        public async Task<List<ItemStockViewModel>> ReadItemListByStock(string itemTypeId, string itemCategory, string itemSubCategory, string warehouseId)
        {
            return await _inventoryManagementQueryBusiness.ReadItemListByStock(itemTypeId, itemCategory, itemSubCategory, warehouseId);
        }
        public async Task<List<ItemStockViewModel>> ReadItemCurrentStockData(string warehouseId, string itemTypeId, string itemCategoryId, string itemSubCategoryId, string itemId)
        {
            return await _inventoryManagementQueryBusiness.ReadItemCurrentStockData(warehouseId, itemTypeId, itemCategoryId, itemSubCategoryId, itemId);
        }
        public async Task<ItemStockViewModel> GetUnitItemData(string Id)
        {
            return await _inventoryManagementQueryBusiness.GetUnitItemData(Id);
        }

        public async Task<bool> CheckItemStockExists(string itemId, string warehouseId)
        {
            var data = await _inventoryManagementQueryBusiness.CheckItemStockExists(itemId, warehouseId);
            if (data.IsNotNull())
            {
                return true;
            }
            return false;
        }

        public async Task<ItemStockViewModel> GetItemHeaderData(string Id, string warehouseId)
        {
            return await _inventoryManagementQueryBusiness.GetItemHeaderData(Id, warehouseId);
        }
        public async Task<IList<CustomerViewModel>> GetCustomerList(string countryId, string stateId, string cityId, string name)
        {
            return await _inventoryManagementQueryBusiness.GetCustomerList(countryId, stateId, cityId, name);
        }
        public async Task<CustomerViewModel> GetCustomerDetailsById(string customerId)
        {
            return await _inventoryManagementQueryBusiness.GetCustomerDetailsById(customerId);
        }

        public async Task<List<ContactsViewModel>> ReadCustomerContactsData(string customerId)
        {
            return await _inventoryManagementQueryBusiness.ReadCustomerContactsData(customerId);
        }
        public async Task<List<POItemsViewModel>> ReadPOItemsData(string poId)
        {
            return await _inventoryManagementQueryBusiness.ReadPOItemsData(poId);
        }
        public async Task DeleteCustomerContacts(string id)
        {
            await _inventoryManagementQueryBusiness.DeleteCustomerContacts(id);
        }
        public async Task DeletePOItem(string poId)
        {
            await _inventoryManagementQueryBusiness.DeletePOItem(poId);
        }

        public async Task<List<POItemsViewModel>> GetRequisistionItemsByRequisitionId(string requisitionIds, bool isValidate = false)
        {
            return await _inventoryManagementQueryBusiness.GetRequisistionItemsByRequisitionId(requisitionIds, isValidate);
        }
        public async Task<IList<RequisitionIssueItemsViewModel>> GetRequisitionIssueItemsByIssueRefId(string issueRefId)
        {
            return await _inventoryManagementQueryBusiness.GetRequisitionIssueItemsByIssueRefId(issueRefId);
        }

        public async Task GeneratePOItems(string itemsData, string POId)
        {
            var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<POItemsViewModel>>(itemsData);
            if (items.IsNotNull() && items.Count() > 0)
            {
                foreach (var data in items)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_PO_ITEM";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    dynamic exo = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo).Add("POId", POId);
                    ((IDictionary<String, Object>)exo).Add("ItemQuantity", data.POQuantity);
                    ((IDictionary<String, Object>)exo).Add("PurcahseRate", data.PurchaseRate);
                    ((IDictionary<String, Object>)exo).Add("RequisitionItemId", data.RequisitionItemId);
                    ((IDictionary<String, Object>)exo).Add("IsTaxable", data.IsTaxable);
                    ((IDictionary<String, Object>)exo).Add("SalesRate", data.SalesRate);
                    ((IDictionary<String, Object>)exo).Add("TotalAmount", Convert.ToString(data.POQuantity * data.PurchaseRate));
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _noteBusiness.ManageNote(notemodel);
                }
            }
        }

        public async Task GeneratePOItemsMobile(List<POItemsViewModel> items, string POId)
        {
            //var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<POItemsViewModel>>(itemsData);
            if (items.IsNotNull() && items.Count() > 0)
            {
                foreach (var data in items)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_PO_ITEM";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    dynamic exo = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo).Add("POId", POId);
                    ((IDictionary<String, Object>)exo).Add("ItemQuantity", data.POQuantity);
                    ((IDictionary<String, Object>)exo).Add("PurcahseRate", data.PurchaseRate);
                    ((IDictionary<String, Object>)exo).Add("RequisitionItemId", data.RequisitionItemId);
                    ((IDictionary<String, Object>)exo).Add("IsTaxable", data.IsTaxable);
                    ((IDictionary<String, Object>)exo).Add("SalesRate", data.SalesRate);
                    ((IDictionary<String, Object>)exo).Add("TotalAmount", Convert.ToString(data.POQuantity * data.PurchaseRate));
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _noteBusiness.ManageNote(notemodel);
                }
            }
        }


        public async Task GenerateInvoiceScheduleItems(string itemsData, string ISId)
        {
            var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ScheduleInvoiceViewModel>>(itemsData);
            if (items.IsNotNull() && items.Count() > 0)
            {
                foreach (var data in items)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_INVOICE_SCHEDULE_ITEM";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    dynamic exo = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo).Add("InvoiceScheduleId", ISId);
                    ((IDictionary<String, Object>)exo).Add("ItemId", data.ItemId);
                    ((IDictionary<String, Object>)exo).Add("RequisitionItemId", data.RequisitionItemId);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _noteBusiness.ManageNote(notemodel);
                }
            }
        }

        public async Task GenerateGoodsReceiptItems(string itemsData, string goodReceiptId, string warehouseId, string GoodsReceiptReferenceId, DateTime ReceiveDate)
        {
            var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GoodsReceiptItemViewModel>>(itemsData);
            if (items.IsNotNull() && items.Count() > 0)
            {
                foreach (var data in items)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_GOODS_RECEIPT_ITEM";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    dynamic exo = new System.Dynamic.ExpandoObject();
                    var goodreceipt = await _serviceBusiness.GetSingle(x => x.UdfNoteTableId == goodReceiptId);
                    var po = await _serviceBusiness.GetSingle(x => x.UdfNoteTableId == GoodsReceiptReferenceId);
                    ((IDictionary<String, Object>)exo).Add("GoodReceiptId", goodReceiptId);
                    ((IDictionary<String, Object>)exo).Add("ItemQuantity", data.ItemQuantity);
                    ((IDictionary<String, Object>)exo).Add("BalanceQuantity", data.ItemQuantity);
                    ((IDictionary<String, Object>)exo).Add("ItemId", data.ItemId);
                    ((IDictionary<String, Object>)exo).Add("WarehouseId", warehouseId);
                    ((IDictionary<String, Object>)exo).Add("ReferenceHeaderItemId", data.ReferenceHeaderItemId);
                    var user = await _noteBusiness.GetSingleById<UserViewModel, User>(_userContext.UserId);
                    ((IDictionary<String, Object>)exo).Add("AdditionalInfo", "<" + goodreceipt.ServiceNo + "> received against purchase order: <" + po.ServiceNo + "> By <" + user.Name + "> on <" + ReceiveDate + ">");


                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _noteBusiness.ManageNote(notemodel);
                }
            }
        }

        public async Task<List<PurchaseOrderViewModel>> GetVendorPOList(string itemHead = null, string vendorId = null, string statusId = null, string From = null, string To = null)
        {
            return await _inventoryManagementQueryBusiness.GetVendorPOList(itemHead, vendorId, statusId, From, To);
        }

        public async Task<double> GetPOValueByPOId(string POId)
        {
            return await _inventoryManagementQueryBusiness.GetPOValueByPOId(POId);
        }
        public async Task UpdateDeliveryNoteAcknowledgement(string deliveryNoteId, string fileId)
        {
            await _inventoryManagementQueryBusiness.UpdateDeliveryNoteAcknowledgement(deliveryNoteId, fileId);
        }
        public async Task UpdatePOValueInPO(string POId, double POValue)
        {
            await _inventoryManagementQueryBusiness.UpdatePOValueInPO(POId, POValue);
        }
        public async Task<List<PurchaseOrderViewModel>> ReadPOData(string ItemHead, string Vendor, string From, string To)
        {
            return await _inventoryManagementQueryBusiness.ReadPOData(ItemHead, Vendor, From, To);
        }
        public async Task<ItemsViewModel> GetRequisitionItemById(string Id)
        {
            return await _inventoryManagementQueryBusiness.GetRequisitionItemById(Id);
        }
        public async Task<POItemsViewModel> GetPOItemById(string Id)
        {
            return await _inventoryManagementQueryBusiness.GetPOItemById(Id);
        }

        public async Task<IList<GoodsReceiptItemViewModel>> GetPOItemsByPOId(string poId)
        {
            return await _inventoryManagementQueryBusiness.GetPOItemsByPOId(poId);
        }
        public async Task<IList<GoodsReceiptItemViewModel>> GetGoodReceiptItemsByReceiptId(string receiptId)
        {
            return await _inventoryManagementQueryBusiness.GetGoodReceiptItemsByReceiptId(receiptId);
        }
        public async Task<IList<GoodsReceiptViewModel>> ReadDeliveryChallanData(string ItemHead, string Vendor, string From, string To, string poId, ImsReceiptTypeEnum? receiptType)
        {
            return await _inventoryManagementQueryBusiness.ReadDeliveryChallanData(ItemHead, Vendor, From, To, poId, receiptType);
        }
        public async Task<IList<GoodsReceiptViewModel>> ReadGoodsReceiptData(string GoodsReceiptReferenceId, ImsReceiptTypeEnum receiptType)
        {
            return await _inventoryManagementQueryBusiness.ReadGoodsReceiptData(GoodsReceiptReferenceId, receiptType);
        }

        public async Task<IList<GoodsReceiptViewModel>> GetChallanDetailsbyPOId(string poId)
        {
            return await _inventoryManagementQueryBusiness.GetChallanDetailsbyPOId(poId);
        }


        public async Task<IList<PurchaseOrderViewModel>> GetPOItemsData(string poId)
        {
            return await _inventoryManagementQueryBusiness.GetPOItemsData(poId);
        }

        public async Task<PurchaseOrderViewModel> GetPOData(string serviceId)
        {
            return await _inventoryManagementQueryBusiness.GetPOData(serviceId);
        }

        public async Task<List<POTermsAndConditionsViewModel>> ReadPOTermsData(string poId)
        {
            return await _inventoryManagementQueryBusiness.ReadPOTermsData(poId);
        }

        public async Task<double> GetClosingBalance(string itemId, string warehouseId)
        {
            return await _inventoryManagementQueryBusiness.GetClosingBalance(itemId, warehouseId);
        }
        public async Task<List<RequisitionIssueItemsViewModel>> GetGoodReceiptItemsToIssue(string requisitionItemId, string itemId, string warehouseId, ImsReceiptTypeEnum receiptType)
        {
            return await _inventoryManagementQueryBusiness.GetGoodReceiptItemsToIssue(requisitionItemId, itemId, warehouseId, receiptType);
        }

        public async Task<GoodsReceiptItemViewModel> GetGoodReceiptItemDetails(string receiptId)
        {
            return await _inventoryManagementQueryBusiness.GetGoodReceiptItemDetails(receiptId);
        }
        public async Task<GoodsReceiptItemViewModel> GetGoodReceiptItemById(string id)
        {
            return await _inventoryManagementQueryBusiness.GetGoodReceiptItemById(id);
        }
        public async Task<bool> UpdateInvoiceNoinGR(string Id, string invoiceNo)
        {
            await _inventoryManagementQueryBusiness.UpdateInvoiceNoinGR(Id, invoiceNo);
            return true;
        }

        public async Task<string> GetGoodReceiptItemIdByPoItemId(string id)
        {
            return await _inventoryManagementQueryBusiness.GetGoodReceiptItemIdByPoItemId(id);
        }

        public async Task<IList<StockTransferViewModel>> GetItemTransferredList(string from, string to, string challanNo)
        {
            return await _inventoryManagementQueryBusiness.GetItemTransferredList(from, to, challanNo);
        }

        public async Task<RequisitionIssueItemsViewModel> GetRequisistionIssueItemsById(string requisitionIssueItemId)
        {
            return await _inventoryManagementQueryBusiness.GetRequisistionIssueItemsById(requisitionIssueItemId);
        }
        public async Task<List<ScheduleInvoiceViewModel>> GetRequisitiononFilters(string ItemHead, string From, string To)
        {
            return await _inventoryManagementQueryBusiness.GetRequisitiononFilters(ItemHead, From, To);
        }
        public async Task<IList<StockTransferViewModel>> GetTransferItemsList(string stockTransferId)
        {
            return await _inventoryManagementQueryBusiness.GetTransferItemsList(stockTransferId);
        }
        public async Task<StockTransferViewModel> GetTransferById(string stockTransferId)
        {
            return await _inventoryManagementQueryBusiness.GetTransferById(stockTransferId);
        }
        public async Task<List<POInvoiceViewModel>> GetPOInvoiceDetailsList(string poId)
        {
            return await _inventoryManagementQueryBusiness.GetPOInvoiceDetailsList(poId);
        }

        public async Task<bool> InvoiceNoExists(string invoiceNo)
        {
            var res = await _inventoryManagementQueryBusiness.InvoiceNoExists(invoiceNo);
            if (res.IsNotNull())
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<List<SerialNoViewModel>> GetSerailNoByHeaderIdandReferenceId(string referenceId, string hearderId)
        {
            return await _inventoryManagementQueryBusiness.GetSerailNoByHeaderIdandReferenceId(referenceId, hearderId);
        }
        public async Task updateSerialNosToIssued(string serialNoIds)
        {
            await _inventoryManagementQueryBusiness.updateSerialNosToIssued(serialNoIds);
        }
        public async Task<double> GetTotalQtyInHandCount()
        {
            return await _inventoryManagementQueryBusiness.GetTotalQtyInHandCount();
        }

        public async Task<double> GetTotalQtyTobeRecievedCount()
        {
            return await _inventoryManagementQueryBusiness.GetTotalQtyTobeRecievedCount();
        }

        public async Task<double> GetTotalAllItem()
        {
            return await _inventoryManagementQueryBusiness.GetTotalAllItem();
        }
        public async Task<double> GetTotalItemsInHand()
        {
            return await _inventoryManagementQueryBusiness.GetTotalItemsInHand();
        }
        public async Task<double> GetTotalItemsToReceive()
        {
            return await _inventoryManagementQueryBusiness.GetTotalItemsToReceive();
        }
        public async Task<double> GetTotalLowStockItems()
        {
            return await _inventoryManagementQueryBusiness.GetTotalLowStockItems();
        }

        public async Task<double> GetTotalAllItemGroupItems()
        {
            return await _inventoryManagementQueryBusiness.GetTotalAllItemGroupItems();
        }

        public async Task<IList<IdNameViewModel>> GetTopSellingsItem(DateTime startDate, DateTime endDate)
        {
            return await _inventoryManagementQueryBusiness.GetTopSellingsItem(startDate, endDate);
        }

        public async Task<double> GetPurchaseOrderQtyOrdered(DateTime startDate, DateTime endDate)
        {
            return await _inventoryManagementQueryBusiness.GetPurchaseOrderQtyOrdered(startDate, endDate);
        }

        public async Task<double> GetPurchaseOrderTotaCost(DateTime startDate, DateTime endDate)
        {
            return await _inventoryManagementQueryBusiness.GetPurchaseOrderTotaCost(startDate, endDate);
        }

        public async Task<IList<SalesOrder>> GetSalesOrderSummaryChart(DateTime startDate, DateTime endDate, InventoryDataFilterEnum filter)
        {
            return await _inventoryManagementQueryBusiness.GetSalesOrderSummaryChart(startDate, endDate, filter);
        }

        public async Task<double> GetDirectSalesAmountSummaryChart(DateTime startDate, DateTime endDate, InventoryDataFilterEnum filter)
        {
            return await _inventoryManagementQueryBusiness.GetDirectSalesAmountSummaryChart(startDate, endDate, filter);
        }
        public async Task<List<ItemStockViewModel>> ReadStockEntriesData(string itemId, string warehouseId, DateTime? FromDate, DateTime? ToDate)
        {
            return await _inventoryManagementQueryBusiness.ReadStockEntriesData(itemId, warehouseId, FromDate, ToDate);
        }

        public async Task<ItemStockViewModel> GetStockDetailsById(string stockId)
        {
            var data = await _inventoryManagementQueryBusiness.GetStockDetailsById(stockId);
            return data;
        }

        public async Task<ItemStockViewModel> GetStockDataByNoteId(string stockNoteId)
        {
            var data = await _inventoryManagementQueryBusiness.GetStockDataByNoteId(stockNoteId);
            return data;
        }

        public async Task<List<ItemsViewModel>> GetActiveItemsFilterBySubCategory(string subCategoryId)
        {

            var data = await _inventoryManagementQueryBusiness.GetActiveItemsFilterBySubCategory(subCategoryId);
            return data;
        }
        public async Task<IList<DirectSalesViewModel>> GetDirectSalesList()
        {
            var data = await _inventoryManagementQueryBusiness.GetDirectSalesList();
            return data;
        }
        public async Task<IList<SalesReturnViewModel>> GetSalesReturnList(string cusId, string From, string To, string serNo)
        {
            var data = await _inventoryManagementQueryBusiness.GetSalesReturnList(cusId, From, To, serNo);
            return data;
        }

        public async Task<SalesReturnViewModel> GetSalesReturnData(string serId)
        {
            var data = await _inventoryManagementQueryBusiness.GetSalesReturnData(serId);
            return data;
        }

        public async Task<IList<SalesReturnViewModel>> GetSalesReturnItemsList(string salesReturnId)
        {
            var data = await _inventoryManagementQueryBusiness.GetSalesReturnItemsList(salesReturnId);
            return data;
        }
        public async Task<IList<StockAdjustmentViewModel>> GetStockAdjustmentList()
        {
            var data = await _inventoryManagementQueryBusiness.GetStockAdjustmentList();
            return data;
        }
        public async Task<IList<StockAdjustmentItemViewModel>> GetStockAdjustmentItemsData(string stockAdjustmentId)
        {
            var data = await _inventoryManagementQueryBusiness.GetStockAdjustmentItemsData(stockAdjustmentId);
            return data;
        }
        public async Task<IList<PurchaseReturnViewModel>> GetPurchaseReturnList(string cusId, string From, string To, string serNo)
        {
            var data = await _inventoryManagementQueryBusiness.GetPurchaseReturnList(cusId, From, To, serNo);
            return data;
        }

        public async Task<PurchaseReturnViewModel> GetPurchaseReturnData(string serId)
        {
            var data = await _inventoryManagementQueryBusiness.GetPurchaseReturnData(serId);
            return data;
        }

        public async Task<List<SalesOrder>> GetItemValueByCategory()
        {
            return await _inventoryManagementQueryBusiness.GetItemValueByCategory();
        }

        public async Task<List<SalesOrder>> GetItemValueByWarehouse()
        {
            return await _inventoryManagementQueryBusiness.GetItemValueByWarehouse();
        }
        public async Task<StockAdjustmentViewModel> GetStockAdjustmentById(string stockAdjustmentId)
        {
            return await _inventoryManagementQueryBusiness.GetStockAdjustmentById(stockAdjustmentId);
        }

        public async Task<List<PurchaseOrderViewModel>> GetPurchaseOrderList()
        {
            var data = await _inventoryManagementQueryBusiness.GetPurchaseOrderList();
            return data;
        }

        public async Task<PurchaseOrderViewModel> GetPurchaseOrderData(string serviceId)
        {

            var data = await _inventoryManagementQueryBusiness.GetPurchaseOrderData(serviceId);
            return data;
        }

        public async Task<List<ItemsViewModel>> GetPurchaseOrderItemsList(string purchaseId)
        {
            var data = await _inventoryManagementQueryBusiness.GetPurchaseOrderItemsList(purchaseId);
            return data;
        }

        public async Task<List<PurchaseReturnViewModel>> GetPurchaseReturnItemsList(string purchaseReturnId)
        {
            var data = await _inventoryManagementQueryBusiness.GetPurchaseReturnItemsList(purchaseReturnId);
            return data;
        }
        public async Task<GoodsReceiptViewModel> GetGoodsReceiptDataBySerId(string goodsReceiptServiceId)
        {
            return await _inventoryManagementQueryBusiness.GetGoodsReceiptDataBySerId(goodsReceiptServiceId);
        }

        public async Task<GoodsReceiptViewModel> GetGoodsReceiptById(string id)
        {
            return await _inventoryManagementQueryBusiness.GetGoodsReceiptById(id);
        }
        public async Task<DeliveryNoteViewModel> GetDeliveryNoteById(string deliveryNoteId)
        {
            return await _inventoryManagementQueryBusiness.GetDeliveryNoteById(deliveryNoteId);
        }
        public async Task<List<ScheduleInvoiceViewModel>> ReadScheduleInvoice(string customerId)
        {
            return await _inventoryManagementQueryBusiness.ReadScheduleInvoice(customerId);
        }
        public async Task<IList<SalesReturnViewModel>> GetPurchaseReturnItemsData(string serviceId)
        {
            return await _inventoryManagementQueryBusiness.GetPurchaseReturnItemsData(serviceId);
        }

        public async Task<List<ItemsViewModel>> GetCRPFRequisitionItemList(string serviceId)
        {
            return await _inventoryManagementQueryBusiness.GetCRPFRequisitionItemList(serviceId);
        }
    }
}
