using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class TemplateController : Controller
    {
        private readonly IStringLocalizer<Synergy.App.WebUtility.SharedResource> _localizer;
        IUserContext _userContext;
        ITemplateCategoryBusiness _categoryBusiness;
        ITemplateBusiness _templateBusiness;
        INoteTemplateBusiness _noteTemplateBusiness;
        ITaskTemplateBusiness _taskTemplateBusiness;
        IServiceTemplateBusiness _serviceTemplateBusiness;
        IFormTemplateBusiness _formTemplateBusiness;
        IPageTemplateBusiness _pageTemlateBusiness;
        ICustomTemplateBusiness _customTemplateBusiness;
        IFormIndexPageTemplateBusiness _formIndexPageTemplateBusiness;
        IFormIndexPageColumnBusiness _formIndexPageColumnBusiness;
        INoteIndexPageTemplateBusiness _noteIndexPageTemplateBusiness;
        INtsLogPageColumnBusiness _ntsLogPageColumnBusiness;
        INoteIndexPageColumnBusiness _noteindexPageColumnBusiness;
        ITaskIndexPageTemplateBusiness _taskIndexPageTemplateBusiness;
        ITaskIndexPageColumnBusiness _taskindexPageColumnBusiness;
        IServiceIndexPageTemplateBusiness _serviceIndexPageTemplateBusiness;
        IServiceIndexPageColumnBusiness _serviceIndexPageColumnBusiness;
        ITableMetadataBusiness _tableMetadataBusiness;
        IColumnMetadataBusiness _columnMetadataBusiness;
        INotificationTemplateBusiness _notificationTemplateBusiness;
        IProcessDesignBusiness _processDesignTemplateBusiness;
        IFileBusiness _fileBusiness;
        ILOVBusiness _lovBusiness;
        ICustomIndexPageColumnBusiness _customindexPageColumnBusiness;
        ICustomIndexPageTemplateBusiness _customindexPageTemplateBusiness;
        IBusinessRuleBusiness _businessRuleBusiness;
        private readonly IWebHelper _webApi;
        private readonly IServiceProvider _serviceProvider;
        IRecTaskTemplateBusiness _recTaskTemplateBusiness;
        ITaskBusiness _taskBusiness;
        INoteBusiness _noteBusiness;
        IServiceBusiness _serviceBusiness;
        IResourceLanguageBusiness _resourceLanguageBusiness;
        IOCRMappingBusiness _ocrMappingBusiness;
        public TemplateController(IUserContext userContext,
            ITemplateCategoryBusiness categoryBusiness,
        ITemplateBusiness templateBusiness,
        INoteTemplateBusiness noteTemplateBusiness,
        ITaskTemplateBusiness taskTemplateBusiness,
        IServiceTemplateBusiness serviceTemplateBusiness,
        IFormTemplateBusiness formTemplateBusiness,
        IPageTemplateBusiness pageTemlateBusiness,
        ICustomTemplateBusiness customTemplateBusiness,
        IFormIndexPageTemplateBusiness IndexPageBusiness,
        ITableMetadataBusiness TableMetadataBusiness,
        IColumnMetadataBusiness columnMetadataBusiness,
        IFormIndexPageColumnBusiness indexPageColumnBusiness,
        INotificationTemplateBusiness notificationTemplateBusiness,
        INoteIndexPageTemplateBusiness noteIndexPageBusiness,
        INoteIndexPageColumnBusiness noteindexPageColumnBusiness,
        ITaskIndexPageTemplateBusiness taskIndexPageBusiness,
        ITaskIndexPageColumnBusiness taskindexPageColumnBusiness,
        IServiceIndexPageTemplateBusiness serviceIndexPageBusiness,
        IServiceIndexPageColumnBusiness serviceindexPageColumnBusiness,
        ICustomIndexPageColumnBusiness customindexPageColumnBusiness,
        ICustomIndexPageTemplateBusiness customindexPageTemplateBusiness,
        IProcessDesignBusiness processDesignTemplateBusiness,
        IFileBusiness fileBusiness,
        ILOVBusiness lovBusiness, INoteBusiness noteBusiness, IServiceBusiness serviceBusiness,
        IBusinessRuleBusiness businessRuleBusiness,
        IWebHelper webApi, IRecTaskTemplateBusiness recTaskTemplateBusiness
            , IServiceProvider serviceProvider, ITaskBusiness taskBusiness
            , IStringLocalizer<Synergy.App.WebUtility.SharedResource> localizer
            , IResourceLanguageBusiness resourceLanguageBusiness
            , INtsLogPageColumnBusiness ntsLogPageColumnBusiness
            , IOCRMappingBusiness ocrMappingBusiness
            )
        {
            _userContext = userContext;
            _categoryBusiness = categoryBusiness;
            _templateBusiness = templateBusiness;
            _noteTemplateBusiness = noteTemplateBusiness;
            _taskTemplateBusiness = taskTemplateBusiness;
            _serviceTemplateBusiness = serviceTemplateBusiness;
            _formTemplateBusiness = formTemplateBusiness;
            _pageTemlateBusiness = pageTemlateBusiness;
            _customTemplateBusiness = customTemplateBusiness;
            _formIndexPageTemplateBusiness = IndexPageBusiness;
            _tableMetadataBusiness = TableMetadataBusiness;
            _columnMetadataBusiness = columnMetadataBusiness;
            _formIndexPageColumnBusiness = indexPageColumnBusiness;
            _notificationTemplateBusiness = notificationTemplateBusiness;
            _processDesignTemplateBusiness = processDesignTemplateBusiness;
            _serviceIndexPageColumnBusiness = serviceindexPageColumnBusiness;
            _serviceIndexPageTemplateBusiness = serviceIndexPageBusiness;
            _noteIndexPageTemplateBusiness = noteIndexPageBusiness;
            _noteindexPageColumnBusiness = noteindexPageColumnBusiness;
            _taskIndexPageTemplateBusiness = taskIndexPageBusiness;
            _taskindexPageColumnBusiness = taskindexPageColumnBusiness;
            _fileBusiness = fileBusiness;
            _lovBusiness = lovBusiness;
            _businessRuleBusiness = businessRuleBusiness;
            _webApi = webApi;
            _recTaskTemplateBusiness = recTaskTemplateBusiness;
            _serviceProvider = serviceProvider;
            _taskBusiness = taskBusiness;
            _noteBusiness = noteBusiness;
            _serviceBusiness = serviceBusiness;
            _localizer = localizer;
            _resourceLanguageBusiness = resourceLanguageBusiness;
            _ntsLogPageColumnBusiness = ntsLogPageColumnBusiness;
            _customindexPageColumnBusiness = customindexPageColumnBusiness;
            _customindexPageTemplateBusiness = customindexPageTemplateBusiness;
            _ocrMappingBusiness = ocrMappingBusiness;
        }
        [Authorize(Policy = nameof(AuthorizeCMS))]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NewIndex()
        {
            return View();
        }
        public IActionResult Chart()
        {
            return View(new FlowchartViewModel());
        }

        public async Task<IActionResult> GetChart(string processdesignId)
        {
            var list = new List<ComponentViewModel>();
            list.Add(new ComponentViewModel { Id = "1", ParentId = null, Name = "Start", ComponentType = ProcessDesignComponentTypeEnum.Start });
            list.Add(new ComponentViewModel { Id = "2", ParentId = "1", Name = "Step Task", ComponentType = ProcessDesignComponentTypeEnum.StepTask });
            list.Add(new ComponentViewModel { Id = "3", ParentId = "1", Name = "Execution Script", ComponentType = ProcessDesignComponentTypeEnum.ExecutionScript });
            list.Add(new ComponentViewModel { Id = "4", ParentId = "3", Name = "Stop", ComponentType = ProcessDesignComponentTypeEnum.Stop });
            list.Add(new ComponentViewModel { Id = "5", ParentId = "2", Name = "Decision Script", ComponentType = ProcessDesignComponentTypeEnum.DecisionScript });
            list.Add(new ComponentViewModel { Id = "6", ParentId = "5", Name = "True", ComponentType = ProcessDesignComponentTypeEnum.True });
            list.Add(new ComponentViewModel { Id = "6", ParentId = "5", Name = "False", ComponentType = ProcessDesignComponentTypeEnum.False });

            return await Task.FromResult(Json(list));
        }
        public async Task<IActionResult> Template(string templateId, string categoryId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null, string portalId = null, string source = null)
        {
            TemplateViewModel model = new TemplateViewModel();
            if (ModelState.IsValid)
            {


                if (templateId != null)
                {
                    model = await _templateBusiness.GetSingleById(templateId);
                    model.DataAction = DataActionEnum.Edit;
                    var category = await _categoryBusiness.GetSingleById(model.TemplateCategoryId);
                    ViewBag.TemplateType = category.TemplateType;
                    model.TemplateCategoryName = category.Name;
                    // ViewBag.TemplateType = type;
                }
                else
                {
                    model.DataAction = DataActionEnum.Create;
                    var category = await _categoryBusiness.GetSingleById(categoryId);
                    ViewBag.TemplateType = category.TemplateType;
                    model.TemplateType = category.TemplateType;
                    model.TemplateCategoryName = category.Name;
                    model.TemplateCategoryId = categoryId;
                    model.Id = Guid.NewGuid().ToString();
                    model.TemplateStatus = TemplateStatusEnum.Draft;
                    model.PortalId = portalId;
                }
                ViewBag.EnableIndexPage = true;
                ViewBag.EnableFormIndex = false;
                ViewBag.EnableNoteIndex = false;
                ViewBag.EnableTaskIndex = false;
                ViewBag.EnableServiceIndex = false;
                ViewBag.EnableProcessDesignIndex = false;
                if (ViewBag.TemplateType == TemplateTypeEnum.Form)
                {
                    var formTemplateDetails = await _formTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
                    if (formTemplateDetails != null)
                    {
                        ViewBag.EnableFormIndex = formTemplateDetails.EnableIndexPage;
                    }
                }
                else if (ViewBag.TemplateType == TemplateTypeEnum.Note)
                {
                    var noteTemplateDetails = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
                    if (noteTemplateDetails != null)
                    {
                        ViewBag.EnableNoteIndex = noteTemplateDetails.EnableIndexPage;
                    }
                }
                else if (ViewBag.TemplateType == TemplateTypeEnum.Task)
                {
                    var taskTemplateDetails = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
                    if (taskTemplateDetails != null)
                    {
                        ViewBag.EnableTaskIndex = taskTemplateDetails.EnableIndexPage;
                    }
                }
                else if (ViewBag.TemplateType == TemplateTypeEnum.Service)
                {
                    var serviceTemplateDetails = await _serviceTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
                    if (serviceTemplateDetails != null)
                    {
                        ViewBag.EnableServiceIndex = serviceTemplateDetails.EnableIndexPage;
                    }

                }
                else if (ViewBag.TemplateType == TemplateTypeEnum.ProcessDesign)
                {


                }

            }
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
            }
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            ViewBag.Source = source;
            return View(model);
        }
        public async Task<IActionResult> DeleteTemplate(string id)
        {
            await _templateBusiness.DeleteTemplate(id);
            return Json(new { success = true });
        }

        public IActionResult PreviewForm(long templateId)
        {
            var model = new SynergyTemplateViewModel { TemplateId = templateId };

            return View(model);
        }

        //public async Task<IActionResult> ManageTemplateGeneralTabData()
        //{
        //    var model = new TemplateViewModel();
        //    model.DataAction = DataActionEnum.Create;
        //    return View("Template", model);
        //}

        [HttpPost]
        public async Task<IActionResult> ManageTemplateGeneralTabData(TemplateViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    //if (model.TableMetadataId.IsNotNullAndNotEmpty())
                    //{
                    //    model.TableSelectionType = TableSelectionTypeEnum.Existing;
                    //}
                    var result = await _templateBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        return Json(new { success = true, templateId = result.Item.Id });
                        //return PopupRedirect("Style created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _templateBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        return Json(new { success = true, templateId = result.Item.Id });
                        //return PopupRedirect("Style edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            //return RedirectToAction("Index");
            // return View("Index", new SynergyTemplateViewModel());
        }

        [HttpGet]
        public async Task<JsonResult> GetTableMetaDataList(string portalId)
        {
            //var data = await _tableMetadataBusiness.GetList(x=>x.TableType == TableTypeEnum.View)
            var data = await _tableMetadataBusiness.GetList();
            if (portalId.IsNotNullAndNotEmpty())
            {
                return Json(data.Where(x => x.PortalId == portalId));
            }
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetNoteTemplateList(string portalId)
        {
            //var data = await _tableMetadataBusiness.GetList(x=>x.TableType == TableTypeEnum.View)
            var data = await _templateBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Note);
            if (portalId.IsNotNullAndNotEmpty())
            {
                return Json(data.Where(x => x.PortalId == portalId));
            }
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplateJson(string templateId)
        {
            var res = "";
            var pagedata = await _templateBusiness.GetSingleById(templateId);
            if (pagedata != null)
            {
                if (pagedata.Json.IsNotNullAndNotEmpty())
                {
                    res = _webApi.AddHost(pagedata.Json);
                    var tableMetadataId = pagedata.TableMetadataId != null && pagedata.TableMetadataId != "" ? pagedata.TableMetadataId : pagedata.UdfTemplateId;
                    //var comps = JObject.Parse(res);
                    //foreach(var comp in comps["components"])
                    //{
                    //    var component = comp as JObject;
                    //    component.Property("key").AddAfterSelf(new JProperty("tableMetadataId", tableMetadataId));
                    //}
                    //res = comps.ToString();
                }
                else
                {
                    //if (pagedata.TableSelectionType == TableSelectionTypeEnum.Existing)
                    //{
                    //    res = "{tableMetadataId=" + pagedata.TableMetadataId ?? pagedata.UdfTemplateId + "}";
                    //}
                }
            }
            else
            {
                //if (pagedata.TableSelectionType == TableSelectionTypeEnum.Existing)
                //{
                //    res = "{tableMetadataId=" + pagedata.TableMetadataId ?? pagedata.UdfTemplateId + "}";
                //}
            }
            return Json(res);
        }
        public async Task<JsonResult> GetNtsTemplateTreeList(string id)
        {
            //var result = await _seetingsBusiness.GetDocumentTypeTreeList(id);
            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                //list.Add(new TreeViewViewModel
                //{
                //    id = TemplateTypeEnum.FormIndexPage.ToString(),
                //    Name = TemplateTypeEnum.FormIndexPage.ToString(),
                //    DisplayName = TemplateTypeEnum.FormIndexPage.ToString(),
                //    ParentId = null,
                //    hasChildren = true,
                //    expanded = true,
                //    Type = "Root",

                //});
                list.Add(new TreeViewViewModel
                {
                    id = TemplateTypeEnum.Page.ToString(),
                    Name = TemplateTypeEnum.Page.ToString(),
                    DisplayName = TemplateTypeEnum.Page.ToString(),
                    ParentId = null,
                    hasChildren = true,
                    expanded = false,
                    Type = "Root",
                    children = true,
                    text = TemplateTypeEnum.Page.ToString(),
                    parent = "#",
                    a_attr = new { data_id = TemplateTypeEnum.Page.ToString(), data_name = TemplateTypeEnum.Page.ToString(), data_type = "Root" },
                });
                list.Add(new TreeViewViewModel
                {
                    id = TemplateTypeEnum.Form.ToString(),
                    Name = TemplateTypeEnum.Form.ToString(),
                    DisplayName = TemplateTypeEnum.Form.ToString(),
                    ParentId = null,
                    hasChildren = true,
                    expanded = false,
                    Type = "Root",
                    children = true,
                    text = TemplateTypeEnum.Form.ToString(),
                    parent = "#",
                    a_attr = new { data_id = TemplateTypeEnum.Form.ToString(), data_name = TemplateTypeEnum.Form.ToString(), data_type = "Root" },

                });
                list.Add(new TreeViewViewModel
                {
                    id = TemplateTypeEnum.Note.ToString(),
                    Name = TemplateTypeEnum.Note.ToString(),
                    DisplayName = TemplateTypeEnum.Note.ToString(),
                    ParentId = null,
                    hasChildren = true,
                    expanded = false,
                    Type = "Root",
                    children = true,
                    text = TemplateTypeEnum.Note.ToString(),
                    parent = "#",
                    a_attr = new { data_id = TemplateTypeEnum.Note.ToString(), data_name = TemplateTypeEnum.Note.ToString(), data_type = "Root" },

                });
                list.Add(new TreeViewViewModel
                {
                    id = TemplateTypeEnum.Task.ToString(),
                    Name = TemplateTypeEnum.Task.ToString(),
                    DisplayName = TemplateTypeEnum.Task.ToString(),
                    ParentId = null,
                    hasChildren = true,
                    expanded = false,
                    Type = "Root",
                    children = true,
                    text = TemplateTypeEnum.Task.ToString(),
                    parent = "#",
                    a_attr = new { data_id = TemplateTypeEnum.Task.ToString(), data_name = TemplateTypeEnum.Task.ToString(), data_type = "Root" },

                });
                //list.Add(new TreeViewViewModel
                //{
                //    id = TemplateTypeEnum.Service.ToString(),
                //    Name = TemplateTypeEnum.Service.ToString(),
                //    DisplayName = TemplateTypeEnum.Service.ToString(),
                //    ParentId = null,
                //    hasChildren = true,
                //    expanded = true,
                //    Type = "Root"
                //});
                list.Add(new TreeViewViewModel
                {
                    id = TemplateTypeEnum.Service.ToString(),
                    Name = TemplateTypeEnum.Service.ToString(),
                    DisplayName = TemplateTypeEnum.Service.ToString(),
                    ParentId = null,
                    hasChildren = true,
                    expanded = false,
                    Type = "Root",
                    children = true,
                    text = TemplateTypeEnum.Service.ToString(),
                    parent = "#",
                    a_attr = new { data_id = TemplateTypeEnum.Service.ToString(), data_name = TemplateTypeEnum.Service.ToString(), data_type = "Root" },
                });
                list.Add(new TreeViewViewModel
                {
                    id = TemplateTypeEnum.Custom.ToString(),
                    Name = TemplateTypeEnum.Custom.ToString(),
                    DisplayName = TemplateTypeEnum.Custom.ToString(),
                    ParentId = null,
                    hasChildren = true,
                    expanded = false,
                    Type = "Root",
                    children = true,
                    text = TemplateTypeEnum.Custom.ToString(),
                    parent = "#",
                    a_attr = new { data_id = TemplateTypeEnum.Custom.ToString(), data_name = TemplateTypeEnum.Custom.ToString(), data_type = "Root" },

                });
            }
            else if (id == TemplateTypeEnum.Note.ToString()
                || id == TemplateTypeEnum.Task.ToString() || id == TemplateTypeEnum.Service.ToString()
                || id == TemplateTypeEnum.Form.ToString() || id == TemplateTypeEnum.Page.ToString()
                || id == TemplateTypeEnum.FormIndexPage.ToString() || id == TemplateTypeEnum.Custom.ToString() || id == TemplateTypeEnum.ProcessDesign.ToString())
            {
                TemplateTypeEnum type = id.ToEnum<TemplateTypeEnum>();
                var category = await _categoryBusiness.GetList(x => x.TemplateType == type && (x.ParentId == null || x.ParentId == ""));
                // category = category.Where().ToList();
                list.AddRange(category.Select(x => new TreeViewViewModel
                {
                    id = x.Id.ToString(),
                    Name = x.Name,
                    DisplayName = x.Name,
                    ParentId = id,
                    hasChildren = true,
                    expanded = false,
                    Type = "Category",
                    children = true,
                    text = x.Name,
                    parent = id,
                    a_attr = new { data_id = x.Id.ToString(), data_name = x.Name, data_type = "Category", data_parentId = id },

                }));

                list = list.OrderBy(x => x.Name).ToList();
            }

            //if ((!id.IsNullOrEmpty()) && (id != TemplateTypeEnum.Note.ToString()
            //    && id != TemplateTypeEnum.Task.ToString() && id != TemplateTypeEnum.Service.ToString()
            //    && id != TemplateTypeEnum.Form.ToString() && id != TemplateTypeEnum.Page.ToString()
            //    && id != TemplateTypeEnum.IndexPage.ToString() && id != TemplateTypeEnum.Custom.ToString()
            //   ))
            else
            {
                var cb = _serviceProvider.GetService<IComponentResultBusiness>();
                var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == id);
                var stepTasks = await cb.GetStepTaskTemplateList(id);
                var category = await _categoryBusiness.GetList(x => x.ParentId == id);
                if (stepTasks != null && stepTasks.Any())
                {
                    list.AddRange(templates.Select(x => new TreeViewViewModel
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.DisplayName.Coalesce(x.Name),
                        ParentId = id,
                        hasChildren = true,
                        expanded = false,
                        Type = "Template",
                        TemplateType = x.TemplateType,
                        children = true,
                        text = x.Name,
                        parent = id,
                        a_attr = new { data_id = x.Id.ToString(), data_name = x.Name, data_type = "Template", data_parentId = id },
                    }));
                }
                else
                {
                    list.AddRange(templates.Select(x => new TreeViewViewModel
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.DisplayName.Coalesce(x.Name),
                        ParentId = id,
                        hasChildren = true,
                        expanded = false,
                        Type = "Template",
                        TemplateType = x.TemplateType,
                        text = x.Name,
                        parent = id,
                        a_attr = new { data_id = x.Id.ToString(), data_name = x.Name, data_type = "Template", data_parentId = id },
                    }));
                }
                list.AddRange(category.Select(x => new TreeViewViewModel
                {
                    id = x.Id.ToString(),
                    Name = x.Name,
                    DisplayName = x.Name,
                    ParentId = id,
                    hasChildren = true,
                    expanded = false,
                    Type = "Category",
                    children = true,
                    text = x.Name,
                    parent = id,
                    a_attr = new { data_id = x.Id.ToString(), data_name = x.Name, data_type = "Category", data_parentId = id },

                }));
                if (stepTasks != null && stepTasks.Any())
                {
                    list.AddRange(stepTasks.Select(x => new TreeViewViewModel
                    {
                        id = x.Id.ToString(),
                        Name = x.Name,
                        DisplayName = x.Name,
                        ParentId = id,
                        hasChildren = false,
                        expanded = false,
                        Type = "Template",
                        text = x.Name,
                        parent = id,
                        a_attr = new { data_id = x.Id.ToString(), data_name = x.Name, data_type = "Template", data_parentId = id },

                    }));
                }

                list = list.OrderBy(x => x.Name).ToList();
            }



            return Json(list.ToList());
        }
        public async Task<JsonResult> GetTaskTemplateTreeList(string id, string parentId)
        {

            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {

                list.Add(new TreeViewViewModel
                {
                    id = TemplateTypeEnum.Task.ToString(),
                    Name = TemplateTypeEnum.Task.ToString(),
                    DisplayName = TemplateTypeEnum.Task.ToString(),
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "Root"

                });

            }
            else if (id == TemplateTypeEnum.Note.ToString()
                || id == TemplateTypeEnum.Task.ToString() || id == TemplateTypeEnum.Service.ToString()
                || id == TemplateTypeEnum.Form.ToString() || id == TemplateTypeEnum.Page.ToString()
                || id == TemplateTypeEnum.FormIndexPage.ToString() || id == TemplateTypeEnum.Custom.ToString() || id == TemplateTypeEnum.ProcessDesign.ToString())
            {
                TemplateTypeEnum type = id.ToEnum<TemplateTypeEnum>();
                var category = await _categoryBusiness.GetList(x => x.TemplateType == type);
                // category = category.Where().ToList();
                list.AddRange(category.Select(x => new TreeViewViewModel
                {
                    id = x.Id.ToString(),
                    Name = x.Name,
                    DisplayName = x.Name,
                    ParentId = id,
                    hasChildren = true,
                    expanded = false,
                    Type = "Category"

                }));
            }
            else
            {
                var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == id);
                list.AddRange(templates.Select(x => new TreeViewViewModel
                {
                    id = x.Id,
                    Name = x.Name,
                    DisplayName = x.DisplayName.Coalesce(x.Name),
                    ParentId = id,
                    hasChildren = false,
                    expanded = false,
                    Type = "Template",
                    TemplateType = x.TemplateType
                }));
            }
            var result = list.Where(x => x.ParentId == id).Select(item => new
            {
                id = item.id,
                Name = item.Name,
                ParentId = item.ParentId,
                hasChildren = item.hasChildren,
                type = item.Type,
            });


            return Json(result);
        }
        public async Task<JsonResult> GetNoteTemplateTreeList(string id, string parentId)
        {

            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {

                list.Add(new TreeViewViewModel
                {
                    id = TemplateTypeEnum.Note.ToString(),
                    Name = TemplateTypeEnum.Note.ToString(),
                    DisplayName = TemplateTypeEnum.Note.ToString(),
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "Root"

                });

            }
            else if (id == TemplateTypeEnum.Note.ToString()
                || id == TemplateTypeEnum.Task.ToString() || id == TemplateTypeEnum.Service.ToString()
                || id == TemplateTypeEnum.Form.ToString() || id == TemplateTypeEnum.Page.ToString()
                || id == TemplateTypeEnum.FormIndexPage.ToString() || id == TemplateTypeEnum.Custom.ToString() || id == TemplateTypeEnum.ProcessDesign.ToString())
            {
                TemplateTypeEnum type = id.ToEnum<TemplateTypeEnum>();
                var category = await _categoryBusiness.GetList(x => x.TemplateType == type);
                // category = category.Where().ToList();
                list.AddRange(category.Select(x => new TreeViewViewModel
                {
                    id = x.Id.ToString(),
                    Name = x.Name,
                    DisplayName = x.Name,
                    ParentId = id,
                    hasChildren = true,
                    expanded = false,
                    Type = "Category"

                }));
            }
            else
            {
                var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == id);
                list.AddRange(templates.Select(x => new TreeViewViewModel
                {
                    id = x.Id,
                    Name = x.Name,
                    DisplayName = x.DisplayName.Coalesce(x.Name),
                    ParentId = id,
                    hasChildren = false,
                    expanded = false,
                    Type = "Template",
                    TemplateType = x.TemplateType
                }));
            }
            var result = list.Where(x => x.ParentId == id).Select(item => new
            {
                id = item.id,
                Name = item.Name,
                ParentId = item.ParentId,
                hasChildren = item.hasChildren,
                type = item.Type,
            });


            return Json(result);
        }
        public async Task<JsonResult> GetServiceTemplateTreeList(string id, string parentId)
        {

            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {

                list.Add(new TreeViewViewModel
                {
                    id = TemplateTypeEnum.Service.ToString(),
                    Name = TemplateTypeEnum.Service.ToString(),
                    DisplayName = TemplateTypeEnum.Service.ToString(),
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "Root"
                });

            }
            else if (id == TemplateTypeEnum.Note.ToString()
                || id == TemplateTypeEnum.Task.ToString() || id == TemplateTypeEnum.Service.ToString()
                || id == TemplateTypeEnum.Form.ToString() || id == TemplateTypeEnum.Page.ToString()
                || id == TemplateTypeEnum.FormIndexPage.ToString() || id == TemplateTypeEnum.Custom.ToString() || id == TemplateTypeEnum.ProcessDesign.ToString())
            {
                TemplateTypeEnum type = id.ToEnum<TemplateTypeEnum>();
                var category = await _categoryBusiness.GetList(x => x.TemplateType == type);
                // category = category.Where().ToList();
                list.AddRange(category.Select(x => new TreeViewViewModel
                {
                    id = x.Id.ToString(),
                    Name = x.Name,
                    DisplayName = x.Name,
                    ParentId = id,
                    hasChildren = true,
                    expanded = false,
                    Type = "Category"

                }));
            }
            else
            {
                var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == id);
                list.AddRange(templates.Select(x => new TreeViewViewModel
                {
                    id = x.Id,
                    Name = x.Name,
                    DisplayName = x.DisplayName.Coalesce(x.Name),
                    ParentId = id,
                    hasChildren = false,
                    expanded = false,
                    Type = "Template",
                    TemplateType = x.TemplateType
                }));
            }
            var result = list.Where(x => x.ParentId == id).Select(item => new
            {
                id = item.id,
                Name = item.Name,
                ParentId = item.ParentId,
                hasChildren = item.hasChildren,
                type = item.Type,
            });


            return Json(list.ToList());
        }
        public async Task<ActionResult> GetTemplateByType(TemplateTypeEnum type, string portalId)
        {
            return Json(await _templateBusiness.GetTemplateByType(type, portalId));
        }

        public async Task<IActionResult> ManageFormIndex(string templateId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            var model = new FormIndexPageTemplateViewModel
            {
                TemplateId = templateId
            };
            var template = await _templateBusiness.GetSingleById(model.TemplateId);
            if (template != null)
            {
                var indexPageTemplate = await _formIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
                if (indexPageTemplate != null)
                {
                    model = indexPageTemplate;
                    model.TableMetadataId = template.TableMetadataId;
                    model.DataAction = DataActionEnum.Edit;
                    model.SelectedTableRows = await _formIndexPageColumnBusiness.GetList(x => x.FormIndexPageTemplateId == model.Id);
                    ViewBag.RowData = JsonConvert.SerializeObject(model.SelectedTableRows);
                }
                else
                {
                    model.TableMetadataId = template.TableMetadataId;
                    model.DataAction = DataActionEnum.Create;

                    model.CreateButtonText = "Create";
                    model.EditButtonText = "Edit";
                    model.DeleteButtonText = "Delete";
                    model.EnableCreateButton = true;
                    model.EnableEditButton = true;
                    model.EnableDeleteButton = true;
                    model.EnableDeleteConfirmation = true;
                    model.DeleteConfirmationMessage = ApplicationConstant.Messages.DeleteConfirmation;

                    var category = await _categoryBusiness.GetSingleById(template.TemplateCategoryId);

                    if (category.TemplateType == TemplateTypeEnum.Form)
                    {
                        model.IndexPageTemplateType = TemplateTypeEnum.Form;
                        var formtemplate = await _formTemplateBusiness.GetSingle(x => x.TemplateId == model.TemplateId);
                        if (formtemplate != null)
                        {
                            model.ParentReferenceId = formtemplate.Id;
                        }

                    }

                }
            }
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            return View("_ManageFormIndex", model);
        }


        public async Task<IActionResult> ManageNote(string id, string templateId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            //var model = new NoteTemplateViewModel();
            //model.Id = templateId;
            //model.DataAction = DataActionEnum.Create;
            //return View("_ManageNote", model);
            var model = new NoteTemplateViewModel();
            model.TemplateId = templateId;
            var temp = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (temp == null)
            {
                model.IsSubjectMandatory = true;
                model.DataAction = DataActionEnum.Create;
            }
            else
            {
                model = temp;
                model.DataAction = DataActionEnum.Edit;
            }
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            ViewBag.SynergyTableMapId = await GetTableMetadataNameByTemplateId(templateId);
            return View("_ManageNote", model);
        }
        public async Task<IActionResult> ManageTask(string id, string templateId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            var model = new TaskTemplateViewModel();

            var temp = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (temp == null)
            {
                model.DataAction = DataActionEnum.Create;
                model.IsSubjectMandatory = true;
            }
            else
            {
                model = temp;
                var template = await _templateBusiness.GetSingleById(templateId);
                if (template != null)
                {
                    model.UdfTemplateId = template.UdfTemplateId;
                    model.UdfTableMetadataId = template.UdfTableMetadataId;
                    var noteTemplate = await _templateBusiness.GetSingle(x => x.Id == template.UdfTemplateId);
                    if (noteTemplate != null)
                    {
                        model.Json = noteTemplate.Json;
                    }

                }
                model.DataAction = DataActionEnum.Edit;
            }
            model.TemplateId = templateId;
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            ViewBag.SynergyTableMapId = await GetTableMetadataNameByTemplateId(templateId);
            return View("_ManageTask", model);
        }
        public async Task<IActionResult> ManageService(string id, string templateId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            var model = new ServiceTemplateViewModel();

            var temp = await _serviceTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (temp == null)
            {
                model.DataAction = DataActionEnum.Create;
                model.IsSubjectMandatory = true;
            }
            else
            {
                model = temp;
                var template = await _templateBusiness.GetSingleById(templateId);
                if (template != null)
                {
                    model.UdfTemplateId = template.UdfTemplateId;
                    model.UdfTableMetadataId = template.UdfTableMetadataId;
                    var noteTemplate = await _templateBusiness.GetSingle(x => x.Id == template.UdfTemplateId);
                    //if (noteTemplate != null)
                    //{
                    //    model.Json = noteTemplate.Json;
                    //}

                }
                model.DataAction = DataActionEnum.Edit;
            }
            model.TemplateId = templateId;
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            ViewBag.SynergyTableMapId = await GetTableMetadataNameByTemplateId(templateId);
            return View("_ManageService", model);
        }

        public async Task<IActionResult> ManageProcessDesign(string id, string templateId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            var model = new ProcessDesignViewModel();

            var temp = await _processDesignTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (temp == null)
            {
                model.DataAction = DataActionEnum.Create;
            }
            else
            {
                model = temp;
                model.DataAction = DataActionEnum.Edit;
            }
            model.TemplateId = templateId;
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            return await Task.FromResult(View("_ManageProcessDesign", model));
        }
        public async Task<ActionResult> ReadFormTableProperties(/*[DataSourceRequest] DataSourceRequest request,*/ string templateId, string indexPageTemplateId)
        {
            // fetch  table with the help of templateId
            var template = await _templateBusiness.GetSingleById(templateId);
            // fetch all columns of the table with the help of tableid
            var columnMetaData = await _columnMetadataBusiness.GetViewableColumnMetadataList(template.TableMetadataId, template.TemplateType);
            var model = new List<FormIndexPageColumnViewModel>();
            var pageColumns = await _formIndexPageColumnBusiness.GetList(x => x.FormIndexPageTemplateId == indexPageTemplateId);
            foreach (var column in columnMetaData.Where(x => x.IsHiddenColumn == false))
            {
                var data = new FormIndexPageColumnViewModel
                {
                    ColumnName = column.Name,
                    ColumnMetadataId = column.Id,
                    IsForeignKeyTableColumn = column.IsForeignKeyTableColumn,
                    ForeignKeyTableAliasName = column.TableAliasName,
                    FormIndexPageTemplateId = indexPageTemplateId,
                    EnableFiltering = true,
                    EnableSorting = true,
                    HeaderName = column.Name,
                    SequenceOrder = column.SequenceOrder,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now,
                    Id = Guid.NewGuid().ToString(),
                    DataAction = DataActionEnum.Create
                };

                if (indexPageTemplateId != null && pageColumns != null)
                {
                    var existingColumn = pageColumns.FirstOrDefault(x => x.ColumnName == column.Alias);
                    if (existingColumn != null)
                    {
                        data.EnableFiltering = existingColumn.EnableFiltering;
                        data.EnableSorting = existingColumn.EnableSorting;
                        data.HeaderName = existingColumn.HeaderName;
                        data.SequenceOrder = existingColumn.SequenceOrder;
                        data.LastUpdatedDate = existingColumn.LastUpdatedDate;
                        data.CreatedDate = existingColumn.CreatedDate;
                        data.Id = existingColumn.Id;
                        data.DataAction = DataActionEnum.Edit;
                    }

                }
                model.Add(data);
            }
            model = model.OrderBy(x => x.SequenceOrder).OrderBy(x => x.ColumnName).ToList();
            //return Json(model.ToDataSourceResult(request));
            return Json(model);
        }
        [HttpGet]
        public async Task<ActionResult> ReadNoteTableProperties(string templateId, string indexPageTemplateId)
        {
            // fetch  table with the help of templateId
            var template = await _templateBusiness.GetSingleById(templateId);
            // fetch all columns of the table with the help of tableid
            var columnMetaData = await _columnMetadataBusiness.GetViewableColumnMetadataList(template.TableMetadataId, template.TemplateType);
            var model = new List<NoteIndexPageColumnViewModel>();
            var pageColumns = await _noteindexPageColumnBusiness.GetList(x => x.NoteIndexPageTemplateId == indexPageTemplateId);
            foreach (var column in columnMetaData.Where(x => x.IsHiddenColumn == false))
            {
                var data = new NoteIndexPageColumnViewModel
                {
                    ColumnName = column.Alias,
                    ColumnMetadataId = column.Id,
                    IsForeignKeyTableColumn = column.IsForeignKeyTableColumn,
                    ForeignKeyTableAliasName = column.TableAliasName,
                    NoteIndexPageTemplateId = indexPageTemplateId,
                    EnableFiltering = true,
                    EnableSorting = true,
                    HeaderName = column.Alias,
                    SequenceOrder = column.SequenceOrder,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now,
                    //Id = Guid.NewGuid().ToString(),
                    DataAction = DataActionEnum.Create,
                    TableName = column.TableName
                };

                if (indexPageTemplateId != null && pageColumns != null)
                {
                    var existingColumn = pageColumns.FirstOrDefault(x => x.ColumnName == column.Alias);
                    if (existingColumn != null)
                    {
                        data.ColumnName = column.Alias;
                        data.ColumnMetadataId = column.Id;
                        data.EnableFiltering = existingColumn.EnableFiltering;
                        data.EnableSorting = existingColumn.EnableSorting;
                        data.HeaderName = existingColumn.HeaderName;
                        data.SequenceOrder = existingColumn.SequenceOrder;
                        data.LastUpdatedDate = existingColumn.LastUpdatedDate;
                        data.CreatedDate = existingColumn.CreatedDate;
                        data.Id = existingColumn.Id;
                        data.DataAction = DataActionEnum.Edit;
                        data.Select = true;
                        //data.TableName = existingColumn.TableName;
                    }
                }
                model.Add(data);
            }
            model = model.OrderBy(x => x.SequenceOrder).OrderBy(x => x.ColumnName).ToList();
            //return Json(model.ToDataSourceResult(request));
            return Json(model);
        }

        [HttpGet]
        public async Task<ActionResult> ReadTaskTableProperties(string templateId, string indexPageTemplateId)
        {
            // fetch  table with the help of templateId
            var template = await _templateBusiness.GetSingleById(templateId);
            // fetch all columns of the table with the help of tableid
            var columnMetaData = await _columnMetadataBusiness.GetViewableColumnMetadataList(template.UdfTableMetadataId, template.TemplateType);
            var model = new List<TaskIndexPageColumnViewModel>();
            var pageColumns = await _taskindexPageColumnBusiness.GetList(x => x.TaskIndexPageTemplateId == indexPageTemplateId);
            foreach (var column in columnMetaData.Where(x => x.IsHiddenColumn == false))
            {
                var data = new TaskIndexPageColumnViewModel
                {
                    ColumnName = column.Alias,
                    ColumnMetadataId = column.Id,
                    IsForeignKeyTableColumn = column.IsForeignKeyTableColumn,
                    ForeignKeyTableAliasName = column.TableAliasName,
                    TaskIndexPageTemplateId = indexPageTemplateId,
                    EnableFiltering = true,
                    EnableSorting = true,
                    HeaderName = column.Alias,
                    SequenceOrder = column.SequenceOrder,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now,
                    // Id = Guid.NewGuid().ToString(),
                    DataAction = DataActionEnum.Create,
                    TableName = column.TableName
                };

                if (indexPageTemplateId != null && pageColumns != null && pageColumns.Any())
                {
                    var existingColumn = pageColumns.FirstOrDefault(x => x.ColumnName == column.Alias);
                    if (existingColumn != null)
                    {
                        data.EnableFiltering = existingColumn.EnableFiltering;
                        data.EnableSorting = existingColumn.EnableSorting;
                        data.HeaderName = existingColumn.HeaderName;
                        data.SequenceOrder = existingColumn.SequenceOrder;
                        data.LastUpdatedDate = existingColumn.LastUpdatedDate;
                        data.CreatedDate = existingColumn.CreatedDate;
                        data.Id = existingColumn.Id;
                        data.DataAction = DataActionEnum.Edit;
                        data.Select = true;
                        // data.TableName = existingColumn.TableName;
                    }

                }
                model.Add(data);
            }
            model = model.OrderBy(x => x.SequenceOrder).OrderBy(x => x.ColumnName).ToList();
            //var js = model.ToDataSourceResult(request);
            return Json(model);
        }
        [HttpGet]
        public async Task<ActionResult> ReadServiceTableProperties(string templateId, string indexPageTemplateId)
        {
            // fetch  table with the help of templateId
            var template = await _templateBusiness.GetSingleById(templateId);
            // fetch all columns of the table with the help of tableid
            var columnMetaData = await _columnMetadataBusiness.GetViewableColumnMetadataList(template.UdfTableMetadataId, template.TemplateType);
            var model = new List<ServiceIndexPageColumnViewModel>();
            var pageColumns = await _serviceIndexPageColumnBusiness.GetList(x => x.ServiceIndexPageTemplateId == indexPageTemplateId);
            foreach (var column in columnMetaData.Where(x => x.IsHiddenColumn == false))
            {
                var data = new ServiceIndexPageColumnViewModel
                {
                    ColumnName = column.Alias,
                    ColumnMetadataId = column.Id,
                    IsForeignKeyTableColumn = column.IsForeignKeyTableColumn,
                    ForeignKeyTableAliasName = column.TableAliasName,
                    ServiceIndexPageTemplateId = indexPageTemplateId,
                    EnableFiltering = true,
                    EnableSorting = true,
                    HeaderName = column.Alias,
                    SequenceOrder = column.SequenceOrder,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now,
                    //Id = Guid.NewGuid().ToString(),
                    DataAction = DataActionEnum.Create,
                    TableName = column.TableName
                };

                if (indexPageTemplateId != null && pageColumns != null)
                {
                    var existingColumn = pageColumns.FirstOrDefault(x => x.ColumnName == column.Alias);
                    if (existingColumn != null)
                    {
                        data.EnableFiltering = existingColumn.EnableFiltering;
                        data.EnableSorting = existingColumn.EnableSorting;
                        data.HeaderName = existingColumn.HeaderName;
                        data.SequenceOrder = existingColumn.SequenceOrder;
                        data.LastUpdatedDate = existingColumn.LastUpdatedDate;
                        data.CreatedDate = existingColumn.CreatedDate;
                        data.Id = existingColumn.Id;
                        data.DataAction = DataActionEnum.Edit;
                        data.Select = true;
                        //data.TableName = existingColumn.TableName;
                    }
                }
                model.Add(data);
            }
            model = model.OrderBy(x => x.SequenceOrder).OrderBy(x => x.ColumnName).ToList();
            //return Json(model.ToDataSourceResult(request));
            return Json(model);
        }

        public async Task<ActionResult> ReadCustomTableProperties(/*[DataSourceRequest] DataSourceRequest request,*/ string templateId, string indexPageTemplateId, NtsTypeEnum? ntsType)
        {
            // fetch  table with the help of templateId
            var template = await _templateBusiness.GetSingleById(templateId);
            // fetch all columns of the table with the help of tableid
            var columnMetaData = new List<ColumnMetadataViewModel>();
            if (ntsType.IsNotNull() && ntsType.Value == NtsTypeEnum.Note)
            {
                columnMetaData = await _noteBusiness.GetNoteViewableColumns();
            }
            else if (ntsType.IsNotNull() && ntsType.Value == NtsTypeEnum.Service)
            {
                columnMetaData = await _serviceBusiness.GetServiceViewableColumns();
            }
            else if (ntsType.IsNotNull() && ntsType.Value == NtsTypeEnum.Task)
            {
                columnMetaData = await _taskBusiness.GetTaskViewableColumns();
            }
            // var columnMetaData = await _columnMetadataBusiness.GetViewableColumnMetadataList(template.UdfTableMetadataId, template.TemplateType);
            var model = new List<CustomIndexPageColumnViewModel>();
            var pageColumns = await _customindexPageColumnBusiness.GetList(x => x.CustomIndexPageTemplateId == indexPageTemplateId);
            foreach (var column in columnMetaData.Where(x => x.IsHiddenColumn == false))
            {
                var data = new CustomIndexPageColumnViewModel
                {
                    ColumnName = column.Alias,
                    ColumnMetadataId = column.Id,
                    //  IsForeignKeyTableColumn = column.IsForeignKeyTableColumn,
                    // ForeignKeyTableAliasName = column.TableAliasName,
                    CustomIndexPageTemplateId = indexPageTemplateId,
                    EnableFiltering = true,
                    EnableSorting = true,
                    HeaderName = column.Alias,
                    SequenceOrder = column.SequenceOrder,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now,
                    Id = Guid.NewGuid().ToString(),
                    DataAction = DataActionEnum.Create,
                    TableName = column.TableName
                };

                if (indexPageTemplateId != null && pageColumns != null)
                {
                    var existingColumn = pageColumns.FirstOrDefault(x => x.ColumnName == column.Alias);
                    if (existingColumn != null)
                    {
                        data.EnableFiltering = existingColumn.EnableFiltering;
                        data.EnableSorting = existingColumn.EnableSorting;
                        data.HeaderName = existingColumn.HeaderName;
                        data.SequenceOrder = existingColumn.SequenceOrder;
                        data.LastUpdatedDate = existingColumn.LastUpdatedDate;
                        data.CreatedDate = existingColumn.CreatedDate;
                        data.Id = existingColumn.Id;
                        data.DataAction = DataActionEnum.Edit;
                        data.IsCustomColumn = existingColumn.IsCustomColumn;
                        data.CustomData = existingColumn.CustomData;
                        data.Select = true;
                        // data.TableName = existingColumn.TableName;
                    }
                }
                model.Add(data);
            }
            model = model.OrderBy(x => x.SequenceOrder).OrderBy(x => x.ColumnName).ToList();
            return Json(model/*.ToDataSourceResult(request)*/);
        }

        public async Task<ActionResult> ReadCustomColumndata([DataSourceRequest] DataSourceRequest request, string templateId, string indexPageTemplateId, NtsTypeEnum? ntsType)
        {
            // fetch  table with the help of templateId
            var template = await _templateBusiness.GetSingleById(templateId);
            // fetch all columns of the table with the help of tableid
            var columnMetaData = new List<ColumnMetadataViewModel>();
            if (ntsType.IsNotNull() && ntsType.Value == NtsTypeEnum.Note)
            {
                columnMetaData = await _noteBusiness.GetNoteViewableColumns();
            }
            else if (ntsType.IsNotNull() && ntsType.Value == NtsTypeEnum.Service)
            {
                columnMetaData = await _serviceBusiness.GetServiceViewableColumns();
            }
            else if (ntsType.IsNotNull() && ntsType.Value == NtsTypeEnum.Task)
            {
                columnMetaData = await _taskBusiness.GetTaskViewableColumns();
            }

            columnMetaData = columnMetaData.OrderBy(x => x.SequenceOrder).OrderBy(x => x.Alias).ToList();
            return Json(columnMetaData);
        }


        public async Task<IActionResult> ManageForm(string templateId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            var model = await _formTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (model == null)
            {
                model = new FormTemplateViewModel { TemplateId = templateId, DataAction = DataActionEnum.Create };
            }
            else
            {
                model.DataAction = DataActionEnum.Edit;
            }
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            ViewBag.SynergyTableMapId = null;
            ViewBag.SynergyTableMapId = await GetTableMetadataNameByTemplateId(templateId);
            return View("_ManageForm", model);
        }

        public async Task<String> GetTableMetadataNameByTemplateId(string templateId)
        {
            var pagedata = await _templateBusiness.GetSingleById(templateId);
            if (pagedata != null)
            {
                if (pagedata.TableSelectionType == TableSelectionTypeEnum.Existing)
                {
                    var tableId = pagedata.TableMetadataId ?? pagedata.UdfTemplateId;
                    if (tableId.IsNotNull())
                    {
                        var data = await _tableMetadataBusiness.GetSingleById(tableId);
                        if (data.IsNotNull())
                        {
                            return data.Schema + "." + data.Name;
                        }
                    }
                }
            }
            return null;
        }



        [HttpPost]
        public async Task<IActionResult> ManageForm(FormTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Json = _webApi.RemoveHost(model.Json);
                if (model.DataAction == DataActionEnum.Create)
                {
                    //var res = await _templateBusiness.CreateTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json,TemplateType=TemplateTypeEnum.Form });
                    //if(res.IsSuccess)
                    //{
                    var formresult = await _formTemplateBusiness.Create(model);
                    if (formresult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = formresult.Item.TemplateId });
                    }
                    // }
                    else
                    {
                        ModelState.AddModelErrors(formresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        //return View("_MangeForm",model);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var temp = await _formTemplateBusiness.GetSingleById(model.Id);
                    model.PreScript = temp.PreScript;
                    model.PostScript = temp.PostScript;
                    var formresult = await _formTemplateBusiness.Edit(model);
                    if (formresult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = formresult.Item.TemplateId });
                    }
                    //}
                    else
                    {
                        ModelState.AddModelErrors(formresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        //return View("_MangeForm", model);
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            //return RedirectToAction("Index", "Template");
        }

        [HttpPost]
        public async Task<ActionResult> ManageTask(TaskTemplateViewModel model)
        {

            if (ModelState.IsValid)
            {

                model.Json = _webApi.RemoveHost(model.Json);
                if (model.DataAction == DataActionEnum.Create)
                {

                    var Taskresult = await _taskTemplateBusiness.Create(model);
                    if (Taskresult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = Taskresult.Item.TemplateId });
                    }

                    else
                    {
                        ModelState.AddModelErrors(Taskresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var temp = await _taskTemplateBusiness.GetSingleById(model.Id);
                    model.PreScript = temp.PreScript;
                    model.PostScript = temp.PostScript;
                    var Taskresult = await _taskTemplateBusiness.Edit(model);
                    if (Taskresult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = Taskresult.Item.TemplateId });
                    }
                    else
                    {
                        ModelState.AddModelErrors(Taskresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }

                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            //return RedirectToAction("Index", "Template");
        }
        [HttpPost]
        public async Task<IActionResult> ManageNote(NoteTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var result = JObject.Parse(model.Json);
                //var comp = result["components"].ToString();
                //JArray rows = (JArray)result.SelectToken("components");
                model.Json = _webApi.RemoveHost(model.Json);
                if (model.DataAction == DataActionEnum.Create)
                {
                    var formresult = await _noteTemplateBusiness.Create(model);
                    if (formresult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = formresult.Item.TemplateId });
                    }
                    else
                    {
                        ModelState.AddModelErrors(formresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var temp = await _noteTemplateBusiness.GetSingleById(model.Id);
                    model.PreScript = temp.PreScript;
                    model.PostScript = temp.PostScript;
                    var formresult = await _noteTemplateBusiness.Edit(model);
                    if (formresult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = formresult.Item.TemplateId });
                    }
                    else
                    {
                        ModelState.AddModelErrors(formresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        [HttpPost]
        public async Task<ActionResult> ManageService(ServiceTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {

                model.Json = _webApi.RemoveHost(model.Json);
                if (model.DataAction == DataActionEnum.Create)
                {

                    //var res = await _templateBusiness.CreateTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Task });
                    //if (res.IsSuccess)
                    //{
                    var Taskresult = await _serviceTemplateBusiness.Create(model);
                    if (Taskresult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = Taskresult.Item.TemplateId });
                    }
                    //}
                    else
                    {
                        ModelState.AddModelErrors(Taskresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {

                    //var res = await _templateBusiness.EditTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Task });
                    //if (res.IsSuccess)
                    //{
                    var temp = await _serviceTemplateBusiness.GetSingleById(model.Id);
                    model.PreScript = temp.PreScript;
                    model.PostScript = temp.PostScript;
                    var Taskresult = await _serviceTemplateBusiness.Edit(model);
                    if (Taskresult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = Taskresult.Item.TemplateId });
                    }
                    // }
                    else
                    {
                        ModelState.AddModelErrors(Taskresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }

                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public async Task<IActionResult> ManageCustom(string templateId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            var model = await _customTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (model == null)
            {
                model = new CustomTemplateViewModel { TemplateId = templateId, DataAction = DataActionEnum.Create };
            }
            else
            {
                model.DataAction = DataActionEnum.Edit;
            }
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            return View("_ManageCustom", model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageCustom(CustomTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var formresult = await _customTemplateBusiness.Create(model);
                    if (formresult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = formresult.Item.TemplateId });
                    }
                    else
                    {
                        ModelState.AddModelErrors(formresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var formresult = await _customTemplateBusiness.Edit(model);
                    if (formresult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = formresult.Item.TemplateId });
                    }
                    else
                    {
                        ModelState.AddModelErrors(formresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public async Task<IActionResult> ManagePage(string templateId)
        {
            var model = await _pageTemlateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (model == null)
            {
                model = new PageTemplateViewModel { TemplateId = templateId, DataAction = DataActionEnum.Create };
            }
            else
            {
                model.DataAction = DataActionEnum.Edit;
            }
            ViewBag.SynergyTableMapId = await GetTableMetadataNameByTemplateId(templateId);
            return View("_ManagePage", model);
        }
        [HttpPost]
        public async Task<IActionResult> ManagePage(PageTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = JObject.Parse(model.Json);
                var comp = result["components"].ToString();
                JArray rows = (JArray)result.SelectToken("components");

                if (model.DataAction == DataActionEnum.Create)
                {
                    var pageResult = await _pageTemlateBusiness.Create(model);
                    if (pageResult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = pageResult.Item.TemplateId });
                    }
                    else
                    {
                        ModelState.AddModelErrors(pageResult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else
                {
                    var pageResult = await _pageTemlateBusiness.Edit(model);
                    if (pageResult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = pageResult.Item.TemplateId });
                    }
                    else
                    {
                        ModelState.AddModelErrors(pageResult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }



        [HttpPost]
        public async Task<ActionResult> ManageFormTemplateIndexPageData(FormIndexPageTemplateViewModel model)
        {
            if (model.RowData.IsNotNullAndNotEmpty())
            {
                model.SelectedTableRows = JsonConvert.DeserializeObject<List<FormIndexPageColumnViewModel>>(model.RowData);
            }
            if (model.DataAction == DataActionEnum.Create)
            {
                var result = await _formIndexPageTemplateBusiness.Create(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, templateId = result.Item.TemplateId });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }

            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                var result = await _formIndexPageTemplateBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, templateId = result.Item.TemplateId });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }

            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public async Task<IActionResult> ManageNotification(string id, string templateId, NtsTypeEnum ntsType, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            var model = new NotificationTemplateViewModel();

            var noteTemplate = await _notificationTemplateBusiness.GetSingleById(id);
            if (noteTemplate == null)
            {
                model.TemplateId = templateId;
                model.NtsType = ntsType;
                model.DataAction = DataActionEnum.Create;
            }
            else
            {
                model = noteTemplate;
                model.DataAction = DataActionEnum.Edit;
            }
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            return View("_ManageNotificationIndex", model);
        }

        public async Task<IActionResult> CreateNotification(string id, string TemplateId, NtsTypeEnum ntsType)
        {
            var model = new NotificationTemplateViewModel();

            var noteTemplate = await _notificationTemplateBusiness.GetSingleById(id);
            if (noteTemplate == null)
            {
                model.TemplateId = TemplateId;
                model.NtsType = ntsType;
                model.DataAction = DataActionEnum.Create;
            }
            else
            {
                model = noteTemplate;
                model.DataAction = DataActionEnum.Edit;
            }
            return View("_ManageNotificationTemplate", model);
        }
        [HttpPost]
        public async Task<ActionResult> ManageNotification(NotificationTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _notificationTemplateBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else
                {
                    var result = await _notificationTemplateBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }

            }
            return View("_ManageNotificationTemplate", model);
        }

        //public async Task<ActionResult> ReadNotificationTemplateData([DataSourceRequest] DataSourceRequest request, string templateId, NtsTypeEnum ntsType)
        //{
        //    var model = await _notificationTemplateBusiness.GetListByTemplate(templateId, ntsType);
        //    return Json(model.ToDataSourceResult(request));
        //}
        public async Task<ActionResult> ReadNotificationTemplateData(string templateId, NtsTypeEnum ntsType)
        {
            var model = await _notificationTemplateBusiness.GetListByTemplate(templateId, ntsType);
            return Json(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetNotificationParentIdNameList(NtsTypeEnum ntsType)
        {
            var dataList = await _notificationTemplateBusiness.GetList(x => x.IsTemplate == true && x.NtsType == ntsType);
            return Json(dataList.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList());
        }
        public async Task<IActionResult> DeleteNotification(string id)
        {
            await _notificationTemplateBusiness.Delete(id);

            return Json(new { success = true });
        }

        public async Task<ActionResult> ManageNoteScript(string templateId)
        {
            NoteTemplateViewModel model = new NoteTemplateViewModel();
            var temp = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (temp != null)
            {
                model = temp;
            }
            else
            {
                model.TemplateId = templateId;
            }
            return View("_ManageNoteScript", model);
        }
        [HttpPost]
        public async Task<ActionResult> ManageNoteScript(NoteTemplateViewModel model)
        {

            var temp = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == model.TemplateId);
            temp.PreScript = model.PreScript;
            temp.PostScript = model.PostScript;
            temp.Json = model.JsonCopy;
            var result1 = await _noteTemplateBusiness.Edit(temp);
            if (result1.IsSuccess)
            {
                return Json(new { success = true, templateId = result1.Item.TemplateId });
            }
            else
            {
                ModelState.AddModelErrors(result1.Messages);
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
        }
        public async Task<ActionResult> ManageTaskScript(string templateId)
        {
            TaskTemplateViewModel model = new TaskTemplateViewModel();
            var temp = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (temp != null)
            {
                model = temp;
            }
            else
            {
                model.TemplateId = templateId;
            }
            return View("_ManageTaskScript", model);
        }
        [HttpPost]
        public async Task<ActionResult> ManageTaskScript(TaskTemplateViewModel model)
        {

            var temp = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == model.TemplateId);
            temp.PreScript = model.PreScript;
            temp.PostScript = model.PostScript;
            temp.Json = model.JsonCopy;
            var result1 = await _taskTemplateBusiness.Edit(temp);
            if (result1.IsSuccess)
            {
                return Json(new { success = true, templateId = result1.Item.TemplateId });
            }
            else
            {
                ModelState.AddModelErrors(result1.Messages);
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
        }
        public async Task<ActionResult> ManageServiceScript(string templateId)
        {
            ServiceTemplateViewModel model = new ServiceTemplateViewModel();
            var temp = await _serviceTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (temp != null)
            {
                model = temp;
            }
            else
            {
                model.TemplateId = templateId;
            }
            return View("_ManageServiceScript", model);
        }

        [HttpPost]
        public async Task<ActionResult> ManageServiceScript(ServiceTemplateViewModel model)
        {

            var temp = await _serviceTemplateBusiness.GetSingle(x => x.TemplateId == model.TemplateId);
            temp.PreScript = model.PreScript;
            temp.PostScript = model.PostScript;
            temp.Json = model.JsonCopy;
            var result1 = await _serviceTemplateBusiness.Edit(temp);
            if (result1.IsSuccess)
            {
                return Json(new { success = true, templateId = result1.Item.TemplateId });
            }
            else
            {
                ModelState.AddModelErrors(result1.Messages);
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
        }
        public async Task<ActionResult> ManageFormScript(string templateId)
        {
            FormTemplateViewModel model = new FormTemplateViewModel();
            var temp = await _formTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (temp != null)
            {
                model = temp;
            }
            else
            {
                model.TemplateId = templateId;
            }
            return View("_ManageFormScript", model);
        }
        [HttpPost]
        public async Task<ActionResult> ManageFormScript(FormTemplateViewModel model)
        {

            var temp = await _formTemplateBusiness.GetSingle(x => x.TemplateId == model.TemplateId);
            temp.PreScript = model.PreScript;
            temp.PostScript = model.PostScript;
            temp.Json = model.JsonCopy;
            var result1 = await _formTemplateBusiness.Edit(temp);
            if (result1.IsSuccess)
            {
                return Json(new { success = true, templateId = result1.Item.TemplateId });
            }
            else
            {
                ModelState.AddModelErrors(result1.Messages);
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
        }
        public async Task<IActionResult> ManageNoteIndex(string templateId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            var model = new NoteIndexPageTemplateViewModel
            {
                TemplateId = templateId
            };
            var template = await _templateBusiness.GetSingleById(model.TemplateId);
            if (template != null)
            {
                var indexPageTemplate = await _noteIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
                if (indexPageTemplate != null)
                {
                    model = indexPageTemplate;
                    model.TableMetadataId = template.TableMetadataId;
                    model.DataAction = DataActionEnum.Edit;
                    model.SelectedTableRows = await _noteindexPageColumnBusiness.GetList(x => x.NoteIndexPageTemplateId == model.Id);
                    ViewBag.RowData = JsonConvert.SerializeObject(model.SelectedTableRows);
                }
                else
                {
                    model.TableMetadataId = template.TableMetadataId;
                    model.DataAction = DataActionEnum.Create;

                    model.CreateButtonText = "Create";
                    model.EditButtonText = "Edit";
                    model.DeleteButtonText = "Delete";
                    model.EnableCreateButton = true;
                    model.EnableEditButton = true;
                    model.EnableDeleteButton = true;
                    model.EnableDeleteConfirmation = true;
                    //model.HideIndexHeader = true;

                    model.DeleteConfirmationMessage = ApplicationConstant.Messages.DeleteConfirmation;

                    var category = await _categoryBusiness.GetSingleById(template.TemplateCategoryId);

                    if (category.TemplateType == TemplateTypeEnum.Note)
                    {
                        model.IndexPageTemplateType = TemplateTypeEnum.Note;
                        var taskTemplate = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == model.TemplateId);
                        if (taskTemplate != null)
                        {
                            model.ParentReferenceId = taskTemplate.Id;
                        }

                    }

                }
            }
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            return View("_ManageNoteIndex", model);

        }
        public async Task<IActionResult> ManageNtsLog(string templateId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            var model = new NtsLogPageColumnViewModel
            {
                TemplateId = templateId
            };
            var template = await _templateBusiness.GetSingleById(model.TemplateId);
            if (template != null)
            {
                var indexPageTemplate = await _ntsLogPageColumnBusiness.GetSingle(x => x.TemplateId == templateId);
                if (indexPageTemplate != null)
                {
                    model = indexPageTemplate;
                    model.TableMetadataId = template.TableMetadataId;
                    model.DataAction = DataActionEnum.Edit;
                    model.SelectedTableRows = await _ntsLogPageColumnBusiness.GetList(x => x.TemplateId == templateId);
                    ViewBag.RowData = JsonConvert.SerializeObject(model.SelectedTableRows);
                }
                else
                {
                    model.TableMetadataId = template.TableMetadataId;
                    model.DataAction = DataActionEnum.Create;
                    var category = await _categoryBusiness.GetSingleById(template.TemplateCategoryId);
                    var taskTemplate = await _templateBusiness.GetSingle(x => x.Id == model.TemplateId);
                    if (taskTemplate != null)
                    {
                        model.TemplateId = taskTemplate.Id;
                    }
                    //if (category.TemplateType == TemplateTypeEnum.Note)
                    //{
                    //   // model.IndexPageTemplateType = TemplateTypeEnum.Note;
                    //    var taskTemplate = await _templateBusiness.GetSingle(x => x.Id == model.TemplateId);
                    //    if (taskTemplate != null)
                    //    {
                    //        model.ParentReferenceId = taskTemplate.Id;
                    //    }

                    //}

                }
            }
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            return View("_ManageNtsLog", model);

        }
        [HttpPost]
        public async Task<ActionResult> ManageNtsLogPageData(NtsLogPageColumnViewModel model)
        {
            model.SelectedTableRows = JsonConvert.DeserializeObject<List<NtsLogPageColumnViewModel>>(model.RowData);
            if (model.DataAction == DataActionEnum.Create)
            {
                var result = await _ntsLogPageColumnBusiness.Create(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, templateId = result.Item.TemplateId });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }

            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                var result = await _ntsLogPageColumnBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, templateId = result.Item.TemplateId });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }

            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public async Task<ActionResult> ReadNtsLogTableProperties(/*[DataSourceRequest] DataSourceRequest request,*/ string templateId, string indexPageTemplateId)
        {
            // fetch  table with the help of templateId
            var template = await _templateBusiness.GetSingleById(templateId);
            // fetch all columns of the table with the help of tableid
            var columnMetaData = await _columnMetadataBusiness.GetViewableColumnMetadataList(template.TableMetadataId.Coalesce(template.UdfTableMetadataId), template.TemplateType);
            var model = new List<NtsLogPageColumnViewModel>();
            var pageColumns = await _ntsLogPageColumnBusiness.GetList(x => x.TemplateId == templateId);
            foreach (var column in columnMetaData.Where(x => x.IsHiddenColumn == false))
            {
                var data = new NtsLogPageColumnViewModel
                {
                    ColumnName = column.Alias,
                    ColumnMetadataId = column.Id,
                    IsForeignKeyTableColumn = column.IsForeignKeyTableColumn,
                    ForeignKeyTableAliasName = column.TableAliasName,
                    TemplateId = templateId,
                    EnableFiltering = true,
                    EnableSorting = true,
                    HeaderName = column.Alias,
                    SequenceOrder = column.SequenceOrder,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now,
                    Id = Guid.NewGuid().ToString(),
                    DataAction = DataActionEnum.Create,
                    TableName = column.TableName
                };

                if (templateId != null && pageColumns != null)
                {
                    var existingColumn = pageColumns.FirstOrDefault(x => x.ColumnName == column.Alias);
                    if (existingColumn != null)
                    {
                        data.EnableFiltering = existingColumn.EnableFiltering;
                        data.EnableSorting = existingColumn.EnableSorting;
                        data.HeaderName = existingColumn.HeaderName;
                        data.SequenceOrder = existingColumn.SequenceOrder;
                        data.LastUpdatedDate = existingColumn.LastUpdatedDate;
                        data.CreatedDate = existingColumn.CreatedDate;
                        data.Id = existingColumn.Id;
                        data.DataAction = DataActionEnum.Edit;
                        data.TableName = column.TableName;
                        data.Select = false;
                    }
                }
                model.Add(data);
            }
            model = model.OrderBy(x => x.SequenceOrder).OrderBy(x => x.ColumnName).ToList();
            return Json(model/*.ToDataSourceResult(request)*/);
        }

        [HttpPost]
        public async Task<ActionResult> ManageNoteTempltaeIndexPageData(NoteIndexPageTemplateViewModel model)
        {
            if (model.RowData.IsNotNullAndNotEmpty())
            {
                model.SelectedTableRows = JsonConvert.DeserializeObject<List<NoteIndexPageColumnViewModel>>(model.RowData);
            }
            if (model.DataAction == DataActionEnum.Create)
            {
                var result = await _noteIndexPageTemplateBusiness.Create(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, templateId = result.Item.TemplateId });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }

            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                var result = await _noteIndexPageTemplateBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, templateId = result.Item.TemplateId });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }

            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCustomIndexPage(CustomIndexPageColumnViewModel item)
        {
            if (item.Id.IsNullOrEmpty() && item.Select)
            {
                await _customindexPageColumnBusiness.Create(item);
            }
            else if (item.Select)
            {
                await _customindexPageColumnBusiness.Edit(item);
            }
            else if (item.Id.IsNotNullAndNotEmpty())
            {
                await _customindexPageColumnBusiness.Delete(item.Id);
            }
            return Json(item);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateNoteIndexPage(NoteIndexPageColumnViewModel item)
        {
            if (item.Id.IsNullOrEmpty() && item.Select)
            {
                await _noteindexPageColumnBusiness.Create(item);
            }
            else if (item.Select)
            {
                await _noteindexPageColumnBusiness.Edit(item);
            }
            else if (item.Id.IsNotNullAndNotEmpty())
            {
                await _noteindexPageColumnBusiness.Delete(item.Id);
            }
            return Json(item);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTaskIndexPage(TaskIndexPageColumnViewModel item)
        {
            if (item.Id.IsNullOrEmpty() && item.Select)
            {
                await _taskindexPageColumnBusiness.Create(item);
            }
            else if (item.Select)
            {
                await _taskindexPageColumnBusiness.Edit(item);
            }
            else if (item.Id.IsNotNullAndNotEmpty())
            {
                await _taskindexPageColumnBusiness.Delete(item.Id);
            }
            return Json(item);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateServiceIndexPage(ServiceIndexPageColumnViewModel item)
        {
            if (item.Id.IsNullOrEmpty() && item.Select)
            {
                await _serviceIndexPageColumnBusiness.Create(item);
            }
            else if (item.Select)
            {
                await _serviceIndexPageColumnBusiness.Edit(item);
            }
            else if (item.Id.IsNotNullAndNotEmpty())
            {
                await _serviceIndexPageColumnBusiness.Delete(item.Id);
            }
            return Json(item);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatFormIndexPage(FormIndexPageColumnViewModel item)
        {
            if (item.Id.IsNullOrEmpty() && item.Select)
            {
                await _formIndexPageColumnBusiness.Create(item);
            }
            else if (item.Select)
            {
                await _formIndexPageColumnBusiness.Edit(item);
            }
            else if (item.Id.IsNotNullAndNotEmpty())
            {
                await _formIndexPageColumnBusiness.Delete(item.Id);
            }
            return Json(item);
        }
        public async Task<IActionResult> ManageTaskIndex(string templateId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            var model = new TaskIndexPageTemplateViewModel
            {
                TemplateId = templateId
            };
            var template = await _templateBusiness.GetSingleById(model.TemplateId);
            if (template != null)
            {
                var indexPageTemplate = await _taskIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
                if (indexPageTemplate != null)
                {
                    model = indexPageTemplate;
                    model.UdfTableMetadataId = template.UdfTableMetadataId;
                    model.UdfTemplateId = template.UdfTemplateId;
                    model.TableMetadataId = template.TableMetadataId;
                    model.DataAction = DataActionEnum.Edit;
                    model.SelectedTableRows = await _taskindexPageColumnBusiness.GetList(x => x.TaskIndexPageTemplateId == model.Id);
                    ViewBag.RowData = JsonConvert.SerializeObject(model.SelectedTableRows);
                }
                else
                {
                    model.DataAction = DataActionEnum.Create;
                    model.TableMetadataId = template.TableMetadataId;
                    model.UdfTemplateId = template.UdfTemplateId;
                    model.UdfTableMetadataId = template.UdfTableMetadataId;
                    model.CreateButtonText = "Create";
                    model.EditButtonText = "Edit";
                    model.DeleteButtonText = "Delete";
                    model.EnableCreateButton = true;
                    model.EnableEditButton = true;
                    model.EnableDeleteButton = true;
                    model.EnableDeleteConfirmation = true;
                    model.DeleteConfirmationMessage = ApplicationConstant.Messages.DeleteConfirmation;

                    var category = await _categoryBusiness.GetSingleById(template.TemplateCategoryId);

                    if (category.TemplateType == TemplateTypeEnum.Task)
                    {
                        model.IndexPageTemplateType = TemplateTypeEnum.Task;
                        var taskTemplate = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == model.TemplateId);
                        if (taskTemplate != null)
                        {
                            model.ParentReferenceId = taskTemplate.Id;
                        }
                    }
                }
            }
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            return View("_ManageTaskIndex", model);
        }

        public async Task<IActionResult> ManageCustomIndex(string templateId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            var model = new CustomIndexPageTemplateViewModel
            {
                TemplateId = templateId
            };
            var template = await _templateBusiness.GetSingleById(model.TemplateId);
            if (template != null)
            {
                var indexPageTemplate = await _customindexPageTemplateBusiness.GetSingle(x => x.TemplateId == templateId);

                if (indexPageTemplate != null)
                {
                    model = indexPageTemplate;
                    // model.UdfTemplateId = template.UdfTemplateId;
                    model.TableMetadataId = template.TableMetadataId;
                    model.DataAction = DataActionEnum.Edit;
                    model.SelectedTableRows = await _customindexPageColumnBusiness.GetList(x => x.CustomIndexPageTemplateId == model.Id);
                    ViewBag.RowData = JsonConvert.SerializeObject(model.SelectedTableRows);
                }
                else
                {
                    model.DataAction = DataActionEnum.Create;
                    model.TableMetadataId = template.TableMetadataId;
                    // model.UdfTemplateId = template.UdfTemplateId;
                    model.CreateButtonText = "Create";
                    model.EditButtonText = "Edit";
                    model.DeleteButtonText = "Delete";
                    model.EnableCreateButton = true;
                    model.EnableEditButton = true;
                    model.EnableDeleteButton = true;
                    model.EnableDeleteConfirmation = true;
                    model.DeleteConfirmationMessage = ApplicationConstant.Messages.DeleteConfirmation;

                }
                model.NtsType = template.NtsType.Value;
            }
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            return View("_ManageCustomIndex", model);
        }

        [HttpPost]
        public async Task<ActionResult> ManageTaskTempltaeIndexPageData(TaskIndexPageTemplateViewModel model)
        {
            if (model.RowData.IsNotNullAndNotEmpty())
            {
                model.SelectedTableRows = JsonConvert.DeserializeObject<List<TaskIndexPageColumnViewModel>>(model.RowData);
            }
            if (model.DataAction == DataActionEnum.Create)
            {
                var result = await _taskIndexPageTemplateBusiness.Create(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, templateId = result.Item.TemplateId });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }

            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                var result = await _taskIndexPageTemplateBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, templateId = result.Item.TemplateId });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }

            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public async Task<IActionResult> ManageServiceIndex(string templateId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            var model = new ServiceIndexPageTemplateViewModel
            {
                TemplateId = templateId
            };
            var template = await _templateBusiness.GetSingleById(model.TemplateId);
            if (template != null)
            {
                var indexPageTemplate = await _serviceIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
                if (indexPageTemplate != null)
                {
                    model = indexPageTemplate;
                    model.UdfTableMetadataId = template.UdfTableMetadataId;
                    model.UdfTemplateId = template.UdfTemplateId;
                    model.TableMetadataId = template.TableMetadataId;
                    model.DataAction = DataActionEnum.Edit;
                    model.SelectedTableRows = await _serviceIndexPageColumnBusiness.GetList(x => x.ServiceIndexPageTemplateId == model.Id);
                    ViewBag.RowData = JsonConvert.SerializeObject(model.SelectedTableRows);
                }
                else
                {
                    model.DataAction = DataActionEnum.Create;
                    model.TableMetadataId = template.TableMetadataId;
                    model.UdfTemplateId = template.UdfTemplateId;
                    model.UdfTableMetadataId = template.UdfTableMetadataId;
                    model.CreateButtonText = "Create";
                    model.EditButtonText = "Edit";
                    model.DeleteButtonText = "Delete";
                    model.EnableCreateButton = true;
                    model.EnableEditButton = true;
                    model.EnableDeleteButton = true;
                    model.EnableDeleteConfirmation = true;
                    model.DeleteConfirmationMessage = ApplicationConstant.Messages.DeleteConfirmation;

                    var category = await _categoryBusiness.GetSingleById(template.TemplateCategoryId);

                    if (category.TemplateType == TemplateTypeEnum.Service)
                    {
                        model.IndexPageTemplateType = TemplateTypeEnum.Service;
                        var taskTemplate = await _serviceTemplateBusiness.GetSingle(x => x.TemplateId == model.TemplateId);
                        if (taskTemplate != null)
                        {
                            model.ParentReferenceId = taskTemplate.Id;
                        }

                    }

                }
            }
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            return View("_ManageServiceIndex", model);
        }

        [HttpPost]
        public async Task<ActionResult> ManageServiceTempltaeIndexPageData(ServiceIndexPageTemplateViewModel model)
        {
            if (model.RowData.IsNotNullAndNotEmpty())
            {
                model.SelectedTableRows = JsonConvert.DeserializeObject<List<ServiceIndexPageColumnViewModel>>(model.RowData);
            }
            if (model.DataAction == DataActionEnum.Create)
            {
                var result = await _serviceIndexPageTemplateBusiness.Create(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, templateId = result.Item.TemplateId });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }

            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                var result = await _serviceIndexPageTemplateBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, templateId = result.Item.TemplateId });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }

            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        [HttpPost]
        public async Task<ActionResult> ManageCustomTemplteIndexPageData(CustomIndexPageTemplateViewModel model)
        {
            if (model.RowData.IsNotNullAndNotEmpty())
            {
                model.SelectedTableRows = JsonConvert.DeserializeObject<List<CustomIndexPageColumnViewModel>>(model.RowData);
            }
            if (model.DataAction == DataActionEnum.Create)
            {
                var result = await _customindexPageTemplateBusiness.Create(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, templateId = result.Item.TemplateId });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }

            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                var result = await _customindexPageTemplateBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, templateId = result.Item.TemplateId });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }

            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        [HttpGet]
        public async Task<ActionResult> GetColumnMetadata(string tableMetadataId, TemplateTypeEnum templateType)
        {
            var data = await _columnMetadataBusiness.GetViewableColumnMetadataList(tableMetadataId, templateType);
            return Json(data);
        }

        public async Task<IActionResult> ManageImportExport(string templateId)
        {
            var model = await _templateBusiness.GetSingleById(templateId);
            return View(model);
        }

        public async Task<IActionResult> ManageExport(string Id)
        {
            //var model = JsonConvert.DeserializeObject<TemplateViewModel>(data);
            var template = await _templateBusiness.GetSingleById(Id);
            ExportTemplateViewModel newmodel = new ExportTemplateViewModel();
            newmodel.Template = template;
            var obj = await _templateBusiness.ExportTemplate(newmodel);
            var data1 = JsonConvert.SerializeObject(obj);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data1);
            var output = new FileContentResult(bytes, "application/octet-stream");
            output.FileDownloadName = "TemplateExport.txt";

            return output;

        }
        public async Task<IActionResult> ManageImport(string categoryId, string type)
        {
            TemplateViewModel model = new TemplateViewModel();
            model.TemplateCategoryId = categoryId;
            model.TemplateType = (TemplateTypeEnum)Enum.Parse(typeof(TemplateTypeEnum), type, true);
            return View("ManageImport", model);
        }
        public ActionResult ManageImportData(string type)
        {
            TemplateViewModel model = new TemplateViewModel();
            model.TemplateType = (TemplateTypeEnum)Enum.Parse(typeof(TemplateTypeEnum), type, true);
            return View(model);
        }
        public async Task<IActionResult> ImportTemplate(TemplateViewModel model)
        {
            var doc = await _fileBusiness.GetFileByte(model.ImportFileId);
            string utfString = Encoding.UTF8.GetString(doc, 0, doc.Length);
            ExportTemplateViewModel Viewmodel = JsonConvert.DeserializeObject<ExportTemplateViewModel>(utfString);
            Viewmodel.Template.Name = model.Name;
            Viewmodel.Template.DisplayName = model.DisplayName;
            Viewmodel.Template.Description = model.Description;
            Viewmodel.Template.TemplateCategoryId = model.TemplateCategoryId;
            if (Viewmodel.Template.TemplateType != model.TemplateType)
            {
                return Json(new { success = false, error = "The Imported Template must be of " + model.TemplateType + " type." });
            }
            if (model.Code == Viewmodel.Template.Code)
            {
                // Over ride existing template
                var result = await _templateBusiness.OverwriteTemplate(Viewmodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = result.Messages.ToHtmlError() });
                }
            }
            else
            {
                Viewmodel.Template.Code = model.Code;
                var result = await _templateBusiness.ImportTemplate(Viewmodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = result.Messages.ToHtmlError() });
                }
            }

        }

        public async Task<IActionResult> ImportData(TemplateViewModel model)
        {
            var doc = await _fileBusiness.GetFileByte(model.ImportFileId);
            string utfString = Encoding.UTF8.GetString(doc, 0, doc.Length);
            ExportTemplateViewModel Viewmodel = JsonConvert.DeserializeObject<ExportTemplateViewModel>(utfString);
            //Viewmodel.Template.Name = model.Name;
            //Viewmodel.Template.DisplayName = model.DisplayName;
            //Viewmodel.Template.Description = model.Description;
            //Viewmodel.Template.TemplateCategoryId = model.TemplateCategoryId;
            if (Viewmodel.Template.TemplateType != model.TemplateType)
            {
                return Json(new { success = false, error = "The Imported Template must be of " + model.TemplateType + " type." });
            }
            var isTemplateExists = await _templateBusiness.CheckTemplate(Viewmodel.Template);
            //if (model.Code == Viewmodel.Template.Code)
            if (isTemplateExists != null)
            {
                // Over ride existing template
                var result = await _templateBusiness.OverwriteTemplate(Viewmodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = result.Messages.ToHtmlError() });
                }
            }
            else
            {
                //Viewmodel.Template.Code = model.Code;
                Viewmodel.Template.Name = Viewmodel.Template.DisplayName;
                var result = await _templateBusiness.ImportTemplate(Viewmodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = result.Messages.ToHtmlError() });
                }
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetNoteList()
        {

            var pagedata = await _templateBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Note);
            return Json(pagedata.Select(x => new IdNameViewModel { Id = x.Id, Name = x.DisplayName }));

        }


        public async Task<IActionResult> GetTemplateDetails(string fileId)
        {
            var doc = await _fileBusiness.GetFileByte(fileId);
            string utfString = Encoding.UTF8.GetString(doc, 0, doc.Length);
            ExportTemplateViewModel Viewmodel = JsonConvert.DeserializeObject<ExportTemplateViewModel>(utfString);
            if (Viewmodel.Template.TemplateType == TemplateTypeEnum.Note)
            {
                Viewmodel.Template.Name = Viewmodel.Template.Name.Replace("N_" + Viewmodel.TemplateCategory.Name + "_", "");
            }
            if (Viewmodel.Template.TemplateType == TemplateTypeEnum.Task)
            {
                Viewmodel.Template.Name = Viewmodel.Template.Name.Replace("T_" + Viewmodel.TemplateCategory.Name + "_", "");
            }
            if (Viewmodel.Template.TemplateType == TemplateTypeEnum.Service)
            {
                Viewmodel.Template.Name = Viewmodel.Template.Name.Replace("S_" + Viewmodel.TemplateCategory.Name + "_", "");
            }
            if (Viewmodel.Template.TemplateType == TemplateTypeEnum.Page)
            {
                Viewmodel.Template.Name = Viewmodel.Template.Name.Replace("P_" + Viewmodel.TemplateCategory.Name + "_", "");
            }
            if (Viewmodel.Template.TemplateType == TemplateTypeEnum.Form)
            {
                Viewmodel.Template.Name = Viewmodel.Template.Name.Replace("F_" + Viewmodel.TemplateCategory.Name + "_", "");
            }
            return Json(new { success = true, Name = Viewmodel.Template.Name, DisplayName = Viewmodel.Template.DisplayName, Code = Viewmodel.Template.Code, Description = Viewmodel.Template.Description });
        }
        public async Task<ActionResult> ManagePreBusinessRule(string templateId, BusinessLogicExecutionTypeEnum type)
        {
            TemplateViewModel model = new TemplateViewModel();
            var temp = await _templateBusiness.GetSingle(x => x.Id == templateId);
            temp.Type = type;
            //if (temp != null)
            //{
            //    model = temp;
            //}
            //else
            //{
            //model.Id = templateId;
            //}
            return View("_ManagePreBusinessRule", temp);
        }
        public async Task<ActionResult> ManageCustomBusinessRule(string templateId, BusinessLogicExecutionTypeEnum type)
        {
            TemplateViewModel model = new TemplateViewModel();
            var temp = await _templateBusiness.GetSingle(x => x.Id == templateId);
            temp.Type = type;
            //if (temp != null)
            //{
            //    model = temp;
            //}
            //else
            //{
            //model.Id = templateId;
            //}
            return View("_ManageCustomBusinessRule", temp);
        }
        public async Task<ActionResult> ManagePostBusinessRule(string templateId, BusinessLogicExecutionTypeEnum type)
        {
            TemplateViewModel model = new TemplateViewModel();
            var temp = await _templateBusiness.GetSingle(x => x.Id == templateId);
            temp.Type = type;
            //if (temp != null)
            //{
            //    model = temp;
            //}
            //else
            //{
            //model.Id = templateId;
            //}
            return View("_ManagePostBusinessRule", temp);
        }
        public async Task<ActionResult> ManageActions(string templateId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            TemplateViewModel model = new TemplateViewModel();
            var temp = await _templateBusiness.GetSingle(x => x.Id == templateId);
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            return View("_ManageActions", temp);
        }

        public async Task<ActionResult> ReadBusinessRuleForAction([DataSourceRequest] DataSourceRequest request, string templateId, TemplateTypeEnum type, BusinessLogicExecutionTypeEnum actionType)
        {
            var ExecutionType = 0;
            if (actionType == BusinessLogicExecutionTypeEnum.PreSubmit)
            {
                ExecutionType = 0;
            }
            else if (actionType == BusinessLogicExecutionTypeEnum.PostSubmit)
            {
                ExecutionType = 1;
            }
            else if (actionType == BusinessLogicExecutionTypeEnum.Custom)
            {
                ExecutionType = 2;
            }
            var model = await _businessRuleBusiness.GetBusinessRuleActionList(templateId, ExecutionType);
            // fetch  table with the help of templateId
            //var template = await _templateBusiness.GetSingleById(templateId);
            //// fetch all columns of the table with the help of tableid
            //string LovType = string.Empty;
            //if (type == TemplateTypeEnum.Service)
            //{
            //    LovType = "LOV_SERVICE_STATUS";///LOV_NOTE_ACTION
            //}
            //else if (type == TemplateTypeEnum.Note)
            //{
            //    LovType = "LOV_NOTE_STATUS";///
            //}
            //else if (type == TemplateTypeEnum.Task)
            //{
            //    LovType = "LOV_TASK_STATUS";///
            //}
            //else if (type == TemplateTypeEnum.Form)
            //{
            //    LovType = "LOV_FORM_STATUS";///
            //}
            //var columnMetaData = await _lovBusiness.GetList(x => x.LOVType == LovType);
            //var model = new List<BusinessRuleViewModel>();
            //var pageColumns = await _businessRuleBusiness.GetList(x => x.TemplateId == templateId && x.BusinessLogicExecutionType == actionType);
            //foreach (var column in columnMetaData)
            //{
            //    var data = new BusinessRuleViewModel
            //    {
            //        ActionId = column.Id,
            //        //Name = column.Name,
            //        ActionName = column.Description.IsNotNullAndNotEmpty() ? column.Description : column.Name,
            //        SequenceOrder = column.SequenceOrder,
            //        CreatedDate = DateTime.Now,
            //        LastUpdatedDate = DateTime.Now,
            //        Id = Guid.NewGuid().ToString(),
            //        DataAction = DataActionEnum.Create,
            //        RuleExist = false
            //    };

            //    if (pageColumns != null)
            //    {
            //        var existingColumn = pageColumns.FirstOrDefault(x => x.ActionId == column.Id);
            //        if (existingColumn != null)
            //        {
            //            data.RuleExist = true;
            //            data.Name = existingColumn.Name;
            //            data.SequenceOrder = existingColumn.SequenceOrder;
            //            data.LastUpdatedDate = existingColumn.LastUpdatedDate;
            //            data.CreatedDate = existingColumn.CreatedDate;
            //            data.Id = existingColumn.Id;
            //            data.DataAction = DataActionEnum.Edit;
            //        }
            //    }
            //    model.Add(data);
            //}
            //model = model.OrderBy(x => x.SequenceOrder).OrderBy(x => x.Name).ToList();
            return Json(model.ToDataSourceResult(request));
        }

        public async Task<ActionResult> ReadAdhocTaskList()
        {
            var list = await _recTaskTemplateBusiness.GetAdhocTaskTemplateList();
            return Json(list);

        }


        public async Task<ActionResult> GetTemplateList(string selectedValues)
        {
            List<TemplateViewModel> list = new List<TemplateViewModel>();
            if (selectedValues.IsNotNullAndNotEmpty())
            {
                var selectedValueslist = selectedValues.Split(",");
                if (selectedValueslist.Length > 0)
                {
                    foreach (var id in selectedValueslist)
                    {
                        var temp = await _templateBusiness.GetSingleById(id);
                        list.Add(temp);
                    }
                }
            }
            return Json(list);
        }
        public async Task<ActionResult> GetTemplatesIdNameList(TemplateTypeEnum type)
        {
            //List<TemplateViewModel> list = new List<TemplateViewModel>();
            var list = await _templateBusiness.GetList(x => x.TemplateType == type);
            return Json(list);
        }

        public async Task<ActionResult> GetTemplateBusinessDiagram(string templateId)
        {
            TaskTemplateViewModel busDiagram = new TaskTemplateViewModel();
            busDiagram.TaskSubject = "Business Diagram";
            var list = await _templateBusiness.GetTemplateBusinessDiagram(templateId);
            string businessDiagramJson = null;
            var data = await _tableMetadataBusiness.GetTableDataByColumn("BUSINESS_DIAGRAM", null, "diagramTemplateId", templateId);
            if (data != null)
            {
                var noteId = Convert.ToString(data["NtsNoteId"]);
                if (noteId.IsNotNullAndNotEmpty())
                {
                    var task = await _taskBusiness.GetSingle(x => x.UdfNoteId == noteId);
                    if (task.IsNotNull())
                    {
                        var bDiagram = new TaskTemplateViewModel
                        {
                            TaskId = task.Id
                        };
                        busDiagram = await _taskBusiness.GetTaskDetails(bDiagram);
                    }
                }
                businessDiagramJson = Convert.ToString(data["diagramJson"]);
            }
            else
            {
                var bDiagram = new TaskTemplateViewModel
                {
                    ActiveUserId = _userContext.UserId,
                    TemplateCode = "BUSINESS_DIAGRAM",
                };
                var bDiagramTask = await _taskBusiness.GetTaskDetails(bDiagram);
                bDiagramTask.TaskSubject = "Business Diagram";
                bDiagramTask.OwnerUserId = _userContext.UserId;
                bDiagramTask.StartDate = DateTime.Now;
                bDiagramTask.DueDate = DateTime.Now;
                bDiagramTask.AssignedToUserId = _userContext.UserId;
                bDiagramTask.DataAction = DataActionEnum.Create;
                bDiagramTask.CreatedBy = _userContext.UserId;
                bDiagramTask.CompanyId = _userContext.UserId;
                bDiagramTask.CreatedDate = DateTime.Now;
                bDiagramTask.LastUpdatedBy = _userContext.UserId;
                bDiagramTask.LastUpdatedDate = DateTime.Now;
                var busDiagramFull = await _taskBusiness.ManageTask(bDiagramTask);
                busDiagram = busDiagramFull.Item;
            }
            return Json(new
            {
                list = list,
                standard = businessDiagramJson,
                diagramDetails = busDiagram
            });
        }

        public async Task<IActionResult> ReadComponentsList(string templateId)
        {
            var result = await _templateBusiness.GetComponentsList(templateId);
            return Json(result);
        }

        public async Task<IActionResult> ManageLanguage(string templateId)
        {
            var model = new TemplateViewModel { Id = templateId };
            return View("_ManageLanguage", model);
        }
        public async Task<ActionResult> GetTemplateLanguage([DataSourceRequest] DataSourceRequest request, string templateId)
        {
            var jsonList = new List<ResourceLanguageViewModel>();
            var template = await _templateBusiness.GetSingleById(templateId);
            var data = JToken.Parse(template.Json);
            JArray rows = (JArray)data.SelectToken("components");
            await BuildLanguageJson(rows, jsonList);
            jsonList.ForEach(x => { x.TemplateId = templateId; x.TemplateType = template.TemplateType; x.GroupCode = ResourceLanguageGroupCodeEnum.Udf; x.DataAction = DataActionEnum.Create; x.Status = StatusEnum.Active; });
            var actualList = await _resourceLanguageBusiness.GetList(x => x.TemplateId == templateId);
            var result = new List<ResourceLanguageViewModel>();
            foreach (var jsonItem in jsonList)
            {
                var item = actualList.Where(x => x.Code == jsonItem.Code).FirstOrDefault();
                if (item.IsNotNull())
                {
                    result.Add(item);
                }
                else
                {
                    result.Add(jsonItem);
                }
            }
            return Json(result.ToDataSourceResult(request));
        }
        private async Task BuildLanguageJson(JArray comps, List<ResourceLanguageViewModel> list)
        {

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
                                await BuildLanguageJson(rows, list);
                        }
                    }
                    else if (type == "table")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("rows");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                await BuildLanguageJson(rows, list);
                        }
                    }
                    else
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                        {
                            await BuildLanguageJson(rows, list);
                        }
                        else
                        {
                            var model = new ResourceLanguageViewModel();
                            JToken key = jcomp["key"];
                            model.Code = key.IsNotNull() ? key.Value<string>() : "";
                            JToken label = jcomp["label"];
                            model.English = label.IsNotNull() ? label.Value<string>() : "";
                            list.Add(model);
                        }
                    }
                }
            }

        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> TemplateLanguage_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<ResourceLanguageViewModel> list)
        {

            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    if (item.Id.IsNullOrEmpty())
                    {
                        await _resourceLanguageBusiness.Create(item);
                    }
                    else
                    {
                        await _resourceLanguageBusiness.Edit(item);
                    }

                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public async Task<IActionResult> LanguageUpdate(ResourceLanguageViewModel item)
        {
            if (item.Id.IsNullOrEmpty())
            {
                await _resourceLanguageBusiness.Create(item);
            }
            else
            {
                await _resourceLanguageBusiness.Edit(item);
            }
            return Json(item);
        }
        [HttpGet]
        public async Task<IActionResult> GetTemplates()
        {

            var pagedata = await _templateBusiness.GetList();
            return Json(pagedata.Select(x => new IdNameViewModel { Id = x.Id, Name = x.DisplayName }));

        }
        public IActionResult TemplateDelete()
        {

            return View();

        }
        //public async Task<ActionResult> ReadTemplateData([DataSourceRequest] DataSourceRequest request)
        //{
        //    var model = await _templateBusiness.GetTemplateDeleteList();
        //    return Json(model.ToDataSourceResult(request));
        //}
        public async Task<ActionResult> ReadTemplateData()
        {
            var model = await _templateBusiness.GetTemplateDeleteList();
            return Json(model);
        }

        public async Task<ActionResult> DeleteTemplateData(string templateIds)
        {
            await _templateBusiness.DeleteTemplateData(templateIds);
            return Json("Succes");
        }
        public async Task<ActionResult> GetWorkSpaceTemplate(string portalId)
        {
            //List<TemplateViewModel> list = new List<TemplateViewModel>();
            var list = await _templateBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Service);
            if (portalId.IsNotNullAndNotEmpty())
            {
                return Json(list.Where(x => x.PortalId == portalId));
            }
            return Json(list);
        }
        [HttpGet]
        public async Task<IEnumerable<object>> GetLanguage(string templateId)
        {
            //var data = await _tableMetadataBusiness.GetList(x=>x.TableType == TableTypeEnum.View)
            //var data = await _tableMetadataBusiness.GetList();
            //if (portalId.IsNotNullAndNotEmpty())
            //{
            //    return Json(data.Where(x => x.PortalId == portalId));
            //}

            var jsonList = new List<ResourceLanguageViewModel>();
            var template = await _templateBusiness.GetSingleById(templateId);
            var data = JToken.Parse(template.Json);
            JArray rows = (JArray)data.SelectToken("components");
            await BuildLanguageJson(rows, jsonList);
            jsonList.ForEach(x => { x.TemplateId = templateId; x.TemplateType = template.TemplateType; x.GroupCode = ResourceLanguageGroupCodeEnum.Udf; x.DataAction = DataActionEnum.Create; x.Status = StatusEnum.Active; });
            var actualList = await _resourceLanguageBusiness.GetList(x => x.TemplateId == templateId);
            var result = new List<ResourceLanguageViewModel>();
            foreach (var jsonItem in jsonList)
            {
                var item = actualList.Where(x => x.Code == jsonItem.Code).FirstOrDefault();
                if (item.IsNotNull())
                {
                    result.Add(item);
                }
                else
                {
                    result.Add(jsonItem);
                }
            }
            return result.ToArray();
        }

        public async Task<ActionResult> ReadTemplateListByCategoryCodes(string categoryCodes)
        {
            var codes = categoryCodes.Split(",").ToArray();
            var catList = await _categoryBusiness.GetList(x => codes.Contains(x.Code));
            var idList = catList.Select(x => x.Id).ToArray();

            var model = await _templateBusiness.GetList(x => idList.Contains(x.TemplateCategoryId) && x.TemplateType == TemplateTypeEnum.Service);
            return Json(model);
        }
        [HttpGet]
        public async Task<ActionResult> GetTemplatesIdNameListByType(TemplateTypeEnum type)
        {
            var list = await _templateBusiness.GetList(x => x.TemplateType == type);
            return Json(list.Select(x => new IdNameViewModel { Id = x.Id, Name = x.DisplayName }));
        }
        public async Task<IActionResult> ManageOCRMapping(string templateId)
        {
            var model = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            var file = await _fileBusiness.GetSingle(x => x.Id == model.OcrTemplateFileId);
            if (file.IsNotNull())
            {
                ViewBag.FileName = file.FileName;
            }
            return View("_ManageOCRMapping", model);
        }
        [HttpGet]
        public async Task<IEnumerable<object>> GetOCRMapping(string templateId)
        {
            var jsonList = new List<OCRMappingViewModel>();
            var template = await _templateBusiness.GetSingleById(templateId);
            var data = JToken.Parse(template.Json);
            JArray rows = (JArray)data.SelectToken("components");
            await BuildOCRMappingJson(rows, jsonList);
            jsonList.ForEach(x => { x.TemplateId = templateId; x.DataAction = DataActionEnum.Create; x.Status = StatusEnum.Active; });
            var actualList = await _ocrMappingBusiness.GetList(x => x.TemplateId == templateId);
            var result = new List<OCRMappingViewModel>();
            foreach (var jsonItem in jsonList)
            {
                var item = actualList.Where(x => x.FieldName == jsonItem.FieldName).FirstOrDefault();
                if (item.IsNotNull())
                {
                    result.Add(item);
                }
                else
                {
                    result.Add(jsonItem);
                }
            }
            return result.ToArray();
        }
        private async Task BuildOCRMappingJson(JArray comps, List<OCRMappingViewModel> list)
        {

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
                                await BuildOCRMappingJson(rows, list);
                        }
                    }
                    else if (type == "table")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("rows");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                await BuildOCRMappingJson(rows, list);
                        }
                    }
                    else
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                        {
                            await BuildOCRMappingJson(rows, list);
                        }
                        else
                        {
                            var model = new OCRMappingViewModel();
                            JToken key = jcomp["key"];
                            model.FieldName = key.IsNotNull() ? key.Value<string>() : "";
                            list.Add(model);
                        }
                    }
                }
            }

        }
        [HttpGet]
        public async Task<JsonResult> SetOcrTemplateFileId(string templateId, string fileId)
        {
            await _templateBusiness.SetOcrTemplateFileId(templateId, fileId);
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> OcrMappingManage(OCRMappingViewModel item)
        {
            if (item.Id.IsNullOrEmpty() || item.Id == "null")
            {
                item.Id = null;
                var res = await _ocrMappingBusiness.Create(item);
                return Json(res);
            }
            else
            {
                var res = await _ocrMappingBusiness.Edit(item);
                return Json(res);
            }

        }
    }
}
