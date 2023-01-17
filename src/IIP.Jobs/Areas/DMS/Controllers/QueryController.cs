using Synergy.App.ViewModel;
using Synergy.App.Business;
using Synergy.App.Business.Interface.DMS;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Api.Areas.DMS.Models;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//Syncfusion.EJ2.FileManager.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.DMS.Controllers
{
    [Route("dms/query")]
    [ApiController]
    public class QueryController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDMSDocumentBusiness _documentBusiness;
        private readonly INoteBusiness _noteBusiness;



        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
         IServiceProvider serviceProvider, IDMSDocumentBusiness documentBusiness, INoteBusiness noteBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _documentBusiness = documentBusiness;
            _noteBusiness = noteBusiness;
        }



        [HttpPost]
        [Route("GetFiles")]
        public async Task<ActionResult> GetFiles(FileOperation args)
        {
            await Authenticate(args.UserId);
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _documentBusiness = _serviceProvider.GetService<IDMSDocumentBusiness>();
            var _userContext = _serviceProvider.GetService<IUserContext>();
            FileResponse readResponse = new FileResponse();
            try
            {

                var pathIds = args.Path.Split('/');
                if (args.Data.Count() == 0 || pathIds.Count() == 2)
                {
                    var user = new DirectoryContent { id = _userContext.UserId, name = _userContext.Name, hasChild = true, FolderType = FolderTypeEnum.Root };
                    // var workspaces = await _noteBusiness.GetList(x => x.TemplateCode == "WORKSPACE_GENERAL" & x.ParentNoteId == null);
                    var workspaces = await _documentBusiness.GetUserWorkspaceTreeDataNew(_userContext.UserId, null);
                    var workspaces1 = workspaces.Where(x => x.FolderCode == "WORKSPACE_GENERAL" && x.ParentId == null).ToList();
                    foreach (var ws in workspaces1)
                    {
                        List<FolderViewModel> childlist = new List<FolderViewModel>();
                        var childs = await _documentBusiness.GetAllChildDocumentByParentId(ws.Id, childlist);
                        ws.DocCount = childs.Where(x => x.TemaplateMasterCatCode == "GENERAL_DOCUMENT").Count();
                    }
                    readResponse.cwd = JsonConvert.DeserializeObject<DirectoryContent>(JsonConvert.SerializeObject(user));
                    readResponse.files = JsonConvert.DeserializeObject<IEnumerable<DirectoryContent>>(JsonConvert.SerializeObject(workspaces1.Select(x => new DirectoryContent { id = x.Id, name = x.Name, hasChild = true, FolderType = FolderTypeEnum.Workspace, Count = x.DocCount.ToString(), WorkspaceId = x.WorkspaceId, parentId = _userContext.UserId, CanOpen = x.CanOpen, ShowMenu = x.ShowMenu, CanCreateSubFolder = x.CanCreateSubFolder, CanRename = x.CanRename, CanShare = x.CanShare, CanMove = x.CanMove, CanCopy = x.CanCopy, CanArchive = x.CanArchive, CanDelete = x.CanDelete, CanSeeDetail = x.CanSeeDetail, CanManagePermission = x.CanManagePermission, TemplateCode = x.FolderCode, CanCreateWorkspace = _userContext.IsSystemAdmin })));
                }
                else
                {
                    var list = new List<DirectoryContent>();
                    if (!args.Data[0].IsNotNull())
                    {
                        return Ok(new object());
                    }
                    var parentId = args.Action != "delete" ? args.Data[0].Id : args.Data[0].ParentId;
                    // var workspaces = await _noteBusiness.GetList(x => x.Id == parentId);
                    var workspaces1 = await _documentBusiness.GetUserWorkspaceTreeDataNew(_userContext.UserId, parentId);
                    var workspaces = workspaces1.Where(x => x.ParentId == parentId).ToList();
                    foreach (var ws in workspaces)
                    {
                        List<FolderViewModel> childlist = new List<FolderViewModel>();
                        var childs = await _documentBusiness.GetAllChildDocumentByParentId(ws.Id, childlist);
                        ws.DocCount = childs.Where(x => x.TemaplateMasterCatCode == "GENERAL_DOCUMENT").Count();
                    }
                    //var workspace = workspaces.FirstOrDefault();
                    //if (!workspace.IsNotNull())
                    //{
                    //    return Json(new object());
                    //}
                    var parent = workspaces1.Where(x => x.Id == parentId).FirstOrDefault();
                    if (parent != null)
                    {
                        var value = new DirectoryContent { id = parentId, parentId = parent.ParentId, WorkspaceId = parent.WorkspaceId, name = parent.Name, hasChild = (workspaces.Count() > 0) ? true : false, FolderType = parent.FolderType.Value, Count = parent.DocCount.ToString(), TemplateCode = parent.FolderCode, CanOpen = parent.CanOpen, ShowMenu = parent.ShowMenu, CanCreateSubFolder = parent.CanCreateSubFolder, CanRename = parent.CanRename, CanShare = parent.CanShare, CanMove = parent.CanMove, CanCopy = parent.CanCopy, CanArchive = parent.CanArchive, CanDelete = parent.CanDelete, CanSeeDetail = parent.CanSeeDetail, CanManagePermission = parent.CanManagePermission, CanCreateWorkspace = _userContext.IsSystemAdmin };
                        readResponse.cwd = JsonConvert.DeserializeObject<DirectoryContent>(JsonConvert.SerializeObject(value));
                        if (parent.FolderCode == "GENERAL_FOLDER" /*|| parent.FolderCode == "WORKSPACE_GENERAL"*/)
                        //if(workspace.TemplateCode == "GENERAL_FOLDER" || workspace.TemplateCode == "WORKSPACE_GENERAL")
                        {
                            var data = await _documentBusiness.GetFoldersAndDocumentsNew(_userContext.UserId, DocumentQueryTypeEnum.Document, parentId);
                            list = data.Select(x => new DirectoryContent { id = x.Id, name = x.Name, hasChild = true, FolderType = x.FolderType.Value, Count = x.DocCount.ToString(), parentId = x.ParentId, WorkspaceId = x.WorkspaceId, CanOpen = x.CanOpen, ShowMenu = x.ShowMenu, CanCreateSubFolder = x.CanCreateSubFolder, CanRename = x.CanRename, CanShare = x.CanShareDocument, CanMove = x.CanMoveDocument, CanCopy = x.CanCopyDocument, CanArchive = x.CanArchiveDocument, CanDelete = x.CanDeleteDocument, CanSeeDetail = x.CanSeeDetail, CanManagePermission = x.CanManagePermissionDocument, TemplateCode = x.FolderCode, CanCreateWorkspace = _userContext.IsSystemAdmin, WorkflowTemplateCode = x.WorkflowCode, DocumentApprovalStatusType = x.DocumentApprovalStatusType, WorkflowServiceId = x.WorkflowServiceId, StatusName = x.NoteStatus, CanEditDocument = x.CanEditDocument }).ToList();
                        }
                    }
                    else
                    {
                        var parentNote1 = await _documentBusiness.GetFoldersAndDocumentsNew(_userContext.UserId, DocumentQueryTypeEnum.Document, null, parentId);
                        var parentNote = parentNote1.FirstOrDefault(); //await _noteBusiness.GetSingleById(parentId);
                        if (parentNote != null)
                        {
                            //var value = new DirectoryContent { id = parentNote.Id, name = parentNote.Name, hasChild = false, FolderType = FolderTypeEnum.File, Count = "0", CanOpen = parentNote.CanOpen, ShowMenu = parentNote.ShowMenu, CanCreateSubFolder = parentNote.CanCreateSubFolder, CanRename = parentNote.CanRename, CanShare = parentNote.CanShareDocument, CanMove = parentNote.CanMoveDocument, CanCopy = parentNote.CanCopyDocument, CanArchive = parentNote.CanArchiveDocument, CanDelete = parentNote.CanDeleteDocument, CanSeeDetail = parentNote.CanSeeDetail, CanManagePermission = parentNote.CanManagePermissionDocument, TemplateCode = parentNote.FolderCode, CanCreateWorkspace = _userContext.IsSystemAdmin, WorkflowTemplateCode = parentNote.WorkflowCode, DocumentApprovalStatusType = parentNote.DocumentApprovalStatusType, WorkflowServiceId = parentNote.WorkflowServiceId, StatusName = parentNote.NoteStatus, CanEditDocument = parentNote.CanEditDocument };
                            var value = new DirectoryContent { id = parentNote.Id, WorkspaceId = parentNote.WorkspaceId, parentId = parentNote.ParentId, name = parentNote.Name, hasChild = false, FolderType = FolderTypeEnum.File, Count = "0" };
                            readResponse.cwd = JsonConvert.DeserializeObject<DirectoryContent>(JsonConvert.SerializeObject(value));
                            list = await _noteBusiness.GetAllDocumentFiles(parentId);
                            if (list.IsNotNull())
                            {
                                list.ForEach(x => x.CanEditDocument = parentNote.CanEditDocument);
                            }
                        }

                    }
                    list.AddRange(workspaces.Select(x => new DirectoryContent { id = x.Id, WorkspaceId = x.WorkspaceId, name = x.Name, hasChild = true, FolderType = x.FolderType.Value, Count = /*workspaces1.Where(y=>y.ParentId== x.Id).Count().ToString()*/ x.DocCount.ToString(), parentId = x.ParentId, CanOpen = x.CanOpen, ShowMenu = x.ShowMenu, CanCreateSubFolder = x.CanCreateSubFolder, CanRename = x.CanRename, CanShare = x.CanShare, CanMove = x.CanMove, CanCopy = x.CanCopy, CanArchive = x.CanArchive, CanDelete = x.CanDelete, CanSeeDetail = x.CanSeeDetail, CanManagePermission = x.CanManagePermission, TemplateCode = x.FolderCode, CanCreateWorkspace = _userContext.IsSystemAdmin }));
                    readResponse.files = JsonConvert.DeserializeObject<IEnumerable<DirectoryContent>>(JsonConvert.SerializeObject(list));
                }
                var json = JsonConvert.SerializeObject(readResponse);
                return Ok(readResponse);

            }
            catch
            {
                //k ErrorDetails er = new ErrorDetails();

            }
            return Ok(new object());
        }

        [HttpGet]
        [Route("DeleteNote")]
        public async Task<IActionResult> DeleteNote(string id)
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _documentBusiness = _serviceProvider.GetService<IDMSDocumentBusiness>();
            List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            var subfolders = await _documentBusiness.GetAllChildByParentId(id, subfoldersList);
            if (subfolders.Any())
            {
                return Ok(new { success = false, errors = "You cannot delete this folder because it has one more child documents." });
            }
            await _noteBusiness.Delete(id);
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("ArchiveNote")]
        public async Task<ActionResult> ArchiveNote(string id)
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _documentBusiness = _serviceProvider.GetService<IDMSDocumentBusiness>();
            List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            var subfolders = await _documentBusiness.GetAllChildByParentId(id, subfoldersList);
            if (subfolders.Any())
            {
                return Ok(new { success = false, errors = "You cannot archive this folder because it has one more child documents." });
            }
            await _noteBusiness.Archive(id);
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("CopyNote")]
        public async Task<ActionResult> CopyNote(string sourceId, string targetId, string userId)
        {
            var _documentBusiness = _serviceProvider.GetService<IDMSDocumentBusiness>();
            var result = await CreateNote(sourceId, targetId, userId);
            if (result.IsSuccess)
            {
                var documents = await _documentBusiness.GetAllFiles(sourceId, null, null);
                foreach (var doc in documents)
                {
                    await CreateNote(doc.Id, result.Item.NoteId, userId);
                }
                List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
                var subfolders = await _documentBusiness.GetAllChildByParentId(sourceId, subfoldersList);
                foreach (var folder in subfolders.Where(x => x.FolderCode == "GENERAL_FOLDER"))
                {
                    await CopyNote(folder.Id, result.Item.NoteId, userId);
                }
                return Ok(new { success = true });

            }
            return Ok(new { success = false });
        }

        [HttpGet]
        [Route("MoveNote")]
        public async Task<ActionResult> MoveNote(string sourceId, string targetId)
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            var model = await _noteBusiness.GetSingleById(sourceId);
            if (model.IsNotNull())
            {
                var previousParentId = model.ParentNoteId;
                model.ParentNoteId = targetId;
                var result = await _noteBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    if (targetId.IsNotNull())
                    {
                        // Delete previous parent permission from note and its child
                        if (previousParentId.IsNotNull())
                        {
                            await _documentPermissionBusiness.DeleteOldParentPermission(sourceId);
                        }
                        //Add new parent permission to the parent note                   
                        await _documentPermissionBusiness.ManageInheritedPermission(sourceId, targetId);

                        // Add new parent permission to all its child
                        await _documentPermissionBusiness.ManageChildPermissions(targetId);
                    }
                    return Ok(new { success = true });

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
            return Ok(new { success = false });
        }

        [HttpPost]
        [Route("SearchFiles")]
        public async Task<ActionResult> SearchFiles(FileOperation args)
        {
            await Authenticate(args.UserId);
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _documentBusiness = _serviceProvider.GetService<IDMSDocumentBusiness>();
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var searchStr = args.SearchString.Replace("*", "");
            FileResponse readResponse = new FileResponse();
            try
            {
                var pathIds = args.Path.Split('/');
                if (args.Data.Count() == 0 || pathIds.Count() == 2)
                {
                    //var user = new DirectoryContent { id = _userContext.UserId, name = _userContext.Name, hasChild = true, FolderType = FolderTypeEnum.Root };
                    //var workspaces = await _noteBusiness.GetList(x => x.TemplateCode == "WORKSPACE_GENERAL" & x.ParentNoteId == null);
                    //workspaces = workspaces.Where(x => x.NoteSubject.ToLower().Contains(searchStr)).ToList();
                    var user = new DirectoryContent { id = _userContext.UserId, name = _userContext.Name, hasChild = true, FolderType = FolderTypeEnum.Root };
                    var workspaces = await _documentBusiness.GetUserWorkspaceTreeDataNew(_userContext.UserId, null);
                    workspaces = workspaces.Where(x => x.FolderCode == "WORKSPACE_GENERAL" && x.ParentId == null && x.Name.ToLower().Contains(searchStr)).ToList();
                    readResponse.cwd = JsonConvert.DeserializeObject<DirectoryContent>(JsonConvert.SerializeObject(user));
                    readResponse.files = JsonConvert.DeserializeObject<IEnumerable<DirectoryContent>>(JsonConvert.SerializeObject(workspaces.Select(x => new DirectoryContent { id = x.Id, name = x.Name, hasChild = true, FolderType = FolderTypeEnum.Workspace, Count = x.DocCount.ToString(), parentId = _userContext.UserId, CanOpen = x.CanOpen, ShowMenu = x.ShowMenu, CanCreateSubFolder = x.CanCreateSubFolder, TemplateCode = x.FolderCode })));

                }
                else
                {
                    var list = new List<DirectoryContent>();
                    var parentId = args.Action != "delete" ? args.Data[0].Id : args.Data[0].ParentId;
                    //var workspaces = await _noteBusiness.GetList(x => x.Id == parentId);
                    //var workspace = workspaces.FirstOrDefault();
                    var workspaces1 = await _documentBusiness.GetUserWorkspaceTreeDataNew(_userContext.UserId, parentId);
                    var workspaces = workspaces1.Where(x => x.ParentId == parentId).ToList();
                    //if (!workspaces.FirstOrDefault().IsNotNull())
                    //{
                    //    return Json(new object());
                    //}
                    var parent = workspaces1.Where(x => x.Id == parentId).FirstOrDefault();
                    if (parent != null)
                    {
                        var value = new DirectoryContent { id = parentId, name = parent.Name, hasChild = (workspaces.Count() > 0) ? true : false, FolderType = parent.FolderType.Value, Count = parent.DocCount.ToString(), TemplateCode = parent.FolderCode /*(workspace.FolderCode == "WORKSPACE_GENERAL") ? FolderTypeEnum.Workspace: (workspace.FolderCode == "GENERAL_FOLDER") ?FolderTypeEnum.Folder :FolderTypeEnum.Document*/ };
                        readResponse.cwd = JsonConvert.DeserializeObject<DirectoryContent>(JsonConvert.SerializeObject(value));
                        if (parent.FolderCode == "GENERAL_FOLDER" /*|| parent.FolderCode == "WORKSPACE_GENERAL"*/)
                        //if(workspace.TemplateCode == "GENERAL_FOLDER" || workspace.TemplateCode == "WORKSPACE_GENERAL")
                        {
                            //list = await _noteBusiness.GetAllChildDocuments(parentId);     
                            //list = await _noteBusiness.GetAllFolderDocuments(parentId);
                            var data = await _documentBusiness.GetFoldersAndDocumentsNew(_userContext.UserId, DocumentQueryTypeEnum.Document, parentId);
                            list = data.Select(x => new DirectoryContent { id = x.Id, name = x.Name, hasChild = true, FolderType = x.FolderType.Value, Count = x.DocCount.ToString(), parentId = x.ParentId, CanOpen = x.CanOpen, ShowMenu = x.ShowMenu, CanCreateSubFolder = x.CanCreateSubFolder, TemplateCode = x.FolderCode }).ToList();
                        }
                    }
                    else
                    {
                        var parentNote = await _noteBusiness.GetSingleById(parentId);
                        if (parentNote != null)
                        {
                            var value = new DirectoryContent { id = parentNote.Id, name = parentNote.NoteSubject, hasChild = false, FolderType = FolderTypeEnum.File, Count = "0"/*, permission = new AccessPermission { Write = false, Copy = false, Read = false } (workspace.FolderCode == "WORKSPACE_GENERAL") ? FolderTypeEnum.Workspace: (workspace.FolderCode == "GENERAL_FOLDER") ?FolderTypeEnum.Folder :FolderTypeEnum.Document*/ };
                            readResponse.cwd = JsonConvert.DeserializeObject<DirectoryContent>(JsonConvert.SerializeObject(value));
                            list = await _noteBusiness.GetAllDocumentFiles(parentId);
                        }

                    }
                    list.AddRange(workspaces.Select(x => new DirectoryContent { id = x.Id, name = x.Name, hasChild = true, FolderType = x.FolderType.Value, Count = x.DocCount.ToString(), parentId = x.ParentId, CanOpen = x.CanOpen, ShowMenu = x.ShowMenu, CanCreateSubFolder = x.CanCreateSubFolder, TemplateCode = x.FolderCode }));
                    list = list.Where(x => x.name.ToLower().Contains(searchStr)).ToList();
                    readResponse.files = JsonConvert.DeserializeObject<IEnumerable<DirectoryContent>>(JsonConvert.SerializeObject(list));
                }
                var json = JsonConvert.SerializeObject(readResponse);
                return Ok(readResponse);

            }
            catch
            {
                //ErrorDetails er = new ErrorDetails();

            }
            return Ok(new object());
        }

        [HttpGet]
        [Route("CreateNote")]
        private async Task<CommandResult<NoteTemplateViewModel>> CreateNote(string sourceId, string targetId, string userId)
        {
            await Authenticate(userId);
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _userContext = _serviceProvider.GetService<IUserContext>();
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

        [HttpGet]
        [Route("RenameNote")]
        public async Task<ActionResult> RenameNote(FileOperation args)
        {
            await Authenticate(args.UserId);
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _userContext = _serviceProvider.GetService<IUserContext>();
            try
            {
                var folderName = args.NewName;
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = DataActionEnum.Edit;
                templateModel.NoteId = args.Data[0].Id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                newmodel.NoteSubject = folderName;
                newmodel.DataAction = DataActionEnum.Edit;
                newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                newmodel.OwnerUserId = _userContext.UserId;
                var result = await _noteBusiness.ManageNote(newmodel);
                if (result.IsSuccess)
                {
                    return await this.GetFiles(args);
                }
                return await this.GetFiles(args);

            }
            catch (Exception ex)
            {
                return await this.GetFiles(args);

            }
        }

        [HttpGet]
        [Route("Create")]
        public async Task<ActionResult> Create(FileOperation args)
        {
            try
            {
                await Authenticate(args.UserId);
                var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
                var _userContext = _serviceProvider.GetService<IUserContext>();
                var parentId = args.Data[0].Id;
                var folderName = args.Name;
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = DataActionEnum.Create;
                templateModel.TemplateCode = "GENERAL_FOLDER";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                newmodel.NoteSubject = folderName;
                newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                newmodel.ParentNoteId = parentId;
                newmodel.OwnerUserId = _userContext.UserId;
                var result = await _noteBusiness.ManageNote(newmodel);
                if (result.IsSuccess)
                {
                    return await this.GetFiles(args);
                }
                return await this.GetFiles(args);
            }
            catch (Exception ex)
            {
                return await this.GetFiles(args);
            }
        }

        [HttpGet]
        [Route("ReadWorkspaceData")]
        public async Task<ActionResult> ReadWorkspaceData()
        {
            var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            var model = await _documentPermissionBusiness.GetWorkspaceList();
            var data = model.ToList();

            var dsResult = data;
            return Ok(dsResult);
        }


        [HttpGet]
        [Route("ViewPermissionData")]
        public async Task<ActionResult> ViewPermissionData(string NoteId)
        {
            var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            var model = await _documentPermissionBusiness.ViewPermissionList(NoteId);
            var data = model.ToList();
            var dsResult = data;
            return Ok(dsResult);

        }


        [HttpGet]
        [Route("ViewPermission")]
        public async Task<ActionResult> ViewPermission(string noteId, string workspaceId, string parentId, string Type)
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var note = await _noteBusiness.GetSingleById(noteId);
            //var note1 = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { NoteId = noteId });
            var model = new DocumentPermissionViewModel
            {
                WorkspaceId = workspaceId,
                NoteId = noteId,
                ParentId = parentId,
                DisablePermissionInheritance = note.DisablePermissionInheritance,
                //NoteType = Type,
                //IsDocument = note1.TemplateCategoryCode == "GENERAL_DOCUMENT",
                //ExpiryDate = note1.ExpiryDate,
                // DocumentName = note.Subject
            };
            return Ok(model);
        }


        [HttpGet]
        [Route("GetPermissionDetails")]
        public async Task<ActionResult> GetPermissionDetails(string noteId)
        {
            var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            var model = await _documentPermissionBusiness.GetPermissionList(noteId);

            //foreach (var item in model)
            //{
            //    item.Access= GetCategoryName(Convert.ToInt32(Convert.ToInt32( item.Access)));
            //}



            return Ok(model);

            //return Json(model.ToDataSourceResult(request));
        }

        [HttpGet]
        [Route("GetUsersIdNameList")]
        public async Task<ActionResult> GetUsersIdNameList(string userId)
        {

            await Authenticate(userId, "DMS");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
            List<UserViewModel> list = new List<UserViewModel>();
            list = await _userBusiness.GetList(x => x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);
            return Ok(list);
        }

        [HttpGet]
        [Route("GetUserGroupIdNameList")]
        public async Task<ActionResult> GetUserGroupIdNameList(string userId)
        {
            await Authenticate(userId, "DMS");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _userGroupBusiness = _serviceProvider.GetService<IUserGroupBusiness>();
            var list = await _userGroupBusiness.GetList(x => x.PortalId == _userContext.PortalId && x.LegalEntityId == _userContext.LegalEntityId);
            return Ok(list);
        }


        [HttpGet]
        [Route("DisableParentPermission")]
        public async Task<ActionResult> DisableParentPermission(string id, string InheritanceStatus)
        {
            var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            await _documentPermissionBusiness.DisableParentPermissions(id, InheritanceStatus);
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("DeletePermission")]
        public async Task<IActionResult> DeletePermission(string Id)
        {
            var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            var permission = await _documentPermissionBusiness.GetSingleById(Id);
            await _documentPermissionBusiness.Delete(Id);
            await _documentPermissionBusiness.DeleteInheritedPermissionFromChildOnPermissionDelete(permission.NoteId, Id);
            await _documentPermissionBusiness.DeleteInheritedPermissionFromParentOnPermissionDelete(permission.NoteId, Id);
            return Ok(true);
        }


        [HttpGet]
        [Route("GetBinDocumentData")]
        public async Task<ActionResult> GetBinDocumentData(string userId)
        {
            var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            var result = await _documentPermissionBusiness.GetBinDocumentData(userId);
            return Ok(result);
        }

        [HttpGet]
        [Route("Getfolderpath")]
        public async Task<ActionResult> Getfolderpath(string NoteId)
        {
            var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
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

                    return Ok(folderpath);
                }

            }
            //   return Json("true");
        }

        [HttpGet]
        [Route("GetArchivedDocumentData")]
        public async Task<ActionResult> GetArchivedDocumentData(string userId)
        {
            var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            var result = await _documentPermissionBusiness.GetArchive(userId);
            return Ok(result);

        }
        [HttpGet]
        [Route("GetAllFolderAndDocument")]
        public async Task<ActionResult> GetAllFolderAndDocument(string userId)
        {
            var _documentBusiness = _serviceProvider.GetService<IDMSDocumentBusiness>();
            var list = await _documentBusiness.GetAllFolderAndDocumentByUserId(userId);
            return Ok(list);
        }
        [HttpGet]
        [Route("ReadDocumentData")]
        public async Task<ActionResult> ReadDocumentData(string templateId, string projectNo, string noteNo, string docDescription, int docCount = 0)
        {
            // var list = new List<DocumentListViewModel>();
            var _documentBusiness = _serviceProvider.GetService<IDMSDocumentBusiness>();
            var list = await _documentBusiness.DocumentReportDataWithFilter(templateId, noteNo, projectNo, docDescription);


            //if (noteNo.IsNotNullAndNotEmpty())
            //{
            //    list = list.Where(x => x.NoteNo.ToLower().Contains(noteNo.ToLower())).ToList();
            //}

            //if (docDescription.IsNotNullAndNotEmpty())
            //{
            //    list = list.Where(x => x.NoteDescription.ToLower().Contains(docDescription.ToLower())).ToList();
            //}
            //if (projectNo.IsNotNullAndNotEmpty())
            //{
            //    list = list.Where(x => x.ProjectNo.Contains(projectNo)).ToList();
            //}
            var j = Ok(list);
            return j;
        }

        [HttpGet]
        [Route("GetAttachments")]
        public async Task<ActionResult> GetAttachments(string userId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _documentBusiness = _serviceProvider.GetService<IDMSDocumentBusiness>();
            var list = await _documentBusiness.GetUserAttachments(userId, _userContext.PortalId);
            return Ok(list);
        }


        [HttpGet]
        [Route("GetSourceFolders")]
        public async Task<object> GetSoureceFolders(string key, string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                            await GetParentNoteId(note, note, list, children, _userContext);
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

        private async Task<List<FileExplorerViewModel>> GetParentNoteId(NoteViewModel key, NoteViewModel model, List<FileExplorerViewModel> list, List<FileExplorerViewModel> children1, IUserContext _userContext)
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
                    return await GetParentNoteId(key, note, list, children, _userContext);
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

        [HttpGet]
        [Route("GetChildFolders")]
        public async Task<ActionResult> GetChildFolders(string key, string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                return Ok(json);
            }

            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("GetChildFoldersAndDocuments")]
        public async Task<ActionResult> GetChildFoldersAndDocuments(string key, string activeId, string viewMode, string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                return Ok(json);
            }

            catch (Exception)
            {

                throw;
            }

        }

    }


}
