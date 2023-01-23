using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.IMS.Controllers
{
    [Area("IMS")]
    public class InventoryMasterController : ApplicationController
    {
        private ICmsBusiness _cmsBusiness;
        private IInventoryManagementBusiness _inventoryManagementBusiness;
        private IUserContext _userContext;
        private INoteBusiness _noteBusiness;
        public InventoryMasterController(ICmsBusiness cmsBusiness, IInventoryManagementBusiness inventoryManagementBusiness
            , IUserContext userContext
            , INoteBusiness noteBusiness)
        {
            _cmsBusiness = cmsBusiness;
            _inventoryManagementBusiness = inventoryManagementBusiness;
            _userContext = userContext;
            _noteBusiness = noteBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ItemSelf()
        {
            var model = new ItemShelfViewModel()
            {
                DataAction = DataActionEnum.Create
            };
            return View(model);
        }
        public IActionResult UnitItem()
        {
            return View();
        }
        public async Task<IActionResult> EditUnitItem(string Id,string warehouseId)
        {
            var isExists = await _inventoryManagementBusiness.CheckItemStockExists(Id,warehouseId);
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
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> EditUnitItem(ItemStockViewModel model)
        {
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
                    return Json(new { success = false, error = result.Messages });
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
                    return Json(new { success = false, error = result.Messages });
                }
                return Json(new { success = true });
            }
            return Json(new { success = true });
        }

        public async Task<IActionResult> SubmitUnitItem(string stockId, string itemId, string noteId, string isSerializable)
        {
            var data = await _inventoryManagementBusiness.GetStockDetailsById(stockId);
            if (isSerializable == "4")
            {
                var serialCount = await _inventoryManagementBusiness.GetSerailNoByHeaderIdandReferenceId(stockId, stockId);
                if (serialCount.Count != data.BalanceQuantity)
                {
                    return Json(new { success = false, error = "Create serial no.s for all items." });
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
                    return Json(new { success = false, error = "Item status not updated" });
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
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = "Item not submitted" });
            }
            return Json(new { success = false, error = "Item does not exists." });
        }

        public async Task<IActionResult> ViewStockEntries(string Id,string warehouseId)
        {
            var data = await _inventoryManagementBusiness.GetItemHeaderData(Id, warehouseId);
            data.WarehouseId = warehouseId;        
            return View(data);
        }
        public async Task<IActionResult> GetItemDeatils(string Id, string warehouseId)
        {
            var data = await _inventoryManagementBusiness.GetItemHeaderData(Id, warehouseId);
            return Json(data);
        }
        public async Task<IActionResult> ReadItemStockData(string itemTypeId, string itemCategory, string itemSubCategory, string warehouseId)
        {
            var data = await _inventoryManagementBusiness.ReadItemStockData(itemTypeId, itemCategory, itemSubCategory, warehouseId);
            return Json(data);
        }
        
        public async Task<IActionResult> ReadItemListByStock(string itemTypeId, string itemCategory, string itemSubCategory, string warehouseId)
        {
            var data = await _inventoryManagementBusiness.ReadItemListByStock(itemTypeId, itemCategory, itemSubCategory, warehouseId);
            return Json(data);
        }

        public async Task<IActionResult> ReadItemCurrentStockData(string warehouseId, string itemTypeId, string itemCategoryId, string itemSubCategoryId, string itemId)
        {
            var data = await _inventoryManagementBusiness.ReadItemCurrentStockData(warehouseId, itemTypeId, itemCategoryId, itemSubCategoryId, itemId);
            return Json(data);
        }
        public async Task<IActionResult> ReadItemDeadStockData(DateTime fromDate,DateTime toDate,string warehouseId, string itemTypeId, string itemCategoryId, string itemSubCategoryId, string itemId)
        {
            //var data = await _inventoryManagementBusiness.ReadItemCurrentStockData(warehouseId, itemTypeId, itemCategoryId, itemSubCategoryId, itemId);
            var data = new List<ItemStockViewModel>(); 
            return Json(data);
        }

        public async Task<IActionResult> ReadShelfList()
        {
            var data = await _inventoryManagementBusiness.ReadShelfList();
            return Json(data);
        }
        public async Task<IActionResult> GetItemCodeMappingList()
        {
            var data = await _inventoryManagementBusiness.GetItemCodeMappingList();
            return Json(data);
        }
        public async Task<IActionResult> GetItemShelfDetail(string noteId)
        {
            var data = await _inventoryManagementBusiness.GetItemShelfDetail(noteId);
            return Json(data);
        }
        public async Task<IActionResult> ManageItemShelf(ItemShelfViewModel model)
        {
            var shelflist = await _inventoryManagementBusiness.ReadShelfList();
            var exist = shelflist.Where(x => x.ShelfNo == model.ShelfNo && x.ShelfLocation == model.ShelfLocation && x.NoteId != model.NoteId).ToList();
            if (exist.Count > 0)
            {
                return Json(new { success = false, error = "Shelf No already exist in this location" });
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
                
                return Json(new { success = result.IsSuccess, error = result.Messages });
            }
            else {                
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

                    return Json(new { success = result.IsSuccess, error = result.Messages });
            }
            
        }
        public IActionResult ItemIndex()
        {
            return View();
        }
        public async Task<IActionResult> CreateItem(string id,bool isOpenItem=false)
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

            return View("ManageItem",model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageItem(ItemsViewModel model)
        {
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
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }else if (model.DataAction == DataActionEnum.Edit)
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
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public async Task<IActionResult> GetItemCategoryList(string itemTypeId=null)
        {
            string where = "";
            if (itemTypeId.IsNotNullAndNotEmpty())
            {
                where = $@" and ""N_IMS_ITEM_CATEGORY"".""ItemType"" = '{itemTypeId}'";
            }
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_ITEM_CATEGORY", "",where);
            return Json(data);
        }
        public async Task<IActionResult> GetItemCategoryListByItemType(string itemTypeId)
        {
            var data = await _inventoryManagementBusiness.GetItemCategoryByItemTypeId(itemTypeId);
            return Json(data);
        }
        public async Task<IActionResult> GetItemSubCategoryListByItemCategory(string itemCategoryId)
        {
            var data = await _inventoryManagementBusiness.GetItemSubCategoryByItemCategoryId(itemCategoryId);
            return Json(data);
        }
        public async Task<IActionResult> GetItemListByItemSubCategory(string itemSubCategoryId)
        {
            var data = await _inventoryManagementBusiness.GetItemByItemSubCategoryId(itemSubCategoryId);
            return Json(data);
        }
        public async Task<IActionResult> GetItemSubCategoryList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_ITEM_SUB_CATEGORY", "");
            return Json(data);
        }
        public async Task<IActionResult> GetItemUnitList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_ITEM_UOM", "");
            return Json(data);
        }
        public async Task<IActionResult> GetWarehouseList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_WAREHOUSE_MASTER", "");
            return Json(data);
        }
        public async Task<IActionResult> ReadItemsData(string itemTypeId, string itemCategory, string itemSubCategory, string itemStatus)
        {
            ItemsSearchViewModel model = new ItemsSearchViewModel();
            model.ItemTypeId = itemTypeId;
            model.ItemCategoryId = itemCategory;
            model.ItemSubCategoryId = itemSubCategory;
            model.ItemStatusId = itemStatus;
            IList<ItemsViewModel> list = new List<ItemsViewModel>();
            list = await _inventoryManagementBusiness.GetItemsListData(model);
            return Json(list);
        }

        public async Task<IActionResult> VendorContacts(string VendorId)
        {
            ContactsViewModel model = new ContactsViewModel();
            var vendor = await _inventoryManagementBusiness.GetVendorDetailsById(VendorId);
            if (vendor.IsNotNull())
            {
                model.VendorName = vendor.VendorName; 
            }
            model.VendorId = VendorId;
            return View(model);
        }
        public async Task<IActionResult> VendorCategory(string VendorId)
        {
            VendorCategoryViewModel model = new VendorCategoryViewModel();
            var vendor = await _inventoryManagementBusiness.GetVendorDetailsById(VendorId);
            if (vendor.IsNotNull())
            {
                model.VendorName = vendor.VendorName;
            }
            model.VendorId = VendorId;
            return View(model);
        }
        public async Task<IActionResult> ItemShelfCategory(string ItemShelfId)
        {
            ItemShelfViewModel model = new ItemShelfViewModel();
            var itemShelf = await _inventoryManagementBusiness.GetItemShelfDetailsById(ItemShelfId);
            if (itemShelf.IsNotNull())
            {
                model.ShelfNo = itemShelf.ShelfNo;
            }
            model.ShelfId = ItemShelfId;
            return View(model);
        }
        public async Task<IActionResult> ReadItemCategoryNotInVendorData(string VendorId)
        {
            var data = await _inventoryManagementBusiness.ReadCategoryNotInVendorCategoryData(VendorId);
            return Json(data);
        }
        public async Task<IActionResult> ReadVendorContactsData(string vendorId)
        {
            var list = await _inventoryManagementBusiness.ReadVendorContactsData(vendorId);
            return Json(list);
        }
        public async Task<IActionResult> ReadVendorCategoryData(string vendorId)
        {
            var list = await _inventoryManagementBusiness.ReadVendorCategoryData(vendorId);
            return Json(list);
        }
        public async Task<IActionResult> DeleteContacts(string ids)
            {
            if (ids.IsNotNullAndNotEmpty())
            {
                var id=ids.Split(",");
                foreach (var data in id)
                {
                     await _inventoryManagementBusiness.DeleteContacts(data);                   
                }
            }
            return Json(new { success=true});
        }
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
            return Json(new { success = true });
        }
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
            return Json(new { success = true });
        }
        public async Task<IActionResult> DeletePOItem(string poId)
        {
            if (poId.IsNotNullAndNotEmpty())
            { 
            
                    await _inventoryManagementBusiness.DeletePOItem(poId);
            }
            return Json(new { success = true });
        }

        public async Task<IActionResult> CustomerContacts(string customerId)
        {
            ContactsViewModel model = new ContactsViewModel();
            var customer = await _inventoryManagementBusiness.GetCustomerDetailsById(customerId);
            if (customer.IsNotNull())
            {
                model.CustomerName = customer.CustomerName;
            }
            model.CustomerId = customerId;
            return View(model);
        }

        public async Task<IActionResult> ReadCustomerContactsData(string customerId)
        {
            var list = await _inventoryManagementBusiness.ReadCustomerContactsData(customerId);
            return Json(list);
        }
        public async Task<IActionResult> DeleteCustomerContacts(string ids)
        {
            if (ids.IsNotNullAndNotEmpty())
            {
                var id = ids.Split(",");
                foreach (var data in id)
                {
                    await _inventoryManagementBusiness.DeleteCustomerContacts(data);
                }
            }
            return Json(new { success = true });
        }
        public async Task<IActionResult> ShelfCategoryMapping(string ItemShelfId)
        {
            ItemShelfViewModel model = new ItemShelfViewModel();
            var itemShelf = await _inventoryManagementBusiness.GetItemShelfDetailsById(ItemShelfId);
            if (itemShelf.IsNotNull())
            {
                model.ShelfNo = itemShelf.ShelfNo;
            }
            model.ShelfId = ItemShelfId;
            return View(model);
        }
        public async Task<IActionResult> ReadItemShelfCategoryData(string itemshelfId)
        {
            var list = await _inventoryManagementBusiness.ReadItemShelfCategoryData(itemshelfId);
            return Json(list);
        }
        public async Task<IActionResult> ReadItemCategoryNotInShelfCategoryData(string itemshelfId)
        {
            var data = await _inventoryManagementBusiness.ReadCategoryNotInItemShelfCategoryData(itemshelfId);
            return Json(data);
        }
        public async Task<IActionResult> ReadStockEntriesData(string itemId, string warehouseId, DateTime? FromDate, DateTime? ToDate)
        {
            var data = await _inventoryManagementBusiness.ReadStockEntriesData(itemId, warehouseId, FromDate, ToDate);
            return Json(data);
        }
        
    }
}
