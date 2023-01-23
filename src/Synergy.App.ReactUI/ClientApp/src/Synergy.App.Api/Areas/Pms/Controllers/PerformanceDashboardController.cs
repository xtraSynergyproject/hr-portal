using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Hangfire;

namespace Synergy.App.Api.Areas.Pms.Controllers
{
    [Route("pms/PerformanceDashboard")]
    [ApiController]
    public class PerformanceDashboardController : ApiController
    {
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IPerformanceManagementBusiness _performanceManagementBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly INoteBusiness _noteBusiness;
        private readonly IServiceProvider _serviceProvider;
        //private readonly IHangfireScheduler _hangfireScheduler;
        public PerformanceDashboardController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IServiceProvider serviceProvider,
            IPerformanceManagementBusiness performanceManagementBusiness, INoteBusiness noteBusiness,
        ITaskBusiness taskBusiness, IUserRoleBusiness userRoleBusiness, IServiceBusiness serviceBusiness
            //, IHangfireScheduler hangfireScheduler
            ) : base(serviceProvider)
        {
            _customUserManager = customUserManager;
            _serviceProvider = serviceProvider;
            _performanceManagementBusiness = performanceManagementBusiness;
            _userRoleBusiness = userRoleBusiness;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
            _noteBusiness = noteBusiness;
            //_hangfireScheduler = hangfireScheduler;
        }

        [HttpGet]
        [Route("GetPerformanceStages")]
        public async Task<ActionResult> GetPerformanceStages(string performanceId, string ownerUserId)
        {
            var data = await _performanceManagementBusiness.GetPerformanceDocumentStageDataByServiceId(performanceId, ownerUserId);
            return Ok(data);
        }

        [HttpGet]
        [Route("LoadBook")]
        public async Task<IActionResult> LoadBook(string id, string userId = null, string docId = null)
        {
            await Authenticate(userId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var userIds = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                userIds = userId;
            }
            else
            {
                if (!_userContext.UserRoleCodes.Contains("ADMIN"))
                {
                    userIds = _userContext.UserId;
                }
            }

            var documents = await _performanceManagementBusiness.LoadWorkBooks(userIds, docId);

            if (id.IsNotNullAndNotEmpty())
            {
                documents = documents.Where(x => x.ParentServiceId == id).ToList();
            }
            else
            {
                documents = documents.Where(x => x.ParentServiceId == null).ToList();
            }
            return Ok(documents);

        }

