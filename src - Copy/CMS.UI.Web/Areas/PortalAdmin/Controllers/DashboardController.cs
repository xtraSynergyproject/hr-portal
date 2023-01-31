using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using CMS.Common;
using CMS.Business;
using CMS.UI.Utility;

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class DashboardController : ApplicationController
    {
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IUserContext _userContext;
        private readonly ICmsBusiness _cmsBusiness;
        public DashboardController(ITemplateBusiness templateBusiness, IServiceBusiness serviceBusiness,
            IUserContext userContext, ITaskBusiness taskBusiness, INoteBusiness noteBusiness, ICmsBusiness cmsBusiness)
        {
            _templateBusiness = templateBusiness;
            _serviceBusiness = serviceBusiness;
            _userContext = userContext;
            _taskBusiness = taskBusiness;
            _noteBusiness = noteBusiness;
            _cmsBusiness = cmsBusiness;
        }   

        public async Task<IActionResult> WorkbookDashbaord(string categoryCodes, string requestby)
        {
            WorklistDashboardSummaryViewModel model = new WorklistDashboardSummaryViewModel();
            var NoteCount=await _noteBusiness.NotesCountForDashboard(_userContext.UserId,null);
            if (NoteCount!=null) 
            {
                model.N_CreatedByMeActive = NoteCount.createdByMeActive;
                model.N_CreatedByMeDraft  = NoteCount.createdByMeDraft;
                model.N_CreatedByMeExpired = NoteCount.createdByMeExpired;
                model.N_CreatedByMeAll = model.N_CreatedByMeActive + model.N_CreatedByMeDraft + model.N_CreatedByMeExpired;

            }
            var TaskCount = await _taskBusiness.TaskCountForDashboard(_userContext.UserId,null);
            if (TaskCount != null)
            {
                model.T_AssignPending = TaskCount.T_AssignPending;
                model.T_AssignCompleted = TaskCount.T_AssignCompleted;
                model.T_AssignOverdue = TaskCount.T_AssignOverdue;
                model.T_AssignReject = TaskCount.T_AssignReject;
                model.T_AssignPendingOverdue = TaskCount.T_AssignPending+ TaskCount.T_AssignOverdue;
                model.T_AssignAll = model.T_AssignPendingOverdue + model.T_AssignCompleted + model.T_AssignReject;
            }
            var notification = await _taskBusiness.NotificationDashboardIndex(_userContext.UserId,null);
            if (notification != null)
            {
                model.ReadCount = notification.Where(x=>x.ReadStatus==ReadStatusEnum.Read).Count();
                model.UnReadCount = notification.Where(x => x.ReadStatus == ReadStatusEnum.NotRead).Count();
                model.ArchivedCount = notification.Where(x => x.IsArchived == true).Count();
                model.AllCount = model.ReadCount + model.UnReadCount + model.ArchivedCount;
            }

            return View(model);
        }

        public async Task<IActionResult> ProcessBookDashboard(string categoryCodes, string requestby)
        {
            WorklistDashboardSummaryViewModel model = new WorklistDashboardSummaryViewModel();
            var NoteCount = await _noteBusiness.ProcessBookCountForDashboard(_userContext.UserId, null);
            if (NoteCount != null)
            {
                model.N_CreatedByMeActive = NoteCount.createdByMeActive;
                model.N_CreatedByMeDraft = NoteCount.createdByMeDraft;
                model.N_CreatedByMeExpired = NoteCount.createdByMeExpired;
                model.N_CreatedByMeOverdue = NoteCount.createdByMeOverdue;
                model.N_CreatedByMeCompleted = NoteCount.createdByMeCompleted;
                model.N_CreatedByMeAll = model.N_CreatedByMeCompleted+ model.N_CreatedByMeOverdue+model.N_CreatedByMeActive + model.N_CreatedByMeDraft + model.N_CreatedByMeExpired;

            }
            var StageCount = await _noteBusiness.ProcessStageCountForDashboard(_userContext.UserId, null);
            if (StageCount != null)
            {
                model.PS_CreatedByMeActive = StageCount.createdByMeActive;
                model.PS_CreatedByMeDraft = StageCount.createdByMeDraft;
                model.PS_CreatedByMeExpired = StageCount.createdByMeExpired;
                model.PS_CreatedByMeOverdue = StageCount.createdByMeOverdue;
                model.PS_CreatedByMeCompleted = StageCount.createdByMeCompleted;
                model.PS_CreatedByMeAll = model.PS_CreatedByMeCompleted+ model.PS_CreatedByMeOverdue + model.PS_CreatedByMeActive + model.PS_CreatedByMeDraft + model.PS_CreatedByMeExpired;

            }
            var TaskCount = await _taskBusiness.TaskCountForDashboard(_userContext.UserId, null);
            if (TaskCount != null)
            {
                model.T_AssignPending = TaskCount.T_AssignPending;
                model.T_AssignCompleted = TaskCount.T_AssignCompleted;
                model.T_AssignOverdue = TaskCount.T_AssignOverdue;
                model.T_AssignReject = TaskCount.T_AssignReject;
                model.T_AssignCancel = TaskCount.T_AssignCancel;
                model.T_AssignPendingOverdue = TaskCount.T_AssignPending + TaskCount.T_AssignOverdue;
                model.T_AssignAll = model.T_AssignCancel+model.T_AssignPendingOverdue + model.T_AssignCompleted + model.T_AssignReject;
            }
            var notification = await _taskBusiness.NotificationDashboardIndex(_userContext.UserId, null);
            if (notification != null)
            {
                model.ReadCount = notification.Where(x => x.ReadStatus == ReadStatusEnum.Read).Count();
                model.UnReadCount = notification.Where(x => x.ReadStatus == ReadStatusEnum.NotRead).Count();
                model.ArchivedCount = notification.Where(x => x.IsArchived == true).Count();
                model.AllCount = model.ReadCount + model.UnReadCount + model.ArchivedCount;
            }

            return View(model);
        }


        public async Task<IActionResult> LoadBook([DataSourceRequest] DataSourceRequest request,string id,string statusFilter)
        {
            var data = await _taskBusiness.LoadWorkBooks(_userContext.UserId,statusFilter);
            if (id.IsNotNullAndNotEmpty())
            {
                data = data.Where(x => x.ParentId == id).ToList();
            }
            else
            {
                data = data.Where(x => x.ParentId == null).ToList();
            }
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return Json(data);
            // return Json(data.ToDataSourceResult(request));
        }

        public async Task<IActionResult> LoadProcessBooks([DataSourceRequest] DataSourceRequest request, string statusFilter)
        {
            var data = await _taskBusiness.LoadProcessBooks(_userContext.UserId, statusFilter);
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<IActionResult> LoadBookTask([DataSourceRequest] DataSourceRequest request, string statusFilter)
        {
            var data = await _taskBusiness.TaskDashboardIndex(_userContext.UserId,statusFilter);
            return Json(data);
        }
        public async Task<IActionResult> LoadNotifications(string statusFilter)
        {
            var data = await _taskBusiness.NotificationDashboardIndex(_userContext.UserId,null);
            if (statusFilter == "Read")
            {
                data = data.Where(x => x.ReadStatus == ReadStatusEnum.Read).ToList();
            }
            else if (statusFilter == "UnRead")
            {
                data = data.Where(x => x.ReadStatus == ReadStatusEnum.NotRead).ToList();
            }
            else if (statusFilter == "Archived")
            {
                data = data.Where(x => x.IsArchived == true).ToList();
            }
            return Json(data);
        }
        public async Task<IActionResult> LoadProcessStage([DataSourceRequest] DataSourceRequest request, string statusFilter)
        {
            var data = await _taskBusiness.LoadProcessStage(_userContext.UserId, statusFilter);
            return Json(data.ToDataSourceResult(request));
        }

        public async Task<IActionResult> WorkBookCount(string bookId)
        {
            WorklistDashboardSummaryViewModel model = new WorklistDashboardSummaryViewModel();
            var NoteCount = await _noteBusiness.NotesCountForDashboard(_userContext.UserId, bookId);
            if (NoteCount != null)
            {
                model.N_CreatedByMeActive = NoteCount.createdByMeActive;
                model.N_CreatedByMeDraft = NoteCount.createdByMeDraft;
                model.N_CreatedByMeExpired = NoteCount.createdByMeExpired;

            }
            var TaskCount = await _taskBusiness.TaskCountForDashboard(_userContext.UserId, bookId);
            if (TaskCount != null)
            {
                model.T_AssignPlanned = TaskCount.T_AssignPlanned;
                model.T_AssignPlannedOverdue = TaskCount.T_AssignPlannedOverdue;
                model.T_AssignPending = TaskCount.T_AssignPending;
                model.T_AssignCompleted = TaskCount.T_AssignCompleted;
                model.T_AssignOverdue = TaskCount.T_AssignOverdue;
                model.T_AssignReject = TaskCount.T_AssignReject;
            }
            var notification = await _taskBusiness.NotificationDashboardIndex(_userContext.UserId, bookId);
            if (notification != null)
            {
                model.ReadCount = notification.Where(x => x.ReadStatus == ReadStatusEnum.Read).Count();
                model.UnReadCount = notification.Where(x => x.ReadStatus == ReadStatusEnum.NotRead).Count();
                model.ArchivedCount = notification.Where(x => x.IsArchived == true).Count();
            }

            return Json(model);
        }
       
    }
}
