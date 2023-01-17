using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
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
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
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
        public async Task<IActionResult> GetItemCategoryList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("IMS_ITEM_CATEGORY", "");
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
    }
}