        [HttpGet]
        [Route("PerformanceBookHTML")]
        public async Task<IActionResult> PerformanceBookHTML(string recordId, string userId)
        {
            await Authenticate(userId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var model = await _performanceManagementBusiness.GetBookDetails(recordId);
            model.PortalId = _userContext.PortalId;
            return Ok(model);
        }

        [HttpGet]
        [Route("WorkBookCount")]
        public async Task<IActionResult> WorkBookCount(string bookId, string userId)
        {
            WorklistDashboardSummaryViewModel model = new WorklistDashboardSummaryViewModel();

            await Authenticate(userId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var books = await _performanceManagementBusiness.GetBookList(bookId, null, true);
            var task = books.Where(x => x.NtsType == NtsTypeEnum.Task && x.AssigneeUserId == _userContext.UserId).ToList();

            var assToMePlanned = task.Where(x => x.StatusCode == "TASK_STATUS_PLANNED").Count();
            var assToMePlannedOverdue = task.Where(x => x.StatusCode == "TASK_STATUS_PLANNED_OVERDUE").Count();
            var assToMeOverdue = task.Where(x => x.StatusCode == "TASK_STATUS_OVERDUE").Count();
            var assToMePending = task.Where(x => x.StatusCode == "TASK_STATUS_INPROGRESS").Count();
            var assToMeCompleted = task.Where(x => x.StatusCode == "TASK_STATUS_COMPLETE").Count();
            var assToMeRejected = task.Where(x => x.StatusCode == "TASK_STATUS_REJECT").Count();
            var assToMeCancel = task.Where(x => x.StatusCode == "TASK_STATUS_CANCEL").Count();

            model.T_AssignPlanned = Convert.ToInt64(assToMePlanned);
            model.T_AssignPlannedOverdue = Convert.ToInt64(assToMePlannedOverdue);
            model.T_AssignPending = Convert.ToInt64(assToMePending);
            model.T_AssignCompleted = Convert.ToInt64(assToMeCompleted);
            model.T_AssignOverdue = Convert.ToInt64(assToMeOverdue);
            model.T_AssignReject = Convert.ToInt64(assToMeRejected);
            model.T_AssignCancel = Convert.ToInt64(assToMeCancel);

            var referenceIds = books.Select(x => x.Id).ToArray();
            var refIds = string.Join(",", referenceIds);

            var notification = await _performanceManagementBusiness.GetNotificationsList(refIds);
            if (notification != null)
            {
                model.ReadCount = notification.Where(x => x.ReadStatus == ReadStatusEnum.Read).Count();
                model.UnReadCount = notification.Where(x => x.ReadStatus == ReadStatusEnum.NotRead).Count();
                model.ArchivedCount = notification.Where(x => x.IsArchived == true).Count();
            }

            return Ok(model);
        }

        #region Performance Result

        [HttpGet]
        [Route("GetPerformanceChart")]
        public async Task<ActionResult> GetPerformanceChart(string pdmId, string deptId = null, string stageId = null, string userId = null)
        {
            //var pdmaster = await _performanceManagementBusiness.GetPDMDetails(pdmId);
            var viewModel = await _performanceManagementBusiness.GetPerformanceFinalReport(pdmId, deptId, null, stageId);
            if (deptId.IsNotNullAndNotEmpty())
            {
                viewModel = viewModel.Where(x => x.DepartmentId == deptId).ToList();
            }
            if (userId.IsNotNullAndNotEmpty())
            {
                viewModel = viewModel.Where(x => x.UserId == userId).ToList();
            }
            var list = new List<ProjectDashboardChartViewModel>();
            //var list1 = viewModel.GroupBy(x => x.RatingCode).Select(group => new { Value = group.Count(), Type = group.Key }).ToList();
            var list1 = viewModel.GroupBy(x => x.FinalRatingRounded).Select(group => new { Value = group.Count(), Type = group.Key }).ToList();

            if (list1.Count > 0)
            {
                list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type.IsNotNull() ? x.Type.ToString() : "0", Value = x.Value }).ToList();
                list = list.Where(x => x.Type != "0").OrderBy(x => x.Type).ToList();

                var newlist = new ProjectDashboardChartViewModel
                {
                    ItemValueLabel = list.Select(x => x.Type).ToList(),
                    ItemValueSeries = list.Select(x => x.Value).ToList()
                };
                return Ok(newlist);
            }
            return Ok(list);

        }

        [HttpGet]
        [Route("GetPerDocMasterStage")]
        public async Task<ActionResult> GetPerDocMasterStage(string performanceId, string ownerUserId)
        {
            var data = await _performanceManagementBusiness.GetPerDocMasterStageDataByServiceId(performanceId, ownerUserId);
            return Ok(data);
        }

        #endregion

        #region Performance Summary

        [HttpGet]
        [Route("ReadPerformanceSummaryData")]
        public async Task<IActionResult> ReadPerformanceSummaryData(string filter)
        {

            var list = await _performanceManagementBusiness.GetPerformanceSummaryData(filter);
            var j = Ok(list);
            return j;
        }

        #endregion


        #region Performance Rating 

        [HttpGet]
        [Route("ReadPerformanceRatingData")]
        public async Task<ActionResult> ReadPerformanceRatingData()
        {
            var model = await _performanceManagementBusiness.GetPerformanceRatingList();
            return Ok(model);
        }


        [HttpGet]
        [Route("PerformanceRating")]
        public async Task<IActionResult> PerformanceRating(string Id)
        {
            var model = new PerformaceRatingViewModel();
            if (Id.IsNotNullAndNotEmpty())
            {
                model = await _performanceManagementBusiness.GetPerformanceRatingDetails(Id);
                model.DataAction = DataActionEnum.Edit;
                if (model.ParentNoteId.IsNullOrEmpty())
                {
                    model.ParentNoteId = "0";
                }

            }
            else
            {
                model.DataAction = DataActionEnum.Create;

                model.ParentNoteId = "0";
            }
            return Ok(model);
        }


