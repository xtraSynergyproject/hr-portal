using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
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
        private IServiceProvider _sp;
        public UserHierarchyController(IHierarchyMasterBusiness hierarchyMasterBusiness, IUserBusiness userBusiness,
            IUserHierarchyBusiness userHierarchyBusiness, IServiceProvider sp)
        {
            _hierarchyMasterBusiness = hierarchyMasterBusiness;
            _userBusiness = userBusiness;
            _userHierarchyBusiness = userHierarchyBusiness;
            _sp = sp;
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
            ViewBag.UserId = userid;
            return View(list.FirstOrDefault());
        }
        public async Task<IActionResult>  UpdateHierarchy(string hierarchyId, string users)
        {
            //var model = _hierarchyBusiness.ViewModelList<UserHierarchyViewModel>("h.Id={HierarchyId}"
            //    , new Dictionary<string, object> { { "HierarchyId", hierarchyId } }).FirstOrDefault();
            var hmodel = new UserHierarchyViewModel();
            var model = await _userHierarchyBusiness.GetSingle(x=>x.HierarchyMasterId==hierarchyId);
            var EmployeeName = "";
            var userS = users.Trim(',');
            var userSplits = userS.Split(',').Distinct();
            foreach (var user in userSplits)
            {
                var usersname = await _userBusiness.GetSingleById(user);
                EmployeeName += usersname.Name + ",";
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
    }
}
