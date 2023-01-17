using CMS.Business;
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CHR.Controllers
{
    [Area("CHR")]
    public class BusinessHierarchyController : ApplicationController
    {
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IUserContext _userContext;
        private readonly IHybridHierarchyBusiness _hybridHierarchyBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IUserHierarchyPermissionBusiness _userHierPerBusiness;
        private readonly IHierarchyMasterBusiness _hierarchyMasterBusiness;
        public BusinessHierarchyController(IHRCoreBusiness hrCoreBusiness, IUserBusiness userBusiness,
            IUserContext userContext, IHybridHierarchyBusiness hybridHierarchyBusiness, INoteBusiness noteBusiness,
            IUserHierarchyPermissionBusiness userHierPerBusiness, IHierarchyMasterBusiness hierarchyMasterBusiness)
        {
            _hrCoreBusiness = hrCoreBusiness;
            _userBusiness = userBusiness;
            _userContext = userContext;
            _hybridHierarchyBusiness = hybridHierarchyBusiness;
            _noteBusiness = noteBusiness;
            _userHierPerBusiness = userHierPerBusiness;
            _hierarchyMasterBusiness = hierarchyMasterBusiness;

        }
        public async Task<IActionResult> Index(string permissions, string portalId, string hierarchyCode, bool isIframe = false, bool enableAOR = false, string bulkRequestId = "", bool isBulkRequest = false, bool isDisplayOnly = false, string bulkRequestStatusCode = "")
        {
            var date = DateTime.Today;

            var rootNodes = await _userBusiness.GetUserHierarchyRootId(_userContext.UserId, hierarchyCode, _userContext.UserId);
            var hierarchyId = rootNodes.Item3;
            var HierarchyRootNodeId = rootNodes.Item1;
            var AllowedRootNodeId = rootNodes.Item2;
            if (_userContext.IsSystemAdmin)
            {
                AllowedRootNodeId = "-1";
                //AllowedRootNodeId = "bfbc8a12-f920-4bed-b22f-2c2cc1f7bdc1";
            }

            var viewModel = new HybridHierarchyViewModel
            {
                HierarchyId = hierarchyId,
                HierarchyRootNodeId = HierarchyRootNodeId,
                AllowedRootNodeId = AllowedRootNodeId,
                CanAddRootNode = HierarchyRootNodeId == AllowedRootNodeId && HierarchyRootNodeId == "",
                AllowedRootNodeLevel = 0,
                AsOnDate = date.ToYYYY_MM_DD_DateFormat(),
            };
            ViewBag.PortalId = portalId;
            ViewBag.IsAsOnDate = date.ToYYYY_MM_DD_DateFormat();
            ViewBag.EnableAOR = enableAOR;
            ViewBag.BulkRequestId = bulkRequestId;
            ViewBag.BulkRequestStatusCode = bulkRequestStatusCode;
            ViewBag.IsBulkRequest = isBulkRequest;
            ViewBag.IsDisplayOnly = isDisplayOnly;
            if (permissions != null && permissions.Contains("CAN_MANAGE_AOR"))
            {
                ViewBag.IsManageAOR = true;
            }
            if (permissions != null && permissions.Contains("CAN_MANAGE_PERMISSIONS"))
            {
                ViewBag.IsManagePermissions = true;
            }
            if (permissions != null && permissions.Contains("CAN_MANAGE_CHANGE_REQUEST"))
            {
                ViewBag.IsManageChangeRequest = true;
            }
            if (permissions != null && permissions.Contains("CAN_MANAGE_ALL_CONTEXTMENU"))
            {
                ViewBag.IsManageAllContextMenu = true;
            }
            if (permissions != null && permissions.Contains("CAN_MANAGE_DOWNLOAD"))
            {
                ViewBag.IsManageDownload = true;
            }
            ViewBag.AsOnDateDisplay = date.ToYYYY_MM_DD_DateFormat();
            if (isIframe)
            {
                ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
            }
            return View(viewModel);
        }

        public async Task<IActionResult> BHPermissionIndex(string portalId, string groupCode, LayoutModeEnum lo)
        {
            ViewBag.PortalId = portalId;
            var model = new BusinessHierarchyPermissionViewModel();
            if (groupCode.IsNotNullAndNotEmpty())
            {
                model.GroupCode = groupCode;
            }
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
            }
            return View(model);
        }
        public async Task<ActionResult> GetBHPermissionGridData(string groupCode)
        {
            var list = await _hybridHierarchyBusiness.GetBusinessHierarchyPermissionData(groupCode);
            var json = Json(list);
            return json;
        }
        public async Task<bool> DeleteBHPermission(string id, string ntsNoteId)
        {
            var model = new BusinessHierarchyPermissionViewModel
            {
                Id = id,
                NtsNoteId = ntsNoteId
            };
            await _hybridHierarchyBusiness.DeleteBusinessHierarchyPermission(model);
            await _noteBusiness.Delete(ntsNoteId);
            return true;
        }
        public async Task<IActionResult> BHServiceIndex(string portalId, bool showAllService)
        {
            ViewBag.PortalId = portalId;
            ViewBag.ShowAllService = showAllService;
            return View();
        }
        public async Task<ActionResult> GetBHServiceData(bool showAllService)
        {
            var list = await _hybridHierarchyBusiness.GetBHServiceData(showAllService);
            var json = Json(list);
            return json;
        }
        public async Task<IActionResult> BHTaskIndex(string portalId)
        {
            ViewBag.PortalId = portalId;
            return View();
        }

        public async Task<ActionResult> BHSearch(string portalId)
        {
            ViewBag.PortalId = portalId;
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetBHDataByReferenceType(string type,string text)
        {
            if(text == null)
            {
                text = "";
            }
            var list = await _hybridHierarchyBusiness.GetBusinessHierarchyList(type);
            if(text != "")
            {
                var list1 = list.Where(x => x.Name.ToLower().Contains(text.ToLower()));
                return Json(list1);
            }
            return Json(list);

        }

        public async Task<ActionResult> GetBHTaskData()
        {
            var list = await _hybridHierarchyBusiness.GetBHTaskData();
            var json = Json(list);
            return json;
        }
        public async Task<ActionResult> GetHybridHierachyChildList(string parentId, int level, int levelUpto, bool enableAOR, string bulkRequestId, bool includeParent)
        {
            var childList = await _hybridHierarchyBusiness.GetBusinessHierarchyChildList(parentId, level, levelUpto, enableAOR, bulkRequestId, includeParent);

            var json = Json(childList);
            return json;
        }

        public async Task<IActionResult> GetDepartmentByType(string type, string level)
        {
            try
            {
                var list = await _hrCoreBusiness.GetDepartmentByType(type, level);
                return Json(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<IActionResult> GetAllJobs()
        {
            try
            {
                var list = await _hrCoreBusiness.GetAllJobs();
                return Json(list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> GetAllPositions()
        {
            try
            {
                var list = await _hrCoreBusiness.GetAllPosition();
                return Json(list);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetAllCareerLevels()
        {
            try
            {
                var list = await _hrCoreBusiness.GetHRMasterList("Career Level");
                return Json(list);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetAllPersonList()
        {
            try
            {
                var list = await _hrCoreBusiness.GetAllActivePersonList();
                return Json(list);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetAllBusinessHierarchyList()
        {
            try
            {
                var list = await _hybridHierarchyBusiness.GetBusinessHierarchyList();
                return Json(list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetBusinessHierarchyOnReferenceType(string type)
        {
            var list = await _hybridHierarchyBusiness.GetBusinessHierarchyList(type);
            return Json(list);
         
        }

        [HttpGet]
        public async Task<JsonResult> ListFilterByRefType(string type)
        {
            var reference = type.ToEnum<BusinessHierarchyItemTypeEnum>().ToString();
            var list = await _hybridHierarchyBusiness.GetBusinessHierarchyList(reference);
            return Json(list);

        }

        public async Task<IActionResult> GetAllAORBusinessHierarchyList(string businessHierarchyId)
        {
            try
            {
                var list = await _hybridHierarchyBusiness.GetAllAORBusinessHierarchyList();
                if (businessHierarchyId.IsNotNullAndNotEmpty())
                {
                    list = list.Where(x => x.BusinessHierarchyId == businessHierarchyId).ToList();
                }
                return Json(list);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddExistingtoHybridHierarchy(string parentHierarchyId, string referenceId, string level, string type)
        {
            try
            {
                var model = new HybridHierarchyViewModel();
                var existingHierarchy = await _hybridHierarchyBusiness.GetSingleById(parentHierarchyId);
                if (existingHierarchy == null)
                {
                    var hm = await _noteBusiness.GetSingle<HierarchyMasterViewModel, HierarchyMaster>(x => x.Code == "BUSINESS_HIERARCHY");
                    existingHierarchy = new HybridHierarchyViewModel
                    {
                        LevelId = 0
                    };
                    if (hm != null)
                    {

                        existingHierarchy.HierarchyMasterId = hm.Id;
                    }
                }

                model.PortalId = _userContext.PortalId;
                model.ParentId = parentHierarchyId;
                model.ReferenceId = referenceId;
                model.LevelId = existingHierarchy.LevelId + 1;
                model.HierarchyMasterId = existingHierarchy.HierarchyMasterId;
                if (type.IsNotNullAndNotEmpty())
                {
                    model.ReferenceType = BusinessExtension.GetHybridHierarchyReferenceType(type);
                }
                else
                {
                    model.ReferenceType = BusinessExtension.GetHybridHierarchyReferenceType(level);
                }

                var result = await _hybridHierarchyBusiness.Create(model);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        //public IActionResult AddExistingChildItem(string parentHierarchyId, string level)
        //{
        //    var levelName = "";
        //    if (level.ToSafeInt() == 0)
        //    {
        //        levelName = "LEVEL1";
        //        return AddExistingDepartment(parentHierarchyId, "DEPARTMENT", levelName);
        //    }
        //    else if (level.ToSafeInt() == 1)
        //    {
        //        levelName = "LEVEL2";
        //        return AddExistingDepartment(parentHierarchyId, "DEPARTMENT", levelName);

        //    }
        //    else if (level.ToSafeInt() == 2)
        //    {
        //        levelName = "LEVEL3";
        //        return AddExistingDepartment(parentHierarchyId, "DEPARTMENT", levelName);

        //    }
        //    else if (level.ToSafeInt() == 3)
        //    {
        //        levelName = "LEVEL4";
        //        return AddExistingDepartment(parentHierarchyId, "DEPARTMENT", levelName);

        //    }
        //    else if (level.ToSafeInt() == 4)
        //    {
        //        levelName = "BRAND";
        //        return AddExistingDepartment(parentHierarchyId, "BRAND", null);

        //    }
        //    else if (level.ToSafeInt() == 5)
        //    {
        //        levelName = "MARKET";
        //        return AddExistingDepartment(parentHierarchyId, "MARKET", null);

        //    }
        //    else if (level.ToSafeInt() == 6)
        //    {
        //        levelName = "PROVINCE";
        //        return AddExistingDepartment(parentHierarchyId, "PROVINCE", null);

        //    }
        //    else if (level.ToSafeInt() == 7)
        //    {
        //        levelName = "Career Level";
        //        return AddExistingCareerLevel(parentHierarchyId, levelName);
        //    }
        //    else if (level.ToSafeInt() == 8)
        //    {
        //        levelName = "DEPARTMENT";
        //        return AddExistingDepartment(parentHierarchyId, "DEPARTMENT", null);
        //    }
        //    else if (level.ToSafeInt() == 9)
        //    {
        //        levelName = "JOB";
        //        return AddExistingJob(parentHierarchyId, levelName);
        //    }
        //    else if (level.ToSafeInt() == 9)
        //    {
        //        levelName = "Employee";
        //    }
        //    return null;
        //}

        public IActionResult AddExistingDepartment(string parentHierarchyId, string type, string level)
        {
            ViewBag.ParentHierarchyId = parentHierarchyId;
            ViewBag.Type = type;
            ViewBag.Level = level;
            return View();
        }
        public IActionResult AddExistingJob(string parentHierarchyId, string level, string type)
        {
            ViewBag.ParentHierarchyId = parentHierarchyId;
            ViewBag.Level = level;
            ViewBag.Type = type;
            return View();
        }
        public IActionResult AddExistingCareerLevel(string parentHierarchyId, string level, string type)
        {
            ViewBag.ParentHierarchyId = parentHierarchyId;
            ViewBag.Level = level;
            ViewBag.Type = type;
            return View();
        }
        public IActionResult AddExistingEmployee(string parentHierarchyId, string level, string type)
        {
            ViewBag.ParentHierarchyId = parentHierarchyId;
            ViewBag.Level = level;
            ViewBag.Type = type;
            return View();
        }
        public IActionResult AddExistingPosition(string parentHierarchyId, string level, string type)
        {
            ViewBag.ParentHierarchyId = parentHierarchyId;
            ViewBag.Level = level;
            ViewBag.Type = type;
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetHybridHierarchyDetails(string hybridHierarchyId)
        {
            var result = await _hybridHierarchyBusiness.GetSingleById(hybridHierarchyId);
            return Json(new { success = true, data = result.ReferenceId });
        }

        public async Task<JsonResult> DeleteFromHierarchy(string hybridHierarchyId)
        {
            await _hybridHierarchyBusiness.RemoveFromBusinessHierarchy(hybridHierarchyId);
            return Json(new { success = true });
        }

        public IActionResult BusinessHierarchyAOR(string portalId, string referenceType, string referenceId, string businessHierarchyId, LayoutModeEnum lo)
        {
            var model = new BusinessHierarchyAORViewModel();
            ViewBag.PortalId = portalId;
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
            }
            ViewBag.BusinessHierarchyId = businessHierarchyId;
            if (businessHierarchyId.IsNotNullAndNotEmpty())
            {
                ViewBag.FromHierarchy = true;
            }
            return View(model);
        }

        public async Task<bool> DeleteBusinessHierarchyAOR(string id, string ntsNoteId)
        {
            var model = new NoteTemplateViewModel
            {
                Id = id,
                NoteId = ntsNoteId
            };
            await _noteBusiness.DeleteAor(model);
            await _noteBusiness.Delete(ntsNoteId);
            return true;
        }
        public async Task<ActionResult> DownloadHybridHierarchy()
        {
            var hierarchydata = await _hybridHierarchyBusiness.DownloadHybridHierarchy();
            return File(hierarchydata, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        public IActionResult BusinessHierarchyDetails(string businessHierarchyItemId)
        {
            ViewBag.BusinessHierarchyId = businessHierarchyItemId;
            return View();
        }
        public async Task<IActionResult> GetBusinessHierarchyDetails(string businessHierarchyItemId)
        {
            try
            {
                var list = await _hybridHierarchyBusiness.GetHierarchyParentDetails(businessHierarchyItemId);
                return Json(list);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ActionResult> DownloadAORData(string businessHierarchyId)
        {
            var list = await _hrCoreBusiness.GetAllAORBusinessHierarchyList();
            if (businessHierarchyId.IsNotNullAndNotEmpty())
            {
                list = list.Where(x => x.BusinessHierarchyId == businessHierarchyId).ToList();
            }

            var hierarchydata = await _hybridHierarchyBusiness.DownloadAORdata(list);
            return File(hierarchydata, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        }
        public async Task<ActionResult> DownloadBusinessPartnerMappingData()
        {
            var list = await _hrCoreBusiness.GetBusinessPartnerMappingList();
            //var list = new List<BusinessHierarchyAORViewModel>();
            //list.Add(new BusinessHierarchyAORViewModel { BusinessPartnerName = "TestPartner01", DepartmentName = "Dept01" });
            //list.Add(new BusinessHierarchyAORViewModel { BusinessPartnerName = "TestPartner02", DepartmentName = "Dept01" });
            //list.Add(new BusinessHierarchyAORViewModel { BusinessPartnerName = "TestPartner03", DepartmentName = "Dept02" });
            //list.Add(new BusinessHierarchyAORViewModel { BusinessPartnerName = "TestPartner04", DepartmentName = "Dept03" });
            var hierarchydata = await _hybridHierarchyBusiness.DownloadBusinessPartnerMappingExcel(list);
            return File(hierarchydata, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        }
        public IActionResult BusinessHierarchyRootPermission()
        {
            var model = new UserHierarchyPermissionViewModel();
            return View(model);
        }
        public async Task<IActionResult> GetBusinessHierarchyRootPermission()
        {
            try
            {
                var list = await _hybridHierarchyBusiness.GetBusinessHierarchyRootPermission();
                return Json(list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> ManageBusinessHierarchyRootPermission(string Id,string UserId)
        {
            if (Id.IsNotNullAndNotEmpty())
            {
               
                var list = await _hybridHierarchyBusiness.GetBusinessHierarchyRootPermission(Id,UserId);
                var hierarchyPermissionData = list.Where(x => x.Id==Id).Single();

                //var type = list.Where(x=>x.UserId == member.UserId);
                //member.ReferenceType = type.Select(x=>x.ReferenceType).ToString();
                hierarchyPermissionData.DataAction = DataActionEnum.Edit;
                return View("ManageBusinessHierarchyRootPermission", hierarchyPermissionData);
            }
            else
            {
                var model = new UserHierarchyPermissionViewModel();
                model.HierarchyPermission = HierarchyPermissionEnum.Custom;
                model.DataAction = DataActionEnum.Create;
                return View("ManageBusinessHierarchyRootPermission", model);
            }

        }
        [HttpPost]
        public async Task<IActionResult> ManageBusinessHierarchyRootPermission(UserHierarchyPermissionViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    model.HierarchyPermission = HierarchyPermissionEnum.Custom;
                    var hierarchy = await _hierarchyMasterBusiness.GetSingle(x => x.Code == "BUSINESS_HIERARCHY");
                    var hierarchyId = hierarchy.Id;
                    model.HierarchyId = hierarchyId;
                    var result = await _userHierPerBusiness.Create(model);

                    if (result.IsSuccess)
                    {

                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    model.HierarchyPermission = HierarchyPermissionEnum.Custom;
                    var hierarchy = await _hierarchyMasterBusiness.GetSingle(x => x.Code == "BUSINESS_HIERARCHY");
                    var hierarchyId = hierarchy.Id;
                    model.HierarchyId = hierarchyId;

                    var result = await _userHierPerBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;


                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("ManageBusinessHierarchyRootPermission", model);
        }
        public async Task<IActionResult> DeleteUserHierarchyPermission(string Id)
        {
            await _userHierPerBusiness.Delete(Id);

            return Json(new { success = true });
        }

        public IActionResult MoveToOtherParent(string curParentId, string refType, string curNodeId)
        {
            ViewBag.CurrentParentId = curParentId;
            ViewBag.RefType = refType;
            ViewBag.HierarchyId = curNodeId;
            return View();
        }

        [HttpPost]
        public async Task<bool> MoveToOtherParent(string curNodeId, string newParentId)
        {
            await _hybridHierarchyBusiness.MoveItemToNewParent(curNodeId, newParentId);
            return true;
        }

        public IActionResult GetParentType(string type)
        {
            List<IdNameViewModel> list = new List<IdNameViewModel>();

            string[] udf = { "LEVEL1", "LEVEL2", "LEVEL3", "LEVEL4", "BRAND", "MARKET", "PROVINCE", "DEPARTMENT", "CAREERLEVEL", "JOB", "POSITION", "EMPLOYEE" };

            for (int i = 0; i < udf.Length; i++)
            {
                if (type == udf[i])
                {
                    break;
                }
                else
                {
                    var item = new IdNameViewModel()
                    {
                        Name = udf[i],
                        Id = udf[i]
                    };
                    list.Add(item);
                }
            }

            return Json(list);

        }
        public async Task<IActionResult> GetUserNotInPermissionHierarchy(string UserId)
        {
                var list = await _userHierPerBusiness.GetUserNotInPermissionHierarchy(UserId);
                return Json(list);
        }


    }

}
