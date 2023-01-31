using AutoMapper;
using Synergy.App.ViewModel;
using Synergy.App.Business;
using Synergy.App.Business.Interface.DMS;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Api.Areas.DMS.Models;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft;
//using Syncfusion.EJ2.FileManager.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.ViewModel.IMS;
using Synergy.App.Api.Areas.IMS.Models;

namespace Synergy.App.Api.Areas.IMS.Controllers
{
    [Route("ims/inventory")]
    [ApiController]
    public class InventoryController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private IInventoryManagementBusiness _inventoryManagementBusiness;
        private ITableMetadataBusiness _tableMetadataBusiness;
        private ILOVBusiness _lovBusiness;
        private ICmsBusiness _cmsBusiness;
        private ICompanyBusiness _companyBusiness;
        private IServiceBusiness _serviceBusinness;
        private INoteBusiness _noteBusiness;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        public InventoryController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
         IServiceProvider serviceProvider, IServiceBusiness serviceBusinness,  ITableMetadataBusiness tableMetadataBusinness,
            ICmsBusiness cmsBusiness,ICompanyBusiness companyBusiness, INoteBusiness noteBusinness, IInventoryManagementBusiness inventoryManagementBusinness, ILOVBusiness lovBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceBusinness = serviceBusinness;
            _tableMetadataBusiness = tableMetadataBusinness;
            _cmsBusiness = cmsBusiness;
            _noteBusiness = noteBusinness;
            _inventoryManagementBusiness = inventoryManagementBusinness;
            _lovBusiness = lovBusiness;
            _companyBusiness = companyBusiness;
        }


        #region Dashboard
        [HttpGet]
        [Route("GetItemValueByCategory")]
        public async Task<IActionResult> GetItemValueByCategory()
        {
            var list = await _inventoryManagementBusiness.GetItemValueByCategory();
            if (list.IsNotNull())
            {
                var model = new InventoryDashboardViewModel
                {
                    ItemValueLabel = list.Select(x => x.ItemName).ToList(),
                    ItemValueSeries = list.Select(x => x.ItemValue.ToSafeInt()).ToList()
                };
                return Ok(model);
            }
            return null;
        }

        [HttpGet]
        [Route("GetItemValueByWarehouse")]
        public async Task<IActionResult> GetItemValueByWarehouse()
        {
            var list = await _inventoryManagementBusiness.GetItemValueByWarehouse();
            if (list.IsNotNull())
            {
                var model = new InventoryDashboardViewModel
                {
                    ItemValueLabel = list.Select(x => x.ItemName).ToList(),
                    ItemValueSeries = list.Select(x => x.ItemValue.ToSafeInt()).ToList()
                };
                return Ok(model);
            }
            return null;
        }


        [HttpGet]
        [Route("GetDashboardSalesActivitySummary")]
        public async Task<IActionResult> GetDashboardSalesActivitySummary()
        {
            var totalQtyInHand = await _inventoryManagementBusiness.GetTotalQtyInHandCount();
            var totalQtyTobeRecieved = await _inventoryManagementBusiness.GetTotalAllItem();
            InventoryDashboardViewModel model = new InventoryDashboardViewModel
            {
                TotalTobePackedItem = 3057,
                TotalTobeShippedItem = 43,
                TotalTobeDeliveredItem = 25,
                TotalTobeInvoicedItem = 4767,

                TotalQtyInHand = totalQtyInHand,
                TotalQtyTobeRecieved = totalQtyTobeRecieved
            };
            return Ok(model);
        }

        [HttpGet]
        [Route("GetDashboardProductDetails")]
        public async Task<IActionResult> GetDashboardProductDetails()
        {
            var totalLowStockItems = await _inventoryManagementBusiness.GetTotalLowStockItems();
            var totalAllItemGroupItems = await _inventoryManagementBusiness.GetTotalAllItemGroupItems();
            var totalAllItem = await _inventoryManagementBusiness.GetTotalAllItem();
            var ItemsInHand = await _inventoryManagementBusiness.GetTotalItemsInHand();
            var ItemsToReceive = await _inventoryManagementBusiness.GetTotalItemsToReceive();

            var model = new InventoryDashboardViewModel();
            model.TotalLowStockItems = totalLowStockItems;
            model.TotalAllItemGroupItems = totalAllItemGroupItems;
            model.TotalAllItem = totalAllItem;
            model.ItemsInHand = ItemsInHand;
            model.ItemsToReceive = ItemsToReceive;
            //model.ActionItemSeries = new List<int> { 44, 55, 41, 17, 15 };
            //model.ActiveItemLabels = new List<string> { "Comedy", "Action", "SciFi", "Drama", "Horror" };

            return Ok(model);
        }

        [HttpGet]
        [Route("GetDashboardPurchaseOrderItem")]
        public async Task<IActionResult> GetDashboardPurchaseOrderItem(InventoryDataFilterEnum filter,string userId)
        {
            await Authenticate(userId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var res = Helper.GetStartAndEndDateByEnum(filter);
            var qtyOrdered = await _inventoryManagementBusiness.GetPurchaseOrderQtyOrdered(res.Item1, res.Item2);
            var totalCost = await _inventoryManagementBusiness.GetPurchaseOrderTotaCost(res.Item1, res.Item2);
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
            return Ok(model);
        }

        [HttpGet]
        [Route("GetDashboardSalesOrderSummaryChart")]
        public async Task<IActionResult> GetDashboardSalesOrderSummaryChart(InventoryDataFilterEnum filter)
        {
            var res = Helper.GetStartAndEndDateByEnum(filter);
            var model1 = await _inventoryManagementBusiness.GetSalesOrderSummaryChart(res.Item1, res.Item2, filter);
            var directSalesAmount = await _inventoryManagementBusiness.GetDirectSalesAmountSummaryChart(res.Item1, res.Item2, filter);
            if (model1.IsNotNull() && model1.Count > 0)
            {

                var model = new InventoryDashboardViewModel
                {
                    SalesOrderSeries = model1.ToList(),
                    DirectSales = "Rs." + directSalesAmount,
                    Categories = model1.ToList()[0].categories,
                };

                return Ok(model);
            }
            return Ok(new InventoryDashboardViewModel());
        }

        [HttpGet]
        [Route("GetDashboardTopSellingsItem")]

        public async Task<IActionResult> GetDashboardTopSellingsItem(InventoryDataFilterEnum filter)
        {
            var res = Helper.GetStartAndEndDateByEnum(filter);
            var model = await _inventoryManagementBusiness.GetTopSellingsItem(res.Item1, res.Item2);
            return Ok(model);
        }
        #endregion Dashboard

        #region Sales Customer

        [HttpGet]
        [Route("GetCountryList")]
        public async Task<IActionResult> GetCountryList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_COUNTRY", "");
            return Ok(data);
        }

        [HttpGet]
        [Route("GetStateList")]
        public async Task<IActionResult> GetStateList(string countryId)
        {
            var where = $@" and ""N_IMS_MASTERDATA_States"".""CountryId"" = '{countryId}'";
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_STATE", "", where);
            return Ok(data);
        }
        [HttpGet]
        [Route("GetCityList")]
        public async Task<IActionResult> GetCityList(string stateId)
        {
            var where = $@" and ""N_IMS_MASTERDATA_City"".""StateId"" = '{stateId}'";
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_CITY", "", where);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadCustomerList")]
        public async Task<IActionResult> ReadCustomerList(string countryId, string stateId, string cityId, string name)
        {
            var data = await _inventoryManagementBusiness.GetCustomerList(countryId, stateId, cityId, name);
            return Ok(data);
        }

        #endregion

        #region Sales Direct
        [HttpGet]
        [Route("GetCustomerList")]
        public async Task<IActionResult> GetCustomerList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_CUSTOMER", "");
            return Ok(data);
        }

        [HttpGet]
        [Route("GetCustomerContacts")]
        public async Task<IActionResult> GetCustomerContacts(string customerId)
        {
            var list = await _inventoryManagementBusiness.ReadCustomerContactsData(customerId);
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadDirectSalesData")]
        public async Task<IActionResult> ReadDirectSalesData(string Customer, string ProposalSource, string WorkflowStatus, DateTime FromDate, DateTime ToDate)
        {
            DirectSalesSearchViewModel model = new DirectSalesSearchViewModel();
            model.Customer = Customer;
            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.WorkflowStatus = WorkflowStatus;
            model.ProposalSource = ProposalSource;
            IList<DirectSalesViewModel> list = new List<DirectSalesViewModel>();
            list = await _inventoryManagementBusiness.FilterDirectSalesData(model);
            return Ok(list);
        }

        [HttpGet]
        [Route("SubmitOrderDetails")]
        public async Task<IActionResult> SubmitOrderDetails(string serviceId, string ProposalValue, bool SalesOrder = false)
        {
            DirectSalesViewModel model = new DirectSalesViewModel();
            model.Id = serviceId;
            model.ProposalValue = ProposalValue;
            model.OrderValue = ProposalValue;
            var service = await _serviceBusinness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = serviceId });
            var existing = await _tableMetadataBusiness.GetTableDataByColumn("SN_IMS_DIRECT_SALES", null, "Id", service.UdfNoteTableId);
            if (existing != null)
            {
                var customer = await _tableMetadataBusiness.GetTableDataByColumn("IMS_CUSTOMER", null, "Id", existing["Customer"].ToString());
                model.CustomerName = customer["CustomerName"].ToString();
                model.Customer = existing["Customer"].ToString();
                model.ContactPerson = existing["ContactPerson"].ToString();
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
            return Ok(model);
        }


        [HttpGet]
        [Route("CreateDirectSales")]
        public IActionResult CreateDirectSales(string id, string CustomerId)
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
            return Ok(model);
        }
        [HttpPost]
        [Route("ManageDirectSales")]
        public async Task<IActionResult> ManageDirectSales(DirectSalesViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                        return Ok(new { success = true });
                    }
                    return Ok(new { success = false, error = ModelState });
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
                        return Ok(new { success = true });
                    }
                    return Ok(new { success = false, error = ModelState });


                }

            }
            return Ok(new { success = false, error = ModelState });
        }


        [HttpGet]
        [Route("ReadDirectSalesList")]
        public async Task<IActionResult> ReadDirectSalesList()
        {
            var data = await _inventoryManagementBusiness.GetDirectSalesList();
            return Ok(data);
        }

        [HttpGet]
        [Route("GetDirectSalesItemsList")]
        public async Task<IActionResult> GetDirectSalesItemsList(string directSalesId)
        {
            var list = await _inventoryManagementBusiness.GetDirectSaleItemsData(directSalesId);
            return Ok(list);

        }

        [HttpGet]
        [Route("GetDirectSalesData")]
        public async Task<IActionResult> GetDirectSalesData(string serviceId)
        {
            var data = await _inventoryManagementBusiness.GetDirectSalesData(serviceId);
            return Ok(data);
        }


        [HttpPost]
        [Route("OnOrderSubmit")]
        public async Task<IActionResult> OnOrderSubmit(DirectSalesViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                return Ok(new { success = true });
            }
            return Ok(new { success = false, error = ModelState.ToString() });

        }

        [HttpPost]
        [Route("ManageDirectSaleItems")]
        public async Task<IActionResult> ManageDirectSaleItems(ItemsViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var list = await _inventoryManagementBusiness.GetDirectSaleItemsData(model.DirectSalesId);
                    if (list.Any(x => x.Item == model.Item))
                    {
                        return Ok(new { success = false, error = "Item Already Exist" });
                    }
                    else
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = model.DataAction;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.TemplateCode = "IMS_DIRECT_SALE_ITEMS";
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        model.Amount = model.SaleRate * model.ItemQuantity;
                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        notemodel.DataAction = DataActionEnum.Create;
                        var result = await _noteBusiness.ManageNote(notemodel);
                        if (result.IsSuccess)
                        {
                            return Ok(new { success = true });
                        }
                        return Ok(new { success = false, error = ModelState });
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
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        var result = await _noteBusiness.ManageNote(notemodel);
                        if (result.IsSuccess)
                        {
                            return Ok(new { success = true });
                        }
                        return Ok(new { success = false, error = ModelState });
                    }

                }

            }
            return Ok(new { success = false, error = ModelState });
        }

        [HttpGet]
        [Route("ReadDirectSalesItemsData")]
        public async Task<IActionResult> ReadDirectSalesItemsData(string directSalesId)
        {
            IList<ItemsViewModel> list = new List<ItemsViewModel>();
            list = await _inventoryManagementBusiness.GetDirectSaleItemsData(directSalesId);
            return Ok(list);
        }

        [HttpGet]
        [Route("IfItemsExits")]
        public async Task<IActionResult> IfItemsExits(string directSalesId)
        {
            IList<ItemsViewModel> list = new List<ItemsViewModel>();
            list = await _inventoryManagementBusiness.GetDirectSaleItemsData(directSalesId);
            if (list.Count() == 0)
            {
                return Ok(false);
            }
            return Ok(true);
        }

        [HttpGet]
        [Route("DeleteSalesItem")]
        public async Task<IActionResult> DeleteSalesItem(string NoteId)
        {
            await _noteBusiness.Delete(NoteId);
            return Ok(new { success = true });
        }
        #endregion

        #region Sales Return

        [HttpGet]
        [Route("ReadSalesReturnList")]
        public async Task<IActionResult> ReadSalesReturnList(string cusId, string From, string To, string serNo)
        {
            var data = await _inventoryManagementBusiness.GetSalesReturnList(cusId, From, To, serNo);
            return Ok(data);
        }

        [HttpGet]
        [Route("NewSalesReturn")]
        public async Task<IActionResult> NewSalesReturn(string serId, string status)
        {
            var model = new SalesReturnViewModel() { DataAction = DataActionEnum.Create };

            if (serId.IsNotNullAndNotEmpty())
            {
                var data = await _inventoryManagementBusiness.GetSalesReturnData(serId);
                data.DataAction = DataActionEnum.Edit;
                return Ok(data);
            }
            else
            {
                return Ok(model);
            }

        }

        [HttpPost]
        [Route("ManageSalesReturn")]
        public async Task<IActionResult> ManageSalesReturn(SalesReturnViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

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

                        var result1 = await _noteBusiness.ManageNote(notemodel);
                    }
                    return Ok(new { success = true });
                }
                else
                {
                    return Ok(new { success = result.IsSuccess, error = result.Messages });
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
                    var returnItems = await _inventoryManagementBusiness.GetSalesReturnItemsList(model.SalesReturnId);

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
                            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

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

                            var result1 = await _noteBusiness.ManageNote(notemodel);
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
                            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                            rowData1["ReturnQuantity"] = item.ReturnQuantity;
                            rowData1["ReturnTypeId"] = item.ReturnTypeId;
                            rowData1["ReturnReason"] = item.ReturnReason;
                            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                            var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                        }
                    }
                    if (deleteItems.Count > 0)
                    {
                        foreach (var item in deleteItems)
                        {
                            var noteTempModel = new NoteTemplateViewModel();
                            noteTempModel.SetUdfValue = true;
                            noteTempModel.NoteId = item.NtsNoteId;
                            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                            var res = await _noteBusiness.DeleteNote(notemodel);
                        }
                    }
                    return Ok(new { success = true });
                }
            }
            return Ok(new { success = false });

        }

        [HttpGet]
        [Route("SubmitSalesReturn")]
        public async Task<IActionResult> SubmitSalesReturn(string serId,string userId)
        {
            await Authenticate(userId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var service = await _serviceBusinness.GetSingle(x => x.Id == serId);
            var serTempModel = new ServiceTemplateViewModel();
            serTempModel.DataAction = DataActionEnum.Edit;
            serTempModel.ActiveUserId = _userContext.UserId;
            serTempModel.ServiceId = service.Id;
            serTempModel.AllowPastStartDate = true;
            var sermodel = await _serviceBusinness.GetServiceDetails(serTempModel);
            var existing = await _inventoryManagementBusiness.GetSalesReturnData(serId);
            if (existing != null)
            {
                sermodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _serviceBusinness.ManageService(sermodel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
                return Ok(new { success = false });
            }
            return Ok(new { success = false });
        }

        [HttpGet]
        [Route("GetSalesReturnReceiveItem")]
        public async Task<IActionResult> GetSalesReturnReceiveItem(string goodsReceiptReferenceId, string goodsReceiptId, ImsReceiptTypeEnum receiptType, string grSerId, string receiptStatus = null)
        {
            var model = await _inventoryManagementBusiness.GetGoodsReceiptDataBySerId(grSerId);

            model.DataAction = DataActionEnum.Edit;
            model.GoodsReceiptReferenceId = goodsReceiptReferenceId;
            model.GoodsReceiptId = goodsReceiptId;
            model.ReceiptType = receiptType;
            model.ReceiptStatus = receiptStatus;
            model.ServiceId = grSerId;
            return Ok(model);
        }

        [HttpPost]
        [Route("ManageSalesReturnReceiveItem")]
        public async Task<IActionResult> ManageSalesReturnReceiveItem(GoodsReceiptViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                        return Ok(new { success = true });
                    }
                    return Ok(new { success = false, error = ModelState });
                }
                else
                {

                }
            }
            return Ok(new { success = false, error = ModelState });
        }

        [HttpGet]
        [Route("SubmitSalesReturnReceiveItem")]
        public async Task<IActionResult> SubmitSalesReturnReceiveItem(string serId,string userId)
        {
            await Authenticate(userId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var service = await _serviceBusinness.GetSingle(x => x.Id == serId);
            var serTempModel = new ServiceTemplateViewModel();
            serTempModel.DataAction = DataActionEnum.Edit;
            serTempModel.ActiveUserId = _userContext.UserId;
            serTempModel.ServiceId = service.Id;
            serTempModel.AllowPastStartDate = true;
            var notemodel = await _serviceBusinness.GetServiceDetails(serTempModel);
            var existing = await _inventoryManagementBusiness.GetGoodsReceiptDataBySerId(serId);
            if (existing != null)
            {
                notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _serviceBusinness.ManageService(notemodel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
                return Ok(new { success = false });
            }
            return Ok(new { success = false });
        }


        #endregion


        #region Requisition

        [HttpGet]
        [Route("ReadVendorList")]
        public async Task<IActionResult> ReadVendorList(string countryId, string stateId, string cityId, string name)
        {
            var data = await _inventoryManagementBusiness.GetVendorList(countryId, stateId, cityId, name);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadRequisitionData")]
        public async Task<IActionResult> ReadRequisitionData(string ItemHead, string Customer, string status, string From, string To)
        {
            var list = await _inventoryManagementBusiness.GetRequisitionDataByItemHead(ItemHead, Customer, status, From, To);
            return Ok(list);
        }

        [HttpGet]
        [Route("AddRequisition")]
        public async Task<IActionResult> AddRequisition(string itemHead, string id)
        {
            RequisitionViewModel model = new RequisitionViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _inventoryManagementBusiness.GetRequisitionDataById(id);
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.ItemHead = itemHead;
            }
            return Ok( model);
        }

        [HttpPost]
        [Route("ManageRequisition")]
        public async Task<IActionResult> ManageRequisition(RequisitionViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                        return Ok(new { success = true });
                    }
                    return Ok(new { success = false, error = ModelState.ToHtmlError() });
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
                        return Ok(new { success = true });
                    }
                    return Ok(new { success = false, error = ModelState });


                }

            }
            return Ok(new { success = false, error = ModelState });
        }

        [HttpGet]
        [Route("ReadRequisitionItemsData")]
        public async Task<IActionResult> ReadRequisitionItemsData(string requisitionId)
        {
            IList<ItemsViewModel> list = new List<ItemsViewModel>();
            list = await _inventoryManagementBusiness.GetRequisistionItemsData(requisitionId);
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadIssueRequisitionData")]

        public async Task<IActionResult> ReadIssueRequisitionData(string ItemHead, string From, string To)
        {
            var list = await _inventoryManagementBusiness.GetIssueRequisitionData(ItemHead, From, To);
            return Ok(list);
        }

        [HttpGet]
        [Route("GetRequisistionIssue")]
        public async Task<IActionResult> GetRequisistionIssue(string requisitionId, ImsIssueTypeEnum issuetype)
        {
            var data = await _inventoryManagementBusiness.GetRequisistionIssue(requisitionId, issuetype);
            return Ok(data);
        }

        [HttpGet]
        [Route("AddRequisitionIssue")]
        public async Task<IActionResult> AddRequisitionIssue(string id, string warehouseId, ImsIssueTypeEnum type)
        {
            IssueRequisitionViewModel model = new IssueRequisitionViewModel();
            model.IssueReferenceId = id;
            model.IssueReferenceType = type;
            model.WarehouseId = warehouseId;
            model.DataAction = DataActionEnum.Create;
            return Ok(model);
        }

        [HttpGet]
        [Route("ReadGoodReceiptItemsToIssue")]
        public async Task<IActionResult> ReadGoodReceiptItemsToIssue(string requisitionItemId, string itemId, string warehouseId, ImsReceiptTypeEnum receiptType)
        {
            IList<RequisitionIssueItemsViewModel> list = new List<RequisitionIssueItemsViewModel>();
            list = await _inventoryManagementBusiness.GetGoodReceiptItemsToIssue(requisitionItemId, itemId, warehouseId, receiptType);
            return Ok(list);
        }


        [HttpGet]
        [Route("GetSerialNos")]
        public async Task<IActionResult> GetSerialNos(string referenceId, string referenceHeaderId)
        {
            var list = await _inventoryManagementBusiness.GetSerailNoByHeaderIdandReferenceId(referenceId, referenceHeaderId);
            if (list.IsNotNull() && list.Count() > 0)
            {
                return Ok(list);
            }
            else
            {
                var item = await _inventoryManagementBusiness.GetGoodReceiptItemById(referenceId);
                if (item != null)
                {
                    var generatedItems = await _noteBusiness.GenerateItemSerialNumbers(Convert.ToInt64(item.ItemQuantity));
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
                            var generatedItems = await _noteBusiness.GenerateItemSerialNumbers(Convert.ToInt64(data["OpeningQuantity"]));
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
                var list1 = await _inventoryManagementBusiness.GetSerailNoByHeaderIdandReferenceId(referenceId, referenceHeaderId);

                return Ok(list1);
            }
        }

       
        [HttpPost]
        [Route("ManageSerialNo")]
        private async Task<IActionResult> ManageSerialNo(SerialNoViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("ReferenceHeaderId", model.ReferenceHeaderId);
                ((IDictionary<String, Object>)exo).Add("ReferenceId", model.ReferenceId);
                ((IDictionary<String, Object>)exo).Add("SerialNo", model.SerialNo);
                ((IDictionary<String, Object>)exo).Add("Specification", model.Specification);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.DataAction = DataActionEnum.Create;
                var result = await _noteBusiness.ManageNote(notemodel);
            }
            else
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.NoteId;
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                rowData["SerialNo"] = model.SerialNo;
                rowData["Specification"] = model.Specification;
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                return Ok(new { success = update.IsSuccess });
            }
            return Ok(new { success = false, error = "Please provide the details" });
        }

        [HttpPost]
        [Route("ManageRequisitionIssue")]
        public async Task<IActionResult> ManageRequisitionIssue(IssueRequisitionViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                                return Ok(new { success = false, error = ModelState });

                            }
                            item.RequisitionIssueId = result.Item.UdfNoteTableId;
                            item.IssuedQuantity = item.IssuedQuantity;
                            item.WarehouseId = model.WarehouseId;
                            item.ReferenceHeaderId = model.IssueReferenceId;
                            item.ReferenceHeaderItemId = model.RequisitionItemId;
                            var user = await _noteBusiness.GetSingleById<UserViewModel, User>(_userContext.UserId);
                            if (model.IssueReferenceType == ImsIssueTypeEnum.StockAdjustment)
                            {
                                var adjustmentStockservice = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == model.IssueReferenceId);
                                item.AdditionalInfo = "<" + result.Item.ServiceNo + "> issued against Stock Adjustment: <" + adjustmentStockservice.ServiceNo + "> By <" + user.Name + "> on <" + model.IssuedOn + ">";
                            }
                            else if (model.IssueReferenceType == ImsIssueTypeEnum.Requisition)
                            {
                                var requisition = await _inventoryManagementBusiness.GetRequisitionDataById(model.IssueReferenceId);
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
                                    await _inventoryManagementBusiness.updateSerialNosToIssued(serial);
                                }
                            }

                        }

                    }

                    await SubmitService(result.Item.UdfNoteTableId, model.OwnerUserId);
                    return Ok(new { success = true });
                }
                return Ok(new { success = false, error = ModelState });

            }
            return Ok(new { success = false, error = ModelState });
        }

        protected async Task<IActionResult> ManageRequisitionIssueItems(RequisitionIssueItemsViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            if (ModelState.IsValid)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "IMS_REQUSISTION_ISSUE_ITEM";
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
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
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {

                    return Ok(new { success = true });
                }
                return Ok(new { success = false, error = ModelState });
            }
            return Ok(new { success = false, error = ModelState });
        }

        protected async Task<IActionResult> SubmitService(string UdfNoteTableId, string userId)
        {
            await Authenticate(userId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                    return Ok(new { success = true });
                }
                return Ok(new { success = false });
            }
            return Ok(new { success = false });
        }

        [HttpGet]
        [Route("GetRequisitiononFilters")]
        public async Task<IActionResult> GetRequisitiononFilters(string ItemHead, string From, string To)
        {
            var list = await _inventoryManagementBusiness.GetRequisitiononFilters(ItemHead, From, To);
            return Ok(list);
        }

        #endregion

        #region Vendor

        [HttpGet]
        [Route("GetVendorsList")]
        public async Task<IActionResult> GetVendorsList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_VENDOR", "");
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadVendorContactsData")]
        public async Task<IActionResult> ReadVendorContactsData(string vendorId)
        {
            var list = await _inventoryManagementBusiness.ReadVendorContactsData(vendorId);
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadVendorCategoryData")]
        public async Task<IActionResult> ReadVendorCategoryData(string vendorId)
        {
            var list = await _inventoryManagementBusiness.ReadVendorCategoryData(vendorId);
            return Ok(list);
        }

        //[HttpGet]
        //[Route("VendorCategoryMapping")]
        //public IActionResult VendorCategoryMapping(string VendorId)
        //{
        //    VendorCategoryViewModel model = new VendorCategoryViewModel();
        //    model.VendorId = VendorId;
        //    return Ok(model);
        //}

        [HttpGet]
        [Route("ManageVendorCategoryMapping")]
        public async Task<IActionResult> ManageVendorCategoryMapping(string vendorId, string ids,string userId,string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        dynamic exo = new System.Dynamic.ExpandoObject();

                        ((IDictionary<String, Object>)exo).Add("VendorId", vendorId);
                        ((IDictionary<String, Object>)exo).Add("CategoryId", item);
                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        var result = await _noteBusiness.ManageNote(notemodel);
                        if (!result.IsSuccess)
                        {
                            return Ok(new { success = false, error = result.Messages });
                        }
                    }
                }

            }

            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("DeleteContacts")]
        public async Task<IActionResult> DeleteContacts(string ids)
        {
            if (ids.IsNotNullAndNotEmpty())
            {
                var id = ids.Split(",");
                foreach (var data in id)
                {
                    await _inventoryManagementBusiness.DeleteContacts(data);
                }
            }
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("DeleteCategories")]
        public async Task<IActionResult> DeleteCategories(string ids)
        {
            if (ids.IsNotNullAndNotEmpty())
            {
                var id = ids.Split(",");
                foreach (var data in id)
                {
                    await _inventoryManagementBusiness.DeleteVendorCategories(data);
                }
            }
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("ReadItemCategoryNotInVendorData")]
        public async Task<IActionResult> ReadItemCategoryNotInVendorData(string VendorId)
        {
            var data = await _inventoryManagementBusiness.ReadCategoryNotInVendorCategoryData(VendorId);
            return Ok(data);
        }

        #endregion

        #region Purchase Order

        [HttpGet]
        [Route("ReadPOList")]
        public async Task<IActionResult> ReadPOList(string itemHead = null, string vendorId = null, string statusId = null, string From = null, string To = null)
        {
            var list = await _inventoryManagementBusiness.GetVendorPOList(itemHead, vendorId, statusId, From, To);
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadPOItemsData")]
        public async Task<IActionResult> ReadPOItemsData(string poId)
        {
            var list = await _inventoryManagementBusiness.ReadPOItemsData(poId);
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadRequisitionList")]
        public async Task<IActionResult> ReadRequisitionList(string requisitionIds)
        {
            var list = await _inventoryManagementBusiness.GetRequisitionDataByItemHead("", "", "", "", "", requisitionIds);
            return Ok(list);
        }
        [HttpGet]
        [Route("GetVendorContactList")]

        public async Task<IActionResult> GetVendorContactList(string vendorId)
        {
            var where = $@" and ""N_IMS_VendorContact"".""VendorId"" = '{vendorId}'";
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_VENDOR_CONTACT", "", where);
            return Ok(data);
        }

        [HttpGet]
        [Route("ChangeVendorForPO")]
        public async Task<IActionResult> ChangeVendorForPO(string serId, string venId, string poId = null)
        {
            var list = await _inventoryManagementBusiness.GetVendorPOList();
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

            return Ok(model);
        }

        [HttpPost]
        [Route("ManageVendorForPO")]
        public async Task<IActionResult> ManageVendorForPO(PurchaseOrderViewModel model)
        {
            ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            serviceModel.ServiceId = model.ServiceId;

            var service = await _serviceBusinness.GetServiceDetails(serviceModel);

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.SetUdfValue = true;
            noteTempModel.NoteId = service.UdfNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

            rowData["VendorId"] = model.VendorId;
            rowData["ContactPersonId"] = model.ContactPersonId;
            rowData["ContactNo"] = model.ContactNo;

            //var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);

            //var update = await _noteBusinness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

            service.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);

            service.DataAction = DataActionEnum.Edit;

            var res = await _serviceBusinness.ManageService(service);

            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("GetRequisitionList")]
        public async Task<IActionResult> GetRequisitionList(string stateId)
        {
            //var where = $@" and ""N_IMS_MASTERDATA_City"".""StateId"" = '{stateId}'";
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_REQUISITION", "", "");
            return Ok(data);
        }
        [HttpGet]
        [Route("GetRequisistionItemsByRequisitionId")]
        public async Task<IActionResult> GetRequisistionItemsByRequisitionId(string requisitionIds)
        {
            var data = await _inventoryManagementBusiness.GetRequisistionItemsByRequisitionId(requisitionIds);
            return Ok(data);
        }


        [HttpPost]
        [Route("ManagePOItems")]
        public async Task<IActionResult> ManagePOItems(SubmitPOItemsModel model)
        {
            await Authenticate(model.userId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            await _inventoryManagementBusiness.GeneratePOItemsMobile(model.itemList,model. poId);
            var POValue = await _inventoryManagementBusiness.GetPOValueByPOId(model.poId);
            await _inventoryManagementBusiness.UpdatePOValueInPO(model.poId, POValue);
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("GetPOItemsByPOId")]
        public async Task<IActionResult> GetPOItemsByPOId(string poId)
        {
            var data = await _inventoryManagementBusiness.GetPOItemsByPOId(poId);
            return Ok(data);
        }

        //Terms and Conditions
        [HttpGet]
        [Route("GetTermsAndConditionsList")]
        public async Task<IActionResult> GetTermsAndConditionsList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_TERMS_AND_CONDITIONS", "", "");
            return Ok(data);
        }
        [HttpGet]
        [Route("ReadPOTermsData")]
        public async Task<IActionResult> ReadPOTermsData(string poId)
        {
            var list = await _inventoryManagementBusiness.ReadPOTermsData(poId);
            return Ok(list);
        }

        [HttpGet]
        [Route("DeletePOTCItem")]
        public async Task<IActionResult> DeletePOTCItem(string NoteId)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.SetUdfValue = true;
            noteTempModel.NoteId = NoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            var res = await _noteBusiness.DeleteNote(notemodel);
            return Ok(new { success = res.IsSuccess });
        }

        [HttpPost]
        [Route("ManagePOTC")]
        public async Task<IActionResult> ManagePOTC(POTermsAndConditionsViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            List<POTermsAndConditionsViewModel> items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<POTermsAndConditionsViewModel>>(model.TermsList);
            if (items.IsNotNull() && items.Count() > 0)
            {
                var existing = await _inventoryManagementBusiness.ReadPOTermsData(model.POID);
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
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        dynamic exo = new System.Dynamic.ExpandoObject();

                        ((IDictionary<String, Object>)exo).Add("POID", model.POID);
                        ((IDictionary<String, Object>)exo).Add("TermsId", data.Id);
                        ((IDictionary<String, Object>)exo).Add("TermsTitle", data.Title);
                        ((IDictionary<String, Object>)exo).Add("TermsDescription", data.Description);

                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        notemodel.DataAction = DataActionEnum.Create;
                        var result = await _noteBusiness.ManageNote(notemodel);
                    }
                }
                else
                {
                    return Ok(new { success = false, errormsg = "Selected Terms and Conditions already added" });
                }

            }

            return Ok(new { success = true });
        }


        //Submit Purchase Order
        [HttpGet]
        [Route("CheckPOItemsExist")]
        public async Task<IActionResult> CheckPOItemsExist(string poId)
        {
            var poitems = await _inventoryManagementBusiness.GetPOItemsData(poId);
            if (poitems != null && poitems.Count > 0)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        [HttpGet]
        [Route("SubmitPO")]
        public async Task<IActionResult> SubmitPO(string serId,string userId)
        {
            await Authenticate(userId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();


            //var service = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == requisitionId);
            var serviceTempModel = new ServiceTemplateViewModel();
            serviceTempModel.DataAction = DataActionEnum.Edit;
            serviceTempModel.ActiveUserId = _userContext.UserId;
            serviceTempModel.ServiceId = serId;
            serviceTempModel.AllowPastStartDate = true;
            var servicemodel = await _serviceBusinness.GetServiceDetails(serviceTempModel);
            var existing = await _inventoryManagementBusiness.GetPOData(serId);
            if (existing != null)
            {
                servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                servicemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _serviceBusinness.ManageService(servicemodel);

                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
                return Ok(new { success = false });
            }
            return Ok(new { success = false });
        }

        #endregion



        #region Purchase Receive Items

        [HttpGet]
        [Route("ReadPOData")]
        public async Task<IActionResult> ReadPOData(string ItemHead, string Vendor, string From, string To)
        {
            var list = await _inventoryManagementBusiness.ReadPOData(ItemHead, Vendor, From, To);
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadDeliveryChallanData")]
        public async Task<IActionResult> ReadDeliveryChallanData(string ItemHead, string Vendor, string From, string To, string poId, ImsReceiptTypeEnum? receiptType)
        {
            var data = await _inventoryManagementBusiness.ReadDeliveryChallanData(ItemHead, Vendor, From, To, poId, receiptType);
            return Ok(data);
        }


        #endregion

        #region Purchase Invoice

        [HttpGet]
        [Route("GetChallanDetailsByPoId")]
        public async Task<IActionResult> GetChallanDetailsByPoId(string poId)
        {
            var data = await _inventoryManagementBusiness.GetChallanDetailsbyPOId(poId);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetPOInvoiceList")]
        public async Task<IActionResult> GetPOInvoiceList(string poId)
        {
            var data = await _inventoryManagementBusiness.GetPOInvoiceDetailsList(poId);
            return Ok(data);
        }

        [HttpPost]
        [Route("ManagePurchaseInvoiceData")]
        public async Task<IActionResult> ManagePurchaseInvoiceData(POInvoiceSubmitModel model)//string userId,string invoiceno, string pidate, List<string> receiptIds, string poId, List<GoodsReceiptViewModel> challanList)
        {
            await Authenticate(model.userId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var isInvoiceNoExists = await _inventoryManagementBusiness.InvoiceNoExists(model.invoiceno);
            if (isInvoiceNoExists)
            {
                return Ok(new { success = false, error = "Invoice No. already exists." });
            }
            var serTempModel = new POInvoiceViewModel();
            serTempModel.DataAction = DataActionEnum.Create;
            serTempModel.ActiveUserId = _userContext.UserId;
            serTempModel.TemplateCode = "IMS_PO_INVOICE";

            var sermodel = await _serviceBusinness.GetServiceDetails(serTempModel);

            dynamic exo = new System.Dynamic.ExpandoObject();
            ((IDictionary<String, Object>)exo).Add("InvoiceNo", model.invoiceno);
            ((IDictionary<String, Object>)exo).Add("InvoiceDate", model.pidate);
            ((IDictionary<String, Object>)exo).Add("PoId", model.poId);

            sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            sermodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
            sermodel.DataAction = DataActionEnum.Create;
            var result = await _serviceBusinness.ManageService(sermodel);

            if (result.IsSuccess)
            {
                foreach (var x in model.receiptIds)
                {
                    var data = await _inventoryManagementBusiness.GetGoodReceiptItemDetails(x);

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_PO_INVOICE_ITEM";

                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
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
                    var result1 = await _noteBusiness.ManageNote(notemodel);
                }
                return Ok(new { success = true });
            }
            return Ok(new { success = false, error = "Invoice Generation Failed" });
        }


        #endregion

        #region Purchase Return

        [HttpGet]
        [Route("ReadPurchaseReturnList")]
        public async Task<IActionResult> ReadPurchaseReturnList(string cusId, string From, string To, string serNo)
        {
            var data = await _inventoryManagementBusiness.GetPurchaseReturnList(cusId, From, To, serNo);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadPurchaseReturnItems")]
        public async Task<IActionResult> ReadPurchaseReturnItems(string serviceId)
        {
            var data = await _inventoryManagementBusiness.GetPurchaseReturnItemsData(serviceId);
            return Ok(data);
        }

        [HttpGet]
        [Route("NewPurchaseReturn")]
        public async Task<IActionResult> NewPurchaseReturn(string serId, string status)
        {
            var model = new PurchaseReturnViewModel() { DataAction = DataActionEnum.Create };

            if (serId.IsNotNullAndNotEmpty())
            {
                var data = await _inventoryManagementBusiness.GetPurchaseReturnData(serId);
                data.DataAction = DataActionEnum.Edit;
                return Ok(data);
            }
            else
            {
                return Ok(model);
            }

        }
        
        [HttpGet]
        [Route("ReadPurchaseOrderList")]
        public async Task<IActionResult> ReadPurchaseOrderList()
        {
            var data = await _inventoryManagementBusiness.GetPurchaseOrderList();
            return Ok(data);
        }

        [HttpGet]
        [Route("GetVendorContacts")]
        public async Task<IActionResult> GetVendorContacts(string vendorId)
        {
            var list = await _inventoryManagementBusiness.ReadVendorContactsData(vendorId);
            return Ok(list);
        }


        [HttpGet]
        [Route("GetPurchaseOrderItemsList")]
        public async Task<IActionResult> GetPurchaseOrderItemsList(string purchaseId)
        {
            var list = await _inventoryManagementBusiness.GetPurchaseOrderItemsList(purchaseId);
            return Ok(list);

        }


        [HttpGet]
        [Route("GetPurchaseOrderData")]
        public async Task<IActionResult> GetPurchaseOrderData(string serviceId)
        {
            var data = await _inventoryManagementBusiness.GetPurchaseOrderData(serviceId);
            return Ok(data);
        }

        [HttpPost]
        [Route("ManagePurchaseReturn")]
        public async Task<IActionResult> ManagePurchaseReturn(PurchaseReturnViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            if (model.DataAction == DataActionEnum.Create)
            {
                var data = await _inventoryManagementBusiness.GetPurchaseOrderData(model.PurchaseOrderServiceId);
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
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                        dynamic exo = new System.Dynamic.ExpandoObject();

                        ((IDictionary<String, Object>)exo).Add("PurchaseReturnId", result.Item.UdfNoteTableId);
                        ((IDictionary<String, Object>)exo).Add("POItemId", item.Id);
                        ((IDictionary<String, Object>)exo).Add("ItemId", item.Item);
                        ((IDictionary<String, Object>)exo).Add("PurchaseQuantity", item.ItemQuantity);
                        ((IDictionary<String, Object>)exo).Add("ReturnQuantity", item.ReturnQuantity);
                        ((IDictionary<String, Object>)exo).Add("ReturnType", item.ReturnTypeId);
                        ((IDictionary<String, Object>)exo).Add("ReturnComment", item.ReturnReason);

                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                        var result1 = await _noteBusiness.ManageNote(notemodel);
                    }
                    return Ok(new { success = true });
                }
                else
                {
                    return Ok(new { success = result.IsSuccess, error = result.Messages });
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
                    var returnItems = await _inventoryManagementBusiness.GetPurchaseReturnItemsList(model.PurchaseReturnId);

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
                            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                            dynamic exo = new System.Dynamic.ExpandoObject();

                            ((IDictionary<String, Object>)exo).Add("PurchaseReturnId", result.Item.UdfNoteTableId);
                            ((IDictionary<String, Object>)exo).Add("POItemId", item.Id);
                            ((IDictionary<String, Object>)exo).Add("ItemId", item.Item);
                            ((IDictionary<String, Object>)exo).Add("PurchaseQuantity", item.ItemQuantity);
                            ((IDictionary<String, Object>)exo).Add("ReturnQuantity", item.ReturnQuantity);
                            ((IDictionary<String, Object>)exo).Add("ReturnType", item.ReturnTypeId);
                            ((IDictionary<String, Object>)exo).Add("ReturnComment", item.ReturnReason);

                            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                            notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                            var result1 = await _noteBusiness.ManageNote(notemodel);
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
                            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                            rowData1["ReturnQuantity"] = item.ReturnQuantity;
                            rowData1["ReturnType"] = item.ReturnTypeId;
                            rowData1["ReturnComment"] = item.ReturnReason;
                            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                            var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                        }
                    }
                    if (deleteItems.Count > 0)
                    {
                        foreach (var item in deleteItems)
                        {
                            var noteTempModel = new NoteTemplateViewModel();
                            noteTempModel.SetUdfValue = true;
                            noteTempModel.NoteId = item.NtsNoteId;
                            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                            var res = await _noteBusiness.DeleteNote(notemodel);
                        }
                    }
                    return Ok(new { success = true });
                }
            }
            return Ok(new { success = false });

        }

        [HttpPost]
        [Route("SubmitPurchaseReturn")]
        public async Task<IActionResult> SubmitPurchaseReturn(string serId,string userId)
        {
            await Authenticate(userId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var serTempModel = new ServiceTemplateViewModel();
            serTempModel.DataAction = DataActionEnum.Edit;
            serTempModel.ActiveUserId = _userContext.UserId;
            serTempModel.ServiceId = serId;
            serTempModel.AllowPastStartDate = true;
            var notemodel = await _serviceBusinness.GetServiceDetails(serTempModel);
            var existing = await _inventoryManagementBusiness.GetPurchaseReturnData(serId);
            if (existing != null)
            {
                notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _serviceBusinness.ManageService(notemodel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
                return Ok(new { success = false });
            }
            return Ok(new { success = false });
        }
        #endregion

        #region Inventory

        [HttpGet]
        [Route("ReadStockAdjustmentList")]
        public async Task<IActionResult> ReadStockAdjustmentList()
        {
            var data = await _inventoryManagementBusiness.GetStockAdjustmentList();
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadGoodsReceiptData")]
        public async Task<IActionResult> ReadGoodsReceiptData(string goodsReceiptReferenceId, ImsReceiptTypeEnum receiptType)
        {
            var data = await _inventoryManagementBusiness.ReadGoodsReceiptData(goodsReceiptReferenceId, receiptType);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetDeliveryChallanItems")]
        public async Task<IActionResult> GetDeliveryChallanItems(string id)
        {
            // var poId = await _inventoryManagementBusiness.GetPoIdByGoodReceiptId(id);
            var data = await _inventoryManagementBusiness.GetGoodReceiptItemsByReceiptId(id);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetRequisistionIssueItemsByRequisitionId")]
        public async Task<IActionResult> GetRequisistionIssueItemsByRequisitionId(string requisitionId)
        {
            var data = await _inventoryManagementBusiness.GetRequisistionIssueItemsByRequisitionId(requisitionId);
            return Ok(data);
        }


        [HttpGet]
        [Route("StockAdjustmentIssues")]
        public IActionResult StockAdjustmentIssues(string stockAdjustmentId, string warehouseId)
        {
            IssueRequisitionViewModel model = new IssueRequisitionViewModel();
            model.IssueReferenceId = stockAdjustmentId;
            model.IssueReferenceType = ImsIssueTypeEnum.StockAdjustment;
            model.WarehouseId = warehouseId;
            model.DataAction = DataActionEnum.Create;
            return Ok(model);
        }

        [HttpGet]
        [Route("ReadStockAdjustmentItemsData")]
        public async Task<IActionResult> ReadStockAdjustmentItemsData(string stockAdjustmentId)
        {
            var list = await _inventoryManagementBusiness.GetStockAdjustmentItemsData(stockAdjustmentId);
            return Ok(list);
        }

        
        [HttpGet]
        [Route("CreateStockAdjustment")]
        public async Task<IActionResult> CreateStockAdjustment(string id)
        {
            StockAdjustmentViewModel model = new StockAdjustmentViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                var service = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == id);
                model = await _inventoryManagementBusiness.GetStockAdjustmentById(id);
                model.DataAction = DataActionEnum.Edit;
                model.ServiceId = service.Id;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }
            return Ok(model);

        }

        [HttpGet]
        [Route("GetWarehouseList")]
        public async Task<IActionResult> GetWarehouseList(string legalEntityId = null)
        {
            var where = "";
            if (legalEntityId.IsNotNullAndNotEmpty())
            {
                where = $@" and ""N_IMS_MASTERDATA_WareHouseMaster"".""WarehouseLegalEntityId"" = '{legalEntityId}'";
            }
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_WAREHOUSE_MASTER", "", where);
            return Ok(data);
        }

        [HttpPost]
        [Route("ManageStockAdjustment")]
        public async Task<IActionResult> ManageStockAdjustment(StockAdjustmentViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                        return Ok(new { success = true });
                    }
                    return Ok(new { success = false, error = ModelState});
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
                        return Ok(new { success = true });
                    }
                    return Ok(new { success = false, error = ModelState});


                }

            }
            return Ok(new { success = false, error = ModelState});
        }

        [HttpGet]
        [Route("GetItemList")]
        public async Task<IActionResult> GetItemList(string subCategoryId)
        {
            var data = await _inventoryManagementBusiness.GetActiveItemsFilterBySubCategory(subCategoryId);
            return Ok(data);
        }

        [HttpGet]
        [Route("AddStockAdjustmentItems")]
        public async Task<IActionResult> AddStockAdjustmentItems(string stockAdjustmentId)
        {
            StockAdjustmentItemViewModel model = new StockAdjustmentItemViewModel();
            model.StockAdjustmentId = stockAdjustmentId;
            var serviceData = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == stockAdjustmentId);
            var lov = await _lovBusiness.GetSingle(x => x.Id == serviceData.ServiceStatusId);
            model.ServiceStatusCode = lov.Code;
            model.DataAction = DataActionEnum.Create;
            return Ok(model);
        }

        [HttpPost]
        [Route("ManageStockAdjustmentItems")]
        public async Task<IActionResult> ManageStockAdjustmentItems(StockAdjustmentItemViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "STOCK_ADJUSTMENT_ITEM";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        return Ok(new { success = true });
                    }
                    return Ok(new { success = false, error = ModelState });
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
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        var result = await _noteBusiness.ManageNote(notemodel);
                        if (result.IsSuccess)
                        {
                            return Ok(new { success = true });
                        }
                        return Ok(new { success = false, error = ModelState });
                    }

                }

            }
            return Ok(new { success = false, error = ModelState });
        }

        #endregion

        #region Stock Transfer

        [HttpGet]
        [Route("ReadItemTransferList")]
        public async Task<IActionResult> ReadItemTransferList(string From, string To, string challanNo)
        {
            var data = await _inventoryManagementBusiness.GetItemTransferredList(From, To, challanNo);
            return Ok(data);
        }

        [HttpGet]
        [Route("NewItemTransfer")]
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
                var data = await _inventoryManagementBusiness.GetItemTransferredList("", "", "");
                model = data.Where(x => x.ServiceId == serid).FirstOrDefault();
                model.DataAction = DataActionEnum.Edit;
            }
            return Ok(model);
        }
        [HttpGet]
        [Route("GetBusinessUnitList")]
        public async Task<IActionResult> GetBusinessUnitList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_BUSINESS_UNIT", "", "");
            return Ok(data);
        }

        [HttpPost]
        [Route("ManageTransferItems")]
        public async Task<IActionResult> ManageTransferItems(StockTransferViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var data = await _inventoryManagementBusiness.GetItemTransferredList("", "", model.ChallanNo);
                    if (data.Count > 0)
                    {
                        return Ok(new { success = false, error = "Challan no already exist" });
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

                            return Ok(new { success = result.IsSuccess, data = result.Item.UdfNoteTableId });
                        }
                    }
                }
                else
                {
                    var service = await _serviceBusinness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = model.ServiceId });
                    var noteTempModel1 = new NoteTemplateViewModel();
                    noteTempModel1.SetUdfValue = true;
                    noteTempModel1.NoteId = service.UdfNoteId;
                    var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                    var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                    rowData["ChallanNo"] = model.ChallanNo;
                    rowData["TransferDate"] = model.TransferDate;
                    rowData["TransferReason"] = model.TransferReason;

                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);
                    if (update.IsSuccess)
                    {
                        return Ok(new { success = true });
                    }
                }

            }
            return Ok(new { success = false, error = ModelState });
        }

        [HttpGet]
        [Route("ReadTransferItemsList")]
        public async Task<IActionResult> ReadTransferItemsList(string stockTransferId)
        {
            var data = await _inventoryManagementBusiness.GetTransferItemsList(stockTransferId);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadItemListByStock")]
        public async Task<IActionResult> ReadItemListByStock(string itemTypeId, string itemCategory, string itemSubCategory, string warehouseId)
        {
            var data = await _inventoryManagementBusiness.ReadItemListByStock(itemTypeId, itemCategory, itemSubCategory, warehouseId);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetItemDeatils")]
        public async Task<IActionResult> GetItemDeatils(string Id, string warehouseId)
        {
            var data = await _inventoryManagementBusiness.GetItemHeaderData(Id, warehouseId);
            return Ok(data);
        }

        [HttpGet]
        [Route("ValidateStockTransferItem")]
        public async Task<IActionResult> ValidateStockTransferItem(string stId, string itemId)
        {
            var data = await _inventoryManagementBusiness.GetTransferItemsList(stId);
            var exist = data.Where(x => x.ItemId == itemId).ToList();
            if (exist.Count > 0)
            {
                return Ok(new { success = true });
            }
            else
            {
                return Ok(new { success = false });
            }
        }

        [HttpGet]
        [Route("CreateTransferItems")]
        public async Task<IActionResult> CreateTransferItems(string stId, string itemId, string transQty,string userId)
        {
            await Authenticate(userId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Create;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "IMS_STOCK_TRANSFER_ITEM";

            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("StockTransferId", stId);
            ((IDictionary<String, Object>)exo).Add("ItemId", itemId);
            ((IDictionary<String, Object>)exo).Add("TransferQuantity", transQty);

            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            //notemodel.ParentServiceId = result.Item.ServiceId;
            notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            notemodel.DataAction = DataActionEnum.Create;
            var result = await _noteBusiness.ManageNote(notemodel);

            return Ok(new { success = result.IsSuccess });

        }

        [HttpGet]
        [Route("CheckStocTransferItemsExist")]
        public async Task<IActionResult> CheckStocTransferItemsExist(string stockTransferId)
        {
            var data = await _inventoryManagementBusiness.GetTransferItemsList(stockTransferId);
            if (data.Count > 0)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        [HttpGet]
        [Route("DeleteStockTransferItem")]
        public async Task<IActionResult> DeleteStockTransferItem(string itemNoteId)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.SetUdfValue = true;
            noteTempModel.NoteId = itemNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            var res = await _noteBusiness.DeleteNote(notemodel);
            return Ok(new { success = res.IsSuccess });
        }

        [HttpGet]
        [Route("SubmitStockTransfer")]
        public async Task<IActionResult> SubmitStockTransfer(string serId,string userId)
        {
            await Authenticate(userId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var service = await _serviceBusinness.GetSingle(x => x.Id == serId);
            var serTempModel = new ServiceTemplateViewModel();
            serTempModel.DataAction = DataActionEnum.Edit;
            serTempModel.ActiveUserId = _userContext.UserId;
            serTempModel.ServiceId = service.Id;
            serTempModel.AllowPastStartDate = true;
            var sermodel = await _serviceBusinness.GetServiceDetails(serTempModel);
            var existing = await _inventoryManagementBusiness.GetItemTransferredList("", "", "");
            var existdata = existing.Where(x => x.ServiceId == serId).FirstOrDefault();

            if (existdata != null)
            {
                sermodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existdata);
                var result = await _serviceBusinness.ManageService(sermodel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
                return Ok(new { success = false });
            }
            return Ok(new { success = false });
        }

        [HttpGet]
        [Route("SubmitStockTransferReceipt")]
        public async Task<IActionResult> SubmitStockTransferReceipt(string serId,string userId)
        {
            await Authenticate(userId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var service = await _serviceBusinness.GetSingle(x => x.Id == serId);
            var serTempModel = new ServiceTemplateViewModel();
            serTempModel.DataAction = DataActionEnum.Edit;
            serTempModel.ActiveUserId = _userContext.UserId;
            serTempModel.ServiceId = service.Id;
            serTempModel.AllowPastStartDate = true;
            var notemodel = await _serviceBusinness.GetServiceDetails(serTempModel);
            var existing = await _inventoryManagementBusiness.GetGoodsReceiptDataBySerId(serId);
            if (existing != null)
            {
                notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _serviceBusinness.ManageService(notemodel);
                if (result.IsSuccess)
                {
                    var items = await _inventoryManagementBusiness.GetGoodReceiptItemsList(result.Item.UdfNoteTableId);
                    if (items != null && items.Count() > 0)
                    {
                        foreach (var data in items)
                        {
                            await GetSerialNos(result.Item.UdfNoteTableId, data.Id);
                            var closingBalance = await _inventoryManagementBusiness.GetClosingBalance(data.ItemId, data.WarehouseId);
                            await _inventoryManagementBusiness.UpdateStockClosingBalance(data.ItemId, data.WarehouseId, closingBalance);

                        }
                    }
                    return Ok(new { success = true });
                }
                return Ok(new { success = false });
            }
            return Ok(new { success = false });
        }

        [HttpGet]
        [Route("CreateStockTransferReceiveItem")]
        public async Task<IActionResult> CreateStockTransferReceiveItem(string grSerId, string receiptStatus, string stId, string toWarehouseId)
        {

            var model = new GoodsReceiptViewModel();
            var data = await _inventoryManagementBusiness.GetGoodsReceiptDataBySerId(grSerId);
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

            return Ok(model);
        }

        [HttpGet]
        [Route("ManageStockTransferReceiveItem")]
        public async Task<IActionResult> ManageStockTransferReceiveItem(GoodsReceiptViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                            dynamic exo = new System.Dynamic.ExpandoObject();
                            //var goodreceipt = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == result.Item.UdfNoteTableId);
                            var po = await _serviceBusinness.GetSingle(x => x.UdfNoteTableId == model.GoodsReceiptReferenceId);
                            ((IDictionary<String, Object>)exo).Add("GoodReceiptId", result.Item.UdfNoteTableId);
                            ((IDictionary<String, Object>)exo).Add("ItemQuantity", data.IssuedQuantity);
                            ((IDictionary<String, Object>)exo).Add("BalanceQuantity", data.IssuedQuantity);
                            ((IDictionary<String, Object>)exo).Add("ItemId", data.ItemId);
                            ((IDictionary<String, Object>)exo).Add("WarehouseId", model.WarehouseId);
                            ((IDictionary<String, Object>)exo).Add("ReferenceHeaderItemId", model.GoodsReceiptReferenceId);
                            var user = await _noteBusiness.GetSingleById<UserViewModel, User>(_userContext.UserId);
                            ((IDictionary<String, Object>)exo).Add("AdditionalInfo", "<" + result.Item.ServiceNo + "> received against stock transfer: <" + po.ServiceNo + "> By <" + user.Name + "> on <" + model.ReceiveDate + ">");

                            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                            notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            notemodel.DataAction = DataActionEnum.Create;
                            var result1 = await _noteBusiness.ManageNote(notemodel);
                        }
                    }

                    return Ok(new { success = true });
                }
                return Ok(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
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
                    return Ok(new { success = true });
                }
                return Ok(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }

        }

        #endregion

        #region Reports

        [HttpGet]
        [Route("ReadItemCurrentStockData")]
        public async Task<IActionResult> ReadItemCurrentStockData(string warehouseId, string itemTypeId, string itemCategoryId, string itemSubCategoryId, string itemId)
        {
            var data = await _inventoryManagementBusiness.ReadItemCurrentStockData(warehouseId, itemTypeId, itemCategoryId, itemSubCategoryId, itemId);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetItemSubCategoryListByItemCategory")]
        public async Task<IActionResult> GetItemSubCategoryListByItemCategory(string itemCategoryId)
        {
            var data = await _inventoryManagementBusiness.GetItemSubCategoryByItemCategoryId(itemCategoryId);
            return Ok(data);
        }
        [HttpGet]
        [Route("GetItemListByItemSubCategory")]
        public async Task<IActionResult> GetItemListByItemSubCategory(string itemSubCategoryId)
        {
            var data = await _inventoryManagementBusiness.GetItemByItemSubCategoryId(itemSubCategoryId);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetItemCategoryListByItemType")]
        public async Task<IActionResult> GetItemCategoryListByItemType(string itemTypeId)
        {
            var data = await _inventoryManagementBusiness.GetItemCategoryByItemTypeId(itemTypeId);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadItemDeadStockData")]
        public async Task<IActionResult> ReadItemDeadStockData(DateTime fromDate, DateTime toDate, string warehouseId, string itemTypeId, string itemCategoryId, string itemSubCategoryId, string itemId)
        {
            //var data = await _inventoryManagementBusiness.ReadItemCurrentStockData(warehouseId, itemTypeId, itemCategoryId, itemSubCategoryId, itemId);
            var data = new List<ItemStockViewModel>();
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadInvoiceDetailsData")]
        public async Task<IActionResult> ReadInvoiceDetailsData(DateTime fromDate, DateTime toDate)
        {
            //var data = new List<POInvoiceViewModel>();
            var data = await _inventoryManagementBusiness.ReadInvoiceDetailsData(fromDate, toDate);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadIssueTypeData")]
        public async Task<IActionResult> ReadIssueTypeData(DateTime fromDate, DateTime toDate, string issueTypeId, string departmentId, string employeeId, string issueToTypeId)
        {
            //var data = new List<RequisitionIssueItemsViewModel>();
            var data = await _inventoryManagementBusiness.ReadIssueTypeData(fromDate, toDate, issueTypeId, departmentId, employeeId, issueToTypeId);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadItemHistoryData")]
        public async Task<IActionResult> ReadItemHistoryData(DateTime fromDate, DateTime toDate, string warehouseId, string itemTypeId, string itemCategoryId, string itemSubCategoryId, string itemId)
        {
            //var data = new List<ItemsViewModel>();
            var data = await _inventoryManagementBusiness.ReadItemHistoryData(fromDate, toDate, warehouseId, itemTypeId, itemCategoryId, itemSubCategoryId, itemId);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadItemTransferData")]
        public async Task<IActionResult> ReadItemTransferData(DateTime fromDate, DateTime toDate, string warehouseId, string itemTypeId, string itemCategoryId, string itemSubCategoryId, string itemId)
        {
            //var data = new List<ItemsViewModel>();
            var data = await _inventoryManagementBusiness.ReadItemTransferData(fromDate, toDate, warehouseId, itemTypeId, itemCategoryId, itemSubCategoryId, itemId);
            return Ok(data);
        }
        [HttpGet]
        [Route("ReadOrderBookData")]
        public async Task<IActionResult> ReadOrderBookData()
        {
            //var data = new List<ItemsViewModel>();
            var data = await _inventoryManagementBusiness.ReadOrderBookData();
            return Ok(data);
        }

        //Order Status

        [HttpGet]
        [Route("GetInvertoryFinancialYear")]
        public async Task<IActionResult> GetInvertoryFinancialYear()
        {
            var data = await _inventoryManagementBusiness.ReadInvertoryFinancialYearIdNameList();
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadOrderStatusData")]
        public async Task<IActionResult> ReadOrderStatusData(string financialYearId)
        {
            var data = await _inventoryManagementBusiness.ReadOrderStatusData(financialYearId);
            return Ok(data);
        }

        //purchse order status

        [HttpGet]
        [Route("ReadPurchaseOrderStatusData")]
        public async Task<IActionResult> ReadPurchaseOrderStatusData(DateTime fromDate, DateTime toDate, string statusId)
        {
            //var data = new List<PurchaseOrderViewModel>();
            var data = await _inventoryManagementBusiness.ReadPurchaseOrderStatusData(fromDate, toDate, statusId);
            return Ok(data);
        }

        //vewndor Category

        [HttpGet]
        [Route("ReadVendorCategoryDataReport")]
        public async Task<IActionResult> ReadVendorCategoryDataReport(string venId, string catId, string subCatId)
        {
            var data = await _inventoryManagementBusiness.ReadVendorCategoryData(venId, catId, subCatId);
            return Ok(data);
        }

        //Return to Vendor


        [HttpGet]
        [Route("ReadReturnToVendorData")]
        public async Task<IActionResult> ReadReturnToVendorData(string venId, DateTime From, DateTime To)
        {
            var data = await _inventoryManagementBusiness.ReadReturnToVendorData(From, To, venId);
            return Ok(data);
        }

        //Requisition Status/Detail

        [HttpGet]
        [Route("ReadRequisitionByStatusData")]
        public async Task<IActionResult> ReadRequisitionByStatusData(string cusId, string typeId, string statusId, DateTime From, DateTime To)
        {
            var data = await _inventoryManagementBusiness.ReadRequisitionByStatusData(From, To, typeId, cusId, statusId);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadRequisitionByDetailsData")]
        public async Task<IActionResult> ReadRequisitionByDetailsData(string cusId, string typeId, string statusId, DateTime From, DateTime To)
        {
            var data = await _inventoryManagementBusiness.ReadRequisitionByDetailsData(From, To, typeId, cusId, statusId);
            return Ok(data);
        }

        //receive from po

        [HttpGet]
        [Route("ReadReceivedFromPOData")]
        public async Task<IActionResult> ReadReceivedFromPOData(string venId, DateTime From, DateTime To)
        {
            var data = await _inventoryManagementBusiness.ReadReceivedFromPOData(From, To, venId);
            return Ok(data);
        }
        #endregion

        #region Masters

        [HttpGet]
        [Route("GetItemUnitList")]
        public async Task<IActionResult> GetItemUnitList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_ITEM_UOM", "");
            return Ok(data);
        }

        [HttpGet]
        [Route("GetItemCategoryList")]
        public async Task<IActionResult> GetItemCategoryList(string itemTypeId = null)
        {
            string where = "";
            if (itemTypeId.IsNotNullAndNotEmpty())
            {
                where = $@" and ""N_IMS_ITEM_CATEGORY"".""ItemType"" = '{itemTypeId}'";
            }
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_ITEM_CATEGORY", "", where);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetItemSubCategoryList")]
        public async Task<IActionResult> GetItemSubCategoryList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_ITEM_SUB_CATEGORY", "");
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadItemsData")]
        public async Task<IActionResult> ReadItemsData(string itemTypeId, string itemCategory, string itemSubCategory, string itemStatus)
        {
            ItemsSearchViewModel model = new ItemsSearchViewModel();
            model.ItemTypeId = itemTypeId;
            model.ItemCategoryId = itemCategory;
            model.ItemSubCategoryId = itemSubCategory;
            model.ItemStatusId = itemStatus;
            IList<ItemsViewModel> list = new List<ItemsViewModel>();
            list = await _inventoryManagementBusiness.GetItemsListData(model);
            return Ok(list);
        }

        [HttpGet]
        [Route("CreateItem")]
        public async Task<IActionResult> CreateItem(string id, bool isOpenItem = false)
        {
           
            ItemsViewModel model = new ItemsViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _inventoryManagementBusiness.GetItemsDetails(id);
                model.DataAction = DataActionEnum.Edit;
                model.IsOpenItem = isOpenItem;
                //model.IsSerializable = BoolStatus.No;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.IsSerializable = BoolStatus.No;
            }

            return Ok( model);
        }

        [HttpPost]
        [Route("ManageItem")]
        public async Task<IActionResult> ManageItem(ItemsViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "IMS_ITEM_MASTER";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    notemodel.NoteSubject = model.ItemName;
                    notemodel.NoteDescription = model.ItemSpecification;
                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        return Ok(new { success = true });
                    }
                    return Ok(new { success = false, error = ModelState });
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.NoteId = model.NoteId;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Edit;
                    notemodel.NoteSubject = model.ItemName;
                    notemodel.NoteDescription = model.ItemSpecification;
                    if (model.IsOpenItem)
                    {
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    }
                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        return Ok(new { success = true });
                    }
                    return Ok(new { success = false, error = ModelState });
                }
            }
            return Ok(new { success = false, error = ModelState});
        }

        //Item Shelf
        [HttpGet]
        [Route("ReadShelfList")]
        public async Task<IActionResult> ReadShelfList()
        {
            var data = await _inventoryManagementBusiness.ReadShelfList();
            return Ok(data);
        }

        [HttpGet]
        [Route("ShelfCategoryMapping")]
        public IActionResult ShelfCategoryMapping(string itemshelfId)
        {
            ItemShelfViewModel model = new ItemShelfViewModel();
            model.ShelfId = itemshelfId;
            return Ok(model);

        }

        [HttpGet]
        [Route("ReadItemCategoryNotInShelfCategoryData")]
        public async Task<IActionResult> ReadItemCategoryNotInShelfCategoryData(string itemshelfId)
        {
            var data = await _inventoryManagementBusiness.ReadCategoryNotInItemShelfCategoryData(itemshelfId);
            return Ok(data);
        }

        [HttpGet]
        [Route("ManageItemShelfCategoryMapping")]
        public async Task<IActionResult> ManageItemShelfCategoryMapping(string itemshelfId, string ids,string userId)
        {
            await Authenticate(userId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        dynamic exo = new System.Dynamic.ExpandoObject();

                        ((IDictionary<String, Object>)exo).Add("ShelfId", itemshelfId);
                        ((IDictionary<String, Object>)exo).Add("CategoryId", item.Trim());
                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        var result = await _noteBusiness.ManageNote(notemodel);
                        //if (!result.IsSuccess)
                        //{
                        //    return Json(new { success = false, error = result.Messages });
                        //}
                    }

                }

            }

            return Ok(new { success = true });
        }


        [HttpGet]
        [Route("ReadStockEntriesData")]

        public async Task<IActionResult> ReadStockEntriesData(string itemId, string warehouseId, DateTime? FromDate, DateTime? ToDate)
        {
            var data = await _inventoryManagementBusiness.ReadStockEntriesData(itemId, warehouseId, FromDate, ToDate);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadItemShelfCategoryData")]
        public async Task<IActionResult> ReadItemShelfCategoryData(string itemshelfId)
        {
            var list = await _inventoryManagementBusiness.ReadItemShelfCategoryData(itemshelfId);
            return Ok(list);
        }

        [HttpGet]
        [Route("GetItemShelfDetial")]
        public async Task<IActionResult> GetItemShelfDetail(string noteId)
        {
            var data = await _inventoryManagementBusiness.GetItemShelfDetail(noteId);
            return Ok(data);
        }
        [HttpPost]
        [Route("ManageItemShelf")]
        public async Task<IActionResult> ManageItemShelf(ItemShelfViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var shelflist = await _inventoryManagementBusiness.ReadShelfList();
            var exist = shelflist.Where(x => x.ShelfNo == model.ShelfNo && x.ShelfLocation == model.ShelfLocation && x.NoteId != model.NoteId).ToList();
            if (exist.Count > 0)
            {
                return Ok(new { success = false, error = "Shelf No already exist in this location" });
            }
            if (model.DataAction == DataActionEnum.Create)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "IMS_ITEM_SHELF";
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("ShelfNo", model.ShelfNo);
                ((IDictionary<String, Object>)exo).Add("ShelfLocation", model.ShelfLocation);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                var result = await _noteBusiness.ManageNote(notemodel);

                return Ok(new { success = result.IsSuccess, error = result.Messages });
            }
            else
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.NoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("ShelfNo", model.ShelfNo);
                ((IDictionary<String, Object>)exo).Add("ShelfLocation", model.ShelfLocation);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                var result = await _noteBusiness.ManageNote(notemodel);

                return Ok(new { success = result.IsSuccess, error = result.Messages });
            }

        }

        [HttpGet]
        [Route("DeleteItemShelfCategories")]
        public async Task<IActionResult> DeleteItemShelfCategories(string ids)
        {
            if (ids.IsNotNullAndNotEmpty())
            {
                var id = ids.Split(",");
                foreach (var data in id)
                {
                    await _inventoryManagementBusiness.DeleteItemShelfCategories(data);
                }
            }
            return Ok(new { success = true });
        }


        [HttpPost]
        [Route("ReadItemStockData")]
        public async Task<IActionResult> ReadItemStockData(string itemTypeId, string itemCategory, string itemSubCategory, string warehouseId)
        {
            var data = await _inventoryManagementBusiness.ReadItemStockData(itemTypeId, itemCategory, itemSubCategory, warehouseId);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadIssuedItems")]
        public async Task<IActionResult> ReadIssuedItems(string serviceId, ImsIssueTypeEnum issueType)
        {
            var data = await _inventoryManagementBusiness.GetRequisistionIssueItems(serviceId, issueType);
            return Ok(data);
        }


        [HttpPost]
        [Route("SaveSerialNo")]
        public async Task<IActionResult> SaveSerialNo(SubmitSerialNo submitSerialNoModel)
        {
            await Authenticate(submitSerialNoModel.userId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            if (submitSerialNoModel.serialNosData.Count>0)
            {
                var item = submitSerialNoModel.serialNosData;
                foreach (var model in item)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.NoteId = model.NoteId;
                    noteTempModel.SetUdfValue = true;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                    var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                    rowData["SerialNo"] = model.SerialNo;
                    rowData["Specification"] = model.Specification;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                }
                return Ok(new { success = true });
            }
            return Ok(new { success = false, error = "Please provide the details" });
        }

        [HttpGet]
        [Route("EditUnitItem")]
        public async Task<IActionResult> EditUnitItem(string Id, string warehouseId)
        {
            var isExists = await _inventoryManagementBusiness.CheckItemStockExists(Id, warehouseId);
            var data = await _inventoryManagementBusiness.GetUnitItemData(Id);
            if (isExists)
            {
                //var warehouseName = await _inventoryManagementBusiness.GetWarehouseNameById(Id);
                data.DataAction = DataActionEnum.Edit;
            }
            else
            {
                data.DataAction = DataActionEnum.Create;
                data.ItemId = Id;
                data.WarehouseId = warehouseId;
            }
            data.AdditionalInfo = "Opening Balance";
            return Ok(data);
        }

        [HttpPost]
        [Route("ManageEditUnitItem")]
        public async Task<IActionResult> ManageEditUnitItem(ItemStockViewModel model)
        {
            await Authenticate(model.OwnerUserId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            if (model.DataAction == DataActionEnum.Edit)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.NoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("UnitRate", model.UnitRate);
                ((IDictionary<String, Object>)exo).Add("MinimumQuantity", model.MinimumQuantity);
                ((IDictionary<String, Object>)exo).Add("MaximumQuantity", model.MaximumQuantity);
                ((IDictionary<String, Object>)exo).Add("OpeningQuantity", model.OpeningQuantity);
                ((IDictionary<String, Object>)exo).Add("ItemId", model.ItemId);
                ((IDictionary<String, Object>)exo).Add("AdditionalInfo", model.AdditionalInfo);
                ((IDictionary<String, Object>)exo).Add("TransactionDate", model.TransactionDate);
                ((IDictionary<String, Object>)exo).Add("BalanceQuantity", model.BalanceQuantity);
                ((IDictionary<String, Object>)exo).Add("ClosingQuantity", model.ClosingQuantity);
                ((IDictionary<String, Object>)exo).Add("WarehouseId", model.WarehouseId);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                notemodel.NoteStatusCode = "NOTE_STATUS_DRAFT";
                var result = await _noteBusiness.ManageNote(notemodel);
                if (!result.IsSuccess)
                {
                    return Ok(new { success = false, error = result.Messages });
                }
            }
            else
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "ITEM_STOCK";
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("UnitRate", model.UnitRate);
                ((IDictionary<String, Object>)exo).Add("MinimumQuantity", model.MinimumQuantity);
                ((IDictionary<String, Object>)exo).Add("MaximumQuantity", model.MaximumQuantity);
                ((IDictionary<String, Object>)exo).Add("ItemId", model.ItemId);
                ((IDictionary<String, Object>)exo).Add("OpeningQuantity", model.OpeningQuantity);
                ((IDictionary<String, Object>)exo).Add("BalanceQuantity", model.BalanceQuantity);
                ((IDictionary<String, Object>)exo).Add("ClosingQuantity", model.BalanceQuantity);
                ((IDictionary<String, Object>)exo).Add("AdditionalInfo", model.AdditionalInfo);
                ((IDictionary<String, Object>)exo).Add("TransactionDate", model.TransactionDate);
                ((IDictionary<String, Object>)exo).Add("WarehouseId", model.WarehouseId);

                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                notemodel.NoteStatusCode = "NOTE_STATUS_DRAFT"; /*DRAFT*/
                var result = await _noteBusiness.ManageNote(notemodel);
                if (!result.IsSuccess)
                {
                    return Ok(new { success = false, error = result.Messages });
                }
                return Ok(new { success = true });
            }
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("SubmitUnitItem")]
        public async Task<IActionResult> SubmitUnitItem(string stockId, string itemId, string noteId, string isSerializable,string userId)
        {
            await Authenticate(userId, "InventoryManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var data = await _inventoryManagementBusiness.GetStockDetailsById(stockId);
            if (isSerializable == "4")
            {
                var serialCount = await _inventoryManagementBusiness.GetSerailNoByHeaderIdandReferenceId(stockId, stockId);
                if (serialCount.Count != data.BalanceQuantity)
                {
                    return Ok(new { success = false, error = "Create serial no.s for all items." });
                }
            }

            // Item Master Note Status Updated
            var itemmaster = await _inventoryManagementBusiness.GetItemsDetails(itemId);
            if (itemmaster != null)
            {
                var itemNoteTempModel = new NoteTemplateViewModel();
                itemNoteTempModel.DataAction = DataActionEnum.Edit;
                itemNoteTempModel.ActiveUserId = _userContext.UserId;
                itemNoteTempModel.NoteId = itemmaster.NoteId;
                itemNoteTempModel.SetUdfValue = true;
                itemNoteTempModel.AllowPastStartDate = true;
                var itemNotemodel = await _noteBusiness.GetNoteDetails(itemNoteTempModel);
                var rowData = itemNotemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                // var itemExisting = await _inventoryManagementBusiness.GetStockDataByNoteId(noteId);
                //if (itemExisting != null)
                //{
                rowData["ClosingQuantity"] = itemmaster.ClosingQuantity + data.ClosingQuantity;
                itemNotemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                itemNotemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                var result = await _noteBusiness.ManageNote(itemNotemodel);

                if (!result.IsSuccess)
                {
                    return Ok(new { success = false, error = "Item status not updated" });
                }

            }

            // Item Stock Note Status Updated

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.NoteId = data.NtsNoteId;
            noteTempModel.AllowPastStartDate = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var existing = await _inventoryManagementBusiness.GetStockDataByNoteId(data.NtsNoteId);
            if (existing != null)
            {
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(existing);
                var result = await _noteBusiness.ManageNote(notemodel);

                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
                return Ok(new { success = false, error = "Item not submitted" });
            }
            return Ok(new { success = false, error = "Item does not exists." });
        }
        
        [HttpGet]
        [Route("ViewStockEntries")]
        public async Task<IActionResult> ViewStockEntries(string Id, string warehouseId)
        {
            var data = await _inventoryManagementBusiness.GetItemHeaderData(Id, warehouseId);
            data.WarehouseId = warehouseId;
            return Ok(data);
        }

        #endregion

    }
}
