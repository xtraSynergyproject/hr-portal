using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class UserHierarchyController : ApplicationController
    {
        private IHierarchyMasterBusiness _hierarchyMasterBusiness;
        private IUserHierarchyBusiness _userHierarchyBusiness;
        private IUserBusiness _userBusiness;
        private IUserContext _userContext;
        public UserHierarchyController(IHierarchyMasterBusiness hierarchyMasterBusiness, IUserBusiness userBusiness,
            IUserHierarchyBusiness userHierarchyBusiness, IUserContext userContext)
        {
            _hierarchyMasterBusiness = hierarchyMasterBusiness;
            _userBusiness = userBusiness;
            _userHierarchyBusiness = userHierarchyBusiness;
            _userContext = userContext;
        }
        public async Task<IActionResult> Index(string hierarchyId,string userid,string layout)
        {
            var list = new List<HierarchyMasterViewModel>();
            if (hierarchyId.IsNotNullAndNotEmpty())
            {
                list= await _hierarchyMasterBusiness.GetList(x=>x.Id==hierarchyId);
            }
            else
            {
                list = await _hierarchyMasterBusiness.GetList(x=> x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);
                
            }
            if (layout != null)
            {
                ViewBag.layout = "HR";
            }
            ViewBag.UserId = userid;
            return View(list.FirstOrDefault());
        }
        public async Task<IActionResult>  UpdateHierarchy(string hierarchyId, string users)
        {
            var portal = await _userBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            var hmodel = new UserHierarchyViewModel();
            var model = await _userHierarchyBusiness.GetSingle(x=>x.HierarchyMasterId==hierarchyId && x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);
            var EmployeeName = "";
            var userS = users.Trim(',');
            var userSplits = userS.Split(',').Distinct();
            foreach (var user in userSplits)
            {
                var usersname = await _userBusiness.GetSingleById(user);
                EmployeeName += usersname.Name + ",";
            }
           var hierarchyModel =await _hierarchyMasterBusiness.GetList(x => x.Id == hierarchyId && x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);

            if (users.IsNotNullAndNotEmpty())
            {
                var userStr = users.Trim(',');
                var userSplit = userStr.Split(',').Distinct();
                string userId = "0";
                if (userSplit.Count() == 1)
                {
                    foreach (var user in userSplit)
                    {
                        userId = user;
                        break;
                    }
                    var data =await _userHierarchyBusiness.GetHierarchyList(hierarchyId,userId);
                    
                    if (model.IsNotNull())
                    {
                        model.ParentUserId = data.FirstOrDefault().ParentUserId;
                        model.Level1ApproverOption1UserId = data.FirstOrDefault().Level1ApproverOption1UserId;
                        model.Level2ApproverOption1UserId = data.FirstOrDefault().Level2ApproverOption1UserId;
                        model.Level3ApproverOption1UserId = data.FirstOrDefault().Level3ApproverOption1UserId;
                        model.Level4ApproverOption1UserId = data.FirstOrDefault().Level4ApproverOption1UserId;
                        model.Level5ApproverOption1UserId = data.FirstOrDefault().Level5ApproverOption1UserId;
                                                                
                        model.Level1ApproverOption2UserId = data.FirstOrDefault().Level1ApproverOption2UserId;
                        model.Level2ApproverOption2UserId = data.FirstOrDefault().Level2ApproverOption2UserId;
                        model.Level3ApproverOption2UserId = data.FirstOrDefault().Level3ApproverOption2UserId;
                        model.Level4ApproverOption2UserId = data.FirstOrDefault().Level4ApproverOption2UserId;
                        model.Level5ApproverOption2UserId = data.FirstOrDefault().Level5ApproverOption2UserId;
                                                                
                        model.Level1ApproverOption3UserId = data.FirstOrDefault().Level1ApproverOption3UserId;
                        model.Level2ApproverOption3UserId = data.FirstOrDefault().Level2ApproverOption3UserId;
                        model.Level3ApproverOption3UserId = data.FirstOrDefault().Level3ApproverOption3UserId;
                        model.Level4ApproverOption3UserId = data.FirstOrDefault().Level4ApproverOption3UserId;
                        model.Level5ApproverOption3UserId = data.FirstOrDefault().Level5ApproverOption3UserId;

                        model.EmployeeName = EmployeeName;
                        model.AdminUserId = data.FirstOrDefault().AdminUserId;
                        model.UserIds = users;
                        model.HierarchyId = hierarchyId;
                        model.HierarchyName = hierarchyModel.FirstOrDefault().Name;
                        model.Id = "0";
                        model.DataAction = DataActionEnum.Create;
                        model.Level1Name = hierarchyModel.FirstOrDefault().Level1Name;
                        model.Level2Name = hierarchyModel.FirstOrDefault().Level2Name;
                        model.Level3Name = hierarchyModel.FirstOrDefault().Level3Name;
                        model.Level4Name = hierarchyModel.FirstOrDefault().Level4Name;
                        model.Level5Name = hierarchyModel.FirstOrDefault().Level5Name;
                        return View(model);
                    }
                  
                }

            }
           
            hmodel.UserIds = users;
            hmodel.EmployeeName = EmployeeName;
            hmodel.HierarchyId = hierarchyId;
            hmodel.HierarchyName = hierarchyModel.FirstOrDefault().Name;
            hmodel.Id = "0";
            hmodel.DataAction = DataActionEnum.Create;
            hmodel.Level1Name = hierarchyModel.FirstOrDefault().Level1Name;
            hmodel.Level2Name = hierarchyModel.FirstOrDefault().Level2Name;
            hmodel.Level3Name = hierarchyModel.FirstOrDefault().Level3Name;
            hmodel.Level4Name = hierarchyModel.FirstOrDefault().Level4Name;
            hmodel.Level5Name = hierarchyModel.FirstOrDefault().Level5Name;
            
            hmodel.LegalEntityId = _userContext.LegalEntityId;
            hmodel.PortalId = _userContext.PortalId;
            return View(hmodel);
         
        }

        [HttpGet]
        public async Task<ActionResult> GetIdNameListByType(HierarchyTypeEnum hierarchyTypeCode)
        {
            var model = await _hierarchyMasterBusiness.GetList(x => x.HierarchyType == hierarchyTypeCode /*&& x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId*/);
            var list= model.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            return Json(list);
        }
        //public async Task<ActionResult> ReadSearchData([DataSourceRequest] DataSourceRequest request, string hierarchyId, string userId)
        //{
        //    var result =await _userHierarchyBusiness.GetHierarchyListForPortal(hierarchyId,userId);     
        //    var json = Json(result.ToDataSourceResult(request));           
        //    return json;
        //}
        public async Task<ActionResult> ReadSearchData(string hierarchyId, string userId)
        {
            var result = await _userHierarchyBusiness.GetHierarchyListForPortal(hierarchyId, userId);
            var json = Json(result);
            return json;
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserHierarchy(UserHierarchyViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _userHierarchyBusiness.CreateUserHierarchyForPortal(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });

                    }
                    else
                    {
                        return Json(new { success = false, error = result.HtmlError });
                    }
                }
               
            }

            return Json(new { success = false, error = ModelState.SerializeErrors() });
        }
    }
}
