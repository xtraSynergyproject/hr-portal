using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Synergy.App.Common;
using Synergy.App.Business;

namespace Synergy.App.Emirates.Areas.Controllers
{
    [Area("CaseManagement")]
    public class HierarchyChartController : ApplicationController
    {
        private readonly IUserContext _userContext;
        private readonly ICaseManagementBusiness _business;

        public HierarchyChartController(IUserContext userContext, ICaseManagementBusiness business)
        {            
            _userContext = userContext;
            _business = business;
        }

        public async Task<ActionResult> HierarchyChart(string hierarchyCode, string permissions, int level = 1, int option = 1)
        {
            var LoggedInUserId = _userContext.UserId;
            //var rootNodes = await _userBusiness.GetHierarchyRootId(_userContext.UserId, hierarchyCode, LoggedInUserId);
            //var hierarchyId = rootNodes.Item3;
            //var HierarchyRootNodeId = rootNodes.Item1;
            //var AllowedRootNodeId = rootNodes.Item2;
            //var lvl = await _userBusiness.GetUserNodeLevel(rootNodes.Item2);
            var viewModel = new PositionChartIndexViewModel
            {
                //HierarchyId = hierarchyId,
                //HierarchyRootNodeId = HierarchyRootNodeId,
                AllowedRootNodeId = "1",

                CanAddRootNode = false,
                AllowedRootNodeLevel = 3,

            };
            ViewBag.Permissons = permissions;
            ViewBag.LevelNo = level;
            ViewBag.OptionNo = option;
            return View(viewModel);
        }
        public async Task<ActionResult> GetChildList(string parentId, int levelUpto)
        {
            var childList = await _business.GetCMDBHierarchyData(parentId, levelUpto);
            //var childList = new List<PositionChartIndexViewModel>();
            //childList.Add(new PositionChartIndexViewModel
            //{
            //    Name="Category1",
            //    Id="1"
            //});
            //childList.Add(new PositionChartIndexViewModel
            //{
            //    Name = "Category2",
            //    Id = "2",
            //    ParentId="1"
            //});
            //childList.Add(new PositionChartIndexViewModel
            //{
            //    Name = "Category3",
            //    Id = "3",
            //    ParentId = "2"
            //});
            var json = Json(childList);
            return json;

        }

    }
}
