using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Hangfire;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Pms.Controllers
{
    [Area("Pms")]
    public class PerformanceDocumentController : ApplicationController
    {

        private readonly IPerformanceManagementBusiness _pmtBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserContext _userContext;
        private readonly IPushNotificationBusiness _notificationBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IEmailBusiness _emailBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IHRCoreBusiness _hRCoreBusiness;
        private readonly IUserHierarchyBusiness _userHierarchyBusiness;
        ICmsBusiness _cmsBusiness;
        IServiceProvider _serviceProvider;
        //private readonly IHangfireScheduler _hangfireScheduler;
        public PerformanceDocumentController(IEmailBusiness emailBusiness, IPushNotificationBusiness notificationBusiness, IPerformanceManagementBusiness pmtBusiness, IUserBusiness _userBusiness,
            IUserContext userContext, IServiceBusiness serviceBusiness, IUserBusiness userBusiness, ITableMetadataBusiness tableMetadataBusiness,
            ITaskBusiness taskBusiness, IHRCoreBusiness hRCoreBusiness, IUserHierarchyBusiness userHierarchyBusiness
            , INoteBusiness noteBusiness, ICmsBusiness cmsBusiness
            , IServiceProvider serviceProvider
            //, IHangfireScheduler hangfireScheduler
            )
        {
            _pmtBusiness = pmtBusiness;
            _userContext = userContext;
            _notificationBusiness = notificationBusiness;
            _serviceBusiness = serviceBusiness;
            _userBusiness = userBusiness;
            _taskBusiness = taskBusiness;
            _noteBusiness = noteBusiness;
            _emailBusiness = emailBusiness;
            _cmsBusiness = cmsBusiness;
            _hRCoreBusiness = hRCoreBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _userHierarchyBusiness = userHierarchyBusiness;
            _serviceProvider = serviceProvider;
            //_hangfireScheduler = hangfireScheduler;
        }

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Inbox()
        {
            var model = new ApplicationSearchViewModel();
            return View(model);
        }
        public ActionResult WbsTimeLine(string PerformanceId)
        {
            var model = new ProgramDashboardViewModel { Id = PerformanceId };
            return View(model);

        }
        public async Task<ActionResult> PerformanceDashboard(string PerformanceId)
        {
            var Performancelist = await _pmtBusiness.GetPerformanceSharedList(_userContext.UserId);
            if (PerformanceId.IsNullOrEmpty() && Performancelist != null && Performancelist.Count > 0)
            {
                PerformanceId = Performancelist.FirstOrDefault().Id;
            }
            var model = await _pmtBusiness.GetPerformanceDashboardDetails(PerformanceId);
            if (model != null)
            {
                model.ProjectList = Performancelist.ToList();
            }
            return View(model);
        }
        public async Task<ActionResult> GetPerformanceTaskChartByStatus(string PerformanceId, string userId, string stageId)
        {
            if (userId.IsNullOrEmpty())
            {
                userId = _userContext.UserId;
            }
            var viewModel = await _pmtBusiness.GetTaskStatus(userId, PerformanceId, stageId);
            return Json(viewModel);
        }
        public async Task<ActionResult> GetPerformanceTaskChartByType(string PerformanceId, string stageId)
        {
            var viewModel = await _pmtBusiness.GetTaskType(_userContext.UserId, PerformanceId, stageId);
            return Json(viewModel);
        }
        public async Task<ActionResult> GetPerformanceStageChart(string PerformanceId)
        {
            var viewModel = await _pmtBusiness.ReadPerformanceStageChartData(_userContext.UserId, PerformanceId);
            return Json(viewModel);
        }
        public async Task<ActionResult> GetPerformanceStageIdNameList(string PerformanceId)
        {
            var viewModel = await _pmtBusiness.GetPerformanceStageIdNameList(_userContext.UserId, PerformanceId);
            return Json(viewModel);
        }
        public async Task<ActionResult> GetPerformanceIdNameList()
        {
            var Performancelist = await _pmtBusiness.GetPerformanceSharedList(_userContext.UserId);
            return Json(Performancelist);
        }

        public ActionResult GetPerformanceTimeLog(long PerformanceId)
        {
            var viewModel = new ProjectDashboardChartViewModel();
            return Json(viewModel);
        }
        public async Task<ActionResult> PerformanceWorkBreakDownStructure(string PerformanceId)
        {
            ViewBag.PerformanceId = PerformanceId;
            var model = new ProgramDashboardViewModel { Id = PerformanceId };
            return View(model);
        }
        public ActionResult GetPerformanceAttachments([DataSourceRequest] DataSourceRequest request, string PerformanceId)
        {
            //var list = _pmtBusiness.ReadTaskFileData(PerformanceId);
            var list = new List<FileViewModel>();
            return Json(list);
            // return Json(list.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetPerformanceNotificationList([DataSourceRequest] DataSourceRequest request, string PerformanceId)
        {
            //var result = _notificationBusiness.GetPerformanceNotificationList(LoggedInUserId, PerformanceId);
            //var result = new List<NotificationViewModel>();
            var userId = _userContext.UserId;
            var result = await _notificationBusiness.GetServiceNotificationList(PerformanceId, userId, 0);
            var j = Json(result);
            //var j = Json(result.ToDataSourceResult(request));
            return j;
        }
        public ActionResult GetPerformanceTeamData([DataSourceRequest] DataSourceRequest request, string PerformanceId)
        {
            //var list = _pmtBusiness.ReadPerformanceTeamData(PerformanceId);
            var list = new List<TeamTaskDashboardViewModel>();
            return Json(list);
            //return Json(list.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetPerformanceOverDueTask([DataSourceRequest] DataSourceRequest request, string PerformanceId)
        {
            var list = await _pmtBusiness.ReadTaskOverdueData(PerformanceId);
            //var list = new List<TaskViewModel>();
            return Json(list);
            //return Json(list.ToDataSourceResult(request));
        }
        public ActionResult PerformanceNotAssigned()
        {
            return View();
        }

        public async Task<IActionResult> ReadWBSTimelineGanttChartData([DataSourceRequest] DataSourceRequest request, string PerformanceId, List<string> PerformanceIds = null, List<string> senderIds = null, List<string> recieverids = null, List<string> statusIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (dateRange.IsNotNull() && dateRange == DateTypeEnum.Today)
            {
                fromDate = System.DateTime.Now;
                toDate = fromDate.Value.AddDays(1);
            }
            else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.NextWeek)
            {
                fromDate = System.DateTime.Now;
                toDate = fromDate.Value.AddDays(8);
            }
            else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.NextMonth)
            {
                fromDate = System.DateTime.Now;
                toDate = fromDate.Value.AddDays(31);
            }
            else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.Between)
            {
                toDate = toDate.Value.AddDays(1);
            }
            var list = await _pmtBusiness.ReadWBSTimelineGanttChartData(_userContext.UserId, PerformanceId, _userContext.UserRoleCodes, PerformanceIds, senderIds, recieverids, statusIds, column, fromDate, toDate);
            var j = Json(list);
            //var j = Json(list.ToDataSourceResult(request));
            return j;
        }
        public async Task<IActionResult> ReadPerformanceTaskGridViewData([DataSourceRequest] DataSourceRequest request, string PerformanceId, string objectiveId, List<string> PerformanceIds = null, List<string> senderIds = null, List<string> recieverids = null, List<string> statusIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (dateRange.IsNotNull() && dateRange == DateTypeEnum.Today)
            {
                fromDate = System.DateTime.Now;
                toDate = fromDate.Value.AddDays(1);
            }
            else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.NextWeek)
            {
                fromDate = System.DateTime.Now;
                toDate = fromDate.Value.AddDays(8);
            }
            else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.NextMonth)
            {
                fromDate = System.DateTime.Now;
                toDate = fromDate.Value.AddDays(31);
            }
            else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.Between)
            {
                toDate = toDate.Value.AddDays(1);
            }
            var list = await _pmtBusiness.ReadPerformanceTaskGridViewData(_userContext.UserId, PerformanceId, objectiveId, _userContext.UserRoleCodes, PerformanceIds, senderIds, recieverids, statusIds, column, fromDate, toDate);
            //var j = Json(list.ToDataSourceResult(request));
            var j = Json(list);

            return j;
        }
        public virtual JsonResult ReadWBSTimelineGanttDependencyData([DataSourceRequest] DataSourceRequest request)
        {
            var list = new List<ProjectGanttTaskViewModel>();
            list.Add(new ProjectGanttTaskViewModel { Title = "Completed Task", Start = DateTime.Now, End = DateTime.Now, UserName = "Saman", OwnerName = "System Admin" });
            list.Add(new ProjectGanttTaskViewModel { Title = "InProgress Task", Start = DateTime.Now, End = DateTime.Now, UserName = "Saman", OwnerName = "System Admin" });
            list.Add(new ProjectGanttTaskViewModel { Title = "Draft Task", Start = DateTime.Now, End = DateTime.Now, UserName = "Saman", OwnerName = "System Admin" });
            list.Add(new ProjectGanttTaskViewModel { Title = "Draft Task", Start = DateTime.Now, End = DateTime.Now, UserName = "Saman", OwnerName = "System Admin" });
            //var list = _pmtBusiness.GetWBSTimelineGanttDependencyData(PerformanceId);
            var j = Json(list);
            //var j = Json(list.ToDataSourceResult(request));
            return j;
        }

        public async Task<IActionResult> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string stageName, string stageId, string performanceId, string expandingList)
        {

            var result = await _pmtBusiness.GetInboxMenuItemByUser(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, performanceId, expandingList, _userContext.UserRoleCodes);
            var model = result.ToList();
            return Json(model);
        }

        public async Task<IActionResult> GetDiagramTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string stageName, string stageId, string batchId, string expandingList)
        {

            var result = await _pmtBusiness.GetDiagramMenuItem(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes);
            var model = result.ToList();
            return Json(model);
        }

        public async Task<IActionResult> PerformanceMindMap(string PerformanceId)
        {
            ViewBag.PerformanceId = PerformanceId;
            return View();
        }

        public async Task<IActionResult> MindMapSettings()
        {
            return View();
        }
        public async Task<IActionResult> WBSDiagram(string PerformanceId)
        {
            ViewBag.PerformanceId = PerformanceId;
            return View();
        }

        public async Task<IActionResult> GetWBSItem(string PerformanceId)
        {
            //var result = await _pmtBusiness.GetWBSItemData(null, null, null, null, _userContext.UserId, _userContext.UserRoleIds, null, null, null, null, _userContext.UserRoleCodes);
            //return Json(result.ToList());

            var result = await _pmtBusiness.ReadMindMapData(PerformanceId);
            return Json(result.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> SaveMindMap(ServiceTemplateViewModel model)
        {
            var res = await _pmtBusiness.CreateMindMap(model.Json);
            return null;
        }
        public async Task<ActionResult> WorklistDashboardCount(string moduleCode)
        {
            var userId = _userContext.UserId;
            var count = await _serviceBusiness.GetWorklistDashboardCount(userId, moduleCode);
            // var j = Json(count);
            return Json(count);
        }
        public async Task<ActionResult> WorklistDashboardNotesCount(string moduleCode)
        {
            var userId = _userContext.UserId;
            var count = await _noteBusiness.NotesDashboardCount(userId, null, moduleCode);
            return Json(count);
        }


        public async Task<IActionResult> TestRecieveEmail(ServiceTemplateViewModel model)
        {
            await _emailBusiness.ReceiveMail();
            return null;
        }
        public async Task<JsonResult> ReadPerformanceMembers(string PerformanceId)
        {
            var model = await _pmtBusiness.ReadPerformanceTaskUserData(PerformanceId);
            return Json(model);
        }
        public async Task<ActionResult> SubOrdinateDashboardCount(string userId)
        {
            var count = await _serviceBusiness.GetWorklistDashboardCount(userId, null);
            // var j = Json(count);
            return Json(count);
        }


        //public async Task<IActionResult> ChangeStatusAndCalculatePerformanceRating(string noteId, string status, string stageNoteId)
        //{
        //    var PDM = await _pmtBusiness.GetPerformanceDocumentMasterByNoteId(noteId);
        //    if (status.IsNotNullAndNotEmpty())
        //    {
        //        if (PDM.IsNotNull())
        //        {
        //            if (status == "Active")
        //            {
        //                // Change status to Active on Unfreeze
        //                await _pmtBusiness.UpdatePerformanceDocumentMasterStatus(PDM.Id, PerformanceDocumentStatusEnum.Active);
        //            }
        //            else if (status == "Freezed")
        //            {
        //                // Change status to freeze
        //                await _pmtBusiness.UpdatePerformanceDocumentMasterStatus(PDM.Id, PerformanceDocumentStatusEnum.Freezed);
        //                await _pmtBusiness.CalculatePerformanceRating(PDM.Id, stageNoteId);
        //            }
        //            else if (status == "Released")
        //            {
        //                // Change status to release 
        //                await _pmtBusiness.UpdatePerformanceDocumentMasterStatus(PDM.Id, PerformanceDocumentStatusEnum.Released);
        //            }
        //            else
        //            {

        //                //var noteTempModel = new NoteTemplateViewModel();                        
        //                //noteTempModel.NoteId = noteId;
        //                //noteTempModel.SetUdfValue = true;
        //                //var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
        //                //var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);  
        //                //rowData1["CalculatedRatingStageId"] = stageId;
        //                //var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
        //                //var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);                            
        //                await _pmtBusiness.CalculatePerformanceRating(PDM.Id, stageNoteId);
        //            }
        //        }

        //    }
        //    return Json(new { success = true });
        //}

        public async Task<IActionResult> ChangeStageStatusAndCalculateStageRating(string noteId, string status, string stageId)
        {
            var PDM = await _pmtBusiness.GetPerformanceDocumentMasterByNoteId(noteId);
            if (PDM.IsNotNull() && status.IsNotNullAndNotEmpty())
            {
                var docStageModel = await _pmtBusiness.GetPerformanceDocumentStageData(noteId, null, stageId);
                switch (status)
                {
                    case "Publish":
                    case "RePublish":
                        var model = new PerformanceDocumentStageViewModel();

                        if (docStageModel.Count > 0)
                        {
                            model = docStageModel.FirstOrDefault();
                        }
                        if (model.EnableReview && model.ReviewStartDate.IsNotNull() && model.ReviewStartDate.Value.Date <= DateTime.Now.Date)
                        {

                            var result = await _pmtBusiness.UpdatePerformanceDocumentMasterStageStatus(stageId, PerformanceDocumentStatusEnum.Publishing);
                            if (result.IsTrue())
                            {
                                //await _pmtBusiness.GeneratePerformanceDocumentStages(model.Id);
                                var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                                await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.GeneratePerformanceDocumentStages(model.Id, _userContext.ToIdentityUser()));
                            }
                            return Json(new { success = true });
                        }
                        else
                        {
                            var result = await _pmtBusiness.UpdatePerformanceDocumentMasterStageStatus(stageId, PerformanceDocumentStatusEnum.Active);
                        }
                        break;

                    case "UnFreeze":
                        await _pmtBusiness.UpdatePerformanceDocumentMasterStageStatus(stageId, PerformanceDocumentStatusEnum.Active);
                        break;
                    case "Freeze":
                        await _pmtBusiness.UpdatePerformanceDocumentMasterStageStatus(stageId, PerformanceDocumentStatusEnum.Freezed);
                        break;
                    case "Release":
                        await _pmtBusiness.UpdatePerformanceDocumentMasterStageStatus(stageId, PerformanceDocumentStatusEnum.Released);
                        break;
                    case "Calculate":
                        await _pmtBusiness.CalculatePerformanceRating(PDM.Id, stageId);
                        break;
                    default:
                        //var noteTempModel = new NoteTemplateViewModel();
                        //noteTempModel.NoteId = noteId;
                        //noteTempModel.SetUdfValue = true;
                        //var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        //var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        //rowData1["CalculatedRatingStageId"] = stageId;
                        //var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                        //var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                        //await _pmtBusiness.CalculatePerformanceRating(PDM.Id, stageId);
                        break;
                }

            }
            return Json(new { success = true });
        }
        public IActionResult PerformanceDocumentMaster()
        {
            return View();
        }
        public IActionResult PerformanceDocumentMasterStage(string noteId, PerformanceDocumentStatusEnum? masterDocStatus, LayoutModeEnum? lo)
        {
            var model = new PerformanceDocumentStageViewModel();
            model.ParentNoteId = noteId;
            model.MasterDocumentStageStatus = masterDocStatus;
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            return View(model);
        }
        public async Task<IActionResult> PerformanceGradeRating(string noteId)
        {
            var model = new PerformanceDocumentViewModel();
            model.ParentNoteId = noteId;
            var docModel = await _pmtBusiness.GetPerformanceDocumentDetails(noteId);
            model.PerformanceRatingId = docModel.PerformanceRatingId;
            return View(model);
        }
        public async Task<ActionResult> ReadPerformanceDocumentData(PerformanceDocumentViewModel search = null)
        {
            var where = $@" order by ""N_PerformanceDocumentMaster_PerformanceDocumentMaster"".""StartDate"" desc";
            var model = await _cmsBusiness.GetDataListByTemplate("PERFORMANCE_DOCUMENT_MASTER", "", where);
            return Json(model);
        }
        public async Task<ActionResult> ReadPerformanceDocumentStageData(string ParentNoteId, PerformanceDocumentStageViewModel search = null)
        {
            //var model = await _cmsBusiness.GetDataListByTemplate("PERFORMACE_DOCUMENT_MASTER_STAGE", "");
            var model = await _pmtBusiness.GetPerformanceDocumentStageData(ParentNoteId);
            return Json(model);
        }
        public async Task<ActionResult> GetPerformanceDocumentStageDataByDocumentMasterNoteId(string masterNoteId, bool isEnableReview = false)
        {

            var model = await _pmtBusiness.GetPerformanceDocumentStageData(masterNoteId, null, null, isEnableReview);
            return Json(model);
        }
        public async Task<ActionResult> ReadPerformanceGradeRatingData([DataSourceRequest] DataSourceRequest request, string ParentNoteId)
        {
            var model = await _pmtBusiness.GetPerformanceGradeRatingData(ParentNoteId);
            return Json(model);
        }
        public async Task<IActionResult> CreatePDM(string noteId)
        {
            var model = new PerformanceDocumentViewModel();
            if (noteId.IsNotNullAndNotEmpty())
            {
                model = await _pmtBusiness.GetPerformanceDocumentDetails(noteId);
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.DocumentStatus = PerformanceDocumentStatusEnum.Draft;
            }
            return View("ManagePerformanceDocument", model);
        }
        [HttpPost]
        public async Task<IActionResult> ManagePerformanceDocument(PerformanceDocumentViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var result = await _pmtBusiness.CreatePerDoc(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = result.HtmlError });
                }
            }
            else
            {
                var result = await _pmtBusiness.EditPerDoc(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = result.HtmlError });
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> ManagePerformanceGradeRating(PerformanceDocumentViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var result = await _pmtBusiness.CreatePerGradeRating(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = result.HtmlError });
                }
            }
            else
            {
                var result = await _pmtBusiness.EditPerGradeRating(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = result.HtmlError });
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeletePerformanceDocument(string Docid)
        {

            var deletedoc = await _tableMetadataBusiness.DeleteTableDataByHeaderId("PERFORMANCE_DOCUMENT_MASTER", null, Docid);
            if (deletedoc.IsNotNull())
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }

        [HttpPost]
        public async Task<IActionResult> DeletePerformanceDocumentStage(string Docid)
        {

            var deletedoc = await _tableMetadataBusiness.DeleteTableDataByHeaderId("PERFORMACE_DOCUMENT_MASTER_STAGE", null, Docid);
            if (deletedoc.IsNotNull())
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }
        [HttpPost]
        public async Task<IActionResult> DeleteGradeRating(string gradeRatingId)
        {
            var deletedoc = await _tableMetadataBusiness.DeleteTableDataByHeaderId("PERFORMANCE_GRADE_RATING_PERCENTAGE", null, gradeRatingId);
            if (deletedoc.IsNotNull())
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public async Task<ActionResult> GetPerformanceDocumentGoalTemplate()
        {
            var list = await _pmtBusiness.GetPerformanceDocumentGoalTemplates();
            return Json(list);
        }

        public async Task<ActionResult> ReadPerformanceRatingList()
        {
            var list = await _pmtBusiness.GetPerformanceRatingsList();
            return Json(list);
        }

        public async Task<ActionResult> GetPerformanceDocumentCompetencyTemplate()
        {
            var list = await _pmtBusiness.GetPerformanceDocumentCompetencyTemplates();
            return Json(list);
        }
        public async Task<ActionResult> GetManagerReviewTemplate()
        {
            var list = await _pmtBusiness.GetManagerReviewTemplate();
            return Json(list);
        }
        public async Task<ActionResult> GetEmployeeReviewTemplate()
        {
            var list = await _pmtBusiness.GetEmployeeReviewTemplate();
            return Json(list);
        }

        public async Task<IActionResult> PerformanceDocumentStageCreate(string noteparentId, string perDocStageId)
        {
            var model = new PerformanceDocumentStageViewModel();
            if (perDocStageId.IsNotNullAndNotEmpty())
            {
                var docStageModel = await _pmtBusiness.GetPerformanceDocumentStageData(noteparentId, perDocStageId);
                if (docStageModel.Count > 0)
                {
                    model = docStageModel.FirstOrDefault();
                }

                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;

            }
            var docMasterData = await _tableMetadataBusiness.GetTableDataByColumn("PERFORMANCE_DOCUMENT_MASTER", "", "NtsNoteId", noteparentId);
            if (docMasterData.IsNotNull())
            {
                model.DocumentMasterId = docMasterData["Id"].ToString();
            }
            model.ParentNoteId = noteparentId;
            model.PerformanceStageObjective = PerformanceObjectiveStageEnum.StageObjective;

            return View("ManagePerformanceDocumentStage", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManagePerDocStage(PerformanceDocumentStageViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var result = await _pmtBusiness.CreatePerDocStage(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = result.HtmlError });
                }
            }
            else
            {
                var result = await _pmtBusiness.EditPerDocStage(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = result.HtmlError });
                }
            }
        }

        public async Task<IActionResult> PublishPerformanceDocumentStage(string pdStageId, string parentNoteId, string status)
        {
            var model = new PerformanceDocumentStageViewModel();
            var docStageModel = await _pmtBusiness.GetPerformanceDocumentStageData(parentNoteId, pdStageId);
            if (docStageModel.Count > 0)
            {
                model = docStageModel.FirstOrDefault();
            }
            model.DataAction = DataActionEnum.Edit;
            ViewBag.Status = status;
            return View(model);
        }
        //[HttpPost]
        //public async Task<IActionResult> PublishPerDocStage(PerformanceDocumentStageViewModel model)
        //{
        //    if (model.EnableReview && model.ReviewStartDate.IsNotNull() && model.ReviewStartDate.Value.Date <= DateTime.Now.Date)
        //    {

        //        ///change statsus of document master stage to publishing
        //        var result = await _pmtBusiness.ChangeStatusforDocumentMasterStage(PerformanceDocumentStatusEnum.Publishing, model);
        //        if (result.IsSuccess)
        //        {
        //            // Call Cron Job 
        //            //await _pmtBusiness.GeneratePerformanceDocumentStages(model.Id);
        //            BackgroundJob.Enqueue<HangfireScheduler>(x => x.GeneratePerformanceDocumentStages(model.Id, _userContext.ToIdentityUser()));
        //        }
        //        return Json(new { success = true });

        //    }
        //    else
        //    {
        //        // Change status to Active 
        //        var result = await _pmtBusiness.ChangeStatusforDocumentMasterStage(PerformanceDocumentStatusEnum.Active, model);
        //    }

        //    return Json(new { success = false, message = "Cannot Publish Document Master Stage" });
        //}
        public async Task<ActionResult> DeletePerDocStage(string noteId)
        {
            var result = await _pmtBusiness.DeleteDocumentStage(noteId);
            return Json(new { success = result });
        }

        public async Task<IActionResult> CreatePGR(string parentNoteId, string perRatingId, string ratingId)
        {
            var model = new PerformanceDocumentViewModel();
            if (ratingId.IsNotNullAndNotEmpty())
            {
                var perGradeRatingModel = await _pmtBusiness.GetPerformanceGradeRatingData(parentNoteId, ratingId);
                if (perGradeRatingModel.Count > 0)
                {
                    model = perGradeRatingModel.FirstOrDefault();
                }
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }
            model.ParentNoteId = parentNoteId;
            model.PerformanceRatingId = perRatingId;

            return View("ManagePerformanceGradeRating", model);
        }

        public async Task<ActionResult> ReadPerformanceGradeRatingList(string perRatingId)
        {
            var list = await _pmtBusiness.GetPerformanceGradeRatingList(perRatingId);
            return Json(list);
        }

        public async Task<ActionResult> ReadGradeList()
        {
            //var list = await _pmtBusiness.GetGradeList();
            var list = await _cmsBusiness.GetDataListByTemplate("HRGrade", "");
            return Json(list);
        }
        public async Task<IActionResult> PublishPerformanceDocument(string pdmId, string status)
        {
            var model = new PerformanceDocumentViewModel();
            model = await _pmtBusiness.GetPerformanceDocumentDetails(pdmId);
            model.DataAction = DataActionEnum.Create;
            ViewBag.Status = status;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PublishPerformanceDocument(PerformanceDocumentViewModel model)
        {
            //var result = await _pmtBusiness.GeneratePerformanceDocument(model.Id);
            if (model.DocumentStatus == PerformanceDocumentStatusEnum.Draft || model.DocumentStatus == PerformanceDocumentStatusEnum.Active)
            {
                var result = await _pmtBusiness.PublishDocumentMaster(model.NoteId);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
            }


            return Json(new { success = false, message = "Cannot Publish Document" });

        }

        public async Task<IActionResult> MapUser(string pdmId)
        {
            var model = new PerformanceDocumentViewModel();
            model.DocId = pdmId;
            return View(model);
        }

        public async Task<ActionResult> ReadMappedUserPerformanceDocumentData(string PmDoc)
        {
            var model = await _pmtBusiness.GetPerformanceDocumentMappedUserData(PmDoc, null);
            return Json(model);
        }

        public async Task<ActionResult> ReadUsers(string deptId = null)
        {
            var model = await _hRCoreBusiness.GetUsersInfo(deptId);
            return Json(model.OrderBy(x => x.PersonFullName));
        }


        [HttpPost]
        public async Task<IActionResult> MapUsers(string mapusers, string Docid)
        {
            var Users = mapusers.Split(",");
            foreach (var user in Users)
            {
                var existing = await _pmtBusiness.GetPerformanceDocumentMappedUserData(Docid, user);
                if (existing.Count == 0)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "PERFORMANCE_DOCUMENT_MASTER_USERS";
                    noteTempModel.ParentNoteId = Docid;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    notemodel.OwnerUserId = user;
                    notemodel.StartDate = DateTime.Now;
                    notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                    notemodel.Json = JsonConvert.SerializeObject(notemodel);
                    var result = await _noteBusiness.ManageNote(notemodel);
                }
                else
                {
                    return Json(new { success = false, message = "User is Already Mapped" });
                }


            }

            return Json(new { success = true });



        }

        [HttpPost]
        public async Task<IActionResult> DeleteMapUsers(string mapusers, string Docid)
        {

            var deletemapuser = await _tableMetadataBusiness.DeleteTableDataByHeaderId("PERFORMANCE_DOCUMENT_MASTER_USERS", null, Docid);
            if (deletemapuser.IsNotNull())
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }


        public IActionResult CompetencyCategory()
        {
            return View();
        }




        public async Task<IActionResult> CompatencyCategoryAdd(string Id)
        {
            var model = new CompetencyCategoryViewModel();
            if (Id.IsNotNullAndNotEmpty())
            {
                model = await _pmtBusiness.GetcompetencyCategoryDetails(Id);
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
            return View("ManageCompatencyCategory", model);
        }


        [HttpPost]
        public async Task<IActionResult> ManageCompatencyCategory(CompetencyCategoryViewModel model)
        {


            var exist = await _pmtBusiness.IsCompetencyCategoryNameExist(model.CategoryName, model.Id);
            if (exist != null)
            {
                return Json(new { success = false, error = "The given competency category name already exist" });
            }
            var exist2 = await _pmtBusiness.IsCompetencyCategoryCodeExist(model.CategoryCode, model.Id);
            if (exist2 != null)
            {
                return Json(new { success = false, error = "The given competency category code already exist" });
            }

            if (model.DataAction == DataActionEnum.Create)
            {

                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "COMPETENCY_CATEGORY";
                noteTempModel.ParentNoteId = model.ParentNoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";

                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    // if (model.TagCategoryType == TagCategoryTypeEnum.Master)
                    // {
                    //     //await _ntsTagBusiness.GenerateTagsForCategory(result.Item.NoteId);
                    //     BackgroundJob.Enqueue<HangfireScheduler>(x => x.GenerateTagsForCategory(result.Item.NoteId));
                    // }
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            else
            {

                if (model.ParentNoteId.IsNotNullAndNotEmpty())
                {
                    var exist3 = await _pmtBusiness.IsParentAssignToCompetencyCategoryExist(model.ParentNoteId, model.NoteId);
                    if (exist3 != null)
                    {
                        return Json(new { success = false, error = "The parent Competency category Name already assign to this competency" });
                    }
                }



                // var exist = await _pmtBusiness.IsDocNameExist(model.Name, model.Id);
                // if (exist != null)
                // {
                //     return Json(new { success = false, error = "The given name already exist" });
                // }
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ParentNoteId = model.ParentNoteId;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.NoteId;


                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.ParentNoteId = model.ParentNoteId;
                notemodel.Json = JsonConvert.SerializeObject(model);
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    //if (model.TagCategoryType == TagCategoryTypeEnum.Master)
                    //{
                    //    //await _ntsTagBusiness.GenerateTagsForCategory(result.Item.NoteId);
                    //    BackgroundJob.Enqueue<HangfireScheduler>(x => x.GenerateTagsForCategory(result.Item.NoteId));
                    //}
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
        }
        public async Task<ActionResult> ReadCompetencyCategoryData([DataSourceRequest] DataSourceRequest request, PerformanceDocumentViewModel search = null)
        {
            var model = await _cmsBusiness.GetDataListByTemplate("COMPETENCY_CATEGORY", "");



            return Json(model);
            // return Json(model.ToDataSourceResult(request));
        }


        public async Task<object> GetDetails(/*[DataSourceRequest] DataSourceRequest request*/string id)
        {
            var model = await _pmtBusiness.GetCompotencyDetails();

            if (id.IsNotNullAndNotEmpty())
            {
                model = model.Where(x => x.ParentTaskId == id).ToList();
            }
            else
            {
                model = model.Where(x => x.ParentTaskId == null).ToList();
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            return json;
            // return Json(model.ToTreeDataSourceResult(request));


            // var result = model.ToTreeDataSourceResult(request,
            //    e => e.NtsNoteId,
            //    e => e.ParentNoteId,
            //    e=>e

            //);

            //  return Json(result);
        }

        public async Task<ActionResult> GetParentCompetencyCategory()
        {
            var model = await _pmtBusiness.GetParentCompatencyCategory();
            return Json(model);
        }

        public async Task<JsonResult> Delete(string Id)
        {
            await _pmtBusiness.DeleteCompetencyCategory(Id);
            return Json("");
        }

        public async Task<IActionResult> CompetencyMasterJob()
        {
            var model = new CompetencyCategoryViewModel();
            model.DataAction = DataActionEnum.Create;


            return View(model);
        }

        public async Task<IActionResult> ManageCompetencyMasterJob(string id)
        {
            var model = new CompetencyCategoryViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                var exist = await _pmtBusiness.ReadCompetencyMasterJob(id);
                model = exist.FirstOrDefault();
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }



            return View(model);
        }

        public async Task<ActionResult> ReadJobs()
        {
            var model = await _hRCoreBusiness.GetAllJobs();
            return Json(model);
        }

        public async Task<ActionResult> ReadCompetencyMaster()
        {
            var model = await _pmtBusiness.GetCompetencyMaster();
            return Json(model);
        }

        public async Task<ActionResult> ReadCompetencyMasterJob()
        {
            var model = await _pmtBusiness.ReadCompetencyMasterJob(null);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageCompetencyMasterJob(CompetencyCategoryViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var masterids = model.MasterIds;
                var existing = await _pmtBusiness.ReadCompetencyMasterJob(null);
                bool anyexisting = false;
                var existinglist = new List<string>();
                foreach (var mid in masterids)
                {
                    var jobids = model.JobIds;
                    foreach (var jid in jobids)
                    {
                        var existingitem = existing.FirstOrDefault(x => x.MasterId == mid && x.JobId == jid);
                        if (existingitem.IsNotNull())
                        {
                            anyexisting = true;
                            existinglist.Add("[" + existingitem.CompetencyName + " - " + existingitem.JobTitle + "] ");
                        }
                    }
                }
                if (anyexisting == false)
                {
                    foreach (var mid in masterids)
                    {
                        var jobids = model.JobIds;
                        foreach (var jid in jobids)
                        {
                            anyexisting = true;
                            var noteTempModel = new NoteTemplateViewModel();
                            var competencymodel = new CompetencyCategoryViewModel();
                            competencymodel.MasterId = mid;
                            competencymodel.JobId = jid;
                            competencymodel.ProficiencyLevelId = model.ProficiencyLevelId;
                            noteTempModel.DataAction = DataActionEnum.Create;
                            noteTempModel.ActiveUserId = _userContext.UserId;
                            noteTempModel.TemplateCode = "COMPETENCY_MASTER_JOB";
                            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                            //notemodel.OwnerUserId = user;
                            notemodel.StartDate = DateTime.Now;
                            notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                            notemodel.Json = JsonConvert.SerializeObject(competencymodel);
                            var result = await _noteBusiness.ManageNote(notemodel);
                            if (!result.IsSuccess)
                            {
                                return Json(new { success = false, error = result.HtmlError });
                            }
                        }
                    }
                }

                else
                {
                    // ModelState.AddModelError(existinglist);
                    existinglist.Add("These Competencies and Jobs are already Mapped");
                    return Json(new { success = false, error = existinglist });
                }

                return Json(new { success = true });

            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.NoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.Json = JsonConvert.SerializeObject(model);
                var result = await _noteBusiness.ManageNote(notemodel);
                if (!result.IsSuccess)
                {
                    return Json(new { success = false, error = result.HtmlError });
                }
                return Json(new { success = true });
            }
            return Json(new { success = false, error = ModelState.ToHtmlError() });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCompetencyMasterJob(string noteid)
        {
            var deletemapuser = await _tableMetadataBusiness.DeleteTableDataByHeaderId("COMPETENCY_MASTER_JOB", null, noteid);
            if (deletemapuser.IsNotNull())
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public IActionResult CompetencyMaster(string noteId)
        {
            var model = new CompetencyViewModel();
            model.ParentNoteId = noteId;
            return View(model);
        }

        public async Task<IActionResult> ManageCompetencyMaster(string compnoteId, string compId)
        {
            var model = new CompetencyViewModel();
            if (compId.IsNotNullAndNotEmpty())
            {
                var compModel = await _pmtBusiness.GetCompetencyData(compnoteId, "", compId);
                if (compModel.Count > 0)
                {
                    model = compModel.FirstOrDefault();
                }
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }
            model.ParentNoteId = compnoteId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageCompetencyMaster(CompetencyViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var result = await _pmtBusiness.CreateComp(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = result.HtmlError });
                }
            }
            else
            {
                var result = await _pmtBusiness.EditComp(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = result.HtmlError });
                }
            }
        }

        public async Task<ActionResult> DeleteComp(string Id)
        {
            var result = await _pmtBusiness.DeleteComp(Id);
            return Json(new { success = result });
        }

        public async Task<ActionResult> ReadCompetencyData(string ParentNoteId)
        {
            //var model = await _cmsBusiness.GetDataListByTemplate("PERFORMACE_DOCUMENT_MASTER_STAGE", "");
            var model = await _pmtBusiness.GetCompetencyData(ParentNoteId);
            return Json(model);
        }


        public async Task<ActionResult> EmployeeReview(string documentMasterId, string docMasterStageId, string stageId, string userType)
        {
            ViewBag.DocMasterStageId = docMasterStageId;
            ViewBag.DocumentMasterId = documentMasterId;
            ViewBag.userType = userType;
            ViewBag.StageId = stageId;
            return View();
        }
        public async Task<ActionResult> ReadGoalServiceData(string documentStageId, string documentMasterStageId, string userType)
        {
            //List<TeamWorkloadViewModel> list1 = new List<TeamWorkloadViewModel>();
            var stage = await _pmtBusiness.GetPerformanceDocumentStage(documentStageId);
            var masterStage = await _pmtBusiness.GetPerformanceDocumentMasterStageById(documentMasterStageId);
            var goals = await _pmtBusiness.GetAllApprovedGoals(stage.OwnerUserId, stage.ParentServiceId, documentStageId);
            //if (masterStage.PerformanceStageObjective == PerformanceObjectiveStageEnum.DocumentObjective)
            //{

            //         var goals = await _pmtBusiness.GetAllApprovedGoals(stage.OwnerUserId, stage.ParentServiceId, documentStageId);                   
            //         list1.AddRange(goals);

            // }
            // else if (masterStage.PerformanceStageObjective == PerformanceObjectiveStageEnum.StageObjective)
            //     {

            //         var list = await _pmtBusiness.GetAllStageGoals(stage.OwnerUserId, stage.ParentServiceId, documentStageId, masterStage.StartDate, masterStage.EndDate);
            //         list = list.Where(x => x.StartDate <= DateTime.Now.Date).ToList();
            //         list1.AddRange(list);

            //     }

            return Json(goals);
        }
        public async Task<ActionResult> ReadCompetencyServiceData(string documentStageId, string documentMasterStageId, string userType)
        {

            //List<TeamWorkloadViewModel> list1 = new List<TeamWorkloadViewModel>();
            var stage = await _pmtBusiness.GetPerformanceDocumentStage(documentStageId);
            var masterStage = await _pmtBusiness.GetPerformanceDocumentMasterStageById(documentMasterStageId);
            var Competencies = await _pmtBusiness.GetAllApprovedCompetencies(stage.OwnerUserId, stage.ParentServiceId, documentStageId);
            //if (masterStage.PerformanceStageObjective == PerformanceObjectiveStageEnum.DocumentObjective)
            //{
            //    var Competencies = await _pmtBusiness.GetAllApprovedCompetencies(stage.OwnerUserId, stage.ParentServiceId, documentStageId);
            //    list1.AddRange(Competencies);
            //}
            //else if (masterStage.PerformanceStageObjective == PerformanceObjectiveStageEnum.StageObjective)
            //{
            //    var list = await _pmtBusiness.GetAllStageCompetencies(stage.OwnerUserId, stage.ParentServiceId, documentStageId, masterStage.StartDate, masterStage.EndDate);
            //    list = list.Where(x => x.StartDate <= DateTime.Now.Date).ToList();
            //    list1.AddRange(list);
            //}
            return Json(Competencies);


        }
        public async Task<ActionResult> GetRatingDetailsFromDocumentMaster(string DocMasterId)
        {
            var list = await _pmtBusiness.GetRatingDetailsFromDocumentMaster(DocMasterId);
            return Json(list);
        }
        //[HttpPost]
        //public async Task<ActionResult> UpdateService (TeamWorkloadViewModel model)
        //{
        //    var service = await _serviceBusiness.GetSingleById(model.Id);
        //    var noteTempModel = new NoteTemplateViewModel();           
        //    noteTempModel.NoteId = service.UdfNoteId;
        //    noteTempModel.SetUdfValue = true;
        //    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
        //    var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
        //    if (model.UserType== "PERFORMANCE_EMPLOYEE")
        //    {
        //        var EmployeeRating = rowData1.ContainsKey("EmployeeRating") ? Convert.ToString(rowData1["EmployeeRating"]) : "";
        //        if (EmployeeRating.IsNotNull())
        //        {
        //            rowData1["EmployeeRating"] = model.EmployeeRating;
        //        }
        //        var EmployeeComments = rowData1.ContainsKey("EmployeeComments") ? Convert.ToString(rowData1["EmployeeComments"]) : "";
        //        if (EmployeeComments.IsNotNull())
        //        {
        //            rowData1["EmployeeComments"] = model.EmployeeComment;
        //        }
        //    }
        //    if (model.UserType == "PERFORMANCE_MANGER")
        //    {
        //        var ManagerRating = rowData1.ContainsKey("ManagerRating") ? Convert.ToString(rowData1["ManagerRating"]) : "";
        //        if (ManagerRating.IsNotNull())
        //        {
        //            rowData1["EmployeeRating"] = model.EmployeeRating;
        //        }
        //        var ManagerComments = rowData1.ContainsKey("ManagerComments") ? Convert.ToString(rowData1["ManagerComments"]) : "";
        //        if (ManagerComments.IsNotNull())
        //        {
        //            rowData1["EmployeeComments"] = model.ManagerComment;
        //        }
        //    }

        //    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
        //    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
        //    if (update.IsSuccess)
        //    {
        //        return Json(new { success = true });
        //    }
        //    return Json(new { success=false});
        //}

        [HttpPost]
        public async Task<ActionResult> UpdateGoalReview(TeamWorkloadViewModel model)
        {
            if (model.ReviewId.IsNotNullAndNotEmpty())
            {
                // Edit
                var note = await _tableMetadataBusiness.GetTableDataByColumn("PMS_STAGE_GOAL_REVIEW", null, "Id", model.ReviewId);
                if (note.IsNotNull())
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.NoteId = Convert.ToString(note["NtsNoteId"]);
                    noteTempModel.SetUdfValue = true;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                    if (model.UserType == "PERFORMANCE_EMPLOYEE")
                    {
                        var EmployeeRating = rowData1.ContainsKey("EmployeeRating") ? Convert.ToString(rowData1["EmployeeRating"]) : "";
                        if (EmployeeRating.IsNotNull())
                        {
                            rowData1["EmployeeRating"] = model.EmployeeRating;
                        }
                        var EmployeeComments = rowData1.ContainsKey("EmployeeComment") ? Convert.ToString(rowData1["EmployeeComment"]) : "";
                        if (EmployeeComments.IsNotNull())
                        {
                            rowData1["EmployeeComment"] = model.EmployeeComment;
                        }
                    }
                    if (model.UserType == "PERFORMANCE_MANAGER")
                    {
                        var ManagerRating = rowData1.ContainsKey("ManagerRating") ? Convert.ToString(rowData1["ManagerRating"]) : "";
                        if (ManagerRating.IsNotNull())
                        {
                            rowData1["ManagerRating"] = model.ManagerRating;
                        }
                        var ManagerComments = rowData1.ContainsKey("ManagerComment") ? Convert.ToString(rowData1["ManagerComment"]) : "";
                        if (ManagerComments.IsNotNull())
                        {
                            rowData1["ManagerComment"] = model.ManagerComment;
                        }
                    }

                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                    if (update.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }
            }
            else // Create
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.TemplateCode = "PMS_STAGE_GOAL_REVIEW";
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.ActiveUserId = model.ProjectOwnerId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.OwnerUserId = model.ProjectOwnerId;
                dynamic exo = new System.Dynamic.ExpandoObject();
                if (model.StageId.IsNotNull())
                {
                    ((IDictionary<String, Object>)exo).Add("StageId", model.StageId);
                }
                ((IDictionary<String, Object>)exo).Add("GoalId", model.GoalId);
                ((IDictionary<String, Object>)exo).Add("UserId", model.ProjectOwnerId);
                if (model.UserType == "PERFORMANCE_EMPLOYEE")
                {
                    ((IDictionary<String, Object>)exo).Add("EmployeeRating", model.EmployeeRating);
                    ((IDictionary<String, Object>)exo).Add("EmployeeComment", model.EmployeeComment);
                }
                else
                {
                    ((IDictionary<String, Object>)exo).Add("ManagerRating", model.ManagerRating);
                    ((IDictionary<String, Object>)exo).Add("ManagerComment", model.ManagerComment);
                }
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }
        [HttpPost]
        public async Task<ActionResult> UpdateCompetencyReview(TeamWorkloadViewModel model)
        {
            if (model.ReviewId.IsNotNullAndNotEmpty())
            {
                // Edit
                var note = await _tableMetadataBusiness.GetTableDataByColumn("PMS_STAGE_COMP_REVIEW", null, "Id", model.ReviewId);
                if (note.IsNotNull())
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.NoteId = Convert.ToString(note["NtsNoteId"]);
                    noteTempModel.SetUdfValue = true;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                    if (model.UserType == "PERFORMANCE_EMPLOYEE")
                    {
                        var EmployeeRating = rowData1.ContainsKey("EmployeeRating") ? Convert.ToString(rowData1["EmployeeRating"]) : "";
                        if (EmployeeRating.IsNotNull())
                        {
                            rowData1["EmployeeRating"] = model.EmployeeRating;
                        }
                        var EmployeeComments = rowData1.ContainsKey("EmployeeComment") ? Convert.ToString(rowData1["EmployeeComment"]) : "";
                        if (EmployeeComments.IsNotNull())
                        {
                            rowData1["EmployeeComment"] = model.EmployeeComment;
                        }
                    }
                    if (model.UserType == "PERFORMANCE_MANAGER")
                    {
                        var ManagerRating = rowData1.ContainsKey("ManagerRating") ? Convert.ToString(rowData1["ManagerRating"]) : "";
                        if (ManagerRating.IsNotNull())
                        {
                            rowData1["ManagerRating"] = model.ManagerRating;
                        }
                        var ManagerComments = rowData1.ContainsKey("ManagerComment") ? Convert.ToString(rowData1["ManagerComment"]) : "";
                        if (ManagerComments.IsNotNull())
                        {
                            rowData1["ManagerComment"] = model.ManagerComment;
                        }
                    }

                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                    if (update.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }
            }
            else // Create
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.TemplateCode = "PMS_STAGE_COMP_REVIEW";
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.ActiveUserId = model.UserId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.OwnerUserId = model.ProjectOwnerId;
                dynamic exo = new System.Dynamic.ExpandoObject();
                if (model.StageId.IsNotNull())
                {
                    ((IDictionary<String, Object>)exo).Add("StageId", model.StageId);
                }
                ((IDictionary<String, Object>)exo).Add("CompetencyId", model.CompetencyId);
                ((IDictionary<String, Object>)exo).Add("UserId", model.ProjectOwnerId);
                if (model.UserType == "PERFORMANCE_EMPLOYEE")
                {
                    ((IDictionary<String, Object>)exo).Add("EmployeeRating", model.EmployeeRating);
                    ((IDictionary<String, Object>)exo).Add("EmployeeComment", model.EmployeeComment);
                }
                else
                {
                    ((IDictionary<String, Object>)exo).Add("ManagerRating", model.ManagerRating);
                    ((IDictionary<String, Object>)exo).Add("ManagerComment", model.ManagerComment);
                }
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }
        public async Task<ActionResult> ReadDepartmentList()
        {
            var list = await _pmtBusiness.GetDepartmentList();
            return Json(list);
        }

        public async Task<ActionResult> GetPerformanceDocumentsList()
        {
            var list = await _pmtBusiness.GetPerformanceDocumentsList();
            return Json(list);
        }

        public async Task<ActionResult> GetSubordinatesIdNameList()
        {
            var list = await _pmtBusiness.GetSubordinatesIdNameList();
            list.Insert(0, new IdNameViewModel() { Id = _userContext.UserId, Name = _userContext.Name });
            //list.Add(new IdNameViewModel() { Id = _userContext.UserId, Name = _userContext.Name });
            return Json(list);
        }

        public async Task<IActionResult> GetPerDocSubordinatesIdNameList(string performanceId)
        {

            var perDocUsers = await _pmtBusiness.GetPerDocMasMappedUserData(performanceId);

            if (_userContext.UserRoleCodes.Contains("ADMIN"))
            {
                perDocUsers.Insert(0, new IdNameViewModel() { Id = "", Name = "--All--" });
                return Json(perDocUsers);
            }
            else
            {
                IList<IdNameViewModel> userList = new List<IdNameViewModel>();
                var subordinate = await _userHierarchyBusiness.GetHierarchyUsers("PERFORMANCE_HIERARCHY", _userContext.UserId, 1, 1);
                userList = subordinate.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();

                IList<IdNameViewModel> CommonList = perDocUsers.Where(p => userList.Any(p2 => p2.Id == p.Id)).ToList();

                CommonList.Insert(0, new IdNameViewModel() { Id = _userContext.UserId, Name = _userContext.Name });

                return Json(CommonList);
            }
        }

        public IActionResult ManagePerformanceBook(string recordId)
        {
            return View(new ServiceTemplateViewModel() { ServiceId = recordId });
        }

        public IActionResult PerformanceBook(string categoryCodes)
        {
            WorklistDashboardSummaryViewModel model = new WorklistDashboardSummaryViewModel();

            ViewBag.Admin = _userContext.UserRoleCodes.Contains("ADMIN") ? true : false;

            //var NoteCount = await _noteBusiness.NotesCountForDashboard(_userContext.UserId, null);
            //if (NoteCount != null)
            //{
            //    model.N_CreatedByMeActive = NoteCount.createdByMeActive;
            //    model.N_CreatedByMeDraft = NoteCount.createdByMeDraft;
            //    model.N_CreatedByMeExpired = NoteCount.createdByMeExpired;
            //    model.N_CreatedByMeAll = model.N_CreatedByMeActive + model.N_CreatedByMeDraft + model.N_CreatedByMeExpired;
            //}
            //var TaskCount = await _taskBusiness.TaskCountForDashboard(_userContext.UserId, null);
            //if (TaskCount != null)
            //{
            //    model.T_AssignPending = TaskCount.T_AssignPending;
            //    model.T_AssignCompleted = TaskCount.T_AssignCompleted;
            //    model.T_AssignOverdue = TaskCount.T_AssignOverdue;
            //    model.T_AssignReject = TaskCount.T_AssignReject;
            //    model.T_AssignPendingOverdue = TaskCount.T_AssignPending + TaskCount.T_AssignOverdue;
            //    model.T_AssignAll = model.T_AssignPendingOverdue + model.T_AssignCompleted + model.T_AssignReject;
            //}
            //var notification = await _taskBusiness.NotificationDashboardIndex(_userContext.UserId, null);
            //if (notification != null)
            //{
            //    model.ReadCount = notification.Where(x => x.ReadStatus == ReadStatusEnum.Read).Count();
            //    model.UnReadCount = notification.Where(x => x.ReadStatus == ReadStatusEnum.NotRead).Count();
            //    model.ArchivedCount = notification.Where(x => x.IsArchived == true).Count();
            //    model.AllCount = model.ReadCount + model.UnReadCount + model.ArchivedCount;
            //}

            return View(model);
        }

        public async Task<IActionResult> LoadBook(string id, string userId = null, string docId = null)
        {
            //var data = await _taskBusiness.LoadWorkBooks(_userContext.UserId, statusFilter);

            var userIds = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                userIds = userId;
            }
            else
            {
                if (!_userContext.UserRoleCodes.Contains("ADMIN"))
                {
                    //var list = await _pmtBusiness.GetSubordinatesIdNameList();
                    //userIds = string.Join("','", list.Select(x => x.Id).ToArray());
                    //userIds = string.Concat(userIds, "','" + _userContext.UserId);
                    userIds = _userContext.UserId;
                }
            }

            var documents = await _pmtBusiness.LoadWorkBooks(userIds, docId);

            if (id.IsNotNullAndNotEmpty())
            {
                documents = documents.Where(x => x.ParentServiceId == id).ToList();
            }
            else
            {
                documents = documents.Where(x => x.ParentServiceId == null).ToList();
            }
            return Json(documents);

        }

        public async Task<IActionResult> PerformanceBookHTML(string recordId)
        {
            var model = await _pmtBusiness.GetBookDetails(recordId);
            model.PortalId = _userContext.PortalId;
            return View(model);
        }

        public async Task<IActionResult> WorkBookCount(string bookId)
        {
            WorklistDashboardSummaryViewModel model = new WorklistDashboardSummaryViewModel();
            //var NoteCount = await _noteBusiness.NotesCountForDashboard(_userContext.UserId, bookId);
            //if (NoteCount != null)
            //{
            //    model.N_CreatedByMeActive = NoteCount.createdByMeActive;
            //    model.N_CreatedByMeDraft = NoteCount.createdByMeDraft;
            //    model.N_CreatedByMeExpired = NoteCount.createdByMeExpired;

            //}
            //var TaskCount = await _taskBusiness.TaskCountForDashboard(_userContext.UserId, bookId);

            //if (TaskCount != null)
            //{
            //    model.T_AssignPlanned = TaskCount.T_AssignPlanned;
            //    model.T_AssignPlannedOverdue = TaskCount.T_AssignPlannedOverdue;
            //    model.T_AssignPending = TaskCount.T_AssignPending;
            //    model.T_AssignCompleted = TaskCount.T_AssignCompleted;
            //    model.T_AssignOverdue = TaskCount.T_AssignOverdue;
            //    model.T_AssignReject = TaskCount.T_AssignReject;
            //}        


            var books = await _pmtBusiness.GetBookList(bookId, null, true);
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

            var notification = await _pmtBusiness.GetNotificationsList(refIds);
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
