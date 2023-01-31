using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Api.Controllers;
using System.Data;
using System.IO;
using Synergy.App.Api.Areas.Cms.Models;

namespace Synergy.App.Api.Areas.CMS.Controllers
{
    [Route("cms/workboard")]
    [ApiController]
    public class WorkboardController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        //private readonly IFileBusiness _fileBusiness;
        //private readonly ITableMetadataBusiness _tableMetadataBusiness;
        //private readonly IHttpContextAccessor _accessor;

        private readonly IWorkboardBusiness _workboardBusiness;

        private readonly INoteBusiness _noteBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        //private readonly ICmsBusiness _cmsBusiness;
        private readonly INotificationBusiness _notificationBusiness;
        private readonly INotificationTemplateBusiness _notificationTemplateBusiness;
        //private readonly ILOVBusiness _lovBusiness;
        //private readonly ITemplateBusiness _templateBusiness;
        private INtsNoteCommentBusiness _ntsNoteCommentBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;


        public WorkboardController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IServiceProvider serviceProvider, INoteBusiness noteBusiness, ITaskBusiness taskBusiness,
          
            ITableMetadataBusiness tableMetadataBusiness, ICmsBusiness cmsBusiness, INotificationBusiness notificationBusiness, ILOVBusiness lovBusiness,
            ITemplateBusiness templateBusiness, Microsoft.Extensions.Configuration.IConfiguration configuration,
            IWorkboardBusiness workboardBusiness,  INotificationTemplateBusiness notificationTemplateBusiness, INtsNoteCommentBusiness ntsNoteCommentBusiness
          
