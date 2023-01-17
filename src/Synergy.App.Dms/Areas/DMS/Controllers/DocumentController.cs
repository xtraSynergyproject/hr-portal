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
//////using Kendo.Mvc.UI;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net.Http;
//File Manager's base functions are available in the below package
//using Syncfusion.EJ2.FileManager.Base;
//File Manager's operations are available in the below package
//using Syncfusion.EJ2.FileManager.PhysicalFileProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http.Features;
using Synergy.App.DataModel;
using Synergy.App.Business.Interface.DMS;
using Telerik.Web.Spreadsheet;
using System.Web;
using System.Runtime.Serialization.Json;
using System.Text;
using SpreadsheetLight;
using Synergy.App.Common.Utilities;
using ICSharpCode.SharpZipLib.Zip;
using Synergy.App.WebUtility;
using Nest;
using ProtoBuf.Meta;
using Telerik.Windows.Documents.Fixed.Model.Objects;
using AutoMapper.Configuration.Annotations;
using UglyToad.PdfPig.Graphics;
using Microsoft.VisualBasic;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace CMS.UI.Web.Areas.DMS.Controllers


{


    [Area("DMS")]
    public class DocumentController : ApplicationController
    {
        private readonly IDMSDocumentBusiness _documentBusiness;
        private readonly IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IWebHelper _webApi;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        private readonly IListOfValueBusiness _lovBusiness;
        private readonly IFileBusiness _fileBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IDocumentPermissionBusiness _documentPermissionBusiness;
        private readonly IPageBusiness _pageBusiness;
        private readonly INtsTagBusiness _tagBusiness;
        private readonly INtsStagingBusiness _stagingBusiness;
        private readonly IHierarchyMasterBusiness _hierarchyBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        // Accessing the File Operations from File Manager package
        // PhysicalFileProvider operation = new PhysicalFileProvider();
        public string basePath;
        string root = "wwwroot\\documents";
        public DocumentController(IUserContext userContext, INoteBusiness noteBusiness, IWebHelper webApi, IHostingEnvironment hostingEnvironment,
            ITemplateBusiness templateBusiness, ITemplateCategoryBusiness templateCategoryBusiness, IUserBusiness userBusiness,
            IListOfValueBusiness lovBusiness, IFileBusiness fileBusiness, IServiceBusiness serviceBusiness,
            ITaskBusiness taskBusiness, IPageBusiness pageBusiness, INtsTagBusiness tagBusiness, INtsStagingBusiness stagingBusiness,
            IDMSDocumentBusiness documentBusiness, IDocumentPermissionBusiness documentPermissionBusiness, IHierarchyMasterBusiness hierarchyBusiness,
             Microsoft.Extensions.Configuration.IConfiguration configuration, ITableMetadataBusiness tableMetadataBusiness)
        {
            _hierarchyBusiness = hierarchyBusiness;
            _userContext = userContext;
            _noteBusiness = noteBusiness;
            _webApi = webApi;
            _templateBusiness = templateBusiness;
            _templateCategoryBusiness = templateCategoryBusiness;
            _documentBusiness = documentBusiness;
            _documentPermissionBusiness = documentPermissionBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            // Map the path of the files to be accessed with the host
            //string webRootPath = _HostEnvironment.WebRootPath;
            //string path = "";
            //path = Path.Combine(webRootPath, "documents");
            // Map the path of the files to be accessed with the host
            this.basePath = hostingEnvironment.ContentRootPath;
            //this.operation = new PhysicalFileProvider();
            // Assign the mapped path as root folder
            //this.operation.RootFolder(this.basePath + "\\" + this.root);
            _userBusiness = userBusiness;
            _lovBusiness = lovBusiness;
            _fileBusiness = fileBusiness;
            _serviceBusiness = serviceBusiness;
            _taskBusiness = taskBusiness;
            _pageBusiness = pageBusiness;
            _tagBusiness = tagBusiness;
            _stagingBusiness = stagingBusiness;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string id)
        {
            //For Updating Document Count
            //var documents = await _documentBusiness.GetAllDocuments();
            //foreach (var document in documents)
            //{
            //    var taglist = await _tagBusiness.GetList(x => x.TagSourceReferenceId == document.Id && x.TagId == null);
            //    if (taglist.Any())
            //    {
            //        continue;
            //    }
            //    var parentlist = await _documentBusiness.GetAllParentByNoteId(document.Id);
            //    foreach (var parent in parentlist)
            //    {
            //        var tag = new NtsTagViewModel { NtsId = parent.Id, NtsType = NtsTypeEnum.Note, TagSourceReferenceId = document.Id };
            //        var res = await _tagBusiness.Create(tag);
            //    }
            //}


            var exists = await _documentBusiness.CheckMyWorkspaceExist(_userContext.UserId);
            if (exists == false)
            {
                await _userBusiness.CreateUserWorkSpace(_userContext.UserId);
            }
            string path = "/";
            if (id.IsNotNullAndNotEmpty())
            {
                var note = await _noteBusiness.GetSingle(x => x.Id == id);
                if (note.IsNotNull())
                {
                    path += id + "/";
                    if (note.ParentNoteId.IsNotNullAndNotEmpty())
                    {
                        path = await GetParentNote(note, path);
                    }
                    var array = path.Split('/');
                    Array.Reverse(array);
                    path = String.Join('/', array);
                }
            }
            var model = new DirectoryContent { path = path };
            return View("FileManager", model);
        }
        public async Task<IActionResult> FileExplorer(string id)
        {
            var exists = await _documentBusiness.CheckMyWorkspaceExist(_userContext.UserId);
            if (exists == false)
            {
                await _userBusiness.CreateUserWorkSpace(_userContext.UserId);
            }
            ViewBag.Key = id;
            return View();
        }
        public async Task<IActionResult> FileExplorerLeft(string id)
        {
            var exists = await _documentBusiness.CheckMyWorkspaceExist(_userContext.UserId);
            if (exists == false)
            {
                await _userBusiness.CreateUserWorkSpace(_userContext.UserId);
            }
            ViewBag.Key = id;
            return View();
        }
        public async Task<IActionResult> FileExplorerRight(string id)
        {
            var exists = await _documentBusiness.CheckMyWorkspaceExist(_userContext.UserId);
            if (exists == false)
            {
                await _userBusiness.CreateUserWorkSpace(_userContext.UserId);
            }
            ViewBag.Key = id;
            return View();
        }

        public async Task<IActionResult> FileExplorerOld(string id)
        {
            var exists = await _documentBusiness.CheckMyWorkspaceExist(_userContext.UserId);
            if (exists == false)
            {
                await _userBusiness.CreateUserWorkSpace(_userContext.UserId);
            }
            ViewBag.Key = id;
            return View();
        }
        public async Task<JsonResult> GetDocumentsTreeList(string id, string parentId)
        {
            var list = new List<TreeViewViewModel>();
            //if (id.IsNullOrEmpty())
            //{               
            //    list.Add(new TreeViewViewModel
            //    {
            //        id = TemplateTypeEnum.Dashboard.ToString(),
            //        Name = TemplateTypeEnum.Dashboard.ToString(),
            //        DisplayName = TemplateTypeEnum.Dashboard.ToString(),
            //        ParentId = null,
            //        hasChildren = true,
            //        expanded = true,
            //        Type = "Root"
            //    });
            //}
            //if (id == TemplateTypeEnum.Dashboard.ToString())
            //{
            //    TemplateTypeEnum type = id.ToEnum<TemplateTypeEnum>();
            //    var dashboards = await _noteBusiness.GetAllDashboardMaster();
            //    list.AddRange(dashboards.Select(x => new TreeViewViewModel
            //    {
            //        id = x.Id,
            //        Name = x.NoteSubject,
            //        DisplayName = x.NoteSubject,
            //        ParentId = id,
            //        hasChildren = true,
            //        expanded = false,
            //        Type = "DashboardMaster"

            //    }));
            //}
            //if (id.IsNotNullAndNotEmpty() && id != TemplateTypeEnum.Dashboard.ToString())
            //{
            //    var dashboardItems = await _noteBusiness.GetList(x => x.ParentNoteId == id);
            //    list.AddRange(dashboardItems.Select(x => new TreeViewViewModel
            //    {
            //        id = x.Id,
            //        Name = x.NoteSubject,
            //        DisplayName = x.NoteSubject,
            //        ParentId = id,
            //        hasChildren = false,
            //        expanded = false,
            //        Type = "DashboardItem"

            //    }));
            //}



            return Json(list.ToList());
        }

        public IActionResult DocumentSearch()
        {
            return View();
        }

        public async Task<IActionResult> GetDocumentList(string id, string searchtext)
        {
            var list = new List<TreeViewViewModel>();
            var doclist = new List<WorkspaceViewModel>();

            if (id.IsNullOrEmpty())
            {
                doclist = await _documentBusiness.GetDocuments(_userContext.UserId, searchtext);
                if (doclist.IsNotNull())
                {
                    var r = doclist.Where(x => x.TemplateCode == "WORKSPACE_GENERAL" && x.ParentNoteId == null);

                    foreach (var item in r)
                    {
                        list.Add(new TreeViewViewModel
                        {
                            id = item.Id.ToString(),
                            Name = item.NoteSubject,
                            DisplayName = item.NoteSubject,
                            ParentId = item.ParentNoteId,
                            hasChildren = true,
                            expanded = true,
                            TemplateCode = item.TemplateCode,
                            Type = item.Type
                        });
                    }
                    return Json(list);
                }
            }
            else
            {
                doclist = await _documentBusiness.GetDocuments(_userContext.UserId, searchtext);
                if (doclist.IsNotNull())
                {
                    var r = doclist.Where(x => x.ParentNoteId == id);

                    foreach (var item in r)
                    {
                        list.Add(new TreeViewViewModel
                        {
                            id = item.Id.ToString(),
                            Name = item.NoteSubject,
                            DisplayName = item.NoteSubject,
                            ParentId = item.ParentNoteId,
                            hasChildren = true,
                            expanded = true,
                            TemplateCode = item.TemplateCode,
                            Type = item.Type
                        });
                    }
                    return Json(list);
                }
            }

            //var workspaces = await _documentBusiness.GetUserWorkspaceTreeDataNew(_userContext.UserId);
            //var workspaces = workspaces.Where(x => x.ParentId == parentId).ToList();
            ////var workspace = workspaces.FirstOrDefault();
            ////if (!workspace.IsNotNull())
            ////{
            ////    return Json(new object());
            ////}
            //var parent = workspaces1.Where(x => x.Id == parentId).FirstOrDefault();
            //if (parent != null)
            //{
            //    var value = new DirectoryContent { id = parentId, name = parent.Name, hasChild = (workspaces.Count() > 0) ? true : false, FolderType = parent.FolderType.Value /*(workspace.FolderCode == "WORKSPACE_GENERAL") ? FolderTypeEnum.Workspace: (workspace.FolderCode == "GENERAL_FOLDER") ?FolderTypeEnum.Folder :FolderTypeEnum.Document*/ };
            //    readResponse.cwd = JsonConvert.DeserializeObject<DirectoryContent>(JsonConvert.SerializeObject(value));
            //    if (parent.FolderCode == "GENERAL_FOLDER" /*|| parent.FolderCode == "WORKSPACE_GENERAL"*/)
            //    //if(workspace.TemplateCode == "GENERAL_FOLDER" || workspace.TemplateCode == "WORKSPACE_GENERAL")
            //    {
            //        //list = await _noteBusiness.GetAllChildDocuments(parentId);     
            //        list = await _noteBusiness.GetAllFolderDocuments(parentId);
            //    }
            //}
            return Json(list);
        }
        public async Task<ActionResult> GetWorkSpaceTreeList([DataSourceRequest] DataSourceRequest request, string id = null)
        {
            var result = await _documentBusiness.GetUserWorkspaceTreeDataNew(_userContext.UserId, null);
            var j = Json(result);
            // var j = Json(result.ToDataSourceResult(request));
            return j;
        }
        public async Task<IActionResult> Workspace(string id, string parentId, DataActionEnum dataAction)
        {
            var model = new WorkspaceViewModel();
            model.ActiveUserId = _userContext.UserId;
            if (dataAction == DataActionEnum.Create)
            {

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "WORKSPACE_GENERAL";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.ParentNoteId = parentId;
                model.CreatedBy = newmodel.CreatedBy;
                model.CreatedDate = System.DateTime.Now;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.NoteId = id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.NoteSubject = newmodel.NoteSubject;
                model.NoteId = id;
                model.CreatedBy = newmodel.CreatedBy;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageWorkspace(WorkspaceViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "WORKSPACE_GENERAL";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    //newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
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
                    //newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }


        public async Task<IActionResult> Folder(string id, string parentId, DataActionEnum dataAction)
        {
            var model = new DashboardItemMasterViewModel();
            model.ActiveUserId = _userContext.UserId;
            if (dataAction == DataActionEnum.Create)
            {

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "FOLDER_GENERAL";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.ParentNoteId = parentId;
                model.CreatedBy = newmodel.CreatedBy;
                model.CreatedDate = System.DateTime.Now;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.NoteId = id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                var udf = await _noteBusiness.GetDashboardItemMasterDetails(id);
                if (udf.IsNotNull())
                {
                    model.chartTypeId = udf.chartTypeId;
                    model.chartMetadata = udf.chartMetadata;
                    if (udf.measuresField.IsNotNullAndNotEmpty())
                    {
                        model.measuresArray = udf.measuresField.Split(',');
                    }
                    if (udf.dimensionsField.IsNotNullAndNotEmpty())
                    {
                        model.dimensionsArray = udf.dimensionsField.Split(',');
                    }
                    if (udf.segmentsField.IsNotNullAndNotEmpty())
                    {
                        model.segmentsArray = udf.segmentsField.Split(',');
                    }
                }
                model.NoteSubject = newmodel.NoteSubject;
                model.NoteId = id;
                model.CreatedBy = newmodel.CreatedBy;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageFolder(DashboardItemMasterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var validateNote = await _noteBusiness.GetList(x => x.NoteSubject == model.NoteSubject);
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "GENERAL_FOLDER";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.ParentNoteId = model.ParentNoteId;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }
                else
                {
                    var validateNote = await _noteBusiness.GetList(x => x.Id != model.NoteId && x.NoteSubject == model.NoteSubject);
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
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
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }

        //public async Task<object> FileOperations([FromBody] FileManagerDirectoryContent args)
        //{
        //    // Processing the File Manager operations
        //    switch (args.Action)
        //    {
        //        case "read":
        //            // Path - Current path; ShowHiddenItems - Boolean value to show/hide hidden items
        //            //var json= Json(operation.ToCamelCase(operation.GetFiles(args.Path, args.ShowHiddenItems)));
        //            //return json; 
        //            return await this.GetFiles(args);

        //        case "delete":
        //            // Path - Current path where of the folder to be deleted; Names - Name of the files to be deleted
        //            //return Json(operation.ToCamelCase(operation.Delete(args.Path, args.Names))); 
        //            Dictionary<string, string> deleteResult = new Dictionary<string, string>();
        //            for (int i = 0; i < args.Data.Length; i++)
        //            {
        //                deleteResult = await DeleteNote(args.Data[i].Id);
        //            }
        //            if (deleteResult.Count > 0)
        //            {
        //                FileResponse readResponse = new FileResponse();
        //                readResponse.files = new List<DirectoryContent>();
        //                readResponse.errorsList = "{" + string.Join(",", deleteResult.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        //                var json = JsonConvert.SerializeObject(readResponse);
        //                return json;
        //            }
        //            return await this.GetFiles(args);
        //        case "copy":
        //            //  Path - Path from where the file was copied; TargetPath - Path where the file/folder is to be copied; RenameFiles - Files with same name in the copied location that is confirmed for renaming; TargetData - Data of the copied file
        //            //return Json(operation.ToCamelCase(operation.Copy(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.TargetData)));
        //            Dictionary<string, string> Copyresult1 = new Dictionary<string, string>();
        //            if (args.TargetData.IsNotNull())
        //            {
        //                var targetId = args.TargetData.Id;
        //                for (int i = 0; i < args.Data.Length; i++)
        //                {
        //                    var sourceId = args.Data[i].Id;
        //                    Copyresult1 = await CopyNote(sourceId, targetId);
        //                }
        //                if (Copyresult1.Count > 0)
        //                {
        //                    FileResponse readResponse = new FileResponse();
        //                    readResponse.files = new List<DirectoryContent>();
        //                    //var data=await this.GetFiles(args);
        //                    //readResponse = JsonConvert.DeserializeObject<FileResponse>(JsonConvert.SerializeObject(data));
        //                    readResponse.errorsList = "{" + string.Join(",", Copyresult1.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        //                    var json = JsonConvert.SerializeObject(readResponse);
        //                    return json;
        //                }
        //            }
        //            return await this.GetFiles(args);
        //        case "move":
        //            // Path - Path from where the file was cut; TargetPath - Path where the file/folder is to be moved; RenameFiles - Files with same name in the moved location that is confirmed for renaming; TargetData - Data of the moved file
        //            //return Json(operation.ToCamelCase(operation.Move(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.TargetData)));
        //            Dictionary<string, string> result1 = new Dictionary<string, string>();
        //            if (args.TargetData.IsNotNull())
        //            {

        //                var targetId = args.TargetData.Id;
        //                var target = await _noteBusiness.GetSingleById(targetId);
        //                for (int i = 0; i < args.Data.Length; i++)
        //                {
        //                    var sourceId = args.Data[i].Id;
        //                    var source = await _noteBusiness.GetSingleById(sourceId);
        //                    //if (target.TemplateCode == "WORKSPACE_GENERAL" && source.TemplateCode != "GENERAL_FOLDER")
        //                    //{
        //                    //    return await this.GetFiles(args);
        //                    //}
        //                    result1 = await MoveNote(sourceId, targetId);
        //                }

        //            }
        //            if (result1.Count > 0)
        //            {
        //                FileResponse readResponse = new FileResponse();
        //                readResponse.files = new List<DirectoryContent>();
        //                //var data=await this.GetFiles(args);
        //                //readResponse = JsonConvert.DeserializeObject<FileResponse>(JsonConvert.SerializeObject(data));
        //                readResponse.errorsList = "{" + string.Join(",", result1.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        //                var json = JsonConvert.SerializeObject(readResponse);
        //                return json;
        //            }
        //            return await this.GetFiles(args);
        //        case "details":
        //            //if (args.Names == null)
        //            //{
        //            //    args.Names = new string[] { };
        //            //}
        //            //// Path - Current path where details of file/folder is requested; Name - Names of the requested folders
        //            //return Json(operation.ToCamelCase(operation.Details(args.Path, args.Names)));
        //            return Json(new object());
        //        case "create":
        //            // Path - Current path where the folder is to be created; Name - Name of the new folder
        //            //return Json(operation.ToCamelCase(operation.Create(args.Path, args.Name)));      
        //            try
        //            {
        //                var parentId = args.Data[0].Id;
        //                var folderName = args.Name;
        //                var templateModel = new NoteTemplateViewModel();
        //                templateModel.ActiveUserId = _userContext.UserId;
        //                templateModel.DataAction = DataActionEnum.Create;
        //                templateModel.TemplateCode = "GENERAL_FOLDER";
        //                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
        //                newmodel.NoteSubject = folderName;
        //                newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
        //                newmodel.ParentNoteId = parentId;
        //                newmodel.OwnerUserId = _userContext.UserId;
        //                var result = await _noteBusiness.ManageNote(newmodel);
        //                if (result.IsSuccess)
        //                {
        //                    return await this.GetFiles(args);
        //                }
        //                return await this.GetFiles(args);
        //            }
        //            catch (Exception ex)
        //            {

        //                return await this.GetFiles(args);
        //            }

        //        case "search":
        //            // Path - Current path where the search is performed; SearchString - String typed in the searchbox; CaseSensitive - Boolean value which specifies whether the search must be casesensitive
        //            //return Json(operation.ToCamelCase(operation.Search(args.Path, args.SearchString, args.ShowHiddenItems, args.CaseSensitive)));
        //            return await this.SearchFiles(args);
        //        case "rename":
        //            // Path - Current path of the renamed file; Name - Old file name; NewName - New file name
        //            //return Json(operation.ToCamelCase(operation.Rename(args.Path, args.Name, args.NewName)));
        //            try
        //            {
        //                var folderName = args.NewName;
        //                var templateModel = new NoteTemplateViewModel();
        //                templateModel.ActiveUserId = _userContext.UserId;
        //                templateModel.DataAction = DataActionEnum.Edit;
        //                templateModel.NoteId = args.Data[0].Id;
        //                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
        //                newmodel.NoteSubject = folderName;
        //                newmodel.DataAction = DataActionEnum.Edit;
        //                newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
        //                newmodel.OwnerUserId = _userContext.UserId;
        //                var result = await _noteBusiness.ManageNote(newmodel);
        //                if (result.IsSuccess)
        //                {
        //                    return await this.GetFiles(args);
        //                }
        //                return await this.GetFiles(args);

        //            }
        //            catch (Exception ex)
        //            {
        //                return await this.GetFiles(args);

        //            }

        //    }
        //    return Json(new object());
        //}
        // Processing the Upload operation
        //public ActionResult Upload(string path, IList<IFormFile> uploadFiles, string action)
        //{
        //    FileManagerResponse uploadResponse;
        //    //Invoking upload operation with the required paramaters
        //    // path - Current path where the file is to uploaded; uploadFiles - Files to be uploaded; action - name of the operation(upload)
        //    uploadResponse = operation.Upload(path, uploadFiles, action, null);

        //    return Content("");
        //}
        // Processing the Download operation
        public ActionResult Download(string downloadInput)
        {
            //FileManagerDirectoryContent args = JsonConvert.DeserializeObject<FileManagerDirectoryContent>(downloadInput);
            //Invoking download operation with the required paramaters
            // path - Current path where the file is downloaded; Names - Files to be downloaded;
            //return operation.Download(args.Path, args.Names);
            return Json(new object());
        }
        // Processing the GetImage operation
        //public ActionResult GetImage(FileManagerDirectoryContent args)
        //{
        //    //Invoking GetImage operation with the required paramaters
        //    // path - Current path of the image file; Id - Image file id;
        //    return operation.GetImage(args.Path, args.Id, false, null, null);
        //}
        private async Task<string> GetParentNote(NoteViewModel model, string path)
        {
            var note = await _noteBusiness.GetSingle(x => x.Id == model.ParentNoteId);
            if (note.IsNotNull())
            {
                path += note.Id + "/";
                if (note.ParentNoteId.IsNotNullAndNotEmpty())
                {
                    return await GetParentNote(note, path);
                }
                else
                {
                    return path;
                }

            }
            else
            {
                return path;
            }
        }
        //public async Task<object> GetFiles(FileManagerDirectoryContent args)
        //{
        //    FileResponse readResponse = new FileResponse();
        //    var list = new List<DirectoryContent>();
        //    try
        //    {                
        //        var pathIds = args.Path.Split('/');
        //        if (args.Data.Count() == 0 || pathIds.Count() == 2)
        //        {
        //            var user = new DirectoryContent { id = _userContext.UserId, name = _userContext.Name, hasChild = true, FolderType = FolderTypeEnum.Root,CanCreateWorkspace = _userContext.IsSystemAdmin };                    
        //            var workspaces = await _documentBusiness.GetFirstLevelWorkspacesByUser(_userContext.UserId);                   
        //            readResponse.cwd = JsonConvert.DeserializeObject<DirectoryContent>(JsonConvert.SerializeObject(user));
        //            list.AddRange(workspaces.Select(x => new DirectoryContent { id = x.Id, name = x.Name, hasChild = true, FolderType = FolderTypeEnum.Workspace, Count = x.DocCount.ToString(), WorkspaceId = x.WorkspaceId, parentId = _userContext.UserId, CanOpen = x.CanOpen, ShowMenu = x.ShowMenu, CanCreateSubFolder = x.CanCreateSubFolder, CanRename = x.CanRename, CanShare = x.CanShare, CanMove = x.CanMove, CanCopy = x.CanCopy, CanArchive = x.CanArchive, CanDelete = x.CanDelete, CanSeeDetail = x.CanSeeDetail, CanManagePermission = x.CanManagePermission, TemplateCode = x.FolderCode, CanCreateWorkspace = (_userContext.IsSystemAdmin && ! x.IsSelfWorkspace) ? true :false, SequenceOrder = x.SequenceNo,dateCreated=x.CreatedDate,dateModified=x.LastUpdatedDate,NoteNo=x.DocumentNo,CreatedBy=x.CreatedByUser }));
        //            list = list.OrderBy(x => x.SequenceOrder).ToList();
        //            readResponse.files = JsonConvert.DeserializeObject<IEnumerable<DirectoryContent>>(JsonConvert.SerializeObject(list));                    
        //        }
        //        else
        //        {

        //            if (!args.Data[0].IsNotNull())
        //            {
        //                return Json(new object());
        //            }
        //            var parentId = args.Action != "delete" ? args.Data[0].Id : args.Data[0].ParentId;                    
        //            var workspaces = await _documentBusiness.GetAllByParent(_userContext.UserId, parentId);
        //            var workspace = workspaces.FirstOrDefault();
        //            if (!workspace.IsNotNull())
        //            {
        //                return Json(new object());
        //            }
        //            var parent = workspaces.Where(x => x.Id == parentId).FirstOrDefault();
        //            if (parent != null)
        //            {
        //                var value = new DirectoryContent { id = parentId, parentId = parent.ParentId, WorkspaceId = parent.WorkspaceId, name = parent.Name, hasChild = true , FolderType = parent.FolderType.Value, Count = parent.DocCount.ToString(), TemplateCode = parent.FolderCode, CanOpen = parent.CanOpen, ShowMenu = parent.ShowMenu, CanCreateSubFolder = parent.CanCreateSubFolder, CanRename = parent.CanRename, CanShare = (parent.FolderCode == "GENERAL_FOLDER" || parent.FolderCode == "WORKSPACE_GENERAL") ? parent.CanShare : parent.CanShareDocument, CanMove = (parent.FolderCode == "GENERAL_FOLDER" || parent.FolderCode == "WORKSPACE_GENERAL") ? parent.CanMove :parent.CanMoveDocument, CanCopy = (parent.FolderCode == "GENERAL_FOLDER" || parent.FolderCode == "WORKSPACE_GENERAL") ? parent.CanCopy : parent.CanCopyDocument, CanArchive = (parent.FolderCode == "GENERAL_FOLDER" || parent.FolderCode == "WORKSPACE_GENERAL") ? parent.CanArchive : parent.CanArchiveDocument, CanDelete = (parent.FolderCode == "GENERAL_FOLDER" || parent.FolderCode == "WORKSPACE_GENERAL") ? parent.CanDelete : parent.CanDeleteDocument, CanSeeDetail = parent.CanSeeDetail, CanManagePermission = (parent.FolderCode == "GENERAL_FOLDER" || parent.FolderCode == "WORKSPACE_GENERAL") ? parent.CanManagePermission : parent.CanManagePermissionDocument, CanCreateWorkspace = (_userContext.IsSystemAdmin && !parent.IsSelfWorkspace) ? true : false, CanCreateDocument = (parent.FolderCode == "WORKSPACE_GENERAL") ? false : parent.CanCreateDocument };
        //                readResponse.cwd = JsonConvert.DeserializeObject<DirectoryContent>(JsonConvert.SerializeObject(value));
        //                if (parent.FolderCode == "GENERAL_FOLDER" || parent.FolderCode == "WORKSPACE_GENERAL")
        //                {
        //                    var data = workspaces.Where(x => x.Id != parentId).ToList();
        //                    list = data.Select(x => new DirectoryContent { id = x.Id, name = x.Name, hasChild = true, FolderType = x.FolderType.Value, Count = x.DocCount.ToString(), parentId = x.ParentId, WorkspaceId = x.WorkspaceId, CanOpen = x.CanOpen, ShowMenu = x.ShowMenu, CanCreateSubFolder = x.CanCreateSubFolder, CanRename = x.CanRename, CanShare =(x.FolderCode== "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanShare : x.CanShareDocument, CanMove = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanMove : x.CanMoveDocument, CanCopy = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanCopy : x.CanCopyDocument, CanArchive = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanArchive : x.CanArchiveDocument, CanDelete = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanDelete : x.CanDeleteDocument
        //                        , CanSeeDetail = x.CanSeeDetail, CanManagePermission = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanManagePermission : x.CanManagePermissionDocument, TemplateCode = x.FolderCode, CanCreateWorkspace = _userContext.IsSystemAdmin, WorkflowTemplateCode = x.WorkflowCode, DocumentApprovalStatusType = x.DocumentApprovalStatusType, WorkflowServiceId = x.WorkflowServiceId, StatusName = x.NoteStatus, CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument, CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument, SequenceOrder = x.SequenceOrder, WorkflowServiceStatus = x.WorkflowServiceStatus,
        //                        WorkflowServiceStatusName=x.WorkflowServiceStatusName,
        //                        dateCreated = x.CreatedDate,
        //                        dateModified = x.LastUpdatedDate,
        //                        NoteNo = x.DocumentNo,
        //                        CreatedBy = x.CreatedByUser
        //                    }).ToList();
        //                }
        //                else
        //                {
        //                    list = await _noteBusiness.GetAllDocumentFiles(parentId);
        //                    if (list.IsNotNull())
        //                    {
        //                        list.ForEach(x => x.CanEditDocument = parent.CanEditDocument);
        //                    }
        //                }
        //            }                    
        //            list = list.OrderBy(x => x.SequenceOrder).ToList();
        //            readResponse.files = JsonConvert.DeserializeObject<IEnumerable<DirectoryContent>>(JsonConvert.SerializeObject(list));
        //        }
        //        var json = JsonConvert.SerializeObject(readResponse);
        //        return json;

        //    }
        //    catch
        //    {
        //        //ErrorDetails er = new ErrorDetails();

        //    }
        //    return Json(new object());
        //}
        //public async Task<object> GetFiles(FileManagerDirectoryContent args)
        //{
        //    FileResponse readResponse = new FileResponse();
        //    var list = new List<DirectoryContent>();
        //    try
        //    {
        //        //var page = await _pageBusiness.GetPageForExecution(_userContext.PortalId, "Workspace");
        //        //var permissions = new PageViewModel();
        //        //if (page.IsNotNull())
        //        //{
        //        //    permissions = await _pageBusiness.GetUserPagePermission(page.Portal.Id, page.Id);
        //        //}
        //        var pathIds = args.Path.Split('/');
        //        if (args.Data.Count() == 0 || pathIds.Count() == 2)
        //        {
        //            var user = new DirectoryContent { id = _userContext.UserId, name = _userContext.Name, hasChild = true, FolderType = FolderTypeEnum.Root };
        //            // var workspaces = await _noteBusiness.GetList(x => x.TemplateCode == "WORKSPACE_GENERAL" & x.ParentNoteId == null);
        //            var workspaces = await _documentBusiness.GetUserWorkspaceTreeDataNew(_userContext.UserId, null);
        //            var workspaces1 = workspaces.Where(x => x.FolderCode == "WORKSPACE_GENERAL" && x.ParentId == null).ToList();
        //            foreach (var ws in workspaces1)
        //            {
        //                List<FolderViewModel> childlist = new List<FolderViewModel>();
        //                var childs = await _documentBusiness.GetAllChildDocumentByParentId(ws.Id, childlist);
        //                ws.DocCount = childs.Where(x => x.TemaplateMasterCatCode == "GENERAL_DOCUMENT").Count();
        //            }
        //            readResponse.cwd = JsonConvert.DeserializeObject<DirectoryContent>(JsonConvert.SerializeObject(user));
        //            list.AddRange(workspaces1.Select(x => new DirectoryContent { id = x.Id, name = x.Name, hasChild = true, FolderType = FolderTypeEnum.Workspace, Count = x.DocCount.ToString(), WorkspaceId = x.WorkspaceId, parentId = _userContext.UserId, CanOpen = x.CanOpen, ShowMenu = x.ShowMenu, CanCreateSubFolder = x.CanCreateSubFolder, CanRename = x.CanRename, CanShare = x.CanShare, CanMove = x.CanMove, CanCopy = x.CanCopy, CanArchive = x.CanArchive, CanDelete = x.CanDelete, CanSeeDetail = x.CanSeeDetail, CanManagePermission = x.CanManagePermission, TemplateCode = x.FolderCode, CanCreateWorkspace = _userContext.IsSystemAdmin, SequenceOrder = x.SequenceNo }));
        //            list = list.OrderBy(x => x.SequenceOrder).ToList();
        //            readResponse.files = JsonConvert.DeserializeObject<IEnumerable<DirectoryContent>>(JsonConvert.SerializeObject(list));
        //            //readResponse.files = JsonConvert.DeserializeObject<IEnumerable<DirectoryContent>>(JsonConvert.SerializeObject(workspaces1.Select(x => new DirectoryContent { id = x.Id, name = x.Name, hasChild = true, FolderType = FolderTypeEnum.Workspace, Count = x.DocCount.ToString(), WorkspaceId = x.WorkspaceId, parentId = _userContext.UserId, CanOpen = x.CanOpen, ShowMenu = x.ShowMenu, CanCreateSubFolder = x.CanCreateSubFolder, CanRename = x.CanRename, CanShare = x.CanShare, CanMove = x.CanMove, CanCopy = x.CanCopy, CanArchive = x.CanArchive, CanDelete = x.CanDelete, CanSeeDetail = x.CanSeeDetail, CanManagePermission = x.CanManagePermission, TemplateCode = x.FolderCode, CanCreateWorkspace = _userContext.IsSystemAdmin, SequenceOrder = x.SequenceNo })));
        //        }
        //        else
        //        {

        //            if (!args.Data[0].IsNotNull())
        //            {
        //                return Json(new object());
        //            }
        //            var parentId = args.Action != "delete" ? args.Data[0].Id : args.Data[0].ParentId;
        //            // var workspaces = await _noteBusiness.GetList(x => x.Id == parentId);
        //            var workspaces1 = await _documentBusiness.GetUserWorkspaceTreeDataNew(_userContext.UserId, parentId);
        //            var workspaces = workspaces1.Where(x => x.ParentId == parentId).ToList();
        //            foreach (var ws in workspaces)
        //            {
        //                List<FolderViewModel> childlist = new List<FolderViewModel>();
        //                var childs = await _documentBusiness.GetAllChildDocumentByParentId(ws.Id, childlist);
        //                ws.DocCount = childs.Where(x => x.TemaplateMasterCatCode == "GENERAL_DOCUMENT").Count();
        //            }
        //            //var workspace = workspaces.FirstOrDefault();
        //            //if (!workspace.IsNotNull())
        //            //{
        //            //    return Json(new object());
        //            //}
        //            var parent = workspaces1.Where(x => x.Id == parentId).FirstOrDefault();
        //            if (parent != null)
        //            {
        //                var value = new DirectoryContent { id = parentId, parentId = parent.ParentId, WorkspaceId = parent.WorkspaceId, name = parent.Name, hasChild = (workspaces.Count() > 0) ? true : false, FolderType = parent.FolderType.Value, Count = parent.DocCount.ToString(), TemplateCode = parent.FolderCode, CanOpen = parent.CanOpen, ShowMenu = parent.ShowMenu, CanCreateSubFolder = parent.CanCreateSubFolder, CanRename = parent.CanRename, CanShare = parent.CanShare, CanMove = parent.CanMove, CanCopy = parent.CanCopy, CanArchive = parent.CanArchive, CanDelete = parent.CanDelete, CanSeeDetail = parent.CanSeeDetail, CanManagePermission = parent.CanManagePermission, CanCreateWorkspace = _userContext.IsSystemAdmin, CanCreateDocument = parent.CanCreateDocument };
        //                readResponse.cwd = JsonConvert.DeserializeObject<DirectoryContent>(JsonConvert.SerializeObject(value));
        //                if (parent.FolderCode == "GENERAL_FOLDER" /*|| parent.FolderCode == "WORKSPACE_GENERAL"*/)
        //                //if(workspace.TemplateCode == "GENERAL_FOLDER" || workspace.TemplateCode == "WORKSPACE_GENERAL")
        //                {
        //                    var data = await _documentBusiness.GetFoldersAndDocumentsNew(_userContext.UserId, DocumentQueryTypeEnum.Document, parentId);
        //                    list = data.Select(x => new DirectoryContent { id = x.Id, name = x.Name, hasChild = true, FolderType = x.FolderType.Value, Count = x.DocCount.ToString(), parentId = x.ParentId, WorkspaceId = x.WorkspaceId, CanOpen = x.CanOpen, ShowMenu = x.ShowMenu, CanCreateSubFolder = x.CanCreateSubFolder, CanRename = x.CanRename, CanShare = x.CanShareDocument, CanMove = x.CanMoveDocument, CanCopy = x.CanCopyDocument, CanArchive = x.CanArchiveDocument, CanDelete = x.CanDeleteDocument, CanSeeDetail = x.CanSeeDetail, CanManagePermission = x.CanManagePermissionDocument, TemplateCode = x.FolderCode, CanCreateWorkspace = _userContext.IsSystemAdmin, WorkflowTemplateCode = x.WorkflowCode, DocumentApprovalStatusType = x.DocumentApprovalStatusType, WorkflowServiceId = x.WorkflowServiceId, StatusName = x.NoteStatus, CanEditDocument = x.CanEditDocument, CanCreateDocument = false, SequenceOrder = x.SequenceOrder, WorkflowServiceStatus = x.WorkflowServiceStatus }).ToList();
        //                }
        //            }
        //            else
        //            {
        //                var parentNote1 = await _documentBusiness.GetFoldersAndDocumentsNew(_userContext.UserId, DocumentQueryTypeEnum.Document, null, parentId);
        //                var parentNote = parentNote1.FirstOrDefault(); //await _noteBusiness.GetSingleById(parentId);
        //                if (parentNote != null)
        //                {
        //                    //var value = new DirectoryContent { id = parentNote.Id, name = parentNote.Name, hasChild = false, FolderType = FolderTypeEnum.File, Count = "0", CanOpen = parentNote.CanOpen, ShowMenu = parentNote.ShowMenu, CanCreateSubFolder = parentNote.CanCreateSubFolder, CanRename = parentNote.CanRename, CanShare = parentNote.CanShareDocument, CanMove = parentNote.CanMoveDocument, CanCopy = parentNote.CanCopyDocument, CanArchive = parentNote.CanArchiveDocument, CanDelete = parentNote.CanDeleteDocument, CanSeeDetail = parentNote.CanSeeDetail, CanManagePermission = parentNote.CanManagePermissionDocument, TemplateCode = parentNote.FolderCode, CanCreateWorkspace = _userContext.IsSystemAdmin, WorkflowTemplateCode = parentNote.WorkflowCode, DocumentApprovalStatusType = parentNote.DocumentApprovalStatusType, WorkflowServiceId = parentNote.WorkflowServiceId, StatusName = parentNote.NoteStatus, CanEditDocument = parentNote.CanEditDocument };
        //                    var value = new DirectoryContent { id = parentNote.Id, WorkspaceId = parentNote.WorkspaceId, parentId = parentNote.ParentId, name = parentNote.Name, hasChild = false, FolderType = FolderTypeEnum.File, Count = "0" };
        //                    readResponse.cwd = JsonConvert.DeserializeObject<DirectoryContent>(JsonConvert.SerializeObject(value));
        //                    list = await _noteBusiness.GetAllDocumentFiles(parentId);
        //                    if (list.IsNotNull())
        //                    {
        //                        list.ForEach(x => x.CanEditDocument = parentNote.CanEditDocument);
        //                    }
        //                }

        //            }
        //            list.AddRange(workspaces.Select(x => new DirectoryContent { id = x.Id, WorkspaceId = x.WorkspaceId, name = x.Name, hasChild = true, FolderType = x.FolderType.Value, Count = /*workspaces1.Where(y=>y.ParentId== x.Id).Count().ToString()*/ x.DocCount.ToString(), parentId = x.ParentId, CanOpen = x.CanOpen, ShowMenu = x.ShowMenu, CanCreateSubFolder = x.CanCreateSubFolder, CanRename = x.CanRename, CanShare = x.CanShare, CanMove = x.CanMove, CanCopy = x.CanCopy, CanArchive = x.CanArchive, CanDelete = x.CanDelete, CanSeeDetail = x.CanSeeDetail, CanManagePermission = x.CanManagePermission, TemplateCode = x.FolderCode, CanCreateWorkspace = _userContext.IsSystemAdmin, CanCreateDocument = x.CanCreateDocument, SequenceOrder = x.SequenceNo }));
        //            list = list.OrderBy(x => x.SequenceOrder).ToList();
        //            readResponse.files = JsonConvert.DeserializeObject<IEnumerable<DirectoryContent>>(JsonConvert.SerializeObject(list));
        //        }
        //        var json = JsonConvert.SerializeObject(readResponse);
        //        return json;

        //    }
        //    catch
        //    {
        //        ErrorDetails er = new ErrorDetails();

        //    }
        //    return Json(new object());
        //}
        //public async Task<object> SearchFiles(FileManagerDirectoryContent args)
        //{
        //    var searchStr = args.SearchString.Replace("*", "");
        //    FileResponse readResponse = new FileResponse();
        //    var list = new List<DirectoryContent>();
        //    try
        //    {
        //        var pathIds = args.Path.Split('/');
        //        if (args.Data.Count() == 0 || pathIds.Count() == 2)
        //        {
        //            var user = new DirectoryContent { id = _userContext.UserId, name = _userContext.Name, hasChild = true, FolderType = FolderTypeEnum.Root };
        //            var workspaces = await _documentBusiness.GetFirstLevelWorkspacesByUser(_userContext.UserId);
        //            workspaces = workspaces.Where(x=> x.Name.ToLower().Contains(searchStr.ToLower())).ToList();
        //            readResponse.cwd = JsonConvert.DeserializeObject<DirectoryContent>(JsonConvert.SerializeObject(user));
        //            list.AddRange(workspaces.Select(x => new DirectoryContent { id = x.Id, name = x.Name, hasChild = true, FolderType = FolderTypeEnum.Workspace, Count = x.DocCount.ToString(), WorkspaceId = x.WorkspaceId, parentId = _userContext.UserId, CanOpen = x.CanOpen, ShowMenu = x.ShowMenu, CanCreateSubFolder = x.CanCreateSubFolder, CanRename = x.CanRename, CanShare = x.CanShare, CanMove = x.CanMove, CanCopy = x.CanCopy, CanArchive = x.CanArchive, CanDelete = x.CanDelete, CanSeeDetail = x.CanSeeDetail, CanManagePermission = x.CanManagePermission, TemplateCode = x.FolderCode, CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false, SequenceOrder = x.SequenceNo, dateCreated = x.CreatedDate, dateModified = x.LastUpdatedDate, NoteNo = x.DocumentNo, CreatedBy = x.CreatedByUser }));
        //            list = list.OrderBy(x => x.SequenceOrder).ToList();
        //            readResponse.files = JsonConvert.DeserializeObject<IEnumerable<DirectoryContent>>(JsonConvert.SerializeObject(list));
        //        }
        //        else
        //        {

        //            var parentId = args.Action != "delete" ? args.Data[0].Id : args.Data[0].ParentId;
        //            var workspaces = await _documentBusiness.GetAllByParent(_userContext.UserId, parentId);
        //            var workspace = workspaces.FirstOrDefault();
        //            if (!workspace.IsNotNull())
        //            {
        //                return Json(new object());
        //            }
        //            var parent = workspaces.Where(x => x.Id == parentId).FirstOrDefault();
        //            if (parent != null)
        //            {
        //                var value = new DirectoryContent { id = parentId, parentId = parent.ParentId, WorkspaceId = parent.WorkspaceId, name = parent.Name, hasChild = true, FolderType = parent.FolderType.Value, Count = parent.DocCount.ToString(), TemplateCode = parent.FolderCode, CanOpen = parent.CanOpen, ShowMenu = parent.ShowMenu, CanCreateSubFolder = parent.CanCreateSubFolder, CanRename = parent.CanRename, CanShare = (parent.FolderCode == "GENERAL_FOLDER" || parent.FolderCode == "WORKSPACE_GENERAL") ? parent.CanShare : parent.CanShareDocument, CanMove = (parent.FolderCode == "GENERAL_FOLDER" || parent.FolderCode == "WORKSPACE_GENERAL") ? parent.CanMove : parent.CanMoveDocument, CanCopy = (parent.FolderCode == "GENERAL_FOLDER" || parent.FolderCode == "WORKSPACE_GENERAL") ? parent.CanCopy : parent.CanCopyDocument, CanArchive = (parent.FolderCode == "GENERAL_FOLDER" || parent.FolderCode == "WORKSPACE_GENERAL") ? parent.CanArchive : parent.CanArchiveDocument, CanDelete = (parent.FolderCode == "GENERAL_FOLDER" || parent.FolderCode == "WORKSPACE_GENERAL") ? parent.CanDelete : parent.CanDeleteDocument, CanSeeDetail = parent.CanSeeDetail, CanManagePermission = (parent.FolderCode == "GENERAL_FOLDER" || parent.FolderCode == "WORKSPACE_GENERAL") ? parent.CanManagePermission : parent.CanManagePermissionDocument, CanCreateWorkspace = (_userContext.IsSystemAdmin && ! parent.IsSelfWorkspace) ? true : false, CanCreateDocument = (parent.FolderCode == "WORKSPACE_GENERAL") ? false : parent.CanCreateDocument };
        //                readResponse.cwd = JsonConvert.DeserializeObject<DirectoryContent>(JsonConvert.SerializeObject(value));
        //                if (parent.FolderCode == "GENERAL_FOLDER" || parent.FolderCode == "WORKSPACE_GENERAL")
        //                {
        //                    var data = workspaces.Where(x => x.Id != parentId).ToList();
        //                    list = data.Select(x => new DirectoryContent
        //                    {
        //                        id = x.Id,
        //                        name = x.Name,
        //                        hasChild = true,
        //                        FolderType = x.FolderType.Value,
        //                        Count = x.DocCount.ToString(),
        //                        parentId = x.ParentId,
        //                        WorkspaceId = x.WorkspaceId,
        //                        CanOpen = x.CanOpen,
        //                        ShowMenu = x.ShowMenu,
        //                        CanCreateSubFolder = x.CanCreateSubFolder,
        //                        CanRename = x.CanRename,
        //                        CanShare = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanShare : x.CanShareDocument,
        //                        CanMove = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanMove : x.CanMoveDocument,
        //                        CanCopy = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanCopy : x.CanCopyDocument,
        //                        CanArchive = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanArchive : x.CanArchiveDocument,
        //                        CanDelete = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanDelete : x.CanDeleteDocument
        //                        ,
        //                        CanSeeDetail = x.CanSeeDetail,
        //                        CanManagePermission = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanManagePermission : x.CanManagePermissionDocument,
        //                        TemplateCode = x.FolderCode,
        //                        CanCreateWorkspace = _userContext.IsSystemAdmin,
        //                        WorkflowTemplateCode = x.WorkflowCode,
        //                        DocumentApprovalStatusType = x.DocumentApprovalStatusType,
        //                        WorkflowServiceId = x.WorkflowServiceId,
        //                        StatusName = x.NoteStatus,
        //                        CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
        //                        CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
        //                        SequenceOrder = x.SequenceOrder,
        //                        WorkflowServiceStatus = x.WorkflowServiceStatus,
        //                        WorkflowServiceStatusName = x.WorkflowServiceStatusName,
        //                        dateCreated = x.CreatedDate,
        //                        dateModified = x.LastUpdatedDate,
        //                        NoteNo = x.DocumentNo,
        //                        CreatedBy = x.CreatedByUser
        //                    }).ToList();
        //                }
        //                else
        //                {
        //                    list = await _noteBusiness.GetAllDocumentFiles(parentId);
        //                    if (list.IsNotNull())
        //                    {
        //                        list.ForEach(x => x.CanEditDocument = parent.CanEditDocument);
        //                    }
        //                }
        //            }
        //            list = list.Where(x => x.name.ToLower().Contains(searchStr.ToLower())).ToList();
        //            list = list.OrderBy(x => x.SequenceOrder).ToList();
        //            readResponse.files = JsonConvert.DeserializeObject<IEnumerable<DirectoryContent>>(JsonConvert.SerializeObject(list));                    
        //        }
        //        var json = JsonConvert.SerializeObject(readResponse);
        //        return json;

        //    }
        //    catch
        //    {
        //        //ErrorDetails er = new ErrorDetails();

        //    }
        //    return Json(new object());
        //}

        public IActionResult BulkUpload()
        {
            var model = new EDRDataViewModel();
            return View(model);
        }

        public async Task<JsonResult> GetWorkspaceList(string id)
        {
            if (id.IsNotNullAndNotEmpty())
            {
                var list = await _noteBusiness.GetList(x => x.TemplateCode == "WORKSPACE_GENERAL" || x.TemplateCode == "GENERAL_FOLDER");
                var result = list.Where(x => x.ParentNoteId == id).Select(item => new
                {
                    id = item.Id,
                    Name = item.NoteSubject,
                    ParentId = item.ParentNoteId,
                    hasChildren = list.Where(x => x.ParentNoteId == item.Id).Count() > 0 ? true : false,
                    type = item.TemplateCode == "WORKSPACE_GENERAL" ? "workspace" : "folder",
                });

                return Json(result.ToList());
            }
            else
            {
                var list = await _noteBusiness.GetList(x => x.TemplateCode == "WORKSPACE_GENERAL" || x.TemplateCode == "GENERAL_FOLDER");
                var result = list.Where(x => x.ParentNoteId == null).Select(item => new
                {
                    id = item.Id,
                    Name = item.NoteSubject,
                    ParentId = item.ParentNoteId,
                    hasChildren = list.Where(x => x.ParentNoteId == item.Id).Count() > 0 ? true : false,
                    type = item.TemplateCode == "WORKSPACE_GENERAL" ? "workspace" : "folder",
                });

                return Json(result.ToList());
            }

        }

        public async Task<JsonResult> GetDocumentTemplateList()
        {
            var templateCatgeory = await _templateCategoryBusiness.GetSingle(x => x.Code == "GENERAL_DOCUMENT");
            if (templateCatgeory.IsNotNull())
            {
                var list = await _templateBusiness.GetList(x => x.TemplateCategoryId == templateCatgeory.Id);
                var result = list.Select(item => new
                {
                    Id = item.Id,
                    Name = item.DisplayName,
                });
                return Json(result.ToList());
            }
            else
            {
                return Json("{}");
            }
        }


        [HttpPost]
        public ActionResult UploadExcel(List<IFormFile> file)
        {
            var ms = new MemoryStream();
            file[0].OpenReadStream().CopyTo(ms);
            var workbook = Workbook.Load(ms, Path.GetExtension(file[0].FileName));
            //var workbook = Workbook.Load(file[0].FileName);
            return Content(workbook.ToJson(), MimeTypes.JSON);
        }


        public async Task<ActionResult> UploadExcelTemplateJson(string templateId)
        {
            var ms = new MemoryStream();
            var template = await _templateBusiness.GetSingleById(templateId);
            if (template.IsNotNull())
            {

                var reportList = new List<string>() { template.DisplayName };
                using (var sl = new SLDocument())
                {
                    var style = sl.CreateStyle();

                    foreach (var rep in reportList)
                    {
                        sl.AddWorksheet(rep);
                        SLPageSettings pageSettings = new SLPageSettings();
                        pageSettings.ShowGridLines = false;
                        sl.SetPageSettings(pageSettings);
                        var columns = await _noteBusiness.GetUdfJsonModel(template.Json);
                        int row = 1;
                        if (columns.IsNotNull())
                        {
                            char first = 'A';
                            char second = '#';
                            var columnList = new List<String>();
                            columnList.Add("Document No");
                            columnList.Add("Document Description");
                            columnList.AddRange(columns.Select(x => x.label));
                            foreach (var col in columnList)
                            {
                                var cell = string.Concat(second.ToString().Replace("#", ""), first, row);
                                if (cell == "Z1")
                                {
                                    second = 'A';
                                }
                                first = (char)((int)first);
                                sl.SetColumnWidth(cell, 50);
                                sl.SetCellValue(cell, col);
                                sl.MergeWorksheetCells(cell, cell);
                                style.Font.FontSize = 12;
                                style.Font.FontName = "Verdana";
                                style.Font.Bold = true;
                                style.Fill.SetPattern(DocumentFormat.OpenXml.Spreadsheet.PatternValues.Solid, System.Drawing.Color.FromArgb(0, 0, 128), System.Drawing.Color.FromArgb(0, 0, 128));
                                style.SetFontColor(System.Drawing.Color.White);
                                style.SetWrapText(true);
                                style.SetHorizontalAlignment(DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Center);
                                style.SetVerticalAlignment(DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues.Center);
                                style.Border.TopBorder.BorderStyle = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin;
                                style.Border.TopBorder.Color = System.Drawing.Color.LightGray;
                                style.Border.LeftBorder.BorderStyle = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin;
                                style.Border.LeftBorder.Color = System.Drawing.Color.LightGray;
                                style.Border.BottomBorder.BorderStyle = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin;
                                style.Border.BottomBorder.Color = System.Drawing.Color.LightGray;
                                style.Border.RightBorder.BorderStyle = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin;
                                style.Border.RightBorder.Color = System.Drawing.Color.LightGray;
                                sl.SetCellStyle(cell, cell, style);
                                for (int i = 1; i < 100; ++i)
                                {
                                    var styleB = sl.CreateStyle();
                                    styleB.Border.TopBorder.BorderStyle = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin;
                                    styleB.Border.TopBorder.Color = System.Drawing.Color.LightGray;
                                    styleB.Border.LeftBorder.BorderStyle = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin;
                                    styleB.Border.LeftBorder.Color = System.Drawing.Color.LightGray;
                                    styleB.Border.BottomBorder.BorderStyle = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin;
                                    styleB.Border.BottomBorder.Color = System.Drawing.Color.LightGray;
                                    styleB.Border.RightBorder.BorderStyle = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin;
                                    styleB.Border.RightBorder.Color = System.Drawing.Color.LightGray;
                                    if (second == 'A')
                                    {
                                        sl.SetCellStyle(string.Concat("A", first, i), string.Concat("A", first, i), styleB);
                                    }
                                    else
                                    {
                                        sl.SetCellStyle(string.Concat(first, i), string.Concat(first, i), styleB);
                                    }


                                };
                                first++;
                            }
                        }
                    }
                    sl.SaveAs(ms);
                    ms.Position = 0;
                }
                Stream stream = ms;
                var workbook = Workbook.Load(stream, ".xlsx");
                workbook.Sheets.RemoveAt(0);
                JObject jObject = JObject.Parse(workbook.ToJson());
                return Content(jObject.ToString(), Telerik.Web.Spreadsheet.MimeTypes.JSON);
            }
            else
            {
                return null;
            }
        }

        private static char CreateColumnHeader(SLDocument sl, int row, char first, string col)
        {
            first = (char)((int)first);
            sl.SetColumnWidth(string.Concat(first, row), 50);
            sl.SetCellValue(string.Concat(first, row), col);
            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
            var style = sl.CreateStyle();
            style.Font.FontSize = 12;
            style.Font.FontName = "Verdana";
            style.SetWrapText(true);
            style.SetHorizontalAlignment(DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues.Center);
            style.Border.TopBorder.BorderStyle = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Black;
            style.Border.LeftBorder.BorderStyle = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;
            style.Border.BottomBorder.BorderStyle = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin;
            style.Border.BottomBorder.Color = System.Drawing.Color.Black;
            style.Border.RightBorder.BorderStyle = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin;
            style.Border.RightBorder.Color = System.Drawing.Color.Black;
            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), style);
            return first;
        }

        public ActionResult LoadExcel(string id)
        {
            var fileType = _fileBusiness.GetSingleById(id).Result;
            var bytes1 = _fileBusiness.GetFileByte(id).Result;
            using (MemoryStream ms = new MemoryStream(bytes1))
            {
                var workbook = Workbook.Load(ms, Path.GetExtension(fileType.FileName));
                return Content(workbook.ToJson(), MimeTypes.JSON);
            }
            return Content("");

        }


        [HttpPost]
        public async Task<ActionResult> ValidateGalfarEngineeringDocuments(string data, string templateId, string templateName, string workspaceId, string folderId)
        {
            workspaceId = await _noteBusiness.GetFolderWorkspace(folderId);
            var errorList = new List<string>();
            var udflist = await _noteBusiness.DynamicUdfColumns(templateId);
            JObject jObject = JObject.Parse(data);
            string sheet1 = (string)jObject.SelectToken("activeSheet");
            JArray sheets = (JArray)jObject.SelectToken("sheets");
            foreach (JToken sheet in sheets)
            {
                string sheetname = (string)sheet.SelectToken("name");
                if (sheetname == sheet1)
                {
                    var list = new List<long>();
                    int z = 1;
                    JArray rows = (JArray)sheet.SelectToken("rows");
                    foreach (JToken row in rows)
                    {


                        var noteno = GetCellValues(row, 0);
                        if (noteno != null)
                        {
                            if (noteno.ToLower().Contains("document") || noteno.ToLower().Contains("rfi no") || noteno.ToLower().Contains("==="))
                            {
                                z++;
                                continue;
                            }
                            var subject = GetCellValues(row, 1);
                            var revision = GetCellValuesByString(row, "rev-");
                            var document = new NoteViewModel();
                            if (revision.IsNotNullAndNotEmpty())
                            {
                                var doc = await _noteBusiness.GetDocumentByNoteAndRevision(templateId, noteno, revision);
                                if (doc.IsNotNull())
                                {
                                    document = doc.FirstOrDefault();
                                }
                            }
                            else
                            {
                                document = await _noteBusiness.GetSingle(x => x.NoteNo == noteno);
                            }
                            if (!document.IsNotNull())
                            {
                                try
                                {
                                    if (workspaceId.IsNotNullAndNotEmpty())
                                    {
                                        var doc = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel
                                        {
                                            Id = "",
                                            TemplateId = templateId,
                                        });
                                        var approveType = "";
                                        var udfdetails = await _noteBusiness.GetUdfJsonModel(doc.Json);
                                        int j = 2;

                                        foreach (var i in udfdetails)
                                        {
                                            var item = doc.ColumnList.Where(x => x.Name == i.key).FirstOrDefault();


                                            var field = GetCellValues(row, j);
                                            if (udfdetails.Where(x => x.key == item.Name).Count() > 0)
                                            {
                                                if (item.UdfUIType == UdfUITypeEnum.file)
                                                {
                                                    if (field.IsNotNullAndNotEmpty())
                                                    {
                                                        var file = await _fileBusiness.GetFileByName(field);
                                                        if (!file.IsNotNull())
                                                        {
                                                            errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,File does not exist :", udflist[j].LabelName));
                                                        }
                                                    }
                                                    //break;
                                                }
                                                else if (item.UdfUIType == UdfUITypeEnum.select && item.ForeignKeyTableName == "LOV")
                                                {
                                                    if (field.IsNotNullAndNotEmpty())
                                                    {
                                                        var parentValue = udfdetails.Where(x => x.key == item.Name).FirstOrDefault();
                                                        //var parentValue = udflist[j].DataSourceHtmlAttributesString;
                                                        //string substr = parentValue.Substring(parentValue.IndexOf(":") + 2);
                                                        //string substr1 = substr.Substring(0, substr.IndexOf("\"}"));
                                                        var lov = await _lovBusiness.GetListOfValueByParentAndValue(parentValue.parameterCode, field);
                                                        if (lov == null || lov.Count == 0)
                                                        {
                                                            errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,value does not exist :", udflist[j].LabelName));
                                                        }
                                                    }
                                                    //break;
                                                }
                                                else if (item.UdfUIType == UdfUITypeEnum.select && item.ForeignKeyTableName == "enum")
                                                {
                                                    if (field.IsNotNullAndNotEmpty())
                                                    {
                                                        var parentValue = udfdetails.Where(x => x.key == item.Name).FirstOrDefault();
                                                        var code = field.Replace(" ", string.Empty);
                                                        if (!GetValidateEnumValue(parentValue.parameterCode, code))
                                                        {
                                                            errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,value does not exist :", udflist[j].LabelName));

                                                        }
                                                    }
                                                    //break;
                                                }
                                                else if (item.UdfUIType == UdfUITypeEnum.select && item.ForeignKeyTableName == "User")
                                                {
                                                    if (field.IsNotNullAndNotEmpty())
                                                    {
                                                        var user = await _userBusiness.GetSingle(x => x.Name == field || x.Email == field);
                                                        if (!user.IsNotNull())
                                                        {
                                                            errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,User does not exist :", udflist[j].LabelName));
                                                        }
                                                    }
                                                    //break;
                                                }
                                                else if (item.UdfUIType == UdfUITypeEnum.datetime)
                                                {
                                                    if (field.IsNotNullAndNotEmpty())
                                                    {
                                                        try
                                                        {
                                                            double d = double.Parse(field);
                                                            DateTime conv = DateTime.FromOADate(d);
                                                        }
                                                        catch (Exception)
                                                        {
                                                            errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,Incorrect Date Format :", udflist[j].LabelName));
                                                        }
                                                    }
                                                    //break;
                                                }
                                                else
                                                {
                                                    //break;
                                                }
                                                //}
                                            }
                                            j++;


                                        }


                                    }

                                }
                                catch (Exception ex)
                                {
                                    //return Json(new { success = false, errors = ex.ToString() });
                                    errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,Upload fialed :", noteno));
                                }
                            }
                            else
                            {
                                try
                                {
                                    var doc = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel
                                    {
                                        NoteId = document.Id,
                                    });
                                    //var isLatest = fields[26];
                                    //var genFile = fields[27];
                                    //if (genFile.IsNotNullAndNotEmpty())
                                    //{
                                    //    var genhead = _fileBusiness.GetList(x=> x.FileName == genFile).OrderByDescending(x => x.Id).FirstOrDefault();
                                    //    if (genhead != null)
                                    //        doc.CSVFileIds = genhead.Id + "";
                                    //}

                                    var udfdetails = await _noteBusiness.GetUdfJsonModel(doc.Json);
                                    int j = 2;

                                    foreach (var i in udfdetails)
                                    {
                                        var item = doc.ColumnList.Where(x => x.Name.ToLower() == i.key.ToLower()).FirstOrDefault();
                                        //for (int j = 0; j <= udflist.Count - 1; j++)
                                        //{
                                        var field = GetCellValues(row, j);
                                        if (udfdetails.Where(x => x.key == item.Name).Count() > 0)
                                        {
                                            if (item.UdfUIType == UdfUITypeEnum.file)
                                            {
                                                if (field.IsNotNullAndNotEmpty())
                                                {
                                                    var file = await _fileBusiness.GetFileByName(field);
                                                    if (!file.IsNotNull())
                                                    {
                                                        errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,File does not exist :", udflist[j].LabelName));

                                                    }
                                                }
                                            }
                                            else if (item.UdfUIType == UdfUITypeEnum.select && item.ForeignKeyTableName == "LOV")
                                            {
                                                if (field.IsNotNullAndNotEmpty())
                                                {
                                                    var parentValue = udfdetails.Where(x => x.key == item.Name).FirstOrDefault();
                                                    var lov = await _lovBusiness.GetListOfValueByParentAndValue(parentValue.parameterCode, field);
                                                    if (lov == null)
                                                    {
                                                        errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,value does not exist :", udflist[j].LabelName));
                                                    }
                                                }
                                            }
                                            else if (item.UdfUIType == UdfUITypeEnum.select && item.ForeignKeyTableName == "enum")
                                            {
                                                if (field.IsNotNullAndNotEmpty())
                                                {
                                                    var parentValue = udfdetails.Where(x => x.key == item.Name).FirstOrDefault();
                                                    var code = field.Replace(" ", string.Empty);
                                                    if (!GetValidateEnumValue(parentValue.parameterCode, code))
                                                    {
                                                        errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,value does not exist :", udflist[j].LabelName));

                                                    }
                                                }
                                            }
                                            else if (item.UdfUIType == UdfUITypeEnum.select && item.ForeignKeyTableName == "User")
                                            {
                                                if (field.IsNotNullAndNotEmpty())
                                                {
                                                    var user = await _userBusiness.GetSingle(x => x.Name == field || x.Email == field);
                                                    if (!user.IsNotNull())
                                                    {
                                                        errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,User does not exist :", udflist[j].LabelName));
                                                    }
                                                }
                                            }
                                            else if (item.UdfUIType == UdfUITypeEnum.datetime || item.ForeignKeyTableName == "NTS_DateTimePicker")
                                            {
                                                if (field.IsNotNullAndNotEmpty())
                                                {
                                                    try
                                                    {
                                                        double d = double.Parse(field);
                                                        DateTime conv = DateTime.FromOADate(d);
                                                    }
                                                    catch (Exception)
                                                    {
                                                        errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,Incorrect Date Format :", udflist[j].LabelName));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                            }
                                        }
                                        //}


                                        j++;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,Upload fialed :", noteno));
                                }


                            }
                            z++;
                        }
                        else
                        {
                            break;
                        }

                    }
                    break;
                }

            }

            if (errorList.Count > 0)
            {
                return Json(new { success = false, errors = errorList });
            }
            else
            {
                return await LoadGalfarEngineeringDocuments1(data, templateId, templateName, workspaceId, folderId);
            }


        }
        [HttpPost]
        public async Task<ActionResult> LoadGalfarEngineeringDocuments1(string data, string templateId, string templateName, string workspaceId, string folderId)
        {
            var errorList = new List<string>();
            var udflist = await _noteBusiness.DynamicUdfColumns(templateId);
            JObject jObject = JObject.Parse(data);
            string sheet1 = (string)jObject.SelectToken("activeSheet");
            JArray sheets = (JArray)jObject.SelectToken("sheets");
            foreach (JToken sheet in sheets)
            {
                string sheetname = (string)sheet.SelectToken("name");
                if (sheetname == sheet1)
                {
                    var list = new List<long>();

                    JArray rows = (JArray)sheet.SelectToken("rows");
                    foreach (JToken row in rows)
                    {

                        var noteno = GetCellValues(row, 0);
                        if (noteno != null && noteno.IsNotNullAndNotEmpty())
                        {
                            if (noteno.ToLower().Contains("document") || noteno.ToLower().Contains("rfi no") || noteno.ToLower().Contains("==="))
                            {
                                continue;
                            }
                            //string pattern = "\\b" + noteno + "\\b";
                            string pattern = "" + noteno + "";
                            //Regex regReplace = new Regex(pattern);

                            var subject = GetCellValues(row, 1);
                            var revision = GetCellValuesByString(row, "rev-");
                            var existDoc = new List<NoteViewModel>();
                            if (revision.IsNotNullAndNotEmpty())
                            {
                                existDoc = await _noteBusiness.GetDocumentByNoteAndRevision(templateId, noteno, revision);
                            }
                            else
                            {
                                var exist = await _noteBusiness.GetSingle(x => x.NoteNo == noteno);
                                if (exist.IsNotNull())
                                {
                                    existDoc.Add(exist);
                                }

                            }

                            if (existDoc.Count == 0)
                            {
                                //var workspace = _noteBusiness.GetWorkspaceDataForAdmin().Where(x => x.Name.ToLower().Contains("technip workspace")).FirstOrDefault();

                                try
                                {
                                    if (workspaceId.IsNotNullAndNotEmpty())
                                    {
                                        var doc = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel
                                        {
                                            Id = null,
                                            TemplateId = templateId,
                                            ActiveUserId = _userContext.UserId,
                                            RequestedByUserId = _userContext.UserId,
                                            OwnerUserId = _userContext.UserId,
                                            DataAction = DataActionEnum.Create,
                                            //Works = workspaceId,//workspace.Id,
                                        });
                                        doc.DataAction = DataActionEnum.Create;
                                        doc.NoteNo = noteno;
                                        doc.NoteSubject = subject;
                                        var approveType = "";
                                        var udfdetails = await _noteBusiness.GetUdfJsonModel(doc.Json);
                                        int j = 2;
                                        var columns = new List<ColumnMetadataViewModel>();
                                        foreach (var i in udfdetails)
                                        {
                                            var item = doc.ColumnList.Where(x => x.Name == i.key).FirstOrDefault();
                                            var field = GetCellValues(row, j);
                                            //if (udfdetails.Where(x => x.key == "WorkspaceId").Count() > 0)
                                            //{
                                            //    item.Value = workspaceId;
                                            //}
                                            if (field.IsNotNullAndNotEmpty())
                                            {
                                                if (udfdetails.Where(x => x.key == item.Name).Count() > 0)
                                                {
                                                    if (item.UdfUIType == UdfUITypeEnum.file)
                                                    {

                                                        var file = await _fileBusiness.GetFileByName(field);
                                                        if (file.IsNotNull())
                                                        {
                                                            item.Value = file.Id;
                                                        }
                                                        else
                                                        {
                                                            item.Value = null;
                                                        }


                                                    }
                                                    else if (item.UdfUIType == UdfUITypeEnum.select && item.ForeignKeyTableName == "LOV")
                                                    {
                                                        var parentValue = udfdetails.Where(x => x.key == item.Name).FirstOrDefault();
                                                        var lov = await _lovBusiness.GetListOfValueByParentAndValue(parentValue.parameterCode, field);
                                                        if (lov.IsNotNull())
                                                        {
                                                            var obj = lov.FirstOrDefault();
                                                            item.Value = obj.Id;
                                                        }
                                                        else
                                                        {
                                                            item.Value = null;
                                                        }
                                                    }
                                                    else if (item.UdfUIType == UdfUITypeEnum.select && item.ForeignKeyTableName == "enum")
                                                    {
                                                        if (field.IsNotNullAndNotEmpty())
                                                        {
                                                            var code = field.Replace(" ", string.Empty);
                                                            if (code != null)
                                                            {
                                                                item.Value = code;
                                                            }
                                                            if (item.Name.Contains("documentApprovalStatusType"))
                                                            {

                                                                if (!field.ToLower().Contains("manual") && !field.ToLower().Contains("not"))
                                                                {
                                                                    var item1 = doc.ColumnList.FirstOrDefault(x => x.Name == "documentApprovalStatus");
                                                                    if (item1.IsNotNull())
                                                                    {
                                                                        approveType = DocumentApprovalStatusEnum.ApprovalInProgress.ToString();
                                                                        item1.Value = DocumentApprovalStatusEnum.ApprovalInProgress.ToString();
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    var item1 = doc.ColumnList.FirstOrDefault(x => x.Name == "documentApprovalStatus");
                                                                    if (item1.IsNotNull())
                                                                    {
                                                                        item1.Value = DocumentApprovalStatusEnum.ApprovedManually.ToString();
                                                                    }
                                                                }

                                                            }

                                                        }
                                                    }
                                                    else if (item.UdfUIType == UdfUITypeEnum.select && item.ForeignKeyTableName == "User")
                                                    {
                                                        if (field.IsNotNullAndNotEmpty())
                                                        {
                                                            var user = await _userBusiness.GetSingle(x => x.Name == field || x.Email == field);
                                                            if (user.IsNotNull())
                                                            {
                                                                item.Value = user.Id;
                                                            }
                                                            else
                                                            {
                                                                item.Value = null;
                                                            }
                                                        }
                                                    }
                                                    else if (item.UdfUIType == UdfUITypeEnum.datetime || item.UdfUIType == UdfUITypeEnum.time)
                                                    {
                                                        if (field.IsNotNullAndNotEmpty())
                                                        {
                                                            double d = double.Parse(field);
                                                            DateTime conv = DateTime.FromOADate(d);
                                                            item.Value = conv.ToString();
                                                        }
                                                        else
                                                        {
                                                            item.Value = null;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        item.Value = field;
                                                    }
                                                }


                                            }
                                            if (item.Name == "WorkspaceId")
                                            {
                                                var workspsaceDetails = await _noteBusiness.GetWorkspaceId(workspaceId);
                                                if (workspsaceDetails.IsNotNull())
                                                {
                                                    item.Value = workspsaceDetails.Id;
                                                }
                                                else
                                                {
                                                    item.Value = null;
                                                }
                                            }
                                            j++;
                                            //columns.Add(item);
                                            doc.ColumnList.Where(x => x.Name == i.key).FirstOrDefault().Value = item.Value;
                                        }

                                        doc.ParentNoteId = folderId;
                                        dynamic exo = new System.Dynamic.ExpandoObject();

                                        foreach (var udf in doc.ColumnList.Where(x => x.IsUdfColumn == true))
                                        {
                                            ((IDictionary<String, Object>)exo).Add(udf.Name, udf.Value);
                                        }
                                        doc.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);


                                        var result = await _noteBusiness.ManageNote(doc);
                                        if (result.IsSuccess)
                                        {
                                            if (approveType == DocumentApprovalStatusEnum.ApprovalInProgress.ToString())
                                            {
                                                var workflowId = await _noteBusiness.GetServiceWorkflowTemplateId(templateId);
                                                if (workflowId.IsNotNull() && workflowId != null)
                                                {
                                                    var service = await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel
                                                    {
                                                        Id = null,
                                                        TemplateId = workflowId,
                                                        RequestedByUserId = _userContext.UserId,
                                                        OwnerUserId = _userContext.UserId,
                                                        DataAction = DataActionEnum.Create

                                                    });
                                                    service.DataAction = DataActionEnum.Create;
                                                    var stepTasks = await _taskBusiness.GetList(x => x.ParentServiceId == service.Id);
                                                    //stepTasks.ForEach(x => x.ActualSLA = null);
                                                    service.StepTasksList = stepTasks;
                                                    //service.service = new JavaScriptSerializer().Serialize(stepTasks);
                                                    var regularDocument1 = service.ColumnList.FirstOrDefault(x => x.Name == "RegularDocument1");
                                                    if (regularDocument1.IsNotNull())
                                                    {
                                                        regularDocument1.Value = result.Item.Id;
                                                    }
                                                    await _serviceBusiness.ManageService(service);
                                                }

                                            }
                                            else
                                            {
                                                //if (templateName == "Engineering Document")
                                                //{
                                                //    await MoveEngineeringDocument(result.Item);
                                                //}
                                                //else if (templateName == "Project Documents")
                                                //{
                                                //    await MoveProjectDocument(result.Item);
                                                //}
                                                //else if (templateName == "Vendor Documents")
                                                //{
                                                //    await MoveVendorDocument(result.Item);
                                                //}
                                            }
                                            var replace1 = noteno + "===Completed";
                                            data = data.Replace(pattern, replace1);
                                            var k = Json(new { success = true, errors = errorList, excelData = data });
                                            //j.MaxJsonLength = int.MaxValue;
                                            // return k;
                                        }
                                        else
                                        {
                                            var replace2 = noteno + "===Error[Mandatory Fields Req.]";
                                            data = data.Replace(pattern, replace2);

                                            var k = Json(new { success = false, errors = errorList, excelData = data });
                                            //j.MaxJsonLength = int.MaxValue;
                                            //return k;
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    var replace2 = noteno + "===Error";
                                    data = data.Replace(pattern, replace2);
                                    var j = Json(new { success = false, errors = errorList, excelData = data });
                                    //j.MaxJsonLength = int.MaxValue;
                                    // return j;
                                }
                            }
                            else
                            {
                                try
                                {
                                    bool success = false;
                                    foreach (var document in existDoc)
                                    {
                                        var doc = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel
                                        {
                                            NoteId = document.Id,
                                            DataAction = DataActionEnum.Edit,
                                        });
                                        doc.NoteNo = noteno;
                                        doc.NoteSubject = subject;
                                        var udfdetails = await _noteBusiness.GetUdfJsonModel(doc.Json);
                                        int j = 2;
                                        var columns = new List<ColumnMetadataViewModel>();

                                        foreach (var i in udfdetails)
                                        {
                                            var item = doc.ColumnList.Where(x => x.Name == i.key).FirstOrDefault();

                                            var field = GetCellValues(row, j);
                                            if (udfdetails.Where(x => x.key == item.Name).Count() > 0)
                                            {
                                                if (item.UdfUIType == UdfUITypeEnum.file)
                                                {
                                                    if (field.IsNotNullAndNotEmpty())
                                                    {
                                                        var file = await _fileBusiness.GetFileByName(field);
                                                        if (file.IsNotNull())
                                                        {
                                                            item.Value = file.Id;
                                                        }
                                                    }
                                                }
                                                else if (item.UdfUIType == UdfUITypeEnum.select && item.ForeignKeyTableName == "LOV")
                                                {
                                                    if (field.IsNotNullAndNotEmpty())
                                                    {
                                                        var parentValue = udfdetails.Where(x => x.key == item.Name).FirstOrDefault();
                                                        //var parentValue = udflist[j].DataSourceHtmlAttributesString;
                                                        //string substr = parentValue.Substring(parentValue.IndexOf(":") + 2);
                                                        //string substr1 = substr.Substring(0, substr.IndexOf("\"}"));
                                                        var lov = await _lovBusiness.GetListOfValueByParentAndValue(parentValue.parameterCode, field);
                                                        if (lov.IsNotNull())
                                                        {
                                                            var obj = lov.FirstOrDefault();
                                                            item.Value = obj.Id;
                                                        }
                                                    }
                                                }
                                                else if (item.UdfUIType == UdfUITypeEnum.select && item.ForeignKeyTableName == "enum")
                                                {
                                                    if (field.IsNotNullAndNotEmpty())
                                                    {
                                                        var code = field.Replace(" ", string.Empty);
                                                        if (code != null)
                                                        {
                                                            item.Value = code;
                                                        }
                                                    }
                                                }
                                                else if (item.UdfUIType == UdfUITypeEnum.select && item.ForeignKeyTableName == "User")
                                                {
                                                    if (field.IsNotNullAndNotEmpty())
                                                    {
                                                        var user = await _userBusiness.GetSingle(x => x.Name == field || x.Email == field);
                                                        if (user.IsNotNull())
                                                        {
                                                            item.Value = user.Id;
                                                        }
                                                    }
                                                }
                                                else if (item.UdfUIType == UdfUITypeEnum.select || item.UdfUIType == UdfUITypeEnum.select)
                                                {
                                                    if (field.IsNotNullAndNotEmpty())
                                                    {
                                                        double d = double.Parse(field);
                                                        DateTime conv = DateTime.FromOADate(d);
                                                        item.Value = conv.ToDefaultDateFormat();
                                                    }
                                                }
                                                else
                                                {
                                                    if (field.IsNotNullAndNotEmpty())
                                                    {
                                                        item.Value = field;
                                                    }
                                                }
                                            }

                                            j++;
                                            doc.ColumnList.Where(x => x.Name == i.key).FirstOrDefault().Value = item.Value;
                                        }

                                        doc.ParentNoteId = folderId;
                                        dynamic exo = new System.Dynamic.ExpandoObject();

                                        foreach (var udf in doc.ColumnList.Where(x => x.IsUdfColumn == true))
                                        {
                                            ((IDictionary<String, Object>)exo).Add(udf.Name, udf.Value);
                                        }
                                        doc.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                        var result = await _noteBusiness.ManageNote(doc);
                                        success = result.IsSuccess;


                                    }
                                    if (!success)
                                    {
                                        var replace2 = noteno + "===Error";
                                        data = data.Replace(pattern, replace2);
                                        var j = Json(new { success = false, errors = errorList, excelData = data });
                                        //  j.MaxJsonLength = int.MaxValue;
                                        return j;
                                    }
                                    else
                                    {
                                        var replace1 = noteno + "===Completed";
                                        data = data.Replace(pattern, replace1);
                                        var j = Json(new { success = true, errors = errorList, excelData = data });
                                        // j.MaxJsonLength = int.MaxValue;
                                        //   return j;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    var replace2 = noteno + "===Error";
                                    data = data.Replace(pattern, replace2);
                                    var j = Json(new { success = false, errors = errorList, excelData = data });
                                    // j.MaxJsonLength = int.MaxValue;
                                    return j;
                                }


                            }
                        }
                        else
                        {
                            break;
                        }

                    }
                    break;
                }

            }
            return Json(new { success = true });

        }

        public async Task<ActionResult> MoveEngineeringDocument(NoteTemplateViewModel doc)
        {
            var desciplin = doc.ColumnList.FirstOrDefault(x => x.Name == "discipline").Value;
            var revision = doc.ColumnList.FirstOrDefault(x => x.Name == "revision").Value;
            var code = doc.ColumnList.FirstOrDefault(x => x.Name == "code");
            var workspaceforDataAdmin = await _noteBusiness.GetWorkspaceDataForAdmin();
            var workspace = workspaceforDataAdmin.Where(x => x.NoteSubject.ToLower().Contains("engineering")).FirstOrDefault();

            if (workspace != null)
            {

                var desiplinist = await _noteBusiness.GetList(x => x.ParentNoteId == workspace.Id);
                var desiplin = desiplinist.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains(desciplin.ToString().ToLower())).FirstOrDefault();
                string desiplinFolderId = null;
                if (desiplin != null)
                {
                    desiplinFolderId = desiplin.Id;

                }
                else
                {
                    var foldermodel = new NoteTemplateViewModel
                    {
                        TemplateCode = "GENERAL_FOLDER",
                        DataAction = DataActionEnum.Create,
                        OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        ParentNoteId = workspace.Id,
                        StartDate = DateTime.Now.ApplicationNow().Date,
                    };

                    var newmodel1 = await _noteBusiness.GetNoteDetails(foldermodel);
                    newmodel1.NoteSubject = desciplin.ToString();
                    newmodel1.DataAction = DataActionEnum.Create;
                    await _noteBusiness.ManageNote(newmodel1);
                    desiplinFolderId = newmodel1.Id;
                }
                var revList = await _noteBusiness.GetList(x => x.ParentNoteId == desiplinFolderId);
                var rev = revList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains(revision.ToString().ToLower())).FirstOrDefault();
                //var rev = await _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains(revision.ToLower())).FirstOrDefault();
                string revParentId = null;
                if (rev != null)
                {
                    revParentId = rev.Id;
                }
                else
                {
                    var foldermodel = new NoteTemplateViewModel
                    {
                        TemplateCode = "GENERAL_FOLDER",
                        DataAction = DataActionEnum.Create,
                        OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        ParentNoteId = desiplinFolderId,
                        StartDate = DateTime.Now.ApplicationNow().Date,
                    };

                    var newmodel1 = await _noteBusiness.GetNoteDetails(foldermodel);
                    newmodel1.NoteSubject = revision.ToString();
                    newmodel1.DataAction = DataActionEnum.Create;
                    await _noteBusiness.ManageNote(newmodel1);
                    revParentId = newmodel1.Id;
                }
                var revnote = await _noteBusiness.CopyDocument(doc.Id, revParentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                //_repoNote.CreateOneToOneRelationshipToReferenceType(revnote.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);


                if (revision.ToString().Contains("Rev"))
                {
                    var latestrevfolderList = await _noteBusiness.GetList(x => x.ParentNoteId == desiplinFolderId);
                    var latestrevfolder = revList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains("latest")).FirstOrDefault();
                    //var latestrevfolder = await _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject.ToLower().Contains("latest")).FirstOrDefault();
                    if (latestrevfolder != null)
                    {

                        var revDoc = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { Id = latestrevfolder.Id });
                        if (revDoc != null)
                        {
                            var check = revDoc.NoteNo == doc.NoteNo ? revDoc : null;// .Where(x => x.Name == doc.NoteNo).FirstOrDefault();

                            if (check != null)
                            {
                                //var doc1 = new NoteTemplateViewModel
                                //{
                                //    Id = check.Id.Value,
                                //    DataAction = DataActionEnum.Edit,
                                //};
                                await _noteBusiness.Delete(check.Id);
                            }
                            var note = await _noteBusiness.CopyDocument(doc.Id, latestrevfolder.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                            //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                        }

                    }
                    else
                    {
                        var foldermodel = new NoteTemplateViewModel
                        {
                            TemplateCode = "GENERAL_FOLDER",
                            DataAction = DataActionEnum.Create,
                            OwnerUserId = workspace.OwnerUserId,
                            ActiveUserId = workspace.OwnerUserId,
                            RequestedByUserId = workspace.OwnerUserId,
                            ParentNoteId = desiplinFolderId,
                            StartDate = DateTime.Now.ApplicationNow().Date,
                        };

                        var newmodel1 = await _noteBusiness.GetNoteDetails(foldermodel);
                        newmodel1.NoteSubject = "Latest revision Files";
                        newmodel1.DataAction = DataActionEnum.Create;
                        await _noteBusiness.ManageNote(newmodel1);
                        var note = await _noteBusiness.CopyDocument(doc.Id, newmodel1.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                        //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                    }

                }
                if (code.IsNotNull() && code.Value.IsNotNull() && code.Value.ToString().Contains("AFC"))
                {

                    string parentId = null;
                    var folderList = await _noteBusiness.GetList(x => x.ParentNoteId == desiplin.Id);
                    var folder = folderList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains("signed")).FirstOrDefault();
                    //var folder = await _noteBusiness.GetAllChildForWorkspace(desiplin.Id).Where(x => x.Subject != null && x.Subject.ToLower().Contains("signed")).FirstOrDefault();
                    if (folder != null)
                    {
                        parentId = folder.Id;
                    }
                    else
                    {
                        var foldermodel = new NoteTemplateViewModel
                        {
                            TemplateCode = "GENERAL_FOLDER",
                            DataAction = DataActionEnum.Create,
                            OwnerUserId = workspace.OwnerUserId,
                            ActiveUserId = workspace.OwnerUserId,
                            RequestedByUserId = workspace.OwnerUserId,
                            ParentNoteId = desiplin.Id,
                            StartDate = DateTime.Now.ApplicationNow().Date,
                            //WorkspaceId = workspace.Id,
                        };

                        var newmodel = await _noteBusiness.GetNoteDetails(foldermodel);
                        newmodel.NoteSubject = "Signed AFC";
                        newmodel.ParentNoteId = desiplin.Id;
                        await _noteBusiness.ManageNote(newmodel);
                        parentId = newmodel.Id;
                    }

                    if (parentId.IsNotNullAndNotEmpty())
                    {
                        var revDoc = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { Id = parentId });
                        if (revDoc != null)
                        {
                            var check = revDoc.NoteNo == doc.NoteNo ? revDoc : null;// .Where(x => x.Name == doc.NoteNo).FirstOrDefault();

                            if (check != null)
                            {
                                var doc1 = new NoteViewModel
                                {
                                    Id = check.Id,
                                    DataAction = DataActionEnum.Edit,
                                };
                                await _noteBusiness.Delete(doc1.Id);
                            }
                            var note = await _noteBusiness.CopyDocument(doc.Id, parentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                        }
                    }

                }

            }
            return null;
        }
        public async Task<ActionResult> MoveProjectDocument(NoteTemplateViewModel doc)
        {
            var desciplin = doc.ColumnList.FirstOrDefault(x => x.Name == "discipline");
            var revision = doc.ColumnList.FirstOrDefault(x => x.Name == "revision");
            var projectFolder = doc.ColumnList.FirstOrDefault(x => x.Name == "projectFolder");
            var projectSubFolder = doc.ColumnList.FirstOrDefault(x => x.Name == "projectSubFolder");
            var code = doc.ColumnList.FirstOrDefault(x => x.Name == "code");
            //var workspace = _noteBusiness.GetWorkspaceDataForAdmin().Where(x => x.Name.ToLower().Contains(projectFolder.Value.ToLower())).FirstOrDefault();
            var workspaceforDataAdmin = await _noteBusiness.GetWorkspaceDataForAdmin();
            var workspace = workspaceforDataAdmin.Where(x => x.NoteSubject.ToLower().Contains(projectFolder.Value.ToString().ToLower())).FirstOrDefault();

            if (workspace != null)
            {
                if (projectSubFolder.Value.ToString() != "PSF_NA")
                {
                    //var desiplin = _noteBusiness.GetAllChildForWorkspace(workspace.Id).Where(x => x.Subject != null && x.Subject.ToLower().Contains(desciplin.Value.ToLower())).FirstOrDefault();
                    var desiplinList = await _noteBusiness.GetList(x => x.ParentNoteId == workspace.Id);
                    var desiplin = desiplinList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains(desciplin.Value.ToString().ToLower())).FirstOrDefault();
                    var desiplinFolderId = "";
                    if (desiplin != null)
                    {
                        desiplinFolderId = desiplin.Id;

                    }
                    else
                    {
                        var foldermodel = new NoteTemplateViewModel
                        {
                            TemplateCode = "GENERAL_FOLDER",
                            DataAction = DataActionEnum.Create,
                            OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            ParentNoteId = workspace.Id,
                            StartDate = DateTime.Now.ApplicationNow().Date,
                        };

                        var newmodel1 = await _noteBusiness.GetNoteDetails(foldermodel);
                        newmodel1.NoteSubject = desciplin.Value.ToString();
                        newmodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        await _noteBusiness.ManageNote(newmodel1);
                        desiplinFolderId = newmodel1.Id;
                    }
                    var statementList = await _noteBusiness.GetList(x => x.ParentNoteId == desiplinFolderId);
                    var statement = desiplinList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains(projectSubFolder.Value.ToString().ToLower())).FirstOrDefault();
                    //var statement = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains(projectSubFolder.Value.ToLower())).FirstOrDefault();
                    var statementFolderId = "";
                    if (statement != null)
                    {
                        statementFolderId = statement.Id;
                    }
                    else
                    {
                        var foldermodel = new NoteTemplateViewModel
                        {
                            TemplateCode = "GENERAL_FOLDER",
                            DataAction = DataActionEnum.Create,
                            OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            NoteStatusCode = "NOTE_STATUS_INPROGRESS",
                            ParentNoteId = desiplinFolderId,
                            StartDate = DateTime.Now.ApplicationNow().Date,
                        };

                        var newmodel1 = await _noteBusiness.GetNoteDetails(foldermodel);
                        newmodel1.NoteSubject = projectSubFolder.Value.ToString();
                        newmodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        await _noteBusiness.ManageNote(newmodel1);
                        statementFolderId = newmodel1.Id;
                    }
                    //var rev = _noteBusiness.GetAllChildForWorkspace(statementFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains(revision.Value.ToLower())).FirstOrDefault();
                    var revList = await _noteBusiness.GetList(x => x.ParentNoteId == statementFolderId);
                    var rev = desiplinList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains(revision.Value.ToString().ToLower())).FirstOrDefault();
                    var revParentId = "";
                    if (rev != null)
                    {
                        revParentId = rev.Id;
                    }
                    else
                    {
                        var foldermodel = new NoteTemplateViewModel
                        {
                            TemplateCode = "GENERAL_FOLDER",
                            DataAction = DataActionEnum.Create,
                            OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            NoteStatusCode = "NOTE_STATUS_INPROGRESS",
                            ParentNoteId = statementFolderId,
                            StartDate = DateTime.Now.ApplicationNow().Date,
                        };

                        var newmodel1 = await _noteBusiness.GetNoteDetails(foldermodel);
                        newmodel1.NoteSubject = revision.Value.ToString();
                        newmodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        await _noteBusiness.ManageNote(newmodel1);
                        revParentId = newmodel1.Id;
                    }
                    var revnote = await _noteBusiness.CopyDocument(doc.Id, revParentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                    //_repoNote.CreateOneToOneRelationshipToReferenceType(revnote.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);


                    if (revision.Value.ToString().Contains("Rev"))
                    {
                        var latestrevfolderList = await _noteBusiness.GetList(x => x.ParentNoteId == statementFolderId);
                        var latestrevfolder = desiplinList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains("latest")).FirstOrDefault();
                        //var latestrevfolder = _noteBusiness.GetAllChildForWorkspace(statementFolderId).Where(x => x.Subject.ToLower().Contains("latest")).FirstOrDefault();
                        if (latestrevfolder != null)
                        {

                            var revDoc = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { Id = latestrevfolder.Id });

                            if (revDoc != null)
                            {
                                var check = revDoc.NoteNo == doc.NoteNo ? revDoc : null;// .Where(x => x.Name == doc.NoteNo).FirstOrDefault();

                                if (check != null)
                                {
                                    //var doc1 = new NoteViewModel
                                    //{
                                    //    Id = check.Id.Value.ToString(),
                                    //    DataAction = DataOperation.Correct,
                                    //};
                                    await _noteBusiness.Delete(check.Id.ToString());
                                }
                                var note = await _noteBusiness.CopyDocument(doc.Id, latestrevfolder.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                                //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                            }

                        }
                        else
                        {
                            var foldermodel = new NoteTemplateViewModel
                            {

                                TemplateCode = "GENERAL_FOLDER",
                                DataAction = DataActionEnum.Create,
                                OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                                ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                                RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                                NoteStatusCode = "NOTE_STATUS_INPROGRESS",
                                ParentNoteId = statementFolderId,
                                StartDate = DateTime.Now.ApplicationNow().Date,
                            };

                            var newmodel1 = await _noteBusiness.GetNoteDetails(foldermodel);
                            newmodel1.NoteSubject = "Latest revision Files";
                            newmodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            await _noteBusiness.ManageNote(newmodel1);
                            var note = await _noteBusiness.CopyDocument(doc.Id, newmodel1.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                            //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                        }

                    }
                    if (code.Value.ToString().Contains("AFC"))
                    {
                        var parentId = "";
                        var folderList = await _noteBusiness.GetList(x => x.ParentNoteId == statementFolderId);
                        var folder = folderList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains("signed")).FirstOrDefault();
                        //var folder = _noteBusiness.GetAllChildForWorkspace(statementFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains("signed")).FirstOrDefault();
                        if (folder != null)
                        {
                            parentId = folder.Id;
                        }
                        else
                        {
                            var foldermodel = new NoteTemplateViewModel
                            {

                                TemplateCode = "GENERAL_FOLDER",
                                DataAction = DataActionEnum.Create,
                                OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                                ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                                RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                                NoteStatusCode = "NOTE_STATUS_INPROGRESS",
                                ParentNoteId = statementFolderId,
                                StartDate = DateTime.Now.ApplicationNow().Date,
                            };

                            var newmodel = await _noteBusiness.GetNoteDetails(foldermodel);
                            newmodel.NoteSubject = "Signed AFC";
                            newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            newmodel.ParentNoteId = statementFolderId;
                            await _noteBusiness.ManageNote(newmodel);
                            parentId = newmodel.Id;
                        }

                        // doc.Id = 0;
                        //doc.Operation = DataOperation.Create;
                        //doc.ParentId = parentId;
                        //_noteBussiness.Manage(doc);
                        if (parentId.IsNotNullAndNotEmpty())
                        {
                            var revDoc = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { Id = parentId });

                            //var revDoc = _noteBusiness.CheckDocumentNoExist(parentId);
                            if (revDoc != null)
                            {
                                var check = revDoc.NoteNo == doc.NoteNo ? revDoc : null;// .Where(x => x.Name == doc.NoteNo).FirstOrDefault();

                                if (check != null)
                                {
                                    //var doc1 = new NoteViewModel
                                    //{
                                    //    Id = check.Id.Value,
                                    //    Operation = DataOperation.Correct,
                                    //};
                                    await _noteBusiness.Delete(check.Id);
                                }
                                var note = await _noteBusiness.CopyDocument(doc.Id, parentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                                //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                            }
                        }

                    }


                }
                else
                {
                    var desiplinList = await _noteBusiness.GetList(x => x.ParentNoteId == workspace.Id);
                    var desiplin = desiplinList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains(desciplin.Value.ToString().ToLower())).FirstOrDefault();

                    //var desiplin = _noteBusiness.GetAllChildForWorkspace(workspace.Id).Where(x => x.Subject != null && x.Subject.ToLower().Contains(desciplin.Value.ToLower())).FirstOrDefault();
                    var desiplinFolderId = "";
                    if (desiplin != null)
                    {
                        desiplinFolderId = desiplin.Id;

                    }
                    else
                    {
                        var foldermodel = new NoteTemplateViewModel
                        {
                            TemplateCode = "GENERAL_FOLDER",
                            DataAction = DataActionEnum.Create,
                            OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            NoteStatusCode = "NOTE_STATUS_INPROGRESS",
                            ParentNoteId = workspace.Id,
                            StartDate = DateTime.Now.ApplicationNow().Date,
                        };

                        var newmodel1 = await _noteBusiness.GetNoteDetails(foldermodel);
                        newmodel1.NoteSubject = desciplin.Value.ToString();
                        newmodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        await _noteBusiness.ManageNote(newmodel1);
                        desiplinFolderId = newmodel1.Id;
                    }

                    var revList = await _noteBusiness.GetList(x => x.ParentNoteId == desiplinFolderId);
                    var rev = revList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains(revision.Value.ToString().ToLower())).FirstOrDefault();

                    //var rev = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains(revision.Value.ToLower())).FirstOrDefault();
                    var revParentId = "";
                    if (rev != null)
                    {
                        revParentId = rev.Id;
                    }
                    else
                    {
                        var foldermodel = new NoteTemplateViewModel
                        {
                            TemplateCode = "GENERAL_FOLDER",
                            DataAction = DataActionEnum.Create,
                            OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            NoteStatusCode = "NOTE_STATUS_INPROGRESS",
                            ParentNoteId = desiplinFolderId,
                            StartDate = DateTime.Now.ApplicationNow().Date,
                        };

                        var newmodel1 = await _noteBusiness.GetNoteDetails(foldermodel);
                        newmodel1.NoteSubject = revision.Value.ToString();
                        newmodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        await _noteBusiness.ManageNote(newmodel1);
                        revParentId = newmodel1.Id;
                    }
                    var revnote = await _noteBusiness.CopyDocument(doc.Id, revParentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                    //_repoNote.CreateOneToOneRelationshipToReferenceType(revnote.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);


                    if (revision.Value.ToString().Contains("Rev"))
                    {
                        //var latestrevfolder = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject.ToLower().Contains("latest")).FirstOrDefault();
                        var latestrevfolderList = await _noteBusiness.GetList(x => x.ParentNoteId == desiplinFolderId);
                        var latestrevfolder = latestrevfolderList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains("latest")).FirstOrDefault();

                        if (latestrevfolder != null)
                        {

                            var revDoc = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { Id = latestrevfolder.Id });

                            if (revDoc != null)
                            {
                                var check = revDoc.NoteNo == doc.NoteNo ? revDoc : null;// .Where(x => x.Name == doc.NoteNo).FirstOrDefault();

                                if (check != null)
                                {

                                    await _noteBusiness.Delete(check.Id);
                                }
                                var note = await _noteBusiness.CopyDocument(doc.Id, latestrevfolder.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                                //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                            }

                        }
                        else
                        {
                            var foldermodel = new NoteTemplateViewModel
                            {
                                TemplateCode = "GENERAL_FOLDER",
                                DataAction = DataActionEnum.Create,
                                OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                                ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                                RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                                NoteStatusCode = "NOTE_STATUS_INPROGRESS",
                                ParentNoteId = desiplinFolderId,
                                StartDate = DateTime.Now.ApplicationNow().Date,
                            };

                            var newmodel1 = await _noteBusiness.GetNoteDetails(foldermodel);
                            newmodel1.NoteSubject = "Latest revision Files";
                            newmodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            await _noteBusiness.ManageNote(newmodel1);
                            var note = await _noteBusiness.CopyDocument(doc.Id, newmodel1.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                            //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                        }

                    }
                    if (code.Value.ToString().Contains("AFC"))
                    {
                        var parentId = "";
                        //var folder = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains("signed")).FirstOrDefault();
                        var folderList = await _noteBusiness.GetList(x => x.ParentNoteId == desiplinFolderId);
                        var folder = folderList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains("signed")).FirstOrDefault();

                        if (folder != null)
                        {
                            parentId = folder.Id;
                        }
                        else
                        {
                            var foldermodel = new NoteTemplateViewModel
                            {
                                TemplateCode = "GENERAL_FOLDER",
                                DataAction = DataActionEnum.Create,
                                OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                                ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                                RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                                NoteStatusCode = "NOTE_STATUS_INPROGRESS",
                                ParentNoteId = desiplin.Id,
                                StartDate = DateTime.Now.ApplicationNow().Date,
                            };

                            var newmodel = await _noteBusiness.GetNoteDetails(foldermodel);
                            newmodel.NoteSubject = "Signed AFC";
                            newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            newmodel.ParentNoteId = desiplinFolderId;
                            await _noteBusiness.ManageNote(newmodel);
                            parentId = newmodel.Id;
                        }

                        // doc.Id = 0;
                        //doc.Operation = DataOperation.Create;
                        //doc.ParentId = parentId;
                        //_noteBussiness.Manage(doc);
                        if (parentId.IsNotNullAndNotEmpty())
                        {
                            //var revDoc = awai _noteBusiness.CheckDocumentNoExist(parentId);
                            var revDoc = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { Id = parentId });

                            if (revDoc != null)
                            {
                                var check = revDoc.NoteNo == doc.NoteNo ? revDoc : null;// .Where(x => x.Name == doc.NoteNo).FirstOrDefault();

                                if (check != null)
                                {
                                    await _noteBusiness.Delete(check.Id);
                                }
                                var note = await _noteBusiness.CopyDocument(doc.Id, parentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                                //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                            }
                        }

                    }

                }

            }
            return null;
        }
        public async Task<ActionResult> MoveVendorDocument(NoteTemplateViewModel doc)
        {
            var desciplin = doc.ColumnList.FirstOrDefault(x => x.Name == "discipline");
            var revision = doc.ColumnList.FirstOrDefault(x => x.Name == "revision");
            var vendor = doc.ColumnList.FirstOrDefault(x => x.Name == "vendorList");
            var code = doc.ColumnList.FirstOrDefault(x => x.Name == "code");
            //var workspace = await _noteBusiness.GetWorkspaceDataForAdmin().Where(x => x.Name.ToLower().Contains("vendor documents")).FirstOrDefault();
            var workspaceforDataAdmin = await _noteBusiness.GetWorkspaceDataForAdmin();
            var workspace = workspaceforDataAdmin.Where(x => x.NoteSubject.ToLower().Contains("vendor documents")).FirstOrDefault();
            if (workspace != null)
            {
                //var vendorFolder = vendorFolderList.Where(x => x.Subject != null && x.Subject.ToLower().Contains(vendor.Value.ToString()ToLower()) && x.Subject.Contains("Vendor Document")).FirstOrDefault();
                var vendorFolderList = await _noteBusiness.GetList(x => x.ParentNoteId == workspace.Id);
                var vendorFolder = vendorFolderList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains("Vendor Document")).FirstOrDefault();

                var venFolderId = "";
                if (vendorFolder != null)
                {
                    venFolderId = vendorFolder.Id;
                }
                else
                {
                    var foldermodel = new NoteTemplateViewModel
                    {
                        TemplateCode = "GENERAL_FOLDER",
                        DataAction = DataActionEnum.Create,
                        OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        NoteStatusCode = "NOTE_STATUS_INPROGRESS",
                        ParentNoteId = workspace.Id,
                        StartDate = DateTime.Now.ApplicationNow().Date,
                    };

                    var newmodel1 = await _noteBusiness.GetNoteDetails(foldermodel);
                    newmodel1.NoteSubject = vendor.Value + " " + "Vendor Document";
                    newmodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    await _noteBusiness.ManageNote(newmodel1);
                    venFolderId = newmodel1.Id;
                }
                //var desiplin = _noteBusiness.GetAllChildForWorkspace(venFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains(desciplin.Value.ToLower())).FirstOrDefault();

                var desiplinList = await _noteBusiness.GetList(x => x.ParentNoteId == venFolderId);
                var desiplin = vendorFolderList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains(desciplin.Value.ToString().ToLower())).FirstOrDefault();

                var desiplinFolderId = "";
                if (desiplin != null)
                {
                    desiplinFolderId = desiplin.Id;

                }
                else
                {
                    var foldermodel = new NoteTemplateViewModel
                    {
                        TemplateCode = "GENERAL_FOLDER",
                        DataAction = DataActionEnum.Create,
                        OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        NoteStatusCode = "NOTE_STATUS_INPROGRESS",
                        ParentNoteId = venFolderId,
                        StartDate = DateTime.Now.ApplicationNow().Date,
                    };

                    var newmodel1 = await _noteBusiness.GetNoteDetails(foldermodel);
                    newmodel1.NoteSubject = desciplin.Value.ToString();
                    newmodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    await _noteBusiness.ManageNote(newmodel1);
                    desiplinFolderId = newmodel1.Id;
                }
                //var rev = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains(revision.Value.ToLower())).FirstOrDefault();

                var revList = await _noteBusiness.GetList(x => x.ParentNoteId == desiplinFolderId);
                var rev = vendorFolderList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains(revision.Value.ToString().ToLower())).FirstOrDefault();


                var revParentId = "";
                if (rev != null)
                {
                    revParentId = rev.Id;
                }
                else
                {
                    var foldermodel = new NoteTemplateViewModel
                    {
                        TemplateCode = "GENERAL_FOLDER",
                        DataAction = DataActionEnum.Create,
                        OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                        NoteStatusCode = "NOTE_STATUS_INPROGRESS",
                        ParentNoteId = desiplinFolderId,
                        StartDate = DateTime.Now.ApplicationNow().Date,
                    };

                    var newmodel1 = await _noteBusiness.GetNoteDetails(foldermodel);
                    newmodel1.NoteSubject = revision.Value.ToString();
                    newmodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    await _noteBusiness.ManageNote(newmodel1);
                    revParentId = newmodel1.Id;
                }
                var revnote = await _noteBusiness.CopyDocument(doc.Id, revParentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                //_repoNote.CreateOneToOneRelationshipToReferenceType(revnote.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);


                if (revision.Value.ToString().Contains("Rev"))
                {
                    //var latestrevfolder = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject.ToLower().Contains("latest")).FirstOrDefault();
                    var latestrevfolderList = await _noteBusiness.GetList(x => x.ParentNoteId == desiplinFolderId);
                    var latestrevfolder = vendorFolderList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains("latest")).FirstOrDefault();


                    if (latestrevfolder != null)
                    {

                        //var revDoc = _noteBusiness.CheckDocumentNoExist(latestrevfolder.Id);
                        var revDoc = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { Id = latestrevfolder.Id });

                        if (revDoc != null)
                        {
                            var check = revDoc.NoteNo == doc.NoteNo ? revDoc : null;// .Where(x => x.Name == doc.NoteNo).FirstOrDefault();

                            if (check != null)
                            {
                                await _noteBusiness.Delete(check.Id);
                            }
                            var note = await _noteBusiness.CopyDocument(doc.Id, latestrevfolder.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                            //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                        }

                    }
                    else
                    {
                        var foldermodel = new NoteTemplateViewModel
                        {
                            TemplateCode = "GENERAL_FOLDER",
                            DataAction = DataActionEnum.Create,
                            OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            NoteStatusCode = "NOTE_STATUS_INPROGRESS",
                            ParentNoteId = desiplinFolderId,
                            StartDate = DateTime.Now.ApplicationNow().Date,
                        };

                        var newmodel1 = await _noteBusiness.GetNoteDetails(foldermodel);
                        newmodel1.NoteSubject = "Latest revision Files";
                        newmodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        await _noteBusiness.ManageNote(newmodel1);
                        var note = await _noteBusiness.CopyDocument(doc.Id, newmodel1.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                        //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                    }

                }
                if (code.Value.ToString().Contains("AFC"))
                {
                    var parentId = "";
                    //var folder = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains("signed")).FirstOrDefault();
                    var folderList = await _noteBusiness.GetList(x => x.ParentNoteId == desiplinFolderId);
                    var folder = vendorFolderList.Where(x => x.NoteSubject != null && x.NoteSubject.ToLower().Contains("signed")).FirstOrDefault();

                    if (folder != null)
                    {
                        parentId = folder.Id;
                    }
                    else
                    {
                        var foldermodel = new NoteTemplateViewModel
                        {
                            TemplateCode = "GENERAL_FOLDER",
                            DataAction = DataActionEnum.Create,
                            OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
                            NoteStatusCode = "NOTE_STATUS_INPROGRESS",
                            ParentNoteId = desiplinFolderId,
                            StartDate = DateTime.Now.ApplicationNow().Date,
                        };

                        var newmodel = await _noteBusiness.GetNoteDetails(foldermodel);
                        newmodel.NoteSubject = "Signed AFC";
                        newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        newmodel.ParentNoteId = desiplinFolderId;
                        await _noteBusiness.ManageNote(newmodel);
                        parentId = newmodel.Id;
                    }

                    // doc.Id = 0;
                    //doc.Operation = DataOperation.Create;
                    //doc.ParentId = parentId;
                    //_noteBussiness.Manage(doc);
                    if (parentId.IsNotNullAndNotEmpty())
                    {
                        var revDoc = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { Id = parentId });
                        if (revDoc != null)
                        {
                            var check = revDoc.NoteNo == doc.NoteNo ? revDoc : null;// .Where(x => x.Name == doc.NoteNo).FirstOrDefault();

                            if (check != null)
                            {
                                //var doc1 = new NoteViewModel
                                //{
                                //    Id = check.Id.Value,
                                //    Operation = DataOperation.Correct,
                                //};
                                await _noteBusiness.Delete(check.Id);
                            }
                            var note = await _noteBusiness.CopyDocument(doc.Id, parentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
                            //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                        }
                    }


                }
            }
            return null;
        }




        public string GetCellValues(JToken row, int index)
        {
            JArray cells = (JArray)row.SelectToken("cells");


            foreach (JToken cell in cells)
            {
                var z = (int)cell.SelectToken("index");
                if (z == index)
                {
                    return (string)cell.SelectToken("value");
                }

            }
            return string.Empty;
        }

        public string GetCellValuesByString(JToken row, string str)
        {
            JArray cells = (JArray)row.SelectToken("cells");


            foreach (JToken cell in cells)
            {
                var z = (int)cell.SelectToken("index");

                var val = (string)cell.SelectToken("value");
                if (val.IsNotNullAndNotEmpty())
                {
                    if (val.ToLower().StartsWith(str.ToLower()))
                    {
                        return (string)cell.SelectToken("value");
                    }
                }
            }
            return string.Empty;
        }

        public bool GetValidateEnumValue(string enumName, string code)
        {

            if (enumName.Contains("DocumentApprovalStatuTypeEnum"))
            {
                if (!Enum.IsDefined(typeof(DocumentApprovalStatuTypeEnum), code))
                    return false;


            }
            else
            {
                if (!Enum.IsDefined(typeof(StageStatuTypeEnum), code))
                    return false;

            }
            return true;
        }


        public async Task<Dictionary<string, string>> DeleteNote(string id)
        {
            Dictionary<string, string> result1 = new Dictionary<string, string>();
            List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            var subfolders = await _documentBusiness.GetAllChildByParentId(id, subfoldersList);
            if (subfolders.Any())
            {
                result1.Add("Alert", "You cannot delete this folder because it has one more child documents.");
                return result1;
            }
            await _noteBusiness.Delete(id);
            return result1;
        }
        public async Task<IActionResult> DeleteFolder(string id)
        {
            //List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            //var subfolders = await _documentBusiness.GetAllChildByParentId(id, subfoldersList);
            //if (subfolders.Any())
            //{
            //    return Json(new { success = false, errors = "You cannot delete this folder because it has one more child documents." });

            //}
            //await _noteBusiness.Delete(id);
            var allChildFoldersAndDocuments = await _documentBusiness.GetAllPermittedChildByParentId(_userContext.UserId, id);
            var ids = allChildFoldersAndDocuments.Select(x => x.Id).ToList();
            ids.Add(id);
            var noteids = "'" + String.Join("','", ids) + "'";
            if (noteids.IsNotNullAndNotEmpty())
            {
                await _documentBusiness.DeleteNotesbyNoteIds(noteids);
            }
            return Json(new { success = true });
        }
        public async Task<IActionResult> DeleteMultipleNote(string ids)
        {
            if (ids.IsNotNullAndNotEmpty())
            {
                ids = ids.Trim(',');
                var idsarray = ids.Split(',').ToList();
                var noteids = new List<string>();
                noteids.AddRange(idsarray);
                foreach (var id in idsarray)
                {
                    //List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
                    //var subfolders = await _documentBusiness.GetAllChildByParentId(id, subfoldersList);
                    //if (subfolders.Any())
                    //{
                    //    return Json(new { success = false, errors = "You cannot delete this folder because it has one more child documents." });
                    //}
                    //await _noteBusiness.Delete(id);
                    var allChildFoldersAndDocuments = await _documentBusiness.GetAllPermittedChildByParentId(_userContext.UserId, id);
                    var childids = allChildFoldersAndDocuments.Select(x => x.Id).ToList();
                    if (childids.Count > 0)
                    {
                        noteids.AddRange(childids);
                    }
                }
                var str = "'" + String.Join("','", noteids) + "'";
                await _documentBusiness.DeleteNotesbyNoteIds(str);
            }
            return Json(new { success = true });
        }
        public async Task<IActionResult> ArchiveNote(string id)
        {
            //List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            //var subfolders = await _documentBusiness.GetAllChildByParentId(id, subfoldersList);
            //if (subfolders.Any())
            //{
            //    return Json(new { success = false, errors = "You cannot archive this folder because it has one more child documents." });
            //}
            //await _noteBusiness.Archive(id);
            var allChildFoldersAndDocuments = await _documentBusiness.GetAllPermittedChildByParentId(_userContext.UserId, id);
            var ids = allChildFoldersAndDocuments.Select(x => x.Id).ToList();
            ids.Add(id);
            var noteids = "'" + String.Join("','", ids) + "'";
            if (noteids.IsNotNullAndNotEmpty())
            {
                await _documentBusiness.ArchiveNotesbyNoteIds(noteids);
            }
            return Json(new { success = true });
        }
        public async Task<IActionResult> ArchiveMultipleNote(string ids)
        {
            if (ids.IsNotNullAndNotEmpty())
            {
                ids = ids.Trim(',');
                var idsarray = ids.Split(',');
                var noteids = new List<string>();
                noteids.AddRange(idsarray);
                foreach (var id in idsarray)
                {
                    //List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
                    //var subfolders = await _documentBusiness.GetAllChildByParentId(id, subfoldersList);
                    //if (subfolders.Any())
                    //{
                    //    return Json(new { success = false, errors = "You cannot archive this folder because it has one more child documents." });
                    //}
                    //await _noteBusiness.Archive(id);
                    var allChildFoldersAndDocuments = await _documentBusiness.GetAllPermittedChildByParentId(_userContext.UserId, id);
                    var childids = allChildFoldersAndDocuments.Select(x => x.Id).ToList();
                    if (childids.Count > 0)
                    {
                        noteids.AddRange(childids);
                    }
                }
                var str = "'" + String.Join("','", noteids) + "'";
                await _documentBusiness.ArchiveNotesbyNoteIds(str);
            }
            return Json(new { success = true });
        }
        public async Task<Dictionary<string, string>> MoveNote(string sourceId, string targetId)
        {
            Dictionary<string, string> result1 = new Dictionary<string, string>();
            var model = await _noteBusiness.GetSingleById(sourceId);
            var isUnique = await _documentBusiness.IsUniqueDocumentFolder(targetId, model.NoteSubject, null);
            if (!isUnique)
            {
                result1.Add("Error", model.NoteSubject + " Folder/Document with same name already exist.");
                return result1;
                //ModelState.AddModelError(model.NoteSubject, "Folder with same name already exist.");
                //return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
            var isUniqueDocument = await _documentBusiness.IsUniqueGeneralDocument(targetId, model.NoteSubject, null);
            if (!isUniqueDocument)
            {
                result1.Add("Error", model.NoteSubject + " File with same No already exist.");
                return result1;
                // ModelState.AddModelError(model.NoteSubject, "File with same name already exist.");
                // return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
            if (model.IsNotNull())
            {
                var previousParentId = model.ParentNoteId;
                model.ParentNoteId = targetId;
                var parents = await _documentBusiness.GetAllParentByNoteId(sourceId);
                var parentids = parents.Select(x => x.Id).ToList();
                var parentnoteIds = string.Join(",", parentids);
                var parentnotePermissions = await _documentBusiness.GetAllNotePermissionByParentId(parentnoteIds);
                var result = await _noteBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    if (targetId.IsNotNull())
                    {
                        // Delete previous parent permission from note and its child
                        if (previousParentId.IsNotNull())
                        {
                            await _documentPermissionBusiness.DeleteChildPermissions(sourceId, parentnotePermissions);
                        }
                        //Add new parent permission to the parent note                   
                        await _documentPermissionBusiness.ManageInheritedPermission(sourceId, targetId);

                        // Add new parent permission to all its child
                        await _documentPermissionBusiness.ManageChildPermissions(sourceId);
                        await _documentPermissionBusiness.ManageParentPermissions(sourceId);
                    }
                    if (model.TemplateCode == "GENERAL_FOLDER")
                    {
                        //var parentlist = await _documentBusiness.GetAllParentByNoteId(targetId);
                        //parentlist.Add(new FolderViewModel { Id = targetId,Level=1 });
                        var docTags = await _tagBusiness.GetList(x => x.NtsId == sourceId && x.TagId == null);
                        foreach (var docTag in docTags)
                        {
                            var tags = await _tagBusiness.GetList(x => x.TagSourceReferenceId == docTag.TagSourceReferenceId && x.TagId == null);
                            if (tags.Count > 0)
                            {
                                var tagsIds = tags.Select(x => x.Id);
                                var ids = "'" + String.Join("','", tagsIds) + "'";
                                var res = await _documentBusiness.UpdateTagsByDocumentIds(ids);
                            }
                            var parentlist = await _documentBusiness.GetAllParentByNoteId(docTag.TagSourceReferenceId);
                            foreach (var parent in parentlist)
                            {
                                var tag = new NtsTagViewModel { NtsId = parent.Id, NtsType = NtsTypeEnum.Note, TagSourceReferenceId = docTag.TagSourceReferenceId };
                                await _tagBusiness.Create(tag);
                            }
                        }
                    }
                    else
                    {
                        var tags = await _tagBusiness.GetList(x => x.TagSourceReferenceId == sourceId && x.TagId == null);
                        if (tags.Count > 0)
                        {
                            var tagsIds = tags.Select(x => x.Id);
                            var ids = "'" + String.Join("','", tagsIds) + "'";
                            var res = await _documentBusiness.UpdateTagsByDocumentIds(ids);
                            if (res)
                            {
                                var parentlist = await _documentBusiness.GetAllParentByNoteId(sourceId);
                                foreach (var parent in parentlist)
                                {
                                    var tag = new NtsTagViewModel { NtsId = parent.Id, NtsType = NtsTypeEnum.Note, TagSourceReferenceId = sourceId };
                                    await _tagBusiness.Create(tag);
                                }
                            }

                        }
                        else
                        {
                            var parentlist = await _documentBusiness.GetAllParentByNoteId(sourceId);
                            foreach (var parent in parentlist)
                            {
                                var tag = new NtsTagViewModel { NtsId = parent.Id, NtsType = NtsTypeEnum.Note, TagSourceReferenceId = sourceId };
                                await _tagBusiness.Create(tag);
                            }
                        }


                    }
                    return result1;
                }
            }
            //var templateModel = new NoteTemplateViewModel();
            //templateModel.ActiveUserId = _userContext.UserId;
            //templateModel.DataAction = DataActionEnum.Edit;
            //templateModel.NoteId = sourceId;
            //var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
            //newmodel.PreviousParentId = newmodel.ParentNoteId;
            //newmodel.ParentNoteId = targetId;
            //var result = await _noteBusiness.ManageNote(newmodel);
            //if (result.IsSuccess)
            //{
            //    //if (targetId.IsNotNull())
            //    //{
            //    //    // Note managing parent inhertited permission as it will get managed from postscript
            //    //    //Add new parent permission to the parent note                   
            //    //   // await _documentPermissionBusiness.ManageInheritedPermission(newmodel.NoteId, targetId);
            //    //    // Delete previous parent permission from note and its child
            //    //    if (newmodel.PreviousParentId.IsNotNull())
            //    //    {
            //    //        await _documentPermissionBusiness.DeleteOldParentPermission(newmodel.NoteId);
            //    //    }
            //    //    // Add new parent permission to all its child
            //    //    await _documentPermissionBusiness.ManageChildPermissions(targetId);
            //    //}
            //    return Json(new { success = true });

            //}
            // return Json(new { success = false });
            return result1;
        }
        public async Task<Dictionary<string, string>> CopyNote(string sourceId, string targetId)
        {
            Dictionary<string, string> result1 = new Dictionary<string, string>();
            var model = await _noteBusiness.GetSingleById(sourceId);
            var isUnique = await _documentBusiness.IsUniqueDocumentFolder(targetId, model.NoteSubject, null);
            if (!isUnique)
            {
                // ModelState.AddModelError(model.NoteSubject, "Folder with same name already exist.");
                // return Json(new { success = false, errors = ModelState.SerializeErrors() });
                result1.Add("Error", model.NoteSubject + " Folder/Document with same name already exist.");
                return result1;
            }

            var ExistingId = await _documentBusiness.IsUniqueGeneralDocumentByNo(targetId, model.NoteNo, null);
            if (ExistingId.IsNotNullAndNotEmpty())
            {
                //ModelState.AddModelError(model.NoteSubject, "File with same No already exist.");
                //return Json(new { success = false, errors = ModelState.SerializeErrors() });
                result1.Add("Error", model.NoteSubject + " File with same No already exist.");
                return result1;
            }
            var result = await CreateNote(sourceId, targetId);
            if (result.IsSuccess)
            {
                var documents = await _documentBusiness.GetAllFiles(sourceId, null, null);
                foreach (var doc in documents)
                {
                    await CreateNote(doc.Id, result.Item.NoteId);
                }
                List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
                var subfolders = await _documentBusiness.GetAllChildByParentId(sourceId, subfoldersList);
                foreach (var folder in subfolders.Where(x => x.FolderCode == "GENERAL_FOLDER"))
                {
                    await CopyNote(folder.Id, result.Item.NoteId);
                }
                // return Json(new { success = true });
                return result1;
            }
            // return Json(new { success = false });
            return result1;
        }
        public async Task<Dictionary<string, string>> CopyMultipleNote(string sourceIds, string targetId)
        {
            Dictionary<string, string> result1 = new Dictionary<string, string>();
            if (sourceIds.IsNotNullAndNotEmpty())
            {
                sourceIds = sourceIds.Trim(',');
                var idsarray = sourceIds.Split(',');
                foreach (var id in idsarray)
                {
                    var result = await CopyNote(id, targetId);
                    result1.ToList().AddRange(result);
                }
                return result1;
            }
            return result1;
        }
        public async Task<Dictionary<string, string>> MoveMultipleNote(string sourceIds, string targetId)
        {
            Dictionary<string, string> result1 = new Dictionary<string, string>();
            if (sourceIds.IsNotNullAndNotEmpty())
            {
                sourceIds = sourceIds.Trim(',');
                var idsarray = sourceIds.Split(',');
                foreach (var id in idsarray)
                {
                    var result = await MoveNote(id, targetId);
                    result1.ToList().AddRange(result);
                }
                return result1;
            }
            return result1;
        }
        private async Task<CommandResult<NoteTemplateViewModel>> CreateNote(string sourceId, string targetId)
        {
            var templateModel = new NoteTemplateViewModel();
            templateModel.ActiveUserId = _userContext.UserId;
            templateModel.DataAction = DataActionEnum.Create;
            templateModel.NoteId = sourceId;
            templateModel.SetUdfValue = true;
            var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
            newmodel.NoteId = null;
            newmodel.ParentNoteId = targetId;
            newmodel.StartDate = System.DateTime.Now;
            newmodel.SetUdfValue = true;
            var result = await _noteBusiness.ManageNote(newmodel);
            return result;
        }

        public ActionResult GetArchive()
        {
            return View();
        }


        public async Task<ActionResult> GetArchivedDocumentData([DataSourceRequest] DataSourceRequest request, DocumentViewModel search = null)
        {
            //search.UserId = LoggedInUserId;
            //var result = _business.GetArchivedDocumentData(search).ToList();
            var result = await _documentPermissionBusiness.GetArchive(_userContext.UserId);
            var j = Json(result);
            //var j = Json(result.ToDataSourceResult(request));
            return j;
        }

        public async Task<ActionResult> GetArchivedDocumentDataGrid(DocumentViewModel search = null)
        {
            //search.UserId = LoggedInUserId;
            //var result = _business.GetArchivedDocumentData(search).ToList();
            var result = await _documentPermissionBusiness.GetArchive(_userContext.UserId);
            var j = Json(result);
            return j;
        }

        public ActionResult GetBinData()
        {
            return View();
        }


        public async Task<ActionResult> GetBinDocumentData([DataSourceRequest] DataSourceRequest request, DocumentViewModel search = null)
        {
            //search.UserId = LoggedInUserId;
            //var result = _business.GetArchivedDocumentData(search).ToList();
            var result = await _documentPermissionBusiness.GetBinDocumentData(_userContext.UserId);
            var j = Json(result);
            //var j = Json(result.ToDataSourceResult(request));
            return j;
        }

        public async Task<ActionResult> GetBinDocumentDataGrid(DocumentViewModel search = null)
        {
            //search.UserId = LoggedInUserId;
            //var result = _business.GetArchivedDocumentData(search).ToList();
            var result = await _documentPermissionBusiness.GetBinDocumentData(_userContext.UserId);
            var j = Json(result);
            return j;
        }


        [HttpGet]

        public async Task<JsonResult> DeleteDocument(string NoteId)
        {
            var result = await _documentPermissionBusiness.DeleteDocument(NoteId);
            return Json("true");
        }



        [HttpGet]

        public async Task<JsonResult> RestorebinDocument(string NoteId)
        {
            var result = await _documentPermissionBusiness.RestoreBinDocument(NoteId);
            return Json("true");
        }


        [HttpGet]

        public async Task<JsonResult> RestoreArchivedDocument(string NoteId)
        {
            var result = await _documentPermissionBusiness.RestoreArchiveDocument(NoteId, "");
            return Json("true");
        }

        [HttpGet]

        public async Task<JsonResult> Getfolderpath(string NoteId)
        {
            var result = await _documentPermissionBusiness.GetFolderByParent(NoteId);
            //   foreach (var item in ressult)
            {
                var folderpath = "";
                //  if (item.ParentId.IsNotNull())
                {
                    // var plist = await GetFolderByParent(item.ParentId);
                    foreach (var i in result)
                    {
                        folderpath += " " + i.Name + " >";
                    }

                    return Json(folderpath);
                }

            }
            //   return Json("true");
        }



        public async Task<ActionResult> SaveFile(IEnumerable<IFormFile> files, bool enableSnapshot = true, bool skipCompression = false, bool skipExistingFile = false)
        {
            if (ModelState.IsValid)
            {
                CommandResult<FileViewModel> result = null;
                foreach (var file in files)
                {

                    var fileName = Path.GetFileName(file.FileName.Split("/").LastOrDefault());
                    if (skipExistingFile)
                    {
                        var isFileExist = await _fileBusiness.GetList(x => x.FileName == fileName);
                        if (isFileExist.Count > 0)
                        {
                            return Json(new { success = false, errors = "Already exist" });
                        }
                    }
                    var divs = fileName.Split('.').ToList();
                    var ext = divs.LastOrDefault();


                    byte[] bytes;
                    var ms = new MemoryStream();
                    using (var memoryStream = new MemoryStream())
                    {
                        file.OpenReadStream().CopyTo(memoryStream);
                        file.OpenReadStream().CopyTo(ms);
                        bytes = memoryStream.ToArray();
                    }

                    var fileVM = new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = fileName,
                        FileExtension = Path.GetExtension(file.FileName)
                    };
                    result = await _fileBusiness.Create(fileVM);
                }
                if (result != null && result.IsSuccess)
                {
                    return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                }
                return Json(new { success = false, errors = ModelState.SerializeErrors() });

            }
            else
            {
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
        }
        public async Task<ActionResult> GetAllFolderAndDocument()
        {
            //var list = new List<FolderViewModel>();
            //list.Add(new FolderViewModel { Name = "Folder 1", ParentName = "Workspace 1", LastUpdatedBy = "Arshad", LastUpdatedDate = System.DateTime.Now });
            //list.Add(new FolderViewModel { Name = "Folder 2", ParentName = "Workspace 2", LastUpdatedBy = "Arshad", LastUpdatedDate = System.DateTime.Now });
            var list = await _documentBusiness.GetAllFolderAndDocumentByUserId(_userContext.UserId);
            return Json(list);
        }
        public async Task<ActionResult> UploadFolder(string parentId)
        {
            var model = new NoteTemplateViewModel
            {

                ParentNoteId = parentId,

            };
            return View("_UploadFolder", model);
        }
        public async Task<ActionResult> UploadFile(string parentId)
        {
            var model = new NoteTemplateViewModel
            {

                ParentNoteId = parentId,

            };
            ViewBag.BatchId = Guid.NewGuid().ToString();
            return View("_UploadFile", model);
        }
        [HttpPost]
        public async Task<ActionResult> ManageUploadedFile(NoteTemplateViewModel model)
        {
            if (model.UploadedContent != null && model.UploadedContent != "")
            {
                var result = await _documentBusiness.ManageUploadedFiles(model);
                if (!result.IsSuccess)
                {
                    foreach (var x in result.Messages)
                    {
                        ModelState.AddModelError(x.Key, x.Value);
                    }
                    return Json(new { success = false, errors = ModelState.SerializeErrors().ToHtmlError() });
                }
            }
            return Json(new { success = true, dataAction = model.DataAction.ToString(), id = model.Id });
        }
        public async Task<ActionResult> UploadFolderAndFiles(IEnumerable<IFormFile> files, string metaData)
        {
            if (metaData == null)
            {
                return Directory_Upload_Save(files);
            }

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(metaData));
            var serializer = new DataContractJsonSerializer(typeof(ChunkMetaData1));
            ChunkMetaData1 chunkData = serializer.ReadObject(ms) as ChunkMetaData1;
            string path = String.Empty;
            // The Name of the Upload component is "files"
            CommandResult<Synergy.App.ViewModel.FileViewModel> result = null;
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(chunkData.FileName);
                    var divs = fileName.Split('.').ToList();
                    var ext = Path.GetExtension(chunkData.FileName);
                    MemoryStream ms1 = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms1);

                    //byte[] bytes;
                    //using (var memoryStream = new MemoryStream())
                    //{
                    //    file.InputStream.CopyTo(memoryStream);
                    //    bytes = memoryStream.ToArray();
                    //}
                    //result = _businessFile.Create(new ViewModel.FileViewModel
                    //{
                    //    AttachmentType = AttachmentTypeEnum.File,
                    //    FileName = fileName,
                    //    FileExtension = ext,
                    //    ContentType = chunkData.ContentType,
                    //    ContentLength = file.ContentLength,
                    //    ContentByte = bytes,
                    //    // AttachmentDescription = desc
                    //}
                    result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms1.ToArray(),
                        ContentType = chunkData.ContentType,
                        ContentLength = file.Length,
                        FileName = fileName,
                        FileExtension = ext
                    }

                      );
                }
            }

            UploadedFileResult fileBlob = new UploadedFileResult();
            fileBlob.FileId = result.Item.Id.ToString();
            fileBlob.uploaded = chunkData.TotalChunks - 1 <= chunkData.ChunkIndex;
            fileBlob.fileUid = chunkData.UploadUid;
            var RelativePath = chunkData.RelativePath.Remove(chunkData.RelativePath.LastIndexOf('/') + 1);
            var folderData = RelativePath.Split('/');
            fileBlob.FolderName = folderData[folderData.Length - 2];
            var parentindexposition = Array.IndexOf(folderData, fileBlob.FolderName) - 1;
            if (parentindexposition >= 0)
            {
                fileBlob.ParentFolderName = folderData[parentindexposition];
            }
            fileBlob.RelativePath = chunkData.RelativePath;
            //fileBlob.RelativePath = RelativePath;
            return Json(fileBlob);

        }
        public ActionResult Directory_Upload_Save(IEnumerable<IFormFile> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
                    var physicalPath = Path.Combine(baseurl, "App_Data", fileName);
                    // The files are not actually saved in this demo
                    // file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult CalenderView()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddUploadedFile(NoteTemplateViewModel model)
        {
            if (model.FileIds != null && model.FileIds != "")
            {
                var result = await _documentBusiness.AddUploadedFiles(model);
                if (!result.IsSuccess)
                {
                    // result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                    foreach (var x in result.Messages)
                    {
                        ModelState.AddModelError(x.Key, x.Value);
                    }
                    return Json(new { success = false, errors = ModelState.SerializeErrors().ToHtmlError() });
                }
            }
            return Json(new { success = true, dataAction = model.DataAction.ToString(), id = model.Id });
        }
        public async Task<ActionResult> UploadFiles(IEnumerable<IFormFile> files, string metaData)
        {

            if (ModelState.IsValid)
            {
                CommandResult<FileViewModel> result = null;
                if (files.IsNotNull())
                {
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var divs = fileName.Split('.').ToList();
                        var ext = Path.GetExtension(file.FileName);
                        var ms = new MemoryStream();
                        file.OpenReadStream().CopyTo(ms);
                        result = await _fileBusiness.Create(new FileViewModel
                        {

                            FileName = fileName,
                            FileExtension = ext,
                            ContentType = file.ContentType,
                            ContentLength = file.Length,
                            ContentByte = ms.ToArray(),
                        }

                           );
                    }
                    if (result != null && result.IsSuccess)
                    {
                        return Json(new { success = true, fileId = result.Item.Id });
                    }
                    return Json(new { success = false, errors = ModelState.SerializeErrors() });
                }
                else
                {
                    return Json(new { success = false, errors = ModelState.SerializeErrors() });
                }
            }
            else
            {
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }

        }




        public async Task<ActionResult> ReadCalendarData([DataSourceRequest] DataSourceRequest request)
        {


            {
                var list = await _documentPermissionBusiness.GetCalenderDetails(_userContext.UserId);
                return Json(list);
            }


        }

        public ActionResult UserAttachments()
        {
            return View();
        }

        public async Task<ActionResult> GetAttachments()
        {
            var list = await _documentBusiness.GetUserAttachments(_userContext.UserId, _userContext.PortalId);
            return Json(list);
        }

        public async Task<IActionResult> ManageFolder(string id, string parentId)
        {

            if (id.IsNotNullAndNotEmpty())
            {
                var note = await _noteBusiness.GetSingle(x => x.Id == id);
                var existing = await _tableMetadataBusiness.GetTableDataByColumn("GENERAL_FOLDER", null, "NtsNoteId", id);
                if (existing != null && existing.Table.Columns["Code"] != null)
                {
                    note.Code = System.Convert.ToString(existing["Code"]);
                }
                note.DataAction = DataActionEnum.Edit;
                return View("ManageFolder", note);
            }
            else
            {

                return View("ManageFolder", new NoteViewModel { ParentNoteId = parentId, DataAction = DataActionEnum.Create });
            }

        }
        [HttpPost]
        public async Task<IActionResult> ManageNewFolder(NoteViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var validateNote = await _noteBusiness.GetList(x => x.NoteSubject == model.NoteSubject && x.ParentNoteId == model.ParentNoteId && (x.TemplateCode == "GENERAL_FOLDER" || x.TemplateCode == "WORKSPACE_GENERAL"));
                if (validateNote.Any())
                {
                    return Json(new { success = false, error = "Name is already Exist" });
                }
                if (model.SequenceOrder.IsNotNull())
                {
                    var validateNote1 = await _noteBusiness.GetList(x => x.SequenceOrder == model.SequenceOrder && x.ParentNoteId == model.ParentNoteId && (x.TemplateCode == "GENERAL_FOLDER" || x.TemplateCode == "WORKSPACE_GENERAL"));
                    if (validateNote1.Any())
                    {
                        return Json(new { success = false, error = "SequenceOrder is already Exist" });
                    }
                }

                var folderName = model.NoteSubject;
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = DataActionEnum.Create;
                templateModel.TemplateCode = "GENERAL_FOLDER";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                newmodel.NoteSubject = folderName;
                newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                newmodel.ParentNoteId = model.ParentNoteId;
                newmodel.SequenceOrder = model.SequenceOrder;
                newmodel.OwnerUserId = _userContext.UserId;
                dynamic exo = new System.Dynamic.ExpandoObject();
                if (model.Code.IsNotNullAndNotEmpty())
                {
                    ((IDictionary<String, Object>)exo).Add("Code", model.Code);
                }
                newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var result = await _noteBusiness.ManageNote(newmodel);
                if (!result.IsSuccess)
                {
                    //  result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                    foreach (var x in result.Messages)
                    {
                        ModelState.AddModelError(x.Key, x.Value);
                    }
                    return Json(new { success = false, errors = ModelState.SerializeErrors().ToHtmlError() });
                }

            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                var validateNote = await _noteBusiness.GetList(x => x.Id != model.Id && x.NoteSubject == model.NoteSubject && x.ParentNoteId == model.ParentNoteId && (x.TemplateCode == "GENERAL_FOLDER" || x.TemplateCode == "WORKSPACE_GENERAL"));
                if (validateNote.Any())
                {
                    return Json(new { success = false, error = "Name is already Exist" });
                }
                if (model.SequenceOrder.IsNotNull())
                {
                    var validateNote1 = await _noteBusiness.GetList(x => x.Id != model.Id && x.SequenceOrder == model.SequenceOrder && x.ParentNoteId == model.ParentNoteId && (x.TemplateCode == "GENERAL_FOLDER" || x.TemplateCode == "WORKSPACE_GENERAL"));
                    if (validateNote1.Any())
                    {
                        return Json(new { success = false, error = "SequenceOrder is already Exist" });
                    }
                }

                var folderName = model.NoteSubject;
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = DataActionEnum.Edit;
                templateModel.NoteId = model.Id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                newmodel.NoteSubject = folderName;
                newmodel.SequenceOrder = model.SequenceOrder;
                newmodel.DataAction = DataActionEnum.Edit;
                newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                newmodel.OwnerUserId = _userContext.UserId;
                dynamic exo = new System.Dynamic.ExpandoObject();
                if (model.Code.IsNotNullAndNotEmpty())
                {
                    ((IDictionary<String, Object>)exo).Add("Code", model.Code);
                }
                newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var result = await _noteBusiness.ManageNote(newmodel);
            }

            return Json(new { success = true });
        }
        public async Task<ActionResult> ReadFolderHierarchyData(string parentId, string noteId)
        {
            var result = await _documentBusiness.GetFolderByParent(parentId, noteId);

            return Json(result);
        }
        public ActionResult DmsTemplateDiagram(string id, bool isPopup = false)
        {
            if (isPopup)
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
                ViewBag.lo = "Popup";
            }
            else
            {
                ViewBag.Layout = "/Areas/Cms/Views/Shared/_LayoutCms.cshtml";
            }
            if (id != null && id != "")
            {
                ViewBag.TempId = id;
            }
            return View();
        }
        public async Task<ActionResult> SetFolderCount()
        {
            return View();
        }
        public async Task<bool> UpdateFolderCount()
        {
            //For Updating Document Count
            try
            {
                var documents = await _documentBusiness.GetAllDocuments();
                foreach (var document in documents)
                {
                    var taglist = await _tagBusiness.GetList(x => x.TagSourceReferenceId == document.Id && x.TagId == null);
                    if (taglist.Any())
                    {
                        continue;
                    }
                    var parentlist = await _documentBusiness.GetAllParentByNoteId(document.Id);
                    foreach (var parent in parentlist)
                    {
                        var tag = new NtsTagViewModel { NtsId = parent.Id, NtsType = NtsTypeEnum.Note, TagSourceReferenceId = document.Id };
                        var res = await _tagBusiness.Create(tag);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task<ActionResult> ResetDocumentTags()
        {
            return View();
        }
        public async Task<bool> ResetTagsForDocuments()
        {
            try
            {
                var documents = await _documentBusiness.GetAllDocuments();
                foreach (var document in documents)
                {
                    var taglist = await _tagBusiness.GetList(x => x.TagSourceReferenceId == document.Id && x.TagId == null);
                    if (taglist.Any())
                    {
                        var tagnoteIds = taglist.Select(x => x.NtsId).ToList();
                        var parentlist = await _documentBusiness.GetAllParentByNoteId(document.Id);
                        if (parentlist.Any())
                        {
                            var parentnoteIds = parentlist.Select(x => x.Id).ToList();
                            var idtobeDeleted = tagnoteIds.Except(parentnoteIds).ToList();
                            if (idtobeDeleted.Any())
                            {
                                var taglisttobedeleted = taglist.Where(x => idtobeDeleted.Contains(x.NtsId)).ToList();
                                if (taglisttobedeleted.Any())
                                {
                                    var tagsIds = taglisttobedeleted.Select(x => x.Id);
                                    var ids = "'" + String.Join("','", tagsIds) + "'";
                                    var res = await _documentBusiness.UpdateTagsByDocumentIds(ids);
                                }

                            }
                            var permissionlist = await _documentPermissionBusiness.GetPermissionList(document.Id);
                            if (permissionlist.Any())
                            {
                                if (idtobeDeleted.Any())
                                {
                                    var permissionlisttobedeleted = permissionlist.Where(x => idtobeDeleted.Contains(x.InheritedFromId)).ToList();
                                    if (permissionlisttobedeleted.Any())
                                    {
                                        var perIds = permissionlisttobedeleted.Select(x => x.Id);
                                        var ids = "'" + String.Join("','", perIds) + "'";
                                        var res = await _documentPermissionBusiness.DeletePermissionByDocumentIds(ids);
                                    }
                                }
                            }
                        }


                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        [Authorize]
        public async Task<object> GetSoureceFolders(string key)
        {
            try
            {
                var list = new List<FileExplorerViewModel>();
                var children = new List<FileExplorerViewModel>();
                if (key.IsNotNullAndNotEmpty())
                {
                    var note = await _noteBusiness.GetSingle(x => x.Id == key);
                    if (note.IsNotNull())
                    {
                        if (note.TemplateCode == "GENERAL_FOLDER" || note.TemplateCode == "WORKSPACE_GENERAL")
                        {
                            var clist = await _documentBusiness.GetAllChildWorkspaceAndFolder(_userContext.UserId, key);
                            children.AddRange(clist.Select(x => new FileExplorerViewModel
                            {
                                key = x.Id,
                                title = x.Name,
                                lazy = true,
                                folder = (x.FolderCode == "GENERAL_FOLDER"),
                                Workspace = (x.FolderCode == "WORKSPACE_GENERAL"),
                                FolderType = x.FolderType.Value,
                                Count = x.DocCount.ToString(),
                                WorkspaceId = x.WorkspaceId,
                                parentId = x.ParentId,
                                CanOpen = x.CanOpen,
                                ShowMenu = x.ShowMenu,
                                CanCreateSubFolder = x.CanCreateSubFolder,
                                CanRename = x.CanRename,
                                CanShare = x.CanShare,
                                CanMove = x.CanMove,
                                CanCopy = x.CanCopy,
                                CanArchive = x.CanArchive,
                                CanDelete = x.CanDelete,
                                CanSeeDetail = x.CanSeeDetail,
                                CanManagePermission = x.CanManagePermission,
                                TemplateCode = x.FolderCode,
                                CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false,
                                Sequence = x.SequenceNo,
                                CreatedDate = x.CreatedDate,
                                UpdatedDate = x.LastUpdatedDate,
                                NoteNo = x.DocumentNo,
                                CreatedBy = x.CreatedByUser,
                                WorkflowTemplateCode = x.WorkflowCode,
                                DocumentApprovalStatusType = x.DocumentApprovalStatusType,
                                WorkflowServiceId = x.WorkflowServiceId,
                                StatusName = x.NoteStatus,
                                CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
                                CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
                                WorkflowServiceStatus = x.WorkflowServiceStatus,
                                IsSelfWorkspace = x.IsSelfWorkspace,
                            }));
                            children = children.OrderBy(x => x.Sequence).ToList();
                        }
                        else
                        {
                            var clist = await _documentBusiness.GetAllChildWorkspaceFolderAndDocument(_userContext.UserId, key);
                            children.AddRange(clist.Select(x => new FileExplorerViewModel
                            {
                                key = x.Id,
                                title = x.Name,
                                lazy = true,
                                folder = (x.FolderCode == "GENERAL_FOLDER"),
                                Workspace = (x.FolderCode == "WORKSPACE_GENERAL"),
                                Document = (x.FolderCode != "GENERAL_FOLDER" && x.FolderCode != "WORKSPACE_GENERAL" && x.FolderType != FolderTypeEnum.File),
                                File = x.FolderType == FolderTypeEnum.File,
                                FileId = x.DocumentId,
                                FileSize = x.FileSize != 0 ? Helper.ByteSizeWithSuffix(x.FileSize) : "",
                                //FolderType = x.FolderCode == "WORKSPACE_GENERAL" ? FolderTypeEnum.Workspace : x.FolderCode == "GENERAL_FOLDER" ? FolderTypeEnum.Folder : FolderTypeEnum.Document,
                                FolderType = x.FolderType.Value,
                                Count = x.DocCount.ToString(),
                                WorkspaceId = x.WorkspaceId,
                                parentId = x.ParentId,
                                CanOpen = x.CanOpen,
                                ShowMenu = x.ShowMenu,
                                CanCreateSubFolder = x.CanCreateSubFolder,
                                CanRename = x.CanRename,
                                CanShare = x.CanShare,
                                CanMove = x.CanMove,
                                CanCopy = x.CanCopy,
                                CanArchive = x.CanArchive,
                                CanDelete = x.CanDelete,
                                CanSeeDetail = x.CanSeeDetail,
                                CanManagePermission = x.CanManagePermission,
                                TemplateCode = x.FolderCode,
                                CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false,
                                Sequence = x.SequenceNo,
                                CreatedDate = x.CreatedDate,
                                UpdatedDate = x.LastUpdatedDate,
                                NoteNo = x.DocumentNo,
                                CreatedBy = x.CreatedByUser,
                                WorkflowTemplateCode = x.WorkflowCode,
                                DocumentApprovalStatusType = x.DocumentApprovalStatusType,
                                WorkflowServiceId = x.WorkflowServiceId,
                                StatusName = x.NoteStatus,
                                CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
                                CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
                                WorkflowServiceStatus = x.WorkflowServiceStatus,
                                WorkflowServiceStatusName = x.WorkflowServiceStatusName,
                                IsSelfWorkspace = x.IsSelfWorkspace,
                            }));
                            children = children.OrderBy(x => x.Sequence).ToList();
                        }
                        if (note.ParentNoteId.IsNotNullAndNotEmpty())
                        {
                            await GetParentNoteId(note, note, list, children);
                        }
                        else
                        {
                            var workspaces = await _documentBusiness.GetFirstLevelWorkspacesByUser(_userContext.UserId);
                            list.AddRange(workspaces.Select(x => new FileExplorerViewModel
                            {
                                key = x.Id,
                                title = x.Name,
                                lazy = true,
                                folder = (x.FolderCode == "GENERAL_FOLDER"),
                                Workspace = (x.FolderCode == "WORKSPACE_GENERAL"),
                                FolderType = x.FolderType.Value,
                                children = x.Id == note.Id ? children : null,
                                active = (note.TemplateCode == "GENERAL_FOLDER" || note.TemplateCode == "WORKSPACE_GENERAL") ? note.Id == x.Id : note.ParentNoteId == x.Id,
                                expanded = x.Id == note.Id,
                                Count = x.DocCount.ToString(),
                                WorkspaceId = x.WorkspaceId,
                                parentId = x.ParentId,
                                ParentId = x.ParentId,
                                CanOpen = x.CanOpen,
                                ShowMenu = x.ShowMenu,
                                CanCreateSubFolder = x.CanCreateSubFolder,
                                CanRename = false,
                                CanShare = x.CanShare,
                                CanMove = x.CanMove,
                                CanCopy = x.CanCopy,
                                CanArchive = x.CanArchive,
                                CanDelete = x.CanDelete,
                                CanSeeDetail = x.CanSeeDetail,
                                CanManagePermission = x.CanManagePermission,
                                TemplateCode = x.FolderCode,
                                CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false,
                                Sequence = x.SequenceNo,
                                CreatedDate = x.CreatedDate,
                                UpdatedDate = x.LastUpdatedDate,
                                NoteNo = x.DocumentNo,
                                CreatedBy = x.CreatedByUser,
                                WorkflowTemplateCode = x.WorkflowCode,
                                DocumentApprovalStatusType = x.DocumentApprovalStatusType,
                                WorkflowServiceId = x.WorkflowServiceId,
                                StatusName = x.NoteStatus,
                                CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
                                CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
                                WorkflowServiceStatus = x.WorkflowServiceStatus,
                                IsSelfWorkspace = x.IsSelfWorkspace,
                            }));
                        }
                    }

                }
                else
                {
                    var workspaces = await _documentBusiness.GetFirstLevelWorkspacesByUser(_userContext.UserId);
                    list.AddRange(workspaces.Select(x => new FileExplorerViewModel
                    {
                        key = x.Id,
                        title = x.Name,
                        lazy = true,
                        folder = (x.FolderCode == "GENERAL_FOLDER"),
                        Workspace = (x.FolderCode == "WORKSPACE_GENERAL"),
                        FolderType = x.FolderType.Value,
                        Count = x.DocCount.ToString(),
                        WorkspaceId = x.WorkspaceId,
                        parentId = x.ParentId,
                        ParentId = x.ParentId,
                        CanOpen = x.CanOpen,
                        ShowMenu = x.ShowMenu,
                        CanCreateSubFolder = x.CanCreateSubFolder,
                        CanRename = false,
                        CanShare = x.CanShare,
                        CanMove = x.CanMove,
                        CanCopy = x.CanCopy,
                        CanArchive = x.CanArchive,
                        CanDelete = x.CanDelete,
                        CanSeeDetail = x.CanSeeDetail,
                        CanManagePermission = x.CanManagePermission,
                        TemplateCode = x.FolderCode,
                        CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false,
                        Sequence = x.SequenceNo,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.LastUpdatedDate,
                        NoteNo = x.DocumentNo,
                        CreatedBy = x.CreatedByUser,
                        WorkflowTemplateCode = x.WorkflowCode,
                        DocumentApprovalStatusType = x.DocumentApprovalStatusType,
                        WorkflowServiceId = x.WorkflowServiceId,
                        StatusName = x.NoteStatus,
                        CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
                        CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
                        WorkflowServiceStatus = x.WorkflowServiceStatus,
                        IsSelfWorkspace = x.IsSelfWorkspace,
                    }));
                }

                list = list.OrderBy(x => x.Sequence).ToList();
                var json = JsonConvert.SerializeObject(list);
                return json;
            }

            catch (Exception)
            {

                throw;
            }

        }
        [Authorize]
        public async Task<object> GetChildFolders(string key)
        {
            try
            {
                var list = new List<FileExplorerViewModel>();
                var workspaces = await _documentBusiness.GetAllChildWorkspaceAndFolder(_userContext.UserId, key);
                list.AddRange(workspaces.Select(x => new FileExplorerViewModel
                {
                    key = x.Id,
                    title = x.Name,
                    lazy = true,
                    folder = (x.FolderCode == "GENERAL_FOLDER"),
                    Workspace = (x.FolderCode == "WORKSPACE_GENERAL"),
                    FolderType = x.FolderType.Value,
                    Count = x.DocCount.ToString(),
                    WorkspaceId = x.WorkspaceId,
                    parentId = x.ParentId,
                    CanOpen = x.CanOpen,
                    ShowMenu = x.ShowMenu,
                    CanCreateSubFolder = x.CanCreateSubFolder,
                    CanRename = x.CanRename,
                    CanShare = x.CanShare,
                    CanMove = x.CanMove,
                    CanCopy = x.CanCopy,
                    CanArchive = x.CanArchive,
                    CanDelete = x.CanDelete,
                    CanSeeDetail = x.CanSeeDetail,
                    CanManagePermission = x.CanManagePermission,
                    TemplateCode = x.FolderCode,
                    CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false,
                    Sequence = x.SequenceNo,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.LastUpdatedDate,
                    NoteNo = x.DocumentNo,
                    CreatedBy = x.CreatedByUser,
                    WorkflowTemplateCode = x.WorkflowCode,
                    DocumentApprovalStatusType = x.DocumentApprovalStatusType,
                    WorkflowServiceId = x.WorkflowServiceId,
                    StatusName = x.NoteStatus,
                    CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
                    CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
                    WorkflowServiceStatus = x.WorkflowServiceStatus,
                    IsSelfWorkspace = x.IsSelfWorkspace,
                }));
                list = list.OrderBy(x => x.Sequence).ToList();
                var json = JsonConvert.SerializeObject(list);
                return json;
            }

            catch (Exception)
            {

                throw;
            }

        }
        [Authorize]
        public async Task<object> GetChildFoldersAndDocuments(string key, string activeId, string viewMode)
        {
            try
            {
                var list = new List<FileExplorerViewModel>();
                var workspaces = viewMode != "file" ? await _documentBusiness.GetAllChildWorkspaceFolderAndDocument(_userContext.UserId, key) : await _documentBusiness.GetAllChildWorkspaceFolderAndFiles(_userContext.UserId, key);
                //list.AddRange(workspaces.Select(x => new FileExplorerViewModel { key = x.Id, title = x.Name, lazy = true, folder = (x.FolderCode == "GENERAL_FOLDER"), Workspace = (x.FolderCode == "WORKSPACE_GENERAL"), Document = (x.FolderCode != "GENERAL_FOLDER" && x.FolderCode != "WORKSPACE_GENERAL"), NoteNo=x.DocumentNo,CreatedDate=x.CreatedDate,UpdatedDate=x.LastUpdatedDate,CreatedBy=x.CreatedByUser,WorkflowServiceStatusName=x.WorkflowServiceStatusName ,Count = x.DocCount.ToString(), Sequence = x.SequenceNo }));
                list.AddRange(workspaces.Select(x => new FileExplorerViewModel
                {
                    key = x.Id,
                    title = x.Name,
                    lazy = true,
                    active = (x.FolderCode != "GENERAL_FOLDER" && x.FolderCode != "WORKSPACE_GENERAL") ? x.Id == activeId : false,
                    folder = (x.FolderCode == "GENERAL_FOLDER"),
                    Workspace = (x.FolderCode == "WORKSPACE_GENERAL"),
                    Document = (x.FolderCode != "GENERAL_FOLDER" && x.FolderCode != "WORKSPACE_GENERAL" && x.FolderType != FolderTypeEnum.File),
                    File = x.FolderType == FolderTypeEnum.File,
                    FileId = x.DocumentId,
                    SnapshotId=x.SnapshotMongoId,
                    FileSize = x.FileSize != 0 ? Helper.ByteSizeWithSuffix(x.FileSize) : "",
                    FolderType = x.FolderType.Value,
                    Count = x.DocCount.ToString(),
                    WorkspaceId = x.WorkspaceId,
                    parentId = x.ParentId,
                    CanOpen = x.CanOpen,
                    ShowMenu = x.ShowMenu,
                    CanCreateSubFolder = x.CanCreateSubFolder,
                    CanRename = x.CanRename,
                    CanShare = x.CanShare,
                    CanMove = x.CanMove,
                    CanCopy = x.CanCopy,
                    CanArchive = x.CanArchive,
                    CanDelete = x.CanDelete,
                    CanSeeDetail = x.CanSeeDetail,
                    CanManagePermission = x.CanManagePermission,
                    TemplateCode = x.FolderCode,
                    CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false,
                    Sequence = x.SequenceNo,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.LastUpdatedDate,
                    NoteNo = x.DocumentNo,
                    CreatedBy = x.CreatedByUser,
                    WorkflowTemplateCode = x.WorkflowCode,
                    DocumentApprovalStatusType = x.DocumentApprovalStatusType,
                    WorkflowServiceId = x.WorkflowServiceId,
                    StatusName = x.NoteStatus,
                    CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
                    CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
                    WorkflowServiceStatus = x.WorkflowServiceStatus,
                    WorkflowServiceStatusName = x.WorkflowServiceStatusName,
                    IsSelfWorkspace = x.IsSelfWorkspace,
                }));
                list = list.OrderBy(x => x.Sequence).ToList();
                var json = JsonConvert.SerializeObject(list);
                return json;
            }

            catch (Exception)
            {

                throw;
            }

        }
        [Authorize]
        public async Task<object> GetChildFoldersAndFiles(string key, string activeId)
        {
            try
            {
                var list = new List<FileExplorerViewModel>();
                var workspaces = await _documentBusiness.GetAllChildWorkspaceFolderAndFiles(_userContext.UserId, key);
                //list.AddRange(workspaces.Select(x => new FileExplorerViewModel { key = x.Id, title = x.Name, lazy = true, folder = (x.FolderCode == "GENERAL_FOLDER"), Workspace = (x.FolderCode == "WORKSPACE_GENERAL"), Document = (x.FolderCode != "GENERAL_FOLDER" && x.FolderCode != "WORKSPACE_GENERAL"), NoteNo=x.DocumentNo,CreatedDate=x.CreatedDate,UpdatedDate=x.LastUpdatedDate,CreatedBy=x.CreatedByUser,WorkflowServiceStatusName=x.WorkflowServiceStatusName ,Count = x.DocCount.ToString(), Sequence = x.SequenceNo }));
                list.AddRange(workspaces.Select(x => new FileExplorerViewModel
                {
                    key = x.Id,
                    title = x.Name,
                    lazy = true,
                    active = (x.FolderCode != "GENERAL_FOLDER" && x.FolderCode != "WORKSPACE_GENERAL") ? x.Id == activeId : false,
                    folder = (x.FolderCode == "GENERAL_FOLDER"),
                    Workspace = (x.FolderCode == "WORKSPACE_GENERAL"),
                    Document = false,
                    File = x.FolderType == FolderTypeEnum.File,
                    FileId = x.DocumentId,
                    FileSize = x.FileSize != 0 ? Helper.ByteSizeWithSuffix(x.FileSize) : "",
                    FolderType = x.FolderType.Value,
                    Count = x.DocCount.ToString(),
                    WorkspaceId = x.WorkspaceId,
                    parentId = x.ParentId,
                    CanOpen = x.CanOpen,
                    ShowMenu = x.ShowMenu,
                    CanCreateSubFolder = x.CanCreateSubFolder,
                    CanRename = x.CanRename,
                    CanShare = x.CanShare,
                    CanMove = x.CanMove,
                    CanCopy = x.CanCopy,
                    CanArchive = x.CanArchive,
                    CanDelete = x.CanDelete,
                    CanSeeDetail = x.CanSeeDetail,
                    CanManagePermission = x.CanManagePermission,
                    TemplateCode = x.FolderCode,
                    CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false,
                    Sequence = x.SequenceNo,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.LastUpdatedDate,
                    NoteNo = x.DocumentNo,
                    CreatedBy = x.CreatedByUser,
                    WorkflowTemplateCode = x.WorkflowCode,
                    DocumentApprovalStatusType = x.DocumentApprovalStatusType,
                    WorkflowServiceId = x.WorkflowServiceId,
                    StatusName = x.NoteStatus,
                    CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
                    CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
                    WorkflowServiceStatus = x.WorkflowServiceStatus,
                    WorkflowServiceStatusName = x.WorkflowServiceStatusName,
                    IsSelfWorkspace = x.IsSelfWorkspace,
                }));
                list = list.OrderBy(x => x.Sequence).ToList();
                var json = JsonConvert.SerializeObject(list);
                return json;
            }

            catch (Exception)
            {

                throw;
            }

        }
        private async Task<List<FileExplorerViewModel>> GetParentNoteId(NoteViewModel key, NoteViewModel model, List<FileExplorerViewModel> list, List<FileExplorerViewModel> children1)
        {
            var children = new List<FileExplorerViewModel>();
            var note = await _noteBusiness.GetSingle(x => x.Id == model.ParentNoteId);
            if (note.IsNotNull())
            {
                if (note.TemplateCode == "GENERAL_FOLDER" || note.TemplateCode == "WORKSPACE_GENERAL")
                {
                    var clist = await _documentBusiness.GetAllChildWorkspaceAndFolder(_userContext.UserId, note.Id);
                    children.AddRange(clist.Select(x => new FileExplorerViewModel
                    {
                        key = x.Id,
                        title = x.Name,
                        lazy = true,
                        active = (key.TemplateCode == "GENERAL_FOLDER" || key.TemplateCode == "WORKSPACE_GENERAL") ? key.Id == x.Id : key.ParentNoteId == x.Id,
                        expanded = x.Id == model.Id,
                        children = x.Id == model.Id ? children1 : null,
                        folder = (x.FolderCode == "GENERAL_FOLDER"),
                        Workspace = (x.FolderCode == "WORKSPACE_GENERAL"),
                        FolderType = x.FolderType.Value,
                        Count = x.DocCount.ToString(),
                        WorkspaceId = x.WorkspaceId,
                        parentId = x.ParentId,
                        CanOpen = x.CanOpen,
                        ShowMenu = x.ShowMenu,
                        CanCreateSubFolder = x.CanCreateSubFolder,
                        CanRename = x.CanRename,
                        CanShare = x.CanShare,
                        CanMove = x.CanMove,
                        CanCopy = x.CanCopy,
                        CanArchive = x.CanArchive,
                        CanDelete = x.CanDelete,
                        CanSeeDetail = x.CanSeeDetail,
                        CanManagePermission = x.CanManagePermission,
                        TemplateCode = x.FolderCode,
                        CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false,
                        Sequence = x.SequenceNo,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.LastUpdatedDate,
                        NoteNo = x.DocumentNo,
                        CreatedBy = x.CreatedByUser,
                        WorkflowTemplateCode = x.WorkflowCode,
                        DocumentApprovalStatusType = x.DocumentApprovalStatusType,
                        WorkflowServiceId = x.WorkflowServiceId,
                        StatusName = x.NoteStatus,
                        CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
                        CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
                        WorkflowServiceStatus = x.WorkflowServiceStatus,
                        IsSelfWorkspace = x.IsSelfWorkspace,
                    }));
                    children = children.OrderBy(x => x.Sequence).ToList();
                }
                else
                {
                    var clist = await _documentBusiness.GetAllChildWorkspaceFolderAndDocument(_userContext.UserId, note.Id);
                    children.AddRange(clist.Select(x => new FileExplorerViewModel
                    {
                        key = x.Id,
                        title = x.Name,
                        lazy = true,
                        active = (key.TemplateCode == "GENERAL_FOLDER" || key.TemplateCode == "WORKSPACE_GENERAL") ? key.Id == x.Id : key.ParentNoteId == x.Id,
                        expanded = x.Id == model.Id,
                        children = x.Id == model.Id ? children1 : null,
                        folder = (x.FolderCode == "GENERAL_FOLDER"),
                        Workspace = (x.FolderCode == "WORKSPACE_GENERAL"),
                        Document = (x.FolderCode != "GENERAL_FOLDER" && x.FolderCode != "WORKSPACE_GENERAL" && x.FolderType != FolderTypeEnum.File),
                        File = x.FolderType == FolderTypeEnum.File,
                        FileId = x.DocumentId,
                        FileSize = x.FileSize != 0 ? Helper.ByteSizeWithSuffix(x.FileSize) : "",
                        FolderType = x.FolderType.Value,
                        Count = x.DocCount.ToString(),
                        WorkspaceId = x.WorkspaceId,
                        parentId = x.ParentId,
                        CanOpen = x.CanOpen,
                        ShowMenu = x.ShowMenu,
                        CanCreateSubFolder = x.CanCreateSubFolder,
                        CanRename = x.CanRename,
                        CanShare = x.CanShare,
                        CanMove = x.CanMove,
                        CanCopy = x.CanCopy,
                        CanArchive = x.CanArchive,
                        CanDelete = x.CanDelete,
                        CanSeeDetail = x.CanSeeDetail,
                        CanManagePermission = x.CanManagePermission,
                        TemplateCode = x.FolderCode,
                        CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false,
                        Sequence = x.SequenceNo,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.LastUpdatedDate,
                        NoteNo = x.DocumentNo,
                        CreatedBy = x.CreatedByUser,
                        WorkflowTemplateCode = x.WorkflowCode,
                        DocumentApprovalStatusType = x.DocumentApprovalStatusType,
                        WorkflowServiceId = x.WorkflowServiceId,
                        StatusName = x.NoteStatus,
                        CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
                        CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
                        WorkflowServiceStatus = x.WorkflowServiceStatus,
                        WorkflowServiceStatusName = x.WorkflowServiceStatusName,
                        IsSelfWorkspace = x.IsSelfWorkspace,
                    }));
                    children = children.OrderBy(x => x.Sequence).ToList();
                }
                if (note.ParentNoteId.IsNotNullAndNotEmpty())
                {
                    return await GetParentNoteId(key, note, list, children);
                }
                else
                {
                    var workspaces = await _documentBusiness.GetFirstLevelWorkspacesByUser(_userContext.UserId);
                    list.AddRange(workspaces.Select(x => new FileExplorerViewModel
                    {
                        key = x.Id,
                        title = x.Name,
                        lazy = true,
                        expanded = x.Id == note.Id,
                        children = x.Id == note.Id ? children : null,
                        folder = (x.FolderCode == "GENERAL_FOLDER"),
                        Workspace = (x.FolderCode == "WORKSPACE_GENERAL"),
                        FolderType = x.FolderType.Value,
                        Count = x.DocCount.ToString(),
                        WorkspaceId = x.WorkspaceId,
                        parentId = x.ParentId,
                        ParentId = x.ParentId,
                        CanOpen = x.CanOpen,
                        ShowMenu = x.ShowMenu,
                        CanCreateSubFolder = x.CanCreateSubFolder,
                        CanRename = false,
                        CanShare = x.CanShare,
                        CanMove = x.CanMove,
                        CanCopy = x.CanCopy,
                        CanArchive = x.CanArchive,
                        CanDelete = x.CanDelete,
                        CanSeeDetail = x.CanSeeDetail,
                        CanManagePermission = x.CanManagePermission,
                        TemplateCode = x.FolderCode,
                        CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false,
                        Sequence = x.SequenceNo,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.LastUpdatedDate,
                        NoteNo = x.DocumentNo,
                        CreatedBy = x.CreatedByUser,
                        WorkflowTemplateCode = x.WorkflowCode,
                        DocumentApprovalStatusType = x.DocumentApprovalStatusType,
                        WorkflowServiceId = x.WorkflowServiceId,
                        StatusName = x.NoteStatus,
                        CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
                        CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
                        WorkflowServiceStatus = x.WorkflowServiceStatus,
                        IsSelfWorkspace = x.IsSelfWorkspace,
                    }));
                    return list;
                }

            }
            else
            {
                return list;
            }
        }
        [Authorize]
        public async Task<object> GetDocumentVersions(string key)
        {
            try
            {
                var list = new List<FileExplorerViewModel>();
                var versionsList = await _documentBusiness.GetDocumentVersions(key);
                var versionsGrpList = versionsList.GroupBy(x => x.VersionNo).ToList();
                foreach (var versionGrp in versionsGrpList)
                {
                    var x = versionGrp.Where(x => x.WorkflowServiceId.IsNotNullAndNotEmpty()).Any() == true ? versionGrp.Where(x => x.WorkflowServiceId.IsNotNullAndNotEmpty()).OrderByDescending(x => x.LastUpdatedDate).FirstOrDefault() : versionGrp.OrderByDescending(x => x.LastUpdatedDate).FirstOrDefault();
                    list.Add(new FileExplorerViewModel
                    {
                        key = x.Id,
                        title = x.Name + " (Ver " + x.VersionNo + ")",
                        lazy = false,
                        active = false,
                        folder = false,
                        Workspace = false,
                        Document = false,
                        VersionDocument = true,
                        VersionNo = x.VersionNo,
                        File = false,
                        FileId = x.DocumentId,
                        SnapshotId = x.SnapshotMongoId,
                        FileSize = x.FileSize != 0 ? Helper.ByteSizeWithSuffix(x.FileSize) : "",
                        FolderType = FolderTypeEnum.Version,
                        Count = "0",
                        WorkspaceId = x.WorkspaceId,
                        parentId = x.ParentId,
                        CanOpen = x.CanOpen,
                        ShowMenu = x.ShowMenu,
                        CanCreateSubFolder = x.CanCreateSubFolder,
                        CanRename = x.CanRename,
                        CanShare = x.CanShare,
                        CanMove = x.CanMove,
                        CanCopy = x.CanCopy,
                        CanArchive = x.CanArchive,
                        CanDelete = x.CanDelete,
                        CanSeeDetail = x.CanSeeDetail,
                        CanManagePermission = x.CanManagePermission,
                        TemplateCode = x.TemplateMasterCode,
                        CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false,
                        Sequence = x.SequenceNo,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.LastUpdatedDate,
                        NoteNo = x.DocumentNo,
                        CreatedBy = x.CreatedByUser,
                        WorkflowTemplateCode = x.WorkflowCode,
                        DocumentApprovalStatusType = x.DocumentApprovalStatusType,
                        WorkflowServiceId = x.WorkflowServiceId,
                        StatusName = x.NoteStatus,
                        CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
                        CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
                        WorkflowServiceStatus = x.WorkflowServiceStatus,
                        WorkflowServiceStatusName = x.WorkflowServiceStatusName,
                        IsSelfWorkspace = x.IsSelfWorkspace,
                    });
                }                
                list = list.OrderByDescending(x => x.VersionNo).ToList();
                var json = JsonConvert.SerializeObject(list);
                return json;
            }

            catch (Exception)
            {

                throw;
            }

        }
        public async Task<bool> Rename(string name, string noteid)
        {
            var folderName = name;
            var templateModel = new NoteTemplateViewModel();
            templateModel.ActiveUserId = _userContext.UserId;
            templateModel.DataAction = DataActionEnum.Edit;
            templateModel.NoteId = noteid;
            var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
            newmodel.NoteSubject = folderName;
            newmodel.DataAction = DataActionEnum.Edit;
            newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            newmodel.OwnerUserId = _userContext.UserId;
            var result = await _noteBusiness.ManageNote(newmodel);
            if (result.IsSuccess)
            {
                return true;
            }
            return false;
        }
        public async Task<ActionResult> TusFileUpload()
        {
            return View();
        }
        public async Task<IActionResult> DownloadAll(string noteid, string name)
        {
            var list = await _documentBusiness.GetAllChildFiles(_userContext.UserId, noteid);
            CodePagesEncodingProvider.Instance.GetEncoding(437);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            MemoryStream outputMemStream = new MemoryStream();
            using (ZipOutputStream zipStream = new ZipOutputStream(outputMemStream))
            {
                zipStream.SetLevel(3);
                foreach (var item in list)
                {
                    byte[] fileBytes = await _fileBusiness.GetFileByte(item.DocumentId);
                    if (fileBytes != null && fileBytes.Length > 0)
                    {
                        var fileEntry = new ZipEntry(item.Name)
                        {
                            Size = fileBytes.Length,
                            DateTime = DateTime.Now
                        };
                        zipStream.PutNextEntry(fileEntry);
                        zipStream.Write(fileBytes, 0, fileBytes.Length);
                    }

                }
                zipStream.Flush();
                zipStream.Close();
            }
            return File(outputMemStream.ToArray(), "application/octet-stream", name + ".zip");
        }
        public async Task<bool> ResetInheritanceForDocuments()
        {
            try
            {
                var documents = await _documentBusiness.GetAllDocuments();
                var docids = documents.Select(x => x.Id);
                var noteids = string.Join(",", docids);
                var permissionlist = await _documentBusiness.GetAllNotePermissionByParentId(noteids);
                if (permissionlist.Any())
                {
                    var permissionlisttobedeleted = permissionlist.Where(x => x.IsInherited == true).ToList();
                    if (permissionlisttobedeleted.Any())
                    {
                        var perIds = permissionlisttobedeleted.Select(x => x.Id);
                        var ids = "'" + String.Join("','", perIds) + "'";
                        var res = await _documentPermissionBusiness.DeletePermissionByDocumentIds(ids);
                    }

                }
                var allPermision = new List<DocumentPermissionViewModel>();
                foreach (var doc in documents)
                {
                    var parentPermissions = await _documentPermissionBusiness.GetNotePermissionData(doc.ParentId);
                    var notePermissions = await _documentPermissionBusiness.GetNotePermissionData(doc.Id);
                    if (parentPermissions.IsNotNull() && parentPermissions.Count > 0)
                    {
                        // Create or Edit the Permission for the documents
                        foreach (var permission in parentPermissions.Where(x => x.IsInheritedFromChild == false))
                        {
                            if (permission.AppliesTo != DmsAppliesToEnum.OnlyThisFolder)
                            {
                                if (!notePermissions.Any(e => e.PermissionType == permission.PermissionType && e.Principal == permission.Principal && e.Access == permission.Access && e.AppliesTo == permission.AppliesTo))
                                {
                                    var existpermission = await _documentPermissionBusiness.GetSingleById(permission.Id);
                                    var permissionData = new DocumentPermissionViewModel
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        PermissionType = existpermission.PermissionType,
                                        Access = existpermission.Access,
                                        AppliesTo = existpermission.AppliesTo,
                                        IsInherited = true,
                                        IsInheritedFromChild = false,
                                        PermittedUserId = existpermission.PermittedUserId,
                                        PermittedUserGroupId = existpermission.PermittedUserGroupId,
                                        NoteId = doc.Id,
                                        InheritedFrom = existpermission.InheritedFrom == null ? existpermission.Id : existpermission.InheritedFrom,
                                        Isowner = existpermission.Isowner,
                                        DisablePermittedNotification = true,
                                        CompanyId = _userContext.CompanyId,
                                        IsDeleted = false,
                                        CreatedBy = _userContext.UserId,
                                        LastUpdatedBy = _userContext.UserId,
                                        CreatedDate = DateTime.Now,
                                        LastUpdatedDate = DateTime.Now,
                                        LegalEntityId = _userContext.LegalEntityId,
                                        PortalId = _userContext.PortalId,
                                        Status = StatusEnum.Active,
                                        VersionNo = 0,
                                    };
                                    allPermision.Add(permissionData);
                                }

                            }
                        }
                    }
                }
                if (allPermision.Count > 0)
                {
                    await _documentPermissionBusiness.CreateBulkPermission(allPermision);
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task<ActionResult> DocumentSearchResult(string serachstr, string parentId)
        {
            ViewBag.SearchStr = serachstr;
            ViewBag.ParentId = parentId;
            return View("_DocumentSearchResult");
        }
        public async Task<IActionResult> ReadDocumentSearchResult(string serachstr, string parentId)
        {

            var allChildFoldersAndDocuments = parentId.IsNotNullAndNotEmpty() ? await _documentBusiness.GetAllPermittedChildByParentId(_userContext.UserId, parentId) : await _documentBusiness.GetAllPermittedWorkspaceFolderAndDocument(_userContext.UserId);
            var ids = allChildFoldersAndDocuments.Select(x => x.Id).ToList();
            var noteids = string.Join(",", ids);
            noteids = noteids.IsNotNullAndNotEmpty() ? noteids.Replace(",", "\",\"") : null;
            var model = new List<DocumentSearchViewModel>();
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var url = eldbUrl + "dms_data/_search?pretty=true";
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            if (serachstr.IsNotNullAndNotEmpty())
            {
                var content = "";
                content = ApplicationConstant.Document.ReadDocumentQuery;
                content = content.Replace("#IDS#", noteids);
                content = content.Replace("#SEARCHWHERE#", serachstr);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient(handler))
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    if (data1.IsNotNull())
                    {
                        var data2 = data1.SelectToken("hits");
                        foreach (var item in data2)
                        {
                            var source = item.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var highlight = item.SelectToken("highlight");
                            var strhighlight = JsonConvert.SerializeObject(highlight);
                            var result = JsonConvert.DeserializeObject<DocumentSearchViewModel>(str);
                            var resultHighlight = JsonConvert.DeserializeObject<DocumentSearchArrayViewModel>(strhighlight);
                            if (result.IsNotNull())
                            {
                                result.NoteSubject = (resultHighlight.IsNotNull() && resultHighlight.notesubject != null) ? string.Join("", resultHighlight.notesubject) : result.NoteSubject;
                                result.NoteDescription = (resultHighlight.IsNotNull() && resultHighlight.notedescription != null) ? string.Join("", resultHighlight.notedescription) : result.NoteDescription;
                                result.NoteNo = (resultHighlight.IsNotNull() && resultHighlight.noteno != null) ? string.Join("", resultHighlight.noteno) : result.NoteNo;
                                result.FileName = (resultHighlight.IsNotNull() && resultHighlight.filename != null) ? string.Join("", resultHighlight.filename) : result.FileName;
                                result.FileExtractedText = (resultHighlight.IsNotNull() && resultHighlight.fileExtractedText != null) ? string.Join("", resultHighlight.fileExtractedText) : "";
                                result.FileExtension = (resultHighlight.IsNotNull() && resultHighlight.fileExtension != null) ? string.Join("", resultHighlight.fileExtension) : result.FileExtension;
                                model.Add(result);
                            }

                        }
                    }


                }
            }

            return Json(model);
        }
        //public async Task<bool> ConvertFileToPdfTest()
        //{
        //    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        //    {

        //        var argsPrepend = "/c ";
        //        var shellName = "cmd";
        //        var folder = Guid.NewGuid().ToString();
        //        var outfolder = Guid.NewGuid().ToString();
        //        string folderpath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), folder);
        //        string folderpathAll = System.IO.Path.Combine(System.IO.Path.GetTempPath(), folder, "*.*");
        //        string outfolderpath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), outfolder);
        //        bool exists = System.IO.Directory.Exists(folderpath);
        //        if (!exists)
        //            System.IO.Directory.CreateDirectory(folderpath);

        //        bool exists1 = System.IO.Directory.Exists(outfolderpath);
        //        if (!exists1)
        //            System.IO.Directory.CreateDirectory(outfolderpath);
        //        var list = await _fileBusiness.GetFileList();
        //        foreach (var item in list)
        //        {
        //            string filename = item.Id + item.FileExtension;
        //            string path = System.IO.Path.Combine(folderpath, filename);
        //            byte[] contentByte = await _fileBusiness.DownloadMongoFileByte(item.MongoFileId);
        //            using (System.IO.Stream file = System.IO.File.OpenWrite(path))
        //            {
        //                file.Write(contentByte, 0, contentByte.Length);
        //            }
        //        }
        //        try
        //        {
        //            ProcessStartInfo procStartInfo = new ProcessStartInfo();
        //            procStartInfo.FileName = shellName;
        //            procStartInfo.Arguments = "soffice.exe " + string.Format("--convert-to pdf --outdir {0} {1}", outfolderpath, folderpathAll);
        //            procStartInfo.RedirectStandardOutput = true;
        //            procStartInfo.UseShellExecute = false;
        //            procStartInfo.CreateNoWindow = true;
        //            procStartInfo.WorkingDirectory = Environment.CurrentDirectory;

        //            Process process = new Process() { StartInfo = procStartInfo, };
        //            process.Start();
        //            process.WaitForExit();

        //            // Check for failed exit code.
        //            if (process.ExitCode == 0)
        //            {
        //                foreach (var item in list)
        //                {
        //                    string newfileName = item.Id + ".pdf";
        //                    string newpath = System.IO.Path.Combine(outfolderpath, newfileName);
        //                    byte[] bytes = System.IO.File.ReadAllBytes(newpath);
        //                    var res = await _fileBusiness.UploadMongoFileByte(item, bytes);

        //                }
        //                if (Directory.Exists(folderpath))
        //                {
        //                    Directory.Delete(folderpath, true);
        //                }
        //                if (Directory.Exists(outfolderpath))
        //                {
        //                    Directory.Delete(outfolderpath, true);
        //                }

        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw;
        //        }

        //    }


        //    return true;


        //}

        public async Task<ActionResult> ResumableUpload()
        {
            return View();
        }
        public async Task<ActionResult> PopupResumableUpload()
        {
            return View();
        }
        public async Task<ActionResult> ResumableUploadUppy()
        {
            return View();
        }
        public async Task<ActionResult> DMSBook(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        public async Task<IActionResult> DmsBookBrowse(string documentNoteId, string templateCode, string documentId)
        {
            var data = new BookDetailViewModel();
            var noteViewModel = new NoteTemplateViewModel();
            noteViewModel.NoteId = documentNoteId;
            var result = await _noteBusiness.GetNoteDetails(noteViewModel);
            if (result.IsNotNull())
            {
                //Dictionary<string, object> rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Json);
                ////var attachmentValue = rowData.ContainsKey("attachment") ? rowData["attachment"].ToString() : rowData.ContainsKey("fileAttachment") ? rowData["fileAttachment"].ToString() : "";
                //var udfs = await _noteBusiness.GetUdfJsonModel(result.Json);
                //var value = udfs.Where(x => x.key == "attachment" || x.key == "fileAttachment").FirstOrDefault().columnMetadataId;
                var userDetails = await _userBusiness.GetSingleById(result.CreatedBy);
                data.BookDetails = new BookViewModel();
                data.BookDetails.BookImage = documentId;
                data.BookDetails.BookName = result.NoteSubject;
                data.BookDetails.BookDescription = result.NoteDescription;
                data.BookDetails.ServiceNo = result.NoteNo;
                data.BookDetails.CreatedBy = userDetails.IsNotNull() ? userDetails.Name : "";
                data.BookDetails.CreatedDate = result.CreatedDate;

            }
            return View(data);
        }


        public async Task<IActionResult> GetAllChildBook(string id, string search)
        {
            var data = await _documentBusiness.GetAllChildWorkspaceFolderAndDocument(_userContext.UserId, id);
            data = data.Where(x => x.FolderType == FolderTypeEnum.Document).ToList();
            data.ForEach(x => { x.Description.HtmlEncode(); x.DocCount = 1; x.LastUpdatedDate.ToDefaultDateFormat(); });
            //data.ForEach(x => x.DocCount=1);
            //data.ForEach(x => x.LastUpdatedDate.ToDefaultDateTimeFormat());
            if (search.IsNotNullAndNotEmpty())
            {
                data = data.Where(x => x.Name.Contains(search) || (x.Description.IsNotNullAndNotEmpty() ? x.Description.Contains(search) : false) || x.DocumentNo.Equals(search)).ToList();
            }
            return Json(data);
        }
        public ActionResult DMSBookListScrollView(string mode, string id)
        {
            // ViewBag.TemplateCodes = templateCodes;
            // ViewBag.Permissions = permissions;
            ViewBag.Id = id;
            ViewBag.Mode = mode;
            return View();
        }
        public async Task<ActionResult> DMSBookListHierarchyView(string mode, string id)
        {

            ViewBag.Id = id;
            ViewBag.Mode = mode;
            var date = DateTime.Now.Date;


            var rootNodes = await _userBusiness.GetUserHierarchyRootId(_userContext.UserId, "DMS_BOOK_HIERARCHY", _userContext.UserId);
            var hierarchyId = rootNodes.Item3;
            var HierarchyRootNodeId = rootNodes.Item1;
            var AllowedRootNodeId = rootNodes.Item2;
            // var lvl = await _hrCoreBusiness.GetUserNodeLevel(rootNodes.Item2, rootNodes.Item3);
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


            ViewBag.IsAsOnDate = date.ToYYYY_MM_DD_DateFormat();


            ViewBag.AsOnDateDisplay = date.ToYYYY_MM_DD_DateFormat();

            return View(viewModel);
        }
        public async Task<ActionResult> GetDMSBookChildList(string parentId, int levelUpto, string hierarchyId, string permissions)
        {
            var hierarchy = await _hierarchyBusiness.GetSingleById(hierarchyId);

            var list = new List<DMSBookHierarchyChartViewModel>();
            //var children = new List<DMSBookHierarchyChartViewModel>();

            if (parentId == "-1")
            {
                var workspaces = await _documentBusiness.GetFirstLevelWorkspacesByUser(_userContext.UserId);
                list.AddRange(workspaces.Select(x => new DMSBookHierarchyChartViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    key = x.Id,
                    title = x.Name,
                    lazy = true,
                    folder = (x.FolderCode == "GENERAL_FOLDER"),
                    Workspace = (x.FolderCode == "WORKSPACE_GENERAL"),
                    FolderType = x.FolderType.Value,
                    NodeType = x.FolderType.Value.ToString(),
                    Count = x.DocCount.ToString(),
                    WorkspaceId = x.WorkspaceId,
                    parentId = "-1",
                    ParentId = "-1",
                    CanOpen = x.CanOpen,
                    ShowMenu = x.ShowMenu,
                    CanCreateSubFolder = x.CanCreateSubFolder,
                    CanRename = x.CanRename,
                    CanShare = x.CanShare,
                    CanMove = x.CanMove,
                    CanCopy = x.CanCopy,
                    CanArchive = x.CanArchive,
                    CanDelete = x.CanDelete,
                    CanSeeDetail = x.CanSeeDetail,
                    CanManagePermission = x.CanManagePermission,
                    TemplateCode = x.FolderCode,
                    CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false,
                    Sequence = x.SequenceNo,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.LastUpdatedDate,
                    NoteNo = x.DocumentNo,
                    CreatedBy = x.CreatedByUser,
                    WorkflowTemplateCode = x.WorkflowCode,
                    DocumentApprovalStatusType = x.DocumentApprovalStatusType,
                    WorkflowServiceId = x.WorkflowServiceId,
                    StatusName = x.NoteStatus,
                    CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
                    CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
                    WorkflowServiceStatus = x.WorkflowServiceStatus,
                    IsSelfWorkspace = x.IsSelfWorkspace,
                    DirectChildCount = _documentBusiness.GetAllChildWorkspaceAndFolder(_userContext.UserId, x.Id).Result.Count
                }));

                var model = new DMSBookHierarchyChartViewModel()
                {
                    Id = "-1",
                    Name = hierarchy.Name,
                    title = hierarchy.Name,
                    DirectChildCount = workspaces.Count()
                };
                list.Insert(0, model);
            }
            else
            {
                if (parentId.IsNotNullAndNotEmpty())
                {
                    var note = await _noteBusiness.GetSingle(x => x.Id == parentId);
                    if (note.IsNotNull())
                    {
                        if (note.TemplateCode == "GENERAL_FOLDER" || note.TemplateCode == "WORKSPACE_GENERAL")
                        {
                            var clist = await _documentBusiness.GetAllChildWorkspaceFolderAndDocument(_userContext.UserId, parentId);
                            list.AddRange(clist.Select(x => new DMSBookHierarchyChartViewModel
                            {
                                Id = x.Id,
                                DocumentId = x.DocumentId,
                                Name = x.Name,
                                key = x.Id,
                                title = x.Name,
                                lazy = true,
                                folder = (x.FolderCode == "GENERAL_FOLDER"),
                                Workspace = (x.FolderCode == "WORKSPACE_GENERAL"),
                                FolderType = x.FolderType.Value,
                                NodeType = x.FolderType.Value.ToString(),
                                Count = x.DocCount.ToString(),
                                WorkspaceId = x.WorkspaceId,
                                parentId = x.ParentId,
                                ParentId = x.ParentId,
                                CanOpen = x.CanOpen,
                                ShowMenu = x.ShowMenu,
                                CanCreateSubFolder = x.CanCreateSubFolder,
                                CanRename = x.CanRename,
                                CanShare = x.CanShare,
                                CanMove = x.CanMove,
                                CanCopy = x.CanCopy,
                                CanArchive = x.CanArchive,
                                CanDelete = x.CanDelete,
                                CanSeeDetail = x.CanSeeDetail,
                                CanManagePermission = x.CanManagePermission,
                                TemplateCode = x.FolderCode,
                                CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false,
                                Sequence = x.SequenceNo,
                                CreatedDate = x.CreatedDate,
                                UpdatedDate = x.LastUpdatedDate,
                                NoteNo = x.DocumentNo,
                                CreatedBy = x.CreatedByUser,
                                WorkflowTemplateCode = x.WorkflowCode,
                                DocumentApprovalStatusType = x.DocumentApprovalStatusType,
                                WorkflowServiceId = x.WorkflowServiceId,
                                StatusName = x.NoteStatus,
                                CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
                                CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
                                WorkflowServiceStatus = x.WorkflowServiceStatus,
                                IsSelfWorkspace = x.IsSelfWorkspace,
                                DirectChildCount = _documentBusiness.GetAllChildWorkspaceFolderAndDocument(_userContext.UserId, x.Id).Result.Count
                            }));
                            //children = children.OrderBy(x => x.Sequence).ToList();
                        }
                        else
                        {
                            var clist = await _documentBusiness.GetAllChildWorkspaceFolderAndDocument(_userContext.UserId, parentId);
                            list.AddRange(clist.Select(x => new DMSBookHierarchyChartViewModel
                            {
                                Id = x.Id,
                                key = x.Id,
                                title = x.Name,
                                Name = x.Name,
                                lazy = true,
                                folder = (x.FolderCode == "GENERAL_FOLDER"),
                                Workspace = (x.FolderCode == "WORKSPACE_GENERAL"),
                                Document = (x.FolderCode != "GENERAL_FOLDER" && x.FolderCode != "WORKSPACE_GENERAL" && x.FolderType != FolderTypeEnum.File),
                                File = x.FolderType == FolderTypeEnum.File,
                                FileId = x.DocumentId,
                                FileSize = x.FileSize != 0 ? Helper.ByteSizeWithSuffix(x.FileSize) : "",
                                //FolderType = x.FolderCode == "WORKSPACE_GENERAL" ? FolderTypeEnum.Workspace : x.FolderCode == "GENERAL_FOLDER" ? FolderTypeEnum.Folder : FolderTypeEnum.Document,
                                FolderType = x.FolderType.Value,
                                NodeType = x.FolderType.Value.ToString(),
                                Count = x.DocCount.ToString(),
                                WorkspaceId = x.WorkspaceId,
                                parentId = x.ParentId,
                                ParentId = x.ParentId,
                                CanOpen = x.CanOpen,
                                ShowMenu = x.ShowMenu,
                                CanCreateSubFolder = x.CanCreateSubFolder,
                                CanRename = x.CanRename,
                                CanShare = x.CanShare,
                                CanMove = x.CanMove,
                                CanCopy = x.CanCopy,
                                CanArchive = x.CanArchive,
                                CanDelete = x.CanDelete,
                                CanSeeDetail = x.CanSeeDetail,
                                CanManagePermission = x.CanManagePermission,
                                TemplateCode = x.FolderCode,
                                CanCreateWorkspace = (_userContext.IsSystemAdmin && !x.IsSelfWorkspace) ? true : false,
                                Sequence = x.SequenceNo,
                                CreatedDate = x.CreatedDate,
                                UpdatedDate = x.LastUpdatedDate,
                                NoteNo = x.DocumentNo,
                                CreatedBy = x.CreatedByUser,
                                WorkflowTemplateCode = x.WorkflowCode,
                                DocumentApprovalStatusType = x.DocumentApprovalStatusType,
                                WorkflowServiceId = x.WorkflowServiceId,
                                StatusName = x.NoteStatus,
                                CanEditDocument = (x.FolderCode == "GENERAL_FOLDER" || x.FolderCode == "WORKSPACE_GENERAL") ? x.CanRename : x.CanEditDocument,
                                CanCreateDocument = (x.FolderCode == "WORKSPACE_GENERAL") ? false : x.CanCreateDocument,
                                WorkflowServiceStatus = x.WorkflowServiceStatus,
                                WorkflowServiceStatusName = x.WorkflowServiceStatusName,
                                IsSelfWorkspace = x.IsSelfWorkspace,
                                DirectChildCount = x.DocCount
                            }));
                            // children = children.OrderBy(x => x.Sequence).ToList();
                        }
                        //    if (note.ParentNoteId.IsNotNullAndNotEmpty())
                        //    {
                        //        await GetParentNoteId(note, note, list, children);
                        //    }
                        //    else
                        //    {
                        //    var workspaces = await _documentBusiness.GetFirstLevelWorkspacesByUser(_userContext.UserId);
                        //    list.AddRange(workspaces.Select(x => new DMSBookHierarchyChartViewModel()
                        //    {
                        //        Id = x.Id,
                        //        ParentId = "-1",
                        //        Name = x.Name,
                        //        NodeType = "Workspace",
                        //        CreatedDate = x.CreatedDate,
                        //        DirectChildCount = x.DocCount
                        //    }));
                        //    var model = new UserHierarchyChartViewModel()
                        //    {
                        //        Id = "-1",
                        //        Name = "Workspaces",
                        //        DirectChildCount = workspaces.Count()
                        //    };
                        //    list.Insert(0, model);
                        //}
                    }

                }
            }
            var json = Json(list);
            return json;
        }
        public IActionResult DMSTagIndex()
        {
            return View();
        }
        public async Task<ActionResult> ReadDMSTagData()
        {
            var datalist = await _documentBusiness.GetDMSTagData();
            var j = Json(datalist);
            return j;
        }
        public async Task<ActionResult> GetDMSTagIdNameList()
        {
            var model = await _documentBusiness.GetDMSTagIdNameList();
            return Json(model);
        }
        public async Task<IActionResult> DMSTag(string tagId)
        {
            var model = new DMSTagViewModel();
            if (tagId.IsNotNullAndNotEmpty())
            {
                model = await _documentBusiness.GetDMSTagDetails(tagId);
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.TagBackgroundColor = "#ffa500";
                model.TagForegroundColor = "#000000";
            }
            return View("ManageDMSTag", model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageDMSTag(DMSTagViewModel model)
        {

            if (model.DataAction == DataActionEnum.Create)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "DMS_TAGS";
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            else
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.NoteId;

                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
        }
        public async Task<JsonResult> DeleteDMSTag(string tagid)
        {
            await _documentBusiness.DeleteDMSTag(tagid);
            return Json(new { success = true });
        }
        public async Task<bool> CreateBulkDocument(string batchId)
        {
            var list = new List<NoteTemplateViewModel>();
            var staginglist = await _stagingBusiness.GetList(x => x.BatchId == batchId && x.StageStatus == NtsStagingEnum.Inprogress);
            foreach (var item in staginglist)
            {
                var file = await _fileBusiness.GetSingleById(item.FileId);
                var templateCode = "GENERAL_DOCUMENT";
                if (item.TemplateId.IsNotNullAndNotEmpty())
                {
                    var template = await _templateBusiness.GetSingleById(item.TemplateId);
                    templateCode = template.Code;
                }
                var model = new NoteTemplateViewModel
                {
                    TemplateCode = templateCode,
                    DataAction = DataActionEnum.Create,
                    OwnerUserId = item.UserId,
                    ActiveUserId = item.UserId,
                    RequestedByUserId = item.UserId,
                    ParentNoteId = item.ReferenceId,
                    StartDate = DateTime.Now.ApplicationNow().Date,

                };
                var newmodel = await _noteBusiness.GetNoteDetails(model);
                newmodel.NoteSubject = file.FileName;
                newmodel.Description = file.FileName;
                newmodel.NoteStatusCode = "NOTE_STATUS_DRAFT";
                newmodel.Json = "{}";
                dynamic exo = new System.Dynamic.ExpandoObject();
                if (item.FileId.IsNotNullAndNotEmpty())
                {
                    if (templateCode == "GENERAL_DOCUMENT" || templateCode == "ENGINEERING_SUBCONTRACT")
                    {
                        ((IDictionary<String, Object>)exo).Add("fileAttachment", item.FileId);
                    }
                    else
                    {
                        ((IDictionary<String, Object>)exo).Add("attachment", item.FileId);
                    }

                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                }
                list.Add(newmodel);
            }
            var result = await _noteBusiness.ManageBulkNote(list);
            if (result.IsSuccess)
            {
                return await _stagingBusiness.UpdateStagingByBatchId(batchId);

            }
            return false;
        }
        public async Task<bool> IncrementalDMSMigration()
        {
            try
            {
                
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var query = ApplicationConstant.BusinessAnalytics.MaxDateQuery;
                query = query.Replace("#FILTERCOLUMN#", "lastUpdatedDate");
                var queryContent = new StringContent(query, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + "dms_data/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, queryContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var _jsondata = await response.Content.ReadAsStringAsync();
                        var _dataToken = JToken.Parse(_jsondata);
                        var _responsedata = _dataToken.SelectToken("aggregations");
                        var _maxdateToken = _responsedata.SelectToken("max_date");
                        var _dateToken = _maxdateToken.Last();
                        var _date = _dateToken.Last();
                        var fromDate = _date.Value<DateTime>();
                        var list = await _documentBusiness.GetAllWorkspaceFolderDocuments(fromDate);
                        BulkDescriptor descriptor = new BulkDescriptor();
                        foreach (var item in list)
                        {
                            item.NoteSubject = Path.GetFileNameWithoutExtension(item.NoteSubject);
                            var id = item.Id;
                            descriptor.Index<object>(i => i
                                   .Index("dms_data")
                                   .Id((Id)id)
                                   .Document(item));
                        }
                        var bulkResponse = client.Bulk(descriptor);
                        if (bulkResponse.IsValid)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        var list = await _documentBusiness.GetAllWorkspaceFolderDocuments(null);
                        BulkDescriptor descriptor = new BulkDescriptor();
                        foreach (var item in list)
                        {
                            item.NoteSubject = Path.GetFileNameWithoutExtension(item.NoteSubject);
                            var id = item.Id;
                                    descriptor.Index<object>(i => i
                                           .Index("dms_data")
                                           .Id((Id)id)
                                           .Document(item));
                        }
                        var bulkResponse = client.Bulk(descriptor);
                        if (bulkResponse.IsValid)
                        {
                            return true;
                        }
                    }


                }
                return false;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return false;
            }

        }
        public async Task<IActionResult> Dashboard()
        {    
            ViewBag.DocumentTypeCount=await _documentBusiness.GetDocumentTypeCount();   
            ViewBag.DocumentCount=await _documentBusiness.GetDocumentCount();   
            return View();
        }
        public async Task<JsonResult> GetRecentDocuments()
        {
            var list = await _documentBusiness.GetTopRecentDocuments(_userContext.UserId);
            return Json(list);
        }
        public async Task<JsonResult> GetTopPendingDocuments()
        {
            var list = await _documentBusiness.GetTopPendingDocuments(_userContext.UserId);
            return Json(list);
        }
        public async Task<JsonResult> GetAllDocumentSummary()
        {
            var list = await _documentBusiness.GetAllDocumentSummary(_userContext.UserId);
            return Json(list);
        }
        public async Task<JsonResult> GetAllDocumentAnalysis()
        {
            var list = await _documentBusiness.GetAllDocumentAnalysis(_userContext.UserId);
            return Json(list);
        }
        public async Task<JsonResult> GetRecentActivities()
        {
            var list = await _documentBusiness.GetTopRecentActivities(_userContext.UserId);
            return Json(list);
        }
        //public async Task<string> TestUdf()
        //{
        //    var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
        //    var list = await _documentBusiness.GetAllDmsDocumentsWithUdf();
        //    var handler = new HttpClientHandler();
        //    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        //    handler.ServerCertificateCustomValidationCallback =
        //        (httpRequestMessage, cert, cetChain, policyErrors) =>
        //        {
        //            return true;
        //        };
        //    using (var httpClient = new HttpClient(handler))
        //    {
        //        foreach (var item in list)
        //        {
        //            if (item.FileExtension.ToLower() == ".pdf" || item.MongoPreviewFileId.IsNotNullAndNotEmpty())
        //            {
        //                byte[] contentByte = await _fileBusiness.DownloadMongoFileByte(item.MongoPreviewFileId.IsNotNullAndNotEmpty() ? item.MongoPreviewFileId : item.MongoFileId);
        //                if (contentByte.Length > 0)
        //                {
        //                    using (var document = UglyToad.PdfPig.PdfDocument.Open(contentByte))
        //                    {
        //                        var pages = document.GetPages();
        //                        foreach (var page in pages)
        //                        {
        //                            item.FileExtractedText += string.Join(" ", page.GetWords());
        //                        }

        //                    }
        //                    byte[] pngByte = new byte[0];  //= Freeware.Pdf2Png.Convert(contentByte, 1); 
        //                    //MagickReadSettings settings = new MagickReadSettings();
        //                    //// Settings the density to 300 dpi will create an image with a better quality
        //                    //settings.Density = new Density(300);

        //                    //using (MagickImageCollection images = new MagickImageCollection())
        //                    //{
        //                    //    // Add all the pages of the pdf file to the collection
        //                    //    images.Read(contentByte);                                
        //                    //    foreach (MagickImage image in images)
        //                    //    {
        //                    //        string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), item.NoteSubject, ".png");
        //                    //        image.Format = MagickFormat.Png;
        //                    //        // Write page to file that contains the page number
        //                    //        image.Write(path);
        //                    //        pngByte = System.IO.File.ReadAllBytes(path);
        //                    //        if (System.IO.File.Exists(path))
        //                    //        {
        //                    //            System.IO.File.Delete(path);
        //                    //        }
        //                    //        break;

        //                    //    }
        //                    //}
        //                    using (var rasterizer = new GhostscriptRasterizer()) //create an instance for GhostscriptRasterizer
        //                    {

        //                        //string fileName = Path.GetFileNameWithoutExtension(inputFile);
        //                        Stream stream = new MemoryStream(contentByte);
        //                        rasterizer.Open(stream); //opens the PDF file for rasterizing 
        //                        string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), item.NoteSubject, ".png");
        //                        var pdf2PNG = rasterizer.GetPage(100,100, 1);
        //                        pdf2PNG.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        //                        pngByte = System.IO.File.ReadAllBytes(path);
        //                        if (System.IO.File.Exists(path))
        //                        {
        //                            System.IO.File.Delete(path);
        //                        }
        //                    }
        //                    var file = await _fileBusiness.GetSingleById(item.FileId);
        //                    if (file.IsNotNull())
        //                    {
        //                        file.FileExtractedText = item.FileExtractedText;
        //                        await _fileBusiness.UploadMongoSnapshotFile(file, pngByte);
        //                        var fileName = Path.GetFileNameWithoutExtension(item.FileName);
        //                        var fileExtension = Path.GetExtension(item.FileName);
        //                        var content2 = @"{""size"":10, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":[""#IDS#""]}}]}},""script"": { ""source"": ""ctx._source['fileId'] ='#FileId#';ctx._source['fileName'] ='#FileName#';ctx._source['fileExtension'] ='#FileExtension#';ctx._source['fileExtractedText'] ='#FileExtractedText#';""} }";
        //                        content2 = content2.Replace("#IDS#", item.Id);
        //                        content2 = content2.Replace("#FileId#", item.FileId);
        //                        content2 = content2.Replace("#FileName#", fileName);
        //                        content2 = content2.Replace("#FileExtension#", fileExtension);
        //                        content2 = content2.Replace("#FileExtractedText#", item.FileExtractedText.Replace("\"", "").Replace("'", ""));
        //                        var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
        //                        var url2 = eldbUrl + "dms_data/_update_by_query";
        //                        var address2 = new Uri(url2);
        //                        var response2 = await httpClient.PostAsync(address2, stringContent2);
        //                    }
        //                }


        //            }
        //        }
        //    }
        //    return "";
        //}

    }

}