        [HttpPost]
        [Route("ManagePerformanceRating")]
        public async Task<IActionResult> ManagePerformanceRating(TagCategoryViewModel model)
        {
            await Authenticate(model.OwnerUserId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var exist = await _performanceManagementBusiness.IsRatingNameExist(model.Name, model.Id);
            if (exist != null)
            {
                return Ok(new { success = false, error = "The given performance rating name already exist" });
            }


            if (model.DataAction == DataActionEnum.Create)
            {

                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "PERFORMANCE_RATING";
                noteTempModel.ParentNoteId = model.ParentNoteId;

                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.Status = model.Status;
                notemodel.Json = JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";


                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
                return Ok(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            else
            {

                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ParentNoteId = model.ParentNoteId;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.NoteId;


                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.Status = model.Status;
                notemodel.Json = JsonConvert.SerializeObject(model);
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    if (model.TagCategoryType == TagCategoryTypeEnum.Master)
                    {
                        //await _ntsTagBusiness.GenerateTagsForCategory(result.Item.NoteId);
                        //BackgroundJob.Enqueue<HangfireScheduler>(x => x.GenerateTagsForCategory(result.Item.NoteId, _userContext.ToIdentityUser()));
                        var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                        await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.GenerateTagsForCategory(result.Item.NoteId, _userContext.ToIdentityUser()));
                    }
                    return Ok(new { success = true });
                }
                return Ok(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string Id)
        {
            await _performanceManagementBusiness.DeletePerformanceRating(Id);
            return Ok("Success");
        }

        //Performance Rating Item


        [HttpGet]
        [Route("CreateItem")]
        public async Task<IActionResult> CreateItem(string Id, string ParentNodeId)
        {
            var model = new PerformanceRatingItemViewModel();
            if (Id.IsNotNullAndNotEmpty())
            {
                model = await _performanceManagementBusiness.GetPerformanceRatingItemDetails(Id);
                model.DataAction = DataActionEnum.Edit;
                if (model.ParentNoteId.IsNullOrEmpty())
                {
                    model.ParentNoteId = ParentNodeId;
                }

            }
            else
            {
                model.DataAction = DataActionEnum.Create;

                model.ParentNoteId = ParentNodeId;
            }
            return Ok(model);
        }


        [HttpPost]
        [Route("ManageItem")]
        public async Task<IActionResult> ManageItem(PerformanceRatingItemViewModel model)
        {
            await Authenticate(model.OwnerUserId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();



            var exist = await _performanceManagementBusiness.IsRatingItemExist(model.ParentNoteId, model.Name, model.Id);
            if (exist != null)
            {
                return Ok(new { success = false, error = "The given performance rating item name already exist" });
            }



            var exist1 = await _performanceManagementBusiness.IsRatingItemCodeExist(model.ParentNoteId, model.code.ToString(), model.Id);
            if (exist1 != null)
            {
                return Ok(new { success = false, error = "The given performance rating item code already exist" });
            }


            if (model.DataAction == DataActionEnum.Create)
            {

                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "PERFORMANCE_RATING_ITEM";
                noteTempModel.ParentNoteId = model.ParentNoteId;
                noteTempModel.Status = model.Status;
                //noteTempModel.NoteSubject = model.NoteSubject;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                notemodel.Status = model.Status;
                //notemodel.NoteSubject = model.NoteSubject;

                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
                return Ok(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            else
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ParentNoteId = model.ParentNoteId;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.NoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model);
                notemodel.NoteSubject = model.NoteSubject;
                notemodel.Status = model.Status;
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
                return Ok(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
        }
        [HttpGet]
        [Route("ReadItemData")]
        public async Task<ActionResult> ReadItemData(string ParentNoteId)
        {
            var model = await _performanceManagementBusiness.GetPerformanceRatingItemList(ParentNoteId);
            var j = Ok(model);
            return j;
        }

        [HttpGet]
        [Route("Deleteitem")]
        public async Task<IActionResult> Deleteitem(string Id)
        {
            await _performanceManagementBusiness.DeletePerformanceRatingItem(Id);
            return Ok("Success");
        }


        #endregion
    }
}
