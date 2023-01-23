using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class UserHierarchyController : ApplicationController
    {
        private IHierarchyMasterBusiness _hierarchyMasterBusiness;
        private IUserHierarchyBusiness _userHierarchyBusiness;
        private IUserBusiness _userBusiness;
        private IUserContext _userContext;
        private IServiceProvider _sp;
        public UserHierarchyController(IHierarchyMasterBusiness hierarchyMasterBusiness, IUserBusiness userBusiness,
            IUserHierarchyBusiness userHierarchyBusiness, IServiceProvider sp, IUserContext userContext)
        {
            _hierarchyMasterBusiness = hierarchyMasterBusiness;
            _userBusiness = userBusiness;
            _userHierarchyBusiness = userHierarchyBusiness;
            _sp = sp;
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
                list = await _hierarchyMasterBusiness.GetList();
                
            }
            if (layout != null)
            {
                ViewBag.layout = "HR";
            }
            var userrole = _userContext.UserRoleCodes.IsNullOrEmpty() ? new string[] { } : _userContext.UserRoleCodes.Split(",");
           
            ViewBag.UserId = userid;
            var data = list.FirstOrDefault();
            data.UserRoleCodes = userrole;
            return View(data);
        }


        public async Task<IActionResult> UserHierarchyGeneral(string hierarchyCode,string hierarchyId, string userid, string layout)
        {
            var list1 = new List<HierarchyMasterViewModel>();
            var list = new HierarchyMasterViewModel();
            if (hierarchyId.IsNotNullAndNotEmpty())
            {
                list = await _hierarchyMasterBusiness.GetSingleById(hierarchyId);
            }
            else if (hierarchyCode.IsNotNullAndNotEmpty())
            {
                list = await _hierarchyMasterBusiness.GetSingle(x => x.Code == hierarchyCode);
            }
            else
            {
                list1 = await _hierarchyMasterBusiness.GetList();
                list = list1.FirstOrDefault();
            }
            if (layout != null)
            {
                ViewBag.layout = "HR";
            }
            var userrole = _userContext.UserRoleCodes.IsNullOrEmpty() ? new string[] { } : _userContext.UserRoleCodes.Split(",");
            
            ViewBag.UserId = userid;
            var data = list;
            data.UserRoleCodes = userrole;
            return View(data);
        }
        public async Task<IActionResult>  UpdateHierarchy(string hierarchyId, string users,string level1ApproverOption1UserId,bool islevel1OptionReadonly)
        {
            //var model = _hierarchyBusiness.ViewModelList<UserHierarchyViewModel>("h.Id={HierarchyId}"
            //    , new Dictionary<string, object> { { "HierarchyId", hierarchyId } }).FirstOrDefault();
            var hmodel = new UserHierarchyViewModel();
            var model = await _userHierarchyBusiness.GetSingle(x=>x.HierarchyMasterId==hierarchyId);
            var EmployeeName = "";
            if (users.IsNotNullAndNotEmpty()) 
            {
                var userS = users.Trim(',');
                var userSplits = userS.Split(',').Distinct();
                foreach (var user in userSplits)
                {
                    var usersname = await _userBusiness.GetSingleById(user);
                    EmployeeName += usersname.Name + ",";
                }
            }
            
           var hierarchyModel =await _hierarchyMasterBusiness.GetList(x => x.Id == hierarchyId);

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
            if (level1ApproverOption1UserId.IsNotNullAndNotEmpty())
            {
                hmodel.Level1ApproverOption1UserId = level1ApproverOption1UserId;
            }
            ViewBag.Islevel1OptionReadonly = islevel1OptionReadonly;
            return View(hmodel);
         
        }

        [HttpGet]
        public async Task<ActionResult> GetIdNameListByType(HierarchyTypeEnum hierarchyTypeCode)
        {
            var model = await _hierarchyMasterBusiness.GetList(x => x.HierarchyType == hierarchyTypeCode);
            var list= model.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            return Json(list);
        }
        public async Task<ActionResult> ReadSearchData(string hierarchyId, string userId)
        {
            var result =await _userHierarchyBusiness.GetHierarchyList(hierarchyId,userId);     
            var json = Json(result/*.ToDataSourceResult(request)*/);           
            return json;
        }
        public async Task<ActionResult> ReadUserHierarchyData(string hierarchyId)
        {
            var result = await _userHierarchyBusiness.GetHierarchyListForAllPortals(hierarchyId);
            var json = Json(result/*.ToDataSourceResult(request)*/);
            return json;
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserHierarchy(UserHierarchyViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _userHierarchyBusiness.CreateUserHierarchy(model);
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


        public async Task<IActionResult> UpdateUserHierarchy(string hierarchyId, string userId, string levelNo, string optionNo)
        {
            var hmodel = new UserHierarchyViewModel();
            var model = await _userHierarchyBusiness.GetSingle(x => x.HierarchyMasterId == hierarchyId);
            var hierarchyModel = await _hierarchyMasterBusiness.GetList(x => x.Id == hierarchyId);
            if (userId.IsNotNullAndNotEmpty())
            {
                //var data = await _userHierarchyBusiness.GetHierarchyList(hierarchyId, userId);
                //if (model.IsNotNull())
                //{
                //    model.ParentUserId = data.FirstOrDefault().ParentUserId;
                //    model.Level1ApproverOption1UserId = data.FirstOrDefault().Level1ApproverOption1UserId;
                //    model.Level2ApproverOption1UserId = data.FirstOrDefault().Level2ApproverOption1UserId;
                //    model.Level3ApproverOption1UserId = data.FirstOrDefault().Level3ApproverOption1UserId;
                //    model.Level4ApproverOption1UserId = data.FirstOrDefault().Level4ApproverOption1UserId;
                //    model.Level5ApproverOption1UserId = data.FirstOrDefault().Level5ApproverOption1UserId;

                //    model.Level1ApproverOption2UserId = data.FirstOrDefault().Level1ApproverOption2UserId;
                //    model.Level2ApproverOption2UserId = data.FirstOrDefault().Level2ApproverOption2UserId;
                //    model.Level3ApproverOption2UserId = data.FirstOrDefault().Level3ApproverOption2UserId;
                //    model.Level4ApproverOption2UserId = data.FirstOrDefault().Level4ApproverOption2UserId;
                //    model.Level5ApproverOption2UserId = data.FirstOrDefault().Level5ApproverOption2UserId;

                //    model.Level1ApproverOption3UserId = data.FirstOrDefault().Level1ApproverOption3UserId;
                //    model.Level2ApproverOption3UserId = data.FirstOrDefault().Level2ApproverOption3UserId;
                //    model.Level3ApproverOption3UserId = data.FirstOrDefault().Level3ApproverOption3UserId;
                //    model.Level4ApproverOption3UserId = data.FirstOrDefault().Level4ApproverOption3UserId;
                //    model.Level5ApproverOption3UserId = data.FirstOrDefault().Level5ApproverOption3UserId;
                //    //model.EmployeeName = EmployeeName;
                //    model.AdminUserId = data.FirstOrDefault().AdminUserId;
                //    model.UserIds = userId;
                //    model.HierarchyId = hierarchyId;
                //    model.HierarchyName = hierarchyModel.FirstOrDefault().Name;
                //    model.Id = "0";
                //    model.DataAction = DataActionEnum.Create;
                //    model.Level1Name = hierarchyModel.FirstOrDefault().Level1Name;
                //    model.Level2Name = hierarchyModel.FirstOrDefault().Level2Name;
                //    model.Level3Name = hierarchyModel.FirstOrDefault().Level3Name;
                //    model.Level4Name = hierarchyModel.FirstOrDefault().Level4Name;
                //    model.Level5Name = hierarchyModel.FirstOrDefault().Level5Name;
                //    return View(model);
                //}
                if (levelNo=="1" && optionNo=="1") 
                {
                    hmodel.Level1ApproverOption1UserId = userId;
                }
                else if (levelNo == "1" && optionNo == "2")
                {
                    hmodel.Level1ApproverOption2UserId = userId;
                }
                else if (levelNo == "1" && optionNo == "3")
                {
                    hmodel.Level1ApproverOption3UserId = userId;
                }
                else if (levelNo == "2" && optionNo == "1")
                {
                    hmodel.Level2ApproverOption1UserId = userId;
                }
                else if (levelNo == "2" && optionNo == "2")
                {
                    hmodel.Level2ApproverOption2UserId = userId;
                }
                else if (levelNo == "2" && optionNo == "3")
                {
                    hmodel.Level2ApproverOption3UserId = userId;
                }
                else if (levelNo == "3" && optionNo == "1")
                {
                    hmodel.Level3ApproverOption1UserId = userId;
                }
                else if (levelNo == "3" && optionNo == "2")
                {
                    hmodel.Level3ApproverOption2UserId = userId;
                }
                else if (levelNo == "3" && optionNo == "3")
                {
                    hmodel.Level3ApproverOption3UserId = userId;
                }
                else if (levelNo == "4" && optionNo == "1")
                {
                    hmodel.Level4ApproverOption1UserId = userId;
                }
                else if (levelNo == "4" && optionNo == "2")
                {
                    hmodel.Level4ApproverOption2UserId = userId;
                }
                else if (levelNo == "4" && optionNo == "3")
                {
                    hmodel.Level4ApproverOption3UserId = userId;
                }
                else if (levelNo == "5" && optionNo == "1")
                {
                    hmodel.Level5ApproverOption1UserId = userId;
                }
                else if (levelNo == "5" && optionNo == "2")
                {
                    hmodel.Level5ApproverOption2UserId = userId;
                }
                else if (levelNo == "5" && optionNo == "3")
                {
                    hmodel.Level5ApproverOption3UserId = userId;
                }
               
              hmodel.ParentUserId = userId;
            //hmodel.EmployeeName = EmployeeName;
            hmodel.HierarchyId = hierarchyId;
            hmodel.HierarchyName = hierarchyModel.FirstOrDefault().Name;
            hmodel.Id = "0";
            hmodel.DataAction = DataActionEnum.Create;
            hmodel.Level1Name = hierarchyModel.FirstOrDefault().Level1Name;
            hmodel.Level2Name = hierarchyModel.FirstOrDefault().Level2Name;
            hmodel.Level3Name = hierarchyModel.FirstOrDefault().Level3Name;
            hmodel.Level4Name = hierarchyModel.FirstOrDefault().Level4Name;
            hmodel.Level5Name = hierarchyModel.FirstOrDefault().Level5Name;
                hmodel.LevelNo =Convert.ToInt32(levelNo);
                hmodel.OptionNo = Convert.ToInt32(optionNo) ;
            }
            return View(hmodel);
        }
        public async Task<JsonResult> GetNonExistingUserlist(string hierarchy, string parentId = null)
        {
            
            var list = await _userHierarchyBusiness.GetNonExistingUser(hierarchy, null);
            if (parentId.IsNotNullAndNotEmpty())
            {
                list = list.Where(x => x.Id != parentId).ToList();
            }
            return Json(list);
        }
        public async Task<ActionResult> UserHierarchyChart(string hierarchyCode, string permissions,bool enableMultipleParent, int level = 1, int option = 1)
        {
            var LoggedInUserId = _userContext.UserId;
            var rootNodes = await _userBusiness.GetHierarchyRootId(_userContext.UserId, hierarchyCode, LoggedInUserId);
            var hierarchyId = rootNodes.Item3;
            var HierarchyRootNodeId = rootNodes.Item1;
            var AllowedRootNodeId = rootNodes.Item2;
            var lvl = await _userBusiness.GetUserNodeLevel(rootNodes.Item2);
            var viewModel = new PositionChartIndexViewModel
            {
                HierarchyId = hierarchyId,
                HierarchyRootNodeId = HierarchyRootNodeId,
                AllowedRootNodeId = AllowedRootNodeId,

                CanAddRootNode = HierarchyRootNodeId == AllowedRootNodeId && HierarchyRootNodeId == "",
                AllowedRootNodeLevel = lvl.Count,

                // RequestSource = rs,
                // ChartMode = chartMode,

            };
            ViewBag.Permissons = permissions;
            ViewBag.LevelNo = level;
            ViewBag.OptionNo = option;
            ViewBag.EnableMultipleParent = enableMultipleParent;
            return View(viewModel);
        }
        public async Task<ActionResult> GetChildList(string parentId, int levelUpto, string hierarchyId)
        {
            var childList = await _userBusiness.GetUserHierarchyChartData(parentId, levelUpto, hierarchyId);
            var json = Json(childList);
            return json;

        }
    }
}
