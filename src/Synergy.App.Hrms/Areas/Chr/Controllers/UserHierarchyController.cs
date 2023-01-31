using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CHR.Controllers
{
    [Area("CHR")]
    public class UserHierarchyController : ApplicationController
    {

        private readonly IUserBusiness _userBusiness;
        private readonly IUserHierarchyBusiness _userHierarchyBusiness;
        private readonly IUserContext _userContext;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        public UserHierarchyController(IUserBusiness userBusiness, IUserContext userContext, IUserHierarchyBusiness userHierarchyBusiness, IHRCoreBusiness hrCoreBusiness)
        {
            _userBusiness = userBusiness;
            _userContext = userContext;
            _userHierarchyBusiness = userHierarchyBusiness;
            _hrCoreBusiness = hrCoreBusiness;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateLevel(string hierarchyId,string users)
        {
            var model = new UserHierarchyViewModel();
            model.EmployeeName = users;
            model.HierarchyName = hierarchyId;
            return View("_Updatelevel",model);
        }


        public async Task<ActionResult> ReadSearchData()
        {
            var list = await _userBusiness.GetList();
            var j = Json(list);
            return j;
        }
        public async Task<ActionResult> UserHierarchy(string hierarchyCode, string permissions,int level=1,int option=1)
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
            return View(viewModel);
        }
        public async Task<ActionResult> GetChildList(string parentId, int levelUpto,string hierarchyId)
        {
            var childList = await _userBusiness.GetUserHierarchy(parentId, levelUpto,hierarchyId);
            var json = Json(childList);
            return json;

        }
        public async Task<ActionResult> AddExistingUser(string parentUserId, string parentUserName, string hierarchy)
        {
            var model = new UserHierarchyChartViewModel();
            model.ParentId = parentUserId;
            model.UserName = parentUserName;
            model.HierarchyId = hierarchy;
            return View(model);

        }
        public async Task<ActionResult> ChangeManager(string userId, string parentUserId,string userName, string hierarchy)
        {
            var model = new UserHierarchyChartViewModel();
            model.ParentId = parentUserId;
            ViewBag.UserId = userId;
            model.UserName = userName;
            var parenrUser = await _userBusiness.GetSingleById(parentUserId);
            ViewBag.ParentUserName = parenrUser.Name;
            model.HierarchyId = hierarchy;
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ChangeManagerPost(string parentUserId, string userId, string hierarchyId)
        {
            var userHierarchy = await _userHierarchyBusiness.GetSingle(x => x.HierarchyMasterId == hierarchyId && x.UserId == userId);
            if (userHierarchy!=null) 
            {
                userHierarchy.ParentUserId = parentUserId;
            } 
            var result = await _userHierarchyBusiness.Edit(userHierarchy);
            if (result.IsSuccess)
            {
                return Json(new { success = true });

            }
            else
            {
                return Json(new { success = false, error = result.HtmlError });
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoveHierarchy(string userId, string hierarchyId)
        {
            var userHierarchy = await _userHierarchyBusiness.GetSingle(x => x.HierarchyMasterId == hierarchyId && x.UserId == userId);
            if (userHierarchy != null)
            {
                userHierarchy.IsDeleted = true;
            }
            var result = await _userHierarchyBusiness.Edit(userHierarchy);
            if (result.IsSuccess)
            {
                return Json(new { success = true });

            }
            else
            {
                return Json(new { success = false, error = result.HtmlError });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddExistingUserPost(string parentUserId, string userId, string hierarchyId)
        {
            var model = new UserHierarchyViewModel();
            model.UserIds = userId;
            model.UserId = userId;
            model.Level1ApproverOption1UserId = parentUserId;
            model.HierarchyId = hierarchyId;
            model.LevelNo = 1;
            model.OptionNo = 1;
            // dynamic exo = new System.Dynamic.ExpandoObject();

            //((IDictionary<String, Object>)exo).Add("ParentUserId", parentUserId);
            // ((IDictionary<String, Object>)exo).Add("CreatedBy", _userContext.UserId);
            // model.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            // model.DataAction = DataActionEnum.Create;
            var result= await _userHierarchyBusiness.CreateUserHierarchy(model);
            if (result.IsSuccess)
            {
                return Json(new { success = true });

            }
            else
            {
                return Json(new { success = false, error = result.HtmlError });
            }
        }
        public async Task<JsonResult> GetNonExistingUserlist(string hierarchy, string parentId = null)
        {
            var list = await _hrCoreBusiness.GetNonExistingUserInHierarchy(hierarchy, null);
            if (parentId.IsNotNullAndNotEmpty())
            {
                list = list.Where(x => x.Id != parentId).ToList();
            }
            return Json(list);
        }
        public async Task<JsonResult> GetExistingUserInHierarchyExceptChild(string hierarchy, string existingParentId, string userId = null)
        {
            var list = await _hrCoreBusiness.GetExistingUserInHierarchyExceptChild(hierarchy, userId);
            if (existingParentId.IsNotNullAndNotEmpty())
            {
                list = list.Where(x => x.Id != existingParentId && x.Id!=userId).ToList();
            }
            return Json(list);
        }
    }
}