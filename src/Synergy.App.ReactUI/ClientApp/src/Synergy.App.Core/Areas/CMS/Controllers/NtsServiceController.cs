using Synergy.App.Business;
using Synergy.App.Repository;
using Synergy.App.Common;
using Synergy.App.ViewModel;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
//using Syncfusion.EJ2.Diagrams;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.DataModel;
using Newtonsoft.Json;
using System.IO;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using System.Text;
using Synergy.App.WebUtility;
using Syncfusion.EJ2.Diagrams;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class NtsServiceController : ApplicationController
    {
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IModuleBusiness _moduleBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IUserContext _userContext;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        private readonly IPageBusiness _pageBusiness;
        private readonly IWebHelper _webApi;
        private readonly IComponentResultBusiness _componentResultBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IRepositoryQueryBase<NtsService> _queryRepo;
        private readonly IPortalBusiness _portalBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        ICompanySettingBusiness _companySettingBusiness;

        public NtsServiceController(IServiceBusiness serviceBusiness, ICmsBusiness cmsBusiness, IUserContext userContext, ITemplateBusiness templateBusiness
            , IPageBusiness pageBusiness
            , IWebHelper webApi,
            IComponentResultBusiness componentResultBusiness,
            IModuleBusiness moduleBusiness,
            ILOVBusiness lovBusiness,
            ITemplateCategoryBusiness templateCategoryBusiness,
            IUserBusiness userBusiness, ICompanySettingBusiness companySettingBusiness,
             ITaskBusiness taskBusiness, INoteBusiness noteBusiness,
             IRepositoryQueryBase<NtsService> queryRepo, IPortalBusiness portalBusiness,
             ITableMetadataBusiness tableMetadataBusiness, IHRCoreBusiness hrCoreBusiness
            )
        {
            _serviceBusiness = serviceBusiness;
            _cmsBusiness = cmsBusiness;
            _userContext = userContext;
            _templateBusiness = templateBusiness;
            _templateCategoryBusiness = templateCategoryBusiness;
            _pageBusiness = pageBusiness;
            _webApi = webApi;
            _lovBusiness = lovBusiness;
            _moduleBusiness = moduleBusiness;
            _componentResultBusiness = componentResultBusiness;
            _userBusiness = userBusiness;
            _taskBusiness = taskBusiness;
            _noteBusiness = noteBusiness;
            _queryRepo = queryRepo;
            _portalBusiness = portalBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _hrCoreBusiness = hrCoreBusiness;
            _companySettingBusiness = companySettingBusiness;
        }
        public async Task<IActionResult> Index(string categoryCode, string templateCode, string pageId, string moduleCode, string pageName, string portalId, string layout)
        {
            var model = new NtsServiceIndexPageViewModel();
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
            await _serviceBusiness.GetNtsServiceIndexPageCount(model);
            ViewBag.Layout = layout;
            return View(model);

        }
        public async Task<IActionResult> LoadNtsServiceIndexPageGrid(NtsActiveUserTypeEnum ownerType, string serviceStatusCode, string categoryCode, string templateCode, string moduleCode)
        {
            var data = await _serviceBusiness.GetNtsServiceIndexPageGridData(null, ownerType, serviceStatusCode, categoryCode, templateCode, moduleCode);
            return Json(data);
        }
        public async Task<IActionResult> NtsServicePage(string templateid, string pageId, string dataAction, string requestSource, string portalId, string pageName, LayoutModeEnum lo, string cbm, string serviceId = null)
        {

            var model = new ServiceTemplateViewModel();
            model.ActiveUserId = _userContext.UserId;

            if (templateid.IsNotNullAndNotEmpty())
            {
                model.TemplateId = templateid;
            }
            if (serviceId.IsNullOrEmpty())
            {
                model.DataAction = DataActionEnum.Create;
            }
            else
            {
                model.ServiceId = serviceId;
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
            var newmodel = await _serviceBusiness.GetServiceDetails(model);
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
            if (requestSource.IsNotNullAndNotEmpty())
            {
                if (requestSource.ToEnum<RequestSourceEnum>() == RequestSourceEnum.Versioning)
                {
                    newmodel.IsVersioning = true;
                    newmodel.VersionNo += 1;
                }
            }
            newmodel.LayoutMode = lo;
            newmodel.PopupCallbackMethod = cbm;
            if (newmodel.LayoutMode == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            return View(newmodel);
        }
        public async Task<IActionResult> GetFormIoJson(string templateId, string pageId, string serviceId)
        {

            var page = await _pageBusiness.GetSingleById(pageId);
            var viewName = page.PageType;
            if (templateId.IsNotNullAndNotEmpty())
            {
                page.Template = await _templateBusiness.GetSingleById(templateId);
                page.Template.TableMetadataId = page.Template.UdfTableMetadataId;
            }
            if (serviceId.IsNotNullAndNotEmpty())
            {
                var service = await _serviceBusiness.GetSingleById(serviceId);
                if (service != null)
                {
                    page.RecordId = service.UdfNoteId;
                    page.PageType = TemplateTypeEnum.Note;
                }
            }
            var data = await GetServiceModel(page, viewName);
            if (page.Template.Json.IsNotNullAndNotEmpty())
            {
                page.Template.Json = _webApi.AddHost(page.Template.Json);
            }
            return Json(new { uiJson = page.Template.Json, dataJson = data });

        }
        private async Task<string> GetServiceModel(PageViewModel page, TemplateTypeEnum viewName)
        {
            if (page.RecordId.IsNullOrEmpty())
            {
                return "{}";
            }
            var data = await _cmsBusiness.GetDataById(viewName, page, page.RecordId);
            if (data == null)
            {
                return "{}";
            }
            return data.ToJson();
        }
        [HttpPost]
        public async Task<IActionResult> ManageNtsServicePage(ServiceTemplateViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var create = await _serviceBusiness.ManageService(model);
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
                            recordId = create.Item.ServiceId,
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
                var edit = await _serviceBusiness.ManageService(model);
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
                            recordId = edit.Item.ServiceId,
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

        public IActionResult SelectServiceTemplate(string templateCode, string categoryCode, string userId, string moduleCodes, string prms, string cbm, string templateIds, string categoryIds, bool allBooks = false, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, ServiceTypeEnum serviceType = ServiceTypeEnum.StandardService, string portalNames = null)
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
            model.AllBooks = allBooks;
            model.PortalNames = portalNames;
            model.ServiceType = serviceType;
            return View(model);
        }
        public async Task<IActionResult> ReadServiceTemplate(string templateCode = null, string categoryCode = null, string moduleCodes = null, string templateIds = null, string categoryIds = null, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks = false, string portalNames = null, ServiceTypeEnum serviceType = ServiceTypeEnum.StandardService)
        {
            var result = await _templateBusiness.GetTemplateServiceList(templateCode, categoryCode, moduleCodes, templateIds, categoryIds, categoryType, allBooks, portalNames, serviceType);
            var st = DateTime.Now.Millisecond;
            result.ForEach(x => x.ClassName = $"border-{(st++ % 8) + 1}");
            result = result.OrderBy(x => x.SequenceOrder).ToList();
            var j = Json(result);
            return j;
        }

        public async Task<IActionResult> ReadServiceTemplateCategory(string categoryCode, string templateCode, string moduleCodes, string categoryIds, string templateIds, TemplateCategoryTypeEnum templateCategoryType, bool allBooks = false, string portalNames = null)
        {
            //var result = new List<TemplateCategoryViewModel>();
            //if (categoryCode.IsNotNullAndNotEmpty())
            //{
            //    var code = categoryCode.Split(",");
            //    result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Service && code.Any(y => y == x.Code));
            //}
            //else if (categoryIds.IsNotNullAndNotEmpty())
            //{
            //    var Ids = categoryIds.Split(",");
            //    result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Service && Ids.Any(y => y == x.Id));
            //}
            //else if (moduleCodes.IsNotNullAndNotEmpty())
            //{
            //    var modules = moduleCodes.Split(",");
            //    result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Service && modules.Any(p => p == x.Module.Code), y => y.Module);
            //}
            //else
            //{
            //    result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Service && x.TemplateCategoryType == TemplateCategoryTypeEnum.Standard);
            //}

            var result = await _templateCategoryBusiness.GetCategoryList(templateCode, categoryCode, moduleCodes, categoryIds, templateIds, TemplateTypeEnum.Service, templateCategoryType, allBooks, portalNames);
            var st = DateTime.Now.Millisecond;
            result = result.GroupBy(x => x.Code)
            .Select(y => new TemplateCategoryViewModel
            {
                Id = y.Max(x => x.Id),
                Code = y.Key,
                Name = y.Max(x => x.Name),
                IconFileId = y.Max(x => x.IconFileId),
                ClassName = $"border-{(st++ % 8) + 1}"
            }).ToList();
            var j = Json(result);
            return j;
        }

        public async Task<IActionResult> ServiceTemplateTiles(string templateCode, string categoryCode, string userId, string moduleCodes, string prms, string cbm, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalId = null)
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
            if (portalId.IsNotNullAndNotEmpty())
            {

                var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(portalId);
                if (portal?.Name == "EGovCustomer")
                {
                    var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
                    ViewBag.SmartCityUrl = cs.Value;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ServiceHome(string userId, string serviceStatus, string mode, LayoutModeEnum? lo, string layoutMode = null, string mobileView = "N", string moduleId = null, string rs = null, string serviceId = null, string moduleCodes = null, string categoryCodes = null, string templateCodes = null, DateTime? date = null, string portalNames = null)
        {
            //serviceId = "b62dbd53-daea-4c9b-a320-7ffe823d62f6";
            var model = new ServiceSearchViewModel { Operation = DataOperation.Read, ServiceStatus = serviceStatus, Mode = mode, ModuleId = moduleId };
            if (!userId.IsNotNull())
            {
                model.UserId = _userContext.UserId;
            }
            if (serviceId.IsNotNullAndNotEmpty())
            {
                model.ServiceId = serviceId;
                //var serviceresult = await _serviceBusiness.GetSearchResult(new ServiceSearchViewModel {UserId= _userContext.UserId });
                var serviceresult = await _serviceBusiness.GetSingleById(serviceId);
                if (serviceresult != null)
                {
                    //ViewBag.ServiceStatus = serviceresult.ServiceStatusCode;
                    ViewBag.TemplateMasterCode = serviceresult.TemplateCode;
                }
            }
            ViewBag.Msg = "Service";
            if (serviceStatus.IsNotNull())
            {
                var val = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_SERVICE_STATUS" && x.Code == serviceStatus);
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
                else if (mode == "REQ_BY")
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

            if (serviceStatus == "SERVICE_STATUS_OVERDUE")
            {
                if (date.IsNotNull())
                {
                    model.DueDate = date.Value.AddDays(-1);
                }

            }
            else if (serviceStatus == "SERVICE_STATUS_INPROGRESS")
            {
                if (date.IsNotNull())
                {
                    model.StartDate = date.Value;
                }

            }
            else if (serviceStatus == "SERVICE_STATUS_CLOSE")
            {
                if (date.IsNotNull())
                {
                    model.ClosedDate = date.Value;
                }

            }
            else if (serviceStatus == "SERVICE_STATUS_NOTSTARTED")
            {
                if (date.IsNotNull())
                {
                    model.StartDate = date.Value;
                }

            }
            else if (serviceStatus == "REMINDER")
            {
                if (date.IsNotNull())
                {
                    model.ReminderDate = date.Value;
                }

                model.ServiceStatus = null;
            }
            model.ModuleCode = moduleCodes;
            model.TemplateCategoryCode = categoryCodes;
            model.TemplateMasterCode = templateCodes;
            model.RequestSource = rs;
            model.PortalNames = portalNames;
            return View(model);
        }
        public async Task<IActionResult> ReadServiceHomeData(ServiceSearchViewModel search = null)
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
            var result = await _serviceBusiness.GetSearchResult(search);
            if (search?.Text == "Today")
            {
                var res = result.Where(x => x.DueDate <= DateTime.Now && x.ServiceStatusCode != "SERVICE_STATUS_COMPLETE" && x.ServiceStatusCode != "SERVICE_STATUS_CANCEL" && x.ServiceStatusCode != "SERVICE_STATUS_DRAFT");
                return Json(res);
            }
            else if (search?.Text == "Week")
            {
                var res = result.Where(x => (x.DueDate <= DateTime.Now.AddDays(7) && DateTime.Now <= x.DueDate && x.ServiceStatusCode != "SERVICE_STATUS_COMPLETE" && x.ServiceStatusCode != "SERVICE_STATUS_CANCEL" && x.ServiceStatusCode != "SERVICE_STATUS_DRAFT")).ToList();
                return Json(res);
            }

            var j = Json(result.OrderByDescending(x => x.LastUpdatedDate));
            return j;
        }
        [HttpPost]
        public async Task<ActionResult> DeleteService(string serviceId)
        {
            await _serviceBusiness.Delete(serviceId);
            return Json(new { success = true });
        }

        public async Task<IActionResult> ServiceHomeDashboard()
        {
            ProjectDashboardViewModel model = new ProjectDashboardViewModel();
            return View(model);
        }
        public async Task<IActionResult> ServiceDashboard(string pageId)
        {
            var page = await _cmsBusiness.GetSingleById<PageViewModel, Page>(pageId);
            if (page != null)
            {
                ViewBag.ServiceDashboardTitle = page.Title.Coalesce(page.Name);
            }
            else
            {
                ViewBag.ServiceDashboardTitle = "Service dashbaord";
            }
            var model = new ProjectDashboardViewModel();
            return View(model);
        }
        public async Task<ActionResult> GetServiceChartByStatus()
        {
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            search.UserId = _userContext.UserId;
            var viewModel = await _serviceBusiness.GetSearchResult(search);
            var list1 = viewModel.GroupBy(x => x.ServiceStatusCode).Select(group => new { Value = group.Count(), Type = group.Select(x => x.ServiceStatusName).FirstOrDefault(), Id = group.Select(x => x.ServiceStatusId).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return Json(list);
        }
        public async Task<ActionResult> GetExternalServiceChartStatus()
        {
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            search.UserId = _userContext.UserId;
            var viewModel = await _serviceBusiness.GetExternalServiceChartByStatus();

            return Json(viewModel);
        }
        public async Task<ActionResult> GetExternalServiceChartByStatus()
        {
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            search.UserId = _userContext.UserId;
            var viewModel = await _serviceBusiness.GetExternalServiceChartByStatus();
            var newlist = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                ItemValueSeries = viewModel.Select(x => x.Value).ToList(),
                TemplateCode = viewModel.Select(x => x.TemplateCode).FirstOrDefault()
            };
            return Json(newlist);
            //return Json(viewModel);
        }
        public async Task<ActionResult> GetInternalServiceChartByStatus()
        {
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            search.UserId = _userContext.UserId;
            var viewModel = await _serviceBusiness.GetInternalServiceChartByStatus();
            var newlist = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                ItemValueSeries = viewModel.Select(x => x.Value).ToList(),
                Code = viewModel.Select(x => x.Code).FirstOrDefault(),
                ItemStatusColor = viewModel.Select(x => x.StatusColor).ToList()
            };
            return Json(newlist);
            //return Json(viewModel);
        }

        public async Task<ActionResult> GetInternalDashboardChartByStatus(string categoryCodes = null, string templateCodes = null)
        {
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            search.TemplateCategoryCode = categoryCodes;
            search.TemplateMasterCode = templateCodes;
            search.UserId = _userContext.UserId;
            var viewModel = await _serviceBusiness.GetInternalDashboardChartByStatus(search);
            var newlist = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                ItemValueSeries = viewModel.Select(x => x.Value).ToList(),
                ItemStatusColor = viewModel.Select(x => x.StatusColor).ToList()
            };
            return Json(newlist);
            //  return Json(viewModel);
        }

        public async Task<ActionResult> GetInternalDashboardTaskChart(string categoryCodes = null, string templateCodes = null)
        {
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            search.TemplateCategoryCode = categoryCodes;
            search.TemplateMasterCode = templateCodes;
            search.UserId = _userContext.UserId;
            var viewModel = await _serviceBusiness.GetInternalDashboardTaskChart(search);
            var newlist = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                ItemValueSeries = viewModel.Select(x => x.Value).ToList(),
                ItemStatusColor = viewModel.Select(x => x.StatusColor).ToList(),
            };
            return Json(newlist);
            //return Json(viewModel);
        }

        public async Task<ActionResult> GetExternalServiceSLA(DateTime? StartDate, DateTime? DueDate)
        {
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            if (StartDate != null && DueDate != null)
            {
                search.UserId = _userContext.UserId;
                search.StartDate = StartDate;
                search.DueDate = DueDate;
                var viewModel = await _serviceBusiness.GetExternalServiceSLA(search);

                var newlist = new ProjectDashboardChartViewModel
                {
                    ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                    LineChartValueSeries = viewModel.Select(x => x.Days).ToList(),
                    LineChartValueSeries1 = viewModel.Select(x => x.ActualSLA).ToList(),
                };

                return Json(newlist);
            }
            else { return Json(""); }
        }
        //public async Task<ActionResult> GetExternalServiceSLA(ServiceSearchViewModel search = null)
        //{
        //    if (search.StartDate != null && search.DueDate != null)
        //    {
        //        search.UserId = _userContext.UserId;
        //        var viewModel = await _serviceBusiness.GetExternalServiceSLA(search);
        //        return Json(viewModel);
        //    }
        //    else { return Json(""); }
        //}

        public async Task<ActionResult> GetRequestSLA(DateTime? StartDate, DateTime? DueDate, string TemplateCategoryCode, string TemplateMasterCode)
        {
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            if (StartDate != null && DueDate != null)
            {
                search.UserId = _userContext.UserId;
                search.StartDate = StartDate;
                search.DueDate = DueDate;
                search.TemplateCategoryCode = TemplateCategoryCode;
                search.TemplateMasterCode = TemplateMasterCode;
                var viewModel = await _serviceBusiness.GetRequestSLA(search);

                var newlist = new ProjectDashboardChartViewModel
                {
                    ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                    LineChartValueSeries = viewModel.Select(x => x.Days).ToList(),
                    LineChartValueSeries1 = viewModel.Select(x => x.ActualSLA).ToList(),
                };

                return Json(newlist);
            }
            else { return Json(""); }
        }

        public async Task<IActionResult> GetSEBIServiceList(ServiceSearchViewModel search = null)
        {

            if (!search.UserId.IsNotNull())
            {
                search.UserId = _userContext.UserId;
            }
            var result = await _serviceBusiness.GetSEBIServiceList();

            if (search.ServiceStatusIds.IsNotNull() && search.ServiceStatusIds.Count() > 0 && !search.ServiceStatusIds.Contains(null))
            {
                var status = string.Join(",", search.ServiceStatusIds);
                result = result.Where(x => status.Contains(x.ServiceStatusName)).ToList();

            }
            if (search.StatusIds.IsNotNull() && search.StatusIds.Count() > 0 && !search.StatusIds.Contains(null))
            {
                var ServiceStatusId = string.Join(",", search.StatusIds);
                result = result.Where(x => ServiceStatusId.Contains(x.ServiceStatusName.ToString())).ToList();

            }
            var j = Json(result);
            return j;
        }


        public async Task<IActionResult> GetInternalDashboardServiceList([DataSourceRequest] DataSourceRequest request, ServiceSearchViewModel search = null, string categoryCodes = null, string templateCodes = null, string ServiceStatusIds = null)
        {

            if (!search.UserId.IsNotNull())
            {
                search.UserId = _userContext.UserId;
            }
            search.TemplateMasterCode = templateCodes;
            search.TemplateCategoryCode = categoryCodes;
            var result = await _serviceBusiness.GetInternalDashboardServiceList(search);

            search.ServiceStatusIds = ServiceStatusIds.IsNotNull() ? ServiceStatusIds.Split(',').ToList() : null;

            if (search.ServiceStatusIds.IsNotNull() && search.ServiceStatusIds.Count() > 0)
            {
                var status = string.Join(",", search.ServiceStatusIds);
                result = result.Where(x => status.Contains(x.ServiceStatusName)).ToList();

            }
            if (search.StatusIds.IsNotNull() && search.StatusIds.Count() > 0)
            {
                var ServiceStatusId = string.Join(",", search.StatusIds);
                result = result.Where(x => ServiceStatusId.Contains(x.ServiceStatusName.ToString())).ToList();

            }
            var j = Json(result);
            //var j = Json(result.ToDataSourceResult(request));
            return j;
        }

        public async Task<ActionResult> GetServiceChartByUserType()
        {
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            search.UserId = _userContext.UserId;
            var viewModel = await _serviceBusiness.GetSearchResult(search);
            var list1 = viewModel.GroupBy(x => x.TemplateUserType).Select(group => new { Value = group.Count(), Type = group.Select(x => x.TemplateUserType).FirstOrDefault(), Id = group.Select(x => Convert.ToInt32(x.TemplateUserType)).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type.ToString(), Value = x.Value, Id = x.Id.ToString() }).ToList();
            return Json(list);
        }
        public async Task<IActionResult> ReadServiceDashBoardGridData(ServiceSearchViewModel search = null)
        {

            if (!search.UserId.IsNotNull())
            {
                search.UserId = _userContext.UserId;
            }
            var result = await _serviceBusiness.GetSearchResult(search);
            if (search?.Text == "Today")
            {
                var res = result.Where(x => x.DueDate <= DateTime.Now && x.ServiceStatusCode != "SERVICE_STATUS_COMPLETE" && x.ServiceStatusCode != "SERVICE_STATUS_CANCEL" && x.ServiceStatusCode != "SERVICE_STATUS_DRAFT");
                return Json(res);
            }
            else if (search?.Text == "Week")
            {
                var res = result.Where(x => (x.DueDate <= DateTime.Now.AddDays(7) && DateTime.Now <= x.DueDate && x.ServiceStatusCode != "SERVICE_STATUS_COMPLETE" && x.ServiceStatusCode != "SERVICE_STATUS_CANCEL" && x.ServiceStatusCode != "SERVICE_STATUS_DRAFT")).ToList();
                return Json(res);
            }
            if (search.ServiceStatusIds.IsNotNull() && search.ServiceStatusIds.Count() > 0)
            {
                var status = string.Join(",", search.ServiceStatusIds);
                result = result.Where(x => status.Contains(x.ServiceStatusId)).ToList();
                // return Json(res.ToDataSourceResult(request));
            }
            if (search.UserType.IsNotNull() && search.UserType.Count() > 0)
            {
                var UserType = string.Join(",", search.UserType);
                result = result.Where(x => UserType.Contains(x.TemplateUserType.ToString())).ToList();
                // return Json(res.ToDataSourceResult(request));
            }
            var j = Json(result);
            return j;
        }

        public async Task<ActionResult> ReadDatewiseServiceSLA(ServiceSearchViewModel search = null)
        {
            if (search.StartDate != null && search.DueDate != null)
            {
                search.UserId = _userContext.UserId;
                var viewModel = await _serviceBusiness.GetDatewiseServiceSLA(search);
                return Json(viewModel);
            }
            else { return Json(""); }
        }
        public async Task<ActionResult> GetServiceSharedUsersIdNameList(string serviceId)
        {
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            list = await _serviceBusiness.GetSharedList(serviceId);
            return Json(list);
        }
        public async Task<ActionResult> GetServiceByUser(string userId)
        {
            IList<ServiceViewModel> model = new List<ServiceViewModel>();
            if (userId != null)
            {
                model = await _serviceBusiness.GetServiceByUser(userId);
            }
            var json = Json(model);
            return json;
        }
        public ActionResult ServiceDiagramImage(string id, string templateCode, bool isTemplate = false)
        {
            ViewBag.TemplateCode = templateCode;
            ViewBag.Id = id;
            return View();
        }

        //public async Task<ActionResult> ServiceDiagram(string id, string templateCode, bool isTemplate = false)
        //{
        //    return View();
        //}

        public async Task<ActionResult> ServiceDiagram(string id, string templateCode, bool isTemplate = false)
        {
            var templateId = "";
            if (!isTemplate)
            {
                var serviceDetails = await _serviceBusiness.GetSingleById(id);
                if (serviceDetails.IsNotNull())
                {
                    ViewBag.TemplateCode = serviceDetails.TemplateCode;
                    templateId = serviceDetails.TemplateId;
                }
            }
            else
            {
                if (templateCode.IsNotNullAndNotEmpty())
                {
                    var templateDetails = await _templateBusiness.GetSingle(x => x.Code == templateCode);
                    if (templateDetails.IsNotNull())
                    {
                        ViewBag.TemplateCode = templateDetails.Code;
                        ViewBag.isTemplate = true;
                        id = templateDetails.Id;
                        templateId = templateDetails.Id;
                    }
                }
                else
                {
                    var serviceDetails = await _templateBusiness.GetSingleById(id);
                    if (serviceDetails.IsNotNull())
                    {
                        ViewBag.TemplateCode = serviceDetails.Code;
                        ViewBag.isTemplate = true;
                        id = serviceDetails.Id;
                        templateId = serviceDetails.Id;
                    }
                }
            }
            ViewBag.Id = id;
            var nodes = new List<DiagramNode>();
            var connectors = new List<DiagramConnector>();
            await GenerateServiceWorkFlowDiagramNodesView(nodes, id, isTemplate, connectors);
            ViewBag.nodes = nodes;
            ViewBag.connectors = connectors;
            ViewBag.getNodeDefaults = "getNodeDefaults";
            ViewBag.getConnectorDefaults = "getConnectorDefaults";
            ViewBag.getContent = "getContent";
            ViewBag.ViewTemplateId = templateId;
            return View();
        }

        public async Task<IActionResult> ServiceDiagramResult(string id, bool isTemplate = false)
        {
            //var swimlane = new SwimlaneViewModel();
            //var nodes = new List<SwimlaneNodeViewModel>();
            //var lanes = new List<SwimlaneLaneViewModel>();

            //var serviceDetails = new ServiceViewModel();
            //var result = new List<WorkflowViewModel>();
            //if (!isTemplate)
            //{
            //    serviceDetails = await _serviceBusiness.GetSingleById(id);
            //    if (serviceDetails.IsNotNull())
            //    {
            //        var status = await _lovBusiness.GetSingleById(serviceDetails.ServiceStatusId);
            //        if (status.IsNotNull())
            //        {
            //            if (status.Code == "SERVICE_STATUS_DRAFT")
            //            {
            //                isTemplate = true;
            //                result = await _templateBusiness.GetWorkFlowDiagramDetailsByTemplate(serviceDetails.TemplateId);
            //            }
            //            else
            //            {
            //                result = await _templateBusiness.GetWorkFlowDiagramDetails(id);
            //            }
            //        }
            //        else
            //        {
            //            result = await _templateBusiness.GetWorkFlowDiagramDetails(id);
            //        }
            //    }
            //}
            //else
            //{
            //    result = await _templateBusiness.GetWorkFlowDiagramDetailsByTemplate(id);
            //}

            //var laneslist = result.Select(x => x.StageId).Distinct().ToList();
            //if (laneslist[0] == null)
            //{
            //    laneslist = new List<string>();
            //    laneslist.Add("Lane");
            //    result.Select(c => { c.StageId = "Lane"; c.StageName = "Lane"; return c; }).ToList();
            //}
            //var Phaselist = result.Select(x => x.StepId).Distinct().ToList();
            //if (Phaselist[0] == null)
            //{
            //    Phaselist = new List<string>();
            //    Phaselist.Add("Phase");
            //    result.Select(c => { c.StepId = "Phase"; c.StepName = "Phase"; return c; }).ToList();

            //}
            //var serviceName = "";
            //var pchildren = new List<SwimlaneChildren>();
            //int count = 0;
            //foreach (var l in laneslist)
            //{

            //    var top = 60;
            //    var steps = result.Where(x => x.StageId == l && x.Type == "Task");
            //    var laneDetails = result.Where(x => x.StageId == l).FirstOrDefault();
            //    var children = new List<SwimlaneChildren>();
            //    foreach (var st in steps)
            //    {
            //        var _bgcolorNote = "";
            //        if (st.StatusName == "Draft")
            //        {
            //            _bgcolorNote = "#5bc0de";
            //        }
            //        else if (st.StatusName == "In Progress")
            //        {
            //            _bgcolorNote = "#f0ad4e";
            //        }
            //        else if (st.StatusName == "Completed")
            //        {
            //            _bgcolorNote = "#5cb85c";
            //        }
            //        else if (st.StatusName == "Overdue")
            //        {
            //            _bgcolorNote = "#d9534f";
            //        }
            //        else if (st.StatusName == "Cancel")
            //        {
            //            _bgcolorNote = "#999999";
            //        }
            //        var content = st.Subject == "" || st.Subject == null ? st.TemplateName : st.Subject;
            //        var taskContent = "";
            //        if (st.Id.IsNotNull())
            //        {
            //            taskContent = "<div style='color:white;background:" + _bgcolorNote + ";' class='node-children'>" + content + "<br/>" + st.AssignedToUserName + "<br/>" + "<a style='font-size:12px;cursor:pointer;' onclick =onViewTask('" + st.Id + "," + st.TemplateCode + "')>View</a>"
            //                        + "<br/>" + st.DueDate + "<br/></div>";
            //            if (isTemplate)
            //            {
            //                _bgcolorNote = "#5bc0de";
            //                taskContent = "<div style='background:#5bc0de;color:white' class='node-children'>" + content + "</div>";
            //            }
            //        }
            //        else
            //        {
            //            _bgcolorNote = "#5bc0de";
            //            taskContent = "<div style='background:#5bc0de;color:white' class='node-children'>" + content + "</div>";
            //        }
            //        if (count == 0)
            //        {

            //            var service = result.Where(x => x.StageId == l && x.Type == "Service").FirstOrDefault();
            //            if (service.IsNotNull())
            //            {
            //                serviceName = service.Subject;
            //                var serviceContent = "";

            //                serviceContent = "<div style='color:white;background:" + _bgcolorNote + ";' class='node-children'>" + service.Subject + "<br/>" + service.AssignedToUserName + "<br/>" + "<a style='font-size:12px;cursor:pointer;' onclick =onViewService('" + service.Id + "," + service.TemplateCode + "')>View</a>"
            //                        + "<br/>" + service.DueDate + "<br/></div>";

            //                if (isTemplate)
            //                {
            //                    _bgcolorNote = "#5bc0de";
            //                    serviceContent = "<div style='background:#5bc0de;color:white' class='node-children'>" + serviceName + "</div>";
            //                }
            //                var serv = new SwimlaneChildren
            //                {
            //                    id = "node_" + service.Id,
            //                    annotations = new List<SwimlaneAnnotation>{
            //                           new SwimlaneAnnotation {
            //                                content= "",
            //                                style= new SwimlaneStyle { fontSize= 11 , fill = "transparent"}
            //                            }
            //                        },
            //                    margin = new SwimlaneMargin { left = 60, top = top },
            //                    height = 100,
            //                    width = 150,
            //                    shape = new SwimlaneChildrenShape { type = "HTML", content = serviceContent }
            //                };
            //                children.Add(serv);
            //                top = top + 150;
            //                count++;
            //            }
            //        }

            //        var taskId = st.Id != null ? st.Id : st.TemplateId;
            //        var c = new SwimlaneChildren
            //        {
            //            id = "node_" + taskId,
            //            annotations = new List<SwimlaneAnnotation>{
            //                           new SwimlaneAnnotation {
            //                                content= "",
            //                                style= new SwimlaneStyle { fontSize= 11 , fill = "transparent"}
            //                            }
            //                        },
            //            margin = new SwimlaneMargin { left = 60, top = top },
            //            height = 100,
            //            width = 150,
            //            shape = new SwimlaneChildrenShape { type = "HTML", content = taskContent }
            //        };
            //        children.Add(c);
            //        top = top + 150;

            //    }
            //    //pchildren.AddRange(children);
            //    lanes.Add(new SwimlaneLaneViewModel
            //    {
            //        id = "stackCanvas12" + l,
            //        header = new SwimlaneHeader
            //        {
            //            annotation = new SwimlaneAnnotation { content = laneDetails.StageName, style = new SwimlaneStyle { fontSize = 11, fill = "transparent" } },
            //            width = 50,
            //            //height = 50,
            //            style = new SwimlaneStyle { fontSize = 11, fill = "white" },
            //        },
            //        height = 200 * children.Count,
            //        children = children
            //    });
            //}


            //var phases = new List<SwimlanePhaseViewModel>();
            //phases.Add(new SwimlanePhaseViewModel
            //{
            //    id = "phase1",
            //    offset = 170,
            //    header = new SwimlaneHeader { annotation = new SwimlaneAnnotation { content = "Phase" } },
            //    children = pchildren
            //});

            //nodes.Add(new SwimlaneNodeViewModel
            //{
            //    id = "swimlane",
            //    shape = new SwimlaneShape
            //    {
            //        type = "SwimLane",
            //        orientation = "Horizontal",
            //        header = new SwimlaneHeader
            //        {
            //            annotation = new SwimlaneAnnotation
            //            {
            //                content = serviceName,
            //                style = new SwimlaneStyle { fill = "transparent", fontSize = 11 },
            //            },
            //            height = 50,
            //            style = new SwimlaneStyle { fontSize = 11, fill = "transparent" }
            //        },
            //        lanes = lanes,
            //        phases = phases,
            //        phaseSize = 20,
            //    },
            //    height = 100,
            //    width = 650,
            //});
            //swimlane.nodes = nodes;
            //var index = 1;

            //var connectors = new List<SwimlaneConnectorViewModel>();
            //foreach (var i in result.Where(x => x.Type == "Task"))
            //{
            //    var src = result.Where(x => x.ComponentId == i.ParentId).FirstOrDefault();
            //    var taskIdst = i.Id != null ? i.Id : i.TemplateId;

            //    if (src.IsNotNull())
            //    {
            //        var taskIds = src.Id != null ? src.Id : src.TemplateId;

            //        var sourceId = "node_" + taskIds;
            //        var targetId = "node_" + taskIdst;

            //        swimlane.connectors.Add(new SwimlaneConnectorViewModel
            //        {
            //            id = "connectors" + index.ToString(),
            //            sourceID = sourceId,
            //            targetID = targetId
            //        });
            //        index++;
            //    }
            //    else
            //    {
            //        var serviceD = result.Where(x => x.Type == "Service").FirstOrDefault();
            //        var sourceId = "node_" + serviceD.Id;
            //        var targetId = "node_" + taskIdst;
            //        swimlane.connectors.Add(new SwimlaneConnectorViewModel
            //        {
            //            id = "connectors" + index.ToString(),
            //            sourceID = sourceId,
            //            targetID = targetId
            //        });
            //        index++;
            //    }
            ////}
            ///
            //var nodes = new List<DiagramNode>();
            //var connectors = new List<DiagramConnector>();
            //var result = await GenerateServiceWorkFlowDiagramNodesView(nodes, id, isTemplate, connectors);
            return Json(null);
        }

        public async Task<IActionResult> GetUserList(string id, NtsTypeEnum type)
        {
            var list = await _componentResultBusiness.GetUserListOnNtsBasis(id, type);
            return Json(list);
        }
        public async Task<JsonResult> GetModuleTreeList(string id, string moduleCodes)
        {

            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {

                list.Add(new TreeViewViewModel
                {
                    id = "Root",
                    Name = "All",
                    DisplayName = "All",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "Root",
                    children = true,
                    text = "All",
                    parent = "#",
                    a_attr = new { data_id = "Root", data_name = "All", data_type = "Root" },

                });
            }
            if (id == "Root")
            {
                var moduleslist = await _moduleBusiness.GetList(x => x.PortalId == _userContext.PortalId);

                if (moduleCodes.IsNotNullAndNotEmpty())
                {
                    var modules = moduleCodes.Split(",");
                    moduleslist = moduleslist.Where(x => x.Code != null && modules.Any(y => y == x.Code)).ToList();

                }
                list = moduleslist.Select(e => new TreeViewViewModel()
                {
                    id = e.Id,
                    Name = e.Name,
                    DisplayName = e.Name,
                    ParentId = id,
                    hasChildren = false,
                    expanded = false,
                    Type = "Child",
                    children = true,
                    text = e.Name,
                    parent = id,
                    a_attr = new { data_id = e.Id, data_name = e.Name, data_type = "Child" },

                })
                   .ToList();

            }


            return Json(list.ToList());
        }

        public async Task<object> GetModuleFancyTreeList(string id, string moduleCodes)
        {

            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {

                list.Add(new TreeViewViewModel
                {
                    id = "Root",
                    Name = "All",
                    DisplayName = "All",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "Root",
                    children = true,
                    text = "All",
                    parent = "#",
                    a_attr = new { data_id = "Root", data_name = "All", data_type = "Root" },

                });
            }
            if (id == "Root")
            {
                var moduleslist = await _moduleBusiness.GetList(x => x.PortalId == _userContext.PortalId);

                if (moduleCodes.IsNotNullAndNotEmpty())
                {
                    var modules = moduleCodes.Split(",");
                    moduleslist = moduleslist.Where(x => x.Code != null && modules.Any(y => y == x.Code)).ToList();

                }
                list = moduleslist.Select(e => new TreeViewViewModel()
                {
                    id = e.Id,
                    Name = e.Name,
                    DisplayName = e.Name,
                    ParentId = id,
                    hasChildren = false,
                    expanded = false,
                    Type = "Child",
                    children = true,
                    text = e.Name,
                    parent = id,
                    a_attr = new { data_id = e.Id, data_name = e.Name, data_type = "Child" },

                })
                   .ToList();

            }
            var newList = new List<FileExplorerViewModel>();
            newList.AddRange(list.ToList().Select(x => new FileExplorerViewModel { key = x.id, title = x.Name, lazy = true }));
            var json = JsonConvert.SerializeObject(newList);
            return json;
        }

        public async Task<IActionResult> WorkPerformance(string moduleCodes)
        {
            ServiceViewModel model = new ServiceViewModel();
            model.ModuleCode = moduleCodes;
            return View(model);
        }

        public async Task<ActionResult> ReadWorkPerformanceCalendarData([DataSourceRequest] DataSourceRequest request, string moduleCodes, DateTime? fromDate = null, DateTime? toDate = null)
        {
            //if (dateRange.IsNotNull() && dateRange == DateTypeEnum.Today)
            //{
            //    fromDate = System.DateTime.Now;
            //    toDate = fromDate.Value.AddDays(1);
            //}            

            var list = await _serviceBusiness.GetWorkPerformanceCount(_userContext.UserId, moduleCodes, fromDate, toDate);
            return Json(list);
            // return Json(list.ToDataSourceResult(request));


        }
        public async Task<IActionResult> ServiceCustomIndex(string pageId, string moduleCodes, string templateCodes, string categoryCodes, bool isDisableCreate = false, bool showAllOwnersService = false, string portalNames = null)
        {
            var page = await _pageBusiness.GetSingleById(pageId);
            ViewBag.UserId = _userContext.UserId;
            ViewBag.PortalNames = portalNames;
            if (page != null)
            {
                var model = await _pageBusiness.GetSingle<CustomIndexPageTemplateViewModel, CustomIndexPageTemplate>(x => x.TemplateId == page.TemplateId);
                if (model != null)
                {
                    model.ModuleCodes = moduleCodes;
                    model.TemplateCodes = templateCodes;
                    model.CategoryCodes = categoryCodes;
                    model.IsDisableCreate = isDisableCreate;
                    model.ShowAllOwnersService = showAllOwnersService;
                    model.Page = page;
                    model.SelectedTableRows = await _cmsBusiness.GetList<CustomIndexPageColumnViewModel, CustomIndexPageColumn>(x => x.CustomIndexPageTemplateId == model.Id);
                    model.SelectedTableRows = model.SelectedTableRows.OrderBy(x => x.SequenceOrder).ToList();
                }
                return View(model);
            }
            return View(new CustomIndexPageTemplateViewModel
            {
                ModuleCodes = moduleCodes,
                TemplateCodes = templateCodes,
                CategoryCodes = categoryCodes,
                IsDisableCreate = isDisableCreate,
                ShowAllOwnersService = showAllOwnersService
            });
        }
        public async Task<IActionResult> LoadCustomServiceIndexPageGrid(string templateId, bool showAllOwnersService, string moduleCodes, string templateCodes, string categoryCodes)
        {
            var dt = await _serviceBusiness.GetCustomServiceIndexPageGridData(null, templateId, showAllOwnersService, moduleCodes, templateCodes, categoryCodes);
            return Json(dt);
        }
        public ActionResult ServiceList(string moduleCodes, string templateCodes, string categoryCodes, bool isDisableCreate = false, bool showAllOwnersService = false)
        {
            ViewBag.IsDisableCreate = isDisableCreate;
            ViewBag.ShowAllOwnersService = showAllOwnersService;
            ViewBag.UserId = _userContext.UserId;
            var model = new ServiceViewModel { ModuleCode = moduleCodes, TemplateCode = templateCodes, TemplateCategoryCode = categoryCodes };
            return View(model);
        }
        public ActionResult SebiExternalServiceIndex(string moduleCodes, string templateCodes, string categoryCodes, bool isDisableCreate = false, bool showAllOwnersService = false, string tab = null, string mode = null)
        {
            ViewBag.IsDisableCreate = isDisableCreate;
            ViewBag.ShowAllOwnersService = showAllOwnersService;
            ViewBag.UserId = _userContext.UserId;
            ViewBag.Tab = tab;
            ViewBag.Mode = mode;
            var model = new ServiceViewModel { ModuleCode = moduleCodes, TemplateCode = templateCodes, TemplateCategoryCode = categoryCodes };
            return View(model);
        }
        public ActionResult SebiExternalServiceList(string moduleCodes, string templateCodes, string categoryCodes, bool isDisableCreate = false, bool showAllOwnersService = false, string tempCode = null, string tab = null, string mode = null)
        {
            ViewBag.IsDisableCreate = isDisableCreate;
            ViewBag.ShowAllOwnersService = showAllOwnersService;
            ViewBag.UserId = _userContext.UserId;
            ViewBag.TemplateCode = tempCode;
            ViewBag.Tab = tab;
            ViewBag.Mode = mode;
            var model = new ServiceViewModel { ModuleCode = moduleCodes, TemplateCode = templateCodes, TemplateCategoryCode = categoryCodes };
            return View(model);
        }



        public ActionResult ExternalServiceList(string moduleCodes, string templateCodes, string categoryCodes, bool isDisableCreate = false, bool showAllOwnersService = false)
        {
            ViewBag.IsDisableCreate = isDisableCreate;
            ViewBag.ShowAllOwnersService = showAllOwnersService;
            ViewBag.UserId = _userContext.UserId;
            var model = new ServiceViewModel { ModuleCode = moduleCodes, TemplateCode = templateCodes, TemplateCategoryCode = categoryCodes };
            return View(model);
        }
        public ActionResult SEBIServices(string moduleCodes, string templateCodes, string categoryCodes, bool isDisableCreate = false, bool showAllOwnersService = false, string portalNames = null)
        {
            ViewBag.IsDisableCreate = isDisableCreate;
            ViewBag.ShowAllOwnersService = showAllOwnersService;
            ViewBag.UserId = _userContext.UserId;
            ViewBag.PortalNames = portalNames;
            var model = new ServiceViewModel { ModuleCode = moduleCodes, TemplateCode = templateCodes, TemplateCategoryCode = categoryCodes };
            return View(model);
        }
        public async Task<ActionResult> ServiceBookIndex(string pageId, string moduleCodes, string templateCodes, string categoryCodes, bool isDisableCreate = false, bool showAllOwnersService = false)
        {
            ViewBag.IsDisableCreate = isDisableCreate;
            ViewBag.ShowAllOwnersService = showAllOwnersService;
            ViewBag.UserId = _userContext.UserId;
            if (pageId.IsNotNullAndNotEmpty())
            {
                var page = await _pageBusiness.GetSingleById(pageId);
                ViewBag.PageName = page.Title;
            }
            else
            {
                ViewBag.PageName = "";
            }
            var model = new ServiceViewModel { ModuleCode = moduleCodes, TemplateCode = templateCodes, TemplateCategoryCode = categoryCodes };
            return View(model);
        }


        public async Task<IActionResult> ReadServiceBookData([DataSourceRequest] DataSourceRequest request, string moduleCodes, string templateCodes, string categoryCodes, string templatecode = null)
        {
            var result = await _serviceBusiness.ReadServiceBookData(moduleCodes, templateCodes, categoryCodes);


            return Json(result);
            //return Json(result.ToDataSourceResult(request));

        }

        public async Task<object> ReadServiceBookDataTree(string id, string moduleCodes, string templateCodes, string categoryCodes, string templatecode = null)
        {
            var result = await _serviceBusiness.ReadServiceBookData(moduleCodes, templateCodes, categoryCodes);
            if (id.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.ParentServiceId == id).ToList();
            }
            else
            {
                result = result.Where(x => x.ParentServiceId == null).ToList();
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return json;

        }

        public async Task<IActionResult> ReadServiceDataInProgress(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, bool isDisableCreate = false, string mode = null, string templatecode = null)
        {
            if (mode.IsNotNullAndNotEmpty())
            {
                var result = await _serviceBusiness.GetSEBIExternalServiceList(templatecode);

                var j = Json(result.Where(x => x.ServiceStatusCode == "TASK_STATUS_INPROGRESS").OrderByDescending(x => x.CreatedDate));
                return j;
            }
            else
            {
                var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
                if (templateCodes == "DMS_SUPPORT_TICKET")
                {
                    var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").OrderByDescending(x => x.CreatedDate));
                    return j;
                }
                else
                {
                    if (isDisableCreate)
                    {
                        var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS").OrderByDescending(x => x.CreatedDate));
                        return j;
                    }
                    else
                    {
                        var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").OrderByDescending(x => x.CreatedDate));
                        return j;
                    }
                }
            }
            return Json(new ServiceViewModel());

        }

        public async Task<IActionResult> ReadTop10ServiceDataInProgress(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, bool isDisableCreate = false, string mode = null, string templatecode = null)
        {
            if (mode.IsNotNullAndNotEmpty())
            {
                var result = await _serviceBusiness.GetSEBIExternalServiceList(templatecode);

                var j = Json(result.Where(x => x.ServiceStatusCode == "TASK_STATUS_INPROGRESS").OrderByDescending(x => x.CreatedDate).Take(10));
                return j;
            }
            else
            {
                var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
                if (templateCodes == "DMS_SUPPORT_TICKET")
                {
                    var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").OrderByDescending(x => x.CreatedDate));
                    return j;
                }
                else
                {
                    if (isDisableCreate)
                    {
                        var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS").OrderByDescending(x => x.CreatedDate));
                        return j;
                    }
                    else
                    {
                        var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").OrderByDescending(x => x.CreatedDate));
                        return j;
                    }
                }
            }
            return Json(new ServiceViewModel());

        }

        public async Task<IActionResult> ReadSEBIServiceDataInProgress(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, bool isDisableCreate = false, string mode = null, string templatecode = null)
        {
            if (mode.IsNotNullAndNotEmpty())
            {
                var result = await _serviceBusiness.GetSEBIExternalServiceList(templatecode);

                var j = Json(result.Where(x => x.ServiceStatusCode == "TASK_STATUS_INPROGRESS" || x.ServiceStatusCode == "TASK_STATUS_OVERDUE").OrderByDescending(x => x.CreatedDate));
                return j;
            }
            else
            {
                var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
                if (templateCodes == "DMS_SUPPORT_TICKET")
                {
                    var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_DRAFT" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").OrderByDescending(x => x.CreatedDate));
                    return j;
                }
                else
                {
                    if (isDisableCreate)
                    {
                        var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").OrderByDescending(x => x.CreatedDate));
                        return j;
                    }
                    else
                    {
                        var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_DRAFT" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").OrderByDescending(x => x.CreatedDate));
                        return j;
                    }
                }
            }
            return Json(new ServiceViewModel());

        }
        [HttpPost]
        public async Task<IActionResult> ReadSEBIServiceDataInProgressPost([FromForm] DataTableRequest value, string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, bool isDisableCreate = false, string mode = null, string templatecode = null)
        {
            if (mode.IsNotNullAndNotEmpty())
            {
                var result = await _serviceBusiness.GetSEBIExternalServiceList(templatecode);

                var j = Json(result.Where(x => x.ServiceStatusCode == "TASK_STATUS_INPROGRESS" || x.ServiceStatusCode == "TASK_STATUS_OVERDUE").OrderByDescending(x => x.CreatedDate));
                return j;
            }
            else
            {
                var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
                if (templateCodes == "DMS_SUPPORT_TICKET")
                {
                    var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_DRAFT" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").OrderByDescending(x => x.CreatedDate));
                    return j;
                }
                else
                {
                    if (isDisableCreate)
                    {
                        var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").OrderByDescending(x => x.CreatedDate));
                        return j;
                    }
                    else
                    {
                        var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_DRAFT" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").OrderByDescending(x => x.CreatedDate));
                        return j;
                    }
                }
            }
            return Json(new ServiceViewModel());

        }
        public async Task<IActionResult> ReadServiceDataOverdue(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, string mode = null, string templatecode = null)
        {

            if (mode.IsNotNullAndNotEmpty())
            {
                var result = await _serviceBusiness.GetSEBIExternalServiceList(templatecode);

                var j = Json(result.Where(x => x.ServiceStatusCode == "TASK_STATUS_OVERDUE").OrderBy(x => x.CreatedDate));
                return j;
            }
            else
            {
                var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
                var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").OrderBy(x => x.CreatedDate));
                return j;
            }
            return Json(new ServiceViewModel());
        }
        public async Task<IActionResult> ReadServiceDataCompleted(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, string mode = null, string templatecode = null)
        {
            if (mode.IsNotNullAndNotEmpty())
            {
                var result = await _serviceBusiness.GetSEBIExternalServiceList(templatecode);
                var j = Json(result.Where(x => x.ServiceStatusCode == "TASK_STATUS_COMPLETE" || x.ServiceStatusCode == "TASK_STATUS_CANCEL" || x.ServiceStatusCode == "TASK_STATUS_REJECT").OrderByDescending(x => x.LastUpdatedDate));
                return j;
            }
            else
            {
                var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
                var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL" || x.ServiceStatusCode == "SERVICE_STATUS_REJECT").OrderByDescending(x => x.LastUpdatedDate));
                return j;
            }
        }
        public async Task<IActionResult> ReadServiceDataClosed(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, string mode = null, string templatecode = null)
        {
            if (mode.IsNotNullAndNotEmpty())
            {
                var result = await _serviceBusiness.GetSEBIExternalServiceList(templatecode);

                var j = Json(result.Where(x => x.ServiceStatusCode == "TASK_STATUS_CLOSE").OrderByDescending(x => x.LastUpdatedDate));
                return j;
            }
            else
            {
                var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
                var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_CLOSE").OrderByDescending(x => x.LastUpdatedDate));
                return j;
            }
        }
        public async Task<IActionResult> ReadServiceDataAccepted(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false)
        {
            var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
            var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_CLOSE").OrderByDescending(x => x.LastUpdatedDate));
            return j;
        }
        public async Task<IActionResult> InternalServiceDashboard(string categoryCodes = null, string templateCodes = null)
        {
            ProjectDashboardViewModel model = new ProjectDashboardViewModel();
            model.CategoryCode = categoryCodes;
            model.TemplateCode = templateCodes;
            return View(model);
        }
        public async Task<IActionResult> ExternalServiceDashboard()
        {
            ProjectDashboardViewModel model = new ProjectDashboardViewModel();

            return View(model);
        }

        public async Task<ActionResult> GetExternalUserServiceChartByStatus()
        {

            var viewModel = await _serviceBusiness.GetExternalUserServiceChartByStatus();

            var chartdata = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                ItemValueSeries = viewModel.Select(x => x.Value).ToList(),
                ItemStatusColor = viewModel.Select(x => x.StatusColor).ToList(),
                Code = viewModel.Select(x => x.Code).FirstOrDefault(),
            };

            return Json(chartdata);
        }
        public async Task<ActionResult> GetExternalUserInternalServiceChartByStatus()
        {
            var viewModel = await _serviceBusiness.GetExternalUserInternalServiceChartByStatus();

            var chartdata = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                ItemValueSeries = viewModel.Select(x => x.Value).ToList(),
                Code = viewModel.Select(x => x.Code).FirstOrDefault(),
            };

            return Json(chartdata);
        }

        public async Task<ActionResult> GetExternalUserExternalServiceSLA(/*ServiceSearchViewModel search = null*/ DateTime? StartDate, DateTime? DueDate)
        {
            if (StartDate != null && DueDate != null)
            {
                var search = new ServiceSearchViewModel()
                {
                    StartDate = StartDate,
                    DueDate = DueDate
                };
                search.UserId = _userContext.UserId;
                var viewModel = await _serviceBusiness.GetExternalUserExternalServiceSLA(search);

                var chartdata = new ProjectDashboardChartViewModel
                {
                    ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                    LineChartValueSeries = viewModel.Select(x => x.Days).ToList(),
                    LineChartValueSeries1 = viewModel.Select(x => x.ActualSLA).ToList(),
                };

                return Json(chartdata);
            }
            else { return Json(""); }
        }
        public async Task<IActionResult> GetExternalUserSEBIServiceList(/*[DataSourceRequest] DataSourceRequest request,*/ string ServiceStatusIds = null, ServiceSearchViewModel search = null)
        {

            var result = await _serviceBusiness.GetExternalSEBIServiceList();

            //search.ServiceStatusIds = ServiceStatusIds;
            search.ServiceStatusIds = ServiceStatusIds.IsNotNull() ? ServiceStatusIds.Split(',').ToList() : null;
            if (search.ServiceStatusIds.IsNotNull() && search.ServiceStatusIds.Count() > 0)
            {
                var status = string.Join(",", search.ServiceStatusIds);
                result = result.Where(x => status.Contains(x.ServiceStatusName)).ToList();

            }
            if (search.StatusIds.IsNotNull() && search.StatusIds.Count() > 0)
            {
                var ServiceStatusId = string.Join(",", search.StatusIds);
                result = result.Where(x => ServiceStatusId.Contains(x.ServiceStatusName.ToString())).ToList();

            }
            var j = Json(result/*.ToDataSourceResult(request)*/);
            return j;

        }


        public async Task<ActionResult> GetServiceLogsDetails(string ServiceId, string templateCode, string UdfNoteId, string TemplateType)
        {
            ViewBag.TemplateType = TemplateType;
            var columnlist = await _serviceBusiness.GetDynamicServiceColumnLst(templateCode, TemplateType);
            columnlist.Insert(0, "LogStartDate");
            var model = new NtsLogViewModel { Id = ServiceId, TemplateCode = templateCode, ColumnName = columnlist };
            return View(model);
        }

        public async Task<ActionResult> ReadDataLog(string ServiceId, string TemplateCode, string TemplateType)
        {
            //var model = await _serviceBusiness.GetServiceLog(ServiceId, TemplateCode);
            DataTable data = await _serviceBusiness.GetDynamicService(TemplateCode, ServiceId, TemplateType);

            var dsResult = data;
            return Json(dsResult);
        }

        public async Task<ActionResult> GetServiceUsersIdNameList(string serviceId)
        {
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            list.Add(new IdNameViewModel { Id = "All", Name = "All" });
            var userList = await _serviceBusiness.GetServiceUserList(serviceId);
            list.AddRange(userList);
            return Json(list);
        }
        public IActionResult Document()
        {
            return View();
        }
        public async Task<IActionResult> GetDocumentTreeviewList(string id, string type, string parentId, string serviceId, string userId, string stageName, string stageId, string batchId, string expandingList, string inboxCode)
        {
            var result = await _serviceBusiness.GetDocumentTreeviewList(id, type, parentId, serviceId, expandingList);
            var model = result.ToList();
            return Json(model);
        }
        public IActionResult DocumentIndex(string serviceId)
        {
            serviceId = "fa7d3beb-193b-412b-817f-4a4bdcd0ea83";
            ViewBag.DocServiceId = serviceId;
            return View();
        }
        public async Task<IActionResult> GetDocumentIndexTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string stageName, string stageId, string batchId, string expandingList, string docServiceId)
        {
            var result = await _serviceBusiness.GetDocumentIndexTreeviewList(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes, docServiceId);
            var model = result.ToList();
            return Json(model);
        }

        public async Task<IActionResult> GenerateServiceWorkFlowDiagramNodesView(List<DiagramNode> Nodes, string id, bool isTemplate = false, List<DiagramConnector> Connectors = null)
        {
            var _serviceId = id;
            ServiceViewModel _service = null;
            var result = new List<WorkflowViewModel>();
            var actualResult = new List<WorkflowViewModel>();

            //if (serviceId != null)
            //{
            //    _service = await _serviceBusiness.GetSingleById(_serviceId);
            //}
            //else
            //{
            //    _service = await _serviceBusiness.GetSingle(x=>x.TemplateId == templateMasterId);
            //}

            if (!isTemplate)
            {
                _service = await _serviceBusiness.GetSingleById(_serviceId);
                if (_service.IsNotNull())
                {
                    var status = await _lovBusiness.GetSingleById(_service.ServiceStatusId);
                    if (status.IsNotNull())
                    {
                        if (status.Code == "SERVICE_STATUS_DRAFT")
                        {
                            isTemplate = true;
                            result = await _templateBusiness.GetWorkFlowDiagramDetailsByTemplate(_service.TemplateId);
                        }
                        else
                        {
                            result = await _templateBusiness.GetWorkFlowDiagramDetails(id);
                        }
                    }
                    else
                    {
                        result = await _templateBusiness.GetWorkFlowDiagramDetails(id);
                    }
                }
            }
            else
            {
                actualResult = await _templateBusiness.GetWorkFlowDiagramDetailsByTemplate(id);
                result = actualResult.DistinctBy(x => x.Id).ToList();
            }



            var _subject = "";
            if (_service.IsNotNull())
            {
                //if (string.IsNullOrEmpty(_service.ServiceSubject))
                //{
                var template = await _templateBusiness.GetSingleById(_service.TemplateId);
                if (template.IsNotNull())
                {
                    _subject = template.DisplayName;

                }
                //  _subject = _service.ServiceDescription;
                //}
                //else
                //{
                //    _subject = _service.ServiceSubject;
                //}
            }
            else
            {
                _service = new ServiceViewModel();
                var serviceTemplate = result.Where(x => x.Type == "Service" && x.ParentId == null).FirstOrDefault();
                if (serviceTemplate.IsNotNull())
                {
                    _service.ServiceSubject = serviceTemplate.Subject;
                    _service.ServiceStatusName = "Draft";
                    _service.RequestedByUserId = "";
                    _service.ServiceStatusId = "";
                    _service.TemplateId = id;
                }
            }
            ///  = (string.IsNullOrEmpty(_service.ServiceSubject)) ? _service.ServiceDescription : _service.ServiceSubject;
            //var tasksList = new List<TaskViewModel>();

            //if (_serviceId != null)
            //{
            //    tasksList = await _taskBusiness.GetList(x=>x.ParentServiceId == _serviceId);
            //}
            //else
            //{
            //    //var model = new ServiceViewModel();

            //   tasksList = await _taskBussiness.GetStepTasks(null, _service);
            //}

          //  result = result.DistinctBy(x => x.TemplateId).ToList();
            List<string> workFlowStepList = new List<string>();
            foreach (var t in result)
            {
                if (!workFlowStepList.Contains(t.StepName) && t.StepId.IsNotNull())
                {
                    workFlowStepList.Add(t.StepName);
                }
            }

            if (workFlowStepList.Count() == 0 & result.IsNotNull())
            {
                workFlowStepList.Add("Lane");
                result.Select(c => { c.StepId = "Lane"; c.StepName = "Lane"; return c; }).ToList();
            }

            //workFlowStepList.Add(tasksList.Select(x => x.WorkFlowStep).Distinct().ToList());
            var workFlowStageList = result.Select(x => x.StageName).Where(x => x != null).Distinct().ToList();
            if (workFlowStageList.Count() == 0 & result.IsNotNull())
            {
                workFlowStageList.Add("Lane");
                result.Select(c => { c.StageId = "Lane"; c.StageName = "Lane"; return c; }).ToList();
            }
            List<Lane> Lanes = new List<Lane>();
            var _fontSize = 0;
            var _bgcolorService = string.Empty;
            var _bgcolorNote = string.Empty;

            //if (From != string.Empty)
            //{
            //    _fontSize = 9;
            //}
            //else
            //{
            //    _fontSize = 11;
            //}

            _fontSize = 8;

            if (_service.ServiceStatusName == "Draft" || _service.ServiceStatusName == null)
            {
                _bgcolorNote = "#5bc0de";
            }
            else if (_service.ServiceStatusName == "In Progress")
            {
                _bgcolorNote = "#f0ad4e";
            }
            else if (_service.ServiceStatusName == "Completed")
            {
                _bgcolorNote = "#5cb85c";
            }
            else if (_service.ServiceStatusName == "Overdue")
            {
                _bgcolorNote = "#d9534f";
            }
            else if (_service.ServiceStatusName == "Cancel")
            {
                _bgcolorNote = "#999999";
            }

            var laneslist = result.Select(x => x.StageId).Distinct().ToList();
            if (laneslist[0] == null)
            {
                laneslist = new List<string>();
                laneslist.Add("Lane");
                result.Select(c => { c.StageId = "Lane"; c.StageName = "Lane"; return c; }).ToList();
            }
            var Phaselist = result.Select(x => x.StepId).Distinct().ToList();
            if (Phaselist[0] == null)
            {
                Phaselist = new List<string>();
                Phaselist.Add("Phase");
                result.Select(c => { c.StepId = "Phase"; c.StepName = "Phase"; return c; }).ToList();

            }

            if (workFlowStepList.Count() > 0)
            {
                if (workFlowStageList[0] != null)
                {
                    List<DiagramPort> ports = new List<DiagramPort>();

                    for (int i = 1; i <= workFlowStepList.Count(); i++)
                    {
                        var _id = string.Concat("Port", i);

                        ports.Add(new DiagramPort() { Id = _id, Offset = new DiagramPoint() { X = 2, Y = 0.5 }, Visibility = PortVisibility.Connect | PortVisibility.Hover, Constraints = PortConstraints.Draw });

                    }

                    int j = 20;
                    int _nodeCount = 0;

                    for (int i = 0; i < workFlowStepList.Distinct().Count(); i++)
                    {
                        var _workFlowStepName = workFlowStepList[i];
                        var _idLanes = string.Concat("stackCanvas", i);

                        List<DiagramNode> firstLaneChildren = new List<DiagramNode>();

                        var assignee = await _userBusiness.GetSingleById(_service.RequestedByUserId);

                        //if (_service.StageName == _workFlowStepName)
                        //{
                        //    List<DiagramNodeAnnotation> node1Annotation = new List<DiagramNodeAnnotation>();
                        //    node1Annotation.Add(new DiagramNodeAnnotation()
                        //    {
                        //        Id = _service.Id.ToString() + "_service",
                        //        //Content = "<b>ABCD</b>".HtmlEncode(),
                        //        //Content = Helper.WordWrap(Environment.NewLine + _subject + Environment.NewLine + Environment.NewLine + assignee.UserName + Environment.NewLine + Environment.NewLine  + _service.SubmittedDate, 30),
                        //        Style = new DiagramTextStyle()
                        //        {
                        //            Color = "black",
                        //            Fill = "transparent",
                        //            TextWrapping = TextWrap.Wrap,
                        //            FontSize = 10
                        //        }

                        //    });
                        //    var idser = _service.Id.ToString() + "_service";
                        //    var subject = Helper.WordWrap(Environment.NewLine + _subject, 30);

                        //    var serviceContent = "<div style='background:" + _bgcolorService + ";height:100%;width:100%;font-size:12px;border-color:black;border-style:solid;border-width:1px;text-align:center'>" + subject + "<br/>" + assignee.UserName
                        //    + "<br/>" + _service.SubmittedDate + "<br/></div>";
                        //    DiagramNode node1 = new DiagramNode()
                        //    {
                        //        Id = "node0",
                        //        Annotations = node1Annotation,
                        //        Style = new DiagramTextStyle() { Fill = _bgcolorService, FontSize = _fontSize },
                        //        Margin = new DiagramMargin() { Left = 80, Top = 60, Right = 30, Bottom = 30 },
                        //        Height = 85,
                        //        Width = 250,
                        //        Ports = ports,
                        //        Constraints = NodeConstraints.Default | NodeConstraints.Tooltip,
                        //        Tooltip = new DiagramDiagramTooltip()
                        //        {
                        //            Content = _service.ServiceStatusName,
                        //            ShowTipPointer = true,
                        //        },
                        //        Shape = new { type = "HTML", content = serviceContent },
                        //    };
                        //    //node1.Constraints = NodeConstraints.ReadOnly;
                        //    firstLaneChildren.Add(node1);
                        //}

                        var _tasks = result.Where(x => x.StepName == _workFlowStepName).ToList();

                        if (_tasks.Count() > 0)
                        {
                            for (int y = 1; y <= _tasks.Count(); y++)
                            {
                                int k = y - 1;
                                _nodeCount = _nodeCount + 1;
                                var _nodeid = string.Concat("node", _tasks[k].Id);
                                var _previousnodeid = string.Concat("node", _nodeCount - 1);
                                var _content = _tasks[k].Subject;
                                // var _sequenceNo = _tasks[k].S;
                                var _workFlowStep = _tasks[k].StepName;

                                if (_tasks[k].StatusName == "Draft" || _tasks[k].StatusName == null)
                                {
                                    _bgcolorNote = "#5bc0de";
                                }
                                else if (_tasks[k].StatusName == "In Progress")
                                {
                                    _bgcolorNote = "#f0ad4e";
                                }
                                else if (_tasks[k].StatusName == "Completed")
                                {
                                    _bgcolorNote = "#5cb85c";
                                }
                                else if (_tasks[k].StatusName == "Overdue")
                                {
                                    _bgcolorNote = "#d9534f";
                                }
                                else if (_tasks[k].StatusName == "Cancel")
                                {
                                    _bgcolorNote = "#999999";
                                }
                                var taskassigneeName = "";
                                var assineeDetails = await _userBusiness.GetSingleById(_tasks[k].AssignedToUserId);
                                if (assineeDetails == null)
                                {
                                    taskassigneeName = "";
                                }
                                else
                                {
                                    taskassigneeName = assineeDetails.UserName;
                                }

                                List<DiagramNodeAnnotation> node2Annotation = new List<DiagramNodeAnnotation>();
                                node2Annotation.Add(new DiagramNodeAnnotation()
                                {
                                    Id = _tasks[k].Id.ToString() + "_task",
                                    //Content = Helper.WordWrap(Environment.NewLine + _content + Environment.NewLine + Environment.NewLine + taskassigneeName + Environment.NewLine + Environment.NewLine + "Due: " + _tasks[k].DueDate, 30),
                                    Style = new DiagramTextStyle()
                                    {
                                        Color = "black",
                                        Fill = "transparent",
                                        TextWrapping = TextWrap.Wrap,
                                        FontSize = 10
                                    },

                                });
                                var idser = _tasks[k].Id.ToString() + "_task";
                                var subject = Helper.WordWrap(Environment.NewLine + _subject, 30);
                                var taskContent = "";
                                //if (_tasks[k].Ass != null)
                                //{
                                //    taskContent = "<div style='background:" + _bgcolorNote + ";height:100%;width:100%;font-size:12px;border-color:black;border-style:solid;border-width:1px;text-align:center'>" + _content + "<br/>" + taskassigneeName + "( " + "<a class='hlkTeam' style = 'font-size:12px;cursor:pointer;' onclick =onTeam('" + _tasks[k].Id + "," + _tasks[k].AssignedToTeam.Name + "') >" + _tasks[k].AssignedToTeam.Name + "</a> )"
                                // + "<br/>" + _tasks[k].DueDate + "<br/></div>";
                                //}
                                //else
                                // {
                                if (_tasks[k].Type == "Task" && _tasks[k].StatusName != null)
                                {
                                    taskContent = "<div style='background:" + _bgcolorNote + ";height:100%;width:100%;font-size:12px;border-color:black;border-style:solid;border-width:1px;text-align:center;color:white'>" + _content + "<br/>" + taskassigneeName
                                        + "<br/>" + _tasks[k].DueDate + "<br/> <a style='font-size:12px;cursor:pointer;' onclick=onViewTask('" + _tasks[k].Id + "," + _tasks[k].TemplateCode + "')>View</a></div>";
                                }
                                else if (_tasks[k].Type == "Task" && _tasks[k].StatusName == null)
                                {
                                    taskContent = "<div style='background:" + _bgcolorNote + ";height:100%;width:100%;font-size:12px;border-color:black;border-style:solid;border-width:1px;text-align:center;color:white'>" + _content + "<br/>" + taskassigneeName
                                        + "<br/>" + _tasks[k].DueDate + "<br/> </div>";
                                }
                                else
                                {
                                    taskContent = "<div style='background:" + _bgcolorNote + ";height:100%;width:100%;font-size:12px;border-color:black;border-style:solid;border-width:1px;text-align:center;color:white'>" + _content + "<br/>" + taskassigneeName
                                                                            + "<br/>" + _tasks[k].DueDate + "<br/><a style = 'font-size:12px;cursor:pointer;' onclick=onViewService('" + _tasks[k].Id + "," + _tasks[k].TemplateCode + "')>View</a></div>";
                                }
                                //}
                                _nodeid = string.Concat("node", _tasks[k].Id);

                                DiagramNode node2 = new DiagramNode()
                                {
                                    Id = _nodeid,
                                    Annotations = node2Annotation,
                                    Constraints = NodeConstraints.Default | NodeConstraints.Tooltip,
                                    Style = new DiagramTextStyle() { Fill = _bgcolorNote, FontSize = _fontSize },
                                    Margin = new DiagramMargin() { Left = 50, Top = j, Right = 30 },
                                    Height = 85,
                                    Width = 250,
                                    Ports = ports,
                                    Tooltip = new DiagramDiagramTooltip()
                                    {
                                        Content = _tasks[k].StatusName,
                                        ShowTipPointer = true,
                                    },
                                    Shape = new { type = "HTML", content = taskContent },

                                };
                                firstLaneChildren.Add(node2);

                                var _idConnector = string.Concat("connector", i, y);


                                j = j + 150;
                            }




                        }

                        Lanes.Add(new Lane()
                        {
                            Orientation = "Vertical",
                            Id = _idLanes,
                            Height = 500,
                            Width = 300,
                            Header = new Header()
                            {
                                Annotation = new DiagramNodeAnnotation() { Content = _workFlowStepName },
                                Width = 100,
                                Orientation = "Horizontal",
                                Height = 20

                            },
                            Children = firstLaneChildren
                        });
                    }
                }
                else
                {
                    List<DiagramNode> firstLaneChildren = new List<DiagramNode>();
                    List<DiagramPort> ports = new List<DiagramPort>();

                    ports.Add(new DiagramPort() { Id = "Port1", Offset = new DiagramPoint() { X = 2, Y = 0.5 }, Visibility = PortVisibility.Connect | PortVisibility.Hover, Constraints = PortConstraints.Draw });

                    var assignee = await _userBusiness.GetSingleById(_service.RequestedByUserId);

                    //List<DiagramNodeAnnotation> node1Annotation = new List<DiagramNodeAnnotation>();
                    //node1Annotation.Add(new DiagramNodeAnnotation()
                    //{
                    //    Id = _service.Id + "_service",
                    //    //Content = "<b>ABCD</b>".HtmlEncode(),
                    //    //Content = Helper.WordWrap(Environment.NewLine + _subject + Environment.NewLine + Environment.NewLine + assignee.UserName + Environment.NewLine + Environment.NewLine + _service.SubmittedDate, 30),
                    //    Style = new DiagramTextStyle()
                    //    {
                    //        Color = "black",
                    //        Fill = "transparent",
                    //        TextWrapping = TextWrap.Wrap,
                    //        FontSize = 10
                    //    },

                    //});
                    //var idser = _service.Id.ToString() + "_service";
                    //var subject = Helper.WordWrap(Environment.NewLine + _subject, 30);
                    //var serviceContent = "<div style='background:" + _bgcolorService + ";height:100%;width:100%;font-size:12px;border-color:black;border-style:solid;border-width:1px;text-align:center'>" + subject + "<br/>" + assignee.UserName
                    //    + "<br/>" + _service.SubmittedDate + "<br/><a style='font-size:12px;cursor:pointer;' onclick=onTeam('" + idser + "')>Team</a></div>";
                    //DiagramNode node1 = new DiagramNode()
                    //{
                    //    Id = "node0",
                    //    Annotations = node1Annotation,
                    //    Constraints = NodeConstraints.Default | NodeConstraints.Tooltip,
                    //    Style = new DiagramTextStyle() { Fill = _bgcolorService, FontSize = _fontSize },
                    //    Margin = new DiagramMargin() { Left = 80, Top = 60, Right = 30, Bottom = 30 },
                    //    Height = 85,
                    //    Width = 250,
                    //    Ports = ports,
                    //    Tooltip = new DiagramDiagramTooltip()
                    //    {
                    //        Content = _service.ServiceStatusName,
                    //        ShowTipPointer = true,
                    //    },
                    //    Shape = new { type = "HTML", content = serviceContent }
                    //};
                    //firstLaneChildren.Add(node1);


                    if (result.Count() > 0)
                    {
                        int j = 180;
                        int _nodeCount = 0;
                        for (int y = 1; y <= result.Count(); y++)
                        {
                            int k = y - 1;
                            _nodeCount = _nodeCount + 1;
                            var _nodeid = string.Concat("node", result[k].Id);
                            var _previousnodeid = string.Concat("node", _nodeCount - 1);
                            var _content = result[k].Subject;
                            // var _sequenceNo = result[k].SequenceOrder;
                            var _workFlowStep = result[k].StepName;

                            if (result[k].StatusName == "Draft")
                            {
                                _bgcolorNote = "#5bc0de";
                            }
                            else if (result[k].StatusName == "In Progress")
                            {
                                _bgcolorNote = "#f0ad4e";
                            }
                            else if (result[k].StatusName == "Completed")
                            {
                                _bgcolorNote = "#5cb85c";
                            }
                            else if (result[k].StatusName == "Overdue")
                            {
                                _bgcolorNote = "#d9534f";
                            }
                            else if (result[k].StatusName == "Cancel")
                            {
                                _bgcolorNote = "#999999";
                            }
                            var taskassigneeName = "";
                            var assineeDetails = await _userBusiness.GetSingleById(result[k].AssignedToUserId);
                            if (assineeDetails == null)
                            {
                                taskassigneeName = "";
                            }
                            else
                            {
                                taskassigneeName = assineeDetails.UserName;
                            }

                            List<DiagramNodeAnnotation> node2Annotation = new List<DiagramNodeAnnotation>();
                            node2Annotation.Add(new DiagramNodeAnnotation()
                            {
                                Id = result[k].Id + "_task",
                                //Content = Helper.WordWrap(Environment.NewLine + _content + Environment.NewLine + Environment.NewLine + tasksList[k].AssignedUserUserName + Environment.NewLine + Environment.NewLine + "Due: " + tasksList[k].DueDate, 30),
                                Style = new DiagramTextStyle()
                                {
                                    Color = "black",
                                    Fill = "transparent",
                                    TextWrapping = TextWrap.Wrap,
                                    FontSize = 10
                                },

                            });
                            var idtask = result[k].Id.ToString() + "_task";
                            var content = Helper.WordWrap(Environment.NewLine + _content, 30);
                            var taskContent = "";
                            //if (result[k].AssignedToTeamId != null)
                            //{
                            //    taskContent = "<div style='background:" + _bgcolorNote + ";height:100%;width:100%;font-size:12px;border-color:black;border-style:solid;border-width:1px;text-align:center'>" + "<br/>" + tasksList[k].AssignedToTeam.Name + _content + "( " + "<a class='hlkTeam' style = 'font-size:12px;cursor:pointer;' onclick = onTeam('" + tasksList[k].Id + "," + tasksList[k].AssignedToTeam.Name + "') >" + tasksList[k].AssignedToTeam.Name + "</a> )"
                            //+ "<br/>" + result[k].DueDate + "<br/></div>";
                            //}
                            //else
                            //{
                            taskContent = "<div style='background:" + _bgcolorNote + ";height:100%;width:100%;font-size:12px;border-color:black;border-style:solid;border-width:1px;text-align:center;color:white'>" + _content + "<br/>" + result[k].AssignedToUserName
                               + "<br/>" + result[k].DueDate + "<br/></div>";
                            // }
                            DiagramNode node2 = new DiagramNode()
                            {
                                Id = _nodeid,
                                Annotations = node2Annotation,
                                Constraints = NodeConstraints.Default | NodeConstraints.Tooltip,
                                Style = new DiagramTextStyle() { Fill = _bgcolorNote, FontSize = _fontSize },
                                Margin = new DiagramMargin() { Left = 50, Top = j, Right = 30 },
                                Height = 85,
                                Width = 250,
                                Ports = ports,
                                Tooltip = new DiagramDiagramTooltip()
                                {
                                    Content = result[k].StatusName,
                                    ShowTipPointer = true,
                                },
                                Shape = new { type = "HTML", content = taskContent }
                            };
                            firstLaneChildren.Add(node2);

                            var _idConnector = string.Concat("connector", y);


                            //if (y == 1)
                            //{
                            //    Connectors.Add(CreateSwimlaneConnector(_idConnector, "node0", _nodeid, ""));
                            //}
                            //else
                            //{
                            //    Connectors.Add(CreateSwimlaneConnector(_idConnector, _previousnodeid, _nodeid, ""));
                            //}

                            j = j + 200;
                        }


                        Lanes.Add(new Lane()
                        {
                            Orientation = "Vertical",
                            Id = "stackCanvas1",
                            Height = 500,
                            Width = 300,
                            Header = new Header()
                            {
                                Annotation = new DiagramNodeAnnotation() { Content = _subject },
                                Width = 250,
                                Orientation = "Horizontal",
                                Height = 20,
                            },
                            Children = firstLaneChildren
                        });




                    }




                }


            }
            else
            {
                Lanes.Add(new Lane()
                {
                    Orientation = "Vertical",
                    Id = "stackCanvas1",
                    Height = 500,
                    Width = 300,
                    Header = new Header()
                    {
                        Annotation = new DiagramNodeAnnotation() { Content = "No Service Workflow Available." },
                        Width = 250,
                        Orientation = "Horizontal",
                        Height = 20,
                    },
                    //Children = firstLaneChildren
                });

            }

            // int cnt = 1;

            //create connectors
            if (result.Count() > 0)
            {
                //var nodeCount = 1;
                //for (int y = 1; y <= result.Count(); y++)
                //{
                //    int k = y - 1;
                //    //cnt = cnt + 1;
                //    var _nodeid = "";//string.Concat("node", cnt);

                //    foreach (var l in Lanes)
                //    {
                //        var found = false;
                //        foreach (var c in l.Children)
                //        {
                //            var b = c.Annotations.Where(x => x.Id == result[k].Id + "_task").FirstOrDefault();
                //            if (b.IsNotNull())
                //            {
                //                _nodeid = c.Id;
                //                break;
                //            }
                //            if (found == true) { break; }
                //        }
                //        if (found == true) { break; }
                //    }


                //if (_nodeid != "")
                //{
                //    var triggeredByItems = _serviceTaskTemplateBusiness.GetTriggeredByServiceTaskTemplates(tasksList[k].ServiceTaskTemplateId ?? 0);
                //    if (triggeredByItems == null || triggeredByItems.Count == 0)
                //    {
                //        var _idConnector = string.Concat("connector", cnt, y);
                //        Connectors.Add(CreateSwimlaneConnectorNew(_idConnector, "node0", _nodeid, ""));
                //        cnt++;
                //    }
                //    else
                //    {
                //        foreach (var triggerBy in triggeredByItems)
                //        {

                //            var ann = tasksList.Where(x => x.ServiceTaskTemplateId == triggerBy.Id).FirstOrDefault();
                //            if (ann.IsNotNull())
                //            {

                //                foreach (var l in Lanes)
                //                {
                //                    var found = false;
                //                    foreach (var c in l.Children)
                //                    {
                //                        var _idConnector = string.Concat("connector", cnt, y);
                //                        var a = c.Annotations.Where(x => x.Id == ann.Id.ToString() + "_task").FirstOrDefault();
                //                        var b = c.Annotations.Where(x => x.Id == tasksList[k].Id + "_task").FirstOrDefault();
                //                        if (a.IsNotNull())
                //                        {
                //                            Connectors.Add(CreateSwimlaneConnectorNew(_idConnector, c.Id, _nodeid, ""));
                //                            cnt++;
                //                            found = true;
                //                            break;
                //                        }
                //                        if (found == true) { break; }
                //                    }
                //                    if (found == true) { break; }
                //                }
                //            }

                //        }
                //        //}
                //    }
                //}

                var index = 1;
                actualResult = actualResult.Where(x => x.Type == "Task").ToList();
                foreach (var i in actualResult)
                {
                    var src = result.Where(x => x.ComponentId == i.ParentId).FirstOrDefault();
                    var taskIdst = i.Id != null ? i.Id : i.TemplateId;

                    if (src.IsNotNull())
                    {
                        var taskIds = src.Id != null ? src.Id : src.TemplateId;

                        var sourceId = "node" + taskIds;
                        var targetId = "node" + taskIdst;

                        Connectors.Add(new DiagramConnector
                        {
                            Id = "connectors" + index.ToString(),
                            SourceID = sourceId,
                            TargetID = targetId
                        });
                        index++;
                    }
                    else
                    {
                        var serviceD = result.Where(x => x.Type == "Service").FirstOrDefault();
                        var sourceId = "node" + serviceD.Id;
                        var targetId = "node" + taskIdst;
                        Connectors.Add(new DiagramConnector
                        {
                            Id = "connectors" + index.ToString(),
                            SourceID = sourceId,
                            TargetID = targetId
                        });
                        index++;
                    }
                    //}
                }
            }


            //Create swimlane
            DiagramNode swimlane = new DiagramNode();
            swimlane.Id = "swimlane";
            swimlane.Width = 1000;
            swimlane.Height = 4000;
            swimlane.OffsetX = 400;
            swimlane.OffsetY = 400;
            swimlane.Constraints = NodeConstraints.ReadOnly;
            //= from s in tasksList Order By s. Select s




            //Create phases
            List<Phase> Phases = new List<Phase>();

            if (workFlowStageList.Count() > 0)
            {
                if (workFlowStageList[0] != null)
                {
                    int z = 1;
                    foreach (string item in workFlowStageList)
                    {
                        var _idPhase = string.Concat("phase", z);

                        Phases.Add(new Phase()
                        {
                            Orientation = "Vertical",
                            Id = _idPhase,
                            Offset = 170,
                            Header = new Header()
                            {
                                Annotation = new DiagramNodeAnnotation() { Content = item },
                            },
                        });

                        z++;
                    }
                }
                else
                {
                    var _content = string.Concat("STAGE - 1 - ", _subject);

                    Phases.Add(new Phase()
                    {
                        Orientation = "Vertical",
                        Id = "phase1",
                        Offset = 170,
                        Header = new Header()
                        {
                            Annotation = new DiagramNodeAnnotation() { Content = _content },
                        },
                    });
                }

            }

            swimlane.Shape = new SwimLane()
            {
                Type = "SwimLane",
                PhaseSize = 20,
                Orientation = "Vertical",
                Header = new Header()
                {
                    Annotation = new DiagramNodeAnnotation() { Content = _service.ServiceSubject },
                    Height = 50,
                    Orientation = "Horizontal",
                    Style = new DiagramTextStyle() { FontSize = _fontSize, TextOverflow = TextOverflow.Wrap }
                },
                Lanes = Lanes,
                Phases = Phases

            };
            Nodes.Add(swimlane);
            return null;
        }

        public DiagramConnector CreateSwimlaneConnectorNew(string id, string sourceID, string targetID, string label)
        {
            var color = Helper.RandomColor();
            DiagramConnector connector = new DiagramConnector();
            connector.Id = id;
            connector.Type = Segments.Orthogonal;
            connector.SourceID = sourceID;
            connector.TargetID = targetID;
            connector.Style = new DiagramStrokeStyle() { StrokeColor = color, Fill = color, StrokeWidth = 2 };
            if (label != "")
            {
                connector.Annotations = new List<DiagramConnectorAnnotation>()
                {
                    new DiagramConnectorAnnotation()
                    {
                        Content = label,
                        Style = new DiagramTextStyle()
                        {
                            Fill = "white"
                        }
                    }
                };
            }
            return connector;
        }


        public IActionResult ServiceItemMove(string serviceId, NtsTypeEnum ntsType, string ntsId)
        {
            var model = new ServiceViewModel();
            model.Id = serviceId;
            model.NtsType = ntsType;
            model.NtsId = ntsId;
            return View(model);
        }

        public async Task<IActionResult> GetMoveToParent(string serviceId, string ntsId, string parentId)
        {
            var result = await _serviceBusiness.GetBookList(serviceId, null);
            result = result.Where(x => x.ItemType != ItemTypeEnum.StepTask && x.Id != "0" && x.Id != serviceId).ToList();
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
        public async Task<IActionResult> ManageMoveToParent(ServiceViewModel model)
        {
            var result = await _serviceBusiness.ManageMoveToParent(model);
            if (result.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, error = result.Message });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteServiceBook(string serviceId)
        {
            await _serviceBusiness.DeleteServiceBook(serviceId);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteNoteBook(string noteId)
        {
            await _noteBusiness.DeleteNoteBook(noteId);
            return Json(new { success = true });
        }


        public async Task<IActionResult> ServiceBookHTML(string serviceId, string templateId)
        {
            var model = await _serviceBusiness.GetBookDetails(serviceId);
            return View(model);
        }

        public async Task<IActionResult> Info(string code, string type)
        {
            if (type == "Category")
            {
                var result = await _templateCategoryBusiness.GetSingle(x => x.Code == code);
                ViewBag.Description = result.Description;
            }
            else if (type == "Template")
            {
                var result = await _templateBusiness.GetSingle(x => x.Code == code);
                ViewBag.Description = result.Description;
            }
            return View();
        }

        public async Task<IActionResult> ServiceListByFilters(string userId = null, string statusCodes = null, string templateCodes = null, string parentServiceId = null, string cbm = null, bool showAllOwnersService = false, string categoryCodes = null, string portalNames = null)
        {
            var portalIds = _userContext.PortalId;
            if (portalNames.IsNotNull())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                portalIds = string.Join(",", portals.Select(x => x.Id).ToArray());
            }
            ViewBag.CBM = cbm;
            ViewBag.showAllOwnersService = showAllOwnersService;
            return View(new ServiceTemplateViewModel
            {
                OwnerUserId = userId,
                TemplateCode = templateCodes,
                ServiceStatusCode = statusCodes,
                ParentServiceId = parentServiceId,
                PortalId = _userContext.PortalId,
                PortalIds = portalIds,
                TemplateCategoryCode = categoryCodes
            });
        }

        public async Task<IActionResult> ReadServiceList(string templateCodes = null, string moduleCodes = null, string catCodes = null, string requestBy = null, bool showAllOwnersService = false, string statusCodes = null, string parentServiceId = null, string portalId = null)
        {
            portalId = portalId.IsNotNull() ? portalId : _userContext.PortalId;
            var dt = await _serviceBusiness.GetServiceList(portalId, moduleCodes, templateCodes, catCodes, requestBy, showAllOwnersService, statusCodes, parentServiceId);
            return Json(dt);
        }
        public ActionResult ServiceBookListScrollView(string mode, string templateCodes, string permissions)
        {
            ViewBag.TemplateCodes = templateCodes;
            ViewBag.Permissions = permissions;
            ViewBag.Mode = mode;
            return View();
        }

        public async Task<ActionResult> ServiceBookListHierarchyView(string mode, string templateCodes, string permissions)
        {
            ViewBag.TemplateCodes = templateCodes;
            ViewBag.Permissions = permissions;
            ViewBag.Mode = mode;
            var date = DateTime.Now.Date;
            //var LoggedInUserPositionId = _userContext.OrganizationId;

            var rootNodes = await _userBusiness.GetUserHierarchyRootId(_userContext.UserId, "BOOK_HIERARCHY", _userContext.UserId);
            var hierarchyId = rootNodes.Item3;
            var HierarchyRootNodeId = rootNodes.Item1;
            var AllowedRootNodeId = rootNodes.Item2;
            var lvl = await _hrCoreBusiness.GetUserNodeLevel(rootNodes.Item2, rootNodes.Item3);
            var viewModel = new UserChartIndexViewModel
            {
                HierarchyId = hierarchyId,
                HierarchyRootNodeId = "-1",
                AllowedRootNodeId = "-1",

                CanAddRootNode = HierarchyRootNodeId == AllowedRootNodeId && HierarchyRootNodeId == "",
                AllowedRootNodeLevel = 0,
                AsOnDate = date.ToYYYY_MM_DD_DateFormat(),
                // RequestSource = rs,               

            };
            // ViewBag.PositionId = orgId;           

            ViewBag.IsAsOnDate = date.ToYYYY_MM_DD_DateFormat();
            // ViewBag.LoggedInEmpId = personId;
            //ViewBag.LoggedInPositionId = LoggedInUserPositionId;

            ViewBag.AsOnDateDisplay = date.ToYYYY_MM_DD_DateFormat();

            return View(viewModel);
        }
        public IActionResult ServiceBookList(string templateCodes, string permissions, string bookId, bool canCreate = false)
        {
            ViewBag.TemplateCodes = templateCodes;
            ViewBag.Permissions = permissions;
            ViewBag.BookId = bookId;
            ViewBag.CanCreate = canCreate;
            return View();
        }
        public async Task<object> GetBookTreeList(string id, string templateCode, string permission)
        {
            var result = await _serviceBusiness.GetBookTreeList(id, templateCode, permission);
            var newList = new List<FileExplorerViewModel>();
            newList.AddRange(result.ToList().Select(x => new FileExplorerViewModel { key = x.id, NodeId = x.id, title = x.Name, lazy = true, type = x.Type, parentId = x.ParentId, WorkflowServiceId = x.StageId, Sequence = x.SequenceOrder, WorkspaceId = x.parent, Count = x.Count.ToString() }));
            var json = JsonConvert.SerializeObject(newList);
            return json;
        }
        public async Task<IActionResult> GetAllBook(string templateCodes, string search, string categoryIds, string groupIds, string permission)
        {
            var data = await _serviceBusiness.GetAllBook(templateCodes, null, groupIds, search, categoryIds, permission);
            return Json(data);
        }
        public async Task<IActionResult> GetTecProcessCategoryList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("SN_TEC_PROCESS_CATEGORY", "");
            return Json(data);
        }
        public async Task<IActionResult> GetTecProcessGroupList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("SN_TEC_PROCESS_GROUP", "");
            return Json(data);
        }

        public async Task<IActionResult> ServiceBookBrowse(string bookId, string pageId, string permissions, string ru, bool canCreate = false)
        {
            var data = new BookDetailViewModel();
            data.BookDetails = await _serviceBusiness.GetBookDetail(bookId);
            data.BookAllPages = await _serviceBusiness.GetBookAllPages(bookId);
            if (pageId.IsNotNullAndNotEmptyAndNotValue("undefined"))
            {
                data.BookPageDetails = await _serviceBusiness.GetBookPageDetail(bookId, pageId);
            }
            else
            {
                data.BookPageDetails = await _serviceBusiness.GetBookPageDetail(bookId, null);
                pageId = data.BookPageDetails.Id;
            }
            ViewBag.PageId = pageId;

            ViewBag.BookId = bookId;
            ViewBag.Permissions = permissions;
            ViewBag.ReturnUrl = ru;
            ViewBag.CanCreate = canCreate;
            return View(data);
        }



        public async Task<IActionResult> GetBookDetails(string bookId)
        {
            var data = new BookDetailViewModel();
            data.BookDetails = await _serviceBusiness.GetBookDetail(bookId);
            data.BookAllPages = await _serviceBusiness.GetBookAllPages(bookId);
            data.BookPageDetails = await _serviceBusiness.GetBookPageDetail(bookId, null);
            return Json(data);
        }


        public async Task<IActionResult> GetBookPageDetails(string bookId, string currentPageId)
        {
            var data = await _serviceBusiness.GetBookPageDetail(bookId, currentPageId);
            return Json(data);
        }

        public async Task<JsonResult> GetBookAllPages(string bookId, string id)
        {
            var result = await _serviceBusiness.GetBookAllPages(bookId);
            var newList = new List<FileExplorerViewModel>();
            //newList.AddRange(result.ToList().Select(x => new FileExplorerViewModel { key = x.Id, title = x.Name, lazy = true, type = x.SequenceOrder.ToString() }));
            //if(id.IsNotNullAndNotEmpty())
            //{
            //    newList = newList.Where(x => x.key == id).ToList();
            //}
            //var treeModel = new List<TreeViewViewModel>();

            //foreach (var x in result)
            //{
            //    treeModel.Add(new TreeViewViewModel
            //    {
            //        id = x.Id,
            //        Name = x.Name,
            //        DisplayName = x.Name,
            //        ParentId = null,
            //        hasChildren = true,
            //        expanded = true,
            //        Type = "generic",
            //        children = true,
            //        text = x.Name,
            //        parent = null,
            //        a_attr = new { data_id = x.Id, data_name = x.Name, data_type = x.Code, data_order = x.Count },
            //    });
            //}
            if (id.IsNotNullAndNotEmpty())
            {
                result = await _serviceBusiness.GetChildPageList(id);
            }
            newList.AddRange(result.ToList().Select(x => new FileExplorerViewModel { key = x.Id, title = x.Name, lazy = true, Sequence = x.SequenceOrder, Count = x.Count.ToString(), WorkflowServiceId = x.Code }));
            var json = JsonConvert.SerializeObject(newList);

            return Json(newList);
        }

        public async Task<JsonResult> GetBookAllPagesTreeList(string id, string parentId, string bookid)
        {
            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                var result = await _serviceBusiness.GetBookAllPages(bookid);
                var treelist = result.Select(x => new TreeViewViewModel()
                {
                    id = x.Id,
                    Name = x.Name,
                    DisplayName = x.Name,
                    ParentId = "#",
                    hasChildren = true,
                    expanded = true,
                    Type = "generic",
                    children = true,
                    text = x.Name,
                    parent = "#",
                    a_attr = new { data_id = x.Id, data_name = x.Name, data_type = x.Code, data_order = x.Count },
                });
                list.AddRange(treelist);
            }
            else
            {
                var result = await _serviceBusiness.GetChildPageList(id);
                var treelist = result.Select(x => new TreeViewViewModel()
                {
                    id = x.Id,
                    Name = x.Name,
                    DisplayName = x.Name,
                    ParentId = parentId,
                    hasChildren = true,
                    expanded = true,
                    Type = "generic",
                    children = true,
                    text = x.Name,
                    parent = parentId,
                    a_attr = new { data_id = x.Id, data_name = x.Name, data_type = x.Code, data_order = x.Count },
                });
                list.AddRange(treelist);
            }
            return Json(list.ToList());
        }

        public async Task<ActionResult> ReadBooks(string bookServiceId)
        {
            ViewBag.BookId = bookServiceId;
            return View();
        }
        public async Task<ActionResult> ReadPages(string pageServiceId)
        {
            ViewBag.PageId = pageServiceId;
            return View();
        }
        public async Task<ActionResult> ReadAllBooks(string serviceId)
        {
            var bookslist = await _serviceBusiness.GetAllProcessBook("TEC_PROCESS_GROUP");
            return Json(bookslist.Where(x => x.ServiceId != serviceId).ToList());
        }
        public async Task<ActionResult> ReadAllPages(string serviceId)
        {
            var pageslist = await _serviceBusiness.GetAllBookPages();
            return Json(pageslist.Where(x => x.ServiceId != serviceId).ToList());
        }
        [HttpPost]
        public async Task<ActionResult> ManageBookRelation(string data, string serviceId)
        {
            if (data.IsNotNullAndNotEmpty())
            {
                var obj = JsonConvert.DeserializeObject<List<BookRealtionViewModel>>(data);
                var service = await _serviceBusiness.GetSingleById(serviceId);
                var pagecol = await _serviceBusiness.GetAllBookRelationBySourceId(service.UdfNoteTableId);
                var existingIds = pagecol.Select(x => x.Id);
                var newIds = obj.Select(x => x.Id);
                var ToDelete = existingIds.Except(newIds).ToList();
                var ToAdd = newIds.Except(existingIds).ToList();
                var ToEdit = newIds.Intersect(existingIds).ToList();
                foreach (var col1 in obj)
                {
                    if (ToAdd.Any(x => x == col1.Id))
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.TemplateCode = "BOOK_RELATION";
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        notemodel.OwnerUserId = _userContext.UserId;
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        if (serviceId.IsNotNullAndNotEmpty())
                        {

                            ((IDictionary<String, Object>)exo).Add("SourceTableId", service.UdfNoteTableId);
                        }
                    ((IDictionary<String, Object>)exo).Add("TargetTableId", col1.Id);
                        ((IDictionary<String, Object>)exo).Add("SourceTableType", NtsTypeEnum.Service);
                        ((IDictionary<String, Object>)exo).Add("TargetTableType", NtsTypeEnum.Service);

                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        var result = await _noteBusiness.ManageNote(notemodel);
                    }
                }
                foreach (var col in ToDelete)
                {
                    await _noteBusiness.Delete(col);
                }
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<ActionResult> ManagePageRelation(string data, string serviceId)
        {
            if (data.IsNotNullAndNotEmpty())
            {
                var obj = JsonConvert.DeserializeObject<BookViewModel>(data);

                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.TemplateCode = "PAGE_RELATION";
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.ActiveUserId = _userContext.UserId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.OwnerUserId = _userContext.UserId;
                dynamic exo = new System.Dynamic.ExpandoObject();
                if (serviceId.IsNotNullAndNotEmpty())
                {
                    var service = await _serviceBusiness.GetSingleById(serviceId);
                    ((IDictionary<String, Object>)exo).Add("SourceTableId", service.UdfNoteTableId);
                }
                    ((IDictionary<String, Object>)exo).Add("TargetTableId", obj.Id);
                ((IDictionary<String, Object>)exo).Add("SourceTableType", NtsTypeEnum.Service);
                ((IDictionary<String, Object>)exo).Add("TargetTableType", NtsTypeEnum.Service);



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

        public IActionResult GetRelatedBooks(string bookId)
        {
            List<BookViewModel> list = new List<BookViewModel>();
            list.Add(new BookViewModel
            {
                Id = "1",
                BookName = "Book 1",
                CategoryName = "Category Name 1"

            });
            list.Add(new BookViewModel
            {
                Id = "2",
                BookName = "Book 2",
                CategoryName = "Category Name 2"

            });
            return Json(list);
        }
        //public Byte[] onBookDownload(string bookId)
        //{
        //    Byte[] res = null;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf("<body>Hello world: {0}</body>", PdfSharpCore.PageSize.A4);
        //        pdf.Save(ms);
        //        res = ms.ToArray();
        //    }
        //    return res;
        //}
        //public async Task<IActionResult> onBookDownload(string bookId, string pageId)
        //{
        //    var config = new PdfGenerateConfig();
        //    config.PageOrientation = (PdfSharpCore.PageOrientation)PageOrientation.Landscape;
        //    config.PageSize = PdfSharpCore.PageSize.A4;
        //    config.MarginTop = 10;
        //    config.MarginBottom = 10;
        //    config.MarginLeft = 10;
        //    config.MarginRight = 10;


        //    Byte[] res = null;
        //    //         var html = @"<body>
        //    // <h1> Example of Embedded Style </h1>
        //    //<p> First paragraph.</p> 
        //    //   </body> ";
        //    //         var css = @"<style>
        //    //     body {
        //    //             background-color:red;
        //    //         }

        //    //         h1 {
        //    //         color:red;
        //    //             font-family:arial;
        //    //         }

        //    //         p {
        //    //         color:gray;
        //    //             font-family:verdana;
        //    //         }
        //    // </style> ";
        //    var html = "";
        //    var css = "";
        //    var list = await _serviceBusiness.GetBookAllPagesByBookId(bookId);
        //    if (pageId.IsNotNullAndNotEmpty())
        //    {
        //        list = list.Where(x => x.Id == pageId).ToList();

        //    }
        //    var Content = new StringBuilder("");
        //    foreach (var item in list)
        //    {
        //        Content.Append(@$"<div style='text-align:center'><h2>{item.SequenceOrder}.{item.BookName}</h2></div></br><div>{item.BookDescription}</div><hr></br>");
        //        if (item.HtmlContent.IsNotNullAndNotEmpty())
        //        {
        //            item.HtmlContent = item.HtmlContent.Trim('"').Replace("\\n", "").Replace("\\r", "");
        //        }
        //        Content.Append(item.HtmlContent);
        //        Content.Append(@$"<hr>");
        //        if (item.HtmlCssField.IsNotNullAndNotEmpty())
        //        {
        //            css += item.HtmlCssField.TrimStart('"').TrimEnd('"');
        //        }
        //    }
        //    css = @$"<style>{css}</style>";
        //    CssData cssdata = PdfGenerator.ParseStyleSheet(css);
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        //var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharpCore.PageSize.A4,20, cssdata);
        //        var pdf = PdfGenerator.GeneratePdf(Content.ToString(), config, cssdata);

        //        pdf.Save(ms);
        //        res = ms.ToArray();
        //    }



        //    if (res != null)
        //    {
        //        return File(res, "application /oc-stream", "Book.pdf");
        //    }
        //    return new EmptyResult();
        //}

        [HttpPost]
        public async Task<ActionResult> DeleteServiceCategory(string serviceId, string parentId)
        {
            var service = await _serviceBusiness.GetSingleById(serviceId);
            await _serviceBusiness.Delete(serviceId);
            var template = await _templateBusiness.GetSingleById(service.TemplateId);
            var model = new ServiceTemplateViewModel()
            {
                UdfTableMetadataId = template.UdfTableMetadataId,
                UdfNoteId = service.UdfNoteId
            };
            await _serviceBusiness.DeleteService(model);
            await _serviceBusiness.UpdateCategorySequenceOrderOnDelete(parentId, service.SequenceOrder, serviceId, service.TemplateCode);
            return Json(new { success = true });
        }
        public async Task<ActionResult> AddExistingPage(string categoryId, string bookId, long? sequenceOrder, bool actualPages)
        {
            ViewBag.CategoryId = categoryId;
            ViewBag.BookId = bookId;
            ViewBag.SequenceOrder = sequenceOrder;
            ViewBag.ActualPages = actualPages;
            return View();
        }

        public async Task<ActionResult> GetCategoryById(string categoryId)
        {
            //var where = $@" and ""N_SNC_TECProcess_ProcessCategory"".""Id"" = '{categoryId}'";
            var data = await _cmsBusiness.GetDataListByTemplate("SN_TEC_PROCESS_CATEGORY", "", null);
            return Json(data);
        }
        public async Task<ActionResult> GetBookByCategoryId(string categoryId)
        {
            //var where = $@" and ""N_SNC_TECProcess_ProcessGroup"".""Id"" = '{bookId}'";
            var data = await _serviceBusiness.GetGroupBookByCategoryId(categoryId);
            return Json(data);
        }
        public async Task<ActionResult> GetAllPages(string bookId, bool actualPages)
        {
            if (actualPages)
            {
                var data = await _serviceBusiness.GetBookAllDirectPages(bookId);
                return Json(data);
            }
            else
            {
                var data = await _serviceBusiness.GetBookAllPages(bookId);
                return Json(data);
            }
        }
        [HttpPost]
        public async Task<ActionResult> CreateBookPageMapping(string bookId, string pageId, long? sequenceOrder)
        {
            await _serviceBusiness.CreateBookPageMapping(pageId, bookId, sequenceOrder);
            return Json(new { success = true });
        }

        public async Task<ActionResult> DeleteBokPageMapping(string bookId, string pageId)
        {
            await _serviceBusiness.DeleteBokPageMapping(bookId, pageId);
            return Json(new { success = true });
        }

        public IActionResult ServiceListDashboard(string categoryCodes, bool enableCreate = false)
        {
            //var templates = await _templateBusiness.GetTemplateServiceList(null, categoryCodes, null, null, null, TemplateCategoryTypeEnum.Standard);

            //string[] codes = templates.Select(x => x.Code).ToArray();
            //var templatecodes = string.Join(",", codes);
            //ViewBag.TemplateCodes = templatecodes;
            ViewBag.CategoryCodes = categoryCodes;
            ViewBag.EnableCreate = enableCreate;
            //var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
            //ViewBag.ReturnUrl = cs.Value;
            return View();
        }

        public async Task<IActionResult> ServiceListDashboardWithDepartment(string categoryCodes, string templateCodes, string portalNames, string moduleCodes, bool enableCreate = false)
        {
            ViewBag.CategoryCodes = categoryCodes;
            if (categoryCodes.IsNotNullAndNotEmpty() && templateCodes.IsNullOrEmpty())
            {
                var templates = await _templateBusiness.GetTemplateServiceList(null, categoryCodes, moduleCodes, null, null, TemplateCategoryTypeEnum.Standard, false, portalNames, ServiceTypeEnum.StandardService, null);
                templates = templates.OrderBy(x => x.DisplayName).ToList();

                string[] codes = templates.Select(x => x.Code).ToArray();
                templateCodes = string.Join(",", codes);
            }
            ViewBag.TemplateCodes = templateCodes;
            ViewBag.ModuleCodes = moduleCodes;
            ViewBag.PortalNames = portalNames;
            ViewBag.EnableCreate = enableCreate;
            ViewBag.PortalId = _userContext.PortalId;
            return View();
        }

        public async Task<IActionResult> ServiceSLADashboard(string categoryCodes, string templateCodes, string portalNames, string moduleCodes)
        {
            ViewBag.CategoryCodes = categoryCodes;
            //ViewBag.TemplateCodes = templateCodes;
            ViewBag.PortalNames = portalNames;
            ViewBag.ModuleCodes = moduleCodes;

            var templates = await _templateBusiness.GetTemplateServiceList(templateCodes, categoryCodes, null, null, null, TemplateCategoryTypeEnum.Standard, false, portalNames, ServiceTypeEnum.StandardService, null);
            templates = templates.OrderBy(x => x.DisplayName).ToList();

            string[] codes = templates.Select(x => x.Code).ToArray();
            ViewBag.TemplateCodes = string.Join(",", codes);

            ViewBag.Templates = codes;

            var temlist = new List<TemplateViewModel>();

            foreach (var temp in templates)
            {
                var tempmodel = new TemplateViewModel()
                {
                    Name = temp.DisplayName,
                    Code = temp.Code
                };
                temlist.Add(tempmodel);
            }

            var model = new TemplateViewModel()
            {
                TemplateCodes = ViewBag.TemplateCodes,
                Templates = ViewBag.Templates,
                TemplatesList = temlist,
            };


            return View(model);
        }

        public async Task<IActionResult> ReadServiceListCount(string categoryCodes)
        {
            var result = await _serviceBusiness.GetServiceCountByServiceTemplateCodes(categoryCodes, _userContext.PortalId);
            var j = Json(result);
            return j;
        }

        public async Task<IActionResult> ReadServiceListCountWithDepartment(string categoryCodes = null, string templateCodes = null, string moduleCodes = null, string portalNames = null, string departmentId = null, string templateId = null, string userId = null)
        {
            var result = await _serviceBusiness.GetServiceListCountWithDepartment(categoryCodes, templateCodes, moduleCodes, portalNames, departmentId, templateId, userId);
            var j = Json(result);
            return j;
        }

        public async Task<IActionResult> ReadServiceSLAListCount(string categoryCodes, string templateCodes, string portalNames, string moduleCodes)
        {
            var result = await _serviceBusiness.GetServiceCountByDifferentCodes(categoryCodes, templateCodes, portalNames, moduleCodes);
            var j = Json(result);
            return j;
        }

        public async Task<IActionResult> ReadServiceSLAData(string categoryCodes, string serviceStatus, string templateCode)
        {
            var list = await _serviceBusiness.GetServiceListByServiceCategoryCodes(categoryCodes, serviceStatus, _userContext.PortalId);
            //List<ServiceViewModel> nList = new();
            if (templateCode.IsNotNullAndNotEmpty())
            {
                list = list.Where(x => x.TemplateCode == templateCode).ToList();
            }
            return Json(list);
        }

        public async Task<IActionResult> ReadServiceData(string categoryCodes, string serviceStatus)
        {
            var list = await _serviceBusiness.GetServiceListByServiceCategoryCodes(categoryCodes, serviceStatus, _userContext.PortalId);
            //var j = Json(list.ToDataSourceResult(request));
            var j = Json(list);
            return j;
        }

        public async Task<IActionResult> ReadServiceDataWithDepartment(string categoryCodes, string templateCodes, string portalNames, string moduleCodes, string serviceStatus, string departmentId = null, string templateId = null, string userId = null)
        {
            var list = await _serviceBusiness.GetServiceListWithDepartment(categoryCodes, templateCodes, portalNames, moduleCodes, serviceStatus, departmentId, templateId, userId);

            var j = Json(list);
            return j;
        }

        public IActionResult SEBIInvestorLandingPage()
        {
            return View();
        }
        public IActionResult SEBIInvestorEntityStatus()
        {
            return View();
        }
        public async Task<ActionResult> GetSEBIInvestorListedCompanyIdNameList(string entityType)
        {
            var list = await _serviceBusiness.GetSEBIInvestorListedCompanyIdNameList(entityType);
            var j = Json(list);
            return j;
        }
        public async Task<ActionResult> GetSEBIInvestorListedCompanyData(string listedCompanyId)
        {
            var querydata = await _serviceBusiness.GetSEBIInvestorListedCompanyData(listedCompanyId);
            var flag = false;
            if (querydata.IsNotNull())
            {
                flag = true;
            }
            var j = Json(new { success = flag, data = querydata });
            return j;
        }
        public async Task<IActionResult> SEBIInvestorLogin(string returnUrl = "", bool eSEBIInvestorLayoutNull = false)
        {
            var portal = await _cmsBusiness.GetSingleGlobal<PortalViewModel, Portal>(x => x.Name == "SEBIInvestor");
            if (returnUrl.IsNullOrEmpty())
            {
                returnUrl = "/portal/SEBIInvestor";
            }
            return RedirectToAction("login", "account", new { @area = "", @portalId = portal?.Id, @returnUrl = returnUrl, @eGovLayoutNull = eSEBIInvestorLayoutNull });
        }

        public IActionResult TemplatesListWithServiceCount(string templateCodes = null, string catCodes = null, string groupCodes = null, bool showAllServicesForAdmin = false, bool showAllTask = false)
        {
            var model = new TemplateViewModel()
            {
                TemplateCodes = templateCodes,
                CategoryCodes = catCodes,
                GroupCodes = groupCodes,
                ShowAllServicesForAdmin = showAllServicesForAdmin,
                UserId = _userContext.UserId
            };
            if (showAllTask)
            {
                model.UserId = null;
            }
            else if (showAllServicesForAdmin && _userContext.IsSystemAdmin)
            {
                model.UserId = null;
            }
            return View(model);
        }
        public async Task<IActionResult> ReadTemplatesListWithServiceCount(string templateCodes = null, string catCodes = null, string groupCodes = null, bool showAllServicesForAdmin = false)
        {
            var dt = await _serviceBusiness.GetTemplatesListWithServiceCount(templateCodes, catCodes, groupCodes, showAllServicesForAdmin);
            return Json(dt);
        }


        public async Task<IActionResult> ServiceSLADashboard2(string categoryCodes, string templateCodes, string portalNames, string moduleCodes)
        {
            return View();
        }

        public async Task<IActionResult> GetSeriesData()
        {
            List<ServiceSLAViewModel> list = new();
            list.Add(new ServiceSLAViewModel()
            {
                DepartmentId = "1",
                DueDate = DateTime.Today.Date,
                TemplateCode = "A"
            });
            list.Add(new ServiceSLAViewModel()
            {
                DepartmentId = "2",
                DueDate = DateTime.Today.Date.AddDays(3),
                TemplateCode = "B"
            });
            list.Add(new ServiceSLAViewModel()
            {
                DepartmentId = "3",
                DueDate = DateTime.Today.Date,
                TemplateCode = "C"
            });
            list.Add(new ServiceSLAViewModel()
            {
                DepartmentId = "4",
                DueDate = DateTime.Today.Date.AddDays(2),
                TemplateCode = "D"
            });
            list.Add(new ServiceSLAViewModel()
            {
                DepartmentId = "5",
                DueDate = DateTime.Today.Date.AddDays(2),
                TemplateCode = "A"
            });
            list.Add(new ServiceSLAViewModel()
            {
                DepartmentId = "6",
                DueDate = DateTime.Today.Date.AddDays(1),
                TemplateCode = "B"
            });
            list.Add(new ServiceSLAViewModel()
            {
                DepartmentId = "6",
                DueDate = DateTime.Today.Date.AddDays(2),
                TemplateCode = "C"
            });
            list.Add(new ServiceSLAViewModel()
            {
                DepartmentId = "4",
                DueDate = DateTime.Today.Date,
                TemplateCode = "D"
            });
            list.Add(new ServiceSLAViewModel()
            {
                DepartmentId = "4",
                DueDate = DateTime.Today.Date.AddDays(45),
                TemplateCode = "D"
            });


            var dueDateList = list.Select(x => x.DueDate).Distinct().ToList();
            List<List<ServiceSLAViewModel>> newlist = new();
            var seriesA = list.Where(x => x.TemplateCode == "A").ToList();
            var seriesB = list.Where(x => x.TemplateCode == "B").ToList();
            var seriesC = list.Where(x => x.TemplateCode == "C").ToList();
            var seriesD = list.Where(x => x.TemplateCode == "D").ToList();
            newlist.Add(seriesA);
            newlist.Add(seriesB);
            newlist.Add(seriesC);
            newlist.Add(seriesD);
            return Json(new { seriesList = newlist, dueDateList = dueDateList }); ;

        }

        public async Task<IActionResult> ServiceTemplateTilesForJammu(string templateCode, string categoryCode, string userId, string moduleCodes, string prms, string cbm, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalId = null)
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
            if (portalId.IsNotNullAndNotEmpty())
            {

                var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(portalId);
                if (portal?.Name == "EGovCustomer")
                {
                    var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
                    ViewBag.SmartCityUrl = cs.Value;
                }
            }
            return View(model);
        }

    }
}
