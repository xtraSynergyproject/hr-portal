using Synergy.App.Business;using Synergy.App.Common;using Synergy.App.DataModel;using Synergy.App.Repository;using Synergy.App.WebUtility;using Synergy.App.ViewModel;using Kendo.Mvc.Extensions;using Kendo.Mvc.UI;using Microsoft.AspNetCore.Diagnostics;using Microsoft.AspNetCore.Http;using Microsoft.AspNetCore.Mvc;using Microsoft.AspNetCore.Mvc.Razor;using Microsoft.AspNetCore.Mvc.ViewFeatures;using Microsoft.CodeAnalysis.CSharp.Scripting;using Microsoft.CodeAnalysis.Scripting;using Microsoft.Extensions.Localization;using Microsoft.Extensions.Logging;using Newtonsoft.Json;using Newtonsoft.Json.Linq;using System;using System.Collections.Generic;using System.Data;using System.Diagnostics;using System.IO;using System.Linq;using System.Reflection;using System.Threading.Tasks;namespace CMS.UI.Web.Controllers{    public class HomeController : ApplicationController    {        private readonly IStringLocalizer<HomeController> _localizer;        private readonly IBreMasterMetadataBusiness _breMasterMetadataBusiness;        private readonly IPageBusiness _pageBusiness;        private readonly IPortalBusiness _portalBusiness;        private readonly IRazorViewEngine _razorViewEngine;        private readonly IHttpContextAccessor _contextAccessor;        private readonly ITempDataProvider _tempDataProvider;        private readonly ITemplateBusiness _templateBusiness;        private readonly ITableMetadataBusiness _tableDataBusiness;        private readonly IColumnMetadataBusiness _columnDataBusiness;        private readonly ICmsBusiness _cmsBusiness;        private readonly IFormTemplateBusiness _formTemplateBusiness;        private readonly INoteTemplateBusiness _noteTemplateBusiness;        private readonly ITaskTemplateBusiness _taskTemplateBusiness;        private readonly IServiceTemplateBusiness _serviceTemplateBusiness;        private readonly ITableMetadataBusiness _tableMetadataBusiness;        private readonly IUserBusiness _userBusiness;        private readonly IServiceProvider _sp;        private readonly ILogger _logger;        public HomeController(IBreMasterMetadataBusiness breMasterMetadataBusiness            , IPageBusiness pageBusiness            , IPortalBusiness portalBusiness, IRazorViewEngine razorViewEngine            , IHttpContextAccessor contextAccessor            , ITempDataProvider tempDataProvider            , ITemplateBusiness templateBusiness            , ITableMetadataBusiness tableDataBusiness            , IColumnMetadataBusiness columnDataBusiness            , ICmsBusiness cmsBusiness            , IFormTemplateBusiness formTemplateBusiness,            INoteTemplateBusiness noteTemplateBusiness, ITaskTemplateBusiness taskTemplateBusiness,            IServiceTemplateBusiness serviceTemplateBusiness            , ITableMetadataBusiness tableMetadataBusiness            , IUserBusiness userBusiness            , IStringLocalizer<HomeController> localizer            , IServiceProvider sp            , ILogger<ApplicationError> logger)        {            _breMasterMetadataBusiness = breMasterMetadataBusiness;            _pageBusiness = pageBusiness;            _portalBusiness = portalBusiness;            _razorViewEngine = razorViewEngine;            _contextAccessor = contextAccessor;            _tempDataProvider = tempDataProvider;            _templateBusiness = templateBusiness;            _tableDataBusiness = tableDataBusiness;            _columnDataBusiness = columnDataBusiness;            _cmsBusiness = cmsBusiness;            _formTemplateBusiness = formTemplateBusiness;            _noteTemplateBusiness = noteTemplateBusiness;            _taskTemplateBusiness = taskTemplateBusiness;            _serviceTemplateBusiness = serviceTemplateBusiness;            _tableMetadataBusiness = tableMetadataBusiness;            _userBusiness = userBusiness;            _localizer = localizer;            _sp = sp;            _logger = logger;        }        string ReportsFolder;


        //[Route("/")]
        // [HttpGet("{portalName?}/{pageName?}")]
        public async Task<IActionResult> Index([FromRoute] string portalName, [FromRoute] string pageName, string mode, string source, string id, string pageUrl)        {            var runningMode = RunningModeEnum.Preview;            if (mode != null && mode.ToLower() == "preview")            {                runningMode = RunningModeEnum.Preview;            }            var requestSource = RequestSourceEnum.Main;            if (source.IsNotNullAndNotEmpty())            {                requestSource = source.ToEnum<RequestSourceEnum>();            }
            //var result = await LoadCms(portalName, pageName, runningMode, requestSource, id, pageUrl);
            //if (result != null)
            //{
            //    return result;
            //}
            return RedirectToAction("index", "content", new { @area = "cms" });        }
        //public IActionResult FastReport(int? reportIndex = 0)
        //{
        //    ReportsFolder = FindReportsFolder();
        //    var model = new HomeModel()
        //    {
        //        WebReport = new WebReport(),
        //        ReportsList = new[]
        //        {

        //            "test",
        //            //"Master-Detail",
        //            //"Badges",
        //            //"Interactive Report, 2-in-1",
        //            //"Hyperlinks, Bookmarks",
        //            //"Outline",
        //            //"Complex (Hyperlinks, Outline, TOC)",
        //            //"Drill-Down Groups",
        //            //"Mail Merge"
        //        },
        //    };

        //    var reportToLoad = model.ReportsList[0];
        //    if (reportIndex >= 0 && reportIndex < model.ReportsList.Length)
        //        reportToLoad = model.ReportsList[reportIndex.Value];

        //    //model.WebReport.Report.Load(Path.Combine(ReportsFolder, $"{reportToLoad}.frx"));
        //    //var k = model.WebReport.Report.Dictionary.Connections[0].ConnectionString;
        //    //var sdsd = Crypter.DecryptString(k);

        //    //var dataSet = new DataSet();
        //    //dataSet.ReadXml(Path.Combine(ReportsFolder, "nwind.xml"));

        //    //model.WebReport.Report.RegisterData(dataSet, "NorthWind");


        //    RegisteredObjects.AddConnection(typeof(JsonDataConnection));
        //    var connection = new JsonDataConnection();
        //    connection.ConnectionString = "Json=https://95.111.235.64:446/cms/query/ColumnIdNameListAlias?tableName=public.User";
        //    connection.CreateAllTables();
        //    model.WebReport.Report.Dictionary.Connections.Add(connection);
        //    model.WebReport.Report.RegisterData(connection.DataSet);
        //    model.WebReport.Report.Load(Path.Combine(ReportsFolder, $"{reportToLoad}.frx"));

        //    return View(model);
        //}
        private readonly Assembly[] DefaultReferences =
{            typeof(TaskTemplateViewModel).Assembly,            typeof(TaskTemplate).Assembly,            typeof(TaskTemplateBusiness).Assembly,            typeof(IRepositoryBase<,>).Assembly,            typeof(Enumerable).Assembly,            typeof(List<string>).Assembly,        };        public async Task<IActionResult> TestDate()        {            return View();        }
        public async Task<IActionResult> Test()        {            return View();        }
        public async Task<IActionResult> ProjectProposal()        {            return View();        }        public async Task<IActionResult> TestReport()        {            ViewBag.Orders = "61e629812b03a5f3715b7101";            return View();        }        public async Task<ActionResult> GetSwitchToUserVirtualData(string page, string pageSize, dynamic filter, string filters)        {            var data = await _cmsBusiness.GetList<TeamViewModel, Team>();
            //if (request.Filters != null)
            //{
            //    request.Filters.Clear();
            //}
            return Json(new { Data = data.Take(40).Select(x => new { Id = x.Id, Name = x.Name }), Total = 78 });        }
        public async Task<ActionResult> GetGridData()        {            var data = await _cmsBusiness.GetList<TeamViewModel, Team>();
            //if (request.Filters != null)
            //{
            //    request.Filters.Clear();
            //}
            //return Json(new { Data = data.Take(40).Select(x => new { Id = x.Id, Name = x.Name }), Total = 78 });
            return Json(data);        }        public async Task<ActionResult> GetSwitchUserValueMapper(string value)        {            var dataItemIndex = -1;            var data = await _cmsBusiness.GetList<TeamViewModel, Team>();            if (value != null)            {                var item = data.FirstOrDefault(x => x.Id == value);                dataItemIndex = data.IndexOf(item);            }            return Json(dataItemIndex);        }
        //public async Task<IActionResult> Test()
        //{

        //    var globals = new Params();
        //    globals.input = new TaskViewModel { Id = "123" };

        //    string script = @$"select c.*,t.""Name"" as ""TableName"" from public.""ColumnMetadata"" c
        //    join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""Schema""='log' 
        //    and c.""ForeignKeyConstraintName"" is not null";
        //    var b = _sp.GetService<IRepositoryQueryBase<ColumnMetadataViewModel>>();
        //    var data = await b.ExecuteQueryList<ColumnMetadataViewModel>(script, null);
        //    foreach (var item in data)
        //    {
        //        item.ForeignKeyConstraintName = item.ForeignKeyConstraintName.Replace(item.TableName.Replace("Log", ""), item.TableName);
        //        await _pageBusiness.Edit<ColumnMetadataViewModel, ColumnMetadata>(item);

        //    }
        //    return await Task.FromResult(View("UpdateTableMetadata", new IdNameViewModel()));
        //    var result = await CSharpScript.EvaluateAsync<TaskViewModel>(script, options:
        //       ScriptOptions.Default // This adds a reference, assuming MyType is a type defined within the MyCompany.MyNamespace
        //       .WithImports("Synergy.App.ViewModel")
        //       .WithReferences(typeof(TaskViewModel).Assembly), globals: globals);



        //    return await Task.FromResult(View("UpdateTableMetadata", new IdNameViewModel()));
        //}
        private string GenerateScript(string methodName, string dynamicScript)        {            return string.Concat(@"            using System; namespace CMS.UI.Web.Controllers{            public class Script : IScript            {                ", methodName, @"                {                ", dynamicScript                , @"                }            }}");        }        private string GenerateMethod(string script)        {            return string.Concat(@" public bool DecisionScript(TaskTemplateViewModel taskViewModel, ServiceTemplateViewModel serviceViewModel, NoteTemplateViewModel noteViewModel, Dictionary<string, object> udf)                {                      ", script, @"                }");        }        public async Task<IActionResult> UpdateTableMetadata()        {            return await Task.FromResult(View("UpdateTableMetadata", new IdNameViewModel()));        }        public async Task<IActionResult> UpdateTemplates()        {            var ntb = _sp.GetService<INoteTemplateBusiness>();            var notes = await ntb.GetList();            foreach (var note in notes.OrderByDescending(x => x.LastUpdatedDate))            {                try                {                    note.DataAction = DataActionEnum.Edit;                    var template = await ntb.GetSingleById<TemplateViewModel, Template>(note.TemplateId);                    if (template != null)                    {                        note.Json = template.Json;                        await _noteTemplateBusiness.Edit(note);                    }                }                catch (Exception ex)                {                    var error = ex.ToString();                }            }            return await Task.FromResult(View("UpdateTableMetadata", new IdNameViewModel()));        }        public async Task<IActionResult> EncryptPassword()        {            var nts = _sp.GetService<INtsBusiness>();            await nts.UpdateOverdueNts(DateTime.Now);            var userList = await _userBusiness.GetList();            foreach (var user in userList)            {                if (user.Password.IsNotNullAndNotEmpty())                {                    try                    {                        user.Password = Helper.Encrypt(user.Password);                        user.ConfirmPassword = user.Password;                        var result = await _userBusiness.Edit(user);                        if (result.IsSuccess)                        {
                            //var dec = Helper.Decrypt(user.Password);
                        }                        else                        {                        }                    }                    catch (Exception ex)                    {                        throw;
                        //try
                        //{
                        //    user.Password = Helper.Encrypt(user.Password);
                        //}
                        //catch (Exception ex2)
                        //{

                        //    throw;
                        //}

                    }                }            }            return Content("Success");        }        [HttpPost]        public async Task<IActionResult> SubmitUpdateTableMetadata(IdNameViewModel model)        {            await _tableMetadataBusiness.UpdateStaticTables(model.Name);            return await Task.FromResult(View("UpdateTableMetadata", "Done"));        }

        //[Route("portal/{portal?}/{page?}/{id?}")]
        //public async Task<IActionResult> Portal(string portal, string page, string id, string pageUrl)
        //{

        //    var runningMode = RunningModeEnum.Preview;
        //    var requestSource = RequestSourceEnum.Main;

        //    //if (mode != null && mode.ToLower() == "preview")
        //    //{
        //    //    runningMode = RunningModeEnum.Preview;
        //    //}

        //    //if (source.IsNotNullAndNotEmpty())
        //    //{
        //    //    requestSource = source.ToEnum<RequestSourceEnum>();
        //    //}
        //    var result = await LoadCms(portal, page, runningMode, requestSource, id, pageUrl);
        //    if (result != null)
        //    {
        //        return result;
        //    }
        //    return RedirectToAction("index", "content", new { @area = "cms" });
        //}

        //public async Task<IActionResult> LoadFormIndexPageGrid([DataSourceRequest] DataSourceRequest request, string indexPageTemplateId)
        //{
        //    var dt = await _cmsBusiness.GetFormIndexPageGridData(indexPageTemplateId, request);
        //    return Json(dt.ToDataSourceResult(request));
        //}
        public async Task<IActionResult> LoadFormIndexPageGrid(string indexPageTemplateId)        {            var dt = await _cmsBusiness.GetFormIndexPageGridData(indexPageTemplateId, null);            return Json(dt);        }        public async Task<IActionResult> LoadNoteIndexPageGrid([DataSourceRequest] DataSourceRequest request, string indexPageTemplateId, NtsActiveUserTypeEnum ownerType)        {            var dt = await _cmsBusiness.GetFormIndexPageGridData(indexPageTemplateId, request);            return Json(dt.ToDataSourceResult(request));        }        public async Task<IActionResult> Page(string id, string pageType, string source, string dataAction, string recordId)        {            var page = await _pageBusiness.GetPageForExecution(id);            if (page == null)            {                return Json(new { view = "", uiJson = "", dataJson = "" });            }            if (source.IsNullOrEmpty())            {                page.RequestSource = RequestSourceEnum.Main;            }            else            {                page.RequestSource = source.ToEnum<RequestSourceEnum>();            }            if (dataAction.IsNullOrEmpty())            {                page.DataAction = DataActionEnum.None;            }            else            {                page.DataAction = dataAction.ToEnum<DataActionEnum>();            }            page.RecordId = recordId;            var viewName = page.PageType;            var viewModel = await GetViewModelForPage(page, viewName);            var enableIndex = (bool)viewModel.EnableIndexPage;            if (page.RequestSource == RequestSourceEnum.Main)            {                switch (page.PageType)                {                    case TemplateTypeEnum.FormIndexPage:                        break;                    case TemplateTypeEnum.Page:                        break;                    case TemplateTypeEnum.Form:                        if (enableIndex)                        {                            viewName = TemplateTypeEnum.FormIndexPage;                            viewModel = await GetViewModelForPage(page, viewName);                        }                        break;                    case TemplateTypeEnum.Note:                        if (enableIndex)                        {                            viewName = TemplateTypeEnum.NoteIndexPage;                            viewModel = await GetViewModelForPage(page, viewName);                        }                        break;                    case TemplateTypeEnum.Task:                        if (enableIndex)                        {                            viewName = TemplateTypeEnum.TaskIndexPage;                            viewModel = await GetViewModelForPage(page, viewName);                        }                        break;                    case TemplateTypeEnum.Service:                        if (enableIndex)                        {                            viewName = TemplateTypeEnum.FormIndexPage;                            viewModel = await GetViewModelForPage(page, viewName);                        }                        break;                    case TemplateTypeEnum.Custom:                        break;                    default:                        break;                }            }            var data = await GetModelForPage(viewName, page, recordId);            var viewStr = await RenderViewToStringAsync(viewName.ToString(), viewModel, _contextAccessor, _razorViewEngine, _tempDataProvider);            return Json(new { view = viewStr, uiJson = page.Template.Json, dataJson = data });        }        private async Task<string> GetModelForPage(TemplateTypeEnum viewName, PageViewModel page, string recordId)        {            if (page.DataAction != DataActionEnum.Edit)            {                return "{}";            }            switch (viewName)            {                case TemplateTypeEnum.Form:                case TemplateTypeEnum.Note:                case TemplateTypeEnum.Task:                case TemplateTypeEnum.Service:                    var dr = await _cmsBusiness.GetDataById(viewName, page, recordId);                    if (dr != null)                    {                        return dr.ToJson();                    }                    return "{}";                case TemplateTypeEnum.Custom:                    break;                case TemplateTypeEnum.FormIndexPage:                case TemplateTypeEnum.Page:                default:                    return null;            }            return null;        }        private async Task<dynamic> GetViewModelForPage(PageViewModel page, TemplateTypeEnum viewName)        {            return viewName switch            {                TemplateTypeEnum.FormIndexPage => await GetIndexPageViewModel(page),                TemplateTypeEnum.Form => await GetFormViewModel(page),                TemplateTypeEnum.Note => await GetNoteViewModel(page),                TemplateTypeEnum.Task => await GetTaskViewModel(page),                TemplateTypeEnum.Service => await GetServiceViewModel(page),                TemplateTypeEnum.Custom => await GetCustomViewModel(page),                _ => await PageView(page),            };        }

        //public async Task<IActionResult> GetPageContent(string id)
        //{
        //    var page = await _pageBusiness.GetSingleById(id);
        //    return Json(page);
        //}

        //private async Task<IActionResult> GetPage(PageViewModel page)
        //{
        //    if (page == null)
        //    {
        //        return NotFound();
        //    }

        //    return page.PageType switch
        //    {
        //        TemplateTypeEnum.IndexPage => await IndexPageView(page),
        //        TemplateTypeEnum.Form => await FormView(page),
        //        TemplateTypeEnum.Note => await NoteView(page),
        //        TemplateTypeEnum.Task => await TaskView(page),
        //        TemplateTypeEnum.Service => await ServiceView(page),
        //        TemplateTypeEnum.Custom => await ServiceView(page),
        //        _ => await PageView(page),
        //    };
        //}

        private async Task<FormIndexPageTemplateViewModel> GetIndexPageViewModel(PageViewModel page)        {            var model = await _cmsBusiness.GetFormIndexPageViewModel(page);            model.Page = page;            model.PageId = page.Id;            return model;        }        public async Task<FormTemplateViewModel> GetFormViewModel(PageViewModel page)        {            var model = await _formTemplateBusiness.GetSingle(x => x.TemplateId == page.TemplateId);            model.Page = page;            model.PageId = page.Id;            model.DataAction = page.DataAction;            model.RecordId = page.RecordId;            model.PortalName = page.Portal.Name;
            //model.TemplateId = page.TemplateId;
            return model;        }

        //private async Task<IActionResult> LoadCms(string portalName, string pageName, RunningModeEnum runningMode, RequestSourceEnum requestSource, string id, string pageUrl)
        //{

        //    PortalViewModel portal = null;
        //    var domain = string.Concat(Request.IsHttps ? "https://" : "http://", Request.Host.Value).ToLower();
        //    if (portalName.IsNullOrEmpty())
        //    {
        //        portal = await _portalBusiness.GetSingleGlobal(x => x.DomainName == domain);
        //        if (portal == null)
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        portal = await _portalBusiness.GetSingleGlobal(x => x.Name == portalName);
        //        if (portal == null)
        //        {
        //            return NotFound();
        //        }
        //    }
        //    if (!Request.HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Login", "Account", new { portalId = portal.Id, returnUrl = Request.Path });
        //    }
        //    portal.DomainName = domain;
        //    var page = new PageViewModel();
        //    if (pageName.IsNullOrEmpty())
        //    {
        //        page = await _pageBusiness.GetDefaultPageDataByPortal(portal, runningMode);
        //    }
        //    else
        //    {
        //        page = await _pageBusiness.GetPageDataForExecution(portal.Id, pageName, runningMode);
        //        if (page == null)
        //        {
        //            return NotFound();
        //        }
        //    }
        //    if (page == null)
        //    {
        //        return NotFound();
        //    }
        //    var pagePermission = await _pageBusiness.GetUserPagePermission(portal.Id, page.Id);
        //    if (pagePermission == null)
        //    {
        //        var view = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/NotAuthorize.cshtml";
        //        return View(view, new LoginViewModel { PortalId = portal.Id });
        //    }
        //    var menus = await _portalBusiness.GetMenuItems(portal,null,null);
        //    if (menus != null && menus.Count > 0)
        //    {
        //        var landingPage = menus.FirstOrDefault(x => x.IsRoot == true);
        //        if (landingPage != null)
        //        {
        //            portal.LandingPage = string.Concat("/Portal/", portal.Name, "/", landingPage.Name);
        //        }
        //    }
        //    if (pageUrl.IsNotNullAndNotEmpty())
        //    {
        //        portal.LandingPage = pageUrl;
        //        var myUri = new Uri(string.Concat("http://localhost?", pageUrl));
        //        string pageType = HttpUtility.ParseQueryString(myUri.Query).Get("pageType");
        //        ViewBag.PageType = pageType;
        //    }
        //    page.Portal = portal;
        //    page.RunningMode = runningMode;
        //    page.RequestSource = requestSource;

        //    ViewBag.Menus = menus.Where(x => x.IsHidden == false).ToList();
        //    ViewBag.Title = string.Concat(page.Title, " - ", portal.DisplayName);
        //    ViewBag.Portal = portal;

        //    return View("cms", page);
        //}

        private async Task<PageTemplateViewModel> PageView(PageViewModel page)        {            var model = new PageTemplateViewModel();            return model;        }        [HttpPost]        public async Task<IActionResult> ManageForm(FormTemplateViewModel model)        {            var result = await _cmsBusiness.ManageForm(model);            return Redirect($"~/Portal/{model.PortalName}");            var template = await _templateBusiness.GetSingleById(model.TemplateId);            var tabledata = await _tableDataBusiness.GetSingleById(template.TableMetadataId);            tabledata.ColumnMetadataView = new List<ColumnMetadataViewModel>();            tabledata.ColumnMetadataView = await _columnDataBusiness.GetList(x => x.TableMetadataId == template.TableMetadataId);            var jsonResult = JObject.Parse(model.Json);            foreach (var item in tabledata.ColumnMetadataView)            {                var valObj = jsonResult.SelectToken(item.Name);                if (valObj != null)                {                    item.Value = valObj;                }            }        }        [HttpPost]        public async Task<IActionResult> ManageNote(NoteTemplateViewModel model)        {
            //if (model.DataAction == DataActionEnum.Create)
            //{
            //    await _cmsBusiness.CreateCmsRecord(model.Json, model.PageId);
            //}
            //else
            //{
            //    await _cmsBusiness.ManageForm(model.RecordId, model.Json, model.PageId);
            //}

            return Redirect($"~/Portal/{model.PortalName}");            var template = await _templateBusiness.GetSingleById(model.TemplateId);            var tabledata = await _tableDataBusiness.GetSingleById(template.TableMetadataId);            tabledata.ColumnMetadataView = new List<ColumnMetadataViewModel>();            tabledata.ColumnMetadataView = await _columnDataBusiness.GetList(x => x.TableMetadataId == template.TableMetadataId);            var result = JObject.Parse(model.Json);            foreach (var item in tabledata.ColumnMetadataView)            {                var valObj = result.SelectToken(item.Name);                if (valObj != null)                {                    item.Value = valObj;                }            }        }        [HttpPost]        public async Task<IActionResult> ManageTask(TaskTemplateViewModel model)        {
            //if (model.DataAction == DataActionEnum.Create)
            //{
            //    await _cmsBusiness.CreateCmsRecord(model.Json, model.PageId);
            //}
            //else
            //{
            //    await _cmsBusiness.EditForm(model.RecordId, model.Json, model.PageId);
            //}

            return Redirect($"~/Portal/{model.PortalName}");            var template = await _templateBusiness.GetSingleById(model.TemplateId);            var tabledata = await _tableDataBusiness.GetSingleById(template.TableMetadataId);            tabledata.ColumnMetadataView = new List<ColumnMetadataViewModel>();            tabledata.ColumnMetadataView = await _columnDataBusiness.GetList(x => x.TableMetadataId == template.TableMetadataId);            var result = JObject.Parse(model.Json);            foreach (var item in tabledata.ColumnMetadataView)            {                var valObj = result.SelectToken(item.Name);                if (valObj != null)                {                    item.Value = valObj;                }            }        }        [HttpPost]        public async Task<IActionResult> ManageService(ServiceTemplateViewModel model)        {
            //if (model.DataAction == DataActionEnum.Create)
            //{
            //    await _cmsBusiness.CreateCmsRecord(model.Json, model.PageId);
            //}
            //else
            //{
            //    await _cmsBusiness.EditForm(model.RecordId, model.Json, model.PageId);
            //}

            return Redirect($"~/Portal/{model.PortalName}");            var template = await _templateBusiness.GetSingleById(model.TemplateId);            var tabledata = await _tableDataBusiness.GetSingleById(template.TableMetadataId);            tabledata.ColumnMetadataView = new List<ColumnMetadataViewModel>();            tabledata.ColumnMetadataView = await _columnDataBusiness.GetList(x => x.TableMetadataId == template.TableMetadataId);            var result = JObject.Parse(model.Json);            foreach (var item in tabledata.ColumnMetadataView)            {                var valObj = result.SelectToken(item.Name);                if (valObj != null)                {                    item.Value = valObj;                }            }        }        private Task<IActionResult> FormReadonlyView(PageViewModel page)        {            throw new NotImplementedException();        }        private async Task<NoteTemplateViewModel> GetNoteViewModel(PageViewModel page)        {            var model = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == page.TemplateId);            model.Page = page;            model.PageId = page.Id;            model.DataAction = page.DataAction;            model.RecordId = page.RecordId;            model.PortalName = page.Portal.Name;
            //model.TemplateId = page.TemplateId;
            return model;        }        private async Task<TaskTemplateViewModel> GetTaskViewModel(PageViewModel page)        {            var model = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == page.TemplateId);            model.Page = page;            model.PageId = page.Id;            model.DataAction = page.DataAction;            model.RecordId = page.RecordId;            model.PortalName = page.Portal.Name;
            //model.TemplateId = page.TemplateId;
            return model;        }        private async Task<ServiceTemplateViewModel> GetServiceViewModel(PageViewModel page)        {            var model = await _serviceTemplateBusiness.GetSingle(x => x.TemplateId == page.TemplateId);            model.Page = page;            model.PageId = page.Id;            model.DataAction = page.DataAction;            model.RecordId = page.RecordId;            model.PortalName = page.Portal.Name;
            //model.TemplateId = page.TemplateId;
            return model;        }        private async Task<IActionResult> GetCustomViewModel(PageViewModel page)        {            throw new NotImplementedException();        }        private async Task<IActionResult> ValidateRequest(PortalViewModel portal, string pageName, string id)        {            var isAuth = Request.HttpContext.User.Identity.IsAuthenticated;            if (!isAuth)            {
                //  var loginurl = $"/Identity/Account/LogIn?returnUrl={Request.Path}&theme={portal.Theme.ToString()}";
                return RedirectToAction("Login", "Account", new { portalId = portal.Id, returnUrl = Request.Path });            }            return null;        }

        //public async Task<IActionResult> cms([FromRoute] string portal , [FromRoute] string page,string id)
        //{
        //    if (!portal.IsNullOrEmpty())
        //    {

        //    }
        //    return RedirectToAction("index", "content", new { @area = "cms" });
        //}

        public async Task<IActionResult> Privacy()        {            var k = await _cmsBusiness.Test();            return View();        }        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]        public async Task<IActionResult> Error()        {            try            {
                //var ex = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                //if (ex != null && IsApplicationPath(ex.Path))
                //{
                //    //_logger.LogError(ex.ToString());
                //    var uc = _sp.GetService<IUserContext>();
                //    var exception = ex.Error;
                //    var pathString = ex.Path;
                //    var appError = new ApplicationError
                //    {
                //        Exception = exception.ToString(),
                //        ErrorMessage = exception.Message,
                //        Url = pathString,
                //        UserId = uc.UserId,
                //        Email = uc.Email,
                //        UserName = uc.Email
                //    };
                //    await _pageBusiness.Create<ApplicationError, ApplicationError>(appError);

                //}
            }            catch (Exception)            {            }            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });        }        private bool IsApplicationPath(string path)        {            var ret = path.Contains("/css/")                 || path.Contains("/js/")                 || path.Contains("/img/")                 || path.Contains(".css")                 || path.Contains(".js")                 || path.Contains(".jpg")                 || path.Contains(".png")                 ;            return !ret;        }        public async Task<IActionResult> PopupSuccess(string msg, bool erp = false, bool ebb = false, string bbUrl = "", string bbText = "", bool ecb = false, string cbUrl = "", string cbText = "")        {            return View(new SuccessViewModel            {                Message = msg,                EnableReloadParent = erp,                EnableBackButton = ebb,                BackButtonUrl = bbUrl,                BackButtonText = bbText,                EnableCreateNewButton = ecb,                CreateNewButtonUrl = cbUrl,                CreateNewButtonText = cbText            });        }        [HttpGet]        public IActionResult GetEnumIdNameList(string enumType, string exculdeItem1 = "", string exculdeItem2 = "")        {            var list = new List<IdNameViewModel>();            var t = string.Concat("Synergy.App.Common.", enumType, ", Synergy.App.Common");            Type type = Type.GetType(t);            if (type.IsEnum)            {                list = Enum.GetValues(type)                    .Cast<Enum>()                     .Where(i => i.ToString() != exculdeItem1 && i.ToString() != exculdeItem2)                    .Select(e => new IdNameViewModel()                    {                        Name = e.Description(),                        EnumId = Convert.ToInt32(e),                        Id = Enum.GetName(type, e)                    })                    .ToList();            }            return Json(list);        }        public JsonResult GetEnumAsTreeList(string id, string enumType)        {            var list = new List<TreeViewViewModel>();            if (id.IsNullOrEmpty())            {                list.Add(new TreeViewViewModel                {                    id = "Root",                    Name = "All",                    DisplayName = "All",                    ParentId = null,                    hasChildren = true,                    expanded = true,                    Type = "Root"                });            }            if (id == "Root")            {                var t = string.Concat("Synergy.App.Common.", enumType, ", Synergy.App.Common");                Type type = Type.GetType(t);                if (type.IsEnum)                {                    list = Enum.GetValues(type)                   .Cast<Enum>()                    //.Where(i => i.ToString() != exculdeItem1 && i.ToString() != exculdeItem2)                   .Select(e => new TreeViewViewModel()
                                                                                                                    {
                                                                                                                        id = Enum.GetName(type, e),
                                                                                                                        Name = e.Description(),
                                                                                                                        DisplayName = e.Description(),
                                                                                                                        ParentId = id,
                                                                                                                        hasChildren = false,
                                                                                                                        expanded = false,
                                                                                                                        Type = "Child"
                                                                                                                        //Name = e.Description(),
                                                                                                                        //EnumId = Convert.ToInt32(e),
                                                                                                                        //Id = Enum.GetName(type, e)
                                                                                                                    })                   .ToList();
                    //var enumList = EnumExtension.SelectListFor(typeof(type));
                    //if (enumList != null && enumList.Count() > 0)
                    //{
                    //    list.AddRange(enumList.Select(x => new TreeViewViewModel
                    //    {
                    //        id = x.Value.ToString(),
                    //        Name = x.Text.ToString(),
                    //        DisplayName = x.Text.ToString(),
                    //        ParentId = id,
                    //        hasChildren = false,
                    //        expanded = false,
                    //        Type = "Child"

                    //    }));
                    //}

                }            }            return Json(list.ToList());        }        public async Task<object> GetEnumAsFancyTreeList(string id, string enumType)        {            var list = new List<TreeViewViewModel>();            if (id.IsNullOrEmpty())            {                list.Add(new TreeViewViewModel                {                    id = "Root",                    Name = "All",                    DisplayName = "All",                    ParentId = null,                    hasChildren = true,                    expanded = true,                    Type = "Root"                });            }            if (id == "Root")            {                var t = string.Concat("Synergy.App.Common.", enumType, ", Synergy.App.Common");                Type type = Type.GetType(t);                if (type.IsEnum)                {                    list = Enum.GetValues(type)                   .Cast<Enum>()                    //.Where(i => i.ToString() != exculdeItem1 && i.ToString() != exculdeItem2)                   .Select(e => new TreeViewViewModel()
                                                                                                                    {
                                                                                                                        id = Enum.GetName(type, e),
                                                                                                                        Name = e.Description(),
                                                                                                                        DisplayName = e.Description(),
                                                                                                                        ParentId = id,
                                                                                                                        hasChildren = false,
                                                                                                                        expanded = false,
                                                                                                                        Type = "Child"
                                                                                                                        //Name = e.Description(),
                                                                                                                        //EnumId = Convert.ToInt32(e),
                                                                                                                        //Id = Enum.GetName(type, e)
                                                                                                                    })                   .ToList();
                    //var enumList = EnumExtension.SelectListFor(typeof(type));
                    //if (enumList != null && enumList.Count() > 0)
                    //{
                    //    list.AddRange(enumList.Select(x => new TreeViewViewModel
                    //    {
                    //        id = x.Value.ToString(),
                    //        Name = x.Text.ToString(),
                    //        DisplayName = x.Text.ToString(),
                    //        ParentId = id,
                    //        hasChildren = false,
                    //        expanded = false,
                    //        Type = "Child"

                    //    }));
                    //}

                }            }            var newList = new List<FileExplorerViewModel>();            newList.AddRange(list.ToList().Select(x => new FileExplorerViewModel { key = x.id, title = x.Name, lazy = true }));            var json = JsonConvert.SerializeObject(newList);            return json;
            //return Json(list.ToList());
        }    }    public interface IScript    {
        //  bool DecisionScript(TaskTemplateViewModel taskViewModel, ServiceTemplateViewModel serviceViewModel, NoteTemplateViewModel noteViewModel, Dictionary<string, object> udf);
        int Sum(int a, int b);    }    public class Params    {
        //  bool DecisionScript(TaskTemplateViewModel taskViewModel, ServiceTemplateViewModel serviceViewModel, NoteTemplateViewModel noteViewModel, Dictionary<string, object> udf);
        public TaskViewModel input { get; set; }    }}