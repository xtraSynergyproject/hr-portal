using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Business;
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Data;
using System.IO;
using System.Web;
using CMS.UI.Utility;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class NtsNoteController : ApplicationController
    {
        private readonly INoteBusiness _noteBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IUserContext _userContext;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IPageBusiness _pageBusiness;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        private readonly IModuleBusiness _moduleBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly IPortalBusiness _portalBusiness;
        public NtsNoteController(INoteBusiness noteBusiness, ICmsBusiness cmsBusiness, IUserContext userContext, ITemplateBusiness templateBusiness
            , IPageBusiness pageBusiness, ITemplateCategoryBusiness templateCategoryBusiness,
            IModuleBusiness moduleBusiness, IPortalBusiness portalBusiness
            , ILOVBusiness lovBusiness)
        {
            _noteBusiness = noteBusiness;
            _cmsBusiness = cmsBusiness;
            _userContext = userContext;
            _templateBusiness = templateBusiness;
            _pageBusiness = pageBusiness;
            _templateCategoryBusiness = templateCategoryBusiness;
            _moduleBusiness = moduleBusiness;
            _lovBusiness = lovBusiness;
            _portalBusiness = portalBusiness;
        }
        public async Task<IActionResult> Index(string pageId, string categoryCode, string templateCode, string moduleCode, string pageName, string portalId)
        {
            var model = new NtsNoteIndexPageViewModel();
            model.PageId = pageId;
            if (pageId.IsNotNullAndNotEmpty())
            {
                model.Page = await _pageBusiness.GetPageForExecution(pageId);
            }
            if (portalId.IsNotNullAndNotEmpty() && pageName.IsNotNullAndNotEmpty())
            {
                model.Page = await _pageBusiness.GetPageForExecution(portalId, pageName);
                model.PageId = model.Page.Id;
            }
            model.CategoryCode = categoryCode;
            model.TemplateCode = templateCode;
            model.ModuleCode = moduleCode;
            model.EnableEditButton = true;
            model.EnableDeleteButton = true;
            await _noteBusiness.GetNtsNoteIndexPageCount(model);
            return View(model);
        }

        public async Task<IActionResult> LoadNtsNoteIndexPageGrid([DataSourceRequest] DataSourceRequest request, NtsActiveUserTypeEnum ownerType, string noteStatusCode, string categoryCode, string templateCode, string moduleCode, NtsViewTypeEnum? ntsViewType)
        {
            var data = await _noteBusiness.GetNtsNoteIndexPageGridData(request, ownerType, noteStatusCode, categoryCode, templateCode, moduleCode, ntsViewType);
            return Json(data.ToDataSourceResult(request));
        }
        public async Task<IActionResult> NtsNotePage(string templateid, string pageId, string dataAction, LayoutModeEnum lo, string cbm, string noteId = null)
        {
            var model = new NoteTemplateViewModel();
            model.ActiveUserId = _userContext.UserId;

            if (templateid.IsNotNullAndNotEmpty())
            {
                model.TemplateId = templateid;
            }
            if (noteId.IsNullOrEmpty())
            {
                model.DataAction = DataActionEnum.Create;
            }
            else
            {
                model.NoteId = noteId;
                if (dataAction.IsNullOrEmpty())
                {
                    model.DataAction = DataActionEnum.Edit;
                }
                else
                {
                    model.DataAction = dataAction.ToEnum<DataActionEnum>();
                }
            }
            //var taskTemplate = await _cmsBusiness.GetSingle<TaskTemplateViewModel, TaskTemplate>(x => x.TemplateId == templateid);
            var newmodel = await _noteBusiness.GetNoteDetails(model);
            newmodel.DataAction = model.DataAction;
            if (pageId.IsNotNullAndNotEmpty())
            {
                newmodel.Page = await _pageBusiness.GetPageForExecution(pageId);
                newmodel.PageId = pageId;
            }
            newmodel.LayoutMode = lo;
            newmodel.PopupCallbackMethod = cbm;
            if (newmodel.LayoutMode == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            return View(newmodel);
        }

        [HttpPost]
        public async Task<IActionResult> ManageNtsTaskPage(NoteTemplateViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var create = await _noteBusiness.ManageNote(model);
                if (create.IsSuccess)
                {
                    var msg = "Item created successfully.";
                    if (model.LayoutMode.HasValue && model.LayoutMode == LayoutModeEnum.Popup)
                    {
                        return Json(new { success = true, msg = msg, vm = model, mode = model.LayoutMode.ToString(), cbm = model.PopupCallbackMethod });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = true,
                            msg = msg,
                            reload = true,
                            pageId = model.PageId,
                            pageType = TemplateTypeEnum.Custom.ToString(),
                            //model.Page.PageType.ToString(),
                            source = RequestSourceEnum.Edit.ToString(),
                            dataAction = DataActionEnum.Edit.ToString(),
                            recordId = create.Item.NoteId,
                        });
                    }
                }
                else
                {
                    return Json(new { success = false, error = create.HtmlError });
                }
            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                var edit = await _noteBusiness.ManageNote(model);

                if (edit.IsSuccess)
                {
                    var msg = "Item updated successfully.";
                    if (model.LayoutMode.HasValue && model.LayoutMode == LayoutModeEnum.Popup)
                    {
                        return Json(new { success = true, msg = msg, vm = model, mode = model.LayoutMode.ToString(), cbm = model.PopupCallbackMethod });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = true,
                            msg = msg,
                            reload = true,
                            pageId = model.PageId,
                            //pageType = model.Page.PageType.ToString(),
                            pageType = TemplateTypeEnum.Custom.ToString(),
                            source = RequestSourceEnum.Edit.ToString(),
                            dataAction = DataActionEnum.Edit.ToString(),
                            recordId = edit.Item.NoteId,
                        });
                    }
                }
                else
                {
                    return Json(new { success = false, error = edit.HtmlError });
                }
            }
            return Json(new { success = false, error = "Invalid action" });
        }

        public IActionResult SelectNoteTemplate(string templateCode, string categoryCode, string userId, string moduleCodes, string prms, string cbm, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard,string portalNames=null)
        {
            var model = new TemplateViewModel();
            model.Code = templateCode;
            model.CategoryCode = categoryCode;
            model.UserId = userId;
            model.ModuleCodes = moduleCodes;
            model.Prms = prms;
            model.CallBackMethodName = cbm;
            model.TemplateIds = templateIds;
            model.CategoryIds = categoryIds;
            model.TemplateCategoryType = categoryType;
            model.PortalNames = portalNames;
            return View(model);
        }

        public async Task<IActionResult> ReadNoteTemplate(string templateCode = null, string categoryCode = null, string moduleCodes = null, string templateIds = null, string categoryIds = null, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalNames = null)
        {
            var result = await _templateBusiness.GetNoteTemplateList(templateCode, categoryCode, moduleCodes, templateIds, categoryIds, categoryType, portalNames);
            var j = Json(result);
            return j;
        }
        public async Task<IActionResult> ReadNoteTemplateCategory(string categoryCode, string templateCode, string moduleCodes, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType, string portalNames = null)
        {
            //var result = new List<TemplateCategoryViewModel>();
            //if (categoryCode.IsNotNullAndNotEmpty())
            //{
            //    result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Note && x.Code == categoryCode);
            //}
            //else if (categoryIds.IsNotNullAndNotEmpty())
            //{
            //    var Ids = categoryIds.Split(",");
            //    result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Note && Ids.Any(y => y == x.Id));
            //}
            //else if (moduleCodes.IsNotNullAndNotEmpty())
            //{
            //    var modules = moduleCodes.Split(",");
            //    result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Note && modules.Any(p => p == x.Module.Code), y => y.Module);
            //}
            //else
            //{
            //    result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Note && x.TemplateCategoryType == TemplateCategoryTypeEnum.Standard);
            //}

            var result = await _templateCategoryBusiness.GetCategoryList(templateCode, categoryCode, moduleCodes, categoryIds, templateIds, TemplateTypeEnum.Note, categoryType,false, portalNames);
            var j = Json(result);
            return j;
        }
        public async Task<IActionResult> GetNoteMessageList(string noteId)
        {
            var result = await _noteBusiness.GetNoteMessageList(_userContext.UserId, noteId);
            return Json(result);
        }
        public async Task<ActionResult> NoteHome(string userId, string noteStatus, string mode, LayoutModeEnum? lo, ModuleEnum? moduleName = null, string layout = null, string moduleId = null, string rs = null, string noteId = null, string moduleCodes = null,string portalNames=null)
        {
            //noteId = "656810b6-bd9b-4fc1-8834-2bd003204e7d";
            var model = new NoteSearchViewModel { Operation = DataOperation.Read, NoteStatus = noteStatus, Mode = mode, ModuleName = moduleName };
            if (!userId.IsNotNull())
            {
                model.UserId = _userContext.UserId;
            }
            if (noteId.IsNotNullAndNotEmpty())
            {
                model.NoteId = noteId;
                var noteresult = await _noteBusiness.GetSingleById(noteId);
                if (noteresult != null)
                {
                    ViewBag.TemplateMasterCode = noteresult.TemplateCode;
                }
            }
            ViewBag.Msg = "Note";
            //if (noteStatus.IsNotNull())
            //{
            //    //var val = _lovBusiness.GetListOfValue("NOTE_STATUS", noteStatus);
            //    //ViewBag.Msg = ViewBag.Msg + " " + val.Name;
            //}
            if (noteStatus.IsNotNull())
            {
                var val = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_NOTE_STATUS" && x.Code == noteStatus);
                //ViewBag.Msg = ViewBag.Msg + " " + val.Name;
                ViewBag.Status = val.Name;
            }
            else
            {
                ViewBag.Status = "All";
            }
            if (mode.IsNotNull())
            {
                if (mode == "SHARE_TO")
                {
                    ViewBag.Person = "Shared with me/Team";
                }

                else if (mode == "REQ_BY")
                {
                    ViewBag.Person = "Owned by me";
                }
                else if (mode == "ASSIGN_BY")
                {
                    ViewBag.Person = "shared by me/Team";
                }
                else
                {
                    ViewBag.Person = "All";
                }
                // ViewBag.Msg = ViewBag.Msg + "(Shared)";
            }
            else
            {
                ViewBag.Person = "All";
            }
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            var moduleslist = await _moduleBusiness.GetList(x => x.PortalId == _userContext.PortalId);

            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modules = moduleCodes.Split(",");
                model.ModuleList = moduleslist.Where(x => x.Code != null && modules.Any(y => y == x.Code)).ToList();

            }
            else
            {
                model.ModuleList = moduleslist;
            }
            if (moduleId.IsNotNullAndNotEmpty())
            {
                ViewBag.Module = model.ModuleList.Where(x => x.Id == moduleId).Select(y => y.Name);
            }
            else
            {
                ViewBag.Module = "All";
            }
            model.ModuleCode = moduleCodes;
            model.RequestSource = rs;
            model.PortalNames = portalNames;
            // model.ModuleList = await _moduleBusiness.GetList();
            ViewBag.LoggedInUser = _userContext.UserId;
            return View(model);

        }
        public async Task<IActionResult> ReadNoteHomeData(NoteSearchViewModel search = null)
        {
            if (!search.UserId.IsNotNull())
            {
                search.UserId = _userContext.UserId;
            }
            if (search.PortalNames.IsNotNullAndNotEmpty())
            {
                string[] names = search.PortalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                string[] ids = portals.Select(x => x.Id).ToArray();
                search.PortalNames = String.Join("','", ids);
            }
            //var result = new List<NoteViewModel>();//await _business.GetSearchResult(search).OrderByDescending(x => x.LastUpdatedDate);
            var result = await _noteBusiness.GetSearchResult(search);
            if (search?.Text == "Today")
            {
                var res = result.Where(x => x.ExpiryDate <= DateTime.Now && x.NoteStatusCode != "COMPLETED" && x.NoteStatusCode != "CANCELED" && x.NoteStatusCode != "DRAFT");
                return Json(res);
            }
            else if (search?.Text == "Week")
            {
                var res = result.Where(x => (x.ExpiryDate <= DateTime.Now.AddDays(7) && x.NoteStatusCode != "COMPLETED" && x.NoteStatusCode != "CANCELED" && x.NoteStatusCode != "DRAFT")).ToList();
                return Json(res);
            }
            var j = Json(result);
            return j;
        }
        public ActionResult NoteHomeDashboard()
        {
            ProjectDashboardViewModel model = new ProjectDashboardViewModel();
            return View(model);
        }
        public async Task<ActionResult> GetNoteChartByStatus()
        {
            NoteSearchViewModel search = new NoteSearchViewModel();
            search.UserId = _userContext.UserId;
            var viewModel = await _noteBusiness.GetSearchResult(search);
            var list1 = viewModel.GroupBy(x => x.NoteStatusCode).Select(group => new { Value = group.Count(), Type = group.Select(x => x.NoteStatusName).FirstOrDefault(), Id = group.Select(x => x.NoteStatusId).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            //var list = new List<ProjectDashboardChartViewModel>();

            return Json(list);
        }
        public async Task<ActionResult> GetNoteChartByUserType()
        {
            //NoteSearchViewModel search = new NoteSearchViewModel();
            //search.UserId = _userContext.UserId;
            //var viewModel = await _noteBusiness.GetSearchResult(search);
            //var list1 = viewModel.GroupBy(x => x.TemplateUserType).Select(group => new { Value = group.Count(), Type = group.Select(x => x.TemplateUserType).FirstOrDefault(), Id = group.Select(x => Convert.ToInt32(x.TemplateUserType)).FirstOrDefault() }).ToList();
            //var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type.ToString(), Value = x.Value, Id = x.Id.ToString() }).ToList();
            var list = new List<ProjectDashboardChartViewModel>();

            return Json(list);
        }
        public async Task<IActionResult> ReadNoteDashBoardGridData(/*[DataSourceRequest] DataSourceRequest request,*/List<string> NoteStatusIds, NoteSearchViewModel search = null)
        {

            if (!search.UserId.IsNotNull())
            {
                search.UserId = _userContext.UserId;
            }
            search.NoteStatusIds = NoteStatusIds;
            var result = await _noteBusiness.GetSearchResult(search);
            //if (search?.Text == "Today")
            //{
            //    var res = result.Where(x => x.DueDate <= DateTime.Now && x.NoteStatusCode != "NOTE_STATUS_COMPLETE" && x.ServiceStatusCode != "SERVICE_STATUS_CANCEL" && x.ServiceStatusCode != "SERVICE_STATUS_DRAFT");
            //    return Json(res.ToDataSourceResult(request));
            //}
            //else if (search?.Text == "Week")
            //{
            //    var res = result.Where(x => (x.DueDate <= DateTime.Now.AddDays(7) && DateTime.Now <= x.DueDate && x.ServiceStatusCode != "SERVICE_STATUS_COMPLETE" && x.ServiceStatusCode != "SERVICE_STATUS_CANCEL" && x.ServiceStatusCode != "SERVICE_STATUS_DRAFT")).ToList();
            //    return Json(res.ToDataSourceResult(request));
            //}
            //else if (search.ServiceStatusIds.IsNotNull() && search.ServiceStatusIds.Count() > 0)
            //{
            //    var status = string.Join(",", search.ServiceStatusIds);
            //    var res = result.Where(x => status.Contains(x.ServiceStatusId)).ToList();
            //    return Json(res.ToDataSourceResult(request));
            //}
            //else if (search.UserType.IsNotNull() && search.UserType.Count() > 0)
            //{
            //    var UserType = string.Join(",", search.UserType);
            //    var res = result.Where(x => UserType.Contains(x.TemplateUserType.ToString())).ToList();
            //    return Json(res.ToDataSourceResult(request));
            //}
            var j = Json(result/*.ToDataSourceResult(request)*/);
            return j;
        }
        public async Task<IActionResult> DeleteNote(string id)
        {
            await _noteBusiness.Delete(id);
            return Json(new { success = true });
        }
        public async Task<ActionResult> GetNoteSharedUsersIdNameList(string noteId)
        {
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            list = await _noteBusiness.GetSharedList(noteId);
            list.Add(new IdNameViewModel { Id = "All", Name = "All" });
            return Json(list);
        }
        public async Task<ActionResult> GetNoteDetails(string id)
        {
            var data = await _noteBusiness.GetSingleById(id);
            return Json(data);
        }
        public async Task<IActionResult> NoteBookHTML(string noteId, string templateId)
        {
            var model = await _noteBusiness.GetBookDetails(noteId);
            return View(model);
        }

        public IActionResult NoteItemMove(string noteId, NtsTypeEnum ntsType, string ntsId)
        {
            var model = new NoteViewModel();
            model.Id = noteId;
            model.NtsType = ntsType;
            model.NtsId = ntsId;
            return View(model);
        }

        public async Task<IActionResult> GetMoveToParent(string noteId, string ntsId, string parentId)
        {
            var result = await _noteBusiness.GetBookList(noteId, null);
            result = result.Where(x => x.ItemType != ItemTypeEnum.StepTask && x.Id != "0" && x.Id != noteId).ToList();
            if (ntsId.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.Id != ntsId).ToList();
            }
            if (parentId.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.parentId == parentId).ToList();
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ManageMoveToParent(NoteViewModel model)
        {
            var result = await _noteBusiness.ManageMoveToParent(model);
            if (result.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, error = result.Message });
        }

    }
}
