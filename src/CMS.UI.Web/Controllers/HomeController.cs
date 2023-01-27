﻿using Synergy.App.Business;


        //[Route("/")]
        // [HttpGet("{portalName?}/{pageName?}")]
        public async Task<IActionResult> Index([FromRoute] string portalName, [FromRoute] string pageName, string mode, string source, string id, string pageUrl)
            //var result = await LoadCms(portalName, pageName, runningMode, requestSource, id, pageUrl);
            //if (result != null)
            //{
            //    return result;
            //}
            return RedirectToAction("index", "content", new { @area = "cms" });
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
{
        public async Task<IActionResult> Test()
        public async Task<IActionResult> ProjectProposal()
            //if (request.Filters != null)
            //{
            //    request.Filters.Clear();
            //}
            return Json(new { Data = data.Take(40).Select(x => new { Id = x.Id, Name = x.Name }), Total = 78 });
        public async Task<ActionResult> GetGridData()
            //if (request.Filters != null)
            //{
            //    request.Filters.Clear();
            //}
            //return Json(new { Data = data.Take(40).Select(x => new { Id = x.Id, Name = x.Name }), Total = 78 });
            return Json(data);
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
        private string GenerateScript(string methodName, string dynamicScript)
                            //var dec = Helper.Decrypt(user.Password);
                        }
                        //try
                        //{
                        //    user.Password = Helper.Encrypt(user.Password);
                        //}
                        //catch (Exception ex2)
                        //{

                        //    throw;
                        //}

                    }

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
        public async Task<IActionResult> LoadFormIndexPageGrid(string indexPageTemplateId)

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

        private async Task<FormIndexPageTemplateViewModel> GetIndexPageViewModel(PageViewModel page)
            //model.TemplateId = page.TemplateId;
            return model;

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

        private async Task<PageTemplateViewModel> PageView(PageViewModel page)
            //if (model.DataAction == DataActionEnum.Create)
            //{
            //    await _cmsBusiness.CreateCmsRecord(model.Json, model.PageId);
            //}
            //else
            //{
            //    await _cmsBusiness.ManageForm(model.RecordId, model.Json, model.PageId);
            //}

            return Redirect($"~/Portal/{model.PortalName}");
            //if (model.DataAction == DataActionEnum.Create)
            //{
            //    await _cmsBusiness.CreateCmsRecord(model.Json, model.PageId);
            //}
            //else
            //{
            //    await _cmsBusiness.EditForm(model.RecordId, model.Json, model.PageId);
            //}

            return Redirect($"~/Portal/{model.PortalName}");
            //if (model.DataAction == DataActionEnum.Create)
            //{
            //    await _cmsBusiness.CreateCmsRecord(model.Json, model.PageId);
            //}
            //else
            //{
            //    await _cmsBusiness.EditForm(model.RecordId, model.Json, model.PageId);
            //}

            return Redirect($"~/Portal/{model.PortalName}");
            //model.TemplateId = page.TemplateId;
            return model;
            //model.TemplateId = page.TemplateId;
            return model;
            //model.TemplateId = page.TemplateId;
            return model;
                //  var loginurl = $"/Identity/Account/LogIn?returnUrl={Request.Path}&theme={portal.Theme.ToString()}";
                return RedirectToAction("Login", "Account", new { portalId = portal.Id, returnUrl = Request.Path });

        //public async Task<IActionResult> cms([FromRoute] string portal , [FromRoute] string page,string id)
        //{
        //    if (!portal.IsNullOrEmpty())
        //    {

        //    }
        //    return RedirectToAction("index", "content", new { @area = "cms" });
        //}

        public async Task<IActionResult> Privacy()
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
            }
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
                                                                                                                    })
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

                }
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
                                                                                                                    })
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

                }
            //return Json(list.ToList());
        }
        //  bool DecisionScript(TaskTemplateViewModel taskViewModel, ServiceTemplateViewModel serviceViewModel, NoteTemplateViewModel noteViewModel, Dictionary<string, object> udf);
        int Sum(int a, int b);
        //  bool DecisionScript(TaskTemplateViewModel taskViewModel, ServiceTemplateViewModel serviceViewModel, NoteTemplateViewModel noteViewModel, Dictionary<string, object> udf);
        public TaskViewModel input { get; set; }