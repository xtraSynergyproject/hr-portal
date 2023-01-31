using AutoMapper;
using Synergy.App.WebUtility;
using FastReport.Data;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Syncfusion.DocIO;
//using Syncfusion.DocIO.DLS;
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
using FastReport.Export.PdfSimple;
using System.Net;
using System.Collections.Specialized;
using FastReport.Web;
using System.Drawing;
using System.Xml;

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
        public IActionResult ServiceComments(string serviceId, bool IsAddCommentEnabled, bool IsAddAttachmentEnabled = true, bool IsAdminDeleteCommentEnabled = false)
        {
            var model = new NtsServiceCommentViewModel();
            model.NtsServiceId = serviceId;
            model.IsAddCommentEnabled = IsAddCommentEnabled;
            model.IsAddAttachmentEnabled = IsAddAttachmentEnabled;
            model.IsAdminDeleteCommentEnabled = IsAdminDeleteCommentEnabled;
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
            return Json(model);
            //return Json(model.ToDataSourceResult(request));
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
            return null;
            //var xsx = await _fileBusiness.GetFileByte(fileId);
            //Stream stream = new MemoryStream(xsx);
            //stream.Position = 0;

            //// WordDocument document = WordDocument.Load(stream, GetFormatType(".docx"));
            //WordDocument document = new WordDocument();
            //document.Open(stream, FormatType.Docx);

            //string sfdt = Newtonsoft.Json.JsonConvert.SerializeObject(document);
            //document.Dispose();
            //return Content(sfdt, "text/html");
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
            ViewBag.Layout = $"~/Areas/Core/Views/Shared/_EmptyLayout.cshtml";
            return View();
        }
        public async Task<IActionResult> MultiplePortalNotAllowed(string returnUrl = null)
        {
            var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.PortalDisplayName = portal.DisplayName;
            }
            ViewBag.Layout = $"~/Areas/Core/Views/Shared/_EmptyLayout.cshtml";
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
        public async Task<IActionResult> Page(string pageId, string pageType, string source, string dataAction, string recordId, string customUrl, string portalId, string pageName, string prms, string udfs, string roudfs, string hiddenudfs, bool popup, string ru, LayoutModeEnum lo, string cbm, string templateCodes, string categoryCodes, string iframe, string keywords, string logId, NtsViewTypeEnum? viewType, NtsViewTypeEnum? viewMode, string attachments, bool introDone = false, string targetId = null, NtsEmailTargetTypeEnum? targetType = null)
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
                ViewBag.UserRoleCodes = _userContext.UserRoleCodes;
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
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/_EmptyLayout.cshtml";
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
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/_EmptyLayout.cshtml";
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
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{page.Portal.Theme.ToString()}/_Layout.cshtml";
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
            ViewBag.UserRoleCodes = _userContext.UserRoleCodes;
            ViewBag.IsPopup = popup;
            ViewBag.IframeName = iframe;
            ViewBag.EnableMultiLanguage = page.Portal.EnableMultiLanguage;
            ViewBag.FavIconId = page.Portal.FavIconId;
            ViewBag.ThemeName = page.Portal.Theme;
            ViewBag.UserName = _userContext.Name;
            page.PopupCallbackMethod = cbm;
            page.TemplateCodes = templateCodes;
            page.ViewMode = viewMode;
            if (viewType != null)
            {
                page.ViewType = viewType;
                page.Template.ViewType = viewType;
            }
            if (targetType.HasValue)
            {
                var model = await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel { ServiceId = recordId, ActiveUserId = _userContext.UserId });
                model.ViewType = NtsViewTypeEnum.Email;
                model.DataAction = DataActionEnum.View;
                model.EmailList = await _componentResultBusiness.GetNtsEmailDetails(recordId, targetId, targetType);
                ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{page.Portal.Theme.ToString()}/_Layout.cshtml";
                return View("ServiceEmail", model);
            }

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
                    formvm.HiddenUdfs = Helper.QueryStringToBooleanDictionary(hiddenudfs);
                    formvm.PopupCallbackMethod = cbm;
                    formvm.TemplateCode = formvm.Template.Code;
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
                    notevm.HiddenUdfs = Helper.QueryStringToBooleanDictionary(hiddenudfs);
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
                            ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{page.Portal.Theme.ToString()}/_Layout.cshtml";
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
                    taskvm.HiddenUdfs = Helper.QueryStringToBooleanDictionary(hiddenudfs);
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
                    serviceVm.HiddenUdfs = Helper.QueryStringToBooleanDictionary(hiddenudfs);
                    serviceVm.LogId = logId;
                    serviceVm.ViewType = page.Template.ViewType;
                    if (serviceVm.EnableIntroPage && source == DataActionEnum.Create.ToString() && !introDone)
                    {
                        return RedirectToIntroPage(serviceVm, page, cbm);
                    }
                    //if (serviceVm.Page.RequestSource != RequestSourceEnum.Post)
                    //{
                    //    if (!(serviceVm.Page.RequestSource == RequestSourceEnum.Main && serviceVm.EnableIndexPage))
                    //    {
                    //        if (serviceVm.LayoutMode.HasValue && serviceVm.LayoutMode == LayoutModeEnum.Popup)
                    //        {
                    //            if (serviceVm.ViewType == NtsViewTypeEnum.Light)
                    //            {
                    //                serviceVm = await GetServiceViewModel(serviceVm);
                    //                serviceVm.ComponentResultList = await GetServiceComponentList(serviceVm);
                    //                ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{serviceVm.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                    //                var viewName = serviceVm.Page.PageType.ToString();
                    //                return View(viewName, serviceVm);
                    //            }


                    //        }
                    //    }
                    //}
                    //if (page.DataAction == DataActionEnum.View)
                    //{
                    //    if (page.Template.ViewType == NtsViewTypeEnum.Email)
                    //    {
                    //        var model = await _serviceBusiness.GetServiceDetails(serviceVm);
                    //        model.ViewType = NtsViewTypeEnum.Email;
                    //        model.DataAction = DataActionEnum.View;
                    //        model.EmailList = await _componentResultBusiness.GetNtsEmailDetails(recordId, targetId, targetType);
                    //        ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{page.Portal.Theme.ToString()}/_Layout.cshtml";
                    //        return View("ServiceEmail", model);
                    //    }
                    //    else
                    //    {
                    //        serviceVm.ViewType = page.Template.ViewType;
                    //    }
                    //}
                    //else
                    if (page.DataAction == DataActionEnum.View && viewMode != NtsViewTypeEnum.Book)
                    {
                        // var template = await _templateBusiness.GetSingleById(page.TemplateId);
                        if (page.Template.ViewType == NtsViewTypeEnum.Book)
                        {
                            var model = await _serviceBusiness.GetBookDetails(recordId);
                            model.ViewType = NtsViewTypeEnum.Book;
                            model.DataAction = DataActionEnum.View;
                            ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{page.Portal.Theme.ToString()}/_Layout.cshtml";
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

        private IActionResult RedirectToIntroPage(ServiceTemplateViewModel serviceTemplate, PageViewModel page, string cbm)
        {
            if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Popup)
            {
                //if (serviceTemplate.ViewType == NtsViewTypeEnum.LightPage)
                //{
                //    var viewStr = await RenderViewToStringAsync(viewName, serviceTemplate, _contextAccessor, _razorViewEngine, _tempDataProvider);
                //    return Json(new { langJson = serviceTemplate.LanguageJson, view = viewStr, uiJson = serviceTemplate.Json, dataJson = serviceTemplate.DataJson, bc = serviceTemplate.Page.Breadcrumbs, mode = Convert.ToString(serviceTemplate.LayoutMode), page = serviceTemplate.Page, am = false });
                //}

                ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{serviceTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                var action = serviceTemplate.IntroPageAction;
                var controller = serviceTemplate.IntroPageController;
                var area = serviceTemplate.IntroPageArea;
                var prms = serviceTemplate.IntroPageParams;
                var url = "";
                if (area.IsNotNullAndNotEmpty())
                {
                    url = $"/{area}/{controller}/{action}?cbm={cbm}&templateCode={page.Template.Code}&{prms}";
                }
                else
                {
                    url = $"/{controller}/{action}?cbm={cbm}&templateCode={serviceTemplate.TemplateCode}&{prms}";
                }
                return Redirect(url);
                //return Redirect(action, controller, new { area = area, templateCode = serviceTemplate.TemplateCode });
            }
            return RedirectToAction("ddf");
            //else if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Iframe)
            //{
            //    serviceTemplate.ChartItems = await BuildChart(serviceTemplate.Json, serviceTemplate.DataJson, serviceTemplate.Prms, serviceTemplate.ServiceId, serviceTemplate.ServiceStatusCode);
            //    ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{serviceTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
            //    return View(viewName, serviceTemplate);
            //}
            //else if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Div)
            //{
            //    var viewStr = await RenderViewToStringAsync(viewName, serviceTemplate, _contextAccessor, _razorViewEngine, _tempDataProvider);
            //    return Json(new { langJson = serviceTemplate.LanguageJson, view = viewStr, uiJson = serviceTemplate.Json, dataJson = serviceTemplate.DataJson, bc = serviceTemplate.Page.Breadcrumbs, mode = Convert.ToString(serviceTemplate.LayoutMode), page = serviceTemplate.Page, am = false });
            //}
            //else
            //{
            //    serviceTemplate.ChartItems = await BuildChart(serviceTemplate.Json, serviceTemplate.DataJson, serviceTemplate.Prms, serviceTemplate.ServiceId, serviceTemplate.ServiceStatusCode);
            //    var viewStr = await RenderViewToStringAsync(viewName, serviceTemplate, _contextAccessor, _razorViewEngine, _tempDataProvider);
            //    return Json(new { langJson = serviceTemplate.LanguageJson, view = viewStr, uiJson = serviceTemplate.Json, dataJson = serviceTemplate.DataJson, bc = serviceTemplate.Page.Breadcrumbs, mode = Convert.ToString(serviceTemplate.LayoutMode), page = serviceTemplate.Page, am = true });
            //}
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
                ViewBag.Layout = string.Concat("~/Areas/Core/Views/Shared/_PopupLayout.cshtml");
            }
            if (portalId.IsNotNullAndNotEmpty())
            {
                if (lo == LayoutModeEnum.Iframe)
                {
                    var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(portalId);
                    if (portal != null)
                    {
                        ViewBag.Layout = string.Concat($"~/Areas/Core/Views/Shared/Themes/{portal.Theme}/_Layout.cshtml");
                    }
                }
            }
            ViewBag.ReportName = rptName;
            ViewBag.ReportUrl = $"{baseurl}{rptUrl}";
            ViewBag.ReportUrl2 = $"{baseurl}{rptUrl2}";
            ViewBag.ReportUrl3 = $"{baseurl}{rptUrl3}";
            //ViewBag.Layout = string.Concat("~/Areas/Core/Views/Shared/Themes/Recruitment/_Layout.cshtml");
            //ViewBag.ReportName = "Pms/PMS_LetterTemplate.trdp";
            //ViewBag.ReportName = "Pay/PaySlip.trdp";
            // ViewBag.ReportUrl = "https://localhost:44389/pms/query/GetLetterTemplateDetails/27f042e1-39dd-4877-a627-dc8bf8dc8140";
            return View();

        }
        [Route("Core/Cms/FastReportPdfFileId")]
        public async Task<IActionResult> FastReportPdfFileId(string rptName = null, string rptUrl = null, string rptUrl2 = null, string rptUrl3 = null, LayoutModeEnum lo = LayoutModeEnum.Main, string ru = null, string cbm = null, string portalId = null)
        {
            var webReport = await GetFastReport(rptName, rptUrl, rptUrl2, rptUrl3, lo, ru, cbm, portalId);
            webReport.Report.Prepare();

            PDFSimpleExport pdfExport = new PDFSimpleExport();
            using (MemoryStream ms = new MemoryStream())
            {
                pdfExport.Export(webReport.Report, ms);
                var result = await _fileBusiness.Create(new FileViewModel
                {
                    ContentByte = ms.ToArray(),
                    ContentType = "application/pdf",
                    ContentLength = ms.Length,
                    FileName = rptName + ".pdf",
                    FileExtension = ".pdf"
                });
                if (result.IsSuccess)
                {
                    return Json(new { success = true, fileId = result.Item.Id }); ;
                }
                else
                {
                    return Json(new { success = false }); ;
                }
            }
        }

        [Route("Core/Cms/FastReportPdf")]
        public async Task<IActionResult> FastReportPdf(string rptName = null, string rptUrl = null, string rptUrl2 = null, string rptUrl3 = null, LayoutModeEnum lo = LayoutModeEnum.Main, string ru = null, string cbm = null, string portalId = null)
        {
            var webReport = await GetFastReport(rptName, rptUrl, rptUrl2, rptUrl3, lo, ru, cbm, portalId);
            webReport.Report.Prepare();

            PDFSimpleExport pdfExport = new PDFSimpleExport();
            using (MemoryStream ms = new MemoryStream())
            {
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
                return File(ms.ToArray(), "application/pdf", rptName + ".pdf");
            }
        }
        public async Task<IActionResult> FastReport(string rptName, string rptUrl, string rptUrl2, string rptUrl3, LayoutModeEnum lo = LayoutModeEnum.Main, string ru = null, string cbm = null, string portalId = null, bool enableESign = false)
        {
            var webReport = await GetFastReport(rptName, rptUrl, rptUrl2, rptUrl3, lo, ru, cbm, portalId);
            ViewBag.rptName = rptName;
            ViewBag.rptUrl = rptUrl;
            ViewBag.rptUrl2 = rptUrl2;
            ViewBag.rptUrl3 = rptUrl3;
            ViewBag.lo = lo;
            ViewBag.ru = ru;
            ViewBag.cbm = cbm;
            ViewBag.EnableESign = enableESign;
            ViewBag.portalId = portalId;
            //webReport.Toolbar.Exports.ExportTypes = Exports.PS | Exports.Hpgl | Exports.Json | Exports.Pdf;
            return View(webReport);
        }
        public async Task<IActionResult> FastReportESign(string rptName, string rptUrl, string rptUrl2, string rptUrl3, LayoutModeEnum lo = LayoutModeEnum.Main, string ru = null, string cbm = null, string portalId = null, string referenceId = null)
        {
            var webReport = await GetFastReport(rptName, rptUrl, rptUrl2, rptUrl3, lo, ru, cbm, portalId);
            webReport.Report.Prepare();
            using (MemoryStream ms = new MemoryStream())
            {
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
                var base64 = Convert.ToBase64String(ms.ToArray());
                var fileName = rptName + ".pdf";
                var file = await _fileBusiness.Create(new FileViewModel
                {
                    FileName = fileName,
                    ContentByte = ms.ToArray(),
                    FileExtension = ".pdf",
                    ContentType = "application/pdf",
                });
                if (file.IsSuccess)
                {
                    await eSignPostData(fileName, base64, file.Item.MongoFileId, file.Item.Id, referenceId);
                }

            }
            return View();
        }
        public async Task eSignPostData(string documentName, string base64, string referenceNumber, string fileId, string referenceId)  //pdf file in bytes
        {
            List<eSignDocumentDetails> olsteSignDocumentDetails = new List<eSignDocumentDetails>();
            // Int64 TransactionID = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            Random rand = new Random();
            // Convert.ToBase64String(compressedBytes);
            eSignDocumentDetails oeSignDocumentDetails = new eSignDocumentDetails();
            int i = 1;
            oeSignDocumentDetails = new eSignDocumentDetails();
            oeSignDocumentDetails.DocumentName = documentName;
            var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
            oeSignDocumentDetails.DocumentURL = $"{baseurl}cms/document/getfilemongo?fileid={fileId}";
            oeSignDocumentDetails.PDFBase64 = base64;
            oeSignDocumentDetails.Page = "LAST";
            oeSignDocumentDetails.Coordinates = "Bottom_Right";
            oeSignDocumentDetails.Cosign = true;
            oeSignDocumentDetails.Pagenumbers = "";
            // 1,345,517,465,457  bottom right  "1,466,86,586,26";
            olsteSignDocumentDetails.Add(oeSignDocumentDetails);
            var doc = await _taskBusiness.Create<DocumentESignViewModel, DocumentESign>(new DocumentESignViewModel
            {
                ReferenceType = ReferenceTypeEnum.NTS_Service,
                ReferenceId = referenceId,
                DocumentFileId = fileId,
                DocumentReferenceNo = referenceNumber,
            });

            emSignerInputs oemSignerInputs = new emSignerInputs();
            oemSignerInputs.AuthToken = "666d9d5a-ecae-404b-adfe-c6a3ded9d6c5"; //      
            oemSignerInputs.CUrl = "https://esign.gujarat.gov.in/Cancel";

            oemSignerInputs.SUrl = $"{baseurl}cms/FastReportESignResponse";    /// local appication url     
            if (doc.IsSuccess)
            {
                oemSignerInputs.SUrl = $"{baseurl}cms/FastReportESignResponse/{doc.Item.Id}";
            }
            // oemSignerInputs.ReferenceNumber = TransactionID.ToString() + "test";
            oemSignerInputs.ReferenceNumber = referenceNumber;
            oemSignerInputs.Name = "Amit Patel";
            oemSignerInputs.eSign_SignerId = "AmiteSign";   ///sign id 
            oemSignerInputs.FUrl = "https://esign.gujarat.gov.in/Error";
            oemSignerInputs.IsCompressed = false;
            oemSignerInputs.IsCosign = true;
            oemSignerInputs.PreviewRequired = true;
            oemSignerInputs.Storetodb = false;
            oemSignerInputs.EnableViewDocumentLink = true;
            oemSignerInputs.Location = "ahmedabad";
            oemSignerInputs.Reason = "sample";

            oemSignerInputs.eSignDocumentdetails = olsteSignDocumentDetails;

            string URI = "https://esign.gujarat.gov.in/api/SigningRequest";

            ServicePointManager.Expect100Continue = true;
            // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            string jsonStringObj = JsonConvert.SerializeObject(oemSignerInputs);
            //jsonStringObj = "{\"eSignDocumentdetails\":[{\"DocumentName\":\"Documentname11\",\"DocumentURL\":\"https://localhost:44327/pdf/file-sample_150kB.pdf\",\"Page\":\"LAST\",\"Coordinates\":\"Bottom_Right\",\"Pagenumbers\":\"\",\"Cosign\":true,\"PDFBase64\":\"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MzMzIDI3OCAyNzggNTU2IDU1NiA1NTYgNTU2IDU1NiA1NTYgNTU2IDU1NiA1NTYgNTU2IDI3OCAyNzggNTg0IDU4NCA1ODQgNTU2IDEwMTUgNjY3IDY2NyA3MjIgNzIyIDY2NyA2MTEgNzc4IDcyMiAyNzggNTAwIDY2NyA1NTYgODMzIDcyMiA3NzggNjY3IDc3OCA3MjIgNjY3IDYxMSA3MjIgNjY3IDk0NCA2NjcgNjY3IDYxMSAyNzggMjc4IDI3OCA0NjkgNTUyIDMzMyA1NTYgNTU2IDUwMCA1NTYgNTU2IDI3OCA1NTYgNTU2IDIyOCAyMjggNTAwIDIyOCA4MzMgNTU2IDU1NiA1NTYgNTU2IDMzMyA1MDAgMjc4IDU1NiA1MDAgNzIyIDUwMCA1MDAgNTAwIDMzNCAyNjAgMzM0IDU4NCA1MDAgNTU2IDUwMCAyNzggNTU2IDM5MiA1NjUgNTU2IDU1NiAzMzMgMTAwMCA2NjcgMzMzIDEwMDAgNTAwIDYxMSA1MDAgNTAwIDIyMiAyMjIgMzMzIDMzMyAzNTAgMjkzIDU4NiAzMzMgNjg0IDUwMCAzMzMgOTQ0IDUwMCA1MDAgNjY3IDI2NiAyNzggNTU2IDU1NiA1NTYgNTU2IDI2MCA1NTYgMzMzIDczNyAzNzAgNTU2IDU4NCAzMzMgNzM3IDUwMCA0MDAgNTg0IDMzMyAzMzMgMzMzIDU1NiA1MzcgMjc4IDMzMyAzMzMgMzY1IDU1NiA4MzQgODM0IDgzNCA1NTYgNjY3IDY2NyA2NjcgNjY3IDY2NyA2NjcgMTAwMCA3MjIgNjY3IDY2NyA2NjcgNjY3IDI3OCAyNzggMjc4IDI3OCA3MjIgNzIyIDc3OCA3NzggNzc4IDc3OCA3NzggNTg0IDc3OCA3MjIgNzIyIDcyMiA3MjIgNjY3IDY2NyA2MTEgNTU2IDU1NiA1NTYgNTU2IDU1NiA1NTYgODg5IDUwMCA1NTYgNTU2IDU1NiA1NTYgMjI4IDIyOCAyMjggMjI4IDU1NiA1NTYgNTU2IDU1NiA1NTYgNTU2IDU1NiA1ODQgNTU2IDU1NiA1NTYgNTU2IDU1NiA1MDAgNTU2IDUwMF0+Pg0KZW5kb2JqDQo5IDAgb2JqDQo8PC9Bc2NlbnQgNzI4L0NhcEhlaWdodCAwL0Rlc2Nl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IDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MCAzOTAgMzkwIDM5MF0+Pg0KZW5kb2JqDQoxMiAwIG9iag0KPDwvQXNjZW50IDk3Ny9DYXBIZWlnaHQgMC9EZXNjZW50IDAvRmxhZ3MgMzIvRm9udEJCb3hbIDAgMCAzNjYgODc5XS9Gb250RmlsZTIgMTMgMCBSIC9Gb250TmFtZS9BQkNERUUrQ29kZSMyMDMjMjBkZSMyMDkvSXRhbGljQW5nbGUgMC9TdGVtViAwL1R5cGUvRm9udERlc2NyaXB0b3I+Pg0KZW5kb2JqDQoxMyAwIG9iag0KPDwvRmlsdGVyL0ZsYXRlRGVjb2RlL0xlbmd0aCA4ODUvTGVuZ3RoMSAyMDY0Pj5zdHJlYW0NClgJvVXNixxFFP919XyZHbOfhOgltfhxmmxmu2V1lhyyBplAls0yumE9SNIzXc4M9HzQs7vEYGDJMSgGhBb8C8R4zM0cRFDEk8RcYi4eFSSMmEBI2MRf1fTsTuJMEFHr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ABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFF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MC9Gb250RGVzY3JpcHRvciAyNCAwIFIgL0xhc3RDaGFyIDI1NS9TdWJ0eXBlL1RydWVUeXBlL1R5cGUvRm9udC9XaWR0aHNbIDUwMCAxMDAwIDUwMCA1MDAgNTAwIDUwMCA1MDAgNTAwIDUwMCA1MDAgMjU5IDUwMCA1MDAgMjU5IDUwMCA1MDAgNTAwIDUwMCA1MDAgNTAwIDUwMCA1MDAgNTAwIDUwMCA1MDAgNTAwIDUwMCA1MDAgNTAwIDUwMCA1MDAgNTAwIDI2NSAyNzcgMzU0IDU1NiA1NTYgODg5IDY2NiAxOTAgMzMzIDMzMyAzODkgNTgzIDI3NyAzMzMgMjc3IDI3NyA1NTYgNTU2IDU1NiA1NTYgNTU2IDU1NiA1NTYgNTU2IDU1NiA1NTYgMjc3IDI3NyA1ODMgNTgzIDU4MyA1NTYgMTAxNSA2NjYgNjY2IDcyMiA3MjIgNjY2IDYxMCA3NzcgNzIyIDI3NyA1MDAgNjY2IDU1NiA4MzMgNzIyIDc3NyA2NjYgNzc3IDcyMiA2NjYgNjEwIDcyMiA2NjYgOTQzIDY2NiA2NjYgNjEwIDI3NyAyNzcgMjc3IDQ2OSA1NTEgMzMzIDU1NiA1NTYgNTAwIDU1NiA1NTYgMjc3IDU1NiA1NTYgMjI4IDIyOCA1MDAgMjI4IDgzMyA1NTYgNTU2IDU1NiA1NTYgMzMzIDUwMCAyNzcgNTU2IDUwMCA3MjIgNTAwIDUwMCA1MDAgMzMzIDI1OSAzMzMgNTgzIDUwMCA1NTYgNTAwIDI3NyA1NTYgMzkxIDU2NSA1NTYgNTU2IDMzMyAxMDAwIDY2NiAzMzMgMTAwMCA1MDAgNjEwIDUwMCA1MDAgMjIyIDIyMiAzMzMgMzMzIDM1MCAyOTIgNTg1IDMzMyA2ODMgNTAwIDMzMyA5NDMgNTAwIDUwMCA2NjYgMjY1IDI3NyA1NTYgNTU2IDU1NiA1NTYgMjU5IDU1NiAzMzMgNzM2IDM3MCA1NTYgNTgzIDMzMyA3MzYgNTAwIDM5OSA1ODMgMzMzIDMzMyAzMzMgNTU2IDUzNyAyNzcgMzMzIDMzMyAzNjUgNTU2IDgzMyA4MzMgODMzIDU1NiA2NjYgNjY2IDY2NiA2NjYgNjY2IDY2NiAxMDAwIDcyMiA2NjYgNjY2IDY2NiA2NjYgMjc3IDI3NyAyNzcgMjc3IDcyMiA3MjIgNzc3IDc3NyA3NzcgNzc3IDc3NyA1ODMgNzc3IDcyMiA3MjIgNzIyIDcyMiA2NjYgNjY2IDYxMCA1NTYgNTU2IDU1NiA1NTYgNTU2IDU1NiA4ODkgNTAwIDU1NiA1NTYgNTU2IDU1NiAyMjggMjI4IDIyOCAyMjggNTU2IDU1NiA1NTYgNTU2IDU1NiA1NTYgNTU2IDU4MyA1NTYgNTU2IDU1NiA1NTYgNTU2IDUwMCA1NTYgNTAwXT4+DQplbmRvYmoNCjI0IDAgb2JqDQo8PC9Bc2NlbnQgNzI4L0NhcEhlaWdodCAwL0Rlc2NlbnQgLTIxMC9GbGFncyAzMi9Gb250QkJveFsgLTU4MCAtMjU3IDE0NzMgMTAwM10vRm9udEZpbGUyIDEwIDAgUiAvRm9udE5hbWUvQUJDREVFK01pY3Jvc29mdCMyMFNhbnMjMjBTZXJpZi9JdGFsaWNBbmdsZSAwL1N0ZW1WIDAvVHlwZS9Gb250RGVzY3JpcHRvcj4+DQplbmRvYmoNCjI1IDAgb2JqDQo8PC9CYXNlRm9udC9BQkNERUUrTWljcm9zb2Z0IzIwU2FucyMyMFNlcmlmL0Rlc2NlbmRhbnRGb250c1sgMjYgMCBSIF0vRW5jb2RpbmcvSWRlbnRpdHktSC9TdWJ0eXBlL1R5cGUwL1R5cGUvRm9udD4+DQplbmRvYmoNCjI2IDAgb2JqDQo8PC9CYXNlRm9udC9BQkNERUUrTWljcm9zb2Z0IzIwU2FucyMyMFNlcmlmL0NJRFN5c3RlbUluZm88PC9PcmRlcmluZyhJZGVudGl0eSkvUmVnaXN0cnkoQWRvYmUpL1N1cHBsZW1lbnQgMD4+L0ZvbnREZXNjcmlwdG9yIDI3IDAgUiAvU3VidHlwZS9DSURGb250VHlwZTIvVHlwZS9Gb250L1dbIDE4MlsgMjIyLjE2OF1dPj4NCmVuZG9iag0KMjcgMCBvYmoNCjw8L0FzY2VudCA3MjgvQ2FwSGVpZ2h0IDAvRGVzY2VudCAtMjEwL0ZsYWdzIDMyL0ZvbnRCQm94WyAtNTgwIC0yNTcgMTQ3MyAxMDAzXS9Gb250RmlsZTIgMTAgMCBSIC9Gb250TmFtZS9BQkNERUUrTWljcm9zb2Z0IzIwU2FucyMyMFNlcmlm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+OCaaeaHk5EbnaQOfuqVU12Hw5/4JH/C/wAKeVPrn9seLL8MXlkvbopHKxXB+RMcZyeSTz1NfUtFAHJ/Dv4E+DfhNp8Vt4c8NaPpMcJDKYLZRIWAxuL43Fsdyc1L8YPhBoXx18AXvhnxJateaVf7TIiyFGDKQysGHIIIBH0rp6KAOY+D3wl0f4GfDvT/AAvoKTx6VpgcQLNKZHG92c5Y9eWNdPRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAIyhxggEehFKBgUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFAH//2Q0KZW5kc3RyZWFtDQplbmRvYmoNCnhyZWYNCjAgMzMNCjAwMDAwMDAwMDAgNjU1MzUgZg0KMDAwMDAwMDAxNyAwMDAwMCBuDQowMDAwMDAwMDY2IDAwMDAwIG4NCjAwMDAwMDAxMzAgMDAwMDAgbg0KMDAwMDAwMDIxNyAwMDAwMCBuDQowMDAwMDAwNDI1IDAwMDAwIG4NCjAwMDAwMDM5NzIgMDAwMDAgbg0KMDAwMDAwNDA2OCAwMDAwMCBuDQowMDAwMDA0MTU5IDAwMDAwIG4NCjAwMDAwMDUzNjAgMDAwMDAgbg0KMDAwMDAwNTU2MCAwMDAwMCBuDQowMDAwMDcyMjY2IDAwMDAwIG4NCjAwMDAwNzM0NTcgMDAwMDAgbg0KMDAwMDA3MzY0MCAwMDAwMCBuDQowMDAwMDc0NjExIDAwMDAwIG4NCjAwMDAwNzQ2NjMgMDAwMDAgbg0KMDAwMDA5NzczNyAwMDAwMCBuDQowMDAwMDk3OTIzIDAwMDAwIG4NCjAwMDAwOTgxOTQgMDAwMDAgbg0KMDAwMDEwMTc1NiAwMDAwMCBuDQowMDAwMTAxODgwIDAwMDAwIG4NCjAwMDAxMDIwNzIgMDAwMDAgbg0KMDAwMDEwMjI2MSAwMDAwMCBuDQowMDAwMTQzMDExIDAwMDAwIG4NCjAwMDAxNDQyMTQgMDAwMDAgbg0KMDAwMDE0NDQxNSAwMDAwMCBuDQowMDAwMTQ0NTUwIDAwMDAwIG4NCjAwMDAxNDQ3NTMgMDAwMDAgbg0KMDAwMDE0NDk1NCAwMDAwMCBuDQowMDAwMTQ1MDcwIDAwMDAwIG4NCjAwMDAxNDUyNjcgMDAwMDAgbg0KMDAwMDE0NTQ0OSAwMDAwMCBuDQowMDAwMjEzNDk3IDAwMDAwIG4NCnRyYWlsZXINCjw8DQovUm9vdCAxIDAgUg0KL0luZm8gMyAwIFINCi9TaXplIDMzL0lEWzxBRTJFODQ3RDZGQTAyRUZFNTRGQTFFQUNGOURFNkQ3Rj48QUUyRTg0N0Q2RkEwMkVGRTU0RkExRUFDRjlERTZEN0Y+XT4+DQpzdGFydHhyZWYNCjIxNjI5NQ0KJSVFT0YNCg==\"}],\"Name\":\"Amit Patel\",\"AuthToken\":\"666d9d5a-ecae-404b-adfe-c6a3ded9d6c5\",\"PreviewRequired\":true,\"SUrl\":\"https://localhost:44327/ProductionWebResponse.aspx\",\"FUrl\":\"https://esign.gujarat.gov.in/Error\",\"CUrl\":\"https://esign.gujarat.gov.in/Cancel\",\"ReferenceNumber\":\"1890222446\",\"IsCompressed\":false,\"IsCosign\":true,\"EnableViewDocumentLink\":true,\"Storetodb\":false,\"eSign_SignerId\":\"AmiteSign\",\"Reason\":\"sample\",\"Location\":\"ahmedabad\"}";
            var retValue = await Helper.HttpRequest(URI, HttpVerb.Post, jsonStringObj);
            var Params = JsonConvert.DeserializeObject<ServiceReturn>(retValue);
            if (doc.IsSuccess)
            {
                var item = await _taskBusiness.GetSingleById<DocumentESignViewModel, DocumentESign>(doc.Item.Id);
                item.Key = Params.SessionKey.ToString();
                item.Transaction = Params.Transaction.ToString();
                await _taskBusiness.Edit<DocumentESignViewModel, DocumentESign>(item);
            }
            ClientRequest ObjClientRequest = new ClientRequest();
            ObjClientRequest = Params.ReturnValue;
            var input = new NameValueCollection();
            input.Add("Parameter1", ObjClientRequest.Parameter1);
            input.Add("Parameter2", ObjClientRequest.Parameter2);
            input.Add("Parameter3", ObjClientRequest.Parameter3);
            await Post(input);
        }


        [Route("Cms/FastReportESignResponse/{id}")]
        public async Task<IActionResult> FastReportESignResponse(string id, string returnValue)
        {
            var esingResp = new eSignresponse();
            var item = await _taskBusiness.GetSingleById<DocumentESignViewModel, DocumentESign>(id);

            var fileName = "Esign.pdf";

            esingResp.Signeddata = returnValue;
            if (item != null)
            {
                esingResp.Transaction = item.Transaction;
                esingResp.SessionKey = item.Key;

                var doc = await _taskBusiness.GetSingleById<DocumentViewModel, Document>(item.DocumentFileId);
                if (doc != null)
                {
                    fileName = doc.Name;
                }
            }


            var url = "https://esign.gujarat.gov.in/api/SigningResponse";
            var data = JsonConvert.SerializeObject(esingResp);
            var retValue = await Helper.HttpRequest(url, HttpVerb.Post, data);
            if (retValue.IsNotNullAndNotEmpty())
            {
                var ds = ObjectToDataSet(retValue);
                if (ds != null)
                {
                    string dsval = Convert.ToString(ds.Tables[0].Rows[0]["ReturnValue"]);
                    if (dsval.IsNotNullAndNotEmpty())
                    {
                        var bytes = Convert.FromBase64String(dsval.ToString());
                        var file = await _fileBusiness.Create(new FileViewModel
                        {
                            FileName = "Esign.pdf",
                            ContentByte = bytes,
                            FileExtension = ".pdf",
                            ContentType = "application/pdf",
                        });
                        if (file.IsSuccess && item != null)
                        {
                            var queryBusiness = _sp.GetService<ICmsQueryBusiness>();
                            await queryBusiness.UpdateMarriageCerfificateFile(item.ReferenceId, file.Item.Id);
                            return View(true);
                        }

                    }

                }

            }
            return View(false);

        }
        private DataSet ObjectToDataSet(string json)
        {
            DataSet ds = new DataSet();

            try
            {
                string jsonresponse = "{'RDATA':" + json + "}";

                var xmlDoc = JsonConvert.DeserializeXmlNode(jsonresponse);
                var xmlReader = new XmlNodeReader(xmlDoc);
                ds.ReadXml(xmlReader);
            }
            catch (Exception ex)
            {
                //IB2Cdb.Insert_ErrorLog("ConvertJsonStringToDataSet:" + ex.ToString());
            }
            return ds;
        }


        private async Task<FastReport.Web.WebReport> GetFastReport(string rptName, string rptUrl, string rptUrl2, string rptUrl3, LayoutModeEnum lo = LayoutModeEnum.Main, string ru = null, string cbm = null, string portalId = null)
        {
            var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = string.Concat("~/Areas/Core/Views/Shared/_PopupLayout.cshtml");
            }
            if (portalId.IsNotNullAndNotEmpty())
            {
                if (lo == LayoutModeEnum.Iframe)
                {
                    var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(portalId);
                    if (portal != null)
                    {
                        ViewBag.Layout = string.Concat($"~/Areas/Core/Views/Shared/Themes/{portal.Theme}/_Layout.cshtml");
                    }
                }
            }
            var root = _sp.GetService<Microsoft.Extensions.Hosting.IHostEnvironment>().ContentRootPath;
            string dir = Path.Combine(root, "Reports");
            if (!Directory.Exists(dir))
            {
                var splt = root.Split("\\");
                if (splt.Length > 0)
                {
                    dir = string.Join("\\", splt.Take(splt.Length - 1));
                    dir = Path.Combine(dir, "Synergy.App.Core", "Reports");
                }
            }
            var webReport = new FastReport.Web.WebReport();

            webReport.Report.Load(Path.Combine(dir, $"{rptName}.frx"));

            if (rptUrl.IsNotNullAndNotEmpty())
            {

                var connection = webReport.Report.Dictionary.Connections[0].ConnectionString;
                var splt = connection.Split(";");
                var url = $"{baseurl}{rptUrl}";
                var byt = System.Text.Encoding.UTF8.GetBytes(url);
                var enc = Convert.ToBase64String(byt);
                webReport.Report.Dictionary.Connections[0].ConnectionString = $"Json={enc};{splt[1]}";

            }
            if (rptUrl2.IsNotNullAndNotEmpty())
            {
                var connection = webReport.Report.Dictionary.Connections[1].ConnectionString;
                var splt = connection.Split(";");
                var url = $"{baseurl}{rptUrl2}";
                var byt = System.Text.Encoding.UTF8.GetBytes(url);
                var enc = Convert.ToBase64String(byt);
                webReport.Report.Dictionary.Connections[1].ConnectionString = $"Json={enc};{splt[1]}";
            }
            if (rptUrl3.IsNotNullAndNotEmpty())
            {
                var connection = webReport.Report.Dictionary.Connections[2].ConnectionString;
                var splt = connection.Split(";");
                var url = $"{baseurl}{rptUrl3}";
                var byt = System.Text.Encoding.UTF8.GetBytes(url);
                var enc = Convert.ToBase64String(byt);
                webReport.Report.Dictionary.Connections[2].ConnectionString = $"Json={enc};{splt[1]}";
            }
            return webReport;
        }



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
                var uId = "";
                if (Request.HttpContext.User.Identity.IsAuthenticated == true && _userContext.IsGuestUser == false)
                {
                    uId = _userContext.UserId;
                }
                page = await _pageBusiness.GetDefaultPageDataByPortal(portal, uId, runningMode);
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
                    ViewBag.Layout = $"~/Areas/Core/Views/Shared/_EmptyLayout.cshtml";
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
                    ViewBag.Layout = $"~/Areas/Core/Views/Shared/_EmptyLayout.cshtml";
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
                MenuViewModel landingPage = null;
                var lp = await _userBusiness.GetUserLandingPage(userId, portal.Id);
                if (lp != null)
                {
                    landingPage = new MenuViewModel
                    {
                        Id = page.Id,
                        Name = page.Name,
                        DisplayName = page.Name
                    };
                }
                if (landingPage == null)
                {
                    landingPage = menus.FirstOrDefault(x => x.IsRoot == true);
                }
                if (landingPage == null)
                {
                    landingPage = menus.FirstOrDefault();
                }
                //if (landingPage == null)
                //{
                //    landingPage = new MenuViewModel
                //    {
                //        Name = "Default",
                //        DisplayName="Home",
                //        Men
                //    };
                //}
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
            ViewBag.UserRoleCodes = _userContext.UserRoleCodes;

            ViewBag.Title = string.Concat(page.Title, " - ", portal.DisplayName);
            ViewBag.Portal = portal;
            if (portal.PortalFooterTeext.IsNotNullAndNotEmpty())
            {
                ViewBag.Footer = portal.PortalFooterTeext.Replace("{{year}}", DateTime.Today.Year.ToString());
            }

            ViewBag.FavIconId = portal.FavIconId;
            ViewBag.ThemeName = portal.Theme;
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
                ViewBag.LanguageImageId = lang?.ImageId;
                var le = legalEntities.FirstOrDefault(x => x.Id == _userContext.LegalEntityId);
                ViewBag.LegalEntityName = le?.Name;

                var prtl = ap.FirstOrDefault(x => x.Id == portal.Id);
                if (prtl != null)
                {
                    ViewBag.PortalDisplayName = prtl.Name;
                }
                var msgs = await _notificationBusiness.GetAllNotifications(_userContext.UserId, portal.Id, 10, false, ReadStatusEnum.NotRead);
                ViewBag.TopMessages = msgs.ToList();
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


            return new Tuple<bool, ApplicationIdentityUser>(true, new ApplicationIdentityUser { Id = _userContext.UserId, CompanyId = _userContext.CompanyId });
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
                ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{page.Portal.Theme.ToString()}/_Layout.cshtml";
                return View("Form", page);
            }
            else if (page.LayoutMode.HasValue && page.LayoutMode == LayoutModeEnum.Iframe)
            {
                ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{page.Portal.Theme.ToString()}/_Layout.cshtml";
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
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{formTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
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
                    await SetLocalizedUI(formTemplate);
                    if (formTemplate.DataJson.IsNotNull())
                    {
                        formTemplate.DataJson = await AttachmentConfiguration(formTemplate.Page.Template.Json, formTemplate.DataJson);
                    }
                    if (formTemplate.Json.IsNotNullAndNotEmpty())
                    {
                        formTemplate.Json = formTemplate.Json.Replace("SaveFileInFormIo", "SaveFileInFormIo?referenceId=" + formTemplate.Id + "&referenceType=" + ReferenceTypeEnum.Form);
                        formTemplate.Json = formTemplate.Page.Template.Json = _webApi.AddHost(formTemplate.Json);
                        ReplaceFormIoJsonDataForForm(formTemplate);
                        SetLocalizedUdf(formTemplate);
                    }

                    if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Popup)
                    {
                        formTemplate.ChartItems = await BuildChart(formTemplate.Json, formTemplate.DataJson, formTemplate.Prms, formTemplate.RecordId);
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{formTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                        return View("Form", formTemplate);
                    }
                    else if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Iframe)
                    {
                        formTemplate.ChartItems = await BuildChart(formTemplate.Json, formTemplate.DataJson, formTemplate.Prms, formTemplate.RecordId);
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{formTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                        return View("Form", formTemplate);
                    }
                    else if (formTemplate.LayoutMode.HasValue && formTemplate.LayoutMode == LayoutModeEnum.Div)
                    {
                        var viewStr = await RenderViewToStringAsync(viewName.ToString(), viewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { view = viewStr, uiJson = formTemplate.Json, dataJson = formTemplate.DataJson, bc = formTemplate.Page.Breadcrumbs, page = formTemplate.Page, am = false, localizedUI = formTemplate.LocalizedUI });
                    }
                    else
                    {
                        var viewStr = await RenderViewToStringAsync(viewName.ToString(), viewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { view = viewStr, uiJson = formTemplate.Json, dataJson = formTemplate.DataJson, bc = formTemplate.Page.Breadcrumbs, page = formTemplate.Page, am = true, localizedUI = formTemplate.LocalizedUI });
                    }
                }
            }
        }

        private async Task SetLocalizedUI(FormTemplateViewModel formTemplate)
        {
            var ui = "";
            if (formTemplate.Page.Portal != null)
            {
                var allowedLanguages = formTemplate.Page.Portal.AllowedLanguageIds;
                var lngs = await _lovBusiness.GetList(x => x.LOVType == "LANGUAGES");
                var localizedData = await _portalBusiness.GetSingle<FormResourceLanguageViewModel, FormResourceLanguage>(x => x.FormTableId == formTemplate.RecordId);
                if (allowedLanguages != null && allowedLanguages.Any())
                {
                    var arabic = lngs.FirstOrDefault(x => x.Code == "ar-SA" && allowedLanguages.Any(y => y == x.Id));
                    if (arabic != null)
                    {
                        ui = $"{ui}<div class='form-label-group p-2'><input lang='ar-SA' value='{localizedData?.Arabic}' type='text' id='lang-Arabic' class='input-lang form-control' placeholder='Arabic(عربي)' /><label for='lang-Arabic'>Arabic(عربي)</label></div>";
                    }
                    var hindi = lngs.FirstOrDefault(x => x.Code == "hi-IN" && allowedLanguages.Any(y => y == x.Id));
                    if (hindi != null)
                    {
                        ui = $"{ui}<div class='form-label-group p-2'><input lang='hi-IN' value='{localizedData?.Hindi}' type='text' id='lang-Hindi' class='input-lang form-control' placeholder='Hindi(हिन्दी)' /><label for='lang-Hindi'>Hindi(हिन्दी)</label></div>";
                    }
                }

            }
            formTemplate.LocalizedUI = ui;
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
                ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{page.Portal.Theme.ToString()}/_Layout.cshtml";
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
                if (viewModel.ActionName == "LoadIframePage")
                {
                    viewModel.Parameter = "src=" + HttpUtility.UrlEncode(viewModel.Parameter);
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
            model.TemplateCode = page.TemplateCodes;

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
            model.TemplateCode = formTemplate.TemplateCode;
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

                    if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Popup)
                    {
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{taskTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";

                        return View(viewName.ToString(), indexViewModel);
                    }
                    else
                    {
                        var viewStr = await RenderViewToStringAsync(viewName.ToString(), indexViewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { view = viewStr, uiJson = taskTemplate.Json, dataJson = "{}", bc = taskTemplate.Page.Breadcrumbs, mode = Convert.ToString(taskTemplate.LayoutMode), page = taskTemplate.Page });
                    }

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
                    if (taskTemplate.CanComplete && taskTemplate.Page.DataAction != DataActionEnum.Create)
                    {
                        taskTemplate.DataAction = DataActionEnum.Edit;
                        taskTemplate.Page.DataAction = DataActionEnum.Edit;
                        taskTemplate.Page.RequestSource = RequestSourceEnum.Edit;
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
                    }
                    else
                    {
                        taskTemplate.Page.Template.Json = taskTemplate.Json = "{}";
                    }
                    if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Popup)
                    {
                        taskTemplate.ChartItems = await BuildChart(taskTemplate.Json, taskTemplate.DataJson, taskTemplate.Prms, taskTemplate.TaskId, NtsTypeEnum.Task, taskTemplate.ParentServiceId, taskTemplate.TaskStatusCode);
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{taskTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                        if (taskTemplate.TaskTemplateType == TaskTypeEnum.StepTask &&
                            (taskTemplate.ServiceViewType == NtsViewTypeEnum.Default || taskTemplate.ServiceViewType == NtsViewTypeEnum.Email
                            || taskTemplate.ServiceViewType == null)
                            && taskTemplate.ServiceBaseTemplate != null && taskTemplate.ParentService != null)
                        {
                            var serviceTemplate = new ServiceTemplateViewModel
                            {
                                ServiceId = taskTemplate.ParentService.Id,
                                ServiceNo = taskTemplate.ParentService.ServiceNo,
                                TemplateDisplayName = taskTemplate.ServiceBaseTemplate.DisplayName,
                                ServiceStatusName = taskTemplate.ParentService.ServiceStatusName,
                                OwnerUserId = taskTemplate.ParentService.OwnerUserId,
                                OwnerUserName = taskTemplate.ParentService.OwnerUserUserName,
                                ServiceStatusCode = taskTemplate.ParentService.ServiceStatusCode,
                                StartDate = taskTemplate.ParentService.StartDate,
                                DueDate = taskTemplate.ParentService.DueDate,
                                InitialItemType = ProcessDesignComponentTypeEnum.StepTask,
                                InitialItemId = taskTemplate.TaskId,
                                TaskTemplate = taskTemplate,
                                ViewType = NtsViewTypeEnum.Default,
                                TemplateCode = taskTemplate.ParentService.TemplateCode

                            };
                            //taskTemplate.ViewType = NtsViewTypeEnum.Light;
                            serviceTemplate.ComponentResultList = await GetServiceComponentList(serviceTemplate);
                            return View("Service", serviceTemplate);
                        }

                        //if (serviceTemplate.ViewType == NtsViewTypeEnum.Light)
                        //{
                        //    serviceTemplate.ComponentResultList = await GetServiceComponentList(serviceTemplate);
                        //    serviceTemplate.InitialItemId = serviceTemplate.ServiceId;
                        //    serviceTemplate.InitialComponentType = ProcessDesignComponentTypeEnum.Service;
                        //}

                        return View("Task", taskTemplate);
                    }
                    else if (taskTemplate.LayoutMode.HasValue && taskTemplate.LayoutMode == LayoutModeEnum.Iframe)
                    {
                        taskTemplate.ChartItems = await BuildChart(taskTemplate.Json, taskTemplate.DataJson, taskTemplate.Prms, taskTemplate.TaskId, NtsTypeEnum.Task, taskTemplate.ParentServiceId, taskTemplate.TaskStatusCode);
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{taskTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
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
            model.ViewType = serviceTemplate.ViewType;
            model.RequestSource = serviceTemplate.Page.RequestSource;

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


                    if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Popup)
                    {
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{serviceTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";

                        return View(viewName.ToString(), indexViewModel);
                    }
                    else
                    {
                        var viewStr = await RenderViewToStringAsync(viewName.ToString(), indexViewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { view = viewStr, uiJson = serviceTemplate.Json, dataJson = "{}", bc = serviceTemplate.Page.Breadcrumbs, mode = Convert.ToString(serviceTemplate.LayoutMode), page = serviceTemplate.Page });

                    }

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
                    if (serviceTemplate.IsInEditMode && serviceTemplate.Page.DataAction != DataActionEnum.Create)
                    {
                        serviceTemplate.DataAction = DataActionEnum.Edit;
                        serviceTemplate.Page.DataAction = DataActionEnum.Edit;
                        serviceTemplate.Page.RequestSource = RequestSourceEnum.Edit;
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
                    serviceTemplate.LanguageJson = await GetLanguageJson(serviceTemplate);
                    if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Popup)
                    {
                        if (serviceTemplate.ViewType == NtsViewTypeEnum.Email)
                        {
                            serviceTemplate.ViewType = NtsViewTypeEnum.Default;
                        }
                        if (serviceTemplate.ViewType == NtsViewTypeEnum.Default || serviceTemplate.ViewType == null)
                        {
                            serviceTemplate.ComponentResultList = await GetServiceComponentList(serviceTemplate);
                            serviceTemplate.InitialItemId = serviceTemplate.ServiceId;
                            serviceTemplate.InitialItemType = ProcessDesignComponentTypeEnum.Service;
                        }
                        serviceTemplate.ChartItems = await BuildChart(serviceTemplate.Json, serviceTemplate.DataJson, serviceTemplate.Prms, serviceTemplate.ServiceId, NtsTypeEnum.Service, null, serviceTemplate.ServiceStatusCode);
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{serviceTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";

                        return View(viewName, serviceTemplate);
                    }
                    else if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Iframe)
                    {
                        serviceTemplate.ChartItems = await BuildChart(serviceTemplate.Json, serviceTemplate.DataJson, serviceTemplate.Prms, serviceTemplate.ServiceId, NtsTypeEnum.Service, null, serviceTemplate.ServiceStatusCode);
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{serviceTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                        return View(viewName, serviceTemplate);
                    }
                    else if (serviceTemplate.LayoutMode.HasValue && serviceTemplate.LayoutMode == LayoutModeEnum.Div)
                    {
                        var viewStr = await RenderViewToStringAsync(viewName, serviceTemplate, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { langJson = serviceTemplate.LanguageJson, view = viewStr, uiJson = serviceTemplate.Json, dataJson = serviceTemplate.DataJson, bc = serviceTemplate.Page.Breadcrumbs, mode = Convert.ToString(serviceTemplate.LayoutMode), page = serviceTemplate.Page, am = false });
                    }
                    else
                    {
                        serviceTemplate.ChartItems = await BuildChart(serviceTemplate.Json, serviceTemplate.DataJson, serviceTemplate.Prms, serviceTemplate.ServiceId, NtsTypeEnum.Service, null, serviceTemplate.ServiceStatusCode);
                        var viewStr = await RenderViewToStringAsync(viewName, serviceTemplate, _contextAccessor, _razorViewEngine, _tempDataProvider);
                        return Json(new { langJson = serviceTemplate.LanguageJson, view = viewStr, uiJson = serviceTemplate.Json, dataJson = serviceTemplate.DataJson, bc = serviceTemplate.Page.Breadcrumbs, mode = Convert.ToString(serviceTemplate.LayoutMode), page = serviceTemplate.Page, am = true });
                    }

                }
            }
        }
        [HttpPost]
        [Route("Core/Cms/GetServiceContent")]
        public async Task<IActionResult> GetServiceContent([FromBody] ServiceTemplateViewModel serviceTemplateViewModel)
        {

            var viewStr = await RenderViewToStringAsync("ServiceLightPage", serviceTemplateViewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
            return Json(new { success = false, view = viewStr });
        }
        [HttpPost]
        [Route("Core/Cms/GetStepTaskContent")]
        public async Task<IActionResult> GetStepTaskContent([FromBody] TaskTemplateViewModel taskTemplateViewModel)
        {
            var viewStr = await RenderViewToStringAsync("TaskLightPage", taskTemplateViewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);
            return Json(new { success = false, view = viewStr });
        }
        private async Task<string> GetLanguageJson(ServiceTemplateViewModel serviceTemplate)
        {
            var json = JObject.Parse($"{{}}");
            var lang = _userContext.CultureName;
            if (lang != "en-US")
            {
                json.Add("language", lang);
                var resourceData = await _cmsBusiness.GetList<ResourceLanguageViewModel, ResourceLanguage>(x => x.TemplateId == serviceTemplate.TemplateId);
                if (lang == "hi-IN")
                {
                    var values = resourceData.Select(x => $"'{x.English}':'{(x.Hindi.IsNullOrEmpty() ? x.English : x.Hindi)}'");
                    var hindi = JObject.Parse(@$"{{'{lang}':{{
                        {string.Join(',', values)}
                    }}}}");
                    json.Add("i18n", hindi);
                }
                else if (lang == "ar-SA")
                {
                    var values = resourceData.Select(x => $"'{x.English}':'{(x.Arabic.IsNullOrEmpty() ? x.English : x.Arabic)}'");
                    var arabic = JObject.Parse(@$"{{'{lang}':{{
                        {string.Join(',', values)}
                    }}}}");
                    json.Add("i18n", arabic);
                }

            }
            return json.ToString();
        }

        private async Task<List<ComponentResultViewModel>> GetServiceComponentList(ServiceTemplateViewModel serviceTemplate)
        {
            var list = await _componentResultBusiness.GetComponentResultList(serviceTemplate.ServiceId);
            if (list == null)
            {
                list = new List<ComponentResultViewModel>();
            }
            list = list.Where(x => x.ComponentType == ProcessDesignComponentTypeEnum.StepTask && x.ComponentStatusCode != "COMPONENT_STATUS_DRAFT" && x.NtsTaskId != null).ToList();
            if (serviceTemplate.ActiveUserType == NtsActiveUserTypeEnum.Assignee)
            {
                if (serviceTemplate.WorkflowVisibility == WorkflowVisibilityEnum.ShowAllToServiceOwner)
                {
                    list = new List<ComponentResultViewModel>();
                }
            }
            else if (serviceTemplate.ActiveUserType == NtsActiveUserTypeEnum.Owner || serviceTemplate.ActiveUserType == NtsActiveUserTypeEnum.Requester
                || serviceTemplate.ActiveUserType == NtsActiveUserTypeEnum.OwnerOrRequester)
            {
                if (serviceTemplate.WorkflowVisibility == WorkflowVisibilityEnum.ShowAllToTaskAssignee)
                {
                    list = new List<ComponentResultViewModel>();
                }
            }
            var serviceItem = new ComponentResultViewModel
            {
                TemplateMasterCode = serviceTemplate.TemplateCode,
                ComponentType = ProcessDesignComponentTypeEnum.Service,
                TaskId = serviceTemplate.ServiceId,
                TaskNo = serviceTemplate.ServiceNo,
                TaskSubject = serviceTemplate.ServiceSubject.Coalesce(serviceTemplate.TemplateDisplayName),
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
            var dt = await _noteBusiness.GetNoteIndexPageGridData(null, indexPageTemplateId, ownerType, noteStatusCode, ignoreJoins: false);
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
                    if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Popup)
                    {
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{noteTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";

                        return View(viewName.ToString(), indexViewModel);
                    }
                    else
                    {
                        return Json(new { view = viewStr, uiJson = noteTemplate.Json, dataJson = "{}", bc = noteTemplate.Page.Breadcrumbs, mode = Convert.ToString(noteTemplate.LayoutMode), page = noteTemplate.Page });

                    }
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
                        noteTemplate.ChartItems = await BuildChart(noteTemplate.Json, noteTemplate.DataJson, noteTemplate.Prms, noteTemplate.NoteId, NtsTypeEnum.Note, null, noteTemplate.NoteStatusCode);
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{noteTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
                        return View("Note", noteTemplate);
                    }
                    else if (noteTemplate.LayoutMode.HasValue && noteTemplate.LayoutMode == LayoutModeEnum.Iframe)
                    {
                        noteTemplate.ChartItems = await BuildChart(noteTemplate.Json, noteTemplate.DataJson, noteTemplate.Prms, noteTemplate.NoteId, NtsTypeEnum.Note, null, noteTemplate.NoteStatusCode);
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{noteTemplate.Page.Portal.Theme.ToString()}/_Layout.cshtml";
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
        private void SetLocalizedUdf(FormTemplateViewModel formTemplate)
        {
            if (formTemplate.Json.IsNotNullAndNotEmpty() && formTemplate.DisplayColumnId.IsNotNullAndNotEmpty())
            {
                var result = JObject.Parse(formTemplate.Json);
                var components = (JArray)result.SelectToken("components");
                foreach (JObject jcomp in components)
                {
                    var columnMetadataId = Convert.ToString(jcomp.SelectToken("columnMetadataId"));
                    if (formTemplate.DisplayColumnId == columnMetadataId)
                    {
                        jcomp.Add("suffix", "<i class='fa fa-language choose-lang'></i>");
                        formTemplate.Json = result.ToString();
                        return;
                    }
                }

            }

        }
        [HttpPost]
        public async Task<IActionResult> GetPreviewJsonWithData([FromBody] TemplateViewModel model)
        {
            try
            {
                //return Json(new { success = true, json = model.Json });
                var result = JObject.Parse(model.Json);
                var components = (JArray)result.SelectToken("components");
                var controls = new List<JObject>();
                var data = JObject.Parse(model.DataJson);
                GetColumnComponents(components, data, controls);
                var rows = JArray.Parse(@$"[]");
                if (controls.Any())
                {
                    foreach (var item in controls)
                    {
                        var label = Convert.ToString(item.SelectToken("label"));
                        var key = Convert.ToString(item.SelectToken("key"));
                        var id = Convert.ToString(item.SelectToken("columnMetadataId"));
                        var props = item.Properties();
                        if (!props.Any(x => x.Name == "hideLabel"))
                        {
                            item.Add("hideLabel", true);
                        }
                        if (!props.Any(x => x.Name == "disabled"))
                        {
                            item.Add("disabled", true);
                        }
                        if (props.Any(x => x.Name == "validate"))
                        {
                            item.Remove("validate");
                        }
                        if (!props.Any(x => x.Name == "customClass"))
                        {
                            item.Add("customClass", "formio-display");
                        }
                        if (!props.Any(x => x.Name == "id"))
                        {
                            item.Add("id", id);
                        }
                        var value = data[key];
                        if (value != null)
                        {
                            if (props.Any(x => x.Name == "defaultValue"))
                            {
                                item.Remove("defaultValue");
                            }
                            item.Add("defaultValue", value);
                        }
                        var labelObj = JObject.Parse(@$"  {{
        ""components"": [
          {{
                    ""label"": ""HTML"",
            ""content"": ""{label}"",
            ""key"": ""html"",
            ""type"": ""htmlelement"",
            ""id"":""label_{id}""
          }}
        ]
      }}");
                        var controlObj = JObject.Parse(@$"{{""components"": [] }}");
                        var ctrlComp = (JArray)controlObj.SelectToken("components");
                        ctrlComp.Add(item);
                        var row = JArray.Parse(@$"[]");
                        row.Add(labelObj);
                        row.Add(controlObj);
                        rows.Add(row);
                    }
                    var ret = JArray.Parse(@$"[]");
                    var obj = JObject.Parse(@$"{{
  ""label"": ""Preview"",
  ""cellAlignment"": ""left"",
  ""striped"": true,
  ""bordered"": true,
  ""condensed"": true,
  ""key"": ""table"",
  ""type"": ""table"",
  ""numRows"": {controls.Count},
  ""numCols"": 2,
  ""input"": false,
  ""tableView"": false, 
  ""id"":""sdsdsdsdsd""
}}");

                    obj.Add("rows", rows);
                    ret.Add(obj);
                    result.Remove("components");
                    result.Add("components", JArray.FromObject(ret));
                    result.Remove("display");
                    result.Add("display", "form");
                    model.Json = result.ToString();
                    return Json(new { success = true, json = model.Json });
                }
                else
                {
                    return Json(new { success = true, json = new { } });
                }


            }
            catch (Exception e)
            {

                throw;
            }

        }
        private void GetColumnComponents(JArray comps, JObject data, List<JObject> controls)
        {
            try
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
                            || type == "datetime" || type == "day" || type == "time" || type == "currency" || type == "button"
                            || type == "signature" || type == "file" || type == "hidden" || type == "datagrid" || type == "editgrid"
                            || (type == "htmlelement" && key == "chartgrid") || (type == "htmlelement" && key == "chartJs"))
                        {

                            try
                            {
                                var reserve = jcomp.SelectToken("reservedKey");
                                if (reserve == null)
                                {
                                    var columnId = jcomp.SelectToken("columnMetadataId");
                                    if (columnId != null)
                                    {
                                        controls.Add(jcomp);
                                        //var newProperty = new JProperty("defaultValue", model.Udfs.GetValue(columnMeta.Name));
                                        //jcomp.Add(newProperty);
                                    }

                                }
                            }
                            catch (Exception we)
                            {

                                throw;
                            }

                        }

                        else if (type == "columns")
                        {
                            try
                            {
                                JArray cols = (JArray)jcomp.SelectToken("columns");
                                foreach (var col in cols)
                                {
                                    JArray rows = (JArray)col.SelectToken("components");
                                    if (rows != null)
                                        GetColumnComponents(rows, data, controls);
                                }
                            }
                            catch (Exception e)
                            {


                            }

                        }
                        else if (type == "panel")
                        {
                            try
                            {
                                var pComps = (JArray)jcomp.SelectToken("components");
                                if (pComps != null)
                                {
                                    GetColumnComponents(pComps, data, controls);
                                }
                            }
                            catch (Exception e)
                            {

                            }


                        }
                        else if (type == "table")
                        {
                            try
                            {
                                var rows = (JArray)jcomp.SelectToken("rows");
                                foreach (var row in rows)
                                {
                                    if (row != null)
                                    {
                                        foreach (JToken cell in row.Children())
                                        {
                                            var comp = (JArray)cell.SelectToken("components");
                                            GetColumnComponents(rows, data, controls);
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {

                                throw;
                            }

                        }
                        else
                        {
                            JArray rows = (JArray)jcomp.SelectToken("components");
                            if (rows != null)
                                GetColumnComponents(rows, data, controls);
                        }
                    }
                }
            }
            catch (Exception e)
            {

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
                            var type = "";
                            var gridkey = "";
                            if (data.SelectToken("type").IsNotNull())
                            {
                                type = data.SelectToken("type").ToString();
                            }
                            string fileId = "";
                            var key = row.SelectToken("key").ToString();
                            var dataJrow = JToken.Parse(dataJson);
                            if (type == "datagrid")
                            {
                                gridkey = data.SelectToken("key").ToString();
                                var gridrows = (JArray)dataJrow.SelectToken(gridkey);
                                for (int i = 0; i < gridrows.Count; i++)
                                {
                                    var gridrowsdata = Newtonsoft.Json.JsonConvert.DeserializeObject(gridrows[i].ToString());
                                    var rowValue = JToken.Parse(gridrowsdata.ToString());
                                    if (rowValue[key].IsNotNull())
                                    {
                                        fileId = Convert.ToString(rowValue[key]);
                                    }
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
                                            gridrows[i][key] = jarrayObj;
                                        }
                                    }
                                    else
                                    {
                                        gridrows[i][key] = new JArray();

                                    }
                                }
                                jsonObj[gridkey] = gridrows;

                            }
                            else
                            {
                                fileId = Convert.ToString(dataJrow.SelectToken(key));
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
                        else if (row.SelectToken("type") != null && row.SelectToken("type").ToString() == "editgrid")
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
                        else if (row.SelectToken("type") != null && row.SelectToken("type").ToString() == "checkbox")
                        {
                            var key = row.SelectToken("key").ToString();
                            var dataJrow = JToken.Parse(dataJson);
                            var str = dataJrow.SelectToken(key).ToString();
                            if (str.IsNotNullAndNotEmpty())
                            {
                                var newstr = Convert.ToBoolean(str);
                                jsonObj[key] = newstr;
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
        private async Task<string> BuildChartJson(JArray comps, string dataJson, string chartCum, Dictionary<string, string> prms, string ntsId, NtsTypeEnum? ntsType = null, string taskServiceId = null, string ntsStatusCode = null)
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
                                chartCum = await BuildChartJson(rows, dataJson, chartCum, prms, ntsId, ntsType, taskServiceId, ntsStatusCode);
                        }
                    }
                    else if (type == "tabs")
                    {

                        JArray cols = (JArray)jcomp.SelectToken("components");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                chartCum = await BuildChartJson(rows, dataJson, chartCum, prms, ntsId, ntsType, taskServiceId, ntsStatusCode);
                        }

                    }
                    else if (type == "table")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("rows");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                chartCum = await BuildChartJson(rows, dataJson, chartCum, prms, ntsId, ntsType, taskServiceId, ntsStatusCode);
                        }
                    }
                    else
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                        {
                            chartCum = await BuildChartJson(rows, dataJson, chartCum, prms, ntsId, ntsType, taskServiceId, ntsStatusCode);
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
                            //var timeDimensions = new List<DashboardItemTimeDimensionViewModel>();
                            //if (dashboardItem.timeDimensionsField.IsNotNullAndNotEmpty())
                            //{
                            //    timeDimensions = JsonConvert.DeserializeObject<List<DashboardItemTimeDimensionViewModel>>(dashboardItem.timeDimensionsField);

                            //}
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
                            if (chartInput.Contains("^^NtsTaskServiceId^^"))
                            {

                                taskServiceId = '"' + taskServiceId + '"';
                                chartInput = chartInput.Replace("^^NtsTaskServiceId^^", taskServiceId);
                            }
                            if (chartInput.Contains("^^NtsType^^"))
                            {
                                var _ntsType = '"' + ntsType.ToString() + '"';
                                chartInput = chartInput.Replace("^^NtsType^^", _ntsType);
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
                                filstr += "]";
                                chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + metadataArray[3];
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
                            }
                            if (chartNamear.Length > 1 && chartNamear[1].Contains("Iframe"))
                            {
                                chartInput = chartInput.Replace("\"", string.Empty);
                            }
                            var chartBPCode = dashboardItem.boilerplateCode;
                            chartBPCode = chartBPCode.Replace("@@input@@", chartInput.Replace("\"\"", "\""));
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

                            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataJson);
                            var lat = rowData != null && rowData.GetValueOrDefault("Latitude").IsNotNull() ? rowData.GetValueOrDefault("Latitude").ToString() : "34.0837";
                            var lon = rowData != null && rowData.GetValueOrDefault("Longitude").IsNotNull() ? rowData.GetValueOrDefault("Longitude").ToString() : "74.7973";
                            var add = rowData != null && rowData.GetValueOrDefault("specificLocation").IsNotNull() ? rowData.GetValueOrDefault("specificLocation").ToString() : "";

                            chartBPCode = chartBPCode.Replace("@@Latitude@@", lat);
                            chartBPCode = chartBPCode.Replace("@@Longitude@@", lon);
                            chartBPCode = chartBPCode.Replace("@@LocationName@@", add.Replace("\n", " ").Replace("\r", " ").Replace(Environment.NewLine, " "));
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
        private async Task<string> BuildChart(string json, string dataJson, Dictionary<string, string> prms, string ntsId, NtsTypeEnum? ntsType = null, string taskServiceId = null, string ntsStatusCode = null)
        {
            var chartCum = "";
            if (json.IsNotNullAndNotEmptyAndNotValue("{}"))
            {
                var data = JToken.Parse(json);
                JArray rows = (JArray)data.SelectToken("components");
                chartCum = await BuildChartJson(rows, dataJson, chartCum, prms, ntsId, ntsType, taskServiceId, ntsStatusCode);
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
                        || type == "datetime" || type == "day" || type == "time" || type == "currency" || type == "button"
                        || type == "signature" || type == "file" || type == "datagrid" || type == "editgrid" || (type == "htmlelement" && key == "chartgrid") || (type == "htmlelement" && key == "chartJs"))
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
                                if (type == "datagrid" || type == "editgrid")
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

        [Route("Core/Cms/GetUserIdNameList")]
        public async Task<ActionResult> GetUserIdNameList(string viewData = null, string portalId = null)
        {
            var data = await _userBusiness.GetUserIdNameList();
            //if (portalId.IsNotNullAndNotEmpty())
            //{
            //    var data1 = await _userPortalBusiness.GetUserByPortal(portalId);
            //    data = data1.Select(x => new IdNameViewModel() { Id = x.Id, Name = x.Name }).ToList();
            //}
            if (viewData != null)
            {
                ViewData[viewData] = data;
            }
            return Json(data);
        }
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
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
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

        [Route("Core/Cms/MasterList")]
        public IActionResult MasterList(string groupCode)
        {
            ViewBag.GroupCode = groupCode;
            return View();
        }

        public async Task<IActionResult> ReadMasterList(string groupCode)
        {
            var data = await _templateBusiness.GetMasterList(groupCode);

            //var result = new List<TemplateViewModel>();
            //var templates = new List<TemplateViewModel>();

            //templates.Add(new TemplateViewModel()
            //{
            //    DisplayName = "Template 1",
            //    Code = "TEMP1",
            //    TemplateType = TemplateTypeEnum.Note
            //});
            //templates.Add(new TemplateViewModel()
            //{
            //    DisplayName = "Template 2",
            //    Code = "TEMP2",
            //    TemplateType = TemplateTypeEnum.Note
            //});

            //var templates1 = new List<TemplateViewModel>();

            //templates1.Add(new TemplateViewModel()
            //{
            //    DisplayName = "Template 1.1",
            //    Code = "TEMP1.1",
            //    TemplateType = TemplateTypeEnum.Service
            //});
            //templates1.Add(new TemplateViewModel()
            //{
            //    DisplayName = "Template 2.1",
            //    Code = "TEMP2.1",
            //    TemplateType = TemplateTypeEnum.Service
            //});

            //result.Add(new TemplateViewModel()
            //{
            //    TemplateCategoryName = "Template Category 1",
            //    TemplatesList = templates
            //});
            //result.Add(new TemplateViewModel()
            //{
            //    TemplateCategoryName = "Template Category 2",
            //    TemplatesList = templates1
            //});

            return Json(data);
        }

        [Route("Core/Cms/TemplatesList")]
        public async Task<IActionResult> TemplatesList(string portalId, bool canCreateTemplate = false)
        {
            var userPortals = await _userBusiness.GetAllowedPortalList(_userContext.UserId);
            string portals = string.Join(",", userPortals.Select(x => x.Id).ToList());

            ViewBag.PortalId = portals;
            ViewBag.CanCreateTemplate = canCreateTemplate;
            return View();
        }

        public async Task<IActionResult> ReadTemplatesList(string portalId, TemplateTypeEnum templateType)
        {
            var data = await _templateBusiness.GetTemplatesList(portalId, templateType);

            return Json(data);
        }
        public async Task Post(NameValueCollection _inputs)
        {

            string URL = "https://esign.gujarat.gov.in/eMsecure/V3_0/Index";
            string Method = "POST";
            string FormName = "emSignerPost";
            _contextAccessor.HttpContext.Response.Clear();
            _contextAccessor.HttpContext.Response.Clear();
            await _contextAccessor.HttpContext.Response.WriteAsync("<html><head>");
            await _contextAccessor.HttpContext.Response.WriteAsync(String.Format(System.Globalization.CultureInfo.InvariantCulture, "</head><body onload=document.{0}.submit()>", FormName));
            await _contextAccessor.HttpContext.Response.WriteAsync(String.Format(System.Globalization.CultureInfo.InvariantCulture, "<form name=\"{0}\" method=\"{1}\" action=\"{2}\" target=\"{3}\">", FormName, Method, URL, "_blank"));

            int length = _inputs.Keys.Count;
            for (int i = 0; i <= length - 1; i++)
            {
                await _contextAccessor.HttpContext.Response.WriteAsync(String.Format(System.Globalization.CultureInfo.InvariantCulture, "<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", _inputs.Keys[i], _inputs.Get(i)));
            }

            await _contextAccessor.HttpContext.Response.WriteAsync("</form>");
            await _contextAccessor.HttpContext.Response.WriteAsync("</body></html>");
            await _contextAccessor.HttpContext.Response.CompleteAsync();
        }

        public IActionResult CaptureImageAndVideo(string cbm, CaptureType type)
        {
            ViewBag.CallbackMethod = cbm;
            ViewBag.CaptureType = type;
            return View();
        }
        public async Task<IActionResult> NtsForm(string portalId, string ntsId, NtsTypeEnum type)
        {
            if (type == NtsTypeEnum.Note)
            {
                var temp = await _noteBusiness.GetSingleById(ntsId);

                var noteTemplate = await _cmsBusiness.GetSingle<NoteTemplateViewModel, NoteTemplate>(x => x.TemplateId == temp.TemplateId);
                if (temp.TemplateCode.IsNotNullAndNotEmpty())
                {
                    var page = await _pageBusiness.GetDefaultPageByTemplate(portalId, temp.TemplateCode);
                    noteTemplate.Page = page;
                }
                var model = await _noteBusiness.GetNoteDetails(noteTemplate);

                model.Page = noteTemplate.Page;
                model.PageId = noteTemplate.Page.Id;
                //model.DataAction = noteTemplate.Page.DataAction;
                model.NoteId = model.RecordId = model.Page.RecordId = ntsId;



                var vn0 = noteTemplate.Page.PageType;

                var data0 = await GetNoteModel(model, vn0);
                var vs1 = await RenderViewToStringAsync(model.Page.PageType.ToString(), model, _contextAccessor, _razorViewEngine, _tempDataProvider);
                if (model.Json.IsNotNullAndNotEmpty())
                {
                    model.Json = _webApi.AddHost(model.Json);
                    model.Json = SetUdfView(model.Json);
                }
                model.DataJson = data0;
                return View(model);
            }
            else if (type == NtsTypeEnum.Task)
            {
                var temp = await _taskBusiness.GetSingleById(ntsId);

                var noteTemplate = await _cmsBusiness.GetSingle<TaskTemplateViewModel, TaskTemplate>(x => x.TemplateId == temp.TemplateId);
                if (temp.TemplateCode.IsNotNullAndNotEmpty())
                {
                    var page = await _pageBusiness.GetDefaultPageByTemplate(portalId, temp.TemplateCode);
                    noteTemplate.Page = page;
                }
                var model = await _taskBusiness.GetTaskDetails(noteTemplate);

                model.Page = noteTemplate.Page;
                model.PageId = noteTemplate.Page.Id;
                //model.DataAction = noteTemplate.Page.DataAction;
                model.TaskId = model.RecordId = model.Page.RecordId = ntsId;



                var vn0 = noteTemplate.Page.PageType;

                var data0 = await GetTaskModel(model, vn0);
                var vs1 = await RenderViewToStringAsync(model.Page.PageType.ToString(), model, _contextAccessor, _razorViewEngine, _tempDataProvider);
                if (model.Json.IsNotNullAndNotEmpty())
                {
                    model.Json = _webApi.AddHost(model.Json);
                    model.Json = SetUdfView(model.Json);
                }
                model.DataJson = data0;
                var data = new NoteTemplateViewModel { Json = model.Json, DataJson = model.DataJson };
                return View(data);
            }
            else if (type == NtsTypeEnum.Service)
            {
                var temp = await _serviceBusiness.GetSingleById(ntsId);

                var noteTemplate = await _cmsBusiness.GetSingle<ServiceTemplateViewModel, ServiceTemplate>(x => x.TemplateId == temp.TemplateId);
                if (temp.TemplateCode.IsNotNullAndNotEmpty())
                {
                    var page = await _pageBusiness.GetDefaultPageByTemplate(portalId, temp.TemplateCode);
                    noteTemplate.Page = page;
                }
                var model = await _serviceBusiness.GetServiceDetails(noteTemplate);
                model.RecordId = ntsId;

                model.Page = noteTemplate.Page;
                model.PageId = noteTemplate.Page.Id;
                //model.DataAction = noteTemplate.Page.DataAction;
                model.ServiceId = model.RecordId = model.Page.RecordId = ntsId;



                var vn0 = noteTemplate.Page.PageType;

                var data0 = await GetServiceModel(model, vn0);
                var vs1 = await RenderViewToStringAsync(model.Page.PageType.ToString(), model, _contextAccessor, _razorViewEngine, _tempDataProvider);
                if (model.Json.IsNotNullAndNotEmpty())
                {
                    model.Json = _webApi.AddHost(model.Json);
                    model.Json = SetUdfView(model.Json);
                }
                model.DataJson = data0;
                var data = new NoteTemplateViewModel { Json = model.Json, DataJson = model.DataJson };
                return View(data);
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ReplyTask(NtsEmailViewModel tmodel)
        {
            var _serviceTemp = _sp.GetService<IServiceTemplateBusiness>();
            var _taskTemp = _sp.GetService<ITaskTemplateBusiness>();
            //var template = await _serviceTemp.GetSingleById(tmodel.TemplateId);
            //if (template != null)
            //{
            //var subtaskid = template.AdhocTaskTemplateIds;
            //var tasktemplate = await _taskTemp.GetSingleById(subtaskid[0]);
            //var emailtemp = await _templateBusiness.GetSingleById(tasktemplate.TemplateId);
            if (tmodel.Action != "ACCEPT_RESOLUTION")
            {
                if (tmodel.CCs.IsNotNull() && tmodel.CCs.Count > 0)
                {
                    tmodel.Tos.AddRange(tmodel.CCs);
                }
                foreach (var item in tmodel.Tos)
                {
                    var model = new TaskTemplateViewModel();
                    model.DataAction = DataActionEnum.Create;
                    model.ActiveUserId = _userContext.UserId;
                    if (tmodel.Action == "REQUEST_ACCEPTION")
                    {
                        model.TemplateCode = "INCIDENT_ACCEPTANCE";
                    }
                    else
                    {
                        model.TemplateCode = "INCIDENT_REPLY";
                    }
                    var taskmodel = await _taskBusiness.GetTaskDetails(model);



                    var lov = await _lovBusiness.GetSingle(x => x.Code == "TASK_ASSIGN_TO_USER");
                    if (lov.IsNotNull())
                    {
                        taskmodel.AssignedToTypeId = lov.Id;
                        taskmodel.AssignedToTypeCode = lov.Code;
                        taskmodel.AssignedToTypeName = lov.Name;
                    }
                    taskmodel.TaskDescription = tmodel.Body;
                    taskmodel.TaskSubject = tmodel.Subject;
                    taskmodel.AssignedToUserId = item;
                    taskmodel.OwnerUserId = _userContext.UserId;
                    taskmodel.ServicePlusId = tmodel.ServiceId;

                    if (tmodel.TargetType == NtsEmailTargetTypeEnum.Service)
                    {
                        taskmodel.ReferenceType = ReferenceTypeEnum.NTS_Service;
                        taskmodel.ParentServiceId = tmodel.NtsId;
                    }
                    else
                    {
                        taskmodel.ReferenceType = ReferenceTypeEnum.NTS_Task;
                        var task = await _taskBusiness.GetSingleById(tmodel.NtsId);
                        if (task != null)
                        {
                            taskmodel.ParentServiceId = task.ParentServiceId;
                        }
                    }
                    taskmodel.ReferenceId = tmodel.NtsId;
                    taskmodel.Json = "{}";
                    taskmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                    taskmodel.StartDate = DateTime.Now;
                    var service = await _serviceBusiness.GetSingleById(tmodel.ServiceId);
                    taskmodel.TaskPriorityId = service.ServicePriorityId;

                    taskmodel.DataAction = DataActionEnum.Create;
                    var result = await _taskBusiness.ManageTask(taskmodel);
                }
            }
            if (tmodel.TargetType != NtsEmailTargetTypeEnum.StepTask)
            {
                TaskTemplateViewModel tasknewmodel = new TaskTemplateViewModel();
                tasknewmodel.TaskId = tmodel.NtsId;
                tasknewmodel.DataAction = DataActionEnum.Edit;
                var taskData = await _taskBusiness.GetTaskDetails(tasknewmodel);
                if (taskData != null)
                {
                    taskData.TaskStatusCode = "TASK_STATUS_COMPLETE";
                    var res = await _taskBusiness.ManageTask(taskData);
                }
            }
            //if (result.IsSuccess)
            //{
            //    return Json(new { success = true });
            //}
            //}
            return Json(new { success = true });
        }

        public async Task<IActionResult> PropertyIndex()
        {
            return View();
        }

        public async Task<IActionResult> EscalateTask()
        {
            await _componentResultBusiness.EscalateTask();
            return View();
        }

        public async Task<IActionResult> GetPropertyData()
        {
            var data = await _cmsBusiness.GetPropertyData(_userContext.UserId);

            return Json(data);
        }

        //[Route("Core/Cms/QRCodeReader")]
        //public async Task<IActionResult> QRCodeReader(string qrCodeId= "5d6bad60-4c43-4e57-adce-131b675d3f00")
        //{
        //    var data = await _cmsBusiness.GetSingleById<QRCodeDataViewModel,QRCodeData>(qrCodeId);
        //    return View(data);
        //}
    }
    public class eSignDocumentDetails
    {
        // public eSignDocumentDetails();

        public string DocumentName { get; set; }
        public string DocumentURL { get; set; }
        public string Page { get; set; }
        public string Coordinates { get; set; }
        public string Pagenumbers { get; set; }
        public bool Cosign { get; set; }
        public string PDFBase64 { get; set; }
    }
    public class emSignerInputs
    {
        //  public emSignerInputs();

        public List<eSignDocumentDetails> eSignDocumentdetails { get; set; }
        public string Name { get; set; }
        public string AuthToken { get; set; }
        public bool PreviewRequired { get; set; }
        public string SUrl { get; set; }
        public string FUrl { get; set; }
        public string CUrl { get; set; }
        public string ReferenceNumber { get; set; }
        public bool IsCompressed { get; set; }
        public bool IsCosign { get; set; }
        public bool EnableViewDocumentLink { get; set; }
        public bool Storetodb { get; set; }
        public string eSign_SignerId { get; set; }
        public string Reason { get; set; }
        public string Location { get; set; }
    }
    public class ServiceReturn
    {
        // public ServiceReturn();
        public ClientRequest ReturnValue { get; set; }
        //  public object ReturnValue { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public long Transaction { get; set; }
        public Status ReturnStatus { get; set; }
        public string SessionKey { get; set; }

        public enum Status
        {
            Success = 0,
            Failure = 1
        }

    }

    public class ClientRequest
    {
        public string Parameter1 { get; set; }
        public string Parameter2 { get; set; }
        public string Parameter3 { get; set; }
    }
    public class eSignresponse
    {
        public string Signeddata { get; set; }
        public string SessionKey { get; set; }
        public string Transaction { get; set; }
    }
}
