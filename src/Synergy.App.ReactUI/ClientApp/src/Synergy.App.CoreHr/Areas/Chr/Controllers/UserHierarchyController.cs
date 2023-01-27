using Synergy.App.Business;
using Synergy.App.Common;
using CMS.UI.Utility;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
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
        private readonly IUserContext _userContext;
        public UserHierarchyController(IUserBusiness userBusiness, IUserContext userContext)
        {
            _userBusiness = userBusiness;
            _userContext = userContext;
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
        public async Task<ActionResult> UserHierarchy(string hierarchyCode,int level=1,int option=1)
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
    }
}