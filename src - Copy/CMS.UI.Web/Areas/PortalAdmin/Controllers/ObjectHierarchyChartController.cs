using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CHR.Controllers
{
    [Area("portalAdmin")]
    public class ObjectHierarchyChartController : ApplicationController
    {

        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IUserContext _userContext;
        private readonly IUserBusiness _userBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        public ObjectHierarchyChartController(IHRCoreBusiness hrCoreBusiness,
             IUserBusiness userBusiness,IUserContext userContext, ITableMetadataBusiness tableMetadataBusiness)
        {
            _hrCoreBusiness = hrCoreBusiness;
            _userContext = userContext;
            _userBusiness = userBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
        }

        public async Task<ActionResult> Index(string hierarchyCode, string permissions)
        {
            var date = DateTime.Now.Date;
            //var LoggedInUserPositionId = _userContext.OrganizationId;

            var rootNodes = await _userBusiness.GetUserHierarchyRootId(_userContext.UserId, hierarchyCode, _userContext.UserId);
            var hierarchyId = rootNodes.Item3;
            var HierarchyRootNodeId = rootNodes.Item1;
            var AllowedRootNodeId = rootNodes.Item2;
            //var lvl = await _hrCoreBusiness.GetUserNodeLevel(rootNodes.Item2, rootNodes.Item3);
            var viewModel = new UserChartIndexViewModel
            {
                HierarchyId = hierarchyId,
                HierarchyRootNodeId = "-1",
                AllowedRootNodeId = "-1",

                CanAddRootNode = HierarchyRootNodeId == AllowedRootNodeId && HierarchyRootNodeId == "",
                AllowedRootNodeLevel = 0,
                AsOnDate = date.ToYYYY_MM_DD_DateFormat(),
               // RequestSource = rs,  
               Permission = permissions

            };
           // ViewBag.PositionId = orgId;           
           
            ViewBag.IsAsOnDate = date.ToYYYY_MM_DD_DateFormat();
           // ViewBag.LoggedInEmpId = personId;
            //ViewBag.LoggedInPositionId = LoggedInUserPositionId;
        
            ViewBag.AsOnDateDisplay = date.ToYYYY_MM_DD_DateFormat();

            return View(viewModel);         
        }

        public async Task<ActionResult> GetChildList(string parentId,int levelUpto,string hierarchyId, string permissions)
        {
            var childList = await _hrCoreBusiness.GetObjectHierarchy(parentId, levelUpto, hierarchyId, permissions);              
                        
            var json = Json(childList);
            return json;
        }

        public IActionResult AddExistingObject(string parentUserId,string parentUserName, string hierarchy)
        {
            var model = new UserHierarchyChartViewModel();
            model.ParentId = parentUserId;
            model.UserName = parentUserName;
            model.HierarchyId = hierarchy;
            return View(model);
        }

        public async Task<JsonResult> GetNonExistingUserlist(string hierarchy,string parentId=null)
        {
            var list = await _hrCoreBusiness.GetNonExistingUser(hierarchy, null);
            if (parentId.IsNotNullAndNotEmpty())
            {
                list = list.Where(x => x.Id != parentId).ToList();
            }
            return Json(list);
        }

        [HttpPost]
        public async Task<IActionResult> AddExistingUserPost(string parentUserId, string userId,string hierarchyId)
        {
            var model = new NoteTemplateViewModel();
            model.UdfNoteTableId = userId;
            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("ParentUserId", parentUserId);
            ((IDictionary<String, Object>)exo).Add("CreatedBy", _userContext.UserId);
            model.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            model.DataAction = DataActionEnum.Create;           
            await _hrCoreBusiness.CreateUserHierarchy(model,hierarchyId);            
            return Json(new { success = true });
        }

        public async Task<IActionResult> DeleteHierarchy(string noteId)
        {
            var result = await _hrCoreBusiness.DeleteUserHierarchy(noteId);
            return Json(new { success = result });
        }
    }
}