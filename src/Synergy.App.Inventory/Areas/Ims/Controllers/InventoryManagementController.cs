using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Synergy.App.ViewModel.IMS;
//using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace CMS.UI.Web.Areas.IMS.Controllers
{
    [Area("IMS")]
    public class InventoryManagementController : ApplicationController
    {
        private IServiceBusiness _serviceBusinness;
        private IInventoryManagementBusiness _inventoryManagementBusinness;
        private INoteBusiness _noteBusinness;
        private IUserContext _userContext;
        private ITableMetadataBusiness _tableMetadataBusiness;
        private ILOVBusiness _lovBusiness;
        private ICmsBusiness _cmsBusiness;
        private ICompanyBusiness _companyBusiness;
        private IUserBusiness _userBusiness;
        public InventoryManagementController(IServiceBusiness serviceBusinness, IUserContext userContext, ITableMetadataBusiness tableMetadataBusinness,
            ICmsBusiness cmsBusiness,
            ICompanyBusiness companyBusiness, INoteBusiness noteBusinness, IInventoryManagementBusiness inventoryManagementBusinness, ILOVBusiness lovBusiness
            ,IUserBusiness userBusiness)
        {
            _serviceBusinness = serviceBusinness;
            _userContext = userContext;
            _tableMetadataBusiness = tableMetadataBusinness;
            _cmsBusiness = cmsBusiness;
            _noteBusinness = noteBusinness;
            _inventoryManagementBusinness = inventoryManagementBusinness;
            _lovBusiness = lovBusiness;
            _companyBusiness = companyBusiness;
            _userBusiness = userBusiness;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult InventoryHome()
        {

            return View();
        }

        public IActionResult DirectSalesIndex()
        {
            return View();
        }
        public IActionResult RequisitionIndex()
        {
            return View();
        }
        public IActionResult IssueRequisitionIndex()
        {
            return View();
        }

        public async Task<IActionResult> ScheduleInvoice(string customerId, string requisitionIds)
        {
            ScheduleInvoiceViewModel model = new ScheduleInvoiceViewModel();
            var existing = await _tableMetadataBusiness.GetTableDataByColumn("IMS_CUSTOMER", null, "Id", customerId);
            if (existing != null)
            {
                model.CustomerName = existing["CustomerName"].ToString();
            }
            model.CustomerId = customerId;
            model.DataAction = DataActionEnum.Create;
            model.RequisitionIds = requisitionIds;
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ManageScheduleInvoice(ScheduleInvoiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_INVOICE_SCHEDULE";
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _serviceBusinness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        if (model.POItemsData.IsNotNullAndNotEmpty())
                        {
                            await _inventoryManagementBusinness.GenerateInvoiceScheduleItems(model.POItemsData, result.Item.UdfNoteTableId);
                            //var POValue = await _inventoryManagementBusinness.GetPOValueByPOId(result.Item.UdfNoteTableId);
                            //await _inventoryManagementBusinness.UpdatePOValueInPO(result.Item.UdfNoteTableId, POValue);
                        }
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {


                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        //[HttpPost]
        //public async Task<IActionResult> ManageScheduleInvoice(ScheduleInvoiceViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (model.DataAction == DataActionEnum.Create)
        //        {
        //            var noteTempModel = new ServiceTemplateViewModel();
        //            noteTempModel.DataAction = model.DataAction;
        //            noteTempModel.ActiveUserId = _userContext.UserId;
        //            noteTempModel.TemplateCode = "IMS_SCHEDULE_INVOICE";
        //            //noteTempModel.ParentNoteId = model.ParentNoteId;
        //            var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
        //            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
        //            notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
        //            notemodel.DataAction = DataActionEnum.Create;
        //            var result = await _serviceBusinness.ManageService(notemodel);
        //            if (result.IsSuccess)
        //            {
        //                return Json(new { success = true });
        //            }
        //            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        //        }
        //        else
        //        {

        //            var noteTempModel = new ServiceTemplateViewModel();
        //            noteTempModel.DataAction = DataActionEnum.Edit;
        //            noteTempModel.ActiveUserId = _userContext.UserId;
        //            noteTempModel.ServiceId = model.ServiceId;
        //            var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);

        //            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
        //            var result = await _serviceBusinness.ManageService(notemodel);
        //            if (result.IsSuccess)
        //            {
        //                return Json(new { success = true });
        //            }
        //            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });


        //        }

        //    }
        //    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        //}
        public IActionResult InvoiceScheduledIndex()
        {
            return View();
        }
        public async Task<IActionResult> CheckRequisitionItemsExist(string requisitionId)
        {
            var requisitionitems = await _inventoryManagementBusinness.GetRequisistionItemsData(requisitionId);
            if (requisitionitems != null && requisitionitems.Count > 0)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        public async Task<IActionResult> CheckIfStockTransferItemsAreIssued(string stockTransferId)
        {
            var items = await _inventoryManagementBusinness.GetTransferItemsList(stockTransferId);
            if (items != null && items.Count > 0)
            {
                if (items.Any(x=>x.Issued!="True")) 
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }
        
        public async Task<IActionResult> AddRequisition(string itemHead, string id)
        {
            RequisitionViewModel model = new RequisitionViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _inventoryManagementBusinness.GetRequisitionDataById(id);
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.ItemHead = itemHead;
            }
            return View("ManageRequisition", model);
        }
        public async Task<IActionResult> ManageDirectSales(string id, string CustomerId)
        {
            DirectSalesViewModel model = new DirectSalesViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                var customer = await _tableMetadataBusiness.GetTableDataByColumn("IMS_CUSTOMER", null, "Id", CustomerId);
                model.Customer = CustomerId;
                if (customer.IsNotNull()) 
                {
                    model.PAN = customer["PanNo"].ToString();
                    model.TAN = customer["TanNo"].ToString();
                    model.PINNo = customer["PIN"].ToString();
                    model.Country = customer["Country"].ToString();
                    model.State = customer["State"].ToString();
                    model.City = customer["City"].ToString();
                    model.CustomerGSTNo = customer["GstNo"].ToString();
                }
               
                model.DataAction = DataActionEnum.Create;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageDirectSales(DirectSalesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_DIRECT_SALES";
                    //noteTempModel.ParentNoteId = model.ParentNoteId;
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _serviceBusinness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {

                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.ServiceId = model.ServiceId;
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);

                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _serviceBusinness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });


                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitSalesDetails(string serviceId)
        {

            var noteTempModel = new ServiceTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.ServiceId = serviceId;
            noteTempModel.AllowPastStartDate = true;
            var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
            var existing = await _inventoryManagementBusinness.GetDirectSalesData(serviceId);
            if (existing != null)
            {
                notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                //notemodel.StartDate = DateTime.Now;
                notemodel.DueDate = DateTime.Now;
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _serviceBusinness.ManageService(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> AddItems(string serviceId)
        {
            ItemsViewModel model = new ItemsViewModel();
            model.DirectSalesId = serviceId;
            var service = await _serviceBusinness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = serviceId });
            if (service != null)
            {
                model.ProposalNo = service.ServiceNo;
            }
            var existing = await _tableMetadataBusiness.GetTableDataByColumn("SN_IMS_DIRECT_SALES", null, "Id", service.UdfNoteTableId);
            if (existing != null)
            {
                var lov = await _lovBusiness.GetSingleById(existing["ProposalType"].ToString());
                model.ProposalTypeName = lov.Name;
            }
            model.DataAction = DataActionEnum.Create;
            return View(model);
        }

        public async Task<IActionResult> CRPFRequistionItems(string WarehouseId,string serviceId)
        {
            ViewBag.ServiceId = serviceId;


            return View();
        }
        public async Task<IActionResult> ReadCRPFItemsList(string serviceId)
        {
            var model = await _inventoryManagementBusinness.GetCRPFRequisitionItemList(serviceId);
            
            return Json(model);
        }
        public async Task<IActionResult> RequisitionIssue(string id, string warehouseId, ImsIssueTypeEnum type)
        {
            IssueRequisitionViewModel model = new IssueRequisitionViewModel();
            //model.RequisitionId = id;
            model.IssueReferenceId = id;
            model.IssueReferenceType = type;
            model.WarehouseId = warehouseId;
            // var service = await _serviceBusinness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = serviceId });
            //if (service != null)
            //{
            //    model.ProposalNo = service.ServiceNo;
            //}
            //var existing = await _tableMetadataBusiness.GetTableDataByColumn("SN_IMS_DIRECT_SALES", null, "Id", service.UdfNoteTableId);
            //if (existing != null)
            //{
            //    var lov = await _lovBusiness.GetSingleById(existing["ProposalType"].ToString());
            //    model.ProposalTypeName = lov.Name;
            //}
            model.DataAction = DataActionEnum.Create;
            return View(model);
        }
        public async Task<IActionResult> AddRequisitionItems(string RequisitionId, string itemHead, string serStatus)
        {
            ItemsViewModel model = new ItemsViewModel();
            model.RequisitionId = RequisitionId;
            model.ItemHead = itemHead;
            var serviceData = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == RequisitionId);
            var service = await _serviceBusinness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = serviceData.Id });
            if (service != null)
            {
                model.ProposalNo = service.ServiceNo;
            }
            var existing = await _tableMetadataBusiness.GetTableDataByColumn("SN_IMS_DIRECT_SALES", null, "Id", service.UdfNoteTableId);
            if (existing != null)
            {
                var lov = await _lovBusiness.GetSingleById(existing["ProposalType"].ToString());
                model.ProposalTypeName = lov.Name;
            }
            model.DataAction = DataActionEnum.Create;
            model.ServiceStatusCode = serStatus;
            return View(model);
        }
        public async Task<IActionResult> ApproveRequisitionItems(string RequisitionId)
        {
            ItemsViewModel model = new ItemsViewModel();
            model.RequisitionId = RequisitionId;
            var serviceData = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == RequisitionId);
            var service = await _serviceBusinness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = serviceData.Id });
            if (service != null)
            {
                model.ProposalNo = service.ServiceNo;
            }
            var existing = await _tableMetadataBusiness.GetTableDataByColumn("SN_IMS_DIRECT_SALES", null, "Id", service.UdfNoteTableId);
            if (existing != null)
            {
                var lov = await _lovBusiness.GetSingleById(existing["ProposalType"].ToString());
                model.ProposalTypeName = lov.Name;
            }
            return View(model);
        }

        public async Task<IActionResult> SalesOrder(string serviceId, string ProposalValue)
        {
            SalesOrderViewModel model = new SalesOrderViewModel();
            model.DirectSalesId = serviceId;
            model.ProposalValue = ProposalValue;
            var service = await _serviceBusinness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = serviceId });
            var existing = await _tableMetadataBusiness.GetTableDataByColumn("SN_IMS_DIRECT_SALES", null, "Id", service.UdfNoteTableId);
            if (existing != null)
            {
                var customer = await _tableMetadataBusiness.GetTableDataByColumn("IMS_CUSTOMER", null, "Id", existing["Customer"].ToString());
                model.CustomerName = customer["CustomerName"].ToString();
                model.ContactPerson = existing["ContactPerson"].ToString();
                model.ContactNo = existing["ContactNo"].ToString();
                model.MobileNo = existing["MobileNo"].ToString();
                model.EmailId = existing["EmailId"].ToString();
                //model.ProposalValue = existing["EmailId"].ToString();
                model.Summary = existing["Summary"].ToString();
                model.CompetitionWith = existing["CompetitionWith"].ToString();
            }
            model.DataAction = DataActionEnum.Create;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageDirectSaleItems(ItemsViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var list = await _inventoryManagementBusinness.GetDirectSaleItemsData(model.DirectSalesId);
                    if (list.Any(x => x.Item == model.Item))
                    {
                        return Json(new { success = false, error = "Item Already Exist" });
                    }
                    else
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = model.DataAction;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.TemplateCode = "IMS_DIRECT_SALE_ITEMS";
                        var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);
                        model.Amount = model.SaleRate * model.ItemQuantity;
                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        notemodel.DataAction = DataActionEnum.Create;
                        var result = await _noteBusinness.ManageNote(notemodel);
                        if (result.IsSuccess)
                        {
                            return Json(new { success = true });
                        }
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }

                }
                else
                {
                    var existing = await _tableMetadataBusiness.GetTableDataByColumn("IMS_DIRECT_SALE_ITEMS", null, "NtsNoteId", model.NoteId);
                    if (existing != null)
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Edit;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.NoteId = model.NoteId;
                        var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        var result = await _noteBusinness.ManageNote(notemodel);
                        if (result.IsSuccess)
                        {
                            return Json(new { success = true });
                        }
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }

                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        [HttpPost]
        public async Task<IActionResult> ManageRequisitionItems(ItemsViewModel model)
        {
           // if (ModelState.IsValid)
           // {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_REQUSITION_ITEM";
                    var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);
                    model.Amount = model.PurchaseRate * model.ItemQuantity;
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _noteBusinness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {
                    var existing = await _tableMetadataBusiness.GetTableDataByColumn("IMS_REQUSITION_ITEM", "", "NtsNoteId", model.NoteId);
                    if (existing != null)
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Edit;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.NoteId = model.NoteId;
                    noteTempModel.SetUdfValue = true;                    
                        var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);
                    var rowdata = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                    rowdata["PurchaseRate"] = model.PurchaseRate;
                    rowdata["ItemQuantity"] = model.ItemQuantity;
                    rowdata["Amount"] = model.PurchaseRate * model.ItemQuantity;                   
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowdata);
                        var result = await _noteBusinness.ManageNote(notemodel);
                        if (result.IsSuccess)
                        {
                            return Json(new { success = true });
                        }
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }

                }

            //}
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }


        [HttpPost]
        public async Task<IActionResult> ManageSalesOrder(SalesOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_PROPOSAL_DETAILS";
                    //noteTempModel.ParentNoteId = model.ParentNoteId;
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _serviceBusinness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {

                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.ServiceId = model.ServiceId;
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);

                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _serviceBusinness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });


                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public IActionResult VendorCategoryMapping(string VendorId)
        {
            VendorCategoryViewModel model = new VendorCategoryViewModel();
            model.VendorId = VendorId;
            return View(model);
        }
        public IActionResult ItemShelfCategory(string itemShelfId)
        {
            ItemShelfViewModel model = new ItemShelfViewModel();
            model.ShelfId = itemShelfId;
            return View(model);
        }


        public async Task<IActionResult> ReadItemsData(string directSalesId)
        {
            IList<ItemsViewModel> list = new List<ItemsViewModel>();
            list = await _inventoryManagementBusinness.GetDirectSaleItemsData(directSalesId);
            return Json(list);
        }

        public async Task<IActionResult> IfItemsExits(string directSalesId)
        {
            IList<ItemsViewModel> list = new List<ItemsViewModel>();
            list = await _inventoryManagementBusinness.GetDirectSaleItemsData(directSalesId);
            if (list.Count() == 0)
            {
                return Json(false);
            }
            return Json(true);
        }

        public async Task<IActionResult> GetItemUnitDetails(string itemId)
        {
            var data = await _inventoryManagementBusinness.GetItemsUnitDetailsByItemId(itemId);
            return Json(data);
        }
        public async Task<IActionResult> ReadDirectSalesData(string Customer, string ProposalSource, string WorkflowStatus, DateTime FromDate, DateTime ToDate)
        {
            //DateTime fromDate = DateTime.ParseExact(FromDate, "d/M/yyyy", null);
            //DateTime toDate = DateTime.ParseExact(ToDate, "d/M/yyyy", null);
            DirectSalesSearchViewModel model = new DirectSalesSearchViewModel();
            model.Customer = Customer;
            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.WorkflowStatus = WorkflowStatus;
            model.ProposalSource = ProposalSource;
            IList<DirectSalesViewModel> list = new List<DirectSalesViewModel>();
            list = await _inventoryManagementBusinness.FilterDirectSalesData(model);
            return Json(list);
        }
        public async Task<IActionResult> GetCustomerList(string issueType=null)
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_CUSTOMER", "");
            if (issueType.IsNotNullAndNotEmpty())
            {
                var lov = await _lovBusiness.GetSingleById(issueType);
                if (lov.Code=="IMS_EMPLOYEE")
                {
                    var userlist = await _userBusiness.GetActiveUserList();
                    var data1 = userlist.Select(x => new CustomerViewModel { CustomerName = x.Name, Id = x.Id }).ToList();
                    return Json(data1);
                }
                return Json(data);
            }
            return Json(data);
        }
        public async Task<IActionResult> GetCountryList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_COUNTRY", "");
            return Json(data);
        }
        public async Task<IActionResult> GetBillToUnitList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_BUSINESS_UNIT", "");
            return Json(data);
        }
        public async Task<IActionResult> GetItemCategoryList(string ItemTypeId)
        {
            var where = $@" and ""N_IMS_ITEM_CATEGORY"".""ItemType"" = '{ItemTypeId}'";
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_ITEM_CATEGORY", "", where);
            return Json(data);
        }
        public async Task<IActionResult> GetItemSubCategoryList(string categoryId)
        {
            var where = $@" and ""N_IMS_ITEM_SUB_CATEGORY"".""ItemCategory"" = '{categoryId}'";
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_ITEM_SUB_CATEGORY", "", where);
            return Json(data);
        }
        public async Task<IActionResult> GetItemList(string subCategoryId)
        {
            var data = await _inventoryManagementBusinness.GetActiveItemsFilterBySubCategory(subCategoryId);
            return Json(data);
            //if (subCategoryId.IsNotNullAndNotEmpty())
            //{
            //    var where = $@" and ""N_IMS_IMS_ITEM_MASTER"".""ItemSubCategory"" = '{subCategoryId}'";
            //    var data = await _cmsBusiness.GetDataListByTemplate("IMS_ITEM_MASTER", "", where);
            //    return Json(data);
            //}
            //else
            //{

            //    var data = await _cmsBusiness.GetDataListByTemplate("IMS_ITEM_MASTER", "");
            //    return Json(data);
            //}

        }
        public async Task<IActionResult> GetStateList(string countryId)
        {
            var where = $@" and ""N_IMS_MASTERDATA_States"".""CountryId"" = '{countryId}'";
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_STATE", "", where);
            return Json(data);
        }
        public async Task<IActionResult> GetCityList(string stateId)
        {
            var where = $@" and ""N_IMS_MASTERDATA_City"".""StateId"" = '{stateId}'";
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_CITY", "", where);
            return Json(data);
        }
        public async Task<IActionResult> GetVendorContactList(string vendorId)
        {
            var where = $@" and ""N_IMS_VendorContact"".""VendorId"" = '{vendorId}'";
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_VENDOR_CONTACT", "", where);
            return Json(data);
        }
        public async Task<IActionResult> GetVendorsList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_VENDOR", "");
            return Json(data);
        }
        public async Task<IActionResult> GetWarehouseList(string legalEntityId = null)
        {
            var where = "";
            if (legalEntityId.IsNotNullAndNotEmpty())
            {
                where = $@" and ""N_IMS_MASTERDATA_WareHouseMaster"".""WarehouseLegalEntityId"" = '{legalEntityId}'";
            }
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_WAREHOUSE_MASTER", "", where);
            return Json(data);
        }

        // [HttpPost]
        public async Task<IActionResult> ManageVendorCategoryMapping(string vendorId, string ids)
        {
            if (vendorId.IsNotNullAndNotEmpty())
            {
                if (ids.IsNotNullAndNotEmpty())
                {
                    var id = ids.Split(",");
                    foreach (var item in id)
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.TemplateCode = "IMS_VENDOR_CATEGORY_MAPPING";
                        var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);
                        dynamic exo = new System.Dynamic.ExpandoObject();

                        ((IDictionary<String, Object>)exo).Add("VendorId", vendorId);
                        ((IDictionary<String, Object>)exo).Add("CategoryId", item);
                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        var result = await _noteBusinness.ManageNote(notemodel);
                        if (!result.IsSuccess)
                        {
                            return Json(new { success = false, error = result.Messages });
                        }
                    }
                }

            }

            return Json(new { success = true });
        }
        public async Task<IActionResult> ManageItemShelfCategoryMapping(string itemshelfId, string ids)
        {
            if (itemshelfId.IsNotNullAndNotEmpty())
            {
                if (ids.IsNotNullAndNotEmpty())
                {
                    var id = ids.Split(",");
                    foreach (var item in id)
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.TemplateCode = "IMS_ITEM_SHELF_CATEGORY";
                        var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);
                        dynamic exo = new System.Dynamic.ExpandoObject();

                        ((IDictionary<String, Object>)exo).Add("ShelfId", itemshelfId);
                        ((IDictionary<String, Object>)exo).Add("CategoryId",item.Trim());
                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        var result = await _noteBusinness.ManageNote(notemodel);
                        //if (!result.IsSuccess)
                        //{
                        //    return Json(new { success = false, error = result.Messages });
                        //}
                    }
                    
                }

            }

            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSalesItem(string NoteId)
        {
            await _noteBusinness.Delete(NoteId);
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRequisitionItem(string NoteId)
        {
            var note = await _noteBusinness.GetSingleById(NoteId);
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.NoteId = note.Id;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var RequisitionService = rowData1.ContainsKey("RequisitionId") ? Convert.ToString(rowData1["RequisitionId"]) : "";
            var Amount = rowData1.ContainsKey("Amount") ? Convert.ToString(rowData1["Amount"]) : "";

            // GET Service 
            var service = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == RequisitionService); //await _serviceBusinness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = RequisitionService });
            var noteTempModel1 = new NoteTemplateViewModel();
            noteTempModel1.SetUdfValue = true;
            noteTempModel1.NoteId = service.UdfNoteId;
            var notemodel1 = await _noteBusinness.GetNoteDetails(noteTempModel1);
            var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);

            rowData["RequisitionValue"] = (Convert.ToDecimal(rowData["RequisitionValue"]) - Convert.ToDecimal(Amount)).ToString();
            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            var update = await _noteBusinness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);

            await _noteBusinness.Delete(NoteId);
            return Json(new { success = true });
        }

        public IActionResult ShelfCategoryMapping(string itemshelfId)
        {
            ItemShelfViewModel model = new ItemShelfViewModel();
            model.ShelfId = itemshelfId;
            return View(model);

        }

        public async Task<IActionResult> GetShelfList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_ITEM_SHELF", "");
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> ManageShelfCategoryMapping(string shelfId, string[] categoryIds)
        {
            if (shelfId.IsNotNullAndNotEmpty() && categoryIds.Length > 0)
            {
                foreach (var item in categoryIds)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_ITEM_SHELF_CATEGORY";
                    var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);
                    dynamic exo = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo).Add("ShelfId", shelfId);
                    ((IDictionary<String, Object>)exo).Add("CategoryId", item);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    var result = await _noteBusinness.ManageNote(notemodel);
                    if (!result.IsSuccess)
                    {
                        return Json(new { success = false, error = result.Messages });
                    }
                }
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ManageRequisition(RequisitionViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_REQUISITION";
                    //noteTempModel.ParentNoteId = model.ParentNoteId;
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _serviceBusinness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {

                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.ServiceId = model.ServiceId;
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);

                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _serviceBusinness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });


                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public async Task<IActionResult> ReadRequisitionData(string ItemHead, string Customer, string status, string From, string To)
        {
            var list = await _inventoryManagementBusinness.GetRequisitionDataByItemHead(ItemHead, Customer, status, From, To);
            return Json(list);
        }
        public async Task<IActionResult> ReadIssueRequisitionData(string ItemHead, string From, string To)
        {
            var list = await _inventoryManagementBusinness.GetIssueRequisitionData(ItemHead, From, To);
            return Json(list);
        }
        public async Task<IActionResult> ReadRequisitionItemsData(string requisitionId)
        {
            IList<ItemsViewModel> list = new List<ItemsViewModel>();
            list = await _inventoryManagementBusinness.GetRequisistionItemsData(requisitionId);
            return Json(list);
        }
        public async Task<IActionResult> ReadRequisitionItemsToIssue(string requisitionId)
        {
            IList<RequisitionIssueItemsViewModel> list = new List<RequisitionIssueItemsViewModel>();
            list = await _inventoryManagementBusinness.GetRequisistionItemsToIssue(requisitionId);
            return Json(list);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitRequisition(string requisitionId)
        {
            var service = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == requisitionId);
            var noteTempModel = new ServiceTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.ServiceId = service.Id;
            noteTempModel.AllowPastStartDate = true;
            var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
            var existing = await _inventoryManagementBusinness.GetRequisitionData(service.Id);
            if (existing != null)
            {
                notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                //notemodel.StartDate = DateTime.Now;
                //notemodel.DueDate = DateTime.Now;
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _serviceBusinness.ManageService(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> ManageRequisitionIssue(IssueRequisitionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var noteTempModel = new ServiceTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "IMS_REQUSISTION_ISSUE";
                var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                notemodel.DataAction = DataActionEnum.Create;
                var result = await _serviceBusinness.ManageService(notemodel);
                if (result.IsSuccess)
                {
                    if (model.Items.IsNotNullAndNotEmpty())
                    {
                        var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RequisitionIssueItemsViewModel>>(model.Items);
                        foreach (var item in items)
                        {
                            if (Convert.ToDecimal(item.IssuedQuantity) > Convert.ToDecimal(item.BalanceQuantity))
                            {

                                ModelState.AddModelError("item", "Current issue quantity cannot exceed balance quantity");
                                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

                            }
                            item.RequisitionIssueId = result.Item.UdfNoteTableId;
                            item.IssuedQuantity = item.IssuedQuantity;
                            item.WarehouseId = model.WarehouseId;
                            item.ReferenceHeaderId = model.IssueReferenceId;
                            item.ReferenceHeaderItemId = model.RequisitionItemId;
                            var user = await _noteBusinness.GetSingleById<UserViewModel, User>(_userContext.UserId);
                            if (model.IssueReferenceType == ImsIssueTypeEnum.StockAdjustment)
                            {
                                var adjustmentStockservice = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == model.IssueReferenceId);
                                item.AdditionalInfo = "<" + result.Item.ServiceNo + "> issued against Stock Adjustment: <" + adjustmentStockservice.ServiceNo + "> By <" + user.Name + "> on <" + model.IssuedOn + ">";
                            }
                            else if (model.IssueReferenceType == ImsIssueTypeEnum.Requisition)
                            {
                                var requisition = await _inventoryManagementBusinness.GetRequisitionDataById(model.IssueReferenceId);
                                item.AdditionalInfo = "<" + result.Item.ServiceNo + "> issued against requisition: <" + requisition.ServiceNo + "> By <" + user.Name + "> on <" + model.IssuedOn + ">";
                            }
                            else if (model.IssueReferenceType == ImsIssueTypeEnum.PurchaseReturn)
                            {
                                var purchaseReturn = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == model.IssueReferenceId);

                                item.AdditionalInfo = "<" + result.Item.ServiceNo + "> issued against Purchase Return: <" + purchaseReturn.ServiceNo + "> By <" + user.Name + "> on <" + model.IssuedOn + ">";
                            }
                            else if (model.IssueReferenceType == ImsIssueTypeEnum.StockTransfer)
                            {
                                var StockTransfer = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == model.IssueReferenceId);

                                item.AdditionalInfo = "<" + result.Item.ServiceNo + "> issued against Stock Transfer: <" + StockTransfer.ServiceNo + "> By <" + user.Name + "> on <" + model.IssuedOn + ">";
                            }
                            await ManageRequisitionIssueItems(item);
                            if (model.SerialNoIds.IsNotNullAndNotEmpty())
                            {
                                var SerialNoIds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(model.SerialNoIds);//model.SerialNoIds.Split(",");
                                foreach (var serial in SerialNoIds)
                                {
                                    await _inventoryManagementBusinness.updateSerialNosToIssued(serial);
                                }
                            }

                        }

                    }

                    await SubmitService(result.Item.UdfNoteTableId);
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        [HttpPost]
        public async Task<IActionResult> ManageRequisitionIssueItems(RequisitionIssueItemsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "IMS_REQUSISTION_ISSUE_ITEM";
                var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);
                dynamic exo = new System.Dynamic.ExpandoObject();
                //string [] issuedFromIds = { model.Id };

                ((IDictionary<String, Object>)exo).Add("RequisitionIssueId", model.RequisitionIssueId);
                ((IDictionary<String, Object>)exo).Add("IssuedQuantity", model.IssuedQuantity);
                ((IDictionary<String, Object>)exo).Add("ReferenceHeaderId", model.ReferenceHeaderId);
                ((IDictionary<String, Object>)exo).Add("ReferenceHeaderItemId", model.ReferenceHeaderItemId);
                ((IDictionary<String, Object>)exo).Add("IssuedFromStockIds", model.Id);
                ((IDictionary<String, Object>)exo).Add("ItemId", model.ItemId);
                ((IDictionary<String, Object>)exo).Add("WarehouseId", model.WarehouseId);
                ((IDictionary<String, Object>)exo).Add("AdditionalInfo", model.AdditionalInfo);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.DataAction = DataActionEnum.Create;
                var result = await _noteBusinness.ManageNote(notemodel);
                if (result.IsSuccess)
                {

                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public async Task<IActionResult> GetRequisistionIssueItemsByRequisitionId(string requisitionId)
        {
            var data = await _inventoryManagementBusinness.GetRequisistionIssueItemsByRequisitionId(requisitionId);
            return Json(data);
        }
        public async Task<IActionResult> ReadRequisitionIssueItemsByIssueRefId(string issueRefId)
        {
            var data = await _inventoryManagementBusinness.GetRequisitionIssueItemsByIssueRefId(issueRefId);
            return Json(data);
        }
        public async Task<IActionResult> GetRequisistionIssueItemsToDeliver(string requisitionId)
        {
            var data = await _inventoryManagementBusinness.GetRequisistionIssueItemsToDeliver(requisitionId);
            return Json(data);
        }
        public async Task<IActionResult> GetRequisistionIssue(string requisitionId, ImsIssueTypeEnum issuetype)
        {
            var data = await _inventoryManagementBusinness.GetRequisistionIssue(requisitionId, issuetype);
            return Json(data);
        }
        public async Task<IActionResult> ManageDeliveryNote(string id)
        {
            DeliveryNoteViewModel model = new DeliveryNoteViewModel();
            model.DataAction = DataActionEnum.Create;
            var service = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == id);
            var data = await _inventoryManagementBusinness.GetRequisitionDataByServiceId(service.Id);
            if (data.IsNotNull())
            {
                model.RequisitionCode = data.ServiceNo;
                model.RequisitionValue = data.RequisitionValue;
                model.RequisitionDate = data.RequisitionDate;
                model.Particular = data.RequisitionParticular;
                model.ItemHead = data.ItemHead;

            }
            model.RequisitionId = id;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageDeliveryNote(DeliveryNoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_DELIVERY_NOTE";
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _serviceBusinness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        if (model.IssuedItemIds.IsNotNullAndNotEmpty())
                        {
                            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(model.IssuedItemIds);
                            foreach (var item in list)
                            {
                                await CreateDeliveryNoteItem(item, result.Item.UdfNoteTableId);
                            }
                        }
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.ServiceId = model.ServiceId;
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _serviceBusinness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        if (model.IssuedItemIds.IsNotNullAndNotEmpty())
                        {
                            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(model.IssuedItemIds);
                            foreach (var item in list)
                            {
                                await CreateDeliveryNoteItem(item, result.Item.UdfNoteTableId);
                            }
                        }
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public async Task<IActionResult> ViewIssueItems(string requisitionIssueId, ImsIssueTypeEnum issueType)
        {
            ViewBag.RequisitionIssueId = requisitionIssueId;
            ViewBag.IssueType = issueType;
            return View();
        }
        public async Task<IActionResult> ViewPurchaseReturnItems(string serviceId)
        {
            ViewBag.ServiceId = serviceId;           
            return View();
        }
        public async Task CreateDeliveryNoteItem(string IssuedItemId, string DeliveryNoteId)
        {
            var item = await _inventoryManagementBusinness.GetRequisistionIssueItemsById(IssuedItemId);
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Create;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "DELIVERY_ITEMS";
            var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("IssuedItemsId", IssuedItemId);
            ((IDictionary<String, Object>)exo).Add("DeliveryNoteId", DeliveryNoteId);
            ((IDictionary<String, Object>)exo).Add("DeliveredQuantity", item.IssuedQuantity);
            ((IDictionary<String, Object>)exo).Add("IsDelivered", true);
            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            notemodel.DataAction = DataActionEnum.Create;
            var result = await _noteBusinness.ManageNote(notemodel);

        }
        public async Task<IActionResult> ReadIssuedItems(string serviceId, ImsIssueTypeEnum issueType)
        {
            var data = await _inventoryManagementBusinness.GetRequisistionIssueItems(serviceId, issueType);
            return Json(data);
        }

        public IActionResult DeliveryNoteIndex()
        {
            return View();
        }
        public async Task<IActionResult> ReadDeliveryNoteData(string ItemHead, string From, string To)
        {
            var list = await _inventoryManagementBusinness.GetDeliveryNoteData(ItemHead, From, To);
            return Json(list);
        }
        public IActionResult Vendor()
        {
            return View();
        }
        public async Task<IActionResult> ReadVendorList(string countryId, string stateId, string cityId, string name)
        {
            var data = await _inventoryManagementBusinness.GetVendorList(countryId, stateId, cityId, name);
            return Json(data);
        }
        public IActionResult Customer()
        {
            return View();
        }
        public async Task<IActionResult> ReadCustomerList(string countryId, string stateId, string cityId, string name)
        {
            var data = await _inventoryManagementBusinness.GetCustomerList(countryId, stateId, cityId, name);
            return Json(data);
        }

        public async Task<IActionResult> GetRequisistionItemsByRequisitionId(string requisitionIds)
        {
            var data = await _inventoryManagementBusinness.GetRequisistionItemsByRequisitionId(requisitionIds);
            return Json(data);
        }
        public async Task<IActionResult> ValidateForGeneratePORequest(string requisitionIds)
        {
            var data = await _inventoryManagementBusinness.GetRequisistionItemsByRequisitionId(requisitionIds, true);
            if (data.Count > 0)
            {
                if (data.Any(x => x.ExistingPOQuantity < x.ApprovedQuantity))
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }

            }
            else
            {
                return Json(new { success = true });
            }
        }

        public IActionResult POItems(string poId, string requisitionIds, string statusCode)
        {
            POItemsViewModel model = new POItemsViewModel();
            model.POId = poId;
            model.RequisitionIds = requisitionIds;
            model.ServiceStatusCode = statusCode;
            return View(model);
        }
        public async Task<IActionResult> ReadPOItemsData(string poId)
        {
            var list = await _inventoryManagementBusinness.ReadPOItemsData(poId);
            return Json(list);
        }
        public IActionResult POGeneration(string requisitionIds, string itemHeadId)
        {
            PurchaseOrderViewModel model = new PurchaseOrderViewModel();
            model.DataAction = DataActionEnum.Create;
            model.RequisitionIds = requisitionIds;
            model.IssueFromIds = requisitionIds;
            model.ItemHeadId = itemHeadId;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManagePOGeneration(PurchaseOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_PO";
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _serviceBusinness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        if (model.POItemsData.IsNotNullAndNotEmpty())
                        {
                            await _inventoryManagementBusinness.GeneratePOItems(model.POItemsData, result.Item.UdfNoteTableId);
                            var POValue = await _inventoryManagementBusinness.GetPOValueByPOId(result.Item.UdfNoteTableId);
                            await _inventoryManagementBusinness.UpdatePOValueInPO(result.Item.UdfNoteTableId, POValue);
                        }
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {
                    //var existing = await _tableMetadataBusiness.GetTableDataByColumn("IMS_REQUISITION_ITEMS", null, "NtsNoteId", model.NoteId);
                    //if (existing != null)
                    //{
                    //    var noteTempModel = new ServiceTemplateViewModel();
                    //    noteTempModel.DataAction = DataActionEnum.Edit;
                    //    noteTempModel.ActiveUserId = _userContext.UserId;
                    //    noteTempModel.ServiceId = model.ServiceId;
                    //    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);

                    //    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    //    var result = await _serviceBusinness.ManageService(notemodel);
                    //    if (result.IsSuccess)
                    //    {
                    //        return Json(new { success = true });
                    //    }
                    //    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    //}

                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        [HttpPost]
        public async Task<IActionResult> ManagePOItems(string POItemsData, string PoId)
        {

            await _inventoryManagementBusinness.GeneratePOItems(POItemsData, PoId);
            var POValue = await _inventoryManagementBusinness.GetPOValueByPOId(PoId);
            await _inventoryManagementBusinness.UpdatePOValueInPO(PoId, POValue);
            return Json(new { success = true });
        }

        public IActionResult PurchaseOrder()
        {
            return View();
        }
        public async Task<IActionResult> ReadPOList(string itemHead = null, string vendorId = null, string statusId = null, string From = null, string To = null)
        {
            var list = await _inventoryManagementBusinness.GetVendorPOList(itemHead, vendorId, statusId, From, To);
            return Json(list);
        }
        public async Task<IActionResult> ReceivePOItems()
        {
            return View();
        }

        public async Task<IActionResult> ChangeVendorForPO(string serId, string venId, string poId = null)
        {
            var list = await _inventoryManagementBusinness.GetVendorPOList();
            var po = list.Single(x => x.ServiceId == serId);

            var model = new PurchaseOrderViewModel()
            {
                ServiceId = serId,
                VendorId = venId,
                POID = poId,
                VendorName = po.VendorName,
                ContactPersonName = po.ContactPersonName,
                VendorAddress = po.VendorAddress,
                ContactNo = po.ContactNo
            };

            return View(model);
        }

        public async Task<IActionResult> ReadPOData(string ItemHead, string Vendor, string From, string To)
        {
            var list = await _inventoryManagementBusinness.ReadPOData(ItemHead, Vendor, From, To);
            return Json(list);
        }

        [HttpPost]
        public async Task<IActionResult> ManageGoodsReceiptGeneration(GoodsReceiptViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_GOODS_RECEIPT";
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _serviceBusinness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        if (model.POItemsData.IsNotNullAndNotEmpty())
                        {
                            await _inventoryManagementBusinness.GenerateGoodsReceiptItems(model.POItemsData, result.Item.UdfNoteTableId, model.WarehouseId, model.GoodsReceiptReferenceId, model.ReceiveDate);
                        }
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {


                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeVendorForPO(PurchaseOrderViewModel model)
        {
            ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            serviceModel.ServiceId = model.ServiceId;

            var service = await _serviceBusinness.GetServiceDetails(serviceModel);

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.SetUdfValue = true;
            noteTempModel.NoteId = service.UdfNoteId;
            var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

            var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

            rowData["VendorId"] = model.VendorId;
            rowData["ContactPersonId"] = model.ContactPersonId;
            rowData["ContactNo"] = model.ContactNo;

            //var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);

            //var update = await _noteBusinness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

            service.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);

            service.DataAction = DataActionEnum.Edit;

            var res = await _serviceBusinness.ManageService(service);

            return Json(new { success = true });
        }

        public async Task<IActionResult> GetCustomerContacts(string customerId)
        {
            var list = await _inventoryManagementBusinness.ReadCustomerContactsData(customerId);
            return Json(list);
        }

        public async Task<IActionResult> GetVendorContacts(string vendorId)
        {
            var list = await _inventoryManagementBusinness.ReadVendorContactsData(vendorId);
            return Json(list);
        }

        public async Task<IActionResult> GetCustomerContactDetails(string CustomerContactId, string CustomerId)
        {
            var list = await _inventoryManagementBusinness.ReadCustomerContactsData(CustomerId);
            list = list.Where(x => x.Id == CustomerContactId).ToList();
            return Json(list);
        }

        public async Task<IActionResult> GetSingleCustomerDetails(string id)
        {
            var where = $@" and ""N_IMS_IMS_CUSTOMER"".""Id"" = '{id}' ";
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_CUSTOMER", "", where);
            return Json(data);
        }
        public IActionResult CurrentStock()
        {
            return View();
        }
        public IActionResult DeadStockIndex()
        {
            return View();
        }
        public IActionResult InvoiceDetailsIndex()
        {
            return View();
        }
        public IActionResult IssueTypeIndex()
        {
            return View();
        }
        public IActionResult ItemHistoryIndex()
        {
            return View();
        }
        public IActionResult ItemTransferIndex()
        {
            return View();
        }
        public IActionResult OrderBookIndex()
        {
            return View();
        }
        public IActionResult PurchaseOrderStatusIndex()
        {
            return View();
        }
        public IActionResult OrderStatusIndex()
        {
            return View();
        }
        public async Task<IActionResult> GetInvertoryFinancialYear()
        {
            //var data = new List<IdNameViewModel>();
            //data.Add(new IdNameViewModel { Id = "1", Name = "2020-2021" });
            //data.Add(new IdNameViewModel { Id = "2", Name = "2021-2022" });
            //data.Add(new IdNameViewModel { Id = "3", Name = "2022-2023" });
            
            var data = await _inventoryManagementBusinness.ReadInvertoryFinancialYearIdNameList();
            return Json(data);
        }
        public async Task<IActionResult> ReadInvoiceDetailsData(DateTime fromDate, DateTime toDate)
        {
            //var data = new List<POInvoiceViewModel>();
            var data = await _inventoryManagementBusinness.ReadInvoiceDetailsData(fromDate, toDate);
            return Json(data);
        }
        public async Task<IActionResult> ReadIssueTypeData(DateTime fromDate, DateTime toDate, string issueTypeId, string departmentId, string employeeId, string issueToTypeId)
        {
            //var data = new List<RequisitionIssueItemsViewModel>();
            var data = await _inventoryManagementBusinness.ReadIssueTypeData(fromDate, toDate, issueTypeId, departmentId, employeeId, issueToTypeId);
            return Json(data);
        }
        public async Task<IActionResult> ReadItemHistoryData(DateTime fromDate, DateTime toDate, string warehouseId, string itemTypeId, string itemCategoryId, string itemSubCategoryId, string itemId)
        {
            //var data = new List<ItemsViewModel>();
            var data = await _inventoryManagementBusinness.ReadItemHistoryData(fromDate, toDate, warehouseId, itemTypeId, itemCategoryId, itemSubCategoryId, itemId);
            return Json(data);
        }
        public async Task<IActionResult> ReadItemTransferData(DateTime fromDate, DateTime toDate, string warehouseId, string itemTypeId, string itemCategoryId, string itemSubCategoryId, string itemId)
        {
            //var data = new List<ItemsViewModel>();
            var data = await _inventoryManagementBusinness.ReadItemTransferData(fromDate, toDate, warehouseId, itemTypeId, itemCategoryId, itemSubCategoryId, itemId);
            return Json(data);
        }
        public async Task<IActionResult> ReadOrderBookData()
        {
            //var data = new List<ItemsViewModel>();
            var data = await _inventoryManagementBusinness.ReadOrderBookData();
            return Json(data);
        }
        public async Task<IActionResult> ReadOrderStatusData(string financialYearId)
        {
            //var data = new List<ItemsViewModel>();
            var data = await _inventoryManagementBusinness.ReadOrderStatusData(financialYearId);
            return Json(data);
        }
        public async Task<IActionResult> ReadPurchaseOrderStatusData(DateTime fromDate, DateTime toDate, string statusId)
        {
            //var data = new List<PurchaseOrderViewModel>();
            var data = await _inventoryManagementBusinness.ReadPurchaseOrderStatusData(fromDate, toDate, statusId);
            return Json(data);
        }
        public async Task<IActionResult> SubmitOrderDetails(string serviceId, string ProposalValue, bool SalesOrder = false)
        {
            DirectSalesViewModel model = new DirectSalesViewModel();
            model.Id = serviceId;
            model.ProposalValue = ProposalValue=="null"?"": ProposalValue;
            //model.OrderValue = ProposalValue;
            var service = await _serviceBusinness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = serviceId });
            var existing = await _tableMetadataBusiness.GetTableDataByColumn("SN_IMS_DIRECT_SALES", null, "Id", service.UdfNoteTableId);
            if (existing != null)
            {
                var customer = await _tableMetadataBusiness.GetTableDataByColumn("IMS_CUSTOMER", null, "Id", existing["Customer"].ToString());
                model.CustomerName = customer["CustomerName"].ToString();
                model.Customer = existing["Customer"].ToString();
                model.ContactPerson = existing["ContactPerson"].ToString();
                model.PAN = customer["PanNo"].ToString();
                model.TAN = customer["TanNo"].ToString();
                model.PINNo = customer["PIN"].ToString();                
                model.GSTNo = customer["GstNo"].ToString();
                model.OrderValue = existing["OrderValue"].ToString();
                var customerContact = await _tableMetadataBusiness.GetTableDataByColumn("IMS_CUSTOMER_CONTACT", null, "Id", existing["ContactPerson"].ToString());
                model.ContactPersonName = customerContact["ContactPersonName"].ToString();
                model.ContactNo = existing["ContactNo"].ToString();
                model.MobileNo = existing["MobileNo"].ToString();
                model.EmailId = existing["EmailId"].ToString();
                //model.ProposalValue = existing["ProposalValue"].ToString();
                model.Summary = existing["Summary"].ToString();
                model.CompetitionWith = existing["CompetitionWith"].ToString();
                model.ProposalDate = existing["ProposalDate"].ToString();
                model.ProposalType = existing["ProposalType"].ToString();
                model.NextFollowUpDate = existing["NextFollowUpDate"].ToString();
                model.Designation = existing["Designation"].ToString();

                model.Country = existing["Country"].ToString();
                model.State = existing["State"].ToString();
                model.City = existing["City"].ToString();
                model.ShippingAddress = existing["ShippingAddress"].ToString();

                if (SalesOrder)
                {
                    model.PAN = existing["PAN"].ToString();
                    model.TAN = existing["TAN"].ToString();
                    model.PINNo = existing["PINNo"].ToString();
                    model.OrderNo = existing["OrderNo"].ToString();
                    model.BaseValue = existing["BaseValue"].ToString();
                    model.TAXValue = existing["TAXValue"].ToString();
                    model.OrderValue = existing["OrderValue"].ToString();
                    model.OrderDocumentId = existing["OrderDocumentId"].ToString();
                    model.Address = existing["Address"].ToString();
                    //if (existing["OrderDate"].IsNotNull())
                    //{
                    //    model.OrderDate = (DateTime?)existing["OrderDate"];
                    //}
                    //if (existing["CompletionDate"].IsNotNull())
                    //{
                    //    model.CompletionDate = (DateTime?)existing["CompletionDate"];
                    //}
                }
            }
            model.DataAction = DataActionEnum.Edit;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> OnOrderSubmit(DirectSalesViewModel model)
        {
            var noteTempModel = new ServiceTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.ServiceId = model.ServiceId;

            var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);

            notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
            notemodel.DueDate = DateTime.Now;

            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var result = await _serviceBusinness.ManageService(notemodel);
            if (result.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

        }

        public async Task<IActionResult> ManageReceiveItem(string goodsReceiptReferenceId, ImsReceiptTypeEnum receiptType, string itemHeadId = null)
        {
            GoodsReceiptViewModel model = new GoodsReceiptViewModel();
            model.DataAction = DataActionEnum.Create;
            model.GoodsReceiptReferenceId = goodsReceiptReferenceId;
            model.ReceiptType = receiptType;
            model.ItemHeadId = itemHeadId;
            return View(model);
        }
        public async Task<IActionResult> GetPOItemsByPOId(string poId)
        {
            var data = await _inventoryManagementBusinness.GetPOItemsByPOId(poId);
            return Json(data);
        }
        public async Task<IActionResult> ReadDeliveryChallanData(string ItemHead, string Vendor, string From, string To, string poId, ImsReceiptTypeEnum? receiptType)
        {
            var data = await _inventoryManagementBusinness.ReadDeliveryChallanData(ItemHead, Vendor, From, To, poId, receiptType);
            return Json(data);
        }
        public async Task<IActionResult> ReadGoodsReceiptData(string goodsReceiptReferenceId, ImsReceiptTypeEnum receiptType)
        {
            var data = await _inventoryManagementBusinness.ReadGoodsReceiptData(goodsReceiptReferenceId, receiptType);
            return Json(data);
        }
        public async Task<IActionResult> CheckPOItemsExist(string poId)
        {
            var poitems = await _inventoryManagementBusinness.GetPOItemsData(poId);
            if (poitems != null && poitems.Count > 0)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitPO(string serId)
        {
            //var service = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == requisitionId);
            var serviceTempModel = new ServiceTemplateViewModel();
            serviceTempModel.DataAction = DataActionEnum.Edit;
            serviceTempModel.ActiveUserId = _userContext.UserId;
            serviceTempModel.ServiceId = serId;
            serviceTempModel.AllowPastStartDate = true;
            var servicemodel = await _serviceBusinness.GetServiceDetails(serviceTempModel);
            var existing = await _inventoryManagementBusinness.GetPOData(serId);
            if (existing != null)
            {
                servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                servicemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _serviceBusinness.ManageService(servicemodel);

                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> InventoryDashboard()
        {
            var legalentity = await _inventoryManagementBusinness.GetSingleById<LegalEntityViewModel, LegalEntity>(_userContext.LegalEntityId);
            ViewBag.CurrencySymbol = legalentity.CurrencySymbol;
            return View();
        }

        public async Task<IActionResult> GetDashboardSalesActivitySummary()
        {
            var totalQtyInHand = await _inventoryManagementBusinness.GetTotalQtyInHandCount();
            var totalQtyTobeRecieved = await _inventoryManagementBusinness.GetTotalAllItem();
            InventoryDashboardViewModel model = new InventoryDashboardViewModel
            {
                TotalTobePackedItem = 3057,
                TotalTobeShippedItem = 43,
                TotalTobeDeliveredItem = 25,
                TotalTobeInvoicedItem = 4767,

                TotalQtyInHand = totalQtyInHand,
                TotalQtyTobeRecieved = totalQtyTobeRecieved
            };
            return Json(model);
        }

        public async Task<IActionResult> GetDashboardProductDetails()
        {
            var totalLowStockItems = await _inventoryManagementBusinness.GetTotalLowStockItems();
            var totalAllItemGroupItems = await _inventoryManagementBusinness.GetTotalAllItemGroupItems();
            var totalAllItem = await _inventoryManagementBusinness.GetTotalAllItem();
            var ItemsInHand = await _inventoryManagementBusinness.GetTotalItemsInHand();
            var ItemsToReceive = await _inventoryManagementBusinness.GetTotalItemsToReceive();
            
            var model = new InventoryDashboardViewModel();
            model.TotalLowStockItems = totalLowStockItems;
            model.TotalAllItemGroupItems = totalAllItemGroupItems;
            model.TotalAllItem = totalAllItem;
            model.ItemsInHand = ItemsInHand;
            model.ItemsToReceive = ItemsToReceive;
            //model.ActionItemSeries = new List<int> { 44, 55, 41, 17, 15 };
            //model.ActiveItemLabels = new List<string> { "Comedy", "Action", "SciFi", "Drama", "Horror" };

            return Json(model);
        }

        public async Task<IActionResult> GetDashboardTopSellingsItem(InventoryDataFilterEnum filter)
        {
            var res = Helper.GetStartAndEndDateByEnum(filter);
            var model = await _inventoryManagementBusinness.GetTopSellingsItem(res.Item1, res.Item2);
            return Json(model);
        }

        public async Task<IActionResult> GetDashboardPurchaseOrderItem(InventoryDataFilterEnum filter)
        {
            var res = Helper.GetStartAndEndDateByEnum(filter);
            var qtyOrdered = await _inventoryManagementBusinness.GetPurchaseOrderQtyOrdered(res.Item1, res.Item2);
            var totalCost = await _inventoryManagementBusinness.GetPurchaseOrderTotaCost(res.Item1, res.Item2);
            var currencyDetails = await _companyBusiness.GetSingleById(_userContext.CompanyId);
            var currency = "";
            if (currencyDetails.IsNotNull())
            {
                currency = currencyDetails.CurrencySymbol;
            }
            var model = new InventoryDashboardViewModel
            {
                Currency = currency,
                QtyOrdered = qtyOrdered,//625.000,
                TotalCost = currency + " " + decimal.Parse(totalCost.ToString(), System.Globalization.NumberStyles.Currency)//"$101,811.084"
            };
            return Json(model);
        }

        public async Task<IActionResult> GetDashboardSalesOrderSummaryChart(InventoryDataFilterEnum filter)
        {
            var res = Helper.GetStartAndEndDateByEnum(filter);
            var model1 = await _inventoryManagementBusinness.GetSalesOrderSummaryChart(res.Item1, res.Item2, filter);
            var directSalesAmount = await _inventoryManagementBusinness.GetDirectSalesAmountSummaryChart(res.Item1, res.Item2, filter);
            if (model1.IsNotNull() && model1.Count > 0)
            {

                var model = new InventoryDashboardViewModel
                {
                    SalesOrderSeries = model1.ToList(),
                    DirectSales = "Rs." + directSalesAmount,
                    Categories = model1.ToList()[0].categories,
                };

                return Json(model);
            }
            return Json(new InventoryDashboardViewModel());
            //return null;
        }
        public async Task<IActionResult> GetItemValueByCategory()
        {
            var list = await _inventoryManagementBusinness.GetItemValueByCategory();
            if (list.IsNotNull())
            {
                list = list.OrderByDescending(x => x.ItemValue).Take(5).ToList();
                var model = new InventoryDashboardViewModel
                {
                    ItemValueLabel = list.Select(x => x.ItemName).ToList(),
                    ItemValueSeries = list.Select(x => x.ItemValue.ToSafeInt()).ToList()
                };
                return Json(model);
            }
            return null;
        }
        public async Task<IActionResult> GetItemValueByWarehouse()
        {
            var list = await _inventoryManagementBusinness.GetItemValueByWarehouse();
            if (list.IsNotNull())
            {
                list = list.OrderByDescending(x => x.ItemValue).Take(5).ToList();
                var model = new InventoryDashboardViewModel
                {
                    ItemValueLabel = list.Select(x => x.ItemName).ToList(),
                    ItemValueSeries = list.Select(x => x.ItemValue.ToSafeInt()).ToList()
                };
                return Json(model);
            }
            return null;
        }
        //public async Task<IActionResult> ReadDashboardSalesOrderList(InventoryDataFilterEnum filter)
        //{
        //    var data = new List<DashboardSalesOrderViewModel> {
        //        new DashboardSalesOrderViewModel {
        //            Channel = "Channel 1",
        //            Draft = "Yes",
        //            Confirmed = "Yes",
        //            Packed = "Yes",
        //            Shipped = "In Progress",
        //            Invoiced = "No"
        //        },
        //         new DashboardSalesOrderViewModel {
        //            Channel = "Channel 2",
        //            Draft = "Yes",
        //            Confirmed = "Yes",
        //            Packed = "Yes",
        //            Shipped = "In Progress",
        //            Invoiced = "No"
        //        },  new DashboardSalesOrderViewModel {
        //            Channel = "Channel 3",
        //            Draft = "Yes",
        //            Confirmed = "Yes",
        //            Packed = "Yes",
        //            Shipped = "In Progress",
        //            Invoiced = "No"
        //        },  new DashboardSalesOrderViewModel {
        //            Channel = "Channel 4",
        //            Draft = "Yes",
        //            Confirmed = "Yes",
        //            Packed = "Yes",
        //            Shipped = "In Progress",
        //            Invoiced = "No"
        //        },
        //        };
        //    return Json(data);
        //}

        public async Task<IActionResult> GetRequisitionList(string stateId)
        {
            //var where = $@" and ""N_IMS_MASTERDATA_City"".""StateId"" = '{stateId}'";
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_REQUISITION", "", "");
            return Json(data);
        }

        public async Task<IActionResult> GetTermsAndConditionsList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_TERMS_AND_CONDITIONS", "", "");
            return Json(data);
        }

        public IActionResult POTermsAndConditions(string poId)
        {
            var model = new POTermsAndConditionsViewModel()
            {
                POID = poId,
                PortalId = _userContext.PortalId
            };
            return View(model);
        }
        public async Task<IActionResult> ReadPOTermsData(string poId)
        {
            var list = await _inventoryManagementBusinness.ReadPOTermsData(poId);
            return Json(list);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePOTCItem(string NoteId)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.SetUdfValue = true;
            noteTempModel.NoteId = NoteId;
            var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

            var res = await _noteBusinness.DeleteNote(notemodel);
            return Json(new { success = res.IsSuccess });
        }

        [HttpPost]
        public async Task<IActionResult> ManagePOTC(POTermsAndConditionsViewModel model)
        {
            List<POTermsAndConditionsViewModel> items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<POTermsAndConditionsViewModel>>(model.TermsList);
            if (items.IsNotNull() && items.Count() > 0)
            {
                var existing = await _inventoryManagementBusinness.ReadPOTermsData(model.POID);
                var existingids = existing.Select(x => x.TermsId).ToArray();

                var newdata = items.Where(x => !existingids.Contains(x.Id)).ToList();

                if (newdata.Count() > 0)
                {
                    foreach (var data in newdata)
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.TemplateCode = "IMS_PO_TANDC";
                        var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);
                        dynamic exo = new System.Dynamic.ExpandoObject();

                        ((IDictionary<String, Object>)exo).Add("POID", model.POID);
                        ((IDictionary<String, Object>)exo).Add("TermsId", data.Id);
                        ((IDictionary<String, Object>)exo).Add("TermsTitle", data.Title);
                        ((IDictionary<String, Object>)exo).Add("TermsDescription", data.Description);

                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        notemodel.DataAction = DataActionEnum.Create;
                        var result = await _noteBusinness.ManageNote(notemodel);
                    }
                }
                else
                {
                    return Json(new { success = false, errormsg = "Selected Terms and Conditions already added" });
                }

            }

            return Json(new { success = true });
        }

        public IActionResult PORequisitionList(string poId)
        {
            ViewBag.RequisitionIds = "99ae53d4-d8c8-4c81-abf2-64b02293580d,57f0f969-be52-4319-855c-92ee5307b376";
            return View();
        }

        public async Task<IActionResult> ReadRequisitionList(string requisitionIds)
        {
            var list = await _inventoryManagementBusinness.GetRequisitionDataByItemHead("", "", "", "", "", requisitionIds);
            return Json(list);
        }

        public IActionResult AddPurchaseInvoice(string poId)
        {
            ViewBag.PoId = poId;
            return View();
        }

        public IActionResult PurchaseInvoice()
        {
            return View();
        }
        public IActionResult RequisitionIssueItems(string requisitionId, string warehouseId, ImsIssueTypeEnum issuetype)
        {
            ViewBag.RequisitionId = requisitionId;
            ViewBag.WarehouseId = warehouseId;
            ViewBag.ImsIssueType = issuetype;
            return View();
        }
        public async Task<IActionResult> ReadGoodReceiptItemsToIssue(string requisitionItemId, string itemId, string warehouseId, ImsReceiptTypeEnum receiptType)
        {
            IList<RequisitionIssueItemsViewModel> list = new List<RequisitionIssueItemsViewModel>();
            list = await _inventoryManagementBusinness.GetGoodReceiptItemsToIssue(requisitionItemId, itemId, warehouseId, receiptType);
            return Json(list);
        }
        public async Task<IActionResult> PORequisitionItemsList(string serId, string requisitionId, string poId)
        {
            var data = await _inventoryManagementBusinness.GetRequisitionDataByServiceId(serId);
            data.RequisitionId = requisitionId;
            data.POID = poId;
            return View(data);
        }
        public async Task<IActionResult> ReadRequisitionItemsList(string requisitionIds)
        {
            var list = await _inventoryManagementBusinness.GetRequisitionDataByItemHead("", "", "", "", "", requisitionIds);
            return Json(list);
        }

        public async Task<IActionResult> SubmitRequisitionIssue(string serviceId)
        {
            var serviceTempModel = new ServiceTemplateViewModel();
            serviceTempModel.DataAction = DataActionEnum.Edit;
            serviceTempModel.ActiveUserId = _userContext.UserId;
            serviceTempModel.ServiceId = serviceId;
            var servicemodel = await _serviceBusinness.GetServiceDetails(serviceTempModel);

            var serviceTempModel1 = new NoteTemplateViewModel();
            serviceTempModel1.SetUdfValue = true;
            serviceTempModel1.NoteId = servicemodel.UdfNoteId;
            var servicemodel1 = await _noteBusinness.GetNoteDetails(serviceTempModel1);
            var rowData2 = servicemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            servicemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData2);
            servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
            servicemodel.DataAction = DataActionEnum.Edit;
            var result1 = await _serviceBusinness.ManageService(servicemodel);
            if (result1.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> GetChallanDetailsByPoId(string poId)
        {
            var data = await _inventoryManagementBusinness.GetChallanDetailsbyPOId(poId);
            return Json(data);
        }

        public async Task<IActionResult> GetPOInvoiceList(string poId)
        {
            var data = await _inventoryManagementBusinness.GetPOInvoiceDetailsList(poId);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> ManagePurchaseInvoiceData(string invoiceno, string pidate, List<string> receiptIds, string poId, List<GoodsReceiptViewModel> challanList)
        {
            var isInvoiceNoExists = await _inventoryManagementBusinness.InvoiceNoExists(invoiceno);
            if (isInvoiceNoExists)
            {
                return Json(new { success = false, error = "Invoice No. already exists." });
            }
            var serTempModel = new POInvoiceViewModel();
            serTempModel.DataAction = DataActionEnum.Create;
            serTempModel.ActiveUserId = _userContext.UserId;
            serTempModel.TemplateCode = "IMS_PO_INVOICE";

            var sermodel = await _serviceBusinness.GetServiceDetails(serTempModel);

            dynamic exo = new System.Dynamic.ExpandoObject();
            ((IDictionary<String, Object>)exo).Add("InvoiceNo", invoiceno);
            ((IDictionary<String, Object>)exo).Add("InvoiceDate", pidate);
            ((IDictionary<String, Object>)exo).Add("PoId", poId);

            sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            sermodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
            sermodel.DataAction = DataActionEnum.Create;
            var result = await _serviceBusinness.ManageService(sermodel);

            if (result.IsSuccess)
            {
                foreach (var x in receiptIds)
                {
                    var data = await _inventoryManagementBusinness.GetGoodReceiptItemDetails(x);

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_PO_INVOICE_ITEM";

                    var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);
                    dynamic exo1 = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo1).Add("POInvoiceId", result.Item.UdfNoteTableId);
                    ((IDictionary<String, Object>)exo1).Add("ItemId", data.ItemId);
                    ((IDictionary<String, Object>)exo1).Add("POQuantity", data.POQuantity);
                    ((IDictionary<String, Object>)exo1).Add("ReceivedQuantity", data.ReceivedQuantity);
                    ((IDictionary<String, Object>)exo1).Add("POItemId", data.ReferenceHeaderItemId);

                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                    notemodel.ParentServiceId = result.Item.ServiceId;
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result1 = await _noteBusinness.ManageNote(notemodel);
                }
                return Json(new { success = true });
            }
            return Json(new { success = false, error = "Invoice Generation Failed" });
        }

        public async Task<IActionResult> AddViewItems(string id, ImsReceiptTypeEnum receiptType)
        {
            // good receipt table id
            ViewBag.Id = id;
            ViewBag.ReceiptType = receiptType;
            var receipt = await _inventoryManagementBusinness.GetGoodsReceiptById(id);
            if (receipt.IsNotNull())
            {
                ViewBag.InvoiceFile = receipt.InvoiceFile;
                ViewBag.ChallanFile = receipt.ChallonFile;
            }

            return View();
        }
        public IActionResult SerializeItems(string id)
        {
            // good receipt table id
            ViewBag.Id = id;
            return View();
        }

        public async Task<IActionResult> GetDeliveryChallanItems(string id)
        {
            // var poId = await _inventoryManagementBusinness.GetPoIdByGoodReceiptId(id);
            var data = await _inventoryManagementBusinness.GetGoodReceiptItemsByReceiptId(id);
            return Json(data);
        }
        public async Task<IActionResult> CheckSerialNoForRecepitItemsExist(string id)
        {
            var receiptitems = await _inventoryManagementBusinness.GetGoodReceiptItemsList(id);
            bool exist = true;
            if (receiptitems != null && receiptitems.Count > 0)
            {
                foreach (var data in receiptitems)
                {
                    var list = await _inventoryManagementBusinness.GetSerailNoByHeaderIdandReferenceId(data.Id, id);
                    if (list.Count() == 0)
                    {
                        exist = false;
                    }

                }
            }
            return Json(new { success = exist });
        }


        public async Task<IActionResult> GetSerialNos(string referenceId, string referenceHeaderId)
        {
            var list = await _inventoryManagementBusinness.GetSerailNoByHeaderIdandReferenceId(referenceId, referenceHeaderId);
            if (list.IsNotNull() && list.Count() > 0)
            {
                return Json(list);
            }
            else
            {
                var item = await _inventoryManagementBusinness.GetGoodReceiptItemById(referenceId);
                if (item != null)
                {
                    var generatedItems = await _noteBusinness.GenerateItemSerialNumbers(Convert.ToInt64(item.ItemQuantity));
                    foreach (var data in generatedItems)
                    {
                        SerialNoViewModel model = new SerialNoViewModel();
                        model.SerialNo = data;
                        model.Specification = data;
                        model.DataAction = DataActionEnum.Create;
                        model.ReferenceHeaderId = referenceHeaderId;
                        model.ReferenceId = referenceId;
                        await ManageSerialNo(model);
                    }
                }
                else
                {
                    var data = await _tableMetadataBusiness.GetTableDataByColumn("ITEM_STOCK", "", "Id", referenceId);
                    if (data != null)
                    {
                        if (Convert.ToInt64(data["OpeningQuantity"]) > 0)
                        {
                            var generatedItems = await _noteBusinness.GenerateItemSerialNumbers(Convert.ToInt64(data["OpeningQuantity"]));
                            foreach (var data1 in generatedItems)
                            {
                                SerialNoViewModel model = new SerialNoViewModel();
                                model.SerialNo = data1;
                                model.Specification = data1;
                                model.DataAction = DataActionEnum.Create;
                                model.ReferenceHeaderId = referenceHeaderId;
                                model.ReferenceId = referenceId;
                                await ManageSerialNo(model);
                            }
                        }

                    }
                }
                var list1 = await _inventoryManagementBusinness.GetSerailNoByHeaderIdandReferenceId(referenceId, referenceHeaderId);

                return Json(list1);
            }
        }

        public IActionResult ItemTransfer()
        {
            return View();
        }

        public async Task<IActionResult> ReadItemTransferList(string From, string To, string challanNo)
        {
            var data = await _inventoryManagementBusinness.GetItemTransferredList(From, To, challanNo);
            return Json(data);
        }

        public async Task<IActionResult> NewItemTransfer(string serid, string serstatus)
        {
            var model = new StockTransferViewModel()
            {
                DataAction = DataActionEnum.Create,
                ServiceId = serid,
                ServiceStatusCode = serstatus,
            };
            if (serid.IsNotNullAndNotEmpty())
            {
                var data = await _inventoryManagementBusinness.GetItemTransferredList("", "", "");
                model = data.Where(x => x.ServiceId == serid).FirstOrDefault();
                model.DataAction = DataActionEnum.Edit;
            }
            return View(model);
        }
        public async Task<IActionResult> GetBusinessUnitList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_BUSINESS_UNIT", "", "");
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> ManageTransferItems(StockTransferViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var data = await _inventoryManagementBusinness.GetItemTransferredList("", "", model.ChallanNo);
                    if (data.Count > 0)
                    {
                        return Json(new { success = false, error = "Challan no already exist" });
                    }
                    else
                    {
                        if (model.DataAction == DataActionEnum.Create)
                        {
                            var serTempModel = new ServiceTemplateViewModel();
                            serTempModel.DataAction = model.DataAction;
                            serTempModel.ActiveUserId = _userContext.UserId;
                            serTempModel.TemplateCode = "IMS_STOCK_TRANSFER";

                            var sermodel = await _serviceBusinness.GetServiceDetails(serTempModel);
                            sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                            sermodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                            sermodel.DataAction = DataActionEnum.Create;
                            var result = await _serviceBusinness.ManageService(sermodel);

                            return Json(new { success = result.IsSuccess, data = result.Item.UdfNoteTableId });
                        }
                    }
                }
                else
                {
                    var service = await _serviceBusinness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = model.ServiceId });
                    var noteTempModel1 = new NoteTemplateViewModel();
                    noteTempModel1.SetUdfValue = true;
                    noteTempModel1.NoteId = service.UdfNoteId;
                    var notemodel1 = await _noteBusinness.GetNoteDetails(noteTempModel1);
                    var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                    rowData["ChallanNo"] = model.ChallanNo;
                    rowData["TransferDate"] = model.TransferDate;
                    rowData["TransferReason"] = model.TransferReason;

                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                    var update = await _noteBusinness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);
                    if (update.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public async Task<IActionResult> CreateTransferItems(string stId, string itemId, string transQty)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Create;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "IMS_STOCK_TRANSFER_ITEM";

            var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);
            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("StockTransferId", stId);
            ((IDictionary<String, Object>)exo).Add("ItemId", itemId);
            ((IDictionary<String, Object>)exo).Add("TransferQuantity", transQty);

            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            //notemodel.ParentServiceId = result.Item.ServiceId;
            notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            notemodel.DataAction = DataActionEnum.Create;
            var result = await _noteBusinness.ManageNote(notemodel);

            return Json(new { success = result.IsSuccess });

        }

        public async Task<IActionResult> ReadTransferItemsList(string stockTransferId)
        {
            var data = await _inventoryManagementBusinness.GetTransferItemsList(stockTransferId);
            return Json(data);
        }

        public async Task<IActionResult> CheckStocTransferItemsExist(string stockTransferId)
        {
            var data = await _inventoryManagementBusinness.GetTransferItemsList(stockTransferId);
            if (data.Count > 0)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStockTransferItem(string itemNoteId)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.SetUdfValue = true;
            noteTempModel.NoteId = itemNoteId;
            var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

            var res = await _noteBusinness.DeleteNote(notemodel);
            return Json(new { success = res.IsSuccess });
        }

        public async Task<IActionResult> SubmitStockTransfer(string serId)
        {
            var service = await _serviceBusinness.GetSingle(x => x.Id == serId);
            var serTempModel = new ServiceTemplateViewModel();
            serTempModel.DataAction = DataActionEnum.Edit;
            serTempModel.ActiveUserId = _userContext.UserId;
            serTempModel.ServiceId = service.Id;
            serTempModel.AllowPastStartDate = true;
            var sermodel = await _serviceBusinness.GetServiceDetails(serTempModel);
            var existing = await _inventoryManagementBusinness.GetItemTransferredList("", "", "");
            var existdata = existing.Where(x => x.ServiceId == serId).FirstOrDefault();

            if (existdata != null)
            {
                sermodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existdata);
                var result = await _serviceBusinness.ManageService(sermodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> SubmitStockTransferReceipt(string serId)
        {
            var service = await _serviceBusinness.GetSingle(x => x.Id == serId);
            var serTempModel = new ServiceTemplateViewModel();
            serTempModel.DataAction = DataActionEnum.Edit;
            serTempModel.ActiveUserId = _userContext.UserId;
            serTempModel.ServiceId = service.Id;
            serTempModel.AllowPastStartDate = true;
            var notemodel = await _serviceBusinness.GetServiceDetails(serTempModel);
            var existing = await _inventoryManagementBusinness.GetGoodsReceiptDataBySerId(serId);
            if (existing != null)
            {
                notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _serviceBusinness.ManageService(notemodel);
                if (result.IsSuccess)
                {
                    var items = await _inventoryManagementBusinness.GetGoodReceiptItemsList(result.Item.UdfNoteTableId);
                    if (items != null && items.Count() > 0)
                    {
                        foreach (var data in items)
                        {
                            await GetSerialNos(result.Item.UdfNoteTableId, data.Id);
                            var closingBalance = await _inventoryManagementBusinness.GetClosingBalance(data.ItemId, data.WarehouseId);
                            await _inventoryManagementBusinness.UpdateStockClosingBalance(data.ItemId, data.WarehouseId, closingBalance);
                           
                        }
                    }
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }


        public async Task<IActionResult> ManageStockTransferReceiveItem(string grSerId, string receiptStatus, string stId, string toWarehouseId)
        {
            var model = new GoodsReceiptViewModel();
            var data = await _inventoryManagementBusinness.GetGoodsReceiptDataBySerId(grSerId);
            if (data.IsNotNull())
            {
                model = data;
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.WarehouseId = toWarehouseId;
                model.ReceiptType = ImsReceiptTypeEnum.StockTransfer;
            }

            model.ReceiptStatus = receiptStatus;
            model.ServiceId = grSerId;
            model.GoodsReceiptReferenceId = stId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageStockTransferReceiveItem(GoodsReceiptViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var serTempModel = new ServiceTemplateViewModel();
                serTempModel.DataAction = model.DataAction;
                serTempModel.ActiveUserId = _userContext.UserId;
                serTempModel.TemplateCode = "IMS_GOODS_RECEIPT";
                var sermodel = await _serviceBusinness.GetServiceDetails(serTempModel);
                sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                sermodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                sermodel.DataAction = DataActionEnum.Create;
                var result = await _serviceBusinness.ManageService(sermodel);
                if (result.IsSuccess)
                {
                    var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GoodsReceiptItemViewModel>>(model.POItemsData);
                        if (items.IsNotNull() && items.Count() > 0)
                        {
                            foreach (var data in items)
                            {
                                var noteTempModel = new NoteTemplateViewModel();
                                noteTempModel.DataAction = DataActionEnum.Create;
                                noteTempModel.ActiveUserId = _userContext.UserId;
                                noteTempModel.TemplateCode = "IMS_GOODS_RECEIPT_ITEM";
                                var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);
                                dynamic exo = new System.Dynamic.ExpandoObject();
                                //var goodreceipt = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == result.Item.UdfNoteTableId);
                                var po = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == model.GoodsReceiptReferenceId);
                                ((IDictionary<String, Object>)exo).Add("GoodReceiptId", result.Item.UdfNoteTableId);
                                ((IDictionary<String, Object>)exo).Add("ItemQuantity", data.IssuedQuantity);
                                ((IDictionary<String, Object>)exo).Add("BalanceQuantity", data.IssuedQuantity);
                                ((IDictionary<String, Object>)exo).Add("ItemId", data.ItemId);
                                ((IDictionary<String, Object>)exo).Add("WarehouseId", model.WarehouseId);
                                ((IDictionary<String, Object>)exo).Add("ReferenceHeaderItemId", model.GoodsReceiptReferenceId);
                                var user = await _noteBusinness.GetSingleById<UserViewModel, User>(_userContext.UserId);
                                ((IDictionary<String, Object>)exo).Add("AdditionalInfo", "<" + result.Item.ServiceNo + "> received against stock transfer: <" + po.ServiceNo + "> By <" + user.Name + "> on <" + model.ReceiveDate + ">");

                                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                notemodel.DataAction = DataActionEnum.Create;
                                var result1 = await _noteBusinness.ManageNote(notemodel);                         
                        }
                        }
                    
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            else
            {
                var noteTempModel = new ServiceTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.ServiceId = model.ServiceId;
                var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);

                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var result = await _serviceBusinness.ManageService(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            
        }

        public async Task<IActionResult> GetRequisitiononFilters(string ItemHead, string From, string To)
        {
            var list = await _inventoryManagementBusinness.GetRequisitiononFilters(ItemHead, From, To);
            return Json(list);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitDeliveryNote(string serviceId)
        {

            var noteTempModel = new ServiceTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.ServiceId = serviceId;
            noteTempModel.AllowPastStartDate = true;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
            var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

            notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
            notemodel.DueDate = DateTime.Now;
            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            var result = await _serviceBusinness.ManageService(notemodel);
            if (result.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = result.Messages.ToHtmlError() });
        }
        [HttpPost]
        public async Task<IActionResult> ManageSerialNo(SerialNoViewModel model)
        {
            //  if (data.SerialNosData.IsNotNullAndNotEmpty()) 
            // { 
            // var item= Newtonsoft.Json.JsonConvert.DeserializeObject<List<SerialNoViewModel>>(data.SerialNosData);
            //  foreach (var model in item) 
            //  {
            if (model.DataAction == DataActionEnum.Create)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "IMS_ITEMS_SERIALNO";
                var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("ReferenceHeaderId", model.ReferenceHeaderId);
                ((IDictionary<String, Object>)exo).Add("ReferenceId", model.ReferenceId);
                ((IDictionary<String, Object>)exo).Add("SerialNo", model.SerialNo);
                ((IDictionary<String, Object>)exo).Add("Specification", model.Specification);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.DataAction = DataActionEnum.Create;
                var result = await _noteBusinness.ManageNote(notemodel);
            }
            else
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.NoteId;
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

                var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                rowData["SerialNo"] = model.SerialNo;
                rowData["Specification"] = model.Specification;
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                var update = await _noteBusinness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                return Json(new { success = update.IsSuccess });
            }
            return Json(new { success = false, error = "Please provide the details" });
        }

        [HttpPost]
        public async Task<IActionResult> ApprovedRequisitionItems(string RequisitionItemsData)
        {
            if (RequisitionItemsData.IsNotNullAndNotEmpty())
            {
                var item = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemsViewModel>>(RequisitionItemsData);
                foreach (var model in item)
                {

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.NoteId = model.NoteId;
                    noteTempModel.SetUdfValue = true;
                    var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

                    var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                    rowData["IsApproved"] = true;
                    rowData["ApprovedQuantity"] = model.ApprovedQuantity;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                    var update = await _noteBusinness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                    //return Json(new { success = update.IsSuccess });
                }
                return Json(new { success = true });
            }
            return Json(new { success = false, error = "Please provide the details" });
        }

        [HttpPost]
        public async Task<IActionResult> SaveSerialNo(string SerialNosData)
        {
            if (SerialNosData.IsNotNullAndNotEmpty())
            {
                var item = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SerialNoViewModel>>(SerialNosData);
                foreach (var model in item)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.NoteId = model.NoteId;
                    noteTempModel.SetUdfValue = true;
                    var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

                    var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                    rowData["SerialNo"] = model.SerialNo;
                    rowData["Specification"] = model.Specification;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                    var update = await _noteBusinness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                }
                return Json(new { success = true });
            }
            return Json(new { success = false, error = "Please provide the details" });
        }

        public async Task<IActionResult> GetSerialNo(string id, string referenceId)
        {
            SerialNoViewModel model = new SerialNoViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                List<SerialNoViewModel> list = new List<SerialNoViewModel>();
                var data = await _tableMetadataBusiness.GetTableDataByColumn("IMS_GOODS_RECEIPT_ITEM", "", "Id", referenceId);
                if (data != null)
                {
                    if (Convert.ToInt64(data["ItemQuantity"]) > 0)
                    {
                        for (var i = 0; i < Convert.ToInt64(data["ItemQuantity"]); i++)
                        {
                            SerialNoViewModel item = new SerialNoViewModel();
                            item.Id = Guid.NewGuid().ToString();
                            item.SerialNo = i.ToString();
                            item.Specification = i.ToString();
                            item.ReferenceId = referenceId;
                            item.DataAction = DataActionEnum.Create;
                            list.Add(item);
                        }
                    }

                }
                else
                {
                    // check for item stock
                    var stockdata = await _tableMetadataBusiness.GetTableDataByColumn("ITEM_STOCK", "", "Id", referenceId);
                    if (stockdata != null)
                    {
                        if (Convert.ToInt64(data["ItemQuantity"]) > 0)
                        {
                            for (var i = 0; i < Convert.ToInt64(data["ItemQuantity"]); i++)
                            {
                                SerialNoViewModel item = new SerialNoViewModel();
                                item.Id = Guid.NewGuid().ToString();
                                item.SerialNo = i.ToString();
                                item.Specification = i.ToString();
                                item.ReferenceId = referenceId;
                                item.ReferenceHeaderId = referenceId;
                                item.DataAction = DataActionEnum.Create;
                                list.Add(item);
                            }
                        }
                    }
                }
                model.DataAction = DataActionEnum.Create;
                model.SerialNos = list;
            }
            return View(model);
        }
        public async Task<IActionResult> ValidateStockTransferItem(string stId, string itemId)
        {
            var data = await _inventoryManagementBusinness.GetTransferItemsList(stId);
            var exist = data.Where(x => x.ItemId == itemId).ToList();
            if (exist.Count > 0)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public IActionResult VendorCategory()
        {
            return View();
        }

        public async Task<IActionResult> ReadVendorCategoryData(string venId, string catId, string subCatId)
        {
            var data = await _inventoryManagementBusinness.ReadVendorCategoryData(venId, catId,subCatId);
            return Json(data);
        }

        public IActionResult ReturnToVendor()
        {
            return View();
        }
        public async Task<IActionResult> ReadReturnToVendorData(string venId, DateTime From, DateTime To)
        {
            //var data = new List<GoodsReceiptItemViewModel>();

            //var list1 = new GoodsReceiptItemViewModel()
            //{
            //    ItemName = "HCL ME ICON AE1V3113-I LAPTOP",
            //    SNo = 202201,
            //    PONumber = "XTPL/PO/2021-2022/77",
            //    ReceivedOn = new DateTime().Date,
            //    ReceivedQuantity = 3,
            //    ReturnedTo = "Computer world",
            //    ReturnedOn = new DateTime().Date,
            //    ReturnedToVendorQuantity = 2,
            //    Description = "ReturnedToVendor By Admin vide IssueNote#:67"
            //};

            //data.Add(list1);
            var data = await _inventoryManagementBusinness.ReadReturnToVendorData(From, To, venId);
            return Json(data);
        }

        public IActionResult RequisitionStatusAndDetail()
        {
            return View();
        }
        public IActionResult ManageSerialNo(string referenceId, string referenceHeaderId)
        {
            var model = new SerialNoViewModel()
            {
                PortalId = _userContext.PortalId
            };
            ViewBag.ReferenceId = referenceId;
            ViewBag.ReferenceHeaderId = referenceHeaderId;
            return View(model);
        }
        public async Task<IActionResult> ReadRequisitionByStatusData(string cusId, string typeId, string statusId, DateTime From, DateTime To)
        {
            //var data = new RequisitionViewModel()
            //{
            //    RequisitionNo = 12345,
            //    RequisitionDate = DateTime.Now.Date,
            //    RequisitionType = "External",
            //    CustomerName = "Anand Tyre",
            //    OrderValue = "20",
            //    RequisitionParticular = "Particular",
            //    StorageType = "Stock",
            //    RequisitionStatus = "Completed"
            //};
            var data = await _inventoryManagementBusinness.ReadRequisitionByStatusData(From, To, typeId,cusId, statusId);
            return Json(data);
        }

        public IActionResult RequisitionDetailReport()
        {
            return View();
        }

        public async Task<IActionResult> ReadRequisitionByDetailsData(string cusId, string typeId, string statusId, DateTime From, DateTime To)
        {
            //var data = new RequisitionViewModel()
            //{
            //    RequisitionNo = 12345,
            //    RequisitionDate = DateTime.Now.Date,
            //    RequisitionType = "External",
            //    CustomerName = "Anand Tyre",
            //    ItemName = "LAYS 100 GM",
            //    RequisitionQuantity = 10,
            //    StorageType = "Stock",
            //    RequisitionStatus = "Completed"
            //};
            var data = await _inventoryManagementBusinness.ReadRequisitionByDetailsData(From, To, typeId, cusId, statusId);
            return Json(data);
        }

        public IActionResult ReceivedFromPO()
        {
            return View();
        }
        public async Task<IActionResult> ReadReceivedFromPOData(string venId, DateTime From, DateTime To)
        {
            //var data = new GoodsReceiptItemViewModel()
            //{

            //};
            var data = await _inventoryManagementBusinness.ReadReceivedFromPOData(From, To, venId);
            return Json(data);
        }

        public IActionResult SalesReturn()
        {
            return View();
        }

        public async Task<IActionResult> ReadSalesReturnList(string cusId, string From, string To, string serNo)
        {
            var data = await _inventoryManagementBusinness.GetSalesReturnList(cusId, From, To, serNo);
            return Json(data);
        }

        public async Task<IActionResult> NewSalesReturn(string serId, string status)
        {
            var model = new SalesReturnViewModel() { DataAction = DataActionEnum.Create };

            if (serId.IsNotNullAndNotEmpty())
            {
                var data = await _inventoryManagementBusinness.GetSalesReturnData(serId);
                data.DataAction = DataActionEnum.Edit;
                return View(data);
            }
            else
            {
                return View(model);
            }

        }

        //public async Task<IActionResult> NewPurchaseReturn(string serId, string status)
        //{
        //    var model = new PurchaseReturnViewModel() { DataAction = DataActionEnum.Create};

        //    //if (serId.IsNotNullAndNotEmpty())
        //    //{
        //    //    var data = await _inventoryManagementBusinness.GetSalesReturnData(serId);
        //    //    data.DataAction = DataActionEnum.Edit;                              
        //    //    return View(data);
        //    //}
        //    //else
        //    //{
        //    //    return View(model);
        //    //}
        //    return View(model);
        //}

        public async Task<IActionResult> ReadDirectSalesList()
        {
            var data = await _inventoryManagementBusinness.GetDirectSalesList();
            return Json(data);
        }

        public async Task<IActionResult> ReadPurchaseOrderList()
        {
            var data = await _inventoryManagementBusinness.GetPurchaseOrderList();
            return Json(data);
        }

        public async Task<IActionResult> GetDirectSalesData(string serviceId)
        {
            var data = await _inventoryManagementBusinness.GetDirectSalesData(serviceId);
            return Json(data);
        }

        public async Task<IActionResult> GetPurchaseOrderData(string serviceId)
        {
            var data = await _inventoryManagementBusinness.GetPurchaseOrderData(serviceId);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> ManageSalesReturn(SalesReturnViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var serTempModel = new ServiceTemplateViewModel();
                serTempModel.DataAction = model.DataAction;
                serTempModel.ActiveUserId = _userContext.UserId;
                serTempModel.TemplateCode = "IMS_SALES_RETURN";
                var sermodel = await _serviceBusinness.GetServiceDetails(serTempModel);
                sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                sermodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                sermodel.DataAction = DataActionEnum.Create;
                var result = await _serviceBusinness.ManageService(sermodel);

                if (result.IsSuccess)
                {
                    //return Json(new { success = result.IsSuccess, data = result.Item.UdfNoteTableId});
                    var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SalesReturnViewModel>>(model.ReturnItems);
                    foreach (var item in list)
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.TemplateCode = "IMS_SALES_RETURN_ITEM";
                        noteTempModel.SetUdfValue = true;
                        var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

                        dynamic exo = new System.Dynamic.ExpandoObject();

                        ((IDictionary<String, Object>)exo).Add("SalesReturnId", result.Item.UdfNoteTableId);
                        ((IDictionary<String, Object>)exo).Add("DirectSaleItemId", item.Id);
                        ((IDictionary<String, Object>)exo).Add("ItemId", item.Item);
                        ((IDictionary<String, Object>)exo).Add("SalesQuantity", item.ItemQuantity);
                        ((IDictionary<String, Object>)exo).Add("ReturnQuantity", item.ReturnQuantity);
                        ((IDictionary<String, Object>)exo).Add("ReturnTypeId", item.ReturnTypeId);
                        ((IDictionary<String, Object>)exo).Add("ReturnReason", item.ReturnReason);

                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                        var result1 = await _noteBusinness.ManageNote(notemodel);
                    }
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = result.IsSuccess, error = result.Messages });
                }
            }
            else
            {
                var serTempModel = new ServiceTemplateViewModel();
                serTempModel.DataAction = model.DataAction;
                serTempModel.ActiveUserId = _userContext.UserId;
                serTempModel.ServiceId = model.ServiceId;
                serTempModel.AllowPastStartDate = true;
                serTempModel.SetUdfValue = true;
                var sermodel = await _serviceBusinness.GetServiceDetails(serTempModel);

                var rowData = sermodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                rowData["ReturnDate"] = model.ReturnDate;
                rowData["ReturnReason"] = model.ReturnReason;

                sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);

                var result = await _serviceBusinness.ManageService(sermodel);

                if (result.IsSuccess)
                {
                    var list = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SalesReturnViewModel>>(model.ReturnItems);
                    var returnItems = await _inventoryManagementBusinness.GetSalesReturnItemsList(model.SalesReturnId);

                    var newItems = list.Where(x => returnItems.All(y => y.DirectSaleItemId != x.Id)).ToList();
                    var updateItems = list.Where(x => returnItems.Any(y => y.DirectSaleItemId == x.Id && (y.ReturnQuantity != x.ReturnQuantity || y.ReturnTypeId != x.ReturnTypeId || y.ReturnReason != x.ReturnReason))).ToList();
                    var deleteItems = returnItems.Where(x => list.All(y => y.Id != x.DirectSaleItemId)).ToList();

                    if (newItems.Count > 0)
                    {
                        foreach (var item in newItems)
                        {
                            var noteTempModel = new NoteTemplateViewModel();
                            noteTempModel.DataAction = DataActionEnum.Create;
                            noteTempModel.ActiveUserId = _userContext.UserId;
                            noteTempModel.TemplateCode = "IMS_SALES_RETURN_ITEM";
                            noteTempModel.SetUdfValue = true;
                            var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

                            dynamic exo = new System.Dynamic.ExpandoObject();

                            ((IDictionary<String, Object>)exo).Add("SalesReturnId", model.SalesReturnId);
                            ((IDictionary<String, Object>)exo).Add("DirectSaleItemId", item.Id);
                            ((IDictionary<String, Object>)exo).Add("ItemId", item.Item);
                            ((IDictionary<String, Object>)exo).Add("SalesQuantity", item.ItemQuantity);
                            ((IDictionary<String, Object>)exo).Add("ReturnQuantity", item.ReturnQuantity);
                            ((IDictionary<String, Object>)exo).Add("ReturnTypeId", item.ReturnTypeId);
                            ((IDictionary<String, Object>)exo).Add("ReturnReason", item.ReturnReason);

                            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                            notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                            var result1 = await _noteBusinness.ManageNote(notemodel);
                        }
                    }
                    if (updateItems.Count > 0)
                    {
                        foreach (var item in updateItems)
                        {
                            var noteTempModel = new NoteTemplateViewModel();
                            noteTempModel.DataAction = DataActionEnum.Edit;
                            noteTempModel.ActiveUserId = _userContext.UserId;
                            noteTempModel.NoteId = item.NtsNoteId;
                            noteTempModel.SetUdfValue = true;
                            var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

                            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                            rowData1["ReturnQuantity"] = item.ReturnQuantity;
                            rowData1["ReturnTypeId"] = item.ReturnTypeId;
                            rowData1["ReturnReason"] = item.ReturnReason;
                            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                            var update = await _noteBusinness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                        }
                    }
                    if (deleteItems.Count > 0)
                    {
                        foreach (var item in deleteItems)
                        {
                            var noteTempModel = new NoteTemplateViewModel();
                            noteTempModel.SetUdfValue = true;
                            noteTempModel.NoteId = item.NtsNoteId;
                            var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

                            var res = await _noteBusinness.DeleteNote(notemodel);
                        }
                    }
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });

        }
        [HttpGet]
        public async Task<IActionResult> GetDirectSalesItemsList(string directSalesId)
        {
            var list = await _inventoryManagementBusinness.GetDirectSaleItemsData(directSalesId);
            return Json(list);

        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrderItemsList(string purchaseId)
        {
            var list = await _inventoryManagementBusinness.GetPurchaseOrderItemsList(purchaseId);
            return Json(list);

        }

        [HttpPost]
        public async Task<IActionResult> SubmitSalesReturn(string serId)
        {
            var service = await _serviceBusinness.GetSingle(x => x.Id == serId);
            var serTempModel = new ServiceTemplateViewModel();
            serTempModel.DataAction = DataActionEnum.Edit;
            serTempModel.ActiveUserId = _userContext.UserId;
            serTempModel.ServiceId = service.Id;
            serTempModel.AllowPastStartDate = true;
            var sermodel = await _serviceBusinness.GetServiceDetails(serTempModel);
            var existing = await _inventoryManagementBusinness.GetSalesReturnData(serId);
            if (existing != null)
            {
                sermodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _serviceBusinness.ManageService(sermodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> ManageSalesReturnReceiveItem(string goodsReceiptReferenceId, string goodsReceiptId, ImsReceiptTypeEnum receiptType, string grSerId, string receiptStatus = null)
        {
            var model = await _inventoryManagementBusinness.GetGoodsReceiptDataBySerId(grSerId);

            model.DataAction = DataActionEnum.Edit;
            model.GoodsReceiptReferenceId = goodsReceiptReferenceId;
            model.GoodsReceiptId = goodsReceiptId;
            model.ReceiptType = receiptType;
            model.ReceiptStatus = receiptStatus;
            model.ServiceId = grSerId;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageSalesReturnReceiveItem(GoodsReceiptViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Edit)
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.ServiceId = model.ServiceId;
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);

                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _serviceBusinness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {

                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitSalesReturnReceiveItem(string serId)
        {
            var service = await _serviceBusinness.GetSingle(x => x.Id == serId);
            var serTempModel = new ServiceTemplateViewModel();
            serTempModel.DataAction = DataActionEnum.Edit;
            serTempModel.ActiveUserId = _userContext.UserId;
            serTempModel.ServiceId = service.Id;
            serTempModel.AllowPastStartDate = true;
            var notemodel = await _serviceBusinness.GetServiceDetails(serTempModel);
            var existing = await _inventoryManagementBusinness.GetGoodsReceiptDataBySerId(serId);
            if (existing != null)
            {
                notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _serviceBusinness.ManageService(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }

        
        [HttpPost]
        public async Task<IActionResult> SubmitPurchaseReturnReceiveItem(string serId)
        {
            var service = await _serviceBusinness.GetSingle(x => x.Id == serId);
            var serTempModel = new ServiceTemplateViewModel();
            serTempModel.DataAction = DataActionEnum.Edit;
            serTempModel.ActiveUserId = _userContext.UserId;
            serTempModel.ServiceId = service.Id;
            serTempModel.AllowPastStartDate = true;
            var notemodel = await _serviceBusinness.GetServiceDetails(serTempModel);
            var existing = await _inventoryManagementBusinness.GetGoodsReceiptDataBySerId(serId);
            if (existing != null)
            {
                notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _serviceBusinness.ManageService(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }


        public IActionResult PurchaseReturn()
        {
            return View();
        }

        public async Task<IActionResult> ReadPurchaseReturnList(string cusId, string From, string To, string serNo)
        {
            var data = await _inventoryManagementBusinness.GetPurchaseReturnList(cusId, From, To, serNo);
            return Json(data);
        }
        public async Task<IActionResult> ReadPurchaseReturnItems(string serviceId)
        {
            var data = await _inventoryManagementBusinness.GetPurchaseReturnItemsData(serviceId);
            return Json(data);
        }

        public async Task<IActionResult> NewPurchaseReturn(string serId, string status)
        {
            var model = new PurchaseReturnViewModel() { 
                DataAction = DataActionEnum.Create 
            };

            if (serId.IsNotNullAndNotEmpty())
            {
                var data = await _inventoryManagementBusinness.GetPurchaseReturnData(serId);
                data.DataAction = DataActionEnum.Edit;
                
                data.PurchaseOrderServiceId = data.POId;

                return View(data);
            }
            else
            {
                return View(model);
            }

        }

        //public async Task<IActionResult> ReadDirectSalesList()
        //{
        //    var data = await _inventoryManagementBusinness.GetDirectSalesList();
        //    return Json(data);
        //}

        //public async Task<IActionResult> GetDirectSalesData(string serviceId)
        //{
        //    var data = await _inventoryManagementBusinness.GetDirectSalesData(serviceId);
        //    return Json(data);
        //}

        public async Task<IActionResult> ManagePurchaseReturn(PurchaseReturnViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var data = await _inventoryManagementBusinness.GetPurchaseOrderData(model.PurchaseOrderServiceId);
                if (data.IsNotNull())
                {
                    model.VendorId = data.VendorId;
                    model.VendorContactId = data.ContactPersonId;
                }
                var serTempModel = new ServiceTemplateViewModel
                {
                    DataAction = model.DataAction,
                    ActiveUserId = _userContext.UserId,
                    TemplateCode = "PURCHASE_RETURN"
                };
                var sermodel = await _serviceBusinness.GetServiceDetails(serTempModel);
                sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                sermodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                sermodel.DataAction = DataActionEnum.Create;
                var result = await _serviceBusinness.ManageService(sermodel);

                if (result.IsSuccess)
                {
                    //return Json(new { success = result.IsSuccess, data = result.Item.UdfNoteTableId});
                    var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SalesReturnViewModel>>(model.ReturnItems);
                    foreach (var item in list)
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.TemplateCode = "PURCHASE_RETURN_ITEM";
                        noteTempModel.SetUdfValue = true;
                        var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

                        dynamic exo = new System.Dynamic.ExpandoObject();

                        ((IDictionary<String, Object>)exo).Add("PurchaseReturnId", result.Item.UdfNoteTableId);
                        ((IDictionary<String, Object>)exo).Add("POItemId", item.Id);
                        ((IDictionary<String, Object>)exo).Add("ItemId", item.ItemId);
                        ((IDictionary<String, Object>)exo).Add("PurchaseQuantity", item.ItemQuantity);
                        ((IDictionary<String, Object>)exo).Add("ReturnQuantity", item.ReturnQuantity);
                        ((IDictionary<String, Object>)exo).Add("ReturnType", item.ReturnTypeId);
                        ((IDictionary<String, Object>)exo).Add("ReturnComment", item.ReturnReason);

                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                        var result1 = await _noteBusinness.ManageNote(notemodel);
                    }
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = result.IsSuccess, error = result.Messages });
                }
            }
            else
            {
                var serTempModel = new ServiceTemplateViewModel();
                serTempModel.DataAction = model.DataAction;
                serTempModel.ActiveUserId = _userContext.UserId;
                serTempModel.ServiceId = model.ServiceId;
                serTempModel.AllowPastStartDate = true;
                serTempModel.SetUdfValue = true;
                var sermodel = await _serviceBusinness.GetServiceDetails(serTempModel);

                var rowData = sermodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                rowData["ReturnDate"] = model.ReturnDate;
                rowData["ReturnReason"] = model.ReturnReason;

                sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);

                var result = await _serviceBusinness.ManageService(sermodel);

                if (result.IsSuccess)
                {
                    var list = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SalesReturnViewModel>>(model.ReturnItems);
                    var returnItems = await _inventoryManagementBusinness.GetPurchaseReturnItemsList(model.PurchaseReturnId);

                    var newItems = list.Where(x => returnItems.All(y => y.PurchaseItemId != x.Id)).ToList();
                    var updateItems = list.Where(x => returnItems.Any(y => y.PurchaseItemId == x.Id && (y.ReturnQuantity != Double.Parse(x.ReturnQuantity) || y.ReturnTypeId != x.ReturnTypeId || y.ReturnReason != x.ReturnReason))).ToList();
                    var deleteItems = returnItems.Where(x => list.All(y => y.Id != x.PurchaseItemId)).ToList();

                    if (newItems.Count > 0)
                    {
                        foreach (var item in newItems)
                        {
                            var noteTempModel = new NoteTemplateViewModel();
                            noteTempModel.DataAction = DataActionEnum.Create;
                            noteTempModel.ActiveUserId = _userContext.UserId;
                            noteTempModel.TemplateCode = "PURCHASE_RETURN_ITEM";
                            noteTempModel.SetUdfValue = true;
                            var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

                            dynamic exo = new System.Dynamic.ExpandoObject();

                            ((IDictionary<String, Object>)exo).Add("PurchaseReturnId", result.Item.UdfNoteTableId);
                            ((IDictionary<String, Object>)exo).Add("POItemId", item.Id);
                            ((IDictionary<String, Object>)exo).Add("ItemId", item.ItemId);
                            ((IDictionary<String, Object>)exo).Add("PurchaseQuantity", item.ItemQuantity);
                            ((IDictionary<String, Object>)exo).Add("ReturnQuantity", item.ReturnQuantity);
                            ((IDictionary<String, Object>)exo).Add("ReturnType", item.ReturnTypeId);
                            ((IDictionary<String, Object>)exo).Add("ReturnComment", item.ReturnReason);

                            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                            notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                            var result1 = await _noteBusinness.ManageNote(notemodel);
                        }
                    }
                    if (updateItems.Count > 0)
                    {
                        foreach (var item in updateItems)
                        {
                            var noteTempModel = new NoteTemplateViewModel();
                            noteTempModel.DataAction = DataActionEnum.Edit;
                            noteTempModel.ActiveUserId = _userContext.UserId;
                            noteTempModel.NoteId = item.NtsNoteId;
                            noteTempModel.SetUdfValue = true;
                            var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

                            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                            rowData1["ReturnQuantity"] = item.ReturnQuantity;
                            rowData1["ReturnType"] = item.ReturnTypeId;
                            rowData1["ReturnComment"] = item.ReturnReason;
                            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                            var update = await _noteBusinness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                        }
                    }
                    if (deleteItems.Count > 0)
                    {
                        foreach (var item in deleteItems)
                        {
                            var noteTempModel = new NoteTemplateViewModel();
                            noteTempModel.SetUdfValue = true;
                            noteTempModel.NoteId = item.NtsNoteId;
                            var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

                            var res = await _noteBusinness.DeleteNote(notemodel);
                        }
                    }
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });

        }

        [HttpPost]
        public async Task<IActionResult> SubmitPurchaseReturn(string serId)
        {
           // var service = await _serviceBusinness.GetSingle(x => x.Id == serId);
            var serTempModel = new ServiceTemplateViewModel();
            serTempModel.DataAction = DataActionEnum.Edit;
            serTempModel.ActiveUserId = _userContext.UserId;
            serTempModel.ServiceId = serId;
            serTempModel.AllowPastStartDate = true;
            var notemodel = await _serviceBusinness.GetServiceDetails(serTempModel);
            var existing = await _inventoryManagementBusinness.GetPurchaseReturnData(serId);
            if (existing != null)
            {
                notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _serviceBusinness.ManageService(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> ManageSalesReturnItems(SalesReturnViewModel model)
        {

            return Json(new { success = true });
        }
        [HttpGet]
        public async Task<IActionResult> StockAdjustmentIndex()
        {
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> ManageStockAdjustment(string id)
        {
            StockAdjustmentViewModel model = new StockAdjustmentViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                var service = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == id);
                model = await _inventoryManagementBusinness.GetStockAdjustmentById(id);
                model.DataAction = DataActionEnum.Edit;
                model.ServiceId = service.Id;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }
            return View(model);

        }
        public async Task<IActionResult> ReadStockAdjustmentList()
        {
            var data = await _inventoryManagementBusinness.GetStockAdjustmentList();
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> ManageStockAdjustment(StockAdjustmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "STOCK_ADJUSTMENT";
                    //noteTempModel.ParentNoteId = model.ParentNoteId;
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _serviceBusinness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {

                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.ServiceId = model.ServiceId;
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);

                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _serviceBusinness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });


                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public async Task<IActionResult> AddStockAdjustmentItems(string stockAdjustmentId)
        {
            StockAdjustmentItemViewModel model = new StockAdjustmentItemViewModel();
            model.StockAdjustmentId = stockAdjustmentId;
            var serviceData = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == stockAdjustmentId);
            var lov = await _lovBusiness.GetSingle(x => x.Id == serviceData.ServiceStatusId);
            model.ServiceStatusCode = lov.Code;
            model.DataAction = DataActionEnum.Create;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageStockAdjustmentItems(StockAdjustmentItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "STOCK_ADJUSTMENT_ITEM";
                    var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _noteBusinness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {
                    var existing = await _tableMetadataBusiness.GetTableDataByColumn("STOCK_ADJUSTMENT_ITEM", null, "NtsNoteId", model.NoteId);
                    if (existing != null)
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Edit;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.NoteId = model.NoteId;
                        var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        var result = await _noteBusinness.ManageNote(notemodel);
                        if (result.IsSuccess)
                        {
                            return Json(new { success = true });
                        }
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }

                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public async Task<IActionResult> ReadStockAdjustmentItemsData(string stockAdjustmentId)
        {

            var list = await _inventoryManagementBusinness.GetStockAdjustmentItemsData(stockAdjustmentId);
            return Json(list);
        }
        public async Task<IActionResult> CheckStockAdjustmentItemsExist(string stockAdjustmentId)
        {
            var requisitionitems = await _inventoryManagementBusinness.GetStockAdjustmentItemsData(stockAdjustmentId);
            if (requisitionitems != null && requisitionitems.Count > 0)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public async Task<IActionResult> SubmitService(string UdfNoteTableId)
        {
            var service = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == UdfNoteTableId);
            var noteTempModel = new ServiceTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.ServiceId = service.Id;
            noteTempModel.AllowPastStartDate = true;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
            var existing = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            if (existing != null)
            {
                notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _serviceBusinness.ManageService(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }

        public IActionResult StockAdjustmentIssues(string stockAdjustmentId, string warehouseId)
        {

            ViewBag.StockAdjustmentId = stockAdjustmentId;
            ViewBag.WarehouseId = warehouseId;
            IssueRequisitionViewModel model = new IssueRequisitionViewModel();
            model.IssueReferenceId = stockAdjustmentId;
            model.IssueReferenceType = ImsIssueTypeEnum.StockAdjustment;
            model.WarehouseId = warehouseId;
            model.DataAction = DataActionEnum.Create;
            return View(model);
        }
        public IActionResult ReceiveStockAdjustment(string stockAdjustmentId)
        {
            ViewBag.StockAdjustmentId = stockAdjustmentId;

            return View();
        }
        public async Task<IActionResult> DeliveryNoteAcknowledgement(string deliveryNoteId)
        {
            ViewBag.DeliveryNoteId = deliveryNoteId;
            var deliveryNote = await _inventoryManagementBusinness.GetDeliveryNoteById(deliveryNoteId);
            if (deliveryNote.IsNotNull())
            {
                ViewBag.FileId = deliveryNote.AccknowledgeFileId;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDeliveryNoteAcknowledgement(string deliveryNoteId,string fileId)
        {
            await _inventoryManagementBusinness.UpdateDeliveryNoteAcknowledgement(deliveryNoteId, fileId);

            return Json(new { success=true});
        }       
        public IActionResult ManageStockTransferIssue(string stockTransferId, string warehouseId)
        {          
          
            IssueRequisitionViewModel model = new IssueRequisitionViewModel();
            model.IssueReferenceId = stockTransferId;
            model.IssueReferenceType = ImsIssueTypeEnum.StockTransfer;
            model.WarehouseId = warehouseId;
            model.DataAction = DataActionEnum.Create;
            return View(model);
        }

        public async Task<IActionResult> ReadScheduleInvoice(string customerId)
        {
            var data= await _inventoryManagementBusinness.ReadScheduleInvoice(customerId);
            return Json(data);
        }
    }
}
