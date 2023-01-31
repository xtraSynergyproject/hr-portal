using Synergy.App.ViewModel;
using Synergy.App.Common;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Business.Interface.DMS;
//using Kendo.Mvc.Extensions;
using Synergy.App.Business;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Areas.DMS.Controllers
{
    [Area("DMS")]

    public class DMSReportController : ApplicationController
    {
        private IDMSDocumentBusiness _dmsDocumentBusiness;
        private ITemplateCategoryBusiness _templateCategoryBusiness;
        private ITemplateBusiness _templateBusiness;
        private IUserContext _userContext;
        private IPortalBusiness _portalBusiness;
        private ITaskBusiness _taskBusiness;
        public DMSReportController(IDMSDocumentBusiness dmsDocumentBusiness, IUserContext userContext, ITaskBusiness taskBusiness,
            ITemplateCategoryBusiness templateCategoryBusiness, ITemplateBusiness templateBusiness,IPortalBusiness portalBusiness)
        {
            _dmsDocumentBusiness = dmsDocumentBusiness;
            _userContext = userContext;
            _templateCategoryBusiness = templateCategoryBusiness;
            _templateBusiness = templateBusiness;
            _taskBusiness = taskBusiness;
            _portalBusiness = portalBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DocumentPendingStatusReport()
        {
            return View();
        }

        public async Task<IActionResult> ReadDocumentStageData([DataSourceRequest] DataSourceRequest request, string stageStatus, string discipline, bool isOverdue)
        {
            var result = await _dmsDocumentBusiness.DocumentStageReportData(_userContext.UserId, stageStatus, discipline, isOverdue);
            return Json(result);
            //return Json(result.ToDataSourceResult(request));
        }

        public async Task<IActionResult> ReadDocumentStageDataGrid(string stageStatus, string discipline, bool isOverdue)
        {
            var result = await _dmsDocumentBusiness.DocumentStageReportData(_userContext.UserId, stageStatus, discipline, isOverdue);
            return Json(result);
        }


        public async Task<List<TemplateViewModel>> GetDocumentList(string code)
        {
            var template = await _templateBusiness.GetNoteTemplateList(null, code, null);

            return template;

        }

        public IActionResult DocumentSubmittedStatusReport()
        {
            //var model = new DocumentListViewModel { FromDate = DateTime.Now, ToDate = DateTime.Now.AddDays(1) };
            return View();
        }
        public async Task<ActionResult> ReadDocumentSubmittedData([DataSourceRequest] DataSourceRequest request, string stageStatus, string discipline, string revesion, DateTime? fromDate, DateTime? toDate)
        {

            var list = await _dmsDocumentBusiness.DocumentSubmittedReportData(_userContext.UserId, stageStatus, discipline, revesion, fromDate, toDate);

            var j = Json(list);
            //var j = Json(list.ToDataSourceResult(request));

            return j;
        }

        public async Task<ActionResult> ReadDocumentSubmittedDataGrid(string stageStatus, string discipline, string revision, DateTime? fromDate, DateTime? toDate)
        {

            var list = await _dmsDocumentBusiness.DocumentSubmittedReportData(_userContext.UserId, stageStatus, discipline, revision, fromDate, toDate);

            var j = Json(list);

            return j;
        }

        public async Task<ActionResult> ReadDocumentReceivedData([DataSourceRequest] DataSourceRequest request, string stageStatus, string discipline, string revesion, DateTime? fromDate, DateTime? toDate)
        {
            var list = await _dmsDocumentBusiness.DocumentReceivedReportData(_userContext.UserId, stageStatus, discipline, revesion, fromDate, toDate);

            var j = Json(list);
           // var j = Json(list.ToDataSourceResult(request));
            // j.MaxJsonLength = int.MaxValue;
            return j;
        }

        public async Task<ActionResult> ReadDocumentReceivedDataGrid(string stageStatus, string discipline, string revision, DateTime? fromDate, DateTime? toDate)
        {
            var list = await _dmsDocumentBusiness.DocumentReceivedReportData(_userContext.UserId, stageStatus, discipline, revision, fromDate, toDate);

            var j = Json(list);
            // j.MaxJsonLength = int.MaxValue;
            return j;
        }

        public IActionResult DocumentReceivedStatusReport()
        {
            //var model = new DocumentListViewModel { FromDate = DateTime.Now, ToDate = DateTime.Now.AddDays(1) };
            return View();
        }

        public IActionResult DocumentReceivedCommentsReport()
        {
            //var model = new DocumentListViewModel { FromDate = DateTime.Now, ToDate = DateTime.Now.AddDays(1) };
            return View();
        }

        public async Task<IActionResult> ReadDocumentReceivedCommentsData([DataSourceRequest] DataSourceRequest request, string templateId, string discipline, string revision, DateTime fromDate, DateTime toDate)
        {
            var result = await _dmsDocumentBusiness.DocumentReceivedCommentsReportData(_userContext.UserId, templateId, discipline, revision, fromDate, toDate);
            return Json(result);
            //return Json(result.ToDataSourceResult(request));
        }

        public async Task<IActionResult> ReadDocumentReceivedCommentsDataGrid(string templateId, string discipline, string revision, DateTime fromDate, DateTime toDate)
        {
            var result = await _dmsDocumentBusiness.DocumentReceivedCommentsReportData(_userContext.UserId, templateId, discipline, revision, fromDate, toDate);
            return Json(result);
        }

        public IActionResult DMSTaskList(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, string portalNames = null)
        {
            ViewBag.reqType = requestby;
            ViewBag.PortalNames = portalNames;
            var model = new TaskViewModel { ModuleCode = moduleCodes, TemplateCode = templateCodes, TemplateCategoryCode = categoryCodes };
            return View(model);
        }

        public async Task<IActionResult> ReadTaskDataInProgress(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, string portalNames = null)
        {
            var ids = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                string[] portalIds = portals.Select(x => x.Id).ToArray();
                ids = String.Join("','", portalIds);
            }
            else
            {
                ids = _userContext.PortalId;
            }

            var result = await _taskBusiness.GetTaskListDataByWithNoteReferenceId(ids, moduleCodes, templateCodes, categoryCodes);
            if (requestby.IsNotNullAndNotEmpty())
            {
                if (requestby == "RequestedByMe")
                {
                    result = result.Where(x => x.RequestedByUserId == _userContext.UserId).ToList();
                }
                else if (requestby == "AssignedToMe")
                {
                    result = result.Where(x => x.AssignedToUserId == _userContext.UserId).ToList();
                }
            }
            if (templateCodes == "DMS_SUPPORT_TICKET")
            {
                //var j = Json(result.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_DRAFT").OrderByDescending(x => x.StartDate));
                var j = Json(result.Where(x => x.StatusGroupCode == "PENDING" || x.TaskStatusCode == "TASK_STATUS_DRAFT").OrderByDescending(x => x.CreatedDate));
                return j;
            }
            else
            {
                //var j = Json(result.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" && x.AssignedToUserId == _userContext.LoggedInAsByUserId).OrderByDescending(x => x.StartDate));
                var j = Json(result.Where(x => x.StatusGroupCode == "PENDING" && x.AssignedToUserId == _userContext.UserId).OrderByDescending(x => x.CreatedDate));
                return j;
            }

        }

        public async Task<IActionResult> ReadTop10TaskDataInProgress(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, string portalNames = null)
        {
            var ids = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                string[] portalIds = portals.Select(x => x.Id).ToArray();
                ids = String.Join("','", portalIds);
            }
            else
            {
                ids = _userContext.PortalId;
            }

            var result = await _taskBusiness.GetTaskList(ids, moduleCodes, templateCodes, categoryCodes);
            if (requestby.IsNotNullAndNotEmpty())
            {
                if (requestby == "RequestedByMe")
                {
                    result = result.Where(x => x.RequestedByUserId == _userContext.UserId).ToList();
                }
                else if (requestby == "AssignedToMe")
                {
                    result = result.Where(x => x.AssignedToUserId == _userContext.UserId).ToList();
                }
            }
            if (templateCodes == "DMS_SUPPORT_TICKET")
            {
                //var j = Json(result.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_DRAFT").OrderByDescending(x => x.StartDate));
                var j = Json(result.Where(x => x.StatusGroupCode == "PENDING" || x.TaskStatusCode == "TASK_STATUS_DRAFT").OrderByDescending(x => x.StartDate));
                return j;
            }
            else
            {
                //var j = Json(result.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" && x.AssignedToUserId == _userContext.LoggedInAsByUserId).OrderByDescending(x => x.StartDate));
                var j = Json(result.Where(x => x.StatusGroupCode == "PENDING" && x.AssignedToUserId == _userContext.UserId).OrderByDescending(x => x.StartDate).Take(10));
                return j;
            }

        }
        public async Task<IActionResult> ReadTaskDataOverdue(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, string portalNames = null)
        {
            var ids = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                string[] portalIds = portals.Select(x => x.Id).ToArray();
                ids = String.Join("','", portalIds);
            }
            else
            {
                ids = _userContext.PortalId;
            }

            var result = await _taskBusiness.GetTaskList(ids, moduleCodes, templateCodes, categoryCodes);
            if (requestby.IsNotNullAndNotEmpty())
            {
                if (requestby == "RequestedByMe")
                {
                    result = result.Where(x => x.RequestedByUserId == _userContext.UserId).ToList();
                }
                else if (requestby == "AssignedToMe")
                {
                    result = result.Where(x => x.AssignedToUserId == _userContext.UserId).ToList();
                }
            }
            var j = Json(result.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE" && x.AssignedToUserId == _userContext.UserId).OrderByDescending(x => x.StartDate));
            return j;
        }
        public async Task<IActionResult> ReadTaskDataCompleted(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, string portalNames = null)
        {
            var ids = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                string[] portalIds = portals.Select(x => x.Id).ToArray();
                ids = String.Join("','", portalIds);
            }
            else
            {
                ids = _userContext.PortalId;
            }

            var result = await _taskBusiness.GetTaskListDataByWithNoteReferenceId(ids, moduleCodes, templateCodes, categoryCodes);
            if (requestby.IsNotNullAndNotEmpty())
            {
                if (requestby == "RequestedByMe")
                {
                    result = result.Where(x => x.RequestedByUserId == _userContext.UserId).ToList();
                }
                else if (requestby == "AssignedToMe")
                {
                    result = result.Where(x => x.AssignedToUserId == _userContext.UserId).ToList();
                }
            }
            //var j = Json(result.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE" && x.AssignedToUserId == _userContext.LoggedInAsByUserId).OrderByDescending(x => x.StartDate));
            var j = Json(result.Where(x => x.StatusGroupCode == "DONE" && x.AssignedToUserId == _userContext.UserId).OrderByDescending(x => x.CreatedDate));
            return j;
        }

    }
}
