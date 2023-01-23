﻿using CMS.Business;
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
    [Area("CHR")]
    public class PositionHierarchyController : ApplicationController
    {

        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IUserContext _userContext;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        public PositionHierarchyController(IHRCoreBusiness hrCoreBusiness, IUserContext userContext,
            IUserBusiness userBusiness,
            ITableMetadataBusiness tableMetadataBusiness)
        {
            _hrCoreBusiness = hrCoreBusiness;
            _userContext = userContext;
            _userBusiness = userBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
        }
       // long? posId = null, DateTime? date = null, ChartViewType? type = null, long hierarchyId = 1, Mode chartMode = Mode.Normal, string rs = null
        public async Task<ActionResult> Index(string personId,string posId,string rs)
        {
            var date =DateTime.Now.Date;
            var LoggedInUserPositionId = _userContext.PositionId;
            
            var rootNodes =await _userBusiness.GetHierarchyRootId(_userContext.UserId, "POS_HIERARCHY", LoggedInUserPositionId);
            var orgRootNode =await _userBusiness.GetHierarchyRootId(_userContext.UserId, "ORG_HIERARCHY", LoggedInUserPositionId);
            var hierarchyId = rootNodes.Item3;
            var HierarchyRootNodeId = rootNodes.Item1;
            var AllowedRootNodeId = rootNodes.Item2;
            var lvl = await _hrCoreBusiness.GetPositionNodeLevel(rootNodes.Item2);
            var viewModel = new PositionChartIndexViewModel
            {
                HierarchyId = hierarchyId,
                HierarchyRootNodeId = rootNodes.Item1,
                AllowedRootNodeId = rootNodes.Item2,

                CanAddRootNode = HierarchyRootNodeId == AllowedRootNodeId && HierarchyRootNodeId == "",
                AllowedRootNodeLevel = lvl.Count,
                AsOnDate = date.ToYYYY_MM_DD_DateFormat(),
                RequestSource = rs,
               // RequestSource = rs,
               // ChartMode = chartMode,

            };
            ViewBag.PositionId = posId;
            //ViewBag.PermissonCSV = Session[Constant.SessionVariable.PermissionCSV];
            // ViewBag.IsAdmin = LoggedInUserIsAdmin;
            ViewBag.IsAsOnDate = date.ToYYYY_MM_DD_DateFormat();
            ViewBag.LoggedInEmpId = personId;
            ViewBag.LoggedInPositionId = LoggedInUserPositionId;
           // ViewBag.LoggedInUserOrganizationMapping = Session[Constant.SessionVariable.UserOrganizationMapping];
          //  ViewBag.LoggedInOrgId = LoggedInUserOrganizationId;
            ViewBag.AsOnDateDisplay = date.ToYYYY_MM_DD_DateFormat();
            //ViewBag.ChartMode = chartMode;
            if (orgRootNode != null)
            {
                ViewBag.OrgHierarchyRootNodeId = orgRootNode.Item1;
                ViewBag.OrgAllowedRootNodeId = orgRootNode.Item2;
            }
            return View(viewModel);

            ////personId = personId.IsNotNullAndNotEmpty() ? personId : _userContext.Per;
            //var model = await _hrCoreBusiness.GetPostionHierarchyParentId(personId);            
            //if (!model.IsNotNull())
            //{
            //    model = new PositionChartIndexViewModel();
            //}

            //return View(model);
        }
        public async Task<ActionResult> GetChildList(string parentId,int levelUpto)
        {
            var childList = await _hrCoreBusiness.GetPositionHierarchy(parentId,levelUpto);            
            var json = Json(childList);
            return json;

        }

        public async Task<ActionResult> GetPositionHierarchyByUserLoggedInUser()
        {
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            var model = await _hrCoreBusiness.GetPostionHierarchyParentId(null);
            if (model!=null) 
            {               
                var data = await _hrCoreBusiness.GetPositionHierarchyUsers(model.Id,100);
                if (data!=null && data.Count()>0)
                {
                    list = data.Select(e => new IdNameViewModel()
                    {
                        Id=e.Id,
                        Name=e.Name
                    }).ToList();
                }
                return Json(list);
            }
            return Json(list);
        }

        public async Task<ActionResult> AddExistingPosition(string parentPostionId, string parentPositionName,string hierarchyId)
        {
            var model = new PositionChartViewModel();
            model.ParentId = parentPostionId;
            model.OrganizationName = parentPositionName;
            model.HierarchyId = hierarchyId;
            return View(model);

        }

        public async Task<JsonResult> GetNonExistingPositionlist(string hierarchyId,string parentId=null)
        {
            var list = await _hrCoreBusiness.GetNonExistingPosition(hierarchyId,null);
            if (parentId.IsNotNullAndNotEmpty())
            {
                list = list.Where(x => x.Id != parentId).ToList();
            }
            return Json(list);
        }
        [HttpPost]
        public async Task<IActionResult> AddExistingPositionPost(string parentPostionId, string positionId)
        {
            var model = new NoteTemplateViewModel();
            model.UdfNoteTableId = positionId;
            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("ParentPositionId", parentPostionId);
            ((IDictionary<String, Object>)exo).Add("CreatedBy", _userContext.UserId);
            model.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            model.DataAction = DataActionEnum.Create;
            var updatecol = new Dictionary<string, object>();
            updatecol.Add("ParentPositionId", parentPostionId);
            var posnoteid = await _tableMetadataBusiness.GetTableDataByColumn("HRPosition", null, "Id", positionId);
            if (posnoteid != null)
            {
                var noteid =Convert.ToString(posnoteid["NtsNoteId"]);
                await _tableMetadataBusiness.EditTableDataByHeaderId("HRPosition", null, noteid, updatecol);
            }
            
            await _hrCoreBusiness.CreatePositionHierarchy(model);

            return Json(new { success = true });


        }
    }
}