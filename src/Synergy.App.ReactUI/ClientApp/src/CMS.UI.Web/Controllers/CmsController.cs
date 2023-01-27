using AutoMapper;
using Synergy.App.WebUtility;
using FastReport.Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
//using FastReport.Utils;
//using FastReport.Data;
//using FastReport.Web;

namespace CMS.UI.Web.Controllers
{
    public class CmsController : ApplicationController
    {
        private readonly IPageBusiness _pageBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IFileBusiness _fileBusiness;
        private readonly IUserContext _userContext;
        private readonly IUserBusiness _userBusiness;
        private readonly INoteIndexPageTemplateBusiness _noteIndexPageTemplateBusiness;
        private readonly ITaskIndexPageTemplateBusiness _taskIndexPageTemplateBusiness;
        private readonly IServiceIndexPageTemplateBusiness _serviceIndexPageTemplateBusiness;
        private readonly ITeamBusiness _teamBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly INtsTaskSharedBusiness _ntsTaskSharedBusiness;
        private readonly INtsNoteSharedBusiness _ntsNoteSharedBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INtsServiceSharedBusiness _ntsServiceSharedBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private IPushNotificationBusiness _pushNotificationBusiness;
        private INotificationBusiness _notificationBusiness;
        private INtsTaskCommentBusiness _ntsTaskCommentBusiness;
        private INtsServiceCommentBusiness _ntsServiceCommentBusiness;
        private INtsNoteCommentBusiness _ntsNoteCommentBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly INtsTaskTimeEntryBusiness _ntsTaskTimeEntryBusiness;
        private readonly INtsTaskPrecedenceBusiness _ntsTaskPrecedenceBusiness;
        private readonly IComponentResultBusiness _componentResultBusiness;
        private readonly IWebHelper _webApi;
        private IMapper _autoMapper;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly INoteTemplateBusiness _noteTemplateBusiness;
        private readonly IEmailBusiness _emailBusiness;
        private readonly IProjectEmailSetupBusiness _projectEmailBusiness;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly IServiceProvider _sp;
        public CmsController(
            IPageBusiness pageBusiness
            , IPortalBusiness portalBusiness
            , IRazorViewEngine razorViewEngine
            , IHttpContextAccessor contextAccessor
            , ITempDataProvider tempDataProvider
            , IFileBusiness fileBusiness
            , ICmsBusiness cmsBusiness
            , IUserContext userContext
            , IMapper autoMapper
            , IUserBusiness userBusiness,
            INoteIndexPageTemplateBusiness noteIndexPageTemplateBusiness
            , ITaskIndexPageTemplateBusiness taskIndexPageTemplateBusiness
            , IServiceIndexPageTemplateBusiness serviceIndexPageTemplateBusiness
             , ITeamBusiness teamBusiness
            , ITaskBusiness taskBusiness
            , IServiceBusiness serviceBusiness
            , INtsServiceSharedBusiness ntsServiceSharedBusiness,
            INtsTaskSharedBusiness ntsTaskSharedBusiness,
            ILOVBusiness lovBusiness,
            IPushNotificationBusiness pushNotificationBusiness,
            INtsTaskCommentBusiness ntsTaskCommentBusiness
            , INtsServiceCommentBusiness ntsServiceCommentBusiness
            , INoteBusiness noteBusiness
            , IEmailBusiness emailBusiness
            , INtsNoteSharedBusiness ntsNoteSharedBusiness
            , INtsNoteCommentBusiness ntsNoteCommentBusiness
            , AuthSignInManager<ApplicationIdentityUser> customUserManager
            , INtsTaskTimeEntryBusiness ntsTaskTimeEntryBusiness
            , INtsTaskPrecedenceBusiness ntsTaskPrecedenceBusiness
            , IComponentResultBusiness componentResultBusiness
            , IWebHelper webApi
            , ITemplateBusiness templateBusiness
            , INoteTemplateBusiness noteTemplateBusiness
            , IProjectEmailSetupBusiness projectEmailBusiness
            , Microsoft.Extensions.Configuration.IConfiguration configuration
             , INotificationBusiness notificationBusiness
             , IServiceProvider sp
          )
        {
            _sp = sp;
            _pageBusiness = pageBusiness;
            _autoMapper = autoMapper;
            _portalBusiness = portalBusiness;
            _razorViewEngine = razorViewEngine;
            _contextAccessor = contextAccessor;
            _tempDataProvider = tempDataProvider;
            _cmsBusiness = cmsBusiness;
            _fileBusiness = fileBusiness;
            _userContext = userContext;
            _userBusiness = userBusiness;
            _customUserManager = customUserManager;
            _noteIndexPageTemplateBusiness = noteIndexPageTemplateBusiness;
            _taskIndexPageTemplateBusiness = taskIndexPageTemplateBusiness;
            _serviceIndexPageTemplateBusiness = serviceIndexPageTemplateBusiness;
            _teamBusiness = teamBusiness;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
            _ntsTaskSharedBusiness = ntsTaskSharedBusiness;
            _lovBusiness = lovBusiness;
            _pushNotificationBusiness = pushNotificationBusiness;
            _notificationBusiness = notificationBusiness;
            _ntsTaskCommentBusiness = ntsTaskCommentBusiness;
            _ntsServiceCommentBusiness = ntsServiceCommentBusiness;
            _ntsServiceSharedBusiness = ntsServiceSharedBusiness;
            _noteBusiness = noteBusiness;
            _ntsNoteSharedBusiness = ntsNoteSharedBusiness;
            _ntsNoteCommentBusiness = ntsNoteCommentBusiness;
            _ntsTaskTimeEntryBusiness = ntsTaskTimeEntryBusiness;
            _ntsTaskPrecedenceBusiness = ntsTaskPrecedenceBusiness;
            _componentResultBusiness = componentResultBusiness;
            _webApi = webApi;
            _templateBusiness = templateBusiness;
            _noteTemplateBusiness = noteTemplateBusiness;
            _emailBusiness = emailBusiness;
            _projectEmailBusiness = projectEmailBusiness;
            _configuration = configuration;
        }

        //public IActionResult Service()
        //{
        //    var model = new ServiceTemplateViewModel { };
        //    return View("Service", model);
        //}

        [Route("portal/{portal?}/{pageName?}/{id?}")]
        public async Task<IActionResult> Index(string portal, string pageName, string pageId, string pageUrl, string customUrl, string prms, string templateCodes, string categoryCodes)
        {
            if (pageUrl.IsNotNullAndNotEmpty())
            {
                pageUrl = HttpUtility.HtmlDecode(pageUrl);
            }
            if (customUrl.IsNotNullAndNotEmpty())
            {
                customUrl = HttpUtility.HtmlDecode(customUrl);
            }
            var runningMode = RunningModeEnum.Preview;
            var requestSource = RequestSourceEnum.Main;

            var result = await LoadCms(portal, pageName, pageId, runningMode, requestSource, pageUrl, customUrl, prms, templateCodes, categoryCodes);
            if (result != null)
            {
                return result;
            }

            return RedirectToAction("index", "content", new { @area = "cms" });
        }

        #region Temporary actions to render UI

        public IActionResult AddTaskAttachment(string taskId, string dataAction, bool IsAddAttachmentEnabled)
        {
            var model = new FileViewModel();
            model.DataAction = dataAction.ToEnum<DataActionEnum>();
            model.ReferenceTypeId = taskId;
            ViewBag.IsAddAttachmentEnabled = IsAddAttachmentEnabled;
            model.ReferenceTypeCode = ReferenceTypeEnum.NTS_Task;

            return View("_NtsAttachment", model);
        }
        public IActionResult AddTaskTimeEntry(string taskId, string assignTo, string dataAction)
        {
            var model = new TaskTimeEntryViewModel();
            model.DataAction = DataActionEnum.Create;
            model.NtsTaskId = taskId;
            model.UserId = assignTo;
            model.StartDate = System.DateTime.Now;
            model.EndDate = model.StartDate.AddDays(1);
            model.Duration = (model.EndDate - model.StartDate);
            return View("_NtsTaskTimeEntry", model);
        }
        public IActionResult AddPredecessor(string taskId)
        {
            var model = new NtsTaskPrecedenceViewModel();
            model.DataAction = DataActionEnum.Create;
            model.NtsTaskId = taskId;
            return View("_NtsTaskPredecessor", model);
        }
        public IActionResult AddServiceAttachment(string serviceId, string dataAction, bool IsAddAttachmentEnabled)
        {
            var model = new FileViewModel();
            model.DataAction = dataAction.ToEnum<DataActionEnum>();
            model.ReferenceTypeId = serviceId;
            ViewBag.IsAddAttachmentEnabled = IsAddAttachmentEnabled;
            model.ReferenceTypeCode = ReferenceTypeEnum.NTS_Service;

            return View("_NtsServiceAttachment", model);
        }
        public IActionResult AddNoteAttachment(string noteId, string dataAction, bool IsAddAttachmentEnabled)
        {
            var model = new FileViewModel();
            model.DataAction = dataAction.ToEnum<DataActionEnum>();
            model.ReferenceTypeId = noteId;
            ViewBag.IsAddAttachmentEnabled = IsAddAttachmentEnabled;
            model.ReferenceTypeCode = ReferenceTypeEnum.NTS_Note;

            return View("_NtsAttachment", model);
        }
        public async Task<IActionResult> ViewNotification(string taskId)
        {
            //var model = new NotificationSearchViewModel();

            var userId = _userContext.UserId;
            NotificationSearchViewModel listModel = new NotificationSearchViewModel
            {
                Notifications = await _pushNotificationBusiness.GetTaskNotificationList(taskId, userId, 0)
            };
            return View("Notifications", listModel);
        }

        public async Task<IActionResult> NotificationDetails(string notificationId)
        {
            var notification = await _notificationBusiness.GetNotificationDetails(notificationId);
            return View("_NotificationDetails", notification);
        }
        public IActionResult Notifications()
        {
            ViewBag.PortalId = _userContext.PortalId;
            return View("_Notifications");
        }

        public async Task<IActionResult> ReadNotificationList()
        {
            var notifications = await _notificationBusiness.GetAllNotifications(_userContext.UserId, _userContext.PortalId);
            return Json(notifications);
        }
        public async Task<IActionResult> ViewServiceNotification(string serviceId)
        {
            //var model = new NotificationSearchViewModel();

            var userId = _userContext.UserId;
            NotificationSearchViewModel listModel = new NotificationSearchViewModel
            {
                Notifications = await _pushNotificationBusiness.GetServiceNotificationList(serviceId, userId, 0)
            };
            return View("Notifications", listModel);
        }
        public IActionResult TaskComments(string taskId, bool IsAddCommentEnabled)
        {
            var model = new NtsTaskCommentViewModel();
            model.NtsTaskId = taskId;
            model.IsAddCommentEnabled = IsAddCommentEnabled;
            return View("_NtsTaskComments", model);
        }

        public async Task<IActionResult> ReplyTaskComments(string parentId)
        {
            var model = new NtsTaskCommentViewModel();
            var parentModel = await _ntsTaskCommentBusiness.GetSingleById(parentId);
            if (parentModel.IsNotNull())
            {
                model.NtsTaskId = parentModel.NtsTaskId;
                model.ParentCommentId = parentModel.Id;
                model.ParentMessage = parentModel.Comment;
                model.CommentedFromUserId = parentModel.CommentedByUserId;
            }
            ViewBag.Mode = "Reply";
            return View("_NtsTaskComments", model);
        }

        public async Task<ActionResult> ReadTaskCommentData([DataSourceRequest] DataSourceRequest request, string id, string taskId)
        {
            var model = await _ntsTaskCommentBusiness.GetCommentTree(taskId, id);

            return Json(model);
        }

        public async Task<ActionResult> ReadTaskCommentDataList(string taskId)
        {
            //var model = await _ntsNoteCommentBusiness.GetCommentTree(noteId, id);
            var model = await _ntsTaskCommentBusiness.GetAllCommentTree(taskId);
            return Json(model);
        }
        public async Task<ActionResult> ReadTaskTimeEntriesData(string taskId)
        {
            var model = await _ntsTaskTimeEntryBusiness.GetSearchResult(taskId);
            return Json(model);
        }
        public async Task<ActionResult> GetServiceDocumentId(string serviceId)
        {
            var docId = await _noteBusiness.GetServiceDocumentId(serviceId);
            return Json(docId);
        }
        public IActionResult ServiceComments(string serviceId, bool IsAddCommentEnabled)
        {
            var model = new NtsServiceCommentViewModel();
            model.NtsServiceId = serviceId;
            model.IsAddCommentEnabled = IsAddCommentEnabled;
            return View("_NtsServiceComments", model);
        }

        public async Task<IActionResult> ReplyServiceComments(string parentId)
        {
            var model = new NtsServiceCommentViewModel();
            var parentModel = await _ntsServiceCommentBusiness.GetSingleById(parentId);
            if (parentModel.IsNotNull())
            {
                model.NtsServiceId = parentModel.NtsServiceId;
                model.ParentCommentId = parentModel.Id;
                model.ParentMessage = parentModel.Comment;
                model.CommentedFromUserId = parentModel.CommentedByUserId;
            }
            ViewBag.Mode = "Reply";
            return View("_NtsServiceComments", model);
        }

        public IActionResult NoteComments(string noteId, bool IsAddCommentEnabled)
        {
            var model = new NtsNoteCommentViewModel();
            model.NtsNoteId = noteId;
            model.IsAddCommentEnabled = IsAddCommentEnabled;
            return View("_NtsNoteComments", model);
        }

        public async Task<IActionResult> ReplyNoteComments(string parentId)
        {
            var model = new NtsNoteCommentViewModel();
            var parentModel = await _ntsNoteCommentBusiness.GetSingleById(parentId);
            if (parentModel.IsNotNull())
            {
                model.NtsNoteId = parentModel.NtsNoteId;
                model.ParentCommentId = parentModel.Id;
                model.ParentMessage = parentModel.Comment;
                model.CommentedFromUserId = parentModel.CommentedByUserId;
            }
            ViewBag.Mode = "Reply";
            return View("_NtsNoteComments", model);
        }

        public IActionResult InlineComment(string noteId)
        {
            var model = new NtsNoteCommentViewModel();
            model.NtsNoteId = noteId;
            return View("_InlineComment", model);
        }
        public async Task<ActionResult> ReadNoteInlineCommentData([DataSourceRequest] DataSourceRequest request, string NoteId)
        {
            var model = await _noteBusiness.GetInlineCommentResult(NoteId);
            return Json(model.ToDataSourceResult(request));
        }

        public async Task<ActionResult> ReadServiceCommentData([DataSourceRequest] DataSourceRequest request, string id, string serviceId)
        {
            var model = await _ntsServiceCommentBusiness.GetCommentTree(serviceId, id);

            return Json(model);
        }

        public async Task<ActionResult> ReadServiceCommentDataList(string serviceId)
        {
            var model = await _ntsServiceCommentBusiness.GetAllCommentTree(serviceId);

            return Json(model);
        }
        public async Task<ActionResult> ReadNoteCommentData(string id, string noteId)
        {
            var model = await _ntsNoteCommentBusiness.GetCommentTree(noteId, id);
            return Json(model);
        }

        public async Task<ActionResult> ReadNoteCommentDataList(string noteId)
        {
            //var model = await _ntsNoteCommentBusiness.GetCommentTree(noteId, id);
            var model = await _ntsNoteCommentBusiness.GetAllCommentTree(noteId);
            return Json(model);
        }
        public async Task<ActionResult> GetTaskAttachmentCount(string taskId)
        {
            var list = await _fileBusiness.GetList(x => x.ReferenceTypeId == taskId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task);

            return Json(list.Count());
        }
        public async Task<ActionResult> GetServiceAttachmentCount(string serviceId)
        {
            var list = await _fileBusiness.GetList(x => x.ReferenceTypeId == serviceId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Service);

            return Json(list.Count());
        }
        public async Task<ActionResult> GetNoteAttachmentCount(string noteId)
        {
            var list = await _fileBusiness.GetList(x => x.ReferenceTypeId == noteId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Note);

            return Json(list.Count());
        }
        public async Task<ActionResult> GetTaskNotificationCount(string taskId)
        {
            var list = await _pushNotificationBusiness.GetTaskNotificationList(taskId, _userContext.UserId, 0);

            return Json(list.Count());
        }
        public async Task<ActionResult> GetNoteNotificationCount(string noteId)
        {
            var list = await _pushNotificationBusiness.GetNoteNotificationList(noteId, _userContext.UserId, 0);

            return Json(list.Count());
        }
        public async Task<ActionResult> GetServiceNotificationCount(string serviceId)
        {
            var list = await _pushNotificationBusiness.GetServiceNotificationList(serviceId, _userContext.UserId, 0);

            return Json(list.Count());
        }

        public async Task<ActionResult> GetTaskSharedCount(string taskId)
        {
            var list = await _ntsTaskSharedBusiness.GetSearchResult(taskId);

            return Json(list.Count());
        }
        public async Task<ActionResult> GetNoteSharedCount(string noteId)
        {
            var list = await _ntsNoteSharedBusiness.GetSearchResult(noteId);

            return Json(list.Count());
        }
        public async Task<ActionResult> GetServiceSharedCount(string serviceId)
        {
            var list = await _ntsServiceSharedBusiness.GetSearchResult(serviceId);

            return Json(list.Count());
        }
        public async Task<ActionResult> GetAttachmentList([DataSourceRequest] DataSourceRequest request, string Id, ReferenceTypeEnum Code)
        {
            var list = await _fileBusiness.GetList(x => x.ReferenceTypeId == Id && x.ReferenceTypeCode == Code);

            return Json(list/*.ToDataSourceResult(request)*/);
        }

        public async Task<ActionResult> GetNtsAttachmentList(string Id, ReferenceTypeEnum Code)
        {
            var list = await _fileBusiness.GetList(x => x.ReferenceTypeId == Id && x.ReferenceTypeCode == Code);

            return Json(list);
        }

