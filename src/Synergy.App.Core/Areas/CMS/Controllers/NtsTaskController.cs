using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Globalization;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class NtsTaskController : ApplicationController
    {
        private readonly ITaskBusiness _taskBusiness;
        private readonly IModuleBusiness _moduleBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IUserContext _userContext;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IPageBusiness _pageBusiness;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IWebHelper _webApi;
        private readonly IPortalBusiness _portalBusiness;
        ICompanySettingBusiness _companySettingBusiness;

        public NtsTaskController(ITaskBusiness taskBusiness, ICmsBusiness cmsBusiness, IUserContext userContext, ITemplateBusiness templateBusiness
            , IPageBusiness pageBusiness
            , IWebHelper webApi
            , IModuleBusiness moduleBusiness
            , ILOVBusiness lovBusiness
            , ITemplateCategoryBusiness templateCategoryBusiness
            , IUserBusiness userBusiness, IPortalBusiness portalBusiness
            , ICompanySettingBusiness companySettingBusiness
            )
        {
            _taskBusiness = taskBusiness;
            _cmsBusiness = cmsBusiness;
            _userContext = userContext;
            _templateBusiness = templateBusiness;
            _pageBusiness = pageBusiness;
            _webApi = webApi;
            _moduleBusiness = moduleBusiness;
            _lovBusiness = lovBusiness;
            _templateCategoryBusiness = templateCategoryBusiness;
            _userBusiness = userBusiness;
            _portalBusiness = portalBusiness;
            _companySettingBusiness = companySettingBusiness;
        }
        public async Task<IActionResult> Index(string pageId, string categoryCode, string templateCode, string moduleCode, string pageName, string portalId)
        {
            var model = new NtsTaskIndexPageViewModel();
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
            await _taskBusiness.GetNtsTaskIndexPageCount(model);
            return View(model);
        }
        public async Task<IActionResult> LoadNtsTaskIndexPageGrid([DataSourceRequest] DataSourceRequest request, NtsActiveUserTypeEnum ownerType, string taskStatusCode, string categoryCode, string templateCode, string moduleCode)
        {
            var data = await _taskBusiness.GetNtsTaskIndexPageGridData(request, ownerType, taskStatusCode, categoryCode, templateCode, moduleCode);
            return Json(data);
            //return Json(data.ToDataSourceResult(request));
        }
        public async Task<IActionResult> NtsTaskPage(string templateid, string pageId, string dataAction, string serviceId, string portalId, string pageName, LayoutModeEnum lo, string cbm, string taskId = null)
        {
            //= "6ebbcff1-69fc-4814-8f3d-fb68d181c183"
            var model = new TaskTemplateViewModel();
            model.ActiveUserId = _userContext.UserId;

            if (templateid.IsNotNullAndNotEmpty())
            {
                model.TemplateId = templateid;
            }
            if (taskId.IsNullOrEmpty())
            {
                model.DataAction = DataActionEnum.Create;
            }
            else
            {
                model.TaskId = taskId;
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
            var newmodel = await _taskBusiness.GetTaskDetails(model);
            newmodel.DataAction = model.DataAction;
            if (pageId.IsNotNullAndNotEmpty())
            {
                newmodel.Page = await _pageBusiness.GetPageForExecution(pageId);
                newmodel.PageId = pageId;
            }
            else
            {
                if (portalId.IsNotNullAndNotEmpty() && pageName.IsNotNullAndNotEmpty())
                {
                    newmodel.Page = await _pageBusiness.GetPageForExecution(portalId, pageName);
                    newmodel.PageId = newmodel.Page.Id;
                }
            }
            newmodel.LayoutMode = lo;
            newmodel.PopupCallbackMethod = cbm;
            if (newmodel.LayoutMode == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            if (serviceId.IsNotNullAndNotEmpty())
            {
                newmodel.ParentServiceId = serviceId;
            }
            return View(newmodel);
        }
        [HttpPost]
        public async Task<IActionResult> ManageNtsTaskPage(TaskTemplateViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var create = await _taskBusiness.ManageTask(model);
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
                            recordId = create.Item.TaskId,
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
                var edit = await _taskBusiness.ManageTask(model);
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
                            recordId = edit.Item.TaskId,
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
        public async Task<IActionResult> GetFormIoJson(string templateId, string pageId, string taskId)
        {

            var page = await _pageBusiness.GetSingleById(pageId);
            var viewName = page.PageType;
            if (templateId.IsNotNullAndNotEmpty())
            {
                page.Template = await _templateBusiness.GetSingleById(templateId);
                page.Template.TableMetadataId = page.Template.UdfTableMetadataId;
            }
            if (taskId.IsNotNullAndNotEmpty())
            {
                var task = await _taskBusiness.GetSingleById(taskId);
                if (task != null)
                {
                    page.RecordId = task.UdfNoteId;
                    page.PageType = TemplateTypeEnum.Note;
                }
            }
            var data = await GetTaskModel(page, viewName);
            if (page.Template.Json.IsNotNullAndNotEmpty())
            {
                page.Template.Json = _webApi.AddHost(page.Template.Json);
            }
            return Json(new { uiJson = page.Template.Json, dataJson = data });

        }
        private async Task<string> GetTaskModel(PageViewModel page, TemplateTypeEnum viewName)
        {
            var data = await _cmsBusiness.GetDataById(viewName, page, page.RecordId);
            if (data == null)
            {
                return "{}";
            }
            return data.ToJson();
        }
        public IActionResult SelectTaskTemplate(string templateCode, string categoryCode, string moduleCodes, string userId, string prms, string cbm, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalNames = null)
        {
            var model = new TemplateViewModel();
            model.Code = templateCode;
            model.CategoryCode = categoryCode;
            model.ModuleCodes = moduleCodes;
            model.UserId = userId;
            model.Prms = prms;
            model.CallBackMethodName = cbm;
            model.TemplateIds = templateIds;
            model.CategoryIds = categoryIds;
            model.TemplateCategoryType = categoryType;
            model.PortalNames = portalNames;
            return View(model);
        }
        public IActionResult SelectAdhocTaskTemplate(string templateCode, string categoryCode, string moduleId)
        {
            var model = new TemplateViewModel();
            model.Code = templateCode;
            model.CategoryCode = categoryCode;
            model.ModuleId = moduleId;
            return View(model);
        }
        public async Task<IActionResult> ReadTaskTemplate(string templateCode, string categoryCode, string moduleCodes, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalNames = null)
        {
            var result = await _templateBusiness.GetTemplateList(templateCode, categoryCode, moduleCodes, templateIds, categoryIds, categoryType, portalNames);
            result = result.OrderBy(x => x.DisplayName).ToList();
            var j = Json(result);
            return j;
        }
        public async Task<IActionResult> ReadAdhocTaskTemplate(string templateCode, string categoryCode, string moduleId)
        {
            var result = await _templateBusiness.GetAdhocTemplateList(templateCode, categoryCode, moduleId);
            result = result.OrderBy(x => x.DisplayName).ToList();
            var j = Json(result);
            return j;

        }
        public async Task<IActionResult> ReadTaskTemplateCategory(string categoryCode, string templateCode, string moduleCodes, string moduleId, string categoryIds, string templateIds, TemplateCategoryTypeEnum templateCategoryType, string portalNames = null)
        {
            //            var result = new List<TemplateCategoryViewModel>();
            //            if (categoryCode.IsNotNullAndNotEmpty())
            //            {
            //                var Code = categoryCode.Split(",");
            //                result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Task && Code.Any(y => y == x.Code));
            //            }
            //            else if(categoryIds.IsNotNullAndNotEmpty())
            //            {
            //                var Ids = categoryIds.Split(",");
            //                result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Task && Ids.Any(y => y == x.Id));
            //}
            //            else if (moduleCodes.IsNotNullAndNotEmpty())
            //            {
            //                var modules = moduleCodes.Split(",");
            //                result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Task && modules.Any(p => p == x.Module.Code), y => y.Module);
            //            }
            //            else if(moduleId.IsNotNullAndNotEmpty())
            //            {
            //                result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Task && x.ModuleId == moduleId && x.TemplateCategoryType == TemplateCategoryTypeEnum.Other);
            //            }
            //            else
            //            {
            //                result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Task && x.TemplateCategoryType == TemplateCategoryTypeEnum.Standard);
            //            }

            var result = await _templateCategoryBusiness.GetCategoryList(templateCode, categoryCode, moduleCodes, categoryIds, templateIds, TemplateTypeEnum.Task, templateCategoryType, false, portalNames);

            var j = Json(result);
            return j;
        }
        public async Task<IActionResult> GetTaskMessageList(string taskId)
        {
            var result = await _taskBusiness.GetTaskMessageList(_userContext.UserId, taskId);
            return Json(result);
        }
        public async Task<IActionResult> LoadServiceAdhocTaskGrid(string adhocTaskTemplateIds, string serviceId)
        {
            var data = await _taskBusiness.GetServiceAdhocTaskGridData(null, adhocTaskTemplateIds, serviceId);
            return Json(data);
        }
        public async Task<IActionResult> TaskHome(string userId, string taskStatus, string mode, LayoutModeEnum? lo, string layoutMode = null, string mobileView = "N", string moduleId = null, string rs = null, string taskId = null, string moduleCodes = null, string categoryCodes = null, string templateCodes = null, DateTime? date = null, string portalNames = null)
        {

            //taskId = "30f5108e-6897-4db1-ad94-e9fe577caf70";
            var model = new TaskSearchViewModel { Operation = DataOperation.Read, TaskStatus = taskStatus, Mode = mode, ModuleId = moduleId };
            if (!userId.IsNotNull())
            {
                model.UserId = _userContext.UserId;
            }
            if (taskId.IsNotNullAndNotEmpty())
            {
                model.TaskId = taskId;
                var taskresult = await _taskBusiness.GetSingleById(taskId);
                if (taskresult != null)
                {
                    ViewBag.TemplateMasterCode = taskresult.TemplateCode;
                }
            }
            ViewBag.Msg = "Task";
            if (taskStatus.IsNotNull())
            {
                var val = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_TASK_STATUS" && x.Code == taskStatus);
                ViewBag.Msg = ViewBag.Msg + " " + val.Name;
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
                else if (mode == "ASSIGN_TO")
                {
                    ViewBag.Person = "Assigned to me";
                }
                else if (mode == "ASSIGN_BY")
                {
                    ViewBag.Person = "Requested by me";
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
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
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

            if (taskStatus == "TASK_STATUS_OVERDUE")
            {
                if (date.IsNotNull())
                {
                    model.DueDate = date.Value.AddDays(-1);
                }

            }
            else if (taskStatus == "TASK_STATUS_INPROGRESS")
            {
                if (date.IsNotNull())
                {
                    model.StartDate = date.Value;
                }

            }
            else if (taskStatus == "TASK_STATUS_CLOSE")
            {
                if (date.IsNotNull())
                {
                    model.ClosedDate = date.Value;
                }

            }
            else if (taskStatus == "TASK_STATUS_NOTSTARTED")
            {
                if (date.IsNotNull())
                {
                    model.StartDate = date.Value;
                }

            }
            else if (taskStatus == "REMINDER")
            {
                if (date.IsNotNull())
                {

                    model.ReminderDate = date.Value;
                }

                model.TaskStatus = null;
            }

            model.ModuleCode = moduleCodes;
            model.TemplateCategoryCode = categoryCodes;
            model.TemplateMasterCode = templateCodes;
            model.RequestSource = rs;
            model.PortalNames = portalNames;
            return View(model);
        }
        public async Task<IActionResult> ReadTaskHomeData(TaskSearchViewModel search = null)
        {
            if (!search.UserId.IsNotNull())
            {
                search.UserId = _userContext.UserId;
            }
            var result = await _taskBusiness.GetSearchResult(search);
            //result = result.Where(x => x.PortalId == _userContext.PortalId).ToList();
            if (search?.Text == "Today")
            {
                var res = result.Where(x => x.DueDate <= DateTime.Now && x.TaskStatusCode != "TASK_STATUS_COMPLETE" && x.TaskStatusCode != "TASK_STATUS_CANCEL" && x.TaskStatusCode != "TASK_STATUS_DRAFT");
                return Json(res);
            }
            else if (search?.Text == "Week")
            {
                var res = result.Where(x => (x.DueDate <= DateTime.Now.AddDays(7) && DateTime.Now <= x.DueDate && x.TaskStatusCode != "TASK_STATUS_COMPLETE" && x.TaskStatusCode != "TASK_STATUS_CANCEL" && x.TaskStatusCode != "TASK_STATUS_DRAFT")).ToList();
                return Json(res);
            }

            var j = Json(result.OrderByDescending(x => x.LastUpdatedDate));
            return j;
        }

        [HttpPost]
        public async Task<ActionResult> DeleteTask(string taskId)
        {
            await _taskBusiness.Delete(taskId);
            return Json(new { success = true });
        }

        public IActionResult TaskHomeDashboard()
        {
            ProjectDashboardViewModel model = new ProjectDashboardViewModel();
            return View(model);
        }
        public async Task<ActionResult> GetTaskChartByStatus()
        {
            TaskSearchViewModel search = new TaskSearchViewModel();
            search.UserId = _userContext.UserId;
            var viewModel = await _taskBusiness.GetSearchResult(search);
            var list1 = viewModel.GroupBy(x => x.TaskStatusCode).Select(group => new { Value = group.Count(), Type = group.Select(x => x.TaskStatusName).FirstOrDefault(), Id = group.Select(x => x.TaskStatusId).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return Json(list);
        }
        public async Task<ActionResult> GetTaskChartByUserType()
        {
            TaskSearchViewModel search = new TaskSearchViewModel();
            search.UserId = _userContext.UserId;
            var viewModel = await _taskBusiness.GetSearchResult(search);
            var list1 = viewModel.GroupBy(x => x.TemplateUserType).Select(group => new { Value = group.Count(), Type = group.Select(x => x.TemplateUserType).FirstOrDefault(), Id = group.Select(x => Convert.ToInt32(x.TemplateUserType)).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type.ToString(), Value = x.Value, Id = x.Id.ToString() }).ToList();
            return Json(list);
        }
        public async Task<IActionResult> ReadTaskDashBoardGridData(TaskSearchViewModel search = null)
        {

            if (!search.UserId.IsNotNull())
            {
                search.UserId = _userContext.UserId;
            }
            var result = await _taskBusiness.GetSearchResult(search);
            //if (search?.Text == "Today")
            //{
            //    var res = result.Where(x => x.DueDate <= DateTime.Now && x.TaskStatusCode != "TASK_STATUS_COMPLETE" && x.TaskStatusCode != "TASK_STATUS_CANCEL" && x.TaskStatusCode != "TASK_STATUS_DRAFT");
            //    return Json(res.ToDataSourceResult(request));
            //}
            //else if (search?.Text == "Week")
            //{
            //    var res = result.Where(x => (x.DueDate <= DateTime.Now.AddDays(7) && DateTime.Now <= x.DueDate && x.TaskStatusCode != "TASK_STATUS_COMPLETE" && x.TaskStatusCode != "TASK_STATUS_CANCEL" && x.TaskStatusCode != "TASK_STATUS_DRAFT")).ToList();
            //    return Json(res.ToDataSourceResult(request));
            //}

            if (search.IsNotNull() && search.TaskOwnerIds.IsNotNull())
            {
                var status = string.Join(",", search.TaskOwnerIds);
                result = result.Where(x => status.Contains(x.OwnerUserId)).ToList();
            }
            if (search.IsNotNull() && search.TaskAssigneeIds.IsNotNull())
            {
                var status = string.Join(",", search.TaskAssigneeIds);
                result = result.Where(x => status.Contains(x.AssignedToUserId)).ToList();
            }
            if (search.IsNotNull() && search.TaskStatusIds.IsNotNull())
            {
                var status = string.Join(",", search.TaskStatusIds);
                result = result.Where(x => status.Contains(x.TaskStatusId)).ToList();
            }

            var j = Json(result);
            return j;
        }
        public async Task<ActionResult> GetOwnerIdNameList()
        {
            TaskSearchViewModel search = new TaskSearchViewModel();
            search.UserId = _userContext.UserId;
            var viewModel = await _taskBusiness.GetSearchResult(search);
            var list1 = viewModel.GroupBy(x => x.OwnerUserName).Select(group => new { Id = group.Select(x => x.OwnerUserId).FirstOrDefault(), Name = group.Select(x => x.OwnerUserName).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new TaskSearchViewModel { Id = x.Id, OwnerName = x.Name }).ToList();
            return Json(list);
        }
        public async Task<ActionResult> GetUserIdNameList()
        {
            TaskSearchViewModel search = new TaskSearchViewModel();
            search.UserId = _userContext.UserId;
            var viewModel = await _taskBusiness.GetSearchResult(search);
            var list1 = viewModel.GroupBy(x => x.AssignedToUserId).Select(group => new { Id = group.Select(x => x.AssignedToUserId).FirstOrDefault(), Name = group.Select(x => x.AssigneeDisplayName).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new TaskSearchViewModel { Id = x.Id, AssignedToUserName = x.Name }).ToList();
            return Json(list);
        }

        public async Task<ActionResult> ReadDatewiseTaskSLA(TaskSearchViewModel search = null)
        {
            if (search.StartDate != null && search.DueDate != null)
            {
                search.UserId = _userContext.UserId;
                var viewModel = await _taskBusiness.GetDatewiseTaskSLA(search);
                return Json(viewModel);
            }
            else { return Json(""); }
        }
        public async Task<ActionResult> GetTaskSharedUsersIdNameList(string taskId)
        {
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            list = await _taskBusiness.GetTaskUserList(taskId);
            list.Add(new IdNameViewModel { Id = "All", Name = "All" });
            return Json(list);
        }
        public async Task<ActionResult> GetTaskByUser(string userId)
        {
            IList<TaskViewModel> model = new List<TaskViewModel>();
            if (userId != null)
            {
                model = await _taskBusiness.GetTaskByUser(userId);
            }
            var json = Json(model);

            return json;
        }
        [HttpPost]
        public async Task<ActionResult> ReAssignTerminatedEmployeeTasks(string tasks, string userId)
        {
            List<string> taskIds = new List<string>();
            var Str = tasks.Trim(',');
            var ids = Str.Split(',').Distinct();
            foreach (var id in ids)
            {
                taskIds.Add(id);
            }
            await _taskBusiness.ReAssignTerminatedEmployeeTasks(userId, taskIds);
            return Json(new { success = false });
        }
        public IActionResult TaskList(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, string portalNames = null)
        {
            ViewBag.reqType = requestby;
            ViewBag.PortalNames = portalNames;
            var model = new TaskViewModel { ModuleCode = moduleCodes, TemplateCode = templateCodes, TemplateCategoryCode = categoryCodes };
            return View(model);
        }
        public IActionResult SupportTicket(string moduleCodes, string templateCodes, string categoryCodes)
        {
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
                var j = Json(result.Where(x => x.StatusGroupCode == "PENDING" && x.AssignedToUserId == _userContext.UserId).OrderByDescending(x => x.StartDate));
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
            //var j = Json(result.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE" && x.AssignedToUserId == _userContext.LoggedInAsByUserId).OrderByDescending(x => x.StartDate));
            var j = Json(result.Where(x => x.StatusGroupCode == "DONE" && x.AssignedToUserId == _userContext.UserId).OrderByDescending(x => x.StartDate));
            return j;
        }
        public IActionResult TaskWorkPerformance(string moduleCodes = null)
        {
            ProjectDashboardViewModel model = new ProjectDashboardViewModel();
            model.ModuleCode = moduleCodes;
            return View(model);
        }
        public async Task<ActionResult> GetTaskChartPerformanceTrend(TaskSearchViewModel search = null)
        {
            search.UserId = _userContext.UserId;

            var list = await _taskBusiness.GetWorkPerformanceTaskList(search);
            return Json(list.ToList());
        }

        public async Task<ActionResult> ReadTaskPerformanceSLA(TaskSearchViewModel search = null)
        {
            search.StartDate = DateTime.Today.AddMonths(-1);
            search.DueDate = DateTime.Today;
            if (search.StartDate != null && search.DueDate != null)
            {
                search.UserId = _userContext.UserId;
                var viewModel = await _taskBusiness.GetDatewiseTaskSLA(search);
                if (viewModel != null && viewModel.Count() > 0)
                {
                    foreach (var item in viewModel)
                    {
                        var sla = ((item.Days * 12) / 12) * 100;
                        if (sla <= 0)
                        {
                            item.SLAValue = 0;
                        }
                        else if (sla > 0 && sla < 60)
                        {
                            item.SLAValue = 100;
                        }
                        else if (sla >= 60 && sla < 80)
                        {
                            item.SLAValue = 85;
                        }
                        else if (sla >= 80 && sla < 100)
                        {
                            item.SLAValue = 70;
                        }
                        else if (sla >= 100 && sla < 120)
                        {
                            item.SLAValue = 55;
                        }
                        else if (sla >= 120 && sla < 140)
                        {
                            item.SLAValue = 40;
                        }
                        else if (sla >= 140)
                        {
                            item.SLAValue = 40;
                        }

                    }
                }
                return Json(viewModel);
            }
            else { return Json(""); }
        }

        public async Task<ActionResult> ReadWorkPerformanceCalendarData([DataSourceRequest] DataSourceRequest request, string moduleCodes, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var list = await _taskBusiness.GetWorkPerformanceCount(_userContext.UserId, moduleCodes, fromDate, toDate);
            return Json(list);
            // return Json(list.ToDataSourceResult(request));

        }

        public IActionResult TaskListByFilters(string userId = null, string statusCodes = null, string templateCodes = null, string parentServiceId = null, string parentNoteId = null, bool enableCreate = false, string cbm = null, string serId = null, string serTempCodes = null,string portalIds=null)
        {
            ViewBag.EnableCreateButton = enableCreate;
            ViewBag.CBM = cbm;
            return View(new TaskTemplateViewModel
            {
                OwnerUserId = userId,
                TemplateCode = templateCodes,
                TaskStatusCode = statusCodes,
                ParentServiceId = parentServiceId,
                ParentNoteId = parentNoteId,
                PortalId = _userContext.PortalId,
                PortalIds = portalIds.IsNotNull()?portalIds: _userContext.PortalId,
                ServicePlusId = serId,
                ServiceTemplateCodes = serTempCodes
            });
        }

        public async Task<IActionResult> ReadTaskList(string templateCodes = null, string moduleCodes = null, string catCodes = null, string statusCodes = null, string parentServiceId = null, string userId = null, string parentNoteId = null, string serId = null, string serTempCodes = null, string portalIds = null)
        {
            var dt = await _taskBusiness.GetTaskList(portalIds, moduleCodes, templateCodes, catCodes, statusCodes, parentServiceId, userId, parentNoteId, serId, serTempCodes);
            return Json(dt);
        }

        public async Task<IActionResult> TaskListDashboard(string categoryCodes = null, string portal=null, bool showAllTaskForAdmin = false,string templateCodes=null,string portalNames=null,string statusCodes=null,string userId=null)
        {
            //var templates = await _templateBusiness.GetTemplateServiceList(null, categoryCodes, null, null, null, TemplateCategoryTypeEnum.Standard);

            //string[] codes = templates.Select(x => x.Code).ToArray();
            //var templatecodes = string.Join(",", codes);
            //ViewBag.TemplateCodes = templatecodes;
            if (portal == "EGOV")
            {
                var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
                ViewBag.ReturnUrl = cs.Value;
            }
            ViewBag.CategoryCodes = categoryCodes;
            ViewBag.ShowAllTaskForAdmin = showAllTaskForAdmin;
            ViewBag.TemplateCodes = templateCodes;
            ViewBag.PortalNames = portalNames;
            ViewBag.StatusCodes = statusCodes;
            ViewBag.UserId = userId;

            return View();
        }

        public async Task<IActionResult> ReadTaskListCount(string categoryCodes = null, bool showAllTaskForAdmin = false, string templateCodes = null, string portalNames = null, string statusCodes = null, string userId = null)
        {
            var result = await _taskBusiness.GetTaskCountByServiceTemplateCodes(categoryCodes, _userContext.PortalId, showAllTaskForAdmin, templateCodes, portalNames, statusCodes, userId);
            var j = Json(result);
            return j;
        }

        public async Task<IActionResult> ReadTaskData(string categoryCodes = null, string taskStatus = null, bool showAllTaskForAdmin = false, string templateCodes = null, string portalNames = null, string userId = null)
        {
            var list = await _taskBusiness.GetTaskListByServiceCategoryCodes(categoryCodes, taskStatus, _userContext.PortalId, showAllTaskForAdmin, templateCodes, portalNames, userId);
            //var j = Json(list.ToDataSourceResult(request));
            var j = Json(list);
            return j;
        }

        public async Task<IActionResult> TemplatesListWithTaskCount(string templateCodes = null, string catCodes = null, string groupCodes = null, bool showAllTasksForAdmin = false, bool showAllTask=false, string portalNames = null)
        {
            var portalId = _userContext.PortalId;
            if (portalNames.IsNotNull())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                portalId = string.Join(",", portals.Select(x => x.Id).ToArray());
            }
            var model = new TemplateViewModel()
            {
                TemplateCodes = templateCodes,
                CategoryCodes = catCodes,
                GroupCodes = groupCodes,
                ShowAllServicesForAdmin = showAllTasksForAdmin,
                UserId = _userContext.UserId,
                PortalId = portalId
            };
            if (showAllTask)
            {
                model.UserId = null;
            }
            else if(showAllTasksForAdmin && _userContext.IsSystemAdmin)
            {
                model.UserId = null;
            }
            return View(model);
        }
        public async Task<IActionResult> ReadTemplatesListWithTaskCount(string templateCodes = null, string catCodes = null, string groupCodes = null, bool showAllTasksForAdmin = false)
        {
            var dt = await _taskBusiness.GetTemplatesListWithTaskCount(templateCodes, catCodes, groupCodes, showAllTasksForAdmin);
            return Json(dt);
        }



    }
}
