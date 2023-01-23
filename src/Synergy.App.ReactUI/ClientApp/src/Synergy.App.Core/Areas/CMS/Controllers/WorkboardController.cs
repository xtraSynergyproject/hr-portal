using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Synergy.App.Business;
using Microsoft.AspNetCore.Mvc;
using Synergy.App.ViewModel;
using Synergy.App.Common;
////using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
////using Kendo.Mvc.UI;
using Synergy.App.DataModel;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net.Http;
//using Syncfusion.EJ2.Base;
using System.Text;
using Npgsql;
using FastMember;
using Microsoft.VisualBasic.FileIO;
using System.IO;
//using Syncfusion.EJ2.Maps;
using System.Data;
using Nest;
using ExcelDataReader;
using AutoMapper;
using Synergy.App.WebUtility;
//using ExcelDataReader;


namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class WorkboardController : ApplicationController
    {

        private readonly IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IWebHelper _webApi;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly INotificationBusiness _notificationBusiness;
        private readonly INotificationTemplateBusiness _notificationTemplateBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IWorkboardBusiness _workboardBusiness;
        private IMapper _autoMapper;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private INtsNoteCommentBusiness _ntsNoteCommentBusiness;

        public WorkboardController(IUserContext userContext, INoteBusiness noteBusiness, ITaskBusiness taskBusiness, IWebHelper webApi,
            ITableMetadataBusiness tableMetadataBusiness, ICmsBusiness cmsBusiness, INotificationBusiness notificationBusiness, ILOVBusiness lovBusiness,
            ITemplateBusiness templateBusiness, Microsoft.Extensions.Configuration.IConfiguration configuration,
            IWorkboardBusiness workboardBusiness, IMapper autoMapper, INotificationTemplateBusiness notificationTemplateBusiness, INtsNoteCommentBusiness ntsNoteCommentBusiness)
        {

            _userContext = userContext;
            _noteBusiness = noteBusiness;
            _taskBusiness = taskBusiness;
            _webApi = webApi;
            _tableMetadataBusiness = tableMetadataBusiness;
            _cmsBusiness = cmsBusiness;
            _notificationBusiness = notificationBusiness;
            _lovBusiness = lovBusiness;
            _templateBusiness = templateBusiness;
            _configuration = configuration;
            _workboardBusiness = workboardBusiness;
            _autoMapper = autoMapper;
            _notificationTemplateBusiness = notificationTemplateBusiness;
            _ntsNoteCommentBusiness = ntsNoteCommentBusiness;
        }

        [HttpPost]
        public async Task<IActionResult> JoinWorkBoard(string ShareId, string ShareKey, bool isPopup)
        {
            var model = await _workboardBusiness.GetWorkBoardDetailsByIdKey(ShareId, ShareKey);
            if (model != null)
            {

                return Json(new { success = true, workboardId = model.WorkboardId });

            }
            else
            {
                return Json(new { success = false, error = "Please enter valid ID and KEY" });
            }

        }
        //[HttpPost]
        //public async Task<IActionResult> UpdateWorkBoardJson(WorkBoardViewModel data)
        //{
        //    data.JsonContent = data.JsonContent.Replace("'", "\"");

        //    await _workboardBusiness.UpdateWorkBoardJson(data);
        //    return Json(new { success = true });

        //}
        [HttpPost]
        public async Task<IActionResult> UpdateWorkBoardSectionAndItemSequence(WorkBoardViewModel data)
        {
            if (data != null && data.boards.IsNotNullAndNotEmpty() /*&& data.WorkBoardSections!=null && data.WorkBoardSections.Count()>0*/)
            {
                data.WorkBoardSections = JsonConvert.DeserializeObject<List<WorkBoardSectionViewModel>>(data.boards);

                // Get Existing Section item List
                var existingSectionlist = await _workboardBusiness.GetWorkboardSectionList(data.WorkBoardSections.Select(x=>x.WorkBoardId).FirstOrDefault());
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
                           
                            var existingItem =await _workboardBusiness.GetWorkBoardItemDetails(item.id);
                            item.SequenceOrder = j;
                            //item.ParentId = null;
                            await _workboardBusiness.UpdateWorkBoardItemSequenceOrder(item);
                            
                            if (existingItem.ItemType==WorkBoardItemTypeEnum.Index) 
                            {
                                var k = 1;
                                foreach (var subItem in item.item)
                                {
                                    subItem.SequenceOrder =k;
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
            }

            //await _workboardBusiness.UpdateWorkBoardJson(data);
            return Json(new { success = true });

        }

        public List<string> GetBoardItemsIds(List<WorkBoardItemViewModel> items)
        {
            List<WorkBoardItemViewModel> list = new List<WorkBoardItemViewModel>();
            foreach (var item in items)
            {
                if (item.item.Count()>0) 
                {
                    list.AddRange(item.item);
                }
                list.Add(item);
            }
            return list.Select(x => x.id).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateWorkBoardSectionAndItemSequenceForIndex(WorkBoardViewModel data)
        {
            if (data != null && data.boards.IsNotNullAndNotEmpty() /*&& data.WorkBoardSections!=null && data.WorkBoardSections.Count()>0*/)
            {
                data.WorkBoardSections = JsonConvert.DeserializeObject<List<WorkBoardSectionViewModel>>(data.boards);
                var i = 1;
                foreach (var section in data.WorkBoardSections)
                {
                    section.SequenceOrder = i;
                    var j = 1;
                    var existingItemlist = await _workboardBusiness.GetWorkBoardItemBySectionId(section.id);
                    var ExistingItemIds = existingItemlist.Select(x => x.Id);
                    var newIds = section.item.Select(x => x.id);
                    var ToDelete = ExistingItemIds.Except(newIds).ToList();
                    foreach (var item in section.item)
                    {

                        var existingItem = await _workboardBusiness.GetWorkBoardItemDetails(item.id);
                        item.SequenceOrder = j;
                        await _workboardBusiness.UpdateWorkBoardItemSequenceOrder(item);

                        if (existingItem.IsNotNull() && existingItem.ItemType == WorkBoardItemTypeEnum.Index)
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
                    i++;
                }
            }
            var model = await _workboardBusiness.GetWorkBoardDetails(data.WorkBoardSections[0].WorkBoardId);
            model.JsonContent = await _workboardBusiness.GetJsonContent(model.TemplateTypeId, model.Id);
            model.DataAction = DataActionEnum.Edit;
            return Json(new { success = true, data = model });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateWorkBoardItemSectionId(WorkBoardItemViewModel data)
        {
            await _workboardBusiness.UpdateWorkBoardItemSectionId(data);
            return Json(new { success = true });
        }
        public async Task<IActionResult> ManageWorkBoard(string id)
        {
            ViewBag.OwnerUserId = _userContext.UserId;
            var model = await _workboardBusiness.GetWorkBoardDetails(id);
            if (_userContext.IsSystemAdmin)
            {
                model.OwnerType = WorkBoardOwnerTypeEnum.Admin;
            }
            else if (_userContext.UserId == model.RequestedByUserId || _userContext.UserId == model.OwnerUserId)
            {
                model.OwnerType = WorkBoardOwnerTypeEnum.OwnerOrRequester;
            }
            else if (model.WorkBoardTeamIds.IsNotNull() && model.WorkBoardTeamIds.Contains(_userContext.UserId))
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
                return View(model);
            }
            model.JsonContent = await _workboardBusiness.GetJsonContent(model.TemplateTypeId, model.Id,ownerType:model.OwnerType);
            model.DataAction = DataActionEnum.Edit;
            return View(model);
        }
        [Route("Workboard/Invite/{id}/{key}")]
        public async Task<IActionResult> Invite(string id, string key)
        {
            var model = await _workboardBusiness.GetWorkBoardDetailsByIdKey(id, key);
            if (model != null)
            {
                return View("Invite", model);

            }
            else
            {
                return View("Invite", new WorkBoardViewModel { Message = "Invalid Workboard URL" });
            }

        }

        [HttpPost]
        public async Task<IActionResult> ManageWorkBoard(WorkBoardViewModel model)
        {
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
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "WORK_BOARD";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    model.WorkBoardStatus = WorkBoardstatusEnum.Open;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        newmodel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel
                        {
                            NoteId = result.Item.NoteId,
                            DataAction = DataActionEnum.Edit,
                            ActiveUserId = _userContext.UserId,
                            SetUdfValue = true
                        });
                        var rowData1 = newmodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        rowData1["JsonContent"] = await _workboardBusiness.GetJsonContent(model.TemplateTypeId, result.Item.UdfNoteTableId, model.WorkBoardDate);
                        rowData1["WorkBoardStatus"] = "0";
                        rowData1["WorkboardType"] = model.WorkBoardType;
                        if (model.WorkBoardTeamIds.IsNotNull())
                        {
                            rowData1["WorkBoardTeamIds"] = string.Join(",", model.WorkBoardTeamIds);
                        }
                        
                        var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                        var update = await _noteBusiness.EditNoteUdfTable(result.Item, data1, result.Item.UdfNoteTableId);
                        // Create Share Invite
                        var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
                        string WorkBoardUniqueId = RandomString(6);
                        string WorkBoardKey = RandomString(6);
                        WorkBoardInviteViewModel invite1 = new WorkBoardInviteViewModel()
                        {
                            WorkBoardUniqueId = WorkBoardUniqueId,
                            ShareKey = WorkBoardKey,
                            //Link= baseurl+ "WorkBoard/Invite/" + WorkBoardUniqueId + "/" + WorkBoardKey,
                            WorkBoardContributionType=WorkBoardContributionTypeEnum.Contributer,
                            DataAction=DataActionEnum.Create,
                            WorkBoardSharingType = WorkBoardSharingTypeEnum.Link,
                            WorkBoardId=result.Item.UdfNoteTableId
                        };
                        await ManageWorkboardInvite(invite1);
                        string WorkBoardUniqueId2 = RandomString(6);
                        string WorkBoardKey2 = RandomString(6);
                        WorkBoardInviteViewModel invite2 = new WorkBoardInviteViewModel()
                        {
                            WorkBoardUniqueId = WorkBoardUniqueId2,
                            ShareKey = WorkBoardKey2,
                            Link = baseurl + "WorkBoard/Invite/" + WorkBoardUniqueId2 + "/" + WorkBoardKey2,
                            WorkBoardContributionType = WorkBoardContributionTypeEnum.Viewer,
                            DataAction = DataActionEnum.Create,
                            WorkBoardSharingType = WorkBoardSharingTypeEnum.Link,
                            WorkBoardId = result.Item.UdfNoteTableId
                        };
                         await  ManageWorkboardInvite(invite2);
                        return Json(new { success = true, result = result.Item });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    templateModel.SetUdfValue = true;

                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    var workboardModel = newmodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                    workboardModel["WorkBoardName"] = model.WorkBoardName;
                    workboardModel["WorkBoardStatus"] = "0";
                    workboardModel["WorkboardType"] = model.WorkBoardType;
                    workboardModel["WorkBoardTeamIds"] = string.Join(",", model.WorkBoardTeamIds);



                    //    var workboardModel = new WorkBoardViewModel()
                    //    {
                    //        WorkBoardName = model.WorkBoardName,
                    //        //model.WorkBoardStatus = WorkBoardstatusEnum.Open

                    //};

                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(workboardModel);
                    newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, result = result.Item });
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
                        var rowData1 = result.Item.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        if (Convert.ToString(rowData1["JsonContent"]).IsNullOrEmpty())
                        {
                            var jsonContent = await _workboardBusiness.GetJsonContent(model.TemplateTypeId, result.Item.UdfNoteTableId);
                            rowData1["JsonContent"] = jsonContent;
                            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                            var update = await _noteBusiness.EditNoteUdfTable(result.Item, data1, result.Item.UdfNoteTableId);
                        }
                        //ViewBag.Success = true;
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

        }

        public async Task<IActionResult> ManageWorkBoardIndex(string workboardItemId)
        {
            var model = new WorkBoardViewModel();
            var item = await _workboardBusiness.GetWorkboardItemById(workboardItemId);
            model.Id = item.WorkBoardId;
            model.IndexId = workboardItemId;
            model.JsonContent = await _workboardBusiness.GetWorkBoardSectionForIndex(item);
            model.DataAction = DataActionEnum.Edit;
            model.TemplateTypeCode = "INDEX";
            ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            ViewBag.LayoutMode = LayoutModeEnum.Popup.ToString();
            return View("ManageWorkBoard", model);
        }


        public async Task<IActionResult> KanbanBoardTemplate()
        {
            return View();
        }
        public async Task<IActionResult> KanbanBoard()
        {
            return View();
        }
        public async Task<IActionResult> AddKanbanBoard(string name, string id)
        {
            ViewBag.Name = name;
            ViewBag.Id = id;
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> ReadKanbanBoard()
        {
            var data = await _noteBusiness.GetKanbanBoard();
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> ManageKanbanBoard(KanbanBoardViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "KANBAN_BOARD";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        //ViewBag.Success = true;
                        return Json(new { success = true, result = result });
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
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

        }
        public async Task<IActionResult> AddWorkBoardContent(string id, string workBoardId, string workBoardSectionId, string parentId)
        {
            var model = new WorkBoardItemViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _workboardBusiness.GetWorkBoardItemByNtsNoteId(id);
                model.DataAction = DataActionEnum.Edit;
                return View(model);
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
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageWorkBoardContent(WorkBoardItemViewModel model)
        {
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
                        return Json(new { success = true, result = workBoardItemViewModel });
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
                        return Json(new { success = true, result = wbItem });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }

        public async Task<IActionResult> CreateWorkboard(string workBoardId)
        {
            WorkBoardViewModel model = new WorkBoardViewModel();
            if (workBoardId.IsNotNullAndNotEmpty())
            {
               // model.WorkboardId = workBoardId;
                var existingmodel = await _workboardBusiness.GetWorkBoardDetail(workBoardId);
                if (existingmodel.WorkBoardTeams.IsNotNullAndNotEmpty())
                {
                    existingmodel.WorkBoardTeamIds = existingmodel.WorkBoardTeams.Split(",");
                }
                existingmodel.DataAction = DataActionEnum.Edit;
                return View(existingmodel);
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
                model.WorkboardUniqueId = RandomString(6);
                model.ShareKey = RandomString(6);
                return View(model);
            }
        }

        public async Task<IActionResult> WorkboardInvite(string workBoardId, string ntsId)
        {
            WorkBoardInviteViewModel model = new WorkBoardInviteViewModel();
            model.Invites= await _workboardBusiness.GetWorkBoardInvites(workBoardId);
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
            return View(model);
        }
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [HttpPost]
        public async Task<IActionResult> ManageWorkboardInvite(WorkBoardInviteViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    // var url = string.Empty;
                    var wbDetails=await _workboardBusiness.GetWorkBoardInvites(model.WorkBoardId);
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
                            return Json(new { success = true });
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
                            return Json(new { success = true });
                        }
                    }
                    else
                    {
                        await SendWorkBoardInvite(model);
                        return Json(new { success = true });
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
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

        }
        private async Task<IActionResult> SendWorkBoardInvite(WorkBoardInviteViewModel model)
        {
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
                return Json(new { success = true, result = result });
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public ActionResult WorkboardDashboard()
        {
            return View();
        }

        public ActionResult Dummy()
        {
            return View();
        }

        public async Task<ActionResult> ChooseTemplate()
        {
            var templateCategories = await _workboardBusiness.GetTemplateCategoryList();
            ViewBag.TemplateCategories = templateCategories;
            var model = await _workboardBusiness.GetTemplateList();
            return View(model);
        }

        public async Task<PartialViewResult> TemplateView(string category, string[] searchWord)
        {
            if (searchWord.Length != 0 && category == null)
            {
                string[] values = searchWord[0].Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = values[i].Trim();
                }
                var testModel = await _workboardBusiness.GetSearchResults(values);
                //foreach (var i in model)
                //{
                //    if (i.TemplateTypeName.Contains(searchWord) || i.TemplateDescription.Contains(searchWord))
                //    {
                //        model1.Add(i);
                //    }
                //}
                return PartialView(testModel);
            }

            else if (searchWord.Length == 0 && category == null)
            {
                var model = await _workboardBusiness.GetTemplateList();
                return PartialView(model);
            }
            else
            {
                var model = await _workboardBusiness.GetTemplateList();
                var result = model.Where(x => x.TemplateDisplayName == category).ToList();
                return PartialView(result);
            }

        }

        public ActionResult JoinWorkboard()
        {
            return View();
        }

        public ActionResult DuplicateWorkboard(string workBoardId)
        {
            var model = new WorkboardDuplicateViewModel();
            if (workBoardId.IsNotNullAndNotEmpty())
            {
                model.WorkBoardId = workBoardId;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DuplicateWorkboard(WorkboardDuplicateViewModel model)
        {
            if (model.WorkBoardId.IsNotNullAndNotEmpty())
            {
                var existingmodel = await _workboardBusiness.GetWorkBoardDetails(model.WorkBoardId);
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = DataActionEnum.Create;
                templateModel.TemplateCode = "WORK_BOARD";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                newmodel.DataAction = DataActionEnum.Create;
                //newmodel.TemplateCode = "WORK_BOARD";
                newmodel.ActiveUserId = _userContext.UserId;

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
                        ActiveUserId = _userContext.UserId,
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
                            templateSectionModel.ActiveUserId = _userContext.UserId;
                            templateSectionModel.DataAction = DataActionEnum.Create;
                            templateSectionModel.TemplateCode = "WB_SECTION";
                            var sectionmodel = await _noteBusiness.GetNoteDetails(templateSectionModel);
                            sectionmodel.DataAction = DataActionEnum.Create;
                            var workBoardSectionViewModel = new WorkBoardSectionViewModel()
                            {                                
                                WorkBoardId = result.Item.UdfNoteTableId,
                                SectionName = section.SectionName,//
                                 SequenceOrder=   section.SequenceOrder,                              
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
                                            await DuplicateChildItems(item,model.IsComments,model.IsTasks, resultsection.Item.UdfNoteTableId,result.Item.UdfNoteTableId,item.ParentId);
                                        }
                                        else if(!item.ParentId.IsNotNullAndNotEmpty())
                                        {
                                            var templateItemModel = new NoteTemplateViewModel();
                                            templateItemModel.ActiveUserId = _userContext.UserId;
                                            templateItemModel.DataAction = DataActionEnum.Create;
                                            templateItemModel.TemplateCode = "WB_ITEM";
                                            var itemmodel = await _noteBusiness.GetNoteDetails(templateItemModel);
                                            itemmodel.DataAction = DataActionEnum.Create;

                                            itemmodel.ActiveUserId = _userContext.UserId;

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
                                                            data.CommentedByUserId = _userContext.UserId;
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
                                                                ActiveUserId = _userContext.UserId,
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
                    return Json(new { success = true, WorkBoardId= result.Item.UdfNoteTableId });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }
        public async Task DuplicateChildItems(WorkBoardItemViewModel model,bool IsComments,bool IsCharts,string sectionId,string workBoardId,string parentId) 
        {

            var childList =await _workboardBusiness.GetWorkBoardItemByParentId(model.Id);
            var templateItemModel = new NoteTemplateViewModel();
            templateItemModel.ActiveUserId = _userContext.UserId;
            templateItemModel.DataAction = DataActionEnum.Create;
            templateItemModel.TemplateCode = "WB_ITEM";
            var itemmodel = await _noteBusiness.GetNoteDetails(templateItemModel);
            itemmodel.DataAction = DataActionEnum.Create;

            itemmodel.ActiveUserId = _userContext.UserId;

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
                            data.CommentedByUserId = _userContext.UserId;
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
                                ActiveUserId = _userContext.UserId,
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
                    await DuplicateChildItems(data, IsComments, IsCharts,sectionId, workBoardId, resultitem.Item.UdfNoteTableId);
                }
            }
        }
        public ActionResult CreateDrawingBoard(string workBoardId, string id)
        {
            if (workBoardId.IsNotNullAndNotEmpty())
            {
                ViewBag.WorkBoardId = workBoardId;
            }
            if (id.IsNotNullAndNotEmpty())
            {
                ViewBag.FileId = id;
            }
            return View();
        }

        public async Task<ActionResult> GetWorkboardDashboardList(WorkBoardstatusEnum status)
        {
            var model = await _workboardBusiness.GetWorkboardList(status);
            return Json(model);
        }

        public async Task<ActionResult> GetSharedWorkboardLists()
        {
            string userId = _userContext.UserId;
            var model = await _workboardBusiness.GetSharedWorkboardList(WorkBoardstatusEnum.Open, userId);
            return Json(model);
        }


        public async Task<ActionResult> GetWorkboardTaskList(string status)
        {
            var model = await _taskBusiness.GetWorkboardTaskList(portalId: _userContext.PortalId, templateCodes: "WB_TASK");
            if (status.IsNotNullAndNotEmpty())
            {
                if(status == "TASK_STATUS_INPROGRESS")
                {
                    model = model.Where(x => x.TaskStatusCode == status || x.TaskStatusCode == "TASK_STATUS_OVERDUE").ToList();
                }

                if(status == "TASK_STATUS_COMPLETE")
                {
                    model = model.Where(x => x.TaskStatusCode == status || x.TaskStatusCode == "TASK_STATUS_REJECT"
                    || x.TaskStatusCode == "TASK_STATUS_CANCEL").ToList();
                }
            }
            return Json(model);
        }

        [HttpPost]
        public async Task<ActionResult> OpenCloseWorkboard(string id, WorkBoardstatusEnum status)
        {
            var model = await _workboardBusiness.UpdateWorkBoardStatus(id, status);
            return Json(model);
        }
        public async Task<ActionResult> WorkBoardSection(string sectionId, string workBoardId)
        {
            var model = new WorkBoardSectionViewModel();
            if (sectionId.IsNotNullAndNotEmpty())
            {
                model = await _workboardBusiness.GetWorkBoardSectionDetails(sectionId);
                if (model != null)
                {
                    model.DataAction = DataActionEnum.Edit;
                    return View(model);
                }
            }
            if (workBoardId.IsNotNullAndNotEmpty())
            {
                model.WorkBoardId = workBoardId;
            }
            model.DataAction = DataActionEnum.Create;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageWorkBoardSection(WorkBoardSectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var templateModel = new NoteTemplateViewModel();
                if (model.DataAction == DataActionEnum.Create)
                {
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = DataActionEnum.Create;
                    templateModel.TemplateCode = "WB_SECTION";

                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.DataAction = DataActionEnum.Create;
                    newmodel.TemplateCode = "WB_SECTION";
                    newmodel.ActiveUserId = _userContext.UserId;

                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        model.WorkBoardSectionId = result.Item.UdfNoteTableId;
                        model.NtsNoteId = result.Item.NoteId;
                        //model.NoteId = newmodel.NoteId;
                        return Json(new { success = true, data = model });
                    }
                }
                else
                {
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = DataActionEnum.Edit;
                    templateModel.NoteId = model.NtsNoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);

                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        var wbSection = await _workboardBusiness.GetWorkBoardSectionDetails(result.Item.UdfNoteTableId);
                        return Json(new { success = true, data = wbSection });
                    }
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });
        }

        public async Task<IActionResult> CopyMoveItems(string workboardId, string itemNoteId)
        {
            var model = await _workboardBusiness.GetWorkboardItemByNoteId(itemNoteId);
            ViewBag.wbId = workboardId;
            ViewBag.wbItemId = model.Id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DuplicateItems(string workboardId, string itemId)
        {
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
                return Json(new { success = true, note = workBoardItemViewModel });
            }
            return Json(new { success = false });
        }

        public async Task<ActionResult> GetOtherWorkboards(WorkBoardstatusEnum status, string id)
        {
            var model = await _workboardBusiness.GetWorkboardList(status, id);
            return Json(model);
        }

        public async Task<ActionResult> GetSectionList(string id)
        {
            var model = await _workboardBusiness.GetWorkboardSectionList(id);
            return Json(model);
        }

        public async Task<ActionResult> ReadUserData([DataSourceRequest] DataSourceRequest request, string noteId)
        {
            var model = await _workboardBusiness.GetUserList(noteId);
            var data = model.ToList();

            // var dsResult = data.ToDataSourceResult(request);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> ManageItemSharing(string workboardId, string secId, string workboardItemId, string postAction)
        {
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
                    return Json(new { success = true });
                }
            }
            else
            {
                //update wbid and section id for item
                var result = await _workboardBusiness.UpdateWorkboardItem(workboardId, secId, workboardItemId);
            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });
        }

        public ActionResult WorkBoardItemComments(string noteId)
        {
            var model = new NtsNoteCommentViewModel();
            //model.NtsNoteId = "6b739e94-2929-4852-8ad5-a9a67bb60e2e";
            model.NtsNoteId = noteId;
            model.IsAddCommentEnabled = true;

            return View(model);
        }
        public IActionResult NoteComments(string noteId, bool IsAddCommentEnabled)
        {
            var model = new NtsNoteCommentViewModel();
            model.NtsNoteId = noteId;
            model.IsAddCommentEnabled = IsAddCommentEnabled;
            return View("_NtsNoteComments", model);
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

        public async Task<ActionResult> ReadNoteCommentDataList(string noteId)
        {
            //var model = await _ntsNoteCommentBusiness.GetCommentTree(noteId, id);
            var model = await _ntsNoteCommentBusiness.GetAllCommentTree(noteId);
            return Json(model);
        }
        public async Task<IActionResult> TogetherJs()
        {

            return View();

        }
    }

}
