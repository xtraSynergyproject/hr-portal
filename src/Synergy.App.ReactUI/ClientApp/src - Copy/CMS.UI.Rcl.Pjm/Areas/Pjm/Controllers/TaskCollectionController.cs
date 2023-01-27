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

namespace CMS.UI.Web.Areas.PJM.Controllers
{
    [Area("PJM")]
    public class TaskCollectionController : ApplicationController
    {
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IUserContext _userContext;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IProjectManagementBusiness _pmtBusiness;

        public TaskCollectionController(ITemplateBusiness templateBusiness, IUserContext userContext,
            ITaskBusiness taskBusiness, IProjectManagementBusiness ProjectManagementBusiness)
        {
            _templateBusiness = templateBusiness;
            _userContext = userContext;            
            _taskBusiness = taskBusiness;
            _pmtBusiness = ProjectManagementBusiness;
        }

        public async Task<ActionResult> Index()
        {
            //var userId = _userContext.UserId;
            var templateList = await _templateBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Task);
            var model = templateList.FirstOrDefault();
            return View(model);
        }

        public async Task<ActionResult> GetTaskTemplateList()
        {
            var templateList = await _templateBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Task);
            var t = Json(templateList);
            return t;
        }

        public async Task<TemplateViewModel> GetTemplateDetailsById(string Id)
        {
            var template = await _templateBusiness.GetSingleById(Id);

            return template;
        }


        public async Task<ActionResult> GetTaskChartByStatus(string templateId)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskStatusByTemplate(templateId, userId);
            return Json(model);
        }

        public async Task<ActionResult> GetTaskChartByOwner(string templateId)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskByUsers(templateId, userId);
            return Json(model);
        }

        public async Task<ActionResult> GetSLAChart(string templateId, DateTime fromDate, DateTime toDate)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetSLADetails(templateId, userId, fromDate, toDate);
            return Json(model);
        }

        public async Task<IActionResult> ReadTaskGridViewData(string templateId, string statusIds = null, string senderIds = null)
        {
            var list = await _pmtBusiness.ReadTaskGridViewData(templateId, _userContext.UserId, statusIds, senderIds);
            var j = Json(list);
            return j;
        }

        public async Task<ActionResult> GetTaskOwnerUserIdNameList(string templateId)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskOwnerUsersList(templateId, userId);
            return Json(model);
        }
        public async Task<ActionResult> GetTaskUserIdNameList(string templateId)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskUsersList(templateId);
            return Json(model);
        }
        //For PM
        public async Task<ActionResult> ProjectManagerTaskList()
        {
            //var userId = _userContext.UserId;
            var templateList = await _templateBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Task);
            var model = templateList.FirstOrDefault();
            return View(model);
        }
        public async Task<ActionResult> GetPMTaskChartByStatus(string templateId)
        {
            var model = await _pmtBusiness.GetPMTaskStatusByTemplate(templateId);
            return Json(model);
        }

        public async Task<ActionResult> GetPMTaskChartByUsers(string templateId)
        {
            var model = await _pmtBusiness.GetPMTaskByUsers(templateId);
            return Json(model);
        }

        public async Task<ActionResult> GetPMSLAChart(string templateId, DateTime fromDate, DateTime toDate)
        {
            var model = await _pmtBusiness.GetPMSLADetails(templateId, fromDate, toDate);
            return Json(model);
        }

        public async Task<IActionResult> ReadPMTaskGridViewData(string templateId, string statusIds = null, string senderIds = null)
        {
            var list = await _pmtBusiness.ReadPMTaskGridViewData(templateId, statusIds, senderIds);
            var j = Json(list);
            return j;
        }


        // For LineManager//

        public async Task<ActionResult> GetChartByStatus(string TemplateID)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskStatusRequestedByMe(TemplateID, userId);
            return Json(model);
        }


        public async Task<ActionResult> GetChartByAssigneduser(string TemplateID)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskStatusAssigneduserid(TemplateID, userId);
            return Json(model);
        }


        public async Task<ActionResult> mdlAssignuser(string TemplateID)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskStatusAssigneduserid(TemplateID, userId);
            return Json(model);
        }

        public async Task<ActionResult> GetDatewiseTask(string TemplateID, DateTime? fromDate = null, DateTime? toDate = null)
        {

            if (fromDate != null && toDate != null)
            {
                var userId = _userContext.UserId;
                var model = await _pmtBusiness.GetDatewiseTask(TemplateID, userId, fromDate, toDate);
                return Json(model);
            }
            else { return Json(""); }
        }


        public async Task<ActionResult> GetGridDetails(string TemplateID, string filterStatus = null, string filterAssignuser = null)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetGridList(TemplateID, userId, filterAssignuser, filterStatus);
            return Json(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetTemplateDet(string ID)
        {

            //  if (fromDate != null && toDate != null)
            // {
            //     var userId = _userContext.UserId;
            //     var model = await _templateBusiness.GetSingle(x=>x.Id==ID);
            //     return Json(model.Description);
            // }
            //else {}
            return Json("");
        }





        #region for Line Manager Group




        public async Task<ActionResult> GroupTemplateLineManager()
        {
            //var userId = _userContext.UserId;
            var templateList = await _pmtBusiness.GetGroupTemplate();
            var model = templateList.FirstOrDefault();
            return View(model);
        }


        //public async Task<ActionResult> GetTaskTemplateListGroup()
        //{
        //    var templateList = await _pmtBusiness.GetGroupTemplate();
        //    var t = Json(templateList);
        //    return t;
        //}



        public async Task<ActionResult> GetChartByStatusGroup(string TemplateID, string StatusLOV = null)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskStatusRequestedByMeGroup(TemplateID, userId, StatusLOV);
            return Json(model);
        }


        public async Task<ActionResult> GetChartByAssigneduserGroup(string TemplateID, string StatusTemplateID = null, string StatusLOV = null)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskStatusAssigneduseridGroup(TemplateID, userId, StatusTemplateID, StatusLOV);
            return Json(model);
        }


        public async Task<ActionResult> mdlAssignuserGroup(string TemplateID)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.MdlassignUserGroup(TemplateID, userId);
            return Json(model);
        }

        public async Task<ActionResult> GetDatewiseTaskGroup(string TemplateID, DateTime? fromDate = null, DateTime? toDate = null)
        {

            if (fromDate != null && toDate != null)
            {
                var userId = _userContext.UserId;
                var model = await _pmtBusiness.GetDatewiseTaskGroup(TemplateID, userId, fromDate, toDate);
                return Json(model);
            }
            else { return Json(""); }
        }


        public async Task<ActionResult> GetGridDetailsGroup(string TemplateID, string filterStatus = null, string filterAssignuser = null)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetGridListGroup(TemplateID, userId, filterAssignuser, filterStatus);
            var j = Json(model);
            return j;
        }

        #endregion

        #region For Self Group
        public async Task<ActionResult> GroupTemplateSelf()
        {
            //var userId = _userContext.UserId;
            var templateList = await _pmtBusiness.GetGroupTemplate();
            var model = templateList.FirstOrDefault();
            return View(model);
        }



        public async Task<ActionResult> GetTaskChartByStatusGroup(string templateId, string StatusLOV = null)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskStatusByTemplateGroup(templateId, userId, StatusLOV);
            return Json(model);
        }

        public async Task<ActionResult> GetTaskChartByOwnerGroup(string templateId, string StatusTemplateID = null, string StatusLOV = null)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskByUsersGroup(templateId, userId, StatusTemplateID, StatusLOV);
            return Json(model);
        }

        public async Task<IActionResult> ReadTaskGridViewDataGroup([DataSourceRequest] DataSourceRequest request, string templateId, string statusIds = null, string senderIds = null)
        {
            var list = await _pmtBusiness.ReadTaskGridViewDataGroup(templateId, _userContext.UserId, statusIds, senderIds);
            var j = Json(list.ToDataSourceResult(request));
            return j;
        }

        public async Task<ActionResult> GetTaskOwnerUserIdNameListGroup(string templateId)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskOwnerUsersListGroup(templateId, userId);
            return Json(model);
        }

        public async Task<ActionResult> GetDatewiseTaskSingleGroup(string TemplateID, DateTime? fromDate = null, DateTime? toDate = null)
        {

            if (fromDate != null && toDate != null)
            {
                var userId = _userContext.UserId;
                var model = await _pmtBusiness.GetDatewiseSingleGroup(TemplateID, userId, fromDate, toDate);
                return Json(model);
            }
            else { return Json(""); }
        }
        #endregion



        #region for Project Manager Group




        public async Task<ActionResult> GroupTemplateProjectManager()
        {
            //var userId = _userContext.UserId;
            var templateList = await _pmtBusiness.GetGroupTemplate();
            var model = templateList.FirstOrDefault();
            return View(model);
        }






        public async Task<ActionResult> GetChartByStatusProjectGroup(string TemplateID, string StatusLOV = null)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskStatusRequestedByMeProjectGroup(TemplateID, StatusLOV);
            return Json(model);
        }


        public async Task<ActionResult> GetChartByAssigneduserProjectGroup(string TemplateID, string StatusTemplateID = null, string StatusLOV = null)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetChartByAssigneduserProjectGroup(TemplateID, StatusTemplateID, StatusLOV);
            return Json(model);
        }


        public async Task<ActionResult> mdlAssignuserProjectGroup(string TemplateID)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskStatusAssigneduseridProjectGroup(TemplateID);
            return Json(model);
        }

        public async Task<ActionResult> GetDatewiseTaskProjectGroup(string TemplateID, DateTime? fromDate = null, DateTime? toDate = null)
        {

            if (fromDate != null && toDate != null)
            {
                var userId = _userContext.UserId;
                var model = await _pmtBusiness.GetDatewiseTaskProjectGroup(TemplateID, fromDate, toDate);
                return Json(model);
            }
            else { return Json(""); }
        }


        public async Task<ActionResult> GetGridDetailsProjectGroup([DataSourceRequest] DataSourceRequest request, string TemplateID, string filterStatus = null, string filterAssignuser = null)
        {
            var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetGridListProjectGroup(TemplateID, filterStatus, filterAssignuser);

            var j = Json(model.ToDataSourceResult(request));
            return j;

        }

        #endregion



    }
}
