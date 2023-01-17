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
    public class OrganizationHierarchyController : ApplicationController
    {

        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IUserContext _userContext;
        private readonly IUserBusiness _userBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        public OrganizationHierarchyController(IHRCoreBusiness hrCoreBusiness,
             IUserBusiness userBusiness,IUserContext userContext, ITableMetadataBusiness tableMetadataBusiness)
        {
            _hrCoreBusiness = hrCoreBusiness;
            _userContext = userContext;
            _userBusiness = userBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
        }

        public async Task<ActionResult> Index(string personId,string orgId,string rs)
        {
            var date = DateTime.Now.Date;
            var LoggedInUserPositionId = _userContext.OrganizationId;

            var rootNodes = await _userBusiness.GetHierarchyRootId(_userContext.UserId, "ORG_HIERARCHY", LoggedInUserPositionId);
            var hierarchyId = rootNodes.Item3;
            var HierarchyRootNodeId = rootNodes.Item1;
            var AllowedRootNodeId = rootNodes.Item2;
            var lvl = await _hrCoreBusiness.GetDepartmentNodeLevel(rootNodes.Item2);
            var viewModel = new OrganizationChartIndexViewModel
            {
                HierarchyId = hierarchyId,
                HierarchyRootNodeId = rootNodes.Item1,
                AllowedRootNodeId = rootNodes.Item2,

                CanAddRootNode = HierarchyRootNodeId == AllowedRootNodeId && HierarchyRootNodeId == "",
                AllowedRootNodeLevel = lvl.Count,
                AsOnDate = date.ToYYYY_MM_DD_DateFormat(),
                RequestSource = rs,
                // ChartMode = chartMode,

            };
            ViewBag.PositionId = orgId;
            //ViewBag.PermissonCSV = Session[Constant.SessionVariable.PermissionCSV];
            // ViewBag.IsAdmin = LoggedInUserIsAdmin;
            ViewBag.IsAsOnDate = date.ToYYYY_MM_DD_DateFormat();
            ViewBag.LoggedInEmpId = personId;
            ViewBag.LoggedInPositionId = LoggedInUserPositionId;
            // ViewBag.LoggedInUserOrganizationMapping = Session[Constant.SessionVariable.UserOrganizationMapping];
            //  ViewBag.LoggedInOrgId = LoggedInUserOrganizationId;
            ViewBag.AsOnDateDisplay = date.ToYYYY_MM_DD_DateFormat();

            return View(viewModel);

            //var model = await _hrCoreBusiness.GetOrgHierarchyParentId(personId);
            //if (!model.IsNotNull())
            //{
            //    model = new OrganizationChartIndexViewModel();
            //}
            //return View(model);
        }
        public async Task<ActionResult> GetChildList(string parentId,int levelUpto)
        {
            var childList = await _hrCoreBusiness.GetOrgHierarchy(parentId,levelUpto);
            var json = Json(childList);
            return json;

        }

        public async Task<ActionResult> AddExistingDepartment(string parentDepartmentId,string parentDepartmentName, string hierarchy)
        {
            var model = new OrganizationChartViewModel();
            model.ParentId = parentDepartmentId;
            model.OrganizationName = parentDepartmentName;
            model.HierarchyId = hierarchy;
            return View(model);

        }

        public async Task<JsonResult> GetNonExistingDepartmentlist(string hierarchy,string parentId=null)
        {
            var list = await _hrCoreBusiness.GetNonExistingDepartment(hierarchy, null);
            if (parentId.IsNotNullAndNotEmpty())
            {
                list = list.Where(x => x.Id != parentId).ToList();
            }
            return Json(list);
        }
        [HttpPost]
        public async Task<IActionResult> AddExistingDepartmentPost(string parentDepartmentId, string departmentId)
        {
            var model = new NoteTemplateViewModel();
            model.UdfNoteTableId = departmentId;
            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("ParentDepartmentId", parentDepartmentId);
            ((IDictionary<String, Object>)exo).Add("CreatedBy", _userContext.UserId);
            model.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            model.DataAction = DataActionEnum.Create;
            var updatecol = new Dictionary<string, object>();
            updatecol.Add("ParentDepartmentId", parentDepartmentId);
            var departmentnoteid = await _tableMetadataBusiness.GetTableDataByColumn("HRDepartment", null, "Id", departmentId);
            if (departmentnoteid != null)
            {
                var noteid = Convert.ToString(departmentnoteid["NtsNoteId"]);
                await _tableMetadataBusiness.EditTableDataByHeaderId("HRDepartment", null, noteid, updatecol);
            }

            await _hrCoreBusiness.CreateDepartmentHierarchy(model);
            
            return Json(new { success = true });

        }
      
        public async Task<ActionResult> GetSwitchOrgValueMapper(string value, string hierarchyId, string parentId, string filters)
        {
            long dataItemIndex = -1;

            if (value != null)
            {
                var list = await _hrCoreBusiness.GetNonExistingDepartmentList(hierarchyId, parentId, filters, 0, 0, value);

                dataItemIndex = list.ItemIndex;
            }
            return Json(dataItemIndex);
        }
        public async Task<ActionResult> GetOrganizationVirtualData(int page, int pageSize, string filters, string hierarchyId, string parentId = null)
        {
            var list = await _hrCoreBusiness.GetNonExistingDepartmentList(hierarchyId, parentId, filters, pageSize, page);
          
            return Json(new { Data = list.Data, Total = list.Total });
        }
    }
}