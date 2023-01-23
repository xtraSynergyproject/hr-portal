﻿using Cms.UI.ViewModel;
using CMS.Business;
using CMS.Business.Interface.DMS;
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using CMS.Web.Api.Areas.DMS.Models;
using CMS.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syncfusion.EJ2.FileManager.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Web.Api.Areas.DMS.Controllers
{
    [Route("dms/query")]
    [ApiController]
    public class QueryController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;


        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
         IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
                ErrorDetails er = new ErrorDetails();

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
                            var value = new DirectoryContent { id = parentNote.Id, name = parentNote.NoteSubject, hasChild = false, FolderType = FolderTypeEnum.File, Count = "0", permission = new AccessPermission { Write = false, Copy = false, Read = false } /*(workspace.FolderCode == "WORKSPACE_GENERAL") ? FolderTypeEnum.Workspace: (workspace.FolderCode == "GENERAL_FOLDER") ?FolderTypeEnum.Folder :FolderTypeEnum.Document*/ };
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
                ErrorDetails er = new ErrorDetails();

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
        [Route("ReadParentWorkspaceIdNameList")]
        public async Task<ActionResult> ReadParentWorkspaceIdNameList(long? legalEntity, string id)
        {
            var result3=new List<object>();
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var pagedata = await _noteBusiness.GetList(x => x.TemplateCode == "WORKSPACE_GENERAL");
            var result = pagedata.Where(x => id.IsNotNull() ? x.ParentNoteId == id : x.ParentNoteId == null)
              .Select(item => new
              {
                  id = item.Id,
                  Name = item.NoteSubject,
                  hasChildren = pagedata.Where(x => x.ParentNoteId == id).Count() > 0 ? true : false,
                  children = pagedata.Where(x => x.ParentNoteId == item.Id).Select(item => new
                  {
                      id = item.Id,
                      Name = item.NoteSubject,
                      hasChildren = pagedata.Where(x => x.ParentNoteId == id).Count() > 0 ? true : false,
                      children = pagedata.Where(x => x.ParentNoteId == item.Id).Select(item => new
                      {
                          id = item.Id,
                          Name = item.NoteSubject,
                          hasChildren = pagedata.Where(x => x.ParentNoteId == id).Count() > 0 ? true : false,
                          children = pagedata.Where(x => x.ParentNoteId == item.Id).Select(item => new
                          {
                              id = item.Id,
                              Name = item.NoteSubject,
                              hasChildren = pagedata.Where(x => x.ParentNoteId == id).Count() > 0 ? true : false,
                              children = pagedata.Where(x => x.ParentNoteId == item.Id).Select(item => new
                              {
                                  id = item.Id,
                                  Name = item.NoteSubject,
                                  hasChildren = pagedata.Where(x => x.ParentNoteId == id).Count() > 0 ? true : false,
                                  children = pagedata.Where(x => x.ParentNoteId == item.Id).ToList()
                              }).ToList()
                          }).ToList()
                      }).ToList()
                  }).ToList()
              }).ToList();

            //var result = pagedata.Where(x => id.IsNotNull() ? x.ParentNoteId == id : x.ParentNoteId == null)
            //  .Select(item => new
            //  {
            //      id = item.Id,
            //      Name = item.NoteSubject,
            //      hasChildren = pagedata.Where(x => x.ParentNoteId == id).Count() > 0 ? true : false,
            //      children = pagedata.Where(x => x.ParentNoteId == id).ToList()
            //  });
            //foreach (var res in result)
            //{
            //    var result2 = pagedata.Where(x => res.id.IsNotNull() ? x.ParentNoteId == res.id : x.ParentNoteId == null)
            //  .Select(item => new
            //  {
            //      id = item.Id,
            //      Name = item.NoteSubject,
            //      hasChildren = pagedata.Where(x => x.ParentNoteId == res.id).Count() > 0 ? true : false,
            //      children = pagedata.Where(x => x.ParentNoteId == res.id).ToList()
            //  });
            //    result3.Add(result2);

            //}
            return Ok(result);
        }

        [HttpGet]
        [Route("GetDocumentTemplateIdNameListByUser")]
        public async Task<ActionResult> GetDocumentTemplateIdNameListByUser()
        {
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            var _templateCategoryBusiness = _serviceProvider.GetService<ITemplateCategoryBusiness>();
            var tempCategoryId = await _templateCategoryBusiness.GetSingle(x => x.Code == "GENERAL_DOCUMENT");
            var templatlist = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategoryId.Id && x.Code != "GENERAL");
            return Ok(templatlist);
        }
        [HttpGet]
        [Route("ReadWorkspaceData")]
        public async Task<ActionResult> ReadWorkspaceData()
        {
            var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            var model =await _documentPermissionBusiness.GetWorkspaceList();
            var data = model.ToList();

            var dsResult = data;
            return Ok(dsResult);
        }

        [HttpGet]
        [Route("DeleteWorkspace")]
        public async Task<ActionResult> DeleteWorkspace(string NoteId)
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            //var deleteworkspace = await _tableMetadataBusiness.DeleteTableDataByHeaderId("WORKSPACE_GENERAL", null, NoteId);
            //if (deleteworkspace.IsNotNull())
            //{
            //    return Json(new { success = true });
            //}
            //return Json(new { success = false });

            var result = await _noteBusiness.DeleteWorkspace(NoteId);
            return Ok(new { success = result });
        }

        [HttpGet]
        [Route("ViewPermissionData")]
        public async Task<ActionResult> ViewPermissionData( string NoteId)
        {
            var _documentPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            var model =await _documentPermissionBusiness.ViewPermissionList(NoteId);
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

            await Authenticate(userId,"DMS");
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
    }


}