        public async Task<ActionResult> GetTaskAttachmentList(/*[DataSourceRequest] DataSourceRequest request, */string taskId)
        {
            var list = await _fileBusiness.GetList(x => x.ReferenceTypeId == taskId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task);

            return Json(list/*.ToDataSourceResult(request)*/);
        }
        public async Task<ActionResult> GetNoteAttachmentList(/*[DataSourceRequest] DataSourceRequest request,*/ string noteId)
        {
            var list = await _fileBusiness.GetList(x => x.ReferenceTypeId == noteId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Note);

            return Json(list/*.ToDataSourceResult(request)*/);
        }
        public async Task<ActionResult> GetServiceAttachmentList(/*[DataSourceRequest] DataSourceRequest request,*/ string serviceId)
        {
            var list = await _fileBusiness.GetList(x => x.ReferenceTypeId == serviceId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Service);

            return Json(list/*.ToDataSourceResult(request)*/);
        }
        [HttpPost]
        public async Task<IActionResult> SaveServiceAttachment(IList<IFormFile> file, string referenceTypeId, ReferenceTypeEnum referenceTypeCode)
        {
            try
            {
                foreach (var f in file)
                {
                    var ms = new MemoryStream();
                    f.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = f.ContentType,
                        ContentLength = f.Length,
                        FileName = f.FileName,
                        ReferenceTypeId = referenceTypeId,
                        ReferenceTypeCode = referenceTypeCode,
                        FileExtension = Path.GetExtension(f.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }



                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        public async Task<ActionResult> Import(string fileId)
        {
            //if (fileId == 0)
            //    return null;
            var xsx = await _fileBusiness.GetFileByte(fileId);
            Stream stream = new MemoryStream(xsx);
            stream.Position = 0;

            // WordDocument document = WordDocument.Load(stream, GetFormatType(".docx"));
            WordDocument document = new WordDocument();
            document.Open(stream, FormatType.Docx);

            string sfdt = Newtonsoft.Json.JsonConvert.SerializeObject(document);
            document.Dispose();
            return Content(sfdt, "text/html");
        }


        [HttpPost]
        public async Task<IActionResult> SaveTaskAttachment(IList<IFormFile> file, string referenceTypeId, ReferenceTypeEnum referenceTypeCode)
        {
            try
            {
                foreach (var f in file)
                {
                    var ms = new MemoryStream();
                    f.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = f.ContentType,
                        ContentLength = f.Length,
                        FileName = f.FileName,
                        ReferenceTypeId = referenceTypeId,
                        ReferenceTypeCode = referenceTypeCode,
                        FileExtension = Path.GetExtension(f.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }



                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        public async Task<ActionResult> ViewAttachment(string fileId, bool canEdit = false)
        {
            var file = await _fileBusiness.GetSingleById(fileId);
            var doc = await _fileBusiness.GetFileByte(fileId);
            if (doc != null)
            {
                file.ContentByte = doc;
                file.ContentBase64 = Convert.ToBase64String(doc, 0, doc.Length);
            }
            //if (Helper.Is2JpegSupportable(file.FileExtension))
            //{
            //    file.FileSnapshotIds = _fileBusiness.GetFileSnapshotIdList(fileId);
            //}

            ViewBag.CanEdit = canEdit;
            return View("_NtsViewAttachment", file);
        }
        public async Task<IActionResult> NotAuthorizedPortal(string returnUrl = null)
        {
            ViewBag.Layout = $"~/Views/Shared/_EmptyLayout.cshtml";
            return View();
        }
        public async Task<IActionResult> MultiplePortalNotAllowed(string returnUrl = null)
        {
            var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.PortalDisplayName = portal.DisplayName;
            }
            ViewBag.Layout = $"~/Views/Shared/_EmptyLayout.cshtml";
            return View();
        }
        public async Task<IActionResult> DeleteAttachment(string Id)
        {
            await _fileBusiness.Delete(Id);
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTaskComment(string Id)
        {
            await _ntsTaskCommentBusiness.Delete(Id);
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteNoteComment(string Id)
        {
            await _ntsNoteCommentBusiness.Delete(Id);
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteServiceComment(string Id)
        {
            await _ntsServiceCommentBusiness.Delete(Id);
            return Json(true);
        }

        public IActionResult Note()
        {
            return View();
        }
        public async Task<IActionResult> NoteIndexPage(string pageId)
        {
            var page = await _pageBusiness.GetPageForExecution(pageId);
            var noteIndex = await _noteIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == page.TemplateId);
            noteIndex.SelectedTableRows = await _noteIndexPageTemplateBusiness.GetList<NoteIndexPageColumnViewModel, NoteIndexPageColumn>(x => x.NoteIndexPageTemplateId == noteIndex.Id);
            //var model = new NoteIndexPageTemplateViewModel
            //{
            //    Page = page,
            //    SelectedTableRows= noteIndex.
            //};
            return await Task.FromResult(View(noteIndex));
        }
        public async Task<IActionResult> ServiceIndexPage(string pageId)
        {
            var page = await _pageBusiness.GetPageForExecution(pageId);
            var serviceIndex = await _serviceIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == page.TemplateId);
            serviceIndex.SelectedTableRows = await _serviceIndexPageTemplateBusiness.GetList<ServiceIndexPageColumnViewModel, ServiceIndexPageColumn>(x => x.ServiceIndexPageTemplateId == serviceIndex.Id, y => y.ColumnMetadata);
            //var model = new ServiceIndexPageTemplateViewModel
            //{
            //    Page = new PageViewModel(),

            //};
            //return await Task.FromResult(View(model));
            return await Task.FromResult(View(serviceIndex));
        }
        public async Task<IActionResult> TaskIndexPage(string pageId)
        {
            var page = await _pageBusiness.GetPageForExecution(pageId);
            var taskIndex = await _taskIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == page.TemplateId);
            taskIndex.SelectedTableRows = await _taskIndexPageTemplateBusiness.GetList<TaskIndexPageColumnViewModel, TaskIndexPageColumn>(x => x.TaskIndexPageTemplateId == taskIndex.Id);
            //var model = new TaskIndexPageTemplateViewModel
            //{
            //    Page = new PageViewModel(),

            //};
            //return await Task.FromResult(View(model));
            return await Task.FromResult(View(taskIndex));
        }
        #endregion
        public async Task<IActionResult> Page(string pageId, string pageType, string source, string dataAction, string recordId, string customUrl, string portalId, string pageName, string prms, string udfs, string roudfs, bool popup, string ru, LayoutModeEnum lo, string cbm, string templateCodes, string categoryCodes, string iframe, string keywords, string logId, NtsViewTypeEnum? viewType, NtsViewTypeEnum? viewMode, string attachments)
        {
            //udfs = "NtsServiceId=38348630-3251-427f-b3c2-704dddf70e3d";
            var isSwitchProfile = Convert.ToBoolean(TempData["IsSwitchProfile"]);
            if (recordId == "undefined" || recordId == "null")
            {
                recordId = null;
            }

            if (customUrl.IsNotNullAndNotEmpty())
            {
                customUrl = HttpUtility.HtmlDecode(customUrl);
            }
            PageViewModel page = null;
            if (pageType == TemplateTypeEnum.MenuGroup.ToString())
            {
                page = new PageViewModel { Id = pageId, MenuGroupId = pageId };
                page.Portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(portalId);
                page.RecordId = recordId;
                page.CustomUrl = customUrl;
                page.Breadcrumbs = await LoadBreadcrumbs(page);
                var menus0 = await _portalBusiness.GetMenuItems(page.Portal, _userContext.UserId, _userContext.CompanyId);
                page.Menus = ViewBag.Menus = menus0.Where(x => x.IsHidden == false && x.DontShowInMainMenu == false).ToList();
                page.ReturnUrl = ru;
                page.LayoutMode = lo;
                ViewBag.LayoutMode = lo;
                ViewBag.PageId = page.Id;
                ViewBag.PageName = page.Name;
                ViewBag.PortalId = page.Portal.Id;
                ViewBag.FavIconId = page.Portal.FavIconId;
                ViewBag.IsPopup = popup;
                ViewBag.IframeName = iframe;
                ViewBag.EnableMultiLanguage = page.Portal.EnableMultiLanguage;
                page.PopupCallbackMethod = cbm;
                page.TemplateCodes = templateCodes;
                return await GetMenuGroup(page);
            }
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                page = await _pageBusiness.GetDefaultPageByTemplate(portalId, templateCodes);
            }
            if (page == null && pageId.IsNotNullAndNotEmpty())
            {
                page = await _pageBusiness.GetPageForExecution(pageId);
            }
            if (page == null)
            {
                page = await _pageBusiness.GetPageForExecution(portalId, pageName);
            }
            if (page == null)
            {
                return Json(new { view = "<h4>Page Not Found</h4>", uiJson = "{}", dataJson = "{}" });
            }

            if (page.Layout.IsNullOrEmpty())
            {
                if (page.Portal.Layout.IsNotNullAndNotEmpty())
                {
                    page.Layout = page.Portal.Layout;
                }
                else
                {
                    page.Layout = "_Layout";
                }

            }

            if (page.AuthorizationNotRequired)
            {
                var hasGuestUser = await ManageGuestUser(page);
                if (!hasGuestUser.Item1)
                {
                    return Json(new { view = "<h4>No Guest User Found</h4>", uiJson = "{}", dataJson = "{}" });
                }
                await ChangeContext(page);
            }
            else
            {

                var returnUrl = $@"/Portal/{page.Portal.Name}?pageurl={HttpUtility.UrlEncode(Request.QueryString.Value.TrimStart('?').TrimStart('&'))}";
                if (Request.HttpContext.User.Identity.IsAuthenticated == false || _userContext.IsGuestUser)
                {
                    var html = @$"<div class='p-3'>
                    <h3 class='text-danger font-weight-bold'>Not Authenticated</h3>
                    <h5 class='text-danger'>You should be authenticated (sign in) in order to access this page/resource.</h5>
                    </div>";

                    var url = Url.Action("Login", "Account", new { portalId = page.Portal.Id, returnUrl = returnUrl });
                    return Json(new { viewContent = "<h4>Not authenticated</h4>", uiJson = "{}", dataJson = "{}", login = url });
                    // return RedirectToAction("Login", "Account", new { portalId = page.Portal.Id, returnUrl = Request.Path });
                }
                var user = await _userBusiness.GetSingleById(_userContext.UserId);
                if (user != null && user.PasswordChanged == false)
                {
                    var url = Url.Action("ChangePasswordPage", "Account", new { portalId = page.Portal.Id, returnUrl = returnUrl });
                    return Json(new { viewContent = "<h4>Change Password</h4>", uiJson = "{}", dataJson = "{}", login = url });
                }

                var userPortals = _userContext.UserPortals.Split(",");
                if (!userPortals.Any(x => x == _userContext.PortalName))
                {
                    if (lo == LayoutModeEnum.Popup)
                    {
                        ViewBag.Layout = $"~/Views/Shared/_EmptyLayout.cshtml";
                        return View("NotAuthorizedPortal");
                    }
                    else
                    {
                        // return Json(new { viewContent = "", uiJson = "{}", dataJson = "{}", login = $@"/Portal/{page.Portal.Name}" });
                        var html = @$"<div class='p-3'>
                        <h3 class='text-danger font-weight-bold'>Not Authorized</h3>
                        <h5 class='text-danger'>You are not authorized to access this application/portal.</h5>
                        </div>";
                        return Json(new { viewContent = html, uiJson = "{}", dataJson = "{}", login = $"/cms/NotAuthorizedPortal" });
                    }
                }
                if (_userContext.PortalId != null && page.PortalId != _userContext.PortalId)
                {
                    if (lo == LayoutModeEnum.Popup)
                    {
                        ViewBag.Layout = $"~/Views/Shared/_EmptyLayout.cshtml";
                        return View("MultiplePortalNotAllowed");
                    }
                    else
                    {
                        // return Json(new { viewContent = "", uiJson = "{}", dataJson = "{}", login = $@"/Portal/{page.Portal.Name}" });
                        var html = @$"<div class='p-3'>
                        <h3 class='text-danger font-weight-bold'>Multiple Application/Portal access not allowed in same session</h3>
                        <h5 class='text-danger'>You have aleady opened an application/portal in current session. You cannot open multiple application/portal in the same session. But you can use switch application/portal option under profile menu</h5>
                        </div>";
                        return Json(new { viewContent = html, uiJson = "{}", dataJson = "{}", login = $"/cms/NotAuthorizedPortal" });
                    }
                }
                var permissions = await _pageBusiness.GetUserPagePermission(page.Portal.Id, page.Id);


                if ((permissions != null && permissions.IsAuthorized) || user.IsSystemAdmin)
                {
                    page.Permissions = permissions.Permissions;
                    if (permissions.Permissions != null)
                    {
                        page.PermissionsText = string.Join(",", permissions.Permissions);
                    }
                    await ChangeContext(page);
                    // page.PermissionsText = string.Join(",", permissions.Permissions);
                }
                else
                {
                    if (isSwitchProfile)
                    {
                        return Json(new { viewContent = "", uiJson = "{}", dataJson = "{}", login = $@"/Portal/{page.Portal.Name}" });
                    }

                    if (lo == LayoutModeEnum.Popup)
                    {
                        ViewBag.Layout = $"~/Views/Shared/Themes/{page.Portal.Theme.ToString()}/_Layout.cshtml";
                        return View("NotAuthorized");
                    }
                    else
                    {
                        var html = @$"<div class='p-3'>
                    <h3 class='text-danger font-weight-bold'>Not Authorized</h3>
                    <h5 class='text-danger'>You are not authorized to access this page/resource.</h5>
                    </div>";
                        return Json(new { viewContent = html, uiJson = "{}", dataJson = "{}" });
                    }

                }
            }
            if (source.IsNullOrEmpty())
            {
                page.RequestSource = RequestSourceEnum.Main;
            }
            else
            {
                page.RequestSource = source.ToEnum<RequestSourceEnum>();
            }
            if (dataAction.IsNullOrEmpty())
            {
                page.DataAction = DataActionEnum.None;
            }
            else
            {
                page.DataAction = dataAction.ToEnum<DataActionEnum>();
            }
            page.RecordId = recordId;
            page.CustomUrl = customUrl;
            page.Breadcrumbs = await LoadBreadcrumbs(page);
            var menus = await _portalBusiness.GetMenuItems(page.Portal, _userContext.UserId, _userContext.CompanyId);
            page.Menus = ViewBag.Menus = menus.Where(x => x.IsHidden == false && x.DontShowInMainMenu == false).ToList();
            page.ReturnUrl = ru;
            page.LayoutMode = lo;
            ViewBag.LayoutMode = lo;
            ViewBag.PageId = page.Id;
            ViewBag.PortalId = page.Portal.Id;
            ViewBag.IsPopup = popup;
            ViewBag.IframeName = iframe;
            ViewBag.EnableMultiLanguage = page.Portal.EnableMultiLanguage;
            page.PopupCallbackMethod = cbm;
            page.TemplateCodes = templateCodes;
            page.ViewMode = viewMode;

            switch (page.PageType)
            {

                case TemplateTypeEnum.Form:
                    // var formvm = new FormTemplateViewModel();                    
                    var formvm = await _cmsBusiness.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == page.TemplateId, x => x.Template);
                    formvm.Json = formvm.Template.Json;
                    formvm.Page = page;
                    formvm.Prms = Helper.QueryStringToDictionary(prms);
                    formvm.TemplateId = page.TemplateId;
                    formvm.RecordId = recordId;
                    formvm.CustomUrl = customUrl;
                    formvm.ReturnUrl = ru;
                    formvm.LayoutMode = lo;
                    formvm.Udfs = Helper.QueryStringToDictionary(udfs);
                    formvm.ReadoOnlyUdfs = Helper.QueryStringToBooleanDictionary(roudfs);
                    formvm.PopupCallbackMethod = cbm;
                    return await GetForm(formvm);
                case TemplateTypeEnum.Note:
                    var notevm = await _cmsBusiness.GetSingle<NoteTemplateViewModel, NoteTemplate>(x => x.TemplateId == page.TemplateId);
                    notevm.Page = page;
                    notevm.Prms = Helper.QueryStringToDictionary(prms);
                    notevm.TemplateId = page.TemplateId;
                    notevm.ActiveUserId = _userContext.UserId;
                    notevm.NoteId = notevm.RecordId = recordId;
                    notevm.IncludeSharedList = true;
                    notevm.CustomUrl = customUrl;
                    notevm.ReturnUrl = ru;
                    notevm.LayoutMode = lo;
                    notevm.PopupCallbackMethod = cbm;
                    notevm.Udfs = Helper.QueryStringToDictionary(udfs);
                    notevm.ReadoOnlyUdfs = Helper.QueryStringToBooleanDictionary(roudfs);
                    notevm.LogId = logId;
                    notevm.AttachmentIds = attachments;

                    if (page.DataAction == DataActionEnum.View && viewMode != NtsViewTypeEnum.Book)
                    {
                        //var template = await _templateBusiness.GetSingleById(page.TemplateId);
                        if (page.Template.ViewType == NtsViewTypeEnum.Book)
                        {
                            var model = await _noteBusiness.GetBookDetails(recordId);
                            model.ViewType = NtsViewTypeEnum.Book;
                            model.DataAction = DataActionEnum.View;
                            ViewBag.Layout = $"~/Views/Shared/Themes/{page.Portal.Theme.ToString()}/_Layout.cshtml";
                            return View("Note", model);
                        }
                        else
                        {
                            notevm.ViewType = page.Template.ViewType;
                        }
                    }
                    else
                    {
                        notevm.ViewType = page.Template.ViewType;
                    }
                    return await GetNote(notevm);

                case TemplateTypeEnum.Task:
                    var taskvm = await _cmsBusiness.GetSingle<TaskTemplateViewModel, TaskTemplate>(x => x.TemplateId == page.TemplateId);
                    taskvm.Page = page;
                    taskvm.Prms = Helper.QueryStringToDictionary(prms);
                    taskvm.TemplateId = page.TemplateId;
                    taskvm.ActiveUserId = _userContext.UserId;
                    taskvm.TaskId = taskvm.RecordId = recordId;
                    taskvm.IncludeSharedList = true;

                    taskvm.CustomUrl = customUrl;
                    taskvm.ReturnUrl = ru;
                    taskvm.LayoutMode = lo;
                    taskvm.PopupCallbackMethod = cbm;
                    taskvm.Udfs = Helper.QueryStringToDictionary(udfs);
                    taskvm.ReadoOnlyUdfs = Helper.QueryStringToBooleanDictionary(roudfs);
                    taskvm.LogId = logId;
                    return await GetTask(taskvm);
                case TemplateTypeEnum.Service:
                    var serviceVm = await _cmsBusiness.GetSingle<ServiceTemplateViewModel, ServiceTemplate>(x => x.TemplateId == page.TemplateId);
                    serviceVm.Page = page;
                    serviceVm.Prms = Helper.QueryStringToDictionary(prms);
                    serviceVm.TemplateId = page.TemplateId;
                    serviceVm.ActiveUserId = _userContext.UserId;
                    serviceVm.ServiceId = recordId;
                    serviceVm.RecordId = recordId;
                    serviceVm.IncludeSharedList = true;

                    serviceVm.CustomUrl = customUrl;
                    serviceVm.ReturnUrl = ru;
                    serviceVm.LayoutMode = lo;
                    serviceVm.PopupCallbackMethod = cbm;
                    serviceVm.Udfs = Helper.QueryStringToDictionary(udfs);
                    serviceVm.ReadoOnlyUdfs = Helper.QueryStringToBooleanDictionary(roudfs);
                    serviceVm.LogId = logId;
                    if (page.DataAction == DataActionEnum.View && viewMode != NtsViewTypeEnum.Book)
                    {
                        // var template = await _templateBusiness.GetSingleById(page.TemplateId);
                        if (page.Template.ViewType == NtsViewTypeEnum.Book)
                        {
                            var model = await _serviceBusiness.GetBookDetails(recordId);
                            model.ViewType = NtsViewTypeEnum.Book;
                            model.DataAction = DataActionEnum.View;
                            ViewBag.Layout = $"~/Views/Shared/Themes/{page.Portal.Theme.ToString()}/_Layout.cshtml";
                            return View("Service", model);
                        }
                        else
                        {
                            serviceVm.ViewType = page.Template.ViewType;
                        }
                    }
                    else
                    {
                        serviceVm.ViewType = page.Template.ViewType;
                    }
                    return await GetService(serviceVm);

                case TemplateTypeEnum.Custom:
                    return await GetCustom(page);
                case TemplateTypeEnum.FormIndexPage:
                case TemplateTypeEnum.Page:
                default:

                    var pageModel = new PageTemplateViewModel();
                    pageModel.Prms = Helper.QueryStringToDictionary(prms);
                    pageModel.ChartItems = await BuildChart(page.Template.Json, "", pageModel.Prms, page.RecordId);
                    var viewStr = await RenderViewToStringAsync(page.PageType.ToString(), pageModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                    page.Template.Json = _webApi.AddHost(page.Template.Json);
                    ReplaceFormIoJsonDataForPage(page);
                    if (lo == LayoutModeEnum.Div)
                    {
                        return Json(new { view = viewStr, uiJson = page.Template.Json, dataJson = "{}", page = page, am = false });
                    }
                    else
                    {
                        return Json(new { view = viewStr, uiJson = page.Template.Json, dataJson = "{}", page = page, am = true });
                    }


            }

        }
        public async Task ChangeContext(PageViewModel page)
        {
            if (Request.HttpContext.User.Identity.IsAuthenticated && page.PortalId != _userContext.PortalId)
            {
                var id = new ApplicationIdentityUser
                {
                    Id = _userContext.UserId,
                    UserName = _userContext.Name,
                    IsSystemAdmin = _userContext.IsSystemAdmin,
                    Email = _userContext.Email,
                    UserUniqueId = _userContext.Email,
                    CompanyId = _userContext.CompanyId,
                    CompanyCode = _userContext.CompanyCode,
                    CompanyName = _userContext.CompanyName,
                    JobTitle = _userContext.JobTitle,
                    PhotoId = _userContext.PhotoId,
                    UserRoleCodes = _userContext.UserRoleCodes,
                    UserRoleIds = _userContext.UserRoleIds,
                    IsGuestUser = _userContext.IsGuestUser,
                    UserPortals = _userContext.UserPortals,
                    PortalTheme = page.Portal.Theme.ToString(),
                    PortalId = page.PortalId,
                    LegalEntityId = _userContext.LegalEntityId,
                    LegalEntityCode = _userContext.LegalEntityCode,
                    PersonId = _userContext.PersonId,
                    PositionId = _userContext.PositionId,
                    DepartmentId = _userContext.OrganizationId,
                    PortalName = page.Portal.Name,
                };
                id.MapClaims();
                await _customUserManager.SignInAsync(id, true);
            }
        }

        public async Task<IActionResult> Report(string rptName, string rptUrl, string rptUrl2, string rptUrl3, LayoutModeEnum lo = LayoutModeEnum.Main, string ru = null, string cbm = null, string portalId = null)
        {
            var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = string.Concat("~/Views/Shared/_PopupLayout.cshtml");
            }
            if (portalId.IsNotNullAndNotEmpty())
            {
                if (lo == LayoutModeEnum.Iframe)
                {
                    var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(portalId);
                    if (portal != null)
                    {
                        ViewBag.Layout = string.Concat($"~/Views/Shared/Themes/{portal.Theme}/_Layout.cshtml");
                    }
                }
            }
            ViewBag.ReportName = rptName;
            ViewBag.ReportUrl = $"{baseurl}{rptUrl}";
            ViewBag.ReportUrl2 = $"{baseurl}{rptUrl2}";
            ViewBag.ReportUrl3 = $"{baseurl}{rptUrl3}";
            //ViewBag.Layout = string.Concat("~/Views/Shared/Themes/Recruitment/_Layout.cshtml");
            //ViewBag.ReportName = "Pms/PMS_LetterTemplate.trdp";
            //ViewBag.ReportName = "Pay/PaySlip.trdp";
            // ViewBag.ReportUrl = "https://localhost:44389/pms/query/GetLetterTemplateDetails/27f042e1-39dd-4877-a627-dc8bf8dc8140";
            return View();

        }

        public async Task<IActionResult> FastReport(string rptName, string rptUrl, string rptUrl2, string rptUrl3, LayoutModeEnum lo = LayoutModeEnum.Main, string ru = null, string cbm = null, string portalId = null)
        {

            //rptName = "CMS_ServiceBookReport";
            //rptUrl = "cms/query/GetServiceBookReport?serviceId=9915ec51-e8d8-402c-a716-079834a52555";
            var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = string.Concat("~/Views/Shared/_PopupLayout.cshtml");
            }
            if (portalId.IsNotNullAndNotEmpty())
            {
                if (lo == LayoutModeEnum.Iframe)
                {
                    var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(portalId);
                    if (portal != null)
                    {
                        ViewBag.Layout = string.Concat($"~/Views/Shared/Themes/{portal.Theme}/_Layout.cshtml");
                    }
                }
            }
            string dir = Path.Combine(_sp.GetService<Microsoft.Extensions.Hosting.IHostEnvironment>().ContentRootPath, "Reports");
            var webReport = new FastReport.Web.WebReport();

            if (rptUrl.IsNotNullAndNotEmpty())
            {
                var connection = new JsonDataConnection();
                connection.ConnectionString = $"Json={baseurl}{rptUrl}";
                //connection.CreateAllTables();
                webReport.Report.Dictionary.Connections.Add(connection);
            }
            if (rptUrl2.IsNotNullAndNotEmpty())
            {
                var connection = new JsonDataConnection();
                connection.ConnectionString = $"Json={baseurl}{rptUrl2}";
                //connection.CreateAllTables();
                webReport.Report.Dictionary.Connections.Add(connection);
            }
            if (rptUrl3.IsNotNullAndNotEmpty())
            {
                var connection = new JsonDataConnection();
                connection.ConnectionString = $"Json={baseurl}{rptUrl3}";
                // connection.CreateAllTables();
                webReport.Report.Dictionary.Connections.Add(connection);
            }

            //webReport.Report.RegisterData(connection.DataSet);
            webReport.Report.Load(Path.Combine(dir, $"{rptName}.frx"));


            //rptName = "test";

            return View(webReport);
        }

        //private async Task SetPortalTheme(PageViewModel page)
        //{

        //    var identity = new ApplicationIdentityUser
        //    {
        //        Id = _userContext.UserId,
        //        UserName = _userContext.Name,
        //        IsSystemAdmin = _userContext.IsSystemAdmin,
        //        Email = _userContext.Email,
        //        UserUniqueId = _userContext.Email,
        //        CompanyId = _userContext.CompanyId,
        //        CompanyCode = _userContext.CompanyCode,
        //        CompanyName = _userContext.CompanyName,
        //        JobTitle = _userContext.JobTitle,
        //        PhotoId = _userContext.PhotoId,
        //        UserRoleCodes = _userContext.UserRoleCodes,
        //        UserRoleIds = _userContext.UserRoleIds,
        //        PortalId = page.Portal.Id,
        //        UserPortals = _userContext.UserPortals,
        //        PortalTheme = page.Portal.Theme.ToString(),
        //        LegalEntityId = _userContext.LegalEntityId,
        //        LegalEntityCode = _userContext.LegalEntityCode,
        //        PersonId = _userContext.PersonId,
        //        PositionId = _userContext.PositionId,
        //        DepartmentId = _userContext.OrganizationId,


        //    };
        //    identity.MapClaims();
        //    // Re-Signin User to reflect the change in the Identity cookieawa
        //    await _customUserManager.SignInAsync(identity, true, authenticationMethod: null);
        //}

        private async Task<List<BreadcrumbViewModel>> LoadBreadcrumbs(PageViewModel page)
        {
            var list = new List<BreadcrumbViewModel>();
            if (page.Portal.EnableBreadcrumb)
            {
                list = await _pageBusiness.GetBreadcrumbs(page);
            }
            return await Task.FromResult(list);
        }

        private async Task<IActionResult> LoadCms(string portalName, string pageName, string pageId, RunningModeEnum runningMode, RequestSourceEnum requestSource, string pageUrl, string customUrl, string prms, string templateCodes, string categoryCodes)
        {

            PortalViewModel portal = null;
            var domain = string.Concat(Request.IsHttps ? "https://" : "http://", Request.Host.Value).ToLower();
            if (portalName.IsNullOrEmpty())
            {
                portal = await _portalBusiness.GetSingleGlobal(x => x.DomainName == domain);
                if (portal == null)
                {
                    return null;
                }
            }
            else
            {
                portal = await _portalBusiness.GetSingleGlobal(x => x.Name == portalName);
                if (portal == null)
                {
                    return NotFound();
                }
            }

            portal.DomainName = domain;
            var page = new PageViewModel();
            if (pageName.IsNotNullAndNotEmpty())
            {
                page = await _pageBusiness.GetPageDataForExecution(portal.Id, pageName, runningMode);

            }
            else if (pageId.IsNotNullAndNotEmpty())
            {
                page = await _pageBusiness.GetPageDataForExecution(pageId);
            }

            else
            {
                page = await _pageBusiness.GetDefaultPageDataByPortal(portal, runningMode);
            }
            if (page == null)
            {
                return NotFound();
            }
            if (page.Layout.IsNullOrEmpty())
            {
                if (portal.Layout.IsNotNullAndNotEmpty())
                {
                    page.Layout = portal.Layout;
                }
                else
                {
                    page.Layout = "_Layout";
                }
            }
            page.Portal = portal;

            ViewBag.PageUrl = $@"pageId={page.Id}&pageType={Convert.ToString(page.PageType)}&source=Main&recordId=&layout={page.Layout}&templateCodes={templateCodes}&categoryCodes={categoryCodes}";
            ViewBag.PageType = page.PageType.ToString();
            var authenticate = new Tuple<bool, ApplicationIdentityUser>(false, null);
            page.Portal = portal;
            if (page.AuthorizationNotRequired)
            {
                authenticate = await ManageGuestUser(page);
                if (!authenticate.Item1)
                {
                    return NotFound("No Guest User Found");
                }
                if (_userContext.PortalId != null && portal.Id != _userContext.PortalId)
                {
                    ViewBag.Layout = $"~/Views/Shared/_EmptyLayout.cshtml";
                    var exPortal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
                    if (exPortal != null)
                    {
                        ViewBag.PortalDisplayName = exPortal.DisplayName;
                    }
                    return View("MultiplePortalNotAllowed");
                }
                await ChangeContext(page);
            }
            else
            {
                if (Request.HttpContext.User.Identity.IsAuthenticated == false || _userContext.IsGuestUser)
                {
                    return RedirectToAction("Login", "Account", new { portalId = portal.Id, returnUrl = Request.Path });
                }
                if (_userContext.PortalId != null && portal.Id != _userContext.PortalId)
                {
                    var exPortal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
                    if (exPortal != null)
                    {
                        ViewBag.PortalDisplayName = exPortal.DisplayName;
                    }
                    ViewBag.Layout = $"~/Views/Shared/_EmptyLayout.cshtml";
                    return View("MultiplePortalNotAllowed");
                }
                await ChangeContext(page);

            }
            string userId = null;
            string companyId = null;
            var les = string.Empty;
            if (authenticate.Item2 != null)
            {
                userId = authenticate.Item2.Id;
                companyId = authenticate.Item2.CompanyId;

            }
            //else
            //{
            //    userId = _userContext.UserId;
            //    companyId = _userContext.CompanyId;
            //}

            var menus = await _portalBusiness.GetMenuItems(portal, userId, companyId);
            if (menus != null && menus.Count > 0)
            {
                var landingPage = menus.FirstOrDefault(x => x.IsRoot == true);
                if (landingPage != null)
                {
                    portal.LandingPage = string.Concat("/Portal/", portal.Name, "/", landingPage.Name);
                }
                else
                {
                    portal.LandingPage = string.Concat("/Portal/", portal.Name);
                }
            }
            if (pageUrl.IsNotNullAndNotEmpty())
            {
                var myUri = new Uri(string.Concat("http://localhost?", pageUrl));
                string pageType = HttpUtility.ParseQueryString(myUri.Query).Get("pageType");
                var lt = HttpUtility.ParseQueryString(myUri.Query).Get("layout");
                if (lt.IsNotNullAndNotEmpty())
                {
                    page.Layout = lt;
                }

                ViewBag.PageType = pageType;
                ViewBag.PageUrl = pageUrl;
            }

            if (page.Layout.IsNullOrEmpty())
            {
                page.Layout = "_Layout";
            }
            page.Portal = portal;
            page.RunningMode = runningMode;
            page.RequestSource = requestSource;

            ViewBag.Menus = menus.Where(x => x.IsHidden == false).ToList();
            ViewBag.PageId = page.Id;
            ViewBag.PortalId = portal.Id;
            ViewBag.Title = string.Concat(page.Title, " - ", portal.DisplayName);
            ViewBag.Portal = portal;
            if (portal.PortalFooterTeext.IsNotNullAndNotEmpty())
            {
                ViewBag.Footer = portal.PortalFooterTeext.Replace("{{year}}", DateTime.Today.Year.ToString());
            }

            ViewBag.FavIconId = portal.FavIconId;
            ViewBag.Page = page;
            ViewBag.PageName = page.Name;
            ViewBag.PortalName = portal.Name;
            ViewBag.CustomUrl = customUrl;
            ViewBag.Prms = prms;
            ViewBag.HideHeader = page.DontShowMenuInThisPage;
            ViewBag.EnableMultiLanguage = portal.EnableMultiLanguage;
            if (_userContext != null && _userContext.UserId.IsNotNullAndNotEmpty())
            {
                var ap = await _userBusiness.GetAllowedPortalList(_userContext.UserId);
                ViewBag.AllowedPortals = ap.Where(x => x.Name != "CMS").ToList();
                var userdetails = await _userBusiness.GetSingleById(_userContext.UserId);
                var legalentity = "'" + string.Join("','", userdetails.LegalEntityIds) + "'";
                var legalEntities = await _userBusiness.GetEntityByIds(legalentity);
                ViewBag.AllowedLegalEntities = legalEntities;
                var lnges = await _userBusiness.GetList<LOVViewModel, LOV>(x => x.LOVType == "LANGUAGES");
                ViewBag.AllowedLanguages = lnges.Where(x => portal.AllowedLanguageIds.Any(y => y == x.Id)).ToList();

                var lang = lnges.FirstOrDefault(x => x.Code == _userContext.CultureName);

                ViewBag.LanguageName = lang?.Name;
                var le = legalEntities.FirstOrDefault(x => x.Id == _userContext.LegalEntityId);
                ViewBag.LegalEntityName = le?.Name;

                var prtl = ap.FirstOrDefault(x => x.Id == portal.Id);
                ViewBag.PortalDisplayName = prtl?.DisplayName;
            }
            return View("Index", page);
        }

        [HttpGet]
        public async Task<ActionResult> GetUnreadNotificationList(string portalId)
        {
            ViewBag.PortalId = portalId;
            NotificationSearchViewModel listModel = new NotificationSearchViewModel
            {
                Notifications = await _notificationBusiness.GetNotificationList(_userContext.UserId, _userContext.PortalId, 20)
            };
            return PartialView("_Notification", listModel);
        }
        [HttpPost]
        public async Task<IActionResult> MarkNotificationAsRead(string id)
        {
            await _notificationBusiness.MarkNotificationAsRead(id);
            return Json(new { success = true });

        }
        [HttpGet]
        public async Task<ActionResult> GetUnreadNotificationCount()
        {
            var count = await _notificationBusiness.GetNotificationCount(_userContext.UserId, _userContext.PortalId);
            return Json(new { UnReadCount = count });
        }
        private async Task<Tuple<bool, ApplicationIdentityUser>> ManageGuestUser(PageViewModel page)
        {
            if (Request.HttpContext.User.Identity.IsAuthenticated == false)
            {
                var user = await _userBusiness.GetGuestUser(page.CompanyId);
                if (user == null)
                {
                    return new Tuple<bool, ApplicationIdentityUser>(false, null);
                }
                var id = new ApplicationIdentityUser
                {
                    Id = user.Id,
                    UserName = user.Name,
                    IsSystemAdmin = user.IsSystemAdmin,
                    Email = user.Email,
                    UserUniqueId = user.Email,
                    CompanyId = user.CompanyId,
                    CompanyCode = user.CompanyCode,
                    CompanyName = user.CompanyName,
                    JobTitle = user.JobTitle,
                    PhotoId = user.PhotoId,
                    UserRoleCodes = string.Join(",", user.UserRoles.Select(x => x.Code)),
                    UserRoleIds = string.Join(",", user.UserRoles.Select(x => x.Id)),
                    IsGuestUser = user.IsGuestUser,
                    UserPortals = user.UserPortals,
                    PortalTheme = page.Portal.Theme.ToString(),
                    PortalId = page.PortalId,
                    LegalEntityId = user.LegalEntityId,
                    LegalEntityCode = user.LegalEntityCode,
                    PersonId = user.PersonId,
                    PositionId = user.PositionId,
                    DepartmentId = user.DepartmentId,
                    PortalName = page.Portal.Name,
                };
                id.MapClaims();
                await _customUserManager.PasswordSignInAsync(id, user.Password, true, false);
                return new Tuple<bool, ApplicationIdentityUser>(true, id);

            }


            return new Tuple<bool, ApplicationIdentityUser>(true, null);
        }

        private async Task<IActionResult> ValidateRequest(PortalViewModel portal, string pageName, string id)
        {
            var isAuth = Request.HttpContext.User.Identity.IsAuthenticated;
            if (!isAuth)
            {
                //  var loginurl = $"/Identity/Account/LogIn?returnUrl={Request.Path}&theme={portal.Theme.ToString()}";
                return RedirectToAction("Login", "Account", new { portalId = portal.Id, returnUrl = Request.Path });
            }
            return null;
        }

        #region Form
        private async Task<IActionResult> GetMenuGroup(PageViewModel page)
        {
            if (page.LayoutMode.HasValue && page.LayoutMode == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{page.Portal.Theme.ToString()}/_Layout.cshtml";
                return View("Form", page);
            }
            else if (page.LayoutMode.HasValue && page.LayoutMode == LayoutModeEnum.Iframe)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{page.Portal.Theme.ToString()}/_Layout.cshtml";
                return View("Form", page);
            }
            else
            {
                var viewStr = await RenderViewToStringAsync(TemplateTypeEnum.MenuGroup.ToString(), page, _contextAccessor, _razorViewEngine, _tempDataProvider);
                return Json(new { view = viewStr, uiJson = "{}", dataJson = "{}", bc = page.Breadcrumbs, page = page, am = true });
            }
        }
        private async Task<IActionResult> GetForm(FormTemplateViewModel formTemplate)
        {
            var viewModel = await GetFormViewModel(formTemplate);
            if (formTemplate.Page.RequestSource == RequestSourceEnum.Post)
            {
                if (formTemplate.Page.DataAction == DataActionEnum.Create)
                {
                    var create = await _cmsBusiness.ManageForm(formTemplate);
                    if (create.IsSuccess)
                    {
                        var msg = "Item created successfully.";
                        if (formTemplate.CreateReturnType == CreateReturnTypeEnum.ReloadDataInEditMode)
                        {

                            if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Popup)
                            {
                                return Json(new { success = true, msg = msg, vm = viewModel, mode = Convert.ToString(formTemplate.LayoutMode), cbm = viewModel.PopupCallbackMethod });
                            }
                            else if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Iframe)
                            {
                                return Json(new { success = true, msg = msg, vm = viewModel, mode = Convert.ToString(formTemplate.LayoutMode), cbm = viewModel.PopupCallbackMethod });
                            }
                            else if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Div)
                            {
                                return Json(new { success = true, msg = msg, vm = viewModel, mode = Convert.ToString(formTemplate.LayoutMode), cbm = viewModel.PopupCallbackMethod });
                            }
                            else if (formTemplate.ReturnUrl.IsNotNullAndNotEmpty())
                            {
                                return Json(new { success = true, msg = msg, uiJson = formTemplate.Json, ru = formTemplate.ReturnUrl, mode = Convert.ToString(formTemplate.LayoutMode) });
                            }
                            else
                            {
                                viewModel.DataAction = DataActionEnum.Edit;
                                var vn0 = formTemplate.Page.PageType;
                                formTemplate.Page.RecordId = viewModel.RecordId = create.Item.RecordId;
                                var data0 = await GetFormModel(formTemplate.Page, vn0);
                                var vs1 = await RenderViewToStringAsync(formTemplate.Page.PageType.ToString(), viewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                                return Json(new { success = true, msg = msg, view = vs1, uiJson = formTemplate.Page.Template.Json, dataJson = data0 });

                            }
                        }
                        else
                        {
                            if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Popup)
                            {
                                return Json(new { success = true, msg = msg, vm = viewModel, mode = Convert.ToString(formTemplate.LayoutMode), cbm = viewModel.PopupCallbackMethod });
                            }
                            else if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Iframe)
                            {
                                return Json(new { success = true, msg = msg, vm = viewModel, mode = Convert.ToString(formTemplate.LayoutMode), cbm = viewModel.PopupCallbackMethod });
                            }
                            else if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Div)
                            {
                                return Json(new { success = true, msg = msg, vm = viewModel, mode = Convert.ToString(formTemplate.LayoutMode), cbm = viewModel.PopupCallbackMethod });
                            }
                            else if (formTemplate.ReturnUrl.IsNotNullAndNotEmpty())
                            {
                                return Json(new { success = true, msg = msg, uiJson = formTemplate.Json, ru = formTemplate.ReturnUrl, mode = Convert.ToString(formTemplate.LayoutMode) });
                            }
                            else
                            {
                                var ivm = await GetFormIndexPageViewModel(formTemplate.Page);
                                var vs = await RenderViewToStringAsync(TemplateTypeEnum.FormIndexPage.ToString(), ivm, _contextAccessor, _razorViewEngine, _tempDataProvider);
                                return Json(new { success = true, msg = msg, view = vs, uiJson = formTemplate.Page.Template.Json, dataJson = "{}", mode = Convert.ToString(formTemplate.LayoutMode) });
                            }
                        }
                    }
                    else
                    {
                        return Json(new { success = false, error = create.HtmlError });
                    }
                }
                else if (formTemplate.Page.DataAction == DataActionEnum.Edit)
                {
                    var edit = await _cmsBusiness.ManageForm(formTemplate);
                    if (edit.IsSuccess)
                    {
                        var msg = "Item updated successfully.";
                        if (formTemplate.EditReturnType == EditReturnTypeEnum.ReloadDataInEditMode)
                        {
                            if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Popup)
                            {
                                return Json(new { success = true, msg = msg, vm = viewModel, mode = Convert.ToString(formTemplate.LayoutMode), cbm = viewModel.PopupCallbackMethod });
                            }
                            else if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Iframe)
                            {
                                return Json(new { success = true, msg = msg, vm = viewModel, mode = Convert.ToString(formTemplate.LayoutMode), cbm = viewModel.PopupCallbackMethod });
                            }
                            else if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Div)
                            {
                                return Json(new { success = true, msg = msg, vm = viewModel, mode = Convert.ToString(formTemplate.LayoutMode), cbm = viewModel.PopupCallbackMethod });
                            }
                            else if (formTemplate.ReturnUrl.IsNotNullAndNotEmpty())
                            {
                                return Json(new { success = true, msg = msg, uiJson = formTemplate.Json, ru = formTemplate.ReturnUrl, mode = Convert.ToString(formTemplate.LayoutMode) });
                            }
                            else
                            {
                                var vn0 = formTemplate.Page.PageType;
                                formTemplate.Page.RecordId = viewModel.RecordId = edit.Item.RecordId;
                                viewModel.DataAction = DataActionEnum.Edit;
                                var data0 = await GetFormModel(formTemplate.Page, vn0);
                                var vs1 = await RenderViewToStringAsync(formTemplate.Page.PageType.ToString(), viewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                                return Json(new { success = true, msg = msg, view = vs1, uiJson = formTemplate.Page.Template.Json, dataJson = data0, mode = Convert.ToString(formTemplate.LayoutMode) });
                            }
                        }
                        else
                        {
                            if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Popup)
                            {
                                return Json(new { success = true, msg = msg, vm = viewModel, mode = Convert.ToString(formTemplate.LayoutMode), cbm = viewModel.PopupCallbackMethod });
                            }
                            if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Iframe)
                            {
                                return Json(new { success = true, msg = msg, vm = viewModel, mode = Convert.ToString(formTemplate.LayoutMode), cbm = viewModel.PopupCallbackMethod });
                            }
                            else if (formTemplate.ReturnUrl.IsNotNullAndNotEmpty())
                            {
                                return Json(new { success = true, msg = msg, uiJson = formTemplate.Json, ru = formTemplate.ReturnUrl, mode = Convert.ToString(formTemplate.LayoutMode) });
                            }
                            else
                            {
                                var ivm = await GetFormIndexPageViewModel(formTemplate.Page);
                                var vs = await RenderViewToStringAsync(TemplateTypeEnum.FormIndexPage.ToString(), ivm, _contextAccessor, _razorViewEngine, _tempDataProvider);
                                return Json(new { success = true, msg = msg, view = vs, uiJson = formTemplate.Page.Template.Json, dataJson = "{}", mode = Convert.ToString(formTemplate.LayoutMode) });
                            }
                        }
                    }
                    else
                    {
                        return Json(new { success = false, error = edit.HtmlError });
                    }
                }
                else if (formTemplate.Page.DataAction == DataActionEnum.Delete)
                {
                    var delete = await _cmsBusiness.ManageForm(new FormTemplateViewModel
                    {
                        DataAction = DataActionEnum.Delete,
                        PageId = formTemplate.Page.Id,
                        RecordId = formTemplate.Page.RecordId

                    });
                    var viewName = TemplateTypeEnum.FormIndexPage;
                    var indexViewModel = await GetFormIndexPageViewModel(formTemplate.Page);
                    var viewStr = await RenderViewToStringAsync(viewName.ToString(), indexViewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                    return Json(new { msg = "Selected item deleted successfully.", view = viewStr, uiJson = formTemplate.Page.Template.Json, dataJson = "{}", mode = Convert.ToString(formTemplate.LayoutMode) });
                }
                else
                {
                    return Json(new { success = false, error = "Invalid action" });
                }
            }
            else
            {
                if (formTemplate.Page.RequestSource == RequestSourceEnum.Main && viewModel.EnableIndexPage)
                {
                    var viewName = TemplateTypeEnum.FormIndexPage.ToString();
                    if (formTemplate.LayoutMode.HasValue && (formTemplate.LayoutMode == LayoutModeEnum.Popup
                      || formTemplate.LayoutMode == LayoutModeEnum.Iframe))
                    {

                        var indexViewModel = await GetFormIndexPageViewModel(formTemplate.Page);
                        ViewBag.Layout = $"~/Views/Shared/Themes/{formTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                        return View(viewName, indexViewModel);
                    }
                    else
                    {

                        var indexViewModel = await GetFormIndexPageViewModel(formTemplate.Page);
                        var viewStr = await RenderViewToStringAsync(viewName, indexViewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { view = viewStr, uiJson = formTemplate.Page.Template.Json, dataJson = "{}", bc = formTemplate.Page.Breadcrumbs, page = formTemplate.Page });
                    }

                }
                else
                {

                    var viewName = formTemplate.Page.PageType;
                    formTemplate.DataJson = await GetFormModel(formTemplate.Page, viewName);
                    if (formTemplate.DataJson.IsNotNull())
                    {
                        formTemplate.DataJson = await AttachmentConfiguration(formTemplate.Page.Template.Json, formTemplate.DataJson);
                    }
                    if (formTemplate.Json.IsNotNullAndNotEmpty())
                    {
                        formTemplate.Json = formTemplate.Json.Replace("SaveFileInFormIo", "SaveFileInFormIo?referenceId=" + formTemplate.Id + "&referenceType=" + ReferenceTypeEnum.Form);
                        formTemplate.Json = formTemplate.Page.Template.Json = _webApi.AddHost(formTemplate.Json);
                        ReplaceFormIoJsonDataForForm(formTemplate);
                    }

                    if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Popup)
                    {
                        formTemplate.ChartItems = await BuildChart(formTemplate.Json, formTemplate.DataJson, formTemplate.Prms, formTemplate.RecordId);
                        ViewBag.Layout = $"~/Views/Shared/Themes/{formTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                        return View("Form", formTemplate);
                    }
                    else if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Iframe)
                    {
                        formTemplate.ChartItems = await BuildChart(formTemplate.Json, formTemplate.DataJson, formTemplate.Prms, formTemplate.RecordId);
                        ViewBag.Layout = $"~/Views/Shared/Themes/{formTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                        return View("Form", formTemplate);
                    }
                    else if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Div)
                    {
                        var viewStr = await RenderViewToStringAsync(viewName.ToString(), viewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { view = viewStr, uiJson = formTemplate.Json, dataJson = formTemplate.DataJson, bc = formTemplate.Page.Breadcrumbs, page = formTemplate.Page, am = false });
                    }
                    else
                    {
                        var viewStr = await RenderViewToStringAsync(viewName.ToString(), viewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { view = viewStr, uiJson = formTemplate.Json, dataJson = formTemplate.DataJson, bc = formTemplate.Page.Breadcrumbs, page = formTemplate.Page, am = true });
                    }
                }
            }
        }
        public async Task<IActionResult> GetFormIoData(NtsTypeEnum ntsType, string templateId, string recordId)
        {
            switch (ntsType)
            {
                case NtsTypeEnum.Note:
                    var note = await _noteBusiness.GetFormIoData(templateId, recordId, _userContext.UserId);
                    if (note != null)
                    {
                        note.Json = _webApi.AddHost(note.Json);
                        return Json(new { uiJson = note.Json, dataJson = note.DataJson });
                    }
                    return Json(new { });

                case NtsTypeEnum.Task:
                    var task = await _taskBusiness.GetFormIoData(templateId, recordId, _userContext.UserId);
                    if (task != null)
                    {
                        task.Json = _webApi.AddHost(task.Json);
                        return Json(new { uiJson = task.Json, dataJson = task.DataJson });
                    }
                    return Json(new { });
                case NtsTypeEnum.Service:
                    var service = await _serviceBusiness.GetFormIoData(templateId, recordId, _userContext.UserId);
                    if (service != null)
                    {
                        service.Json = _webApi.AddHost(service.Json);
                        return Json(new { uiJson = service.Json, dataJson = service.DataJson });
                    }
                    return Json(new { });
                default:
                    break;
            }
            return null;
            //return Json(new { view = viewStr, uiJson = formTemplate.Json, dataJson = formTemplate.DataJson, bc = formTemplate.Page.Breadcrumbs, page = formTemplate.Page, am = false });
        }
        public async Task<IActionResult> LoadFormIoData(NtsTypeEnum ntsType, string templateId, string recordId, string t)
        {
            switch (ntsType)
            {
                case NtsTypeEnum.Note:
                    var note = await _noteBusiness.GetFormIoData(templateId, recordId, _userContext.UserId);
                    if (note != null)
                    {
                        return View(new NoteTemplateViewModel { Json = note.Json, DataJson = note.DataJson });
                    }
                    return View(new NoteTemplateViewModel { });

                case NtsTypeEnum.Task:
                    var task = await _taskBusiness.GetFormIoData(templateId, recordId, _userContext.UserId);
                    if (task != null)
                    {
                        return View(new NoteTemplateViewModel { Json = task.Json, DataJson = task.DataJson });
                    }
                    return View(new NoteTemplateViewModel { });
                case NtsTypeEnum.Service:
                    var service = await _serviceBusiness.GetFormIoData(templateId, recordId, _userContext.UserId);
                    if (service != null)
                    {
                        return View(new NoteTemplateViewModel { Json = service.Json, DataJson = service.DataJson });
                    }
                    return View(new NoteTemplateViewModel { });
                default:
                    break;
            }
            return null;
            //return Json(new { view = viewStr, uiJson = formTemplate.Json, dataJson = formTemplate.DataJson, bc = formTemplate.Page.Breadcrumbs, page = formTemplate.Page, am = false });
        }
        private async Task<IActionResult> GetCustom(PageViewModel page)
        {
            var viewModel = await GetCustomViewModel(page);
            var viewName = page.PageType;

            if (page.LayoutMode.HasValue && page.LayoutMode == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{page.Portal.Theme.ToString()}/_Layout.cshtml";
                var customTemplate = await _cmsBusiness.GetSingle<CustomTemplateViewModel, CustomTemplate>(x => x.TemplateId == page.Template.Id);
                if (customTemplate != null)
                {
                    var prms = Helper.QueryStringToDictionary(customTemplate.Parameter);
                    return RedirectToAction(customTemplate.ActionName, customTemplate.ControllerName, new { area = customTemplate.AreaName, portalId = page.Portal.Id, pageId = page.Id, layout = ViewBag.Layout });
                }
                return View(viewName.ToString(), viewModel);
            }
            else if (page.LayoutMode.HasValue && page.LayoutMode == LayoutModeEnum.Div)
            {
                var viewStr = await RenderViewToStringAsync(viewName.ToString(), viewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                return Json(new { view = viewStr, uiJson = page.Template.Json, dataJson = viewModel, customUrl = page.CustomUrl, bc = page.Breadcrumbs, page = page, am = false });
            }
            else
            {
                if(viewModel.ActionName== "LoadIframePage")
                {
                    viewModel.Parameter = "src="+HttpUtility.UrlEncode(viewModel.Parameter);
                }
                var viewStr = await RenderViewToStringAsync(viewName.ToString(), viewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                return Json(new { view = viewStr, uiJson = page.Template.Json, dataJson = viewModel, customUrl = page.CustomUrl, bc = page.Breadcrumbs, page = page, am = true });

            }
        }

        private async Task<string> GetFormModel(PageViewModel page, TemplateTypeEnum viewName)
        {
            var data = await _cmsBusiness.GetDataById(viewName, page, page.RecordId);
            if (data == null)
            {
                return "{}";
            }
            return data.ToJson();
        }

        //public async Task<IActionResult> LoadFormIndexPageGrid([DataSourceRequest] DataSourceRequest request, string indexPageTemplateId)
        //{
        //    var dt = await _cmsBusiness.GetFormIndexPageGridData(indexPageTemplateId, request);
        //    return Json(dt.ToDataSourceResult(request));
        //}
        public async Task<IActionResult> LoadFormIndexPageGrid(string indexPageTemplateId)
        {
            var dt = await _cmsBusiness.GetFormIndexPageGridData(indexPageTemplateId, null);
            return Json(dt);
        }
        public async Task<FormIndexPageTemplateViewModel> GetFormIndexPageViewModel(PageViewModel page)
        {
            var model = await _cmsBusiness.GetFormIndexPageViewModel(page);
            model.Page = page;
            model.PageId = page.Id;
            return model;
        }
        private async Task<CustomTemplateViewModel> GetCustomViewModel(PageViewModel page)
        {
            var model = await _cmsBusiness.GetSingleGlobal<CustomTemplateViewModel, CustomTemplate>(x => x.TemplateId == page.TemplateId);
            model.Page = page;
            model.PageId = page.Id;
            model.DataAction = page.DataAction;
            model.RecordId = page.RecordId;
            model.PortalName = page.Portal.Name;
            //model.TemplateId = page.TemplateId;
            return model;
        }
        public async Task<FormTemplateViewModel> GetFormViewModel(FormTemplateViewModel formTemplate)
        {
            var model = formTemplate;
            model.RecordId = formTemplate.Page.RecordId;
            if (model.Page.RequestSource != RequestSourceEnum.Main
                && (model.Page.RequestSource != RequestSourceEnum.Post || model.Page.DataAction == DataActionEnum.Delete))
            {
                await _cmsBusiness.GetFormDetails(model);
            }

            model.Page = formTemplate.Page;
            model.PageId = formTemplate.Page.Id;
            model.DataAction = formTemplate.Page.DataAction;
            model.RecordId = formTemplate.Page.RecordId;
            model.PortalName = formTemplate.Page.Portal.Name;

            model.CustomUrl = formTemplate.Page.CustomUrl;
            model.ReturnUrl = formTemplate.Page.ReturnUrl;
            //model.LayoutMode = formTemplate.Page.LayoutMode;
            //model.PopupCallbackMethod = formTemplate.Page.PopupCallbackMethod;
            return model;
        }


        [HttpPost]
        public async Task<IActionResult> ManageForm(FormTemplateViewModel model)
        {
            var page = await _pageBusiness.GetPageForExecution(model.PageId);
            if (page != null)
            {
                page.RecordId = model.RecordId;
                page.DataAction = model.DataAction;
                page.RequestSource = RequestSourceEnum.Post;
                model.Page = page;
                return await GetForm(model);
            }
            return Json(new { success = false, error = "Page not found" });

        }

        #endregion


        #region Task

        //public async Task<IActionResult> GetTask(PageViewModel page)
        //{
        //    var viewName = page.PageType;
        //    dynamic viewModel = await GetTaskViewModel(page);

        //    if (page.RequestSource == RequestSourceEnum.Main && viewModel.EnableIndexPage)
        //    {
        //        viewName = TemplateTypeEnum.TaskIndexPage;

        //        viewModel = await GetTaskIndexPageViewModel(page);
        //    }
        //    var data = GetTaskModel(page, viewName);
        //    var viewStr = await RenderViewToStringAsync(viewName.ToString(), new PageTemplateViewModel(), _contextAccessor, _razorViewEngine, _tempDataProvider);
        //    return Json(new { view = viewStr, uiJson = page.Template.Json, dataJson = data });
        //}
        private async Task<IActionResult> GetTask(TaskTemplateViewModel taskTemplate)
        {
            taskTemplate = await GetTaskViewModel(taskTemplate);
            if (taskTemplate.Page.RequestSource == RequestSourceEnum.Post)
            {
                if (taskTemplate.Page.DataAction == DataActionEnum.Create)
                {
                    var create = await _taskBusiness.ManageTask(taskTemplate);
                    if (create.IsSuccess)
                    {
                        var msg = "Item created successfully.";
                        if (taskTemplate.CreateReturnType == CreateReturnTypeEnum.ReloadDataInEditMode)
                        {
                            if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Popup)
                            {
                                return Json(new { success = true, msg = msg, vm = taskTemplate, mode = Convert.ToString(taskTemplate.LayoutMode), cbm = taskTemplate.PopupCallbackMethod });
                            }
                            else if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Iframe)
                            {
                                return Json(new { success = true, msg = msg, vm = taskTemplate, mode = Convert.ToString(taskTemplate.LayoutMode), cbm = taskTemplate.PopupCallbackMethod });
                            }
                            else if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Div)
                            {
                                return Json(new { success = true, msg = msg, vm = taskTemplate, mode = Convert.ToString(taskTemplate.LayoutMode), cbm = taskTemplate.PopupCallbackMethod });
                            }
                            else if (taskTemplate.ReturnUrl.IsNotNullAndNotEmpty())
                            {
                                return Json(new { success = true, msg = msg, uiJson = taskTemplate.Json, ru = taskTemplate.ReturnUrl, mode = Convert.ToString(taskTemplate.LayoutMode) });
                            }
                            else
                            {
                                return Json(new
                                {
                                    success = true,
                                    msg = msg,
                                    reload = true,
                                    pageId = taskTemplate.Page.Id,
                                    pageType = taskTemplate.Page.PageType.ToString(),
                                    source = RequestSourceEnum.Edit.ToString(),
                                    dataAction = DataActionEnum.Edit.ToString(),
                                    recordId = create.Item.TaskId,
                                    mode = Convert.ToString(taskTemplate.LayoutMode)
                                });
                            }

                        }
                        else
                        {
                            if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Popup)
                            {
                                return Json(new { success = true, msg = msg, vm = taskTemplate, mode = Convert.ToString(taskTemplate.LayoutMode), cbm = taskTemplate.PopupCallbackMethod });
                            }
                            else if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Iframe)
                            {
                                return Json(new { success = true, msg = msg, vm = taskTemplate, mode = Convert.ToString(taskTemplate.LayoutMode), cbm = taskTemplate.PopupCallbackMethod });
                            }
                            else if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Div)
                            {
                                return Json(new { success = true, msg = msg, vm = taskTemplate, mode = Convert.ToString(taskTemplate.LayoutMode), cbm = taskTemplate.PopupCallbackMethod });
                            }
                            else if (taskTemplate.ReturnUrl.IsNotNullAndNotEmpty())
                            {
                                return Json(new { success = true, msg = msg, uiJson = taskTemplate.Json, ru = taskTemplate.ReturnUrl, mode = Convert.ToString(taskTemplate.LayoutMode) });
                            }
                            else
                            {
                                return Json(new
                                {
                                    success = true,
                                    msg = msg,
                                    reload = true,
                                    pageId = taskTemplate.Page.Id,
                                    pageType = taskTemplate.Page.PageType.ToString(),
                                    source = RequestSourceEnum.Main.ToString(),
                                    dataAction = DataActionEnum.None.ToString(),
                                    recordId = create.Item.TaskId,
                                    mode = Convert.ToString(taskTemplate.LayoutMode)
                                });

                            }
                        }
                    }
                    else
                    {
                        return Json(new { success = false, error = create.HtmlError });
                    }
                }
                else if (taskTemplate.Page.DataAction == DataActionEnum.Edit)
                {
                    var edit = await _taskBusiness.ManageTask(taskTemplate);
                    if (edit.IsSuccess)
                    {
                        var msg = "Item updated successfully.";
                        if (taskTemplate.EditReturnType == EditReturnTypeEnum.ReloadDataInEditMode)
                        {
                            if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Popup)
                            {
                                return Json(new { success = true, msg = msg, vm = taskTemplate, mode = Convert.ToString(taskTemplate.LayoutMode), cbm = taskTemplate.PopupCallbackMethod });
                            }
                            else if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Iframe)
                            {
                                return Json(new { success = true, msg = msg, vm = taskTemplate, mode = Convert.ToString(taskTemplate.LayoutMode), cbm = taskTemplate.PopupCallbackMethod });
                            }
                            else if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Div)
                            {
                                return Json(new { success = true, msg = msg, vm = taskTemplate, mode = Convert.ToString(taskTemplate.LayoutMode), cbm = taskTemplate.PopupCallbackMethod });
                            }
                            else if (taskTemplate.Page.ReturnUrl.IsNotNullAndNotEmpty())
                            {
                                return Json(new { success = true, msg = msg, uiJson = taskTemplate.Json, ru = taskTemplate.Page.ReturnUrl, mode = Convert.ToString(taskTemplate.LayoutMode), cbm = taskTemplate.PopupCallbackMethod });
                            }
                            else
                            {
                                var model = await _taskBusiness.GetTaskDetails(taskTemplate);
                                model.RecordId = taskTemplate.Page.RecordId;

                                model.Page = taskTemplate.Page;
                                model.PageId = taskTemplate.Page.Id;
                                model.DataAction = taskTemplate.Page.DataAction;
                                model.TaskId = model.RecordId = taskTemplate.Page.RecordId;
                                model.PortalName = taskTemplate.Page.Portal.Name;


                                var vn0 = taskTemplate.Page.PageType;
                                model.Page.RecordId = taskTemplate.RecordId = edit.Item.RecordId;
                                model.Page.DataAction = taskTemplate.DataAction = DataActionEnum.Edit;
                                model.Page.RequestSource = RequestSourceEnum.Edit;
                                var data0 = await GetTaskModel(model, vn0);
                                var vs1 = await RenderViewToStringAsync(model.Page.PageType.ToString(), model, _contextAccessor, _razorViewEngine, _tempDataProvider);
                                return Json(new { success = true, msg = msg, view = vs1, uiJson = model.Json, dataJson = data0, ru = model.Page.ReturnUrl, mode = Convert.ToString(taskTemplate.LayoutMode), cbm = taskTemplate.PopupCallbackMethod });
                            }
                        }
                        else
                        {
                            if (taskTemplate.Page.ReturnUrl.IsNotNullAndNotEmpty())
                            {
                                return Json(new { success = true, msg = msg, ru = taskTemplate.Page.ReturnUrl, mode = Convert.ToString(taskTemplate.LayoutMode) });
                            }
                            else
                            {

                                var ivm = await GetTaskIndexPageViewModel(taskTemplate.Page);
                                var vs = await RenderViewToStringAsync(TemplateTypeEnum.TaskIndexPage.ToString(), ivm, _contextAccessor, _razorViewEngine, _tempDataProvider);
                                return Json(new { success = true, msg = msg, view = vs, uiJson = taskTemplate.Json, dataJson = "{}", mode = Convert.ToString(taskTemplate.LayoutMode), cbm = taskTemplate.PopupCallbackMethod });
                            }

                        }
                    }
                    else
                    {
                        return Json(new { success = false, error = edit.HtmlError });
                    }
                }
                else if (taskTemplate.Page.DataAction == DataActionEnum.Delete)
                {
                    taskTemplate.UdfTableMetadataId = taskTemplate.Page.Template.UdfTableMetadataId;
                    var delete = await _taskBusiness.DeleteTask(taskTemplate);
                    var viewName = TemplateTypeEnum.TaskIndexPage;
                    var indexViewModel = await GetTaskIndexPageViewModel(taskTemplate.Page);
                    var viewStr = await RenderViewToStringAsync(viewName.ToString(), indexViewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                    return Json(new { msg = "Selected item deleted successfully.", view = viewStr, uiJson = taskTemplate.Json, dataJson = "{}", mode = Convert.ToString(taskTemplate.LayoutMode) });
                }
                else
                {
                    return Json(new { success = false, error = "Invalid action" });
                }
            }
            else
            {
                if (taskTemplate.Page.RequestSource == RequestSourceEnum.Main && taskTemplate.EnableIndexPage)
                {
                    var viewName = TemplateTypeEnum.TaskIndexPage;
                    var indexViewModel = await GetTaskIndexPageViewModel(taskTemplate.Page);
                    var viewStr = await RenderViewToStringAsync(viewName.ToString(), indexViewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                    return Json(new { view = viewStr, uiJson = taskTemplate.Json, dataJson = "{}", bc = taskTemplate.Page.Breadcrumbs, mode = Convert.ToString(taskTemplate.LayoutMode), page = taskTemplate.Page });
                }
                else
                {
                    var viewName = taskTemplate.Page.PageType;
                    if (taskTemplate.TaskStatusCode != "TASK_STATUS_DRAFT")
                    {
                        if (taskTemplate.Page.RequestSource == RequestSourceEnum.Versioning)
                        {
                            taskTemplate.IsVersioning = true;
                            taskTemplate.VersionNo += 1;
                        }
                    }
                    if (taskTemplate.Page.RequestSource == RequestSourceEnum.View)
                    {
                        taskTemplate.Json = SetUdfView(taskTemplate.Json);
                    }
                    taskTemplate.DataJson = await GetTaskModel(taskTemplate, viewName);
                    if (taskTemplate.DataJson.IsNotNull())
                    {
                        taskTemplate.DataJson = await AttachmentConfiguration(taskTemplate.Json, taskTemplate.DataJson);
                    }
                    if (taskTemplate.Json.IsNotNullAndNotEmpty())
                    {
                        taskTemplate.Json = taskTemplate.Json.Replace("SaveFileInFormIo", "SaveFileInFormIo?referenceId=" + taskTemplate.TaskId + "&referenceType=" + ReferenceTypeEnum.NTS_Task);
                        taskTemplate.Page.Template.Json = taskTemplate.Json = _webApi.AddHost(taskTemplate.Json);
                        ReplaceFormIoJsonDataForTask(taskTemplate);
                        //existing line
                        ////added by arshad for demo
                        //if (taskTemplate.TemplateCode != "INCIDENT")
                        //{

                        //}
                        //else
                        //{
                        //    taskTemplate.Json = taskTemplate.Page.Template.Json = _webApi.AddHost(taskTemplate.Page.Template.Json);
                        //    if (TempData["Keywords"] != null)
                        //    {
                        //        taskTemplate.TaskDescription = TempData["Keywords"].ToString();
                        //    }
                        //    else
                        //    {
                        //        taskTemplate.Json = taskTemplate.Json.Replace("{IncidentKeywords}", taskTemplate.TaskDescription);
                        //    }
                        //}
                        ////end
                    }
                    else
                    {
                        taskTemplate.Page.Template.Json = taskTemplate.Json = "{}";
                    }
                    if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Popup)
                    {
                        taskTemplate.ChartItems = await BuildChart(taskTemplate.Json, taskTemplate.DataJson, taskTemplate.Prms, taskTemplate.TaskId, taskTemplate.TaskStatusCode);
                        ViewBag.Layout = $"~/Views/Shared/Themes/{taskTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                        return View("Task", taskTemplate);
                    }
                    else if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Iframe)
                    {
                        taskTemplate.ChartItems = await BuildChart(taskTemplate.Json, taskTemplate.DataJson, taskTemplate.Prms, taskTemplate.TaskId, taskTemplate.TaskStatusCode);
                        ViewBag.Layout = $"~/Views/Shared/Themes/{taskTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                        return View("Task", taskTemplate);
                    }
                    else if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Div)
                    {
                        var viewStr = await RenderViewToStringAsync(viewName.ToString(), taskTemplate, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { view = viewStr, uiJson = taskTemplate.Json, dataJson = taskTemplate.DataJson, bc = taskTemplate.Page.Breadcrumbs, mode = Convert.ToString(taskTemplate.LayoutMode), page = taskTemplate.Page, am = false });
                    }
                    else
                    {
                        var viewStr = await RenderViewToStringAsync(viewName.ToString(), taskTemplate, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { view = viewStr, uiJson = taskTemplate.Json, dataJson = taskTemplate.DataJson, bc = taskTemplate.Page.Breadcrumbs, mode = Convert.ToString(taskTemplate.LayoutMode), page = taskTemplate.Page, am = true });

                    }
                }
            }
        }

        private async Task<string> GetTaskModel(TaskTemplateViewModel taskTemplateVM, TemplateTypeEnum viewName)
        {
            var data = await _cmsBusiness.GetDataById(viewName, taskTemplateVM.Page, taskTemplateVM.Page.RecordId, taskTemplateVM.IsLogRecord, taskTemplateVM.LogId);
            if (data == null)
            {
                return "{}";
            }
            return data.ToJson();
        }
        public async Task<IActionResult> GetTaskMessageList(string taskId)
        {
            var result = await _taskBusiness.GetTaskMessageList(_userContext.UserId, taskId);
            return Json(result);
        }
        public async Task<IActionResult> GetNoteMessageList(string noteId)
        {
            var result = await _noteBusiness.GetNoteMessageList(_userContext.UserId, noteId);
            return Json(result);
        }
        //public async Task<IActionResult> LoadTaskIndexPageGrid([DataSourceRequest] DataSourceRequest request, NtsActiveUserTypeEnum ownerType, string indexPageTemplateId, string taskStatusCode)
        //{
        //    var dt = await _taskBusiness.GetTaskIndexPageGridData(request, indexPageTemplateId, ownerType, taskStatusCode);
        //    return Json(dt.ToDataSourceResult(request));
        //}
        public async Task<IActionResult> LoadTaskIndexPageGrid(NtsActiveUserTypeEnum ownerType, string indexPageTemplateId, string taskStatusCode)
        {
            var dt = await _taskBusiness.GetTaskIndexPageGridData(null, indexPageTemplateId, ownerType, taskStatusCode);
            return Json(dt);
        }
        public async Task<TaskIndexPageTemplateViewModel> GetTaskIndexPageViewModel(PageViewModel page)
        {
            var model = await _taskBusiness.GetTaskIndexPageViewModel(page);
            model.Page = page;
            model.PageId = page.Id;
            return model;
        }
        public async Task<TaskTemplateViewModel> GetTaskViewModel(TaskTemplateViewModel taskTemplate)
        {
            var model = taskTemplate;
            model.RecordId = taskTemplate.Page.RecordId;
            if (taskTemplate.Page.RequestSource != RequestSourceEnum.Main
                && (taskTemplate.Page.RequestSource != RequestSourceEnum.Post || taskTemplate.Page.DataAction == DataActionEnum.Delete))
            {
                model = await _taskBusiness.GetTaskDetails(taskTemplate);
            }
            model.Page = taskTemplate.Page;
            model.PageId = taskTemplate.Page.Id;
            model.DataAction = taskTemplate.Page.DataAction;
            model.RecordId = taskTemplate.Page.RecordId;
            model.PortalName = taskTemplate.Page.Portal.Name;

            model.CustomUrl = taskTemplate.CustomUrl;
            model.ReturnUrl = taskTemplate.ReturnUrl;
            model.LayoutMode = taskTemplate.LayoutMode;
            model.PopupCallbackMethod = taskTemplate.PopupCallbackMethod;
            model.ViewType = taskTemplate.Page.ViewType;
            model.ViewMode = taskTemplate.Page.ViewMode;

            return model;
        }


        [HttpPost]
        public async Task<IActionResult> ManageTask(TaskTemplateViewModel model)
        {
            var page = await _pageBusiness.GetPageForExecution(model.PageId);
            if (page != null)
            {
                page.RecordId = model.RecordId;
                page.DataAction = model.DataAction;
                page.RequestSource = RequestSourceEnum.Post;
                model.Page = page;
                return await GetTask(model);
            }
            return Json(new { success = false, error = "Page not found" });

            //var result = await _cmsBusiness.ManageTask(model);
            //return Redirect($"~/Portal/{model.PortalName}");
            ////var template = await _templateBusiness.GetSingleById(model.TemplateId);
            ////var tabledata = await _tableDataBusiness.GetSingleById(template.TableMetadataId);
            ////tabledata.ColumnMetadataView = new List<ColumnMetadataViewModel>();
            ////tabledata.ColumnMetadataView = await _columnDataBusiness.GetList(x => x.TableMetadataId == template.TableMetadataId);

            ////var jsonResult = JObject.Parse(model.Json);
            ////foreach (var item in tabledata.ColumnMetadataView)
            ////{
            ////    var valObj = jsonResult.SelectToken(item.Name);
            ////    if (valObj != null)
            ////    {
            ////        item.Value = valObj;

            ////    }
            //}

        }

        #endregion

        #region Service

        private async Task<string> GetServiceModel(ServiceTemplateViewModel template, TemplateTypeEnum viewName)
        {
            if (template.Page.RecordId.IsNullOrEmpty())
            {
                return "{}";
            }
            var data = await _cmsBusiness.GetDataById(viewName, template.Page, template.Page.RecordId, template.IsLogRecord, template.LogId);
            if (data == null)
            {
                return "{}";
            }
            return data.ToJson();
        }

        //public async Task<IActionResult> LoadServiceIndexPageGrid([DataSourceRequest] DataSourceRequest request, NtsActiveUserTypeEnum ownerType, string indexPageTemplateId, string serviceStatusCode)
        //{
        //    var dt = await _serviceBusiness.GetServiceIndexPageGridData(request, indexPageTemplateId, ownerType, serviceStatusCode);
        //    return Json(dt.ToDataSourceResult(request));
        //}
        public async Task<IActionResult> LoadServiceIndexPageGrid(NtsActiveUserTypeEnum ownerType, string indexPageTemplateId, string serviceStatusCode)
        {
            var dt = await _serviceBusiness.GetServiceIndexPageGridData(null, indexPageTemplateId, ownerType, serviceStatusCode);
            return Json(dt);
        }
        public async Task<ServiceIndexPageTemplateViewModel> GetServiceIndexPageViewModel(PageViewModel page)
        {
            var model = await _serviceBusiness.GetServiceIndexPageViewModel(page);
            model.Page = page;
            model.PageId = page.Id;
            return model;

        }
        public async Task<ServiceTemplateViewModel> GetServiceViewModel(ServiceTemplateViewModel serviceTemplate)
        {
            var model = serviceTemplate;
            model.RecordId = serviceTemplate.Page.RecordId;
            if (serviceTemplate.Page.RequestSource != RequestSourceEnum.Main
                && (serviceTemplate.Page.RequestSource != RequestSourceEnum.Post || serviceTemplate.Page.DataAction == DataActionEnum.Delete))
            {
                model = await _serviceBusiness.GetServiceDetails(serviceTemplate);
            }
            model.Page = serviceTemplate.Page;
            model.PageId = serviceTemplate.Page.Id;
            model.DataAction = serviceTemplate.Page.DataAction;
            model.RecordId = serviceTemplate.Page.RecordId;
            model.PortalName = serviceTemplate.Page.Portal.Name;
            model.CustomUrl = serviceTemplate.CustomUrl;
            model.ReturnUrl = serviceTemplate.ReturnUrl;
            model.LayoutMode = serviceTemplate.LayoutMode;
            model.PopupCallbackMethod = serviceTemplate.PopupCallbackMethod;
            model.ViewMode = serviceTemplate.Page.ViewMode;

            return model;

        }


        [HttpPost]
        public async Task<IActionResult> ManageService(ServiceTemplateViewModel model)
        {

            var page = await _pageBusiness.GetPageForExecution(model.PageId);
            if (page != null)
            {
                page.DataAction = model.DataAction;
                page.RecordId = model.RecordId;
                page.RequestSource = RequestSourceEnum.Post;
                model.Page = page;
                return await GetService(model);
            }
            return Json(new { success = false, error = "Page not found" });

            //var result = await _cmsBusiness.ManageService(model);
            //return Redirect($"~/Portal/{model.PortalName}");
            //var template = await _templateBusiness.GetSingleById(model.TemplateId);
            //var tabledata = await _tableDataBusiness.GetSingleById(template.TableMetadataId);
            //tabledata.ColumnMetadataView = new List<ColumnMetadataViewModel>();
            //tabledata.ColumnMetadataView = await _columnDataBusiness.GetList(x => x.TableMetadataId == template.TableMetadataId);

            //var jsonResult = JObject.Parse(model.Json);
            //foreach (var item in tabledata.ColumnMetadataView)
            //{
            //    var valObj = jsonResult.SelectToken(item.Name);
            //    if (valObj != null)
            //    {
            //        item.Value = valObj;

            //    }
            //}

        }
        private async Task<IActionResult> GetService(ServiceTemplateViewModel serviceTemplate)
        {
            serviceTemplate = await GetServiceViewModel(serviceTemplate);
            if (serviceTemplate.Page.RequestSource == RequestSourceEnum.Post)
            {
                if (serviceTemplate.Page.DataAction == DataActionEnum.Create)
                {

                    var create = await _serviceBusiness.ManageService(serviceTemplate);
                    serviceTemplate.CustomMessageOnCreation = Helper.DynamicValueBind(serviceTemplate.CustomMessageOnCreation, serviceTemplate);
                    if (create.IsSuccess)
                    {
                        var msg = "Service created successfully.";
                        if (serviceTemplate.CreateReturnType == CreateReturnTypeEnum.ReloadDataInEditMode)
                        {
                            if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Popup)
                            {
                                return Json(new
                                {
                                    success = true,
                                    msg = msg,
                                    vm = serviceTemplate,
                                    mode = Convert.ToString(serviceTemplate.LayoutMode),
                                    cbm = serviceTemplate.PopupCallbackMethod,
                                    openMsgPopup = serviceTemplate.EnableCustomMessageOnCreation,
                                    customMsg = serviceTemplate.CustomMessageOnCreation
                                });
                            }
                            else if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Iframe)
                            {
                                return Json(new
                                {
                                    success = true,
                                    msg = msg,
                                    vm = serviceTemplate,
                                    mode = Convert.ToString(serviceTemplate.LayoutMode),
                                    cbm = serviceTemplate.PopupCallbackMethod,
                                    openMsgPopup = serviceTemplate.EnableCustomMessageOnCreation,
                                    customMsg = serviceTemplate.CustomMessageOnCreation
                                });
                            }
                            else if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Div)
                            {
                                return Json(new
                                {
                                    success = true,
                                    msg = msg,
                                    vm = serviceTemplate,
                                    mode = Convert.ToString(serviceTemplate.LayoutMode),
                                    cbm = serviceTemplate.PopupCallbackMethod,
                                    openMsgPopup = serviceTemplate.EnableCustomMessageOnCreation,
                                    customMsg = serviceTemplate.CustomMessageOnCreation
                                });
                            }
                            else if (serviceTemplate.ReturnUrl.IsNotNullAndNotEmpty())
                            {
                                return Json(new
                                {
                                    success = true,
                                    msg = msg,
                                    ru = serviceTemplate.ReturnUrl,
                                    mode = Convert.ToString(serviceTemplate.LayoutMode),
                                    openMsgPopup = serviceTemplate.EnableCustomMessageOnCreation,
                                    customMsg = serviceTemplate.CustomMessageOnCreation
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    success = true,
                                    msg = msg,
                                    reload = true,
                                    pageId = serviceTemplate.Page.Id,
                                    pageType = serviceTemplate.Page.PageType.ToString(),
                                    source = RequestSourceEnum.Edit.ToString(),
                                    dataAction = DataActionEnum.Edit.ToString(),
                                    recordId = create.Item.ServiceId,
                                    mode = Convert.ToString(serviceTemplate.LayoutMode),
                                    openMsgPopup = serviceTemplate.EnableCustomMessageOnCreation,
                                    customMsg = serviceTemplate.CustomMessageOnCreation
                                });
                            }

                        }
                        else
                        {
                            if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Popup)
                            {
                                return Json(new
                                {
                                    success = true,
                                    msg = msg,
                                    vm = serviceTemplate,
                                    mode = Convert.ToString(serviceTemplate.LayoutMode),
                                    cbm = serviceTemplate.PopupCallbackMethod,
                                    openMsgPopup = serviceTemplate.EnableCustomMessageOnCreation,
                                    customMsg = serviceTemplate.CustomMessageOnCreation
                                });
                            }
                            else if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Iframe)
                            {
                                return Json(new
                                {
                                    success = true,
                                    msg = msg,
                                    vm = serviceTemplate,
                                    mode = Convert.ToString(serviceTemplate.LayoutMode),
                                    cbm = serviceTemplate.PopupCallbackMethod,
                                    openMsgPopup = serviceTemplate.EnableCustomMessageOnCreation,
                                    customMsg = serviceTemplate.CustomMessageOnCreation
                                });
                            }
                            else if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Div)
                            {
                                return Json(new
                                {
                                    success = true,
                                    msg = msg,
                                    vm = serviceTemplate,
                                    mode = Convert.ToString(serviceTemplate.LayoutMode),
                                    cbm = serviceTemplate.PopupCallbackMethod,
                                    openMsgPopup = serviceTemplate.EnableCustomMessageOnCreation,
                                    customMsg = serviceTemplate.CustomMessageOnCreation
                                });
                            }
                            else if (serviceTemplate.ReturnUrl.IsNotNullAndNotEmpty())
                            {
                                return Json(new
                                {
                                    success = true,
                                    msg = msg,
                                    uiJson = serviceTemplate.Json,
                                    ru = serviceTemplate.ReturnUrl,
                                    mode = Convert.ToString(serviceTemplate.LayoutMode),
                                    openMsgPopup = serviceTemplate.EnableCustomMessageOnCreation,
                                    customMsg = serviceTemplate.CustomMessageOnCreation
                                });
                            }
                            else
                            {

                                return Json(new
                                {
                                    success = true,
                                    msg = msg,
                                    reload = true,
                                    pageId = serviceTemplate.Page.Id,
                                    pageType = serviceTemplate.Page.PageType.ToString(),
                                    source = RequestSourceEnum.Main.ToString(),
                                    dataAction = DataActionEnum.None.ToString(),
                                    recordId = create.Item.ServiceId,
                                    mode = Convert.ToString(serviceTemplate.LayoutMode),
                                    openMsgPopup = serviceTemplate.EnableCustomMessageOnCreation,
                                    customMsg = serviceTemplate.CustomMessageOnCreation
                                });
                            }
                        }
                    }
                    else
                    {
                        return Json(new { success = false, error = create.HtmlError });
                    }
                }
                else if (serviceTemplate.Page.DataAction == DataActionEnum.Edit)
                {

                    var edit = await _serviceBusiness.ManageService(serviceTemplate);
                    serviceTemplate.CustomMessageOnEdit = Helper.DynamicValueBind(serviceTemplate.CustomMessageOnEdit, serviceTemplate);
                    if (edit.IsSuccess)
                    {
                        var msg = "Item updated successfully.";
                        if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Popup)
                        {
                            return Json(new
                            {
                                success = true,
                                msg = msg,
                                vm = serviceTemplate,
                                mode = Convert.ToString(serviceTemplate.LayoutMode),
                                cbm = serviceTemplate.PopupCallbackMethod,
                                openMsgPopup = serviceTemplate.EnableCustomMessageOnEdit,
                                customMsg = serviceTemplate.CustomMessageOnEdit
                            });
                        }
                        else if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Iframe)
                        {
                            return Json(new
                            {
                                success = true,
                                msg = msg,
                                vm = serviceTemplate,
                                mode = Convert.ToString(serviceTemplate.LayoutMode),
                                cbm = serviceTemplate.PopupCallbackMethod,
                                openMsgPopup = serviceTemplate.EnableCustomMessageOnEdit,
                                customMsg = serviceTemplate.CustomMessageOnEdit
                            });
                        }
                        else if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Div)
                        {
                            return Json(new
                            {
                                success = true,
                                msg = msg,
                                vm = serviceTemplate,
                                mode = Convert.ToString(serviceTemplate.LayoutMode),
                                cbm = serviceTemplate.PopupCallbackMethod,
                                openMsgPopup = serviceTemplate.EnableCustomMessageOnEdit,
                                customMsg = serviceTemplate.CustomMessageOnEdit
                            });
                        }
                        else if (serviceTemplate.ReturnUrl.IsNotNullAndNotEmpty())
                        {
                            return Json(new
                            {
                                success = true,
                                msg = msg,
                                ru = serviceTemplate.ReturnUrl,
                                mode = Convert.ToString(serviceTemplate.LayoutMode),
                                openMsgPopup = serviceTemplate.EnableCustomMessageOnEdit,
                                customMsg = serviceTemplate.CustomMessageOnEdit
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                success = true,
                                msg = msg,
                                reload = true,
                                pageId = serviceTemplate.Page.Id,
                                pageType = serviceTemplate.Page.PageType.ToString(),
                                source = RequestSourceEnum.Edit.ToString(),
                                dataAction = DataActionEnum.Edit.ToString(),
                                recordId = edit.Item.ServiceId,
                                mode = Convert.ToString(serviceTemplate.LayoutMode),
                                openMsgPopup = serviceTemplate.EnableCustomMessageOnEdit,
                                customMsg = serviceTemplate.CustomMessageOnEdit
                            });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, error = edit.HtmlError });
                    }
                }
                else if (serviceTemplate.Page.DataAction == DataActionEnum.Delete)
                {
                    serviceTemplate.UdfTableMetadataId = serviceTemplate.Page.Template.UdfTableMetadataId;
                    var delete = await _serviceBusiness.DeleteService(serviceTemplate);
                    var viewName = TemplateTypeEnum.ServiceIndexPage;
                    var indexViewModel = await GetServiceIndexPageViewModel(serviceTemplate.Page);
                    var viewStr = await RenderViewToStringAsync(viewName.ToString(), indexViewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                    return Json(new { msg = "Selected item deleted successfully.", view = viewStr, uiJson = serviceTemplate.Page.Template.Json, dataJson = "{}", mode = Convert.ToString(serviceTemplate.LayoutMode) });
                }
                else
                {
                    return Json(new { success = false, error = "Invalid action" });
                }
            }
            else
            {
                if (serviceTemplate.Page.RequestSource == RequestSourceEnum.Main && serviceTemplate.EnableIndexPage)
                {
                    var viewName = TemplateTypeEnum.ServiceIndexPage;
                    var indexViewModel = await GetServiceIndexPageViewModel(serviceTemplate.Page);
                    var viewStr = await RenderViewToStringAsync(viewName.ToString(), indexViewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                    return Json(new { view = viewStr, uiJson = serviceTemplate.Json, dataJson = "{}", bc = serviceTemplate.Page.Breadcrumbs, mode = Convert.ToString(serviceTemplate.LayoutMode), page = serviceTemplate.Page });
                }
                else
                {
                    var viewName = serviceTemplate.Page.PageType.ToString();

                    if (serviceTemplate.ServiceStatusCode != "SERVICE_STATUS_DRAFT")
                    {
                        if (serviceTemplate.Page.RequestSource == RequestSourceEnum.Versioning)
                        {
                            serviceTemplate.IsVersioning = true;
                            serviceTemplate.VersionNo += 1;
                        }
                    }
                    if (serviceTemplate.Page.RequestSource == RequestSourceEnum.View)
                    {
                        serviceTemplate.Json = SetUdfView(serviceTemplate.Json);
                    }
                    serviceTemplate.DataJson = await GetServiceModel(serviceTemplate, serviceTemplate.Page.PageType);
                    if (serviceTemplate.DataJson.IsNotNull())
                    {
                        serviceTemplate.DataJson = await AttachmentConfiguration(serviceTemplate.Page.Template.Json, serviceTemplate.DataJson);
                    }
                    if (serviceTemplate.Json.IsNotNullAndNotEmpty())
                    {
                        serviceTemplate.Json = serviceTemplate.Json.Replace("SaveFileInFormIo", "SaveFileInFormIo?referenceId=" + serviceTemplate.ServiceId + "&referenceType=" + ReferenceTypeEnum.NTS_Service);

                        serviceTemplate.Json = serviceTemplate.Page.Template.Json = _webApi.AddHost(serviceTemplate.Json);
                        ReplaceFormIoJsonDataForService(serviceTemplate);
                    }
                    if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Popup)
                    {
                        if (serviceTemplate.ViewType == NtsViewTypeEnum.Light)
                        {
                            serviceTemplate.ComponentResultList = await GetServiceComponentList(serviceTemplate);
                        }

                        serviceTemplate.ChartItems = await BuildChart(serviceTemplate.Json, serviceTemplate.DataJson, serviceTemplate.Prms, serviceTemplate.ServiceId, serviceTemplate.ServiceStatusCode);
                        ViewBag.Layout = $"~/Views/Shared/Themes/{serviceTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";

                        return View(viewName, serviceTemplate);
                    }
                    else if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Iframe)
                    {
                        serviceTemplate.ChartItems = await BuildChart(serviceTemplate.Json, serviceTemplate.DataJson, serviceTemplate.Prms, serviceTemplate.ServiceId, serviceTemplate.ServiceStatusCode);
                        ViewBag.Layout = $"~/Views/Shared/Themes/{serviceTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                        return View(viewName, serviceTemplate);
                    }
                    else if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Div)
                    {
                        var viewStr = await RenderViewToStringAsync(viewName, serviceTemplate, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { view = viewStr, uiJson = serviceTemplate.Json, dataJson = serviceTemplate.DataJson, bc = serviceTemplate.Page.Breadcrumbs, mode = Convert.ToString(serviceTemplate.LayoutMode), page = serviceTemplate.Page, am = false });
                    }
                    else
                    {
                        serviceTemplate.ChartItems = await BuildChart(serviceTemplate.Json, serviceTemplate.DataJson, serviceTemplate.Prms, serviceTemplate.ServiceId, serviceTemplate.ServiceStatusCode);
                        var viewStr = await RenderViewToStringAsync(viewName, serviceTemplate, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { view = viewStr, uiJson = serviceTemplate.Json, dataJson = serviceTemplate.DataJson, bc = serviceTemplate.Page.Breadcrumbs, mode = Convert.ToString(serviceTemplate.LayoutMode), page = serviceTemplate.Page, am = true });
                    }

                }
            }
        }

        private async Task<List<ComponentResultViewModel>> GetServiceComponentList(ServiceTemplateViewModel serviceTemplate)
        {
            var list = await _componentResultBusiness.GetComponentResultList(serviceTemplate.ServiceId);
            if (list == null)
            {
                list = new List<ComponentResultViewModel>();
            }
            list = list.Where(x => x.ComponentType == ProcessDesignComponentTypeEnum.StepTask).ToList();
            var serviceItem = new ComponentResultViewModel
            {
                ComponentType = ProcessDesignComponentTypeEnum.Service,
                TaskId = serviceTemplate.ServiceId,
                TaskNo = serviceTemplate.ServiceNo,
                TaskSubject = serviceTemplate.ServiceSubject,
                ComponentStatusName = serviceTemplate.ServiceStatusName,
                AssigneeId = serviceTemplate.OwnerUserId,
                Assignee = serviceTemplate.OwnerUserName,
                StartDate = serviceTemplate.StartDate,
                EndDate = serviceTemplate.DueDate,
                ComponentStatusCode = serviceTemplate.ServiceStatusCode,
                SequenceOrder = -1
            };
            list.Insert(0, serviceItem);
            return list;
        }

        #endregion

        #region Note
        [HttpPost]
        public async Task<IActionResult> ManageNote(NoteTemplateViewModel model)
        {
            var page = await _pageBusiness.GetPageForExecution(model.PageId);
            if (page != null)
            {
                page.RecordId = model.RecordId;
                page.DataAction = model.DataAction;
                page.RequestSource = RequestSourceEnum.Post;
                model.Page = page;
                return await GetNote(model);
            }
            return Json(new { success = false, error = "Page not found" });

        }
        //public async Task<IActionResult> LoadNoteIndexPageGrid([DataSourceRequest] DataSourceRequest request, string indexPageTemplateId, NtsActiveUserTypeEnum ownerType, string noteStatusCode)
        //{
        //    var dt = await _noteBusiness.GetNoteIndexPageGridData(request, indexPageTemplateId, ownerType, noteStatusCode);
        //    return Json(dt.ToDataSourceResult(request));
        //}
        public async Task<IActionResult> LoadNoteIndexPageGrid(string indexPageTemplateId, NtsActiveUserTypeEnum ownerType, string noteStatusCode)
        {
            var dt = await _noteBusiness.GetNoteIndexPageGridData(null, indexPageTemplateId, ownerType, noteStatusCode);
            return Json(dt);
        }
        private async Task<IActionResult> GetNote(NoteTemplateViewModel noteTemplate)
        {
            noteTemplate = await GetNoteViewModel(noteTemplate);

            if (noteTemplate.Page.RequestSource == RequestSourceEnum.Post)
            {
                if (noteTemplate.Page.DataAction == DataActionEnum.Create)
                {
                    var create = await _noteBusiness.ManageNote(noteTemplate);
                    if (create.IsSuccess)
                    {
                        var msg = "Item created successfully.";
                        if (noteTemplate.CreateReturnType == CreateReturnTypeEnum.ReloadDataInEditMode)
                        {
                            if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Popup)
                            {
                                return Json(new { success = true, msg = msg, vm = noteTemplate, mode = Convert.ToString(noteTemplate.LayoutMode), cbm = noteTemplate.PopupCallbackMethod });
                            }
                            else if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Iframe)
                            {
                                return Json(new { success = true, msg = msg, vm = noteTemplate, mode = Convert.ToString(noteTemplate.LayoutMode), cbm = noteTemplate.PopupCallbackMethod });
                            }
                            else if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Div)
                            {
                                return Json(new { success = true, msg = msg, vm = noteTemplate, mode = Convert.ToString(noteTemplate.LayoutMode), cbm = noteTemplate.PopupCallbackMethod });
                            }
                            else if (noteTemplate.ReturnUrl.IsNotNullAndNotEmpty())
                            {
                                return Json(new { success = true, msg = msg, uiJson = noteTemplate.Json, ru = noteTemplate.ReturnUrl, mode = Convert.ToString(noteTemplate.LayoutMode) });
                            }
                            else
                            {
                                return Json(new
                                {
                                    success = true,
                                    msg = msg,
                                    reload = true,
                                    pageId = noteTemplate.Page.Id,
                                    pageType = noteTemplate.Page.PageType.ToString(),
                                    source = RequestSourceEnum.Edit.ToString(),
                                    dataAction = DataActionEnum.Edit.ToString(),
                                    recordId = create.Item.NoteId,
                                    mode = Convert.ToString(noteTemplate.LayoutMode)
                                });
                            }
                        }
                        else
                        {
                            if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Popup)
                            {
                                return Json(new { success = true, msg = msg, vm = noteTemplate, mode = Convert.ToString(noteTemplate.LayoutMode), cbm = noteTemplate.PopupCallbackMethod });
                            }
                            else if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Iframe)
                            {
                                return Json(new { success = true, msg = msg, vm = noteTemplate, mode = Convert.ToString(noteTemplate.LayoutMode), cbm = noteTemplate.PopupCallbackMethod });
                            }
                            else if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Div)
                            {
                                return Json(new { success = true, msg = msg, vm = noteTemplate, mode = Convert.ToString(noteTemplate.LayoutMode), cbm = noteTemplate.PopupCallbackMethod });
                            }
                            else if (noteTemplate.ReturnUrl.IsNotNullAndNotEmpty())
                            {
                                return Json(new { success = true, msg = msg, uiJson = noteTemplate.Json, ru = noteTemplate.ReturnUrl, mode = Convert.ToString(noteTemplate.LayoutMode) });
                            }
                            else
                            {
                                return Json(new
                                {
                                    success = true,
                                    msg = msg,
                                    reload = true,
                                    pageId = noteTemplate.Page.Id,
                                    pageType = noteTemplate.Page.PageType.ToString(),
                                    source = RequestSourceEnum.Main.ToString(),
                                    dataAction = DataActionEnum.None.ToString(),
                                    recordId = create.Item.NoteId,
                                    mode = Convert.ToString(noteTemplate.LayoutMode)
                                });
                            }
                        }
                    }
                    else
                    {
                        return Json(new { success = false, error = create.HtmlError });
                    }
                }
                else if (noteTemplate.Page.DataAction == DataActionEnum.Edit)
                {
                    var edit = await _noteBusiness.ManageNote(noteTemplate);
                    if (edit.IsSuccess)
                    {
                        var msg = "Item updated successfully.";
                        if (noteTemplate.EditReturnType == EditReturnTypeEnum.ReloadDataInEditMode)
                        {

                            if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Popup)
                            {
                                return Json(new { success = true, msg = msg, vm = noteTemplate, mode = Convert.ToString(noteTemplate.LayoutMode), cbm = noteTemplate.PopupCallbackMethod });
                            }
                            else if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Iframe)
                            {
                                return Json(new { success = true, msg = msg, vm = noteTemplate, mode = Convert.ToString(noteTemplate.LayoutMode), cbm = noteTemplate.PopupCallbackMethod });
                            }
                            else if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Div)
                            {
                                return Json(new { success = true, msg = msg, vm = noteTemplate, mode = Convert.ToString(noteTemplate.LayoutMode), cbm = noteTemplate.PopupCallbackMethod });
                            }
                            else if (noteTemplate.Page.ReturnUrl.IsNotNullAndNotEmpty())
                            {
                                return Json(new { success = true, msg = msg, uiJson = noteTemplate.Json, ru = noteTemplate.Page.ReturnUrl, mode = Convert.ToString(noteTemplate.LayoutMode) });
                            }
                            else
                            {
                                var model = await _noteBusiness.GetNoteDetails(noteTemplate);
                                model.RecordId = noteTemplate.Page.RecordId;

                                model.Page = noteTemplate.Page;
                                model.PageId = noteTemplate.Page.Id;
                                model.DataAction = noteTemplate.Page.DataAction;
                                model.NoteId = model.RecordId = noteTemplate.Page.RecordId;
                                model.PortalName = noteTemplate.Page.Portal.Name;


                                var vn0 = noteTemplate.Page.PageType;
                                model.Page.RecordId = noteTemplate.RecordId = edit.Item.RecordId;
                                model.Page.DataAction = noteTemplate.DataAction = DataActionEnum.Edit;
                                model.Page.RequestSource = RequestSourceEnum.Edit;
                                var data0 = await GetNoteModel(model, vn0);
                                var vs1 = await RenderViewToStringAsync(model.Page.PageType.ToString(), model, _contextAccessor, _razorViewEngine, _tempDataProvider);
                                if (model.Json.IsNotNullAndNotEmpty())
                                {
                                    model.Json = _webApi.AddHost(model.Json);
                                }
                                return Json(new { success = true, msg = msg, view = vs1, uiJson = model.Json, dataJson = data0, ru = model.Page.ReturnUrl, mode = Convert.ToString(noteTemplate.LayoutMode) });
                            }

                        }
                        else
                        {
                            if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Popup)
                            {
                                return Json(new { success = true, msg = msg, vm = noteTemplate, mode = Convert.ToString(noteTemplate.LayoutMode), cbm = noteTemplate.PopupCallbackMethod });
                            }
                            else if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Iframe)
                            {
                                return Json(new { success = true, msg = msg, vm = noteTemplate, mode = Convert.ToString(noteTemplate.LayoutMode), cbm = noteTemplate.PopupCallbackMethod });
                            }
                            else if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Div)
                            {
                                return Json(new { success = true, msg = msg, vm = noteTemplate, mode = Convert.ToString(noteTemplate.LayoutMode), cbm = noteTemplate.PopupCallbackMethod });
                            }
                            else if (noteTemplate.Page.ReturnUrl.IsNotNullAndNotEmpty())
                            {
                                return Json(new { success = true, msg = msg, ru = noteTemplate.Page.ReturnUrl, mode = Convert.ToString(noteTemplate.LayoutMode) });
                            }
                            else
                            {
                                var ivm = await GetNoteIndexPageViewModel(noteTemplate.Page);
                                var vs = await RenderViewToStringAsync(TemplateTypeEnum.TaskIndexPage.ToString(), ivm, _contextAccessor, _razorViewEngine, _tempDataProvider);
                                if (noteTemplate.Json.IsNotNullAndNotEmpty())
                                {
                                    noteTemplate.Json = noteTemplate.Json.Replace("SaveFileInFormIo", "SaveFileInFormIo?referenceId=" + noteTemplate.NoteId + "&referenceType=" + ReferenceTypeEnum.NTS_Note);

                                    noteTemplate.Json = _webApi.AddHost(noteTemplate.Json);
                                    ReplaceFormIoJsonDataForNote(noteTemplate);
                                }
                                return Json(new { success = true, msg = msg, view = vs, uiJson = noteTemplate.Json, dataJson = "{}", mode = Convert.ToString(noteTemplate.LayoutMode) });
                            }

                        }
                    }
                    else
                    {
                        return Json(new { success = false, error = edit.HtmlError });
                    }
                }
                else if (noteTemplate.Page.DataAction == DataActionEnum.Delete)
                {
                    noteTemplate.TableMetadataId = noteTemplate.Page.Template.TableMetadataId;
                    var delete = await _noteBusiness.DeleteNote(noteTemplate);
                    var viewName = TemplateTypeEnum.NoteIndexPage;
                    var indexViewModel = await GetNoteIndexPageViewModel(noteTemplate.Page);
                    var viewStr = await RenderViewToStringAsync(viewName.ToString(), indexViewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                    if (noteTemplate.Json.IsNotNullAndNotEmpty())
                    {
                        noteTemplate.Json = noteTemplate.Json.Replace("SaveFileInFormIo", "SaveFileInFormIo?referenceId=" + noteTemplate.NoteId + "&referenceType=" + ReferenceTypeEnum.NTS_Note);
                        noteTemplate.Json = _webApi.AddHost(noteTemplate.Json);
                        ReplaceFormIoJsonDataForNote(noteTemplate);
                    }
                    return Json(new { msg = "Selected item deleted successfully.", view = viewStr, uiJson = noteTemplate.Json, dataJson = "{}", mode = Convert.ToString(noteTemplate.LayoutMode) });

                }
                else
                {
                    return Json(new { success = false, error = "Invalid action" });
                }
            }
            else
            {

                if (noteTemplate.Page.RequestSource == RequestSourceEnum.Main && noteTemplate.EnableIndexPage)
                {
                    var viewName = TemplateTypeEnum.NoteIndexPage;
                    var indexViewModel = await GetNoteIndexPageViewModel(noteTemplate.Page);
                    var viewStr = await RenderViewToStringAsync(viewName.ToString(), indexViewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                    noteTemplate.DataJson = await GetNoteModel(noteTemplate, viewName);
                    if (noteTemplate.Json.IsNotNullAndNotEmpty())
                    {
                        noteTemplate.Json = noteTemplate.Json.Replace("SaveFileInFormIo", "SaveFileInFormIo?referenceId=" + noteTemplate.NoteId + "&referenceType=" + ReferenceTypeEnum.NTS_Note);
                        noteTemplate.Page.Template.Json = noteTemplate.Json = _webApi.AddHost(noteTemplate.Json);
                        ReplaceFormIoJsonDataForNote(noteTemplate);
                    }
                    return Json(new { view = viewStr, uiJson = noteTemplate.Json, dataJson = "{}", bc = noteTemplate.Page.Breadcrumbs, mode = Convert.ToString(noteTemplate.LayoutMode), page = noteTemplate.Page });
                }
                else
                {
                    var viewName = noteTemplate.Page.PageType;
                    if (noteTemplate.NoteStatusCode != "NOTE_STATUS_DRAFT")
                    {
                        if (noteTemplate.Page.RequestSource == RequestSourceEnum.Versioning)
                        {
                            noteTemplate.IsVersioning = true;
                            noteTemplate.VersionNo += 1;
                        }
                    }
                    if (noteTemplate.Page.RequestSource == RequestSourceEnum.View)
                    {
                        noteTemplate.Json = SetUdfView(noteTemplate.Json);
                    }

                    noteTemplate.DataJson = await GetNoteModel(noteTemplate, viewName);
                    if (noteTemplate.Json.IsNotNullAndNotEmpty())
                    {
                        noteTemplate.Json = noteTemplate.Json.Replace("SaveFileInFormIo", "SaveFileInFormIo?referenceId=" + noteTemplate.NoteId + "&referenceType=" + ReferenceTypeEnum.NTS_Note);
                        noteTemplate.Page.Template.Json = noteTemplate.Json = _webApi.AddHost(noteTemplate.Json);
                        ReplaceFormIoJsonDataForNote(noteTemplate);
                    }
                    if (noteTemplate.DataJson.IsNotNull())
                    {
                        noteTemplate.DataJson = await AttachmentConfiguration(noteTemplate.Page.Template.Json, noteTemplate.DataJson);
                    }


                    if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Popup)
                    {
                        noteTemplate.ChartItems = await BuildChart(noteTemplate.Json, noteTemplate.DataJson, noteTemplate.Prms, noteTemplate.NoteId, noteTemplate.NoteStatusCode);
                        ViewBag.Layout = $"~/Views/Shared/Themes/{noteTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                        return View("Note", noteTemplate);
                    }
                    else if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Iframe)
                    {
                        noteTemplate.ChartItems = await BuildChart(noteTemplate.Json, noteTemplate.DataJson, noteTemplate.Prms, noteTemplate.NoteId, noteTemplate.NoteStatusCode);
                        ViewBag.Layout = $"~/Views/Shared/Themes/{noteTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                        return View("Note", noteTemplate);
                    }
                    else if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Div)
                    {
                        var viewStr = await RenderViewToStringAsync(viewName.ToString(), noteTemplate, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { view = viewStr, uiJson = noteTemplate.Json, dataJson = noteTemplate.DataJson, bc = noteTemplate.Page.Breadcrumbs, mode = Convert.ToString(noteTemplate.LayoutMode), page = noteTemplate.Page, am = false });
                    }
                    else
                    {
                        var viewStr = await RenderViewToStringAsync(viewName.ToString(), noteTemplate, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { view = viewStr, uiJson = noteTemplate.Json, dataJson = noteTemplate.DataJson, bc = noteTemplate.Page.Breadcrumbs, mode = Convert.ToString(noteTemplate.LayoutMode), page = noteTemplate.Page, am = true });
                    }
                }
            }
        }
        private void ReplaceFormIoJsonDataForNote(NoteTemplateViewModel noteTemplate)
        {
            if (noteTemplate.Json.Contains("{{viewmodel."))
            {
                var serializedModel = Newtonsoft.Json.JsonConvert.SerializeObject(noteTemplate);
                var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(serializedModel);
                foreach (var item in dict)
                {
                    var key = "{{viewmodel." + item.Key + "}}";
                    var value = item.Value != null ? item.Value.ToString() : null;
                    if (noteTemplate.Json.Contains(key))
                    {
                        noteTemplate.Json = noteTemplate.Json.Replace(key, value);
                    }

                }
            }
        }
        private void ReplaceFormIoJsonDataForTask(TaskTemplateViewModel teskTemplate)
        {
            if (teskTemplate.Json.Contains("{{viewmodel."))
            {
                var serializedModel = Newtonsoft.Json.JsonConvert.SerializeObject(teskTemplate);
                var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(serializedModel);
                foreach (var item in dict)
                {
                    var key = "{{viewmodel." + item.Key + "}}";
                    var value = item.Value != null ? item.Value.ToString() : null;
                    if (teskTemplate.Json.Contains(key))
                    {
                        teskTemplate.Json = teskTemplate.Json.Replace(key, value);
                    }

                }
            }
        }
        private void ReplaceFormIoJsonDataForService(ServiceTemplateViewModel serviceTemplate)
        {
            if (serviceTemplate.Json.Contains("{{viewmodel."))
            {
                var serializedModel = Newtonsoft.Json.JsonConvert.SerializeObject(serviceTemplate);
                var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(serializedModel);
                foreach (var item in dict)
                {
                    var key = "{{viewmodel." + item.Key + "}}";
                    var value = item.Value != null ? item.Value.ToString() : null;
                    if (serviceTemplate.Json.Contains(key))
                    {
                        serviceTemplate.Json = serviceTemplate.Json.Replace(key, value);
                    }

                }
            }
        }
        private void ReplaceFormIoJsonDataForPage(PageViewModel page)
        {
            if (page.Template.Json.Contains("{{viewmodel."))
            {
                var serializedModel = Newtonsoft.Json.JsonConvert.SerializeObject(page);
                var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(serializedModel);
                foreach (var item in dict)
                {
                    var key = "{{viewmodel." + item.Key + "}}";
                    var value = item.Value != null ? item.Value.ToString() : null;
                    if (page.Template.Json.Contains(key))
                    {
                        page.Template.Json = page.Template.Json.Replace(key, value);
                    }

                }
            }
        }
        private void ReplaceFormIoJsonDataForForm(FormTemplateViewModel formTemplate)
        {
            if (formTemplate.Json.Contains("{{viewmodel."))
            {
                var serializedModel = Newtonsoft.Json.JsonConvert.SerializeObject(formTemplate);
                var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(serializedModel);
                foreach (var item in dict)
                {
                    var key = "{{viewmodel." + item.Key + "}}";
                    var value = item.Value != null ? item.Value.ToString() : null;
                    if (formTemplate.Json.Contains(key))
                    {
                        formTemplate.Json = formTemplate.Json.Replace(key, value);
                    }

                }
            }
        }
        private async Task<string> AttachmentConfiguration(string templateJson, string dataJson)
        {
            if (dataJson.IsNotNull() && dataJson != "{}" && templateJson.IsNotNull())
            {
                var data = JToken.Parse(templateJson);
                JArray rows = (JArray)data.SelectToken("components");
                if (rows == null && data.IsNotNull())
                {
                    rows = (JArray)data.SelectToken("columns");
                    //rows = (JArray)col.SelectToken("components");
                }
                var index = 0;
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(dataJson);
                if (rows.IsNotNull())
                {
                    foreach (var row in rows)
                    {
                        if (row.SelectToken("storage") != null)
                        {
                            var key = row.SelectToken("key").ToString();
                            var dataJrow = JToken.Parse(dataJson);
                            var fileId = Convert.ToString(dataJrow.SelectToken(key));
                            if (fileId.IsNotNull() && fileId != "[]" && fileId != "")
                            {
                                var fileDetails = await _fileBusiness.GetSingleById(fileId);
                                if (fileDetails != null)
                                {
                                    JArray jarrayObj = new JArray();
                                    var jObj = new JObject
                                {
                                    { "id", fileId },
                                    { "storage", "url" },
                                    { "name", fileDetails.FileName },
                                    { "size", fileDetails.ContentLength },
                                    { "type", fileDetails.ContentType },
                                    { "originalName", fileDetails.FileName },
                                    //{ "url", "/cms/Document/GetFileMongo?fileId="+ fileId}//pass url
                                    { "url", "/cms/PreviewAttachment?ntsType=NTS_Service&Id="+ fileId + "&canEdit=true"}//pass url
                                        //@Url.Action("PreviewAttachment", "Cms", new { @area = "" })?ntsType=NTS_Service&Id=' + parentId + '&canEdit=' + args.fileDetails[0].CanEditDocument

                                };
                                    jarrayObj.Add(jObj);
                                    jsonObj[key] = jarrayObj;
                                    index++;
                                }
                            }
                            else
                            {
                                jsonObj[key] = new JArray();
                                index++;
                            }
                        }

                        else if (row.SelectToken("type") != null && row.SelectToken("type").ToString() == "datagrid")
                        {
                            var key = row.SelectToken("key").ToString();
                            var dataJrow = JToken.Parse(dataJson);
                            var gridValues = dataJrow.SelectToken(key).ToString();
                            if (gridValues.IsNotNullAndNotEmpty())
                            {
                                var array = JArray.Parse(gridValues);
                                jsonObj[key] = array;
                                index++;
                            }
                        }

                        //var insideComponent = row[0].SelectToken("components");
                        //if(insideComponent.IsNotNull())
                        //{
                        else if (row.SelectToken("multiple") != null && (bool)row.SelectToken("multiple") && row.SelectToken("type").ToString() == "select")
                        {
                            var key = row.SelectToken("key").ToString();
                            var dataJrow = JToken.Parse(dataJson);
                            var str = dataJrow.SelectToken(key).ToString();
                            if (str.IsNotNullAndNotEmpty())
                            {
                                str = str.Replace("\r", "");
                                str = str.Replace("\n", "");
                                str = str.Replace("\"", "'");
                                var array = JArray.Parse(str);
                                jsonObj[key] = array;
                                index++;
                            }
                        }
                        var inJson = Newtonsoft.Json.JsonConvert.SerializeObject(row, Newtonsoft.Json.Formatting.Indented);
                        var jobject = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                        var res = await AttachmentConfiguration(inJson, jobject);
                        jsonObj = JsonConvert.DeserializeObject(res);
                        //}
                    }
                }
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                return output;
            }
            return dataJson;
        }
        private async Task<string> BuildChartJson(JArray comps, string dataJson, string chartCum, Dictionary<string, string> prms, string ntsId, string ntsStatusCode = null)
        {
            JToken nodes;
            foreach (JObject jcomp in comps)
            {
                var typeObj = jcomp.SelectToken("type");
                if (typeObj.IsNotNull())
                {
                    var type = typeObj.ToString();
                    if (type == "columns")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("columns");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                chartCum = await BuildChartJson(rows, dataJson, chartCum, prms, ntsId, ntsStatusCode);
                        }
                    }
                    else if (type == "tabs")
                    {

                        JArray cols = (JArray)jcomp.SelectToken("components");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                chartCum = await BuildChartJson(rows, dataJson, chartCum, prms, ntsId, ntsStatusCode);
                        }

                    }
                    else if (type == "table")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("rows");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                chartCum = await BuildChartJson(rows, dataJson, chartCum, prms, ntsId, ntsStatusCode);
                        }
                    }
                    else
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                        {
                            chartCum = await BuildChartJson(rows, dataJson, chartCum, prms, ntsId, ntsStatusCode);
                        }
                        else
                        {
                            nodes = jcomp["chartItemName"];
                            if (nodes == null)
                            {
                                continue;
                            }
                            var chartName = nodes.Value<string>();
                            var chartNamear = chartName.Split('-');
                            var dashboardItem = await _noteBusiness.GetDashboardItemDetailsByName(chartNamear[0]);
                            if (!dashboardItem.IsNotNull())
                            {
                                continue;
                            }
                            var layers = await _noteBusiness.GetMapLayerItemList(dashboardItem.NoteId);
                            string layersString = null;
                            if (layers.Count > 0)
                            {
                                layersString = JsonConvert.SerializeObject(layers);
                            }
                            var chartInput = dashboardItem.chartMetadata;
                            var clickFunction = dashboardItem.onChartClickFunction;
                            var clickFunctionName = "On" + chartNamear[0].Replace(" ", string.Empty) + "Click";
                            var filters = JsonConvert.DeserializeObject<List<DashboardItemFilterViewModel>>(dashboardItem.filterField);
                            var timeDimensions = new List<DashboardItemTimeDimensionViewModel>();
                            if (dashboardItem.timeDimensionsField.IsNotNullAndNotEmpty())
                            {
                                timeDimensions = JsonConvert.DeserializeObject<List<DashboardItemTimeDimensionViewModel>>(dashboardItem.timeDimensionsField);

                            }
                            if (prms.IsNotNull())
                            {
                                foreach (var pitem in prms)
                                {
                                    var pstr = "";
                                    var pvalue = pitem.Value.Split(',');
                                    for (int p = 0; p < pvalue.Count(); p++)
                                    {
                                        pstr += '"' + pvalue[p] + '"' + ',';
                                    }
                                    pstr = pstr.Trim(',');
                                    var pkey = '{' + pitem.Key + '}';
                                    chartInput = chartInput.Replace(pkey, pstr);
                                }
                            }
                            if (chartInput.Contains("^^NtsId^^"))
                            {

                                ntsId = '"' + ntsId + '"';
                                chartInput = chartInput.Replace("^^NtsId^^", ntsId);
                            }
                            if (chartInput.Contains("^^NtsStatusCode^^"))
                            {

                                ntsStatusCode = '"' + ntsStatusCode + '"';
                                chartInput = chartInput.Replace("^^NtsStatusCode^^", ntsStatusCode);
                            }
                            if (chartInput.Contains("^^") && dashboardItem.DynamicMetadata)
                            {
                                var metadataArray = chartInput.Split("],");
                                var dataJsontoken = dataJson.IsNotNullAndNotEmpty() ? JToken.Parse(dataJson) : null;
                                var filstr = "filters: [";
                                var timeDimsStr = "timeDimensions: [";
                                foreach (var filter in filters)
                                {
                                    var filterKey = filter.FilterText.Replace("^", string.Empty);
                                    var key = dataJsontoken.IsNotNull() ? dataJsontoken[filterKey] : null;
                                    var filterValue = key.IsNotNull() ? key.Value<string>() : "";
                                    if (filter.FilterText.Contains("^^") && filter.DefaultValue == "All" && filterValue.IsNullOrEmpty())
                                    {
                                        continue;
                                    }
                                    else if (filter.FilterText.Contains("^^") && filterValue.IsNotNullAndNotEmpty())
                                    {
                                        var value = "";
                                        if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                        {
                                            value = filterValue;
                                        }
                                        else
                                        {
                                            value = '"' + filterValue + '"';
                                        }
                                        var fil = "{\"member\": \"" + filter.FilterField + "\",\"operator\": \"" + filter.FilterOperator + "\",\"values\": [" + value + "]},";
                                        filstr += fil;
                                    }
                                    else if (filter.FilterText.Contains("^^") && filter.DefaultValue.IsNotNullAndNotEmpty())
                                    {
                                        var value = "";
                                        if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                        {
                                            value = filter.DefaultValue;
                                        }
                                        else
                                        {
                                            value = '"' + filter.DefaultValue + '"';
                                        }
                                        var fil = "{\"member\": \"" + filter.FilterField + "\",\"operator\": \"" + filter.FilterOperator + "\",\"values\": [" + value + "]},";
                                        filstr += fil;
                                    }

                                }
                                foreach (var timdim in timeDimensions)
                                {
                                    var filterKey = timdim.TimeParams.Replace("^", string.Empty);
                                    var key = dataJsontoken.IsNotNull() ? dataJsontoken[filterKey] : null;
                                    var filterValue = key.IsNotNull() ? key.Value<string>() : "";
                                    if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue == "All" && filterValue.IsNullOrEmpty())
                                    {
                                        continue;
                                    }
                                    else if (timdim.TimeParams.Contains("^^") && filterValue.IsNotNullAndNotEmpty())
                                    {
                                        var value = filterValue;
                                        var fil = "{\"dimension\": \"" + timdim.TimeDimensionField + "\",\"granularity\": \"" + timdim.RangeBy + "\",\"dateRange\": [" + value + "]},";
                                        timeDimsStr += fil;
                                    }
                                    else if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue.IsNotNullAndNotEmpty())
                                    {
                                        var value = timdim.DefaultValue;
                                        var fil = "{\"dimension\": \"" + timdim.TimeDimensionField + "\",\"granularity\": \"" + timdim.RangeBy + "\",\"dateRange\": [" + value + "]},";
                                        timeDimsStr += fil;
                                    }

                                }
                                filstr += "]";
                                timeDimsStr += "]";
                                chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + timeDimsStr;
                                chartInput = ReplaceChartSessionValue(chartInput);
                            }
                            else if (chartInput.Contains("^^"))
                            {
                                var dataJsontoken = dataJson.IsNotNullAndNotEmpty() ? JToken.Parse(dataJson) : null;
                                chartInput = ReplaceChartSessionValue(chartInput);
                                foreach (var filter in filters)
                                {
                                    var filterKey = filter.FilterText.Replace("^", string.Empty);
                                    var key = dataJsontoken.IsNotNull() ? dataJsontoken[filterKey] : null;
                                    var filterValue = key.IsNotNull() ? key.Value<string>() : "";
                                    if (filter.FilterText.Contains("^^") && filterValue.IsNotNullAndNotEmpty())
                                    {
                                        var value = "";
                                        if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                        {
                                            value = filterValue;
                                        }
                                        else
                                        {
                                            value = '"' + filterValue + '"';
                                        }
                                        chartInput = chartInput.Replace(filter.FilterText, value);

                                    }
                                    else if (filter.FilterText.Contains("^^") && filter.DefaultValue.IsNotNullAndNotEmpty())
                                    {
                                        var value = "";
                                        if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                        {
                                            value = filter.DefaultValue;
                                        }
                                        else
                                        {
                                            value = '"' + filter.DefaultValue + '"';
                                        }
                                        chartInput = chartInput.Replace(filter.FilterText, value);
                                    }
                                }
                                foreach (var timdim in timeDimensions)
                                {
                                    var filterKey = timdim.TimeParams.Replace("^", string.Empty);
                                    var key = dataJsontoken.IsNotNull() ? dataJsontoken[filterKey] : null;
                                    var filterValue = key.IsNotNull() ? key.Value<string>() : "";
                                    if (timdim.TimeParams.Contains("^^") && filterValue.IsNotNullAndNotEmpty())
                                    {
                                        var value = filterValue;
                                        chartInput = chartInput.Replace(timdim.TimeParams, value);
                                    }
                                    else if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue.IsNotNullAndNotEmpty())
                                    {
                                        var value = timdim.DefaultValue;
                                        chartInput = chartInput.Replace(timdim.TimeParams, value);
                                    }

                                }
                            }
                            if (chartNamear.Length > 1 && chartNamear[1].Contains("Iframe"))
                            {
                                chartInput = chartInput.Replace("\"", string.Empty);
                            }
                            var chartBPCode = dashboardItem.boilerplateCode;
                            chartBPCode = chartBPCode.Replace("@@input@@", chartInput);
                            chartBPCode = chartBPCode.Replace("@@xaxis@@", dashboardItem.Xaxis);
                            chartBPCode = chartBPCode.Replace("@@yaxis@@", dashboardItem.Yaxis);
                            chartBPCode = chartBPCode.Replace("@@Count@@", dashboardItem.Count);
                            chartBPCode = chartBPCode.Replace("@@DataSource@@", layersString);
                            chartBPCode = chartBPCode.Replace("@@chartid@@", "'" + dashboardItem.NoteSubject.Replace(" ", string.Empty) + "'");
                            chartBPCode = chartBPCode.Replace("@@chartHtml@@", dashboardItem.NoteDescription);
                            chartBPCode = chartBPCode.Replace("@@ctx@@", "'2d'");
                            chartBPCode = chartBPCode.Replace("@@mapUrl@@", dashboardItem.mapUrl);
                            chartBPCode = chartBPCode.Replace("@@mapLayer@@", dashboardItem.mapLayer);
                            chartBPCode = chartBPCode.Replace("@@mode@@", dashboardItem.ThemeMode);
                            chartBPCode = chartBPCode.Replace("@@palette@@", dashboardItem.Palette);
                            chartBPCode = chartBPCode.Replace("@@monochrome@@", dashboardItem.MonocromeColor.IsNotNullAndNotEmpty() ? "true" : "false");
                            chartBPCode = chartBPCode.Replace("@@color@@", dashboardItem.MonocromeColor);
                            chartBPCode = chartBPCode.Replace("OnSeriesClick", clickFunctionName);
                            if (clickFunction.IsNotNullAndNotEmpty())
                            {
                                clickFunction = clickFunction.Replace("OnSeriesClick", clickFunctionName);
                                chartBPCode += clickFunction;
                            }
                            dashboardItem.chartMetadata = chartBPCode;
                            chartCum = chartCum + " \n " + chartBPCode;


                        }
                    }
                }
            }
            return chartCum;
        }
        private string ReplaceChartSessionValue(string chartMetadata)
        {
            if (chartMetadata.Contains("^^UserId^^"))
            {
                chartMetadata = chartMetadata.Replace("^^UserId^^", '"' + _userContext.UserId + '"');
            }
            if (chartMetadata.Contains("^^UserUniqueId^^"))
            {
                chartMetadata = chartMetadata.Replace("^^UserUniqueId^^", '"' + _userContext.UserUniqueId + '"');
            }
            if (chartMetadata.Contains("^^Name^^"))
            {
                chartMetadata = chartMetadata.Replace("^^Name^^", '"' + _userContext.Name + '"');
            }
            if (chartMetadata.Contains("^^CompanyId^^"))
            {
                chartMetadata = chartMetadata.Replace("^^CompanyId^^", '"' + _userContext.CompanyId + '"');
            }
            if (chartMetadata.Contains("^^CompanyCode^^"))
            {
                chartMetadata = chartMetadata.Replace("^^CompanyCode^^", '"' + _userContext.CompanyCode + '"');
            }
            if (chartMetadata.Contains("^^CompanyName^^"))
            {
                chartMetadata = chartMetadata.Replace("^^CompanyName^^", '"' + _userContext.CompanyName + '"');
            }
            if (chartMetadata.Contains("^^Email^^"))
            {
                chartMetadata = chartMetadata.Replace("^^Email^^", '"' + _userContext.Email + '"');
            }
            if (chartMetadata.Contains("^^JobTitle^^"))
            {
                chartMetadata = chartMetadata.Replace("^^JobTitle^^", '"' + _userContext.JobTitle + '"');
            }
            if (chartMetadata.Contains("^^PhotoId^^"))
            {
                chartMetadata = chartMetadata.Replace("^^PhotoId^^", '"' + _userContext.PhotoId + '"');
            }
            if (chartMetadata.Contains("^^IsSystemAdmin^^"))
            {
                chartMetadata = chartMetadata.Replace("^^IsSystemAdmin^^", '"' + _userContext.IsSystemAdmin.ToString() + '"');
            }
            if (chartMetadata.Contains("^^IsGuestUser^^"))
            {
                chartMetadata = chartMetadata.Replace("^^IsGuestUser^^", '"' + _userContext.IsGuestUser.ToString() + '"');
            }
            if (chartMetadata.Contains("^^UserRoleIds^^"))
            {
                chartMetadata = chartMetadata.Replace("^^UserRoleIds^^", '"' + _userContext.UserRoleIds + '"');
            }
            if (chartMetadata.Contains("^^UserRoleCodes^^"))
            {
                chartMetadata = chartMetadata.Replace("^^UserRoleCodes^^", '"' + _userContext.UserRoleCodes + '"');
            }
            if (chartMetadata.Contains("^^PortalTheme^^"))
            {
                chartMetadata = chartMetadata.Replace("^^PortalTheme^^", '"' + _userContext.PortalTheme + '"');
            }
            if (chartMetadata.Contains("^^UserPortals^^"))
            {
                chartMetadata = chartMetadata.Replace("^^UserPortals^^", '"' + _userContext.UserPortals + '"');
            }
            if (chartMetadata.Contains("^^PortalId^^"))
            {
                chartMetadata = chartMetadata.Replace("^^PortalId^^", '"' + _userContext.PortalId + '"');
            }
            if (chartMetadata.Contains("^^CurrentDate^^"))
            {
                chartMetadata = chartMetadata.Replace("^^CurrentDate^^", '"' + _userContext.GetLocalNow.Date.ToString() + '"');
            }
            if (chartMetadata.Contains("^^CurrentDateTime^^"))
            {
                chartMetadata = chartMetadata.Replace("^^CurrentDateTime^^", '"' + _userContext.GetLocalNow.ToString() + '"');
            }
            if (chartMetadata.Contains("^^LoggedInAsType^^"))
            {
                chartMetadata = chartMetadata.Replace("^^LoggedInAsType^^", '"' + _userContext.LoggedInAsType.ToString() + '"');
            }
            if (chartMetadata.Contains("^^LoggedInAsByUserId^^"))
            {
                chartMetadata = chartMetadata.Replace("^^LoggedInAsByUserId^^", '"' + _userContext.LoggedInAsByUserId + '"');
            }
            if (chartMetadata.Contains("^^LoggedInAsByUserName^^"))
            {
                chartMetadata = chartMetadata.Replace("^^LoggedInAsByUserName^^", '"' + _userContext.LoggedInAsByUserName + '"');
            }
            if (chartMetadata.Contains("^^LegalEntityId^^"))
            {
                chartMetadata = chartMetadata.Replace("^^LegalEntityId^^", '"' + _userContext.LegalEntityId + '"');
            }
            if (chartMetadata.Contains("^^CultureName^^"))
            {
                chartMetadata = chartMetadata.Replace("^^CultureName^^", '"' + _userContext.CultureName + '"');
            }
            return chartMetadata;
        }
        private async Task<string> BuildChart(string json, string dataJson, Dictionary<string, string> prms, string ntsId, string ntsStatusCode = null)
        {
            var chartCum = "";
            if (json.IsNotNullAndNotEmptyAndNotValue("{}"))
            {
                var data = JToken.Parse(json);
                JArray rows = (JArray)data.SelectToken("components");
                chartCum = await BuildChartJson(rows, dataJson, chartCum, prms, ntsId, ntsStatusCode);
            }
            return chartCum;

        }

        private string SetUdfView(string json)
        {

            if (json.IsNotNullAndNotEmpty())
            {
                var result = JObject.Parse(json);
                JArray rows = (JArray)result.SelectToken("components");
                ChildComp(rows);
                return result.ToString();
            }
            return json;
        }
        private void ChildComp(JArray comps)
        {

            foreach (JObject jcomp in comps)
            {
                var typeObj = jcomp.SelectToken("type");
                var keyObj = jcomp.SelectToken("key");
                if (typeObj.IsNotNull())
                {
                    var type = typeObj.ToString();
                    var key = keyObj.ToString();
                    if (type == "textfield" || type == "textarea" || type == "number" || type == "password"
                        || type == "selectboxes" || type == "checkbox" || type == "select" || type == "radio"
                        || type == "email" || type == "url" || type == "phoneNumber" || type == "tags"
                        || type == "datetime" || type == "day" || type == "time" || type == "currency"
                        || type == "signature" || type == "file" || type == "datagrid" || (type == "htmlelement" && key == "chartgrid"))
                    {

                        var reserve = jcomp.SelectToken("reservedKey");
                        if (reserve == null)
                        {
                            var tempmodel = JsonConvert.DeserializeObject<FormFieldViewModel>(jcomp.ToString());

                            var disableProperty = jcomp.SelectToken("disabled");
                            if (disableProperty == null)
                            {
                                var newProperty = new JProperty("disabled", true);
                                jcomp.Add(newProperty);
                                if (type == "datagrid")
                                {
                                    JArray dataRows = (JArray)jcomp.SelectToken("components");
                                    foreach (JObject jcomp1 in dataRows)
                                    {
                                        var newProperty1 = new JProperty("disabled", true);
                                        jcomp1.Add(newProperty1);
                                    }
                                }
                            }

                        }
                    }
                    else if (type == "columns")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("columns");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                ChildComp(rows);
                        }
                    }
                    else if (type == "table")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("rows");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                ChildComp(rows);
                        }
                    }
                    else
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                            ChildComp(rows);
                    }
                }
            }
        }
        public async Task<NoteTemplateViewModel> GetNoteViewModel(NoteTemplateViewModel noteTemplate)
        {
            var model = noteTemplate;
            model.RecordId = noteTemplate.Page.RecordId;
            if (noteTemplate.Page.RequestSource != RequestSourceEnum.Main
                && (noteTemplate.Page.RequestSource != RequestSourceEnum.Post || noteTemplate.Page.DataAction == DataActionEnum.Delete))
            {
                model = await _noteBusiness.GetNoteDetails(noteTemplate);
            }
            model.Page = noteTemplate.Page;
            model.PageId = noteTemplate.Page.Id;
            model.DataAction = noteTemplate.Page.DataAction;
            model.RecordId = noteTemplate.Page.RecordId;
            model.PortalName = noteTemplate.Page.Portal.Name;

            model.CustomUrl = noteTemplate.CustomUrl;
            model.ReturnUrl = noteTemplate.ReturnUrl;
            model.LayoutMode = noteTemplate.LayoutMode;
            model.PopupCallbackMethod = noteTemplate.PopupCallbackMethod;
            model.ViewMode = noteTemplate.ViewMode;

            return model;
        }
        private async Task<string> GetNoteModel(NoteTemplateViewModel noteTemplate, TemplateTypeEnum viewName)
        {
            var data = await _cmsBusiness.GetDataById(viewName, noteTemplate.Page, noteTemplate.Page.RecordId, noteTemplate.IsLogRecord, noteTemplate.LogId);
            if (data == null)
            {
                return "{}";
            }
            return data.ToJson();
        }
        public async Task<NoteIndexPageTemplateViewModel> GetNoteIndexPageViewModel(PageViewModel page)
        {
            var model = await _noteBusiness.GetNoteIndexPageViewModel(page);
            model.Page = page;
            model.PageId = page.Id;
            return model;
        }
        #endregion
        public async Task<ActionResult> ReadTeamData([DataSourceRequest] DataSourceRequest request, string portalId)
        {
            var model = await _teamBusiness.GetList();
            if (portalId.IsNotNullAndNotEmpty())
            {
                model = model.Where(x => x.PortalId == portalId).ToList();
            }
            var data = model.ToList();

            // var dsResult = data.ToDataSourceResult(request);
            return Json(data);
        }

        public IActionResult NtsTaskShared(string taskId, bool IsSharingEnabled)
        {
            var model = new NtsTaskSharedViewModel();
            model.NtsTaskId = taskId;
            model.IsSharingEnabled = IsSharingEnabled;
            return View("NtsTaskShared", model);
        }
        public IActionResult NtsNoteShared(string noteId, bool IsSharingEnabled)
        {
            var model = new NtsNoteSharedViewModel();
            model.NtsNoteId = noteId;
            model.IsSharingEnabled = IsSharingEnabled;
            return View("NtsNoteShared", model);
        }
        public IActionResult NtsServiceShared(string serviceId, bool IsSharingEnabled)
        {
            var model = new NtsServiceSharedViewModel();
            model.NtsServiceId = serviceId;
            model.IsSharingEnabled = IsSharingEnabled;
            return View("NtsServiceShared", model);
        }
        public IActionResult NtsServiceProcessDesignResult(string serviceId)
        {
            var model = new ComponentResultViewModel();
            model.NtsServiceId = serviceId;
            return View("_NtsServiceProcessDesignResult", model);
        }

        public IActionResult NtsVersion(string recordId, NtsTypeEnum type, string portalId)
        {
            var model = new NtsLogViewModel();
            model.RecordId = recordId;
            model.NtsType = type;
            ViewBag.PortalId = portalId;
            return View(model);
        }

        public async Task<IActionResult> ReadNtsVersionData(string recordId, NtsTypeEnum type)
        {
            if (type == NtsTypeEnum.Service)
            {
                var model = await _serviceBusiness.GetVersionDetails(recordId);
                return Json(model);
            }
            else if (type == NtsTypeEnum.Task)
            {
                var model = await _taskBusiness.GetVersionDetails(recordId);
                return Json(model);
            }
            else
            {
                var model = await _noteBusiness.GetVersionDetails(recordId);
                return Json(model);
            }

        }
        public async Task<IActionResult> ServiceAdhocTask(string templateId, string serviceId, string moduleId)
        {
            var model = await _cmsBusiness.GetSingle<ServiceTemplateViewModel, ServiceTemplate>(x => x.TemplateId == templateId);
            model.ServiceId = serviceId;
            if (model.AdhocTaskTemplateIds != null && model.AdhocTaskTemplateIds.Count() > 0)
            {
                model.AdhocTaskTemplateId = string.Join(",", model.AdhocTaskTemplateIds);
                var templatelist = await _templateBusiness.GetTemplateListByTaskTemplate(model.AdhocTaskTemplateId);
                if (templatelist != null)
                {
                    model.TemplateCode = string.Join(",", templatelist.Select(x => x.Code));
                }
            }
            var templateDetails = await _templateBusiness.GetSingleById(templateId);
            model.ModuleId = templateDetails.ModuleId.IsNotNullAndNotEmpty() ? templateDetails.ModuleId : "";
            return View(model);
        }

        public async Task<ActionResult> ReadTaskSharedData(string taskId)
        {
            var result = await _ntsTaskSharedBusiness.GetSearchResult(taskId);
            var j = Json(result);
            return j;
        }
        public async Task<ActionResult> ReadNoteSharedData(string noteId)
        {
            var result = await _ntsNoteSharedBusiness.GetSearchResult(noteId);
            var j = Json(result);
            return j;
        }
        public async Task<ActionResult> ReadServiceSharedData(string serviceId)
        {
            var result = await _ntsServiceSharedBusiness.GetSearchResult(serviceId);
            var j = Json(result);
            return j;
        }
        //public ActionResult GetAssignToValueMapper(long value, AssignToTypeEnum? assignToType, long? teamId)
        //{
        //    var dataItemIndex = -1;

        //    if (value != 0)
        //    {
        //        var data = GetAssignToList(null, assignToType, teamId);
        //        var item = data.FirstOrDefault(x => x.Id == value);
        //        dataItemIndex = data.IndexOf(item);
        //    }
        //    return Json(dataItemIndex, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public ActionResult GetAssignToList(DataSourceRequest request, string assigneeType)
        //{
        //    var data = new List<NtsTaskSharedViewModel>();
        //    if (assigneeType == 1)
        //    {
        //        data = _userBusiness.GetUserList.ToList();
        //    }
        //    else
        //    {
        //        data = _userBusiness.ToList();

        //    }

        //    return Json(data);
        //}
        [HttpPost]
        public async Task<ActionResult> Create(NtsTaskSharedViewModel model)
        {
            if (model.TaskSharedWithTypeId == "1")
            {
                var lov = await _lovBusiness.GetSingle(x => x.Code == "SHARED_TYPE_USER" && x.LOVType == "SHARED_TYPE");
                model.TaskSharedWithTypeId = lov.Id;
            }
            else
            {
                var lov = await _lovBusiness.GetSingle(x => x.Code == "SHARED_TYPE_TEAM" && x.LOVType == "SHARED_TYPE");
                model.TaskSharedWithTypeId = lov.Id;
            }
            model.SharedDate = DateTime.Now;
            model.SharedByUserId = _userContext.UserId;
            var result = await _ntsTaskSharedBusiness.Create(model);
            return Json(new { success = result.IsSuccess, TaskId = result.Item.NtsTaskId });
        }
        [HttpPost]
        public async Task<ActionResult> ShareNote(NtsNoteSharedViewModel model)
        {
            if (model.NoteSharedWithTypeId == "1")
            {
                var lov = await _lovBusiness.GetSingle(x => x.Code == "SHARED_TYPE_USER" && x.LOVType == "SHARED_TYPE");
                model.NoteSharedWithTypeId = lov.Id;
            }
            else
            {
                var lov = await _lovBusiness.GetSingle(x => x.Code == "SHARED_TYPE_TEAM" && x.LOVType == "SHARED_TYPE");
                model.NoteSharedWithTypeId = lov.Id;
            }
            model.SharedDate = DateTime.Now;
            model.SharedByUserId = _userContext.UserId;
            var result = await _ntsNoteSharedBusiness.Create(model);
            return Json(new { success = result.IsSuccess, TaskId = result.Item.NtsNoteId });
        }
        [HttpPost]
        public async Task<ActionResult> ShareService(NtsServiceSharedViewModel model)
        {
            if (model.ServiceSharedWithTypeId == "1")
            {
                var lov = await _lovBusiness.GetSingle(x => x.Code == "SHARED_TYPE_USER" && x.LOVType == "SHARED_TYPE");
                model.ServiceSharedWithTypeId = lov.Id;
            }
            else
            {
                var lov = await _lovBusiness.GetSingle(x => x.Code == "SHARED_TYPE_TEAM" && x.LOVType == "SHARED_TYPE");
                model.ServiceSharedWithTypeId = lov.Id;
            }
            model.SharedDate = DateTime.Now;
            model.SharedByUserId = _userContext.UserId;
            var result = await _ntsServiceSharedBusiness.Create(model);
            return Json(new { success = result.IsSuccess, TaskId = result.Item.NtsServiceId });
        }
        public async Task<ActionResult> DeleteTaskShared(string id)
        {
            await _ntsTaskSharedBusiness.Delete(id);
            return Json(new { success = true });

        }
        public async Task<ActionResult> DeleteNoteShared(string id)
        {
            await _ntsNoteSharedBusiness.Delete(id);
            return Json(new { success = true });

        }
        public async Task<ActionResult> DeleteServiceShared(string id)
        {
            await _ntsServiceSharedBusiness.Delete(id);
            return Json(new { success = true });

        }

        [HttpPost]
        public async Task<IActionResult> PostTaskComment(NtsTaskCommentViewModel model)
        {
            model.CommentedByUserId = _userContext.UserId;
            model.CommentedDate = DateTime.Now;
            var result = await _ntsTaskCommentBusiness.Create(model);
            if (result.IsSuccess)
            {
                var data = await _ntsTaskCommentBusiness.GetSearchResult(model.NtsTaskId);
                return Json(new { success = true, CommentsCount = data.Count() });
            }
            return Json(new { success = false, error = result.HtmlError });
        }
        [HttpPost]
        public async Task<IActionResult> PostNoteComment(NtsNoteCommentViewModel model)
        {
            model.CommentedByUserId = _userContext.UserId;
            model.CommentedDate = DateTime.Now;
            var result = await _ntsNoteCommentBusiness.Create(model);
            if (result.IsSuccess)
            {
                var data = await _ntsNoteCommentBusiness.GetSearchResult(model.NtsNoteId);
                return Json(new { success = true, CommentsCount = data.Count() });
            }
            return Json(new { success = false, error = result.HtmlError });
        }
        public async Task<ActionResult> GetTaskCommentCount(string taskId)
        {
            var list = await _ntsTaskCommentBusiness.GetSearchResult(taskId);

            return Json(list.Count());
        }
        public async Task<ActionResult> GetNoteCommentCount(string noteId)
        {
            var list = await _ntsNoteCommentBusiness.GetSearchResult(noteId);

            return Json(list.Count());
        }
        [HttpPost]
        public async Task<IActionResult> PostServiceComment(NtsServiceCommentViewModel model)
        {

            model.CommentedByUserId = _userContext.UserId;
            model.CommentedDate = DateTime.Now;
            var result = await _ntsServiceCommentBusiness.Create(model);
            if (result.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, error = result.HtmlError });
        }
        public async Task<ActionResult> GetServiceCommentCount(string serviceId)
        {
            var list = await _ntsServiceCommentBusiness.GetSearchResult(serviceId);
            return Json(list.Count());
        }
        public async Task<bool> UpdateAllReadNotification(string taskId)
        {
            await _pushNotificationBusiness.SetAllTaskNotificationRead(_userContext.UserId, taskId);
            return true;
        }
        [HttpPost]
        public async Task<IActionResult> ManageTaskTimeEntry(TaskTimeEntryViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.TimeEntryId.IsNotNullAndNotEmpty())
                {
                    model.Id = model.TimeEntryId;
                }
                if (model.StartDate > model.EndDate)
                {
                    return Json(new { success = false, error = "Start date should be less than or equal to end date." });
                }
                model.Duration = model.EndDate - model.StartDate;
                if (model.DataAction == DataActionEnum.Create)
                {
                    model.Id = null;
                    var result = await _ntsTaskTimeEntryBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        return Json(new { success = true });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _ntsTaskTimeEntryBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        return Json(new { success = true });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }
        public async Task<IActionResult> DeleteTaskTimeEntry(string Id)
        {
            await _ntsTaskTimeEntryBusiness.Delete(Id);
            return Json(true);
        }
        public async Task<IActionResult> GetTaskTimeEntry(string Id)
        {
            var data = await _ntsTaskTimeEntryBusiness.GetSingleById(Id);
            return Json(new { success = true, result = data });
        }
        public async Task<IActionResult> DeleteTaskPrecedence(string Id)
        {
            await _ntsTaskPrecedenceBusiness.Delete(Id);
            return Json(true);
        }
        public async Task<ActionResult> GetPredecessorList(string Id, NtsTypeEnum type)
        {
            List<IdNameViewModel> data = new List<IdNameViewModel>();
            if (type == NtsTypeEnum.Task)
            {
                var list = await _taskBusiness.GetList(x => x.Id != Id);
                data = list.Select(x => new IdNameViewModel
                {
                    Id = x.Id,
                    Name = x.TaskNo + "- " + x.TaskSubject
                }).ToList();
            }
            else if (type == NtsTypeEnum.Service)
            {
                var list = await _serviceBusiness.GetList(x => x.Id != Id);
                data = list.Select(x => new IdNameViewModel
                {
                    Id = x.Id,
                    Name = x.ServiceNo + "- " + x.ServiceSubject
                }).ToList();
            }
            else if (type == NtsTypeEnum.Note)
            {
                var list = await _noteBusiness.GetList(x => x.Id != Id);
                data = list.Select(x => new IdNameViewModel
                {
                    Id = x.Id,
                    Name = x.NoteNo + "- " + x.NoteSubject
                }).ToList();
            }
            return Json(data);
        }
        public async Task<ActionResult> ReadTaskSuccessorsEntriesData(string taskId)
        {
            var list = await _ntsTaskPrecedenceBusiness.GetTaskSuccessor(taskId);
            return Json(list);
        }
        public async Task<ActionResult> ReadTaskPrecedenceEntriesData(string taskId)
        {
            var list = await _ntsTaskPrecedenceBusiness.GetTaskPredecessor(taskId);
            return Json(list);
        }
        [HttpPost]
        public async Task<IActionResult> ManageTaskPrecedence(NtsTaskPrecedenceViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.PredecessorsIds.Length > 0)
                {
                    foreach (var id in model.PredecessorsIds)
                    {
                        var data = new NtsTaskPrecedenceViewModel();
                        data.NtsTaskId = model.NtsTaskId;
                        data.PrecedenceRelationshipType = model.PrecedenceRelationshipType;
                        data.PredecessorType = model.PredecessorType;
                        data.PredecessorId = id;
                        var result = await _ntsTaskPrecedenceBusiness.Create(data);
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }
        public async Task<IActionResult> ReadComponentResultData(string serviceId)
        {
            var model = await _componentResultBusiness.GetComponentResultList(serviceId);
            return Json(model);
        }

        public IActionResult AddEmailAttachment(string taskId, string dataAction, bool IsAddAttachmentEnabled)
        {
            var model = new FileViewModel();
            model.DataAction = dataAction.ToEnum<DataActionEnum>();
            model.ReferenceTypeId = taskId;
            ViewBag.IsAddAttachmentEnabled = IsAddAttachmentEnabled;
            model.ReferenceTypeCode = ReferenceTypeEnum.NTS_Task;

            return View("_NtsEmailAttachment", model);
        }

        public IActionResult NtsEmail(string refId, ReferenceTypeEnum refType)
        {
            var model = new TaskViewModel();
            model.ReferenceId = refId;
            model.ReferenceType = refType;
            return View("_NtsEmail", model);
        }

        public async Task<IActionResult> ManageNtsEmail(string refId, string mode, string newid, ReferenceTypeEnum refType)
        {
            var model = new TaskTemplateViewModel();
            var emailmodel = new EmailTaskViewModel();
            var bodyFooter = "";
            if (mode == "Create" || mode == "Reply")
            {
                ViewBag.Mode = "Create";
                model.ReferenceId = refId;
                model.ReferenceType = refType;
                model.DataAction = DataActionEnum.Create;
                model.ActiveUserId = _userContext.UserId;
                // model.StartDate = DateTime.Now;
                model.TemplateCode = "EMAIL_TASK";
                var taskmodel = await _taskBusiness.GetTaskDetails(model);
                var emailsetup = await _projectEmailBusiness.GetSingle(x => x.UserId == _userContext.UserId);
                if (emailsetup.IsNotNull())
                {
                    bodyFooter = "<br><br><br>" + emailsetup.Signature;
                }
                emailmodel = _autoMapper.Map<TaskTemplateViewModel, EmailTaskViewModel>(taskmodel, emailmodel);
                emailmodel.ReferenceId = refId;
                emailmodel.ReferenceType = refType;
                emailmodel.DataAction = DataActionEnum.Create;
                emailmodel.ActiveUserType = NtsActiveUserTypeEnum.Owner;
                emailmodel.TaskDescription = bodyFooter;
                if (mode == "Reply")
                {
                    var taskdata = await _cmsBusiness.ReadEmailTaskData(refId, refType);
                    if (taskdata.Count > 0)
                    {
                        taskdata = taskdata.Where(x => x.Id == newid).ToList();
                        emailmodel.To = taskdata.FirstOrDefault().RequestedByUserName;
                        emailmodel.TaskDescription = "<br/><br/><br/><br/>From: " + taskdata.FirstOrDefault().OwnerUserName +
                        "<br> Sent: " + taskdata.FirstOrDefault().SentDate +
                        "<br> To: " + taskdata.FirstOrDefault().AssignedToUserName;
                        if (taskdata.FirstOrDefault().CC.IsNotNullAndNotEmpty())
                        {
                            emailmodel.TaskDescription += "<br>Cc: " + taskdata.FirstOrDefault().CC;
                        }
                        if (taskdata.FirstOrDefault().BCC.IsNotNullAndNotEmpty())
                        {
                            emailmodel.TaskDescription += "<br>BCc: " + taskdata.FirstOrDefault().BCC;
                        }
                        emailmodel.TaskDescription += "<br>Subject: " + taskdata.FirstOrDefault().TaskSubject + "<br><br>" + taskdata.FirstOrDefault().TaskDescription + "" + bodyFooter;
                        emailmodel.ParentTaskId = taskdata.FirstOrDefault().Id;
                        emailmodel.ReferenceId = refId;
                        emailmodel.ReferenceType = refType;
                    }


                }
                emailmodel.TemplateCode = "EMAIL_TASK";

            }
            else if (mode == "Edit")
            {
                ViewBag.Mode = "Edit";
                var taskmodel = await _taskBusiness.GetTaskDetails(new TaskTemplateViewModel { TaskId = newid });

                emailmodel = _autoMapper.Map<TaskTemplateViewModel, EmailTaskViewModel>(taskmodel, emailmodel);
                var taskdata = await _cmsBusiness.ReadEmailTaskData(refId, refType);
                taskdata = taskdata.Where(x => x.Id == newid).ToList();
                emailmodel.To = taskdata.FirstOrDefault().AssignedToUserName;
                emailmodel.From = taskdata.FirstOrDefault().OwnerUserName;
                emailmodel.BCC = taskdata.FirstOrDefault().BCC;
                emailmodel.CC = taskdata.FirstOrDefault().CC;
                emailmodel.TaskSubject = taskdata.FirstOrDefault().TaskSubject;
                emailmodel.TaskDescription = taskdata.FirstOrDefault().TaskDescription;
                emailmodel.TaskId = taskdata.FirstOrDefault().Id;
                emailmodel.DataAction = DataActionEnum.Edit;
                emailmodel.ActiveUserType = NtsActiveUserTypeEnum.Owner;
            }
            else if (mode == "View")
            {
                ViewBag.Mode = "View";
                var taskdata = await _cmsBusiness.ReadEmailTaskData(refId, refType);
                taskdata = taskdata.Where(x => x.Id == newid).ToList();
                emailmodel.To = taskdata.FirstOrDefault().To;
                emailmodel.From = taskdata.FirstOrDefault().From;
                emailmodel.BCC = taskdata.FirstOrDefault().BCC;
                emailmodel.CC = taskdata.FirstOrDefault().CC;
                emailmodel.TaskSubject = taskdata.FirstOrDefault().TaskSubject;
                emailmodel.TaskDescription = taskdata.FirstOrDefault().TaskDescription;
                emailmodel.TaskId = taskdata.FirstOrDefault().Id;
            }
            //  model.TaskDescription = HttpUtility.HtmlDecode(model.TaskDescription);

            return View("_ManageNtsEmail", emailmodel);
        }

        [HttpPost]
        public async Task<IActionResult> ManageNtsEmail(EmailTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {

                    var parenttask = await _taskBusiness.GetSingleById(model.ParentTaskId);
                    if (parenttask.IsNotNull())
                    {
                        model.StartDate = DateTime.Now;
                        model.TaskSLA = parenttask.TaskSLA;
                        model.DueDate = model.StartDate + model.TaskSLA;
                        model.TaskPriorityId = parenttask.TaskPriorityId;
                        var lov = await _lovBusiness.GetSingle(x => x.Code == "TASK_ASSIGN_TO_USER");
                        if (lov.IsNotNull())
                        {
                            model.AssignedToTypeId = lov.Id;
                            model.AssignedToTypeCode = lov.Code;
                            model.AssignedToTypeName = lov.Name;
                        }
                    }

                }

                model.TaskDescription = HttpUtility.HtmlDecode(model.TaskDescription);
                var ToUser = await _userBusiness.GetSingle(x => x.Email == model.To);
                var userModel = new UserViewModel();

                if (ToUser == null)
                {
                    model.AssignedToUserId = model.OwnerUserId;
                }
                else
                {
                    model.AssignedToUserId = ToUser.Id;
                    //model.To = FromUser.Email;
                }
                // var taskviewmodel = new TaskTemplateViewModel();
                // taskviewmodel = _autoMapper.Map<EmailTaskViewModel,TaskTemplateViewModel>(model,taskviewmodel);
                // model.From = model.OwnerUserEmail;
                model.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var result = await _taskBusiness.ManageTask(model);
                if (result.IsSuccess)
                {
                    if (model.TaskStatusCode != "TASK_STATUS_DRAFT")
                    {
                        var email = new EmailViewModel();
                        email.From = result.Item.OwnerUserId;
                        email.Subject = result.Item.TaskSubject;
                        email.Body = result.Item.TaskDescription;
                        email.ReferenceId = result.Item.TaskId;
                        email.CC = model.CC;
                        email.BCC = model.BCC;
                        email.To = model.To;
                        email.OwnerUserId = result.Item.OwnerUserId;
                        email.SenderEmail = model.From;
                        var attachments = await _fileBusiness.GetList(x => x.ReferenceTypeId == result.Item.TaskId);
                        if (attachments.Count > 0)
                        {
                            email.AttachmentIds = attachments.Select(x => x.Id).ToArray();
                        }
                        await _emailBusiness.SendMailTask(email);
                    }
                    return Json(new { success = true });
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }
        public async Task<object> ReadEmailData(/*[DataSourceRequest] DataSourceRequest request,*/string id, string refId, ReferenceTypeEnum refType)
        {
            var model = await _cmsBusiness.ReadEmailTaskData(refId, refType);
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
            //  return Json(model.ToDataSourceResult(request));
        }
        public async Task<IActionResult> GetUserEmail(string text)
        {
            var model = await _userBusiness.GetUserListWithEmailText();
            var matchtext = text;
            if (text.IsNotNull())
            {
                var texts = text.Split(",");
                if (texts.Length > 1)
                {
                    matchtext = texts.Last();
                }
            }

            model = model.Where(x => x.Email != null && x.Email.Contains(matchtext)).ToList();
            return Json(model);
        }

        public async Task<IActionResult> GeFromUsers()
        {
            var model = await _projectEmailBusiness.GetSmtpEmailId();
            return Json(model);
        }

        // public async Task<IActionResult> ViewEmail(string subject,string body,string to, string from, string cc,string bcc)
        public async Task<IActionResult> ViewEmail(string prms)
        {
            var emailmodel = new EmailTaskViewModel();
            //emailmodel.To = to;
            //emailmodel.From = from;
            //emailmodel.BCC = bcc;
            //emailmodel.CC = cc;
            //emailmodel.TaskSubject = subject;
            //emailmodel.TaskDescription = body;
            //var userdeatils = await _userBusiness.GetSingle(x => x.Email == from);
            var Prms = new Dictionary<string, string>();
            Prms = Helper.QueryStringToDictionary(prms);
            emailmodel.To = Prms.GetValue("to");
            emailmodel.From = Prms.GetValue("from");
            emailmodel.BCC = Prms.GetValue("bcc");
            emailmodel.CC = Prms.GetValue("cc");
            emailmodel.TaskSubject = Prms.GetValue("subject");
            emailmodel.TaskDescription = Prms.GetValue("body");
            var userdeatils = await _userBusiness.GetSingle(x => x.Email == Prms.GetValue("from"));
            if (userdeatils.IsNotNull())
            {
                emailmodel.PhotoId = userdeatils.PhotoId;
            }
            ViewBag.Mode = "View";
            //emailmodel.TaskId = taskdata.FirstOrDefault().Id;
            return View("_ManageNtsEmail", emailmodel);
        }
        public ActionResult WorkListDashboard(string permissions, string moduleCodes, string userId, LayoutModeEnum? lo)
        {
            //  moduleCodes = "PJM,HR";
            DashboardViewModel model = new DashboardViewModel();
            ViewBag.Permissions = permissions;
            model.ModuleCodes = moduleCodes;
            model.UserId = userId;
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            return View(model);
        }

        public async Task<IActionResult> GetTableData(string tableName, string columns = null, string filter = null, string orderbyColumns = null, string orderBy = null)
        {
            try
            {
                var split = tableName.Split('.');
                if (split.Length != 2)
                {
                    throw new ArgumentException("Invalid table name");
                }
                var data = await _cmsBusiness.GetData(split[0], split[1], columns, filter);
                return Json(data);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public async Task<ActionResult> ReAssignTerminatedEmployeeServices(string services, string userId)
        {
            List<string> serviceIds = new List<string>();
            var Str = services.Trim(',');
            var ids = Str.Split(',').Distinct();
            foreach (var id in ids)
            {
                serviceIds.Add(id);
            }
            await _serviceBusiness.ReAssignTerminatedEmployeeServices(userId, serviceIds);
            return Json(new { success = false });
        }
        public async Task<ActionResult> PreviewAttachment(ReferenceTypeEnum ntsType, string Id, bool canEdit, string logId)
        {
            var List = new List<FileViewModel>();
            List = await _fileBusiness.GetList(x => x.Id == Id);
            if (List.Count > 0)
            {
                if (List.First().FileExtension.ToLower().Contains("pdf") || List.First().MongoPreviewFileId.IsNotNullAndNotEmpty())
                {
                    return View("_PreviewDocuments", List.FirstOrDefault());
                }
                else if (Helper.IsExcelFile(List.First().FileExtension))
                {
                    var model = List.FirstOrDefault();
                    //model.ContentBase64 = Convert.ToBase64String( await _fileBusiness.DownloadMongoFileByte(model.MongoFileId));
                    return View("_PreviewExcels", model);
                }
            }
            if (logId.IsNotNullAndNotEmpty())
            {
                List = await _fileBusiness.GetFileLogsDetailsById(logId);
            }
            //List = await _fileBusiness.GetList(x => x.ReferenceTypeId == Id && x.ReferenceTypeCode == ntsType);
            ViewBag.CanEdit = canEdit;
            ViewBag.LogId = logId;
            return View("_PreviewAttachment", List);

        }
        public async Task<ActionResult> PreviewDocuments(ReferenceTypeEnum ntsType, string Id, bool canEdit, string logId)
        {
            var model = await _fileBusiness.GetSingle(x => x.Id == Id);
            return View("_PreviewDocuments", model);

        }
        public async Task<ActionResult> PreviewOCRDocuments(string templateId, string ocrId, string fileId, string fieldName)
        {
            var model = await _fileBusiness.GetSingle(x => x.Id == fileId);
            ViewBag.TemplateId = templateId;
            ViewBag.OcrId = ocrId;
            ViewBag.FieldName = fieldName;
            return View("_PreviewOCRDocuments", model);

        }

        public async Task<ActionResult> PreviewExcels(ReferenceTypeEnum ntsType, string Id, bool canEdit, string logId)
        {
            var model = await _fileBusiness.GetSingle(x => x.Id == Id);
            model.ContentBase64 = Convert.ToBase64String(model.ContentByte, 0, model.ContentByte.Length);
            return View("_PreviewExcels", model);

        }
        public async Task<ActionResult> GetFileLog(string FileId)
        {
            var data = await _fileBusiness.GetFileLogsDetails(FileId);
            return Json(data);
        }
        public async Task<string> GetFileBase64(string mongoId)
        {
            var ContentBase64 = Convert.ToBase64String(await _fileBusiness.DownloadMongoFileByte(mongoId));
            return ContentBase64;
        }
        [HttpPost]
        public async Task<ActionResult> LockTask(string taskId, string userId)
        {
            try
            {
                if (userId.IsNullOrEmpty())
                {
                    userId = _userContext.UserId;
                }
                var result = await _taskBusiness.ChangeAssignee(taskId, userId);
                return Json("Success");

            }
            catch (Exception e)
            {
                //return Json("Problem in Locking the Task, Please contact administrator", JsonRequestBehavior.AllowGet);
                return Json("Error");
            }
        }
        [HttpPost]
        public async Task<ActionResult> ReleaseTask(string taskId)
        {
            try
            {
                var result = await _taskBusiness.ChangeLockStatus(taskId);
                return Json("Success");

            }
            catch (Exception e)
            {
                //return Json("Problem in Locking the Task, Please contact administrator", JsonRequestBehavior.AllowGet);
                return Json("Error");
            }
        }
        [HttpPost]
        public async Task<ActionResult> StartTask(string taskId)
        {
            try
            {
                var result = await _taskBusiness.UpdateActualStartDate(taskId);
                return Json("Success");

            }
            catch (Exception e)
            {

                return Json("Error");
            }
        }
        [HttpPost]
        public async Task<ActionResult> DeleteServiceBookItems(string serviceId, string parentId, NtsTypeEnum parentType)
        {
            await _serviceBusiness.DeleteServiceBookItem(serviceId, parentId, parentType);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteNoteBookItems(string noteId, string parentId, NtsTypeEnum parentType)
        {
            await _noteBusiness.DeleteServiceBookItem(noteId, parentId, parentType);
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<ActionResult> SaveNextStepComponent(string ServiceId, string StepCompId)
        {
            var service = await _serviceBusiness.GetSingleById(ServiceId);
            service.NextStepTaskComponentId = StepCompId;
            await _serviceBusiness.Edit(service);
            return Json(new { success = true });
        }
        public async Task<ActionResult> StepComponent(string templateId, string serviceId, string taskId)
        {
            var model = new ServiceTemplateViewModel { TemplateId = templateId, ServiceId = serviceId, TaskPlusId = taskId };
            if (templateId.IsNullOrEmpty())
            {
                var service = await _serviceBusiness.GetSingleById(serviceId);
                model.TemplateId = service.TemplateId;
            }

            return View("_StepComponent", model);
        }
        [HttpGet]
        public async Task<IActionResult> GetTimezoneList()
        {
            //throw new Exception("test error");
            // var j = TimeZoneInfo.FindSystemTimeZoneById("Dateline Standard Time");
            var timezones = TimeZoneInfo.GetSystemTimeZones();
            var data = timezones.Select(x => new IdNameViewModel { Id = x.Id, Name = x.DisplayName }).ToList();
            return Json(data);
        }
    }
}