            //IHttpContextAccessor accessor ,  IFileBusiness fileBusiness, ITableMetadataBusiness tableMetadataBusiness
            ) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _workboardBusiness = workboardBusiness;
            _taskBusiness = taskBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            //_cmsBusiness = cmsBusiness;
            _notificationBusiness = notificationBusiness;
            _notificationTemplateBusiness = notificationTemplateBusiness;
            //_lovBusiness = lovBusiness;
            //_templateBusiness = templateBusiness;
            _ntsNoteCommentBusiness = ntsNoteCommentBusiness;
            _noteBusiness = noteBusiness;
        }

        [HttpGet]
        [Route("GetSharedWorkboardLists")]
        public async Task<ActionResult> GetSharedWorkboardLists(string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var model = await _workboardBusiness.GetSharedWorkboardList(WorkBoardstatusEnum.Open, userId);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetWorkboardTaskList")]
        public async Task<ActionResult> GetWorkboardTaskList(string userId, string portalName, string status)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var model = await _taskBusiness.GetWorkboardTaskList(portalId: _context.PortalId, templateCodes: "WB_TASK");
            if (status.IsNotNullAndNotEmpty())
            {
                model = model.Where(x => x.TaskStatusCode == status).ToList();
            }
            return Ok(model);
        }

        [HttpGet]
        [Route("GetWorkboardDashboardList")]
        public async Task<ActionResult> GetWorkboardDashboardList(string userId, string portalName, WorkBoardstatusEnum status)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var model = await _workboardBusiness.GetWorkboardList(status);
            return Ok(model);
        }

        [HttpGet]
        [Route("CreateWorkboard")]
        public async Task<ActionResult> CreateWorkboard(string workBoardId)
        {WorkBoardViewModel model = new WorkBoardViewModel();
            if (workBoardId.IsNotNullAndNotEmpty())
            {
                model.WorkboardId = workBoardId;
                var existingmodel = await _workboardBusiness.GetWorkBoardDetails(workBoardId);
                model.WorkBoardName = existingmodel.WorkBoardName;
                model.DataAction = DataActionEnum.Edit;
                return Ok(model);
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                var generalWorkBoardTemplate = await _tableMetadataBusiness.GetTableDataByColumn("WB_TEMPLATE_TYPE", "", "TemplateTypeCode", "BASIC");
                if (generalWorkBoardTemplate.IsNotNull())
                {
                    model.TemplateTypeId = Convert.ToString(generalWorkBoardTemplate["Id"]);
                }
                model.WorkBoardStatus = WorkBoardstatusEnum.Open;
                return Ok(model);
            }
        }

        [HttpGet]
        [Route("ChooseTemplate")]
        public async Task<ActionResult> ChooseTemplate()
        {
            var templateCategories = await _workboardBusiness.GetTemplateCategoryList();
            var categories = templateCategories;
            var model = await _workboardBusiness.GetTemplateList();
            return Ok(model);
        }

        [HttpPost]
        [Route("ManageWorkBoard")]
        public async Task<ActionResult> ManageWorkBoard(WorkBoardViewModel model)
        {
            await Authenticate(model.RequestedByUserId, model.PortalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            if (ModelState.IsValid)
            {
                //var WorkBoardTemplate = await _tableMetadataBusiness.GetTableDataByColumn("WB_TEMPLATE_TYPE", "", "Id", model.TemplateTypeId);
                //if (WorkBoardTemplate.IsNotNull())
                //{
                //    model.JsonContent = Convert.ToString(WorkBoardTemplate["SampleContent"]);
                //}
                if (model.DataAction == DataActionEnum.Create)
                {
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _context.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "WORK_BOARD";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    model.WorkBoardStatus = WorkBoardstatusEnum.Open;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        newmodel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel
                        {
                            NoteId = result.Item.NoteId,
                            DataAction = DataActionEnum.Edit,
                            ActiveUserId = _context.UserId,
                            SetUdfValue = true
                        });
                        var rowData1 = newmodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        rowData1["JsonContent"] = await _workboardBusiness.GetJsonContent(model.TemplateTypeId, result.Item.UdfNoteTableId, model.WorkBoardDate);
                        rowData1["WorkBoardStatus"] = "0";
                        var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                        var update = await _noteBusiness.EditNoteUdfTable(result.Item, data1, result.Item.UdfNoteTableId);
                        return Ok(new { success = true, result = result.Item });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _context.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "WORK_BOARD";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;

                    var workboardModel = new WorkBoardViewModel()
                    {
                        WorkBoardName = model.WorkBoardName,
                        //model.WorkBoardStatus = WorkBoardstatusEnum.Open

                    };

                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(workboardModel);

                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Ok(new { success = true, result = result.Item });
                    }
                }
                else
                {
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _context.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        var rowData1 = result.Item.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        if (Convert.ToString(rowData1["JsonContent"]).IsNullOrEmpty())
                        {
                            var jsonContent = await _workboardBusiness.GetJsonContent(model.TemplateTypeId, result.Item.UdfNoteTableId);
                            rowData1["JsonContent"] = jsonContent;
                            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                            var update = await _noteBusiness.EditNoteUdfTable(result.Item, data1, result.Item.UdfNoteTableId);
                        }
                        //ViewBag.Success = true;
                        return Ok(new { success = true });
                    }
                }

            }
            return Ok(new { success = false, error = ModelState.ToString()});

        }

        [HttpGet]
        [Route("ManageWorkBoardDetails")]
        public async Task<ActionResult> ManageWorkBoardDetails(string userId, string portalName, string id)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var model = await _workboardBusiness.GetWorkBoardDetails(id);
            if (_context.IsSystemAdmin)
            {
                model.OwnerType = WorkBoardOwnerTypeEnum.Admin;
            }
            else if (_context.UserId == model.RequestedByUserId || _context.UserId == model.OwnerUserId)
            {
                model.OwnerType = WorkBoardOwnerTypeEnum.OwnerOrRequester;
            }
            else if (model.WorkBoardTeamIds.IsNotNull() && model.WorkBoardTeamIds.Contains(_context.UserId))
            {
                model.OwnerType = WorkBoardOwnerTypeEnum.TeamMember;
            }
            else
            if (model.WorkBoardContributionType.IsNotNull())
            {
                model.OwnerType = model.WorkBoardContributionType == WorkBoardContributionTypeEnum.Contributer ? WorkBoardOwnerTypeEnum.Contributor : WorkBoardOwnerTypeEnum.Viewer;
            }
            if (model == null)
            {
                return Ok(model);
            }
            //model.JsonContent = await _workboardBusiness.GetJsonContent1(model.TemplateTypeId, model.Id, ownerType: model.OwnerType);
            var list = await _workboardBusiness.GetSectionContent(model.TemplateTypeId, model.Id, ownerType: model.OwnerType);
            model.WorkBoardSections = new List<WorkBoardSectionViewModel>();
            model.WorkBoardSections.AddRange(list);
            model.DataAction = DataActionEnum.Edit;
            return Ok(model);
        }

        [HttpGet]
        [Route("WorkBoardSection")]
        public async Task<ActionResult> WorkBoardSection(string sectionId, string workBoardId)
        {
            var model = new WorkBoardSectionViewModel();
            if (sectionId.IsNotNullAndNotEmpty())
            {
                model = await _workboardBusiness.GetWorkBoardSectionDetails(sectionId);
                if (model != null)
                {
                    model.DataAction = DataActionEnum.Edit;
                    return Ok(model);
                }
            }
            if (workBoardId.IsNotNullAndNotEmpty())
            {
                model.WorkBoardId = workBoardId;
            }
            model.DataAction = DataActionEnum.Create;
            return Ok(model);
        }

        [HttpPost]
        [Route("ManageWorkBoardSection")]
        public async Task<ActionResult> ManageWorkBoardSection(WorkBoardSectionViewModel model)
        {
            await Authenticate(model.OwnerUserId, model.PortalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            if (ModelState.IsValid)
            {
                var templateModel = new NoteTemplateViewModel();
                if (model.DataAction == DataActionEnum.Create)
                {
                    templateModel.ActiveUserId = _context.UserId;
                    templateModel.DataAction = DataActionEnum.Create;
                    templateModel.TemplateCode = "WB_SECTION";

                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.DataAction = DataActionEnum.Create;
                    newmodel.TemplateCode = "WB_SECTION";
                    newmodel.ActiveUserId = _context.UserId;

                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        model.WorkBoardSectionId = result.Item.UdfNoteTableId;
                        model.NtsNoteId = result.Item.NoteId;
                        //model.NoteId = newmodel.NoteId;
                        return Ok(new { success = true, data = model });
                    }
                }
                else
                {
                    templateModel.ActiveUserId = _context.UserId;
                    templateModel.DataAction = DataActionEnum.Edit;
                    templateModel.NoteId = model.NtsNoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);

                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        var wbSection = await _workboardBusiness.GetWorkBoardSectionDetails(result.Item.UdfNoteTableId);
                        return Ok(new { success = true, data = wbSection });
                    }
                }
            }
            return Ok(new { success = false, error = ModelState.ToString() });
        }

        [HttpGet]
        [Route("WorkboardInvite")]
        public async Task<ActionResult> WorkboardInvite(string workBoardId, string ntsId)
        {
            WorkBoardInviteViewModel model = new WorkBoardInviteViewModel();
            model.Invites = await _workboardBusiness.GetWorkBoardInvites(workBoardId);
            var notificationTemplateModel = await _notificationTemplateBusiness.GetSingle(x => x.Code == "WORKBOARD_INVITE");
            if (notificationTemplateModel.IsNotNull())
            {
                model.TemplateContent = notificationTemplateModel.Body;
            }
            //// model.NoteId = Guid.NewGuid().ToString();
            // //model.WorkBoardUniqueId = RandomString(6);
            //// model.WorkBoardKey = RandomString(6);
            // var wbDetails = await _workboardBusiness.GetWorkBoardDetails(workBoardId);
            // model.WorkBoardUniqueId = wbDetails.WorkboardUniqueId;
            // model.WorkBoardKey = wbDetails.ShareKey;
            // var url = string.Empty;
            // var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
            // var urlnew = "WorkBoard/Invite/" + wbDetails.WorkboardUniqueId + "/" + wbDetails.ShareKey;
            // url = baseurl + urlnew;
            // model.Link = url;
            // var templateModel = new NoteTemplateViewModel();

            // model.WorkBoardId = workBoardId;
            // var WorkBoardTemplate = await _tableMetadataBusiness.GetTableDataByColumn("WORK_BOARD", "", "Id", model.WorkBoardId);
            // model.NoteId = Convert.ToString(WorkBoardTemplate["NtsNoteId"]);
            // //model.NoteId = ntsId;
            // model.WorkBoardSharingType = WorkBoardSharingTypeEnum.Link;
            // model.DataAction = DataActionEnum.Create;
            return Ok(model);
        }

        [HttpPost]
        [Route("ManageWorkboardInvite")]

        public async Task<IActionResult> ManageWorkboardInvite(WorkBoardInviteViewModel model)
        {
            await Authenticate(model.OwnerUserId, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    // var url = string.Empty;
                    var wbDetails = await _workboardBusiness.GetWorkBoardInvites(model.WorkBoardId);
                    var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);

                    if (model.WorkBoardSharingType == WorkBoardSharingTypeEnum.Email)
                    {
                        if (model.WorkBoardContributionType == WorkBoardContributionTypeEnum.Viewer)
                        {
                            var urlnew = "WorkBoard/Invite/" + wbDetails.Where(x => x.WorkBoardContributionType == WorkBoardContributionTypeEnum.Viewer).Select(y => y.WorkBoardUniqueId) + "/" + wbDetails.Where(x => x.WorkBoardContributionType == WorkBoardContributionTypeEnum.Viewer).Select(y => y.ShareKey);
                            model.Link = baseurl + urlnew;
                        }
                        else
                        {
                            var urlnew = "WorkBoard/Invite/" + wbDetails.Where(x => x.WorkBoardContributionType == WorkBoardContributionTypeEnum.Contributer).Select(y => y.WorkBoardUniqueId) + "/" + wbDetails.Where(x => x.WorkBoardContributionType == WorkBoardContributionTypeEnum.Contributer).Select(y => y.ShareKey);
                            model.Link = baseurl + urlnew;
                        }
                        var WorkBoardTemplate = await _tableMetadataBusiness.GetTableDataByColumn("WORK_BOARD", "", "Id", model.WorkBoardId);
                        var emails = model.EmailAddress.Split(",");
                        if (emails.Length > 1)
                        {
                            foreach (var data in emails)
                            {
                                model.EmailAddress = data;
                                await SendWorkBoardInvite(model);
                                // Send Email

                                var notificationTemplateModel = await _notificationTemplateBusiness.GetSingle(x => x.Code == "WORKBOARD_INVITE");
                                if (notificationTemplateModel.IsNotNull())
                                {
                                    notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{LINK}}", model.Link.ToString());
                                    notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{Email}}", model.EmailAddress.ToString());
                                    if (model.OptionalMessage != null)
                                    {
                                        notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{OptionalMessage}}", model.OptionalMessage.ToString());
                                    }
                                    notificationTemplateModel.Body = WorkBoardTemplate["WorkBoardName"].IsNotNull() ? notificationTemplateModel.Body.Replace("{{WorkBoardName}}", WorkBoardTemplate["WorkBoardName"].ToString()) : "";
                                    var viewModel = new NotificationViewModel()
                                    {
                                        To = data,
                                        //ToUserId = model.Id,
                                        //FromUserId = model.CreatedBy,
                                        Subject = notificationTemplateModel.Subject,
                                        Body = notificationTemplateModel.Body,
                                        SendAlways = true,
                                        NotifyByEmail = true,
                                        DynamicObject = model
                                    };
                                    await _notificationBusiness.Create(viewModel);
                                }
                            }
                            return Ok(new { success = true });
                        }
                        else
                        {
                            await SendWorkBoardInvite(model);
                            // Send Email
                            var notificationTemplateModel = await _notificationTemplateBusiness.GetSingle(x => x.Code == "WORKBOARD_INVITE");
                            if (notificationTemplateModel.IsNotNull())
                            {
                                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{LINK}}", model.Link.ToString());
                                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{Email}}", model.EmailAddress.ToString());
                                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{OptionalMessage}}", model.OptionalMessage.IsNotNullAndNotEmpty() ? model.OptionalMessage.ToString() : "");
                                notificationTemplateModel.Body = WorkBoardTemplate["WorkBoardName"].IsNotNull() ? notificationTemplateModel.Body.Replace("{{WorkBoardName}}", WorkBoardTemplate["WorkBoardName"].ToString()) : "";
                                var viewModel = new NotificationViewModel()
                                {
                                    To = model.EmailAddress,
                                    //ToUserId = model.Id,
                                    //FromUserId = model.CreatedBy,
                                    Subject = notificationTemplateModel.Subject,
                                    Body = notificationTemplateModel.Body,
                                    SendAlways = true,
                                    NotifyByEmail = true,
                                    DynamicObject = model
                                };
                                await _notificationBusiness.Create(viewModel);
                            }
                            return Ok(new { success = true });
                        }
                    }
                    else
                    {
                        await SendWorkBoardInvite(model);
                        return Ok(new { success = true });
                    }
                }
                else
                {
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        //ViewBag.Success = true;
                        return Ok(new { success = true });
                    }
                }

            }
            return Ok(new { success = false, error = ModelState });

        }


        private async Task<IActionResult> SendWorkBoardInvite(WorkBoardInviteViewModel model)
        {
            await Authenticate(model.OwnerUserId, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var templateModel = new NoteTemplateViewModel();
            templateModel.ActiveUserId = _userContext.UserId;
            templateModel.DataAction = model.DataAction;
            templateModel.TemplateCode = "WB_SHARE";
            //templateModel.NoteId = model.NoteId;
            var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
            newmodel.NoteSubject = model.NoteSubject;
            newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var result = await _noteBusiness.ManageNote(newmodel);
            if (result.IsSuccess)
            {
                //ViewBag.Success = true;
                return Ok(new { success = true, result = result });
            }
            return Ok(new { success = false, error = ModelState });
        }


        [HttpGet]
        [Route("DuplicateWorkboard")]
        public ActionResult DuplicateWorkboard(string workBoardId)
        {                                                                               
            var model = new WorkboardDuplicateViewModel();
            if (workBoardId.IsNotNullAndNotEmpty())
            {
                model.WorkBoardId = workBoardId;
            }
            return Ok(model);
        }

        [HttpPost]
        [Route("PostDuplicateWorkboard")]
        public async Task<IActionResult> PostDuplicateWorkboard(WorkboardDublicateModel model)
        {
            await Authenticate(model.UserId, model.PortalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            if (model.WorkBoardId.IsNotNullAndNotEmpty())
            {
                var existingmodel = await _workboardBusiness.GetWorkBoardDetails(model.WorkBoardId);
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _context.UserId;
                templateModel.DataAction = DataActionEnum.Create;
                templateModel.TemplateCode = "WORK_BOARD";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                newmodel.DataAction = DataActionEnum.Create;
                //newmodel.TemplateCode = "WORK_BOARD";
                newmodel.ActiveUserId = _context.UserId;

                var workBoardViewModel = new WorkBoardViewModel()
                {
                    WorkBoardName = model.WorkBoardName,//

                    TemplateTypeId = existingmodel.TemplateTypeId,//

                    WorkBoardDescription = existingmodel.WorkBoardDescription,//
                    ColorCode = existingmodel.ColorCode,

                    IconFileId = existingmodel.IconFileId,//
                                                          //boards = existingmodel.IconFileId,
                                                          //WorkBoardSections = existingmodel.WorkBoardSections,
                    WorkBoardDate = existingmodel.WorkBoardDate,//
                    WorkBoardStatus = existingmodel.WorkBoardStatus,//
                                                                    //NoteId = existingmodel.IconFileId,

                    //WorkBoardSectionId = secId
                };
                newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(workBoardViewModel);
                var result = await _noteBusiness.ManageNote(newmodel);
                if (result.IsSuccess)
                {
                    newmodel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel
                    {
                        NoteId = result.Item.NoteId,
                        DataAction = DataActionEnum.Edit,
                        ActiveUserId = _context.UserId,
                        SetUdfValue = true
                    });
                    var rowData1 = newmodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                    rowData1["WorkBoardStatus"] = "0";
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(result.Item, data1, result.Item.UdfNoteTableId);

                    ///////get list of section by workboard 
                    var sectionList = await _workboardBusiness.GetWorkBoardSectionListByWorkbBoardId(model.WorkBoardId);
                    if (sectionList.IsNotNull() && sectionList.Count() > 0)
                    {
                        ///for each for list inside (get section with id )
                        foreach (var section in sectionList)
                        {
                            var templateSectionModel = new NoteTemplateViewModel();
                            templateSectionModel.ActiveUserId = _context.UserId;
                            templateSectionModel.DataAction = DataActionEnum.Create;
                            templateSectionModel.TemplateCode = "WB_SECTION";
                            var sectionmodel = await _noteBusiness.GetNoteDetails(templateSectionModel);
                            sectionmodel.DataAction = DataActionEnum.Create;
                            var workBoardSectionViewModel = new WorkBoardSectionViewModel()
                            {
                                WorkBoardId = result.Item.UdfNoteTableId,
                                SectionName = section.SectionName,//
                                SequenceOrder = section.SequenceOrder,
                                SectionDigit = section.SectionDigit,//
                                HeaderColor = section.HeaderColor,//
                                SectionDescription = section.SectionDescription, // 
                            };
                            sectionmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(workBoardSectionViewModel);
                            var resultsection = await _noteBusiness.ManageNote(sectionmodel);

                            if (resultsection.IsSuccess)
                            {
                                var itemList = await _workboardBusiness.GetItemBySectionId(section.Id);
                                if (itemList.IsNotNull() && itemList.Count() > 0)
                                {
                                    ///for each for list inside (get section with id )
                                    foreach (var item in itemList)
                                    {
                                        if (item.ItemType == WorkBoardItemTypeEnum.Index)
                                        {
                                            await DuplicateChildItems(item, model.IsComments, model.IsTasks, resultsection.Item.UdfNoteTableId, result.Item.UdfNoteTableId, item.ParentId,_context);
                                        }
                                        else if (!item.ParentId.IsNotNullAndNotEmpty())
                                        {
                                            var templateItemModel = new NoteTemplateViewModel();
                                            templateItemModel.ActiveUserId = _context.UserId;
                                            templateItemModel.DataAction = DataActionEnum.Create;
                                            templateItemModel.TemplateCode = "WB_ITEM";
                                            var itemmodel = await _noteBusiness.GetNoteDetails(templateItemModel);
                                            itemmodel.DataAction = DataActionEnum.Create;

                                            itemmodel.ActiveUserId = _context.UserId;

                                            var workBoardItemViewModel = new WorkBoardItemViewModel()
                                            {
                                                WorkBoardId = result.Item.UdfNoteTableId,
                                                ItemType = item.ItemType,
                                                ItemContent = item.ItemContent,
                                                ItemFileId = item.ItemFileId,
                                                ColorCode = item.ColorCode,
                                                IconFileId = item.IconFileId,
                                                WorkBoardSectionId = resultsection.Item.UdfNoteTableId,
                                                WorkBoardItemShape = item.WorkBoardItemShape,
                                                WorkBoardItemSize = item.WorkBoardItemSize,
                                                ItemName = item.ItemName,
                                            };
                                            itemmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(workBoardItemViewModel);
                                            var resultitem = await _noteBusiness.ManageNote(itemmodel);
                                            if (resultitem.IsSuccess)
                                            {
                                                if (model.IsComments)
                                                {
                                                    // GetCommentlist for the items
                                                    var commentList = await _ntsNoteCommentBusiness.GetList(x => x.NtsNoteId == item.NoteId);
                                                    if (commentList.IsNotNull() && commentList.Count() > 0)
                                                    {
                                                        foreach (var data in commentList)
                                                        {
                                                            data.NtsNoteId = resultitem.Item.NoteId;
                                                            data.Id = null;
                                                            data.CommentedByUserId = _context.UserId;
                                                            data.CommentedDate = DateTime.Now;
                                                            await _ntsNoteCommentBusiness.Create(data);
                                                        }
                                                    }
                                                }
                                                if (model.IsCharts)
                                                {
                                                    // Get Tasks list for the items
                                                    var taskList = await _taskBusiness.GetList(x => x.ParentNoteId == item.NoteId);
                                                    if (taskList.IsNotNull() && taskList.Count() > 0)
                                                    {
                                                        foreach (var data in taskList)
                                                        {
                                                            var task = await _taskBusiness.GetTaskDetails(new TaskTemplateViewModel()
                                                            {
                                                                TaskId = data.Id,
                                                                SetUdfValue = true
                                                            });
                                                            var columns = task.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                                                            var newtask = await _taskBusiness.GetTaskDetails(new TaskTemplateViewModel()
                                                            {
                                                                DataAction = DataActionEnum.Create,
                                                                ActiveUserId = _context.UserId,
                                                                TemplateCode = "WB_TASK",
                                                                ParentNoteId = resultitem.Item.UdfNoteTableId
                                                            });
                                                            newtask.ParentNoteId = resultitem.Item.UdfNoteTableId;
                                                            newtask.Json = Newtonsoft.Json.JsonConvert.SerializeObject(columns);
                                                            newtask.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                                                            await _taskBusiness.ManageTask(newtask);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    return Ok(new { success = true, WorkBoardId = result.Item.UdfNoteTableId });
                }
                return Ok(new { success = false });
            }
            return Ok(new { success = false });
        }

        private async Task DuplicateChildItems(WorkBoardItemViewModel model, bool IsComments, bool IsCharts, string sectionId, string workBoardId, string parentId, IUserContext _context)
        {

            var childList = await _workboardBusiness.GetWorkBoardItemByParentId(model.Id);
            var templateItemModel = new NoteTemplateViewModel();
            templateItemModel.ActiveUserId = _context.UserId;
            templateItemModel.DataAction = DataActionEnum.Create;
            templateItemModel.TemplateCode = "WB_ITEM";
            var itemmodel = await _noteBusiness.GetNoteDetails(templateItemModel);
            itemmodel.DataAction = DataActionEnum.Create;

            itemmodel.ActiveUserId = _context.UserId;

            var workBoardItemViewModel = new WorkBoardItemViewModel()
            {
                WorkBoardId = workBoardId,
                ItemType = model.ItemType,
                ItemContent = model.ItemContent,
                ItemFileId = model.ItemFileId,
                ParentId = parentId,
                ColorCode = model.ColorCode,
                IconFileId = model.IconFileId,
                WorkBoardSectionId = sectionId,
                WorkBoardItemShape = model.WorkBoardItemShape,
                WorkBoardItemSize = model.WorkBoardItemSize,
                ItemName = model.ItemName,
            };
            itemmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(workBoardItemViewModel);
            var resultitem = await _noteBusiness.ManageNote(itemmodel);
            if (resultitem.IsSuccess)
            {
                if (IsComments)
                {
                    // GetCommentlist for the items
                    var commentList = await _ntsNoteCommentBusiness.GetList(x => x.NtsNoteId == model.NoteId);
                    if (commentList.IsNotNull() && commentList.Count() > 0)
                    {
                        foreach (var data in commentList)
                        {
                            data.NtsNoteId = resultitem.Item.NoteId;
                            data.Id = null;
                            data.CommentedByUserId = _context.UserId;
                            data.CommentedDate = DateTime.Now;
                            await _ntsNoteCommentBusiness.Create(data);
                        }
                    }
                }
                if (IsCharts)
                {
                    // Get Tasks list for the items
                    var taskList = await _taskBusiness.GetList(x => x.ParentNoteId == model.NoteId);
                    if (taskList.IsNotNull() && taskList.Count() > 0)
                    {
                        foreach (var data in taskList)
                        {
                            var task = await _taskBusiness.GetTaskDetails(new TaskTemplateViewModel()
                            {
                                TaskId = data.Id,
                                SetUdfValue = true
                            });
                            var columns = task.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                            var newtask = await _taskBusiness.GetTaskDetails(new TaskTemplateViewModel()
                            {
                                DataAction = DataActionEnum.Create,
                                ActiveUserId = _context.UserId,
                                TemplateCode = "WB_TASK",
                                ParentNoteId = resultitem.Item.UdfNoteTableId
                            });
                            newtask.ParentNoteId = resultitem.Item.UdfNoteTableId;
                            newtask.Json = Newtonsoft.Json.JsonConvert.SerializeObject(columns);
                            newtask.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                            await _taskBusiness.ManageTask(newtask);
                        }
                    }
                }
                foreach (var data in childList)
                {
                    await DuplicateChildItems(data, IsComments, IsCharts, sectionId, workBoardId, resultitem.Item.UdfNoteTableId, _context);
                }
            }
        }


        [HttpGet]
        [Route("OpenCloseWorkboard")]
        public async Task<ActionResult> OpenCloseWorkboard(string id, WorkBoardstatusEnum status)
        {
            var model = await _workboardBusiness.UpdateWorkBoardStatus(id, status);
            return Ok(model);
        }

        [HttpGet]
        [Route("AddWorkBoardContent")]
        public async Task<IActionResult> AddWorkBoardContent(string id, string workBoardId, string workBoardSectionId, string parentId)
        {
            var model = new WorkBoardItemViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _workboardBusiness.GetWorkBoardItemByNtsNoteId(id);
                model.DataAction = DataActionEnum.Edit;
                return Ok(model);
            }

            model.DataAction = DataActionEnum.Create;
            if (workBoardId.IsNotNullAndNotEmpty())
            {
                model.WorkBoardId = workBoardId;
            }
            if (workBoardSectionId.IsNotNullAndNotEmpty())
            {
                model.WorkBoardSectionId = workBoardSectionId;
            }
            model.ParentId = parentId;
            return Ok(model);
        }

        [HttpPost]
        [Route("ManageWorkBoardContent")]
        public async Task<IActionResult> ManageWorkBoardContent(WorkBoardItemViewModel model)

        {
            await Authenticate(model.RequestedByUserId, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            if (ModelState.IsValid)
            {
                var templateModel = new NoteTemplateViewModel();
                if (model.DataAction == DataActionEnum.Create)
                {
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "WB_ITEM";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.DataAction = DataActionEnum.Create;
                    newmodel.TemplateCode = "WB_ITEM";
                    newmodel.ActiveUserId = _userContext.UserId;
                    if (model.ItemType == WorkBoardItemTypeEnum.Index)
                    {
                        //model.ItemContent = model.ItemContentIndex;
                        model.ItemName = model.ItemContentIndex;
                    }
                    if (model.ItemType == WorkBoardItemTypeEnum.File)
                    {
                        model.ItemFileId = model.ItemFileFileId;
                    }
                    if (model.ItemType != WorkBoardItemTypeEnum.Index)
                    {
                        model.ItemName = model.ItemContent;
                    }
                    var workBoardItemViewModel = new WorkBoardItemViewModel()
                    {
                        ItemType = model.ItemType,
                        ItemContent = model.ItemContent,
                        ItemFileId = model.ItemFileId,
                        WorkBoardId = model.WorkBoardId,
                        ParentId = model.ParentId,
                        ColorCode = model.ColorCode,
                        IconFileId = model.IconFileId,
                        WorkBoardSectionId = model.WorkBoardSectionId,
                        ItemName = model.ItemName
                    };
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(workBoardItemViewModel);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        workBoardItemViewModel.WorkBoardItemId = result.Item.UdfNoteTableId;
                        workBoardItemViewModel.NtsNoteId = result.Item.NoteId;
                        //workBoardItemViewModel.NoteId = newmodel.NoteId;
                        return Ok(new { success = true, result = workBoardItemViewModel });
                    }
                }
                else
                {
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = DataActionEnum.Edit;
                    templateModel.NoteId = model.NtsNoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    if (model.ItemType == WorkBoardItemTypeEnum.Index)
                    {
                        model.ItemContent = model.ItemContentIndex;
                    }
                    if (model.ItemType == WorkBoardItemTypeEnum.File)
                    {
                        model.ItemFileId = model.ItemFileFileId;
                    }
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    var wbItem = await _workboardBusiness.GetWorkboardItemById(result.Item.UdfNoteTableId);
                    if (result.IsSuccess)
                    {
                        return Ok(new { success = true, result = wbItem });
                    }
                }

            }
            return Ok(new { success = false, error = ModelState });

        }


        [HttpPost]
        [Route("PostNoteComment")]
        public async Task<IActionResult> PostNoteComment(NtsNoteCommentViewModel model)
        {
            model.CommentedByUserId = model.CommentedByUserId;
            model.CommentedDate = DateTime.Now;
            var result = await _ntsNoteCommentBusiness.Create(model);
            if (result.IsSuccess)
            {
                var data = await _ntsNoteCommentBusiness.GetSearchResult(model.NtsNoteId);
                return Ok(new { success = true, CommentsCount = data.Count() });
            }
            return Ok(new { success = false, error = result.HtmlError });
        }

        [HttpGet]
        [Route("ReadNoteCommentDataList")]
        public async Task<ActionResult> ReadNoteCommentDataList(string noteId)
        {
            //var model = await _ntsNoteCommentBusiness.GetCommentTree(noteId, id);
            var model = await _ntsNoteCommentBusiness.GetAllCommentTree(noteId);
            return Ok(model);
        }

        [HttpGet]
        [Route("CopyMoveItems")]
        public async Task<IActionResult> CopyMoveItems(string workboardId, string itemNoteId)
        {
            var model = await _workboardBusiness.GetWorkboardItemByNoteId(itemNoteId);

            return Ok(model);
        }
        [HttpPost]
        [Route("DuplicateItems")]

        public async Task<IActionResult> DuplicateItems(string workboardId, string itemId, string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var model = await _workboardBusiness.GetWorkboardItemByNoteId(itemId);
            var templateModel = new NoteTemplateViewModel();
            templateModel.ActiveUserId = _userContext.UserId;
            templateModel.DataAction = DataActionEnum.Create;
            templateModel.TemplateCode = "WB_ITEM";
            var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
            newmodel.DataAction = DataActionEnum.Create;
            newmodel.TemplateCode = "WB_ITEM";
            newmodel.ActiveUserId = _userContext.UserId;

            var workBoardItemViewModel = new WorkBoardItemViewModel()
            {
                ItemType = model.ItemType,
                ItemContent = model.ItemContent,
                ItemFileId = model.ItemFileId,
                WorkBoardId = workboardId,
                ParentId = model.ParentId,
                ColorCode = model.ColorCode,
                IconFileId = model.IconFileId,
                WorkBoardSectionId = model.WorkBoardSectionId
            };
            newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(workBoardItemViewModel);
            var result = await _noteBusiness.ManageNote(newmodel);
            // var wbItem = await _workboardBusiness.GetWorkboardItemById(result.Item.UdfNoteTableId);
            if (result.IsSuccess)
            {
                workBoardItemViewModel.WorkBoardItemId = result.Item.UdfNoteTableId;
                workBoardItemViewModel.NtsNoteId = result.Item.NoteId;
                return Ok(new { success = true, note = workBoardItemViewModel });
            }
            return Ok(new { success = false });


        }

        [HttpPost]
        [Route("ManageItemSharing")]
        public async Task<IActionResult> ManageItemSharing(string workboardId, string secId, string workboardItemId, string postAction, string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            if (postAction == "Copy")
            {
                // create a new item
                var model = await _workboardBusiness.GetWorkboardItemById(workboardItemId);
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = DataActionEnum.Create;
                templateModel.TemplateCode = "WB_ITEM";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                newmodel.DataAction = DataActionEnum.Create;
                newmodel.TemplateCode = "WB_ITEM";
                newmodel.ActiveUserId = _userContext.UserId;

                var workBoardItemViewModel = new WorkBoardItemViewModel()
                {
                    ItemType = model.ItemType,
                    ItemContent = model.ItemContent,
                    ItemFileId = model.ItemFileId,
                    WorkBoardId = workboardId,
                    ParentId = model.ParentId,
                    ColorCode = model.ColorCode,
                    IconFileId = model.IconFileId,
                    WorkBoardSectionId = secId
                };
                newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(workBoardItemViewModel);
                var result = await _noteBusiness.ManageNote(newmodel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
            }
            else
            {
                //update wbid and section id for item
                var result = await _workboardBusiness.UpdateWorkboardItem(workboardId, secId, workboardItemId);
            }
            return Ok(new { success = false, error = ModelState });
        }

        [HttpPost]
        [Route("UpdateWorkBoardSectionAndItemSequence")]
        public async Task<IActionResult> UpdateWorkBoardSectionAndItemSequence(WorkBoardViewModel data)
        {
            if (data != null && data.boards.IsNotNullAndNotEmpty() /*&& data.WorkBoardSections!=null && data.WorkBoardSections.Count()>0*/)
            {
                data.WorkBoardSections = JsonConvert.DeserializeObject<List<WorkBoardSectionViewModel>>(data.boards);

                // Get Existing Section item List
                var existingSectionlist = await _workboardBusiness.GetWorkboardSectionList(data.WorkBoardSections.Select(x => x.WorkBoardId).FirstOrDefault());
                var ExistingSectionIds = existingSectionlist.Select(x => x.Id);
                var newSecIds = data.WorkBoardSections.Select(x => x.id);
                var ToDeleteSections = ExistingSectionIds.Except(newSecIds).ToList();
                // update Workboard Sections Sequence Order               
                var i = 1;
                foreach (var section in data.WorkBoardSections)
                {
                    section.SequenceOrder = i;
                    await _workboardBusiness.UpdateWorkBoardSectionSequenceOrder(section);
                    //if (section.item != null && section.item.Count() > 0)
                    //{
                    // update Workboard Items Sequence Order within workboard Sections
                    var j = 1;
                    // Get Existing Section item List
                    var existingItemlist = await _workboardBusiness.GetWorkBoardItemBySectionId(section.id);
                    var ExistingItemIds = existingItemlist.Select(x => x.Id);
                    var newIds = GetBoardItemsIds(section.item); //section.item.Select(x => x.id);
                    var ToDelete = ExistingItemIds.Except(newIds).ToList();
                    foreach (var item in section.item)
                    {

                        var existingItem = await _workboardBusiness.GetWorkBoardItemDetails(item.id);
                        item.SequenceOrder = j;
                        //item.ParentId = null;
                        await _workboardBusiness.UpdateWorkBoardItemSequenceOrder(item);

                        if (existingItem.ItemType == WorkBoardItemTypeEnum.Index)
                        {
                            var k = 1;
                            foreach (var subItem in item.item)
                            {
                                subItem.SequenceOrder = k;
                                subItem.WorkBoardSectionId = existingItem.WorkBoardSectionId;
                                await _workboardBusiness.UpdateWorkBoardItemDetails(subItem);
                                k++;
                            }
                        }

                        j++;
                    }
                    if (ToDelete.IsNotNull() && ToDelete.Count() > 0)
                    {
                        foreach (var itemId in ToDelete)
                        {
                            var existing = await _workboardBusiness.GetWorkBoardItemDetails(itemId);
                            await _workboardBusiness.DeleteItem(itemId);
                            await _noteBusiness.Delete(existing.NoteId);
                        }
                    }
                    // }                  
                    i++;
                }
                if (ToDeleteSections.IsNotNull() && ToDeleteSections.Count() > 0)
                {
                    foreach (var sectionId in ToDeleteSections)
                    {
                        var existing = await _workboardBusiness.GetWorkBoardSectionDetails(sectionId);
                        await _workboardBusiness.DeleteSection(sectionId);
                        await _noteBusiness.Delete(existing.NoteId);
                    }
                }
                return Ok(new { success = true });
            }

            //await _workboardBusiness.UpdateWorkBoardJson(data);
            return Ok(new { success = false });

        }

        protected List<string> GetBoardItemsIds(List<WorkBoardItemViewModel> items)
        {
            List<WorkBoardItemViewModel> list = new List<WorkBoardItemViewModel>();
            foreach (var item in items)
            {
                if (item.item.Count() > 0)
                {
                    list.AddRange(item.item);
                }
                list.Add(item);
            }
            return list.Select(x => x.id).ToList();
        }


    }
}
