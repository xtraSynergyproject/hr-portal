using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Inv.Controllers
{
    [Area("INV")]
    public class InventoryController : ApplicationController
    {
        private IServiceBusiness _serviceBusinness;
        private IInventoryManagementBusiness _inventoryManagementBusinness;
        private INoteBusiness _noteBusinness;
        private IUserContext _userContext;
        private ITableMetadataBusiness _tableMetadataBusiness;
        private ILOVBusiness _lovBusiness;
        private ICmsBusiness _cmsBusiness;
        private readonly IPortalBusiness _portalBusiness;
        public InventoryController(IServiceBusiness serviceBusinness, IUserContext userContext, ITableMetadataBusiness tableMetadataBusinness,
            ICmsBusiness cmsBusiness, INoteBusiness noteBusinness, IInventoryManagementBusiness inventoryManagementBusinness, ILOVBusiness lovBusiness, IPortalBusiness portalBusiness) 
        {
            _serviceBusinness = serviceBusinness;
            _userContext = userContext;
            _tableMetadataBusiness = tableMetadataBusinness;
            _cmsBusiness = cmsBusiness;
            _noteBusinness = noteBusinness;
            _inventoryManagementBusinness = inventoryManagementBusinness;
            _lovBusiness = lovBusiness;
            _portalBusiness = portalBusiness;
        }
        public async Task<IActionResult> Index(string pName = null)
        {
            var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            var menus = await _portalBusiness.GetMenuItems(portal, _userContext.UserId, _userContext.CompanyId, "INV");
            if (pName.IsNullOrEmpty())
            {
                var mg = menus.Where(x => x.PageType == TemplateTypeEnum.MenuGroup).FirstOrDefault();
                var page = menus.Where(x => x.PageType != TemplateTypeEnum.MenuGroup && x.ParentId == mg.Id).OrderBy(x => x.SequenceOrder).FirstOrDefault();
                ViewBag.PageName = page.Name;
            }
            else
            {
                ViewBag.PageName = pName;
            }
            ViewBag.Menus = menus;
            ViewBag.PortalId = _userContext.PortalId;

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

        public async Task<IActionResult> ScheduleInvoice(string customerId)
        {
            ScheduleInvoiceViewModel model = new ScheduleInvoiceViewModel();
            var existing = await _tableMetadataBusiness.GetTableDataByColumn("IMS_CUSTOMER", null, "Id", customerId);
            if (existing != null)
            {
                model.CustomerName = existing["CustomerName"].ToString();
            }
            model.CustomerId = customerId;
            model.DataAction = DataActionEnum.Create;
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
                    noteTempModel.TemplateCode = "IMS_SCHEDULE_INVOICE";
                    //noteTempModel.ParentNoteId = model.ParentNoteId;
                    var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
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
        public IActionResult InvoiceScheduledIndex()
        {
            return View();
        }
        public async Task<IActionResult> CheckRequisitionItemsExist(string requisitionId)
        {
            var requisitionitems=await _inventoryManagementBusinness.GetRequisistionItemsData(requisitionId);
            if (requisitionitems!=null && requisitionitems.Count>0) 
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        public async Task<IActionResult> AddRequisition(string itemHead,string id)
        {
            RequisitionViewModel model = new RequisitionViewModel();           
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _inventoryManagementBusinness.GetRequisitionDataByServiceId(id);
                model.DataAction = DataActionEnum.Edit;
            }
            else 
            {
                model.DataAction = DataActionEnum.Create;
                model.ItemHead = itemHead;
            }           
            return View("ManageRequisition",model);
        }
        public IActionResult ManageDirectSales(string id,string CustomerId)
        {
            DirectSalesViewModel model = new DirectSalesViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                model.DataAction = DataActionEnum.Edit;
            }
            else 
            {
                model.Customer = CustomerId;
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
                return Json(new { success = false});
        }

        public async Task<IActionResult> AddItems(string serviceId)
        {
            ItemsViewModel model = new ItemsViewModel();
            model.DirectSalesId = serviceId;
            var service = await _serviceBusinness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = serviceId });
            if (service!=null) 
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
        //public async Task<IActionResult> RequisitionIssue(string serviceId)
        //{
        //    IssueRequisitionViewModel model = new IssueRequisitionViewModel();
        //    model.RequisitionId = serviceId;
          
        //    model.DataAction = DataActionEnum.Create;
        //    return View(model);
        //}
        public async Task<IActionResult> AddRequisitionItems(string serviceId,string itemHead)
        {
            ItemsViewModel model = new ItemsViewModel();
            model.RequisitionId = serviceId;
            model.ItemHead = itemHead;
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

        public async Task<IActionResult> SalesOrder(string serviceId,string ProposalValue)
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
                else
                {
                    var existing = await _tableMetadataBusiness.GetTableDataByColumn("IMS_DIRECT_SALE_ITEMS", null, "NtsNoteId",model.NoteId);
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
            if (ModelState.IsValid)
            {
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
                    var existing = await _tableMetadataBusiness.GetTableDataByColumn("IMS_REQUISITION_ITEMS", null, "NtsNoteId", model.NoteId);
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
        public IActionResult VendorCategoryMapping()
        {
            return View();
        }
        public async Task<IActionResult> ReadItemsData(string directSalesId) 
        {
            IList<ItemsViewModel> list = new List<ItemsViewModel>();
            list = await _inventoryManagementBusinness.GetDirectSaleItemsData(directSalesId);
            return Json(list);
        }

        public async Task<IActionResult> GetItemUnitDetails(string itemId)
        {
            var data=await _inventoryManagementBusinness.GetItemsUnitDetailsByItemId(itemId);
            return Json(data);
        }
        public async Task<IActionResult> ReadDirectSalesData(string Customer,string ProposalSource, string WorkflowStatus,DateTime FromDate,DateTime ToDate)
        {
            DirectSalesSearchViewModel model = new DirectSalesSearchViewModel();
            model.Customer = Customer;
            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.WorkflowStatus = WorkflowStatus;
            model.ProposalSource = ProposalSource;
            IList<DirectSalesViewModel> list = new List<DirectSalesViewModel>();
            list = await _inventoryManagementBusinness.GetDirectSalesData(model);
            return Json(list);
        }
        public async Task<IActionResult> GetCustomerList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_CUSTOMER", "");
            return Json(data);
        }
        public async Task<IActionResult> GetCountryList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_COUNTRY", "");
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
            if (subCategoryId.IsNotNullAndNotEmpty())
            {
                var where = $@" and ""N_IMS_IMS_ITEM_MASTER"".""ItemSubCategory"" = '{subCategoryId}'";
                var data = await _cmsBusiness.GetDataListByTemplate("IMS_ITEM_MASTER", "", where);
                return Json(data);
            }
            else 
            {
                
                var data = await _cmsBusiness.GetDataListByTemplate("IMS_ITEM_MASTER", "");
                return Json(data);
            }
           
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
        public async Task<IActionResult> GetVendorsList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_VENDOR", "");
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> ManageVendorCategoryMapping(string vendorId,string[] categoryIds)
        {
            if (vendorId.IsNotNullAndNotEmpty() && categoryIds.Length>0)
            {
                foreach(var item in categoryIds)
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
                        return Json(new { success = false,error=result.Messages });
                    }
                }
            }

            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSalesItem(string NoteId)
        {
            await _noteBusinness.Delete(NoteId);
            return Json(new { success=true});
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
            var service = await _serviceBusinness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = RequisitionService });
            var noteTempModel1 = new NoteTemplateViewModel();
            noteTempModel1.SetUdfValue = true;
            noteTempModel1.NoteId = service.UdfNoteId;
            var notemodel1 = await _noteBusinness.GetNoteDetails(noteTempModel1);
            var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);

            rowData["RequisitionValue"] = (Convert.ToDecimal(rowData["RequisitionValue"]) -Convert.ToDecimal(Amount)).ToString();
            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            var update = await _noteBusinness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);

            await _noteBusinness.Delete(NoteId);
            return Json(new { success = true });
        }
        
        public IActionResult ShelfCategoryMapping()
        {
            return View();
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
        public async Task<IActionResult> SubmitRequisition(string serviceId)
        {

            var noteTempModel = new ServiceTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.ServiceId = serviceId;
            noteTempModel.AllowPastStartDate = true;
            var notemodel = await _serviceBusinness.GetServiceDetails(noteTempModel);
            var existing = await _inventoryManagementBusinness.GetRequisitionData(serviceId);
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
                            //foreach (var item in items)
                            //{
                            //if (Convert.ToDecimal(item.InventoryQuantity) > Convert.ToDecimal(item.BalanceQuantity))
                            //{
                            //    if (Convert.ToDecimal(item.CurrentIssueQuantity) > Convert.ToDecimal(item.BalanceQuantity))
                            //    {
                            //        ModelState.AddModelError("item", "Current issue quantity cannot exceed balance quantity");
                            //        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                            //    }
                            //}
                            //else 
                            //{
                            //    if (Convert.ToDecimal(item.CurrentIssueQuantity) > Convert.ToDecimal(item.InventoryQuantity))
                            //    {
                            //        ModelState.AddModelError("item", "Current issue quantity cannot exceed balance quantity");
                            //        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                            //    }
                            //}
                            //item.RequisitionIssueId = result.Item.ServiceId;
                            //item.IssuedQuantity = item.CurrentIssueQuantity;
                            //await ManageRequisitionIssueItems(item);

                            //    InventoryItemViewModel inventoryItem = new InventoryItemViewModel();
                            //    inventoryItem.ItemHeadReferenceId = result.Item.Id;
                            //    inventoryItem.ItemHeadReferenceType = result.Item.UdfNoteTableId;
                            //    inventoryItem.ItemReferenceId = item.NoteId;
                            //    inventoryItem.ItemReferenceType = item.UdfNoteTableId;
                            //    var requsistion = await _serviceBusinness.GetSingleById(model.RequisitionId);
                            //    inventoryItem.ItemDescription = "Issued against- "+ requsistion .ServiceNo+ ", Issue No- " + result.Item.ServiceNo + "," +
                            //        "Issued by " + result.Item.OwnerUserName + ", on " + model.IssuedOn;
                            //    inventoryItem.ItemQuantity =Convert.ToInt64(item.ItemQuantity);
                            //    inventoryItem.ItemPurchaseRate =Convert.ToInt64(item.PurchaseRate);
                            //    await _inventoryManagementBusinness.InsertInventory(inventoryItem);
                            //}
                        #region mark issue service as inprogress                        
                        var serviceTempModel = new ServiceTemplateViewModel();
                                serviceTempModel.DataAction =DataActionEnum.Edit;
                                serviceTempModel.ActiveUserId = _userContext.UserId;
                                serviceTempModel.ServiceId = result.Item.ServiceId;                                
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
                        #endregion
                    }
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
                    // model.Amount = model.PurchaseRate * model.ItemQuantity;
                    model.Id = null;
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
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public async Task<IActionResult> GetRequisistionIssueItemsByRequisitionId(string requisitionId)
        {
            var data=await _inventoryManagementBusinness.GetRequisistionIssueItemsByRequisitionId(requisitionId);
            return Json(data);
        }
        //public async Task<IActionResult> GetRequisistionIssue(string requisitionId)
        //{
        //    var data = await _inventoryManagementBusinness.GetRequisistionIssue(requisitionId);
        //    return Json(data);
        //}
        public async Task<IActionResult> ManageDeliveryNote(string serviceId)
        {
            DeliveryNoteViewModel model = new DeliveryNoteViewModel();
            model.DataAction = DataActionEnum.Create;
            var data = await _inventoryManagementBusinness.GetRequisitionDataByServiceId(serviceId);
            if (data.IsNotNull()) 
            {
                model.RequisitionCode = data.ServiceNo;
                model.RequisitionValue = data.RequisitionValue;
                model.RequisitionDate = data.RequisitionDate;               
                model.Particular = data.RequisitionParticular;
                model.ItemHead = data.ItemHead;
               
            }
            model.RequisitionId = serviceId;           
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
                               await CreateDeliveryNoteItem(item,result.Item.UdfNoteTableId);
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
        public async Task<IActionResult> ViewIssueItems(string serviceId)
        {
            ViewBag.RequisitionIssueId = serviceId;
            return View();
        }
        public async Task CreateDeliveryNoteItem(string IssuedItemId,string DeliveryNoteId)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Create;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "DELIVERY_ITEMS";
            var notemodel = await _noteBusinness.GetNoteDetails(noteTempModel);

            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("IssuedItemId", IssuedItemId);
            ((IDictionary<String, Object>)exo).Add("DeliveryNoteId", DeliveryNoteId);
            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            notemodel.DataAction = DataActionEnum.Create;
            var result = await _noteBusinness.ManageNote(notemodel);
            
        }
        //public async Task<IActionResult> ReadIssuedItems(string serviceId)
        //{
        //    var data = await _inventoryManagementBusinness.GetRequisistionIssueItems(serviceId);
        //    return Json(data);
        //}

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
            var data= await _inventoryManagementBusinness.GetVendorList(countryId, stateId, cityId, name);
            return Json(data);
        }
    }
}
