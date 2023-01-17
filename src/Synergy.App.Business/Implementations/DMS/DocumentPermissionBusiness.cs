using AutoMapper;
using Synergy.App.ViewModel;
using Synergy.App.Business.Interface;
using Synergy.App.Business.Interface.DMS;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class DocumentPermissionBusiness: BusinessBase<DocumentPermissionViewModel, DocumentPermission>, IDocumentPermissionBusiness

    {

        private readonly IRepositoryQueryBase<TemplateViewModel> _queryRepo;
        private readonly IServiceProvider _sp;
        private readonly IRepositoryQueryBase<DocumentPermissionViewModel> _querytag;
        private readonly IRepositoryQueryBase<DMSDocumentViewModel> _queryDMSDocument;
        ITemplateCategoryBusiness _templateCategoryBusiness;
        ITemplateBusiness _templateBusiness;
        ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IRepositoryQueryBase<WorkspaceViewModel> _queryworkspace;
        private readonly IRepositoryQueryBase<DMSCalenderViewModel> _querycalndarDocument;
        private readonly IUserContext _userContext;
        private IRepositoryQueryBase<NoteLinkShareViewModel> _QueryShareLink;
        INoteBusiness _noteBusiness;
        IUserGroupUserBusiness _userGroupBusiness;
        private readonly IDocumentManagementQueryBusiness _documentManagementQueryBusiness;
        //INoteBusiness _note


        // public DocumentPermissionBusiness(IRepositoryBase<DocumentPermissionViewModel, DocumentPermission> repo, IRepositoryQueryBase<DocumentPermissionViewModel> querytag, IRepositoryQueryBase<TemplateViewModel> queryRepo, IMapper autoMapper, IRepositoryQueryBase<DMSDocumentViewModel> queryDMSDocument, ITemplateCategoryBusiness templateCategoryBusiness, ITemplateBusiness templateBusiness, ITableMetadataBusiness tableMetadataBusiness) : base(repo, autoMapper)
        public DocumentPermissionBusiness(IRepositoryBase<DocumentPermissionViewModel, DocumentPermission> repo,IRepositoryQueryBase<WorkspaceViewModel> queryworkspace, IRepositoryQueryBase<DocumentPermissionViewModel> querytag, IRepositoryQueryBase<TemplateViewModel> queryRepo, IMapper autoMapper, IRepositoryQueryBase<DMSDocumentViewModel> queryDMSDocument, ITemplateCategoryBusiness templateCategoryBusiness, ITemplateBusiness templateBusiness, ITableMetadataBusiness tableMetadataBusiness
            , IServiceProvider sp, IUserContext userContext, IDocumentManagementQueryBusiness documentManagementQueryBusiness, IRepositoryQueryBase<DMSCalenderViewModel> querycalndarDocument, IRepositoryQueryBase<NoteLinkShareViewModel> QueryShareLink, INoteBusiness noteBusiness, IUserGroupUserBusiness userGroupBusiness) : base(repo, autoMapper)
            
        {
            _querytag = querytag;
            _queryRepo = queryRepo;
            _queryworkspace = queryworkspace;

            _queryDMSDocument = queryDMSDocument;
            _templateCategoryBusiness = templateCategoryBusiness;
            _templateBusiness = templateBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _sp = sp;
            _userContext = userContext;
            _querycalndarDocument = querycalndarDocument;
            _QueryShareLink = QueryShareLink;
            _noteBusiness = noteBusiness;
            _userGroupBusiness = userGroupBusiness;
            _documentManagementQueryBusiness = documentManagementQueryBusiness;
        }



        public async override Task<CommandResult<DocumentPermissionViewModel>> Create(DocumentPermissionViewModel model, bool autoCommit = true)
        {      
            
           
            var result = await base.Create(model,autoCommit);
            if(result.IsSuccess && !model.DisablePermittedNotification)
            {
                await SendNotification(model);
            }
           
            return CommandResult<DocumentPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task SendNotification(DocumentPermissionViewModel model)
        {
            var note = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { NoteId = model.NoteId, DataAction = DataActionEnum.Read });
            note.OwnerUserId = _repo.UserContext.UserId;
            var notificationTemplate = await _repo.GetSingle<NotificationTemplate, NotificationTemplate>(x => x.Code == "NOTE_PERMITTED_USER_TEMPLATE" && x.NtsType == NtsTypeEnum.Note);
            if (notificationTemplate != null)
            {
                if (model.PermittedUserId.IsNotNull() && model.PermittedUserId != _repo.UserContext.UserId)
                {
                    await _noteBusiness.SendNotification(note, notificationTemplate, model.PermittedUserId);

                }
                if (model.PermittedUserGroupId.IsNotNull())
                {
                    var users = await _userGroupBusiness.GetList(x => x.UserGroupId == model.PermittedUserGroupId);
                    foreach (var item in users)
                    {
                        await _noteBusiness.SendNotification(note, notificationTemplate, item.UserId);
                    }

                }
            }
        }
        public async Task<List<DocumentPermissionViewModel>> GetPermissionList(string noteId)
        {


            var queryData = await _documentManagementQueryBusiness.GetPermissionList(noteId);
            return queryData;
        }
        public async Task<List<TemplateViewModel>> GetTemplateList(string parentId)
        {



            var list = await _documentManagementQueryBusiness.GetTemplateList(parentId);
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);
           
                return list;
            
           
        }

        public async Task<List<DMSDocumentViewModel>> GetArchive(string UserID)
        {


            var ressult = await _documentManagementQueryBusiness.GetArchive(UserID);

            return ressult;
        }



        public async Task<List<DMSDocumentViewModel>> GetFolderByParent(string ParentId)
        {
            var ressult = await _documentManagementQueryBusiness.GetFolderByParent(ParentId);
            return ressult;

        }

        public async Task<List<DMSDocumentViewModel>> GetBinDocumentData(string UserID)
        {

            var ressult = await _documentManagementQueryBusiness.GetBinDocumentData(UserID);


            return ressult;
        }


        public async Task<bool> CheckUserPermission(string NoteId, string UserId)
        {
            var ressult = await _documentManagementQueryBusiness.getDocData(NoteId, UserId);
            if (ressult.Count > 0)
            {
                return true;
            }
            else
            {
                var ressult1 = await _documentManagementQueryBusiness.getDocumentPermission(NoteId, UserId);

                if (ressult1.Count > 0)
                {
                    return true;
                }
                else { return false; }


            }
        }


        public async Task<bool> DeleteDocument(string NoteId)
        {
            await _documentManagementQueryBusiness.DeleteDocument(NoteId);
            return true;
        }

        public async Task<CommandResult<WorkspaceViewModel>> ValidateSequenceOrder(WorkspaceViewModel model)
        {


            var result = await _documentManagementQueryBusiness.ValidateSequenceOrder(model);
            var exist = result.Where(x => x.SequenceOrder == model.SequenceOrder && x.NoteId != model.NoteId);

            if (exist.Any())
            {
                return CommandResult<WorkspaceViewModel>.Instance(model, false, "Sequence No already exist");
            }

            return CommandResult<WorkspaceViewModel>.Instance(model, true,"");

        }
        public async Task<CommandResult<WorkspaceViewModel>> ValidateWorkspace(WorkspaceViewModel model)
        {

            var result = await _documentManagementQueryBusiness.ValidateWorkspace(model);
            var exist = result.Where(x => x.NoteSubject == model.WorkspaceName && x.NoteId != model.NoteId);

            if (exist.Any())
            {
                return CommandResult<WorkspaceViewModel>.Instance(model, false, "Workspace Name already exist");
            }
            if (model.Code.IsNotNullAndNotEmpty() && result.Any(x => x.Code == model.Code && x.NoteId != model.NoteId))
            {
                return CommandResult<WorkspaceViewModel>.Instance(model, false, "Code already exist");
            }
            return CommandResult<WorkspaceViewModel>.Instance(model, true, "");

        }
        public async Task<List<WorkspaceViewModel>> GetWorkspaceList()
        {

            var list = await _documentManagementQueryBusiness.GetWorkspaceList();


            return list;
        }
        public async Task<WorkspaceViewModel> GetWorkspaceEdit(string workspaceId)
        {

            var queryData = await _documentManagementQueryBusiness.GetWorkspaceEdit(workspaceId);
            var list = queryData;
            return list;
        }
        public async Task<List<WorkspaceViewModel>> DocumentTemplateList(string workspaceId)
        {



            var list = await _documentManagementQueryBusiness.DocumentTemplateList(workspaceId);
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);
            return list;
        }



        public async Task<bool> RestoreBinDocument(string NoteId)
        {
            await _documentManagementQueryBusiness.RestoreBinDocument(NoteId);
            return true;
        }


        public async Task<bool> RestoreArchiveDocument(string Id,string TableMetadataid)
        {

            await _documentManagementQueryBusiness.RestoreArchiveDocument(Id, TableMetadataid);
            return true;
        }
        public async Task<List<DocumentPermissionViewModel>> ViewPermissionList(string NoteId)
        {



            var list = await _documentManagementQueryBusiness.ViewPermissionList(NoteId);
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);
            return list;
        }
        public async Task<bool> DeleteWorkspace(string NoteId)
        {
            var note = await _repo.GetSingleById(NoteId);
            if (note != null)
            {
                await _documentManagementQueryBusiness.DeleteWorkspace(NoteId);

                await Delete(NoteId);
                return true;
            }
            return false;
        }

        public async Task ManageInheritedPermission(string noteId, string parentId)
        {
            
            var ParentPermissionList =await GetNotePermissionData(parentId);
            var notePermissions = await GetNotePermissionData(noteId);
            if (ParentPermissionList != null && ParentPermissionList.Count() > 0)
            {
                foreach (var permission in ParentPermissionList.Where(x => x.IsInheritedFromChild == false))
                {
                    if (permission.DisablePermissionInheritance == null || permission.DisablePermissionInheritance == false)
                    {
                        if (permission.AppliesTo == DmsAppliesToEnum.ThisFolderSubFoldersAndFiles && permission.Isowner != true)
                        {
                            if (notePermissions.Any(e => e.PermissionType == permission.PermissionType && e.Principal == permission.Principal && e.Access == permission.Access && e.AppliesTo == permission.AppliesTo))
                            {
                                // Do not create or edit the permission  as it already have such permission
                            }
                            else 
                            {
                                var permissionData = new DocumentPermissionViewModel
                                {
                                    DataAction = DataActionEnum.Create,
                                    PermissionType = permission.PermissionType,
                                    Access = permission.Access,
                                    AppliesTo = permission.AppliesTo,
                                    InheritedFrom =permission.IsInherited==true ? permission.InheritedFrom:permission.Id,
                                    PermittedUserId = permission.PermittedUserId,
                                    PermittedUserGroupId = permission.PermittedUserGroupId,
                                    NoteId = noteId,
                                    Isowner = permission.Isowner,
                                    IsInherited = true,
                                };
                                await Create(permissionData);
                            }
                            
                        }
                    }
                }

            }
        }
        public async Task<IList<DocumentPermissionViewModel>> GetNotePermissionDataForUser(string noteId)
        {


            return await _documentManagementQueryBusiness.GetNotePermissionDataForUser(noteId);
        }
        public async Task<IList<DocumentPermissionViewModel>> GetNotePermissionData(string noteId)
        {


            return await _documentManagementQueryBusiness.GetNotePermissionData(noteId);
        }
        public async Task<IList<DocumentPermissionViewModel>> GetNotePermissionDataExceptDeafultPermission(string noteId)
        {


            return await _documentManagementQueryBusiness.GetNotePermissionDataExceptDeafultPermission(noteId);
        }
        public async Task<DocumentPermissionViewModel> GetNotePermissionDataPermissionId(string noteId,string permissionId)
        {


            return await _documentManagementQueryBusiness.GetNotePermissionDataPermissionId(noteId, permissionId);
        }

        public async Task ManageChildPermissions(string noteId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            var documents = await _documentBusiness.GetAllFiles(noteId, null, null);
            var docids = documents.Select(x => x.Id);
            var parentPermissions = await GetNotePermissionData(noteId);          
            var allPermision = new List<DocumentPermissionViewModel>();
            List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            var subfolders = await _documentBusiness.GetAllPermissionChildByParentId(noteId, subfoldersList);
            var ids = subfolders.Select(x => x.Id);
            var noteids = string.Join(",", ids);
            var notePermissions = await _documentBusiness.GetAllNotePermissionByParentId(noteids);
            foreach (var doc in documents)
            {               
                // Create or Edit the Permission for the documents
                var notePermissionscheck = notePermissions.Where(x => x.NoteId == doc.Id).ToList();
                if (doc.DisablePermissionInheritance == null || doc.DisablePermissionInheritance == false)
                {
                    foreach (var permission in parentPermissions.Where(x => x.IsInheritedFromChild == false))
                    {
                        if (permission.AppliesTo != DmsAppliesToEnum.OnlyThisFolder)
                        {
                            if (notePermissionscheck.Any(e => e.PermissionType == permission.PermissionType && e.Principal == permission.Principal && e.Access == permission.Access && e.AppliesTo == permission.AppliesTo 
                            && (permission.IsInherited == true ? e.InheritedFrom == permission.InheritedFrom : true)))
                            {
                                // Do not create or edit the permission  as it already have such permission
                            }
                            else
                            {
                                var existpermission = await GetSingleById(permission.Id);
                                var permissionData = new DocumentPermissionViewModel
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    PermissionType = existpermission.PermissionType,
                                    Access = existpermission.Access,
                                    AppliesTo = existpermission.AppliesTo,
                                    IsInherited = true,
                                    PermittedUserId = existpermission.PermittedUserId,
                                    PermittedUserGroupId = existpermission.PermittedUserGroupId,
                                    NoteId = doc.Id,
                                    InheritedFrom = existpermission.IsInherited==true ? existpermission.InheritedFrom:existpermission.Id,
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
            subfolders = subfolders.Where(x => !docids.Contains(x.Id)).ToList();            
            foreach (var folder in subfolders)
            {
                // Create or edit new permission for the subfolder                
                var notePermissionscheck = notePermissions.Where(x => x.NoteId == folder.Id).ToList();
                if (folder.DisablePermissionInheritance == null || folder.DisablePermissionInheritance == false)
                {
                    foreach (var permission in parentPermissions.Where(x => x.IsInheritedFromChild == false))
                    {
                        if (permission.AppliesTo != DmsAppliesToEnum.OnlyThisFolder && permission.AppliesTo != DmsAppliesToEnum.ThisFolderAndFiles)
                        {
                            if (notePermissionscheck.Any(e => e.PermissionType == permission.PermissionType && e.Principal == permission.Principal && e.Access == permission.Access && e.AppliesTo == permission.AppliesTo 
                            && (permission.IsInherited == true ? e.InheritedFrom == permission.InheritedFrom : true)))
                            {
                                // Do not create as it already have such permission
                            }
                            else
                            {
                                var existpermission = permission;
                                var permissionData = new DocumentPermissionViewModel
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    PermissionType = existpermission.PermissionType,
                                    Access = existpermission.Access,
                                    AppliesTo = existpermission.AppliesTo,
                                    IsInherited = true,
                                    PermittedUserId = existpermission.PermittedUserId,
                                    PermittedUserGroupId = existpermission.PermittedUserGroupId,
                                    NoteId = folder.Id,
                                    InheritedFrom = existpermission.IsInherited == true ? existpermission.InheritedFrom : existpermission.Id,
                                    Isowner = existpermission.Isowner,
                                    DisablePermittedNotification = true,
                                    CompanyId = _userContext.CompanyId,
                                    IsDeleted = false,
                                    CreatedBy = _userContext.UserId,
                                    LastUpdatedBy =_userContext.UserId,
                                    CreatedDate = DateTime.Now,
                                    LastUpdatedDate = DateTime.Now,
                                    LegalEntityId = _userContext.LegalEntityId,
                                    PortalId = _userContext.PortalId,
                                    Status = StatusEnum.Active,
                                    VersionNo =0,
                                };
                            allPermision.Add(permissionData);                           
                            }
                        }
                    }
                }               
            }

            await CreateBulkPermission(allPermision);
        }
        public async Task ManageParentPermissions(string noteId,string PermissionId=null)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();           
            var parents = await _documentBusiness.GetAllParentByNoteId(noteId);
            var allPermision = new List<DocumentPermissionViewModel>();
            var sourcePermissions = new List<DocumentPermissionViewModel>();
            if (PermissionId.IsNotNullAndNotEmpty())
            {
                var permission = await GetNotePermissionDataPermissionId(noteId, PermissionId);
                sourcePermissions.Add(permission);
            }
            else
            {
                var permission = await GetNotePermissionDataExceptDeafultPermission(noteId);
                sourcePermissions.AddRange(permission);
            }
            var ids = parents.Select(x => x.Id);
            var noteids = string.Join(",", ids);
            var parentnotePermissions = await _documentBusiness.GetAllNotePermissionByParentId(noteids);
            foreach (var folder in parents)
            {                               
                var parentnotePermissionscheck = parentnotePermissions.Where(x => x.NoteId == folder.Id);                
                foreach (var permission in sourcePermissions)
                {
                    if (!parentnotePermissionscheck.Any(e => e.PermissionType == permission.PermissionType && e.Principal == permission.Principal && e.Access == permission.Access && e.AppliesTo == permission.AppliesTo 
                    && (permission.IsInheritedFromChild==true?e.InheritedFrom == permission.InheritedFrom:e.InheritedFrom == permission.Id)))
                        {
                            var existpermission = sourcePermissions.Where(x=>x.Id==permission.Id).FirstOrDefault();
                            var permissionData = new DocumentPermissionViewModel
                            {
                                Id = Guid.NewGuid().ToString(),
                                PermissionType = existpermission.PermissionType,
                                Access = DmsAccessEnum.ReadOnly,
                                AppliesTo = DmsAppliesToEnum.OnlyThisFolder,
                                IsInherited = false,
                                IsInheritedFromChild = true,
                                PermittedUserId = existpermission.PermittedUserId,
                                PermittedUserGroupId = existpermission.PermittedUserGroupId,
                                NoteId = folder.Id,
                                InheritedFrom = existpermission.IsInheritedFromChild == true ? existpermission.InheritedFrom : existpermission.Id,
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
            await CreateBulkPermission(allPermision);
        }
        public async Task CreateBulkPermission(List<DocumentPermissionViewModel> permissions)
        {
            await _documentManagementQueryBusiness.CreateBulkPermission(permissions);
        }
        public async Task UpdateBulkPermission(List<DocumentPermissionViewModel> permissions)
        {
            await _documentManagementQueryBusiness.UpdateBulkPermission(permissions);
        }
        public async Task DeleteInheritedPermissionFromChildOnPermissionDelete(string noteId, string PermissionId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            var documents = await _documentBusiness.GetAllFiles(noteId, null, null);
            foreach (var doc in documents)
            {
                var notePermissions = await GetNotePermissionData(doc.Id);
                //  Delete Inherited Permission
                if (doc.DisablePermissionInheritance == null || doc.DisablePermissionInheritance == false)
                {
                    foreach (var permission in notePermissions.Where(x => x.InheritedFrom == PermissionId))
                    {
                       
                        await Delete(permission.Id);
                    }
                }
            }
            List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            var subfolders = await _documentBusiness.GetAllChildByParentId(noteId, subfoldersList);

            foreach (var folder in subfolders)
            {
                var notePermissions = await GetNotePermissionData(folder.Id);
                if (folder.DisablePermissionInheritance == null || folder.DisablePermissionInheritance == false)
                {
                    var permissions = notePermissions.Where(x => x.InheritedFrom == PermissionId).ToList();
                    foreach (var permission in permissions)
                    {
                        await Delete(permission.Id);
                        await DeleteInheritedPermissionFromChildOnPermissionDelete(folder.Id, permission.Id);
                    }
                 
                }
                //await DeleteInheritedPermissionFromChildOnPermissionDelete(folder.Id, PermissionId);
            }
        }
        public async Task DeleteInheritedPermissionFromParentOnPermissionDelete(string noteId, string PermissionId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();           
            List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            var subfolders = await _documentBusiness.GetAllParentByChildId(noteId, subfoldersList);

            foreach (var folder in subfolders)
            {
                var notePermissions = await GetNotePermissionData(folder.Id);
               // if (folder.DisablePermissionInheritance == null || folder.DisablePermissionInheritance == false)
               // {
                    var permissions = notePermissions.Where(x => x.InheritedFrom == PermissionId && x.IsInheritedFromChild == true).ToList();
                    foreach (var permission in permissions)
                    {
                        await Delete(permission.Id);
                        await DeleteInheritedPermissionFromParentOnPermissionDelete(folder.Id, PermissionId);
                    }

                //}
                //await DeleteInheritedPermissionFromChildOnPermissionDelete(folder.Id, PermissionId);
            }
        }
        //public async Task ManageChildPermissionsOnEdit(string noteId,string PermissionId)
        //{
        //    var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
        //    var documents = await _documentBusiness.GetAllFiles(noteId, null, null);
        //    foreach (var doc in documents)
        //    {
        //        var parentPermission = await GetSingleById(PermissionId);
        //        //var parentPermissions = await GetNotePermissionData(noteId);
        //        var notePermissions = await GetNotePermissionData(doc.Id);
        //        //  Edit the Permission for the documents
        //        if (doc.DisablePermissionInheritance == null || doc.DisablePermissionInheritance == false)
        //        {
        //            foreach (var permission in notePermissions.Where(x=>x.InheritedFrom== PermissionId))
        //            {
        //                var notePermission = await GetSingleById(permission.Id);
        //                notePermission.PermissionType = parentPermission.PermissionType;
        //                notePermission.Access = parentPermission.Access;
        //                notePermission.AppliesTo = parentPermission.AppliesTo;
        //                notePermission.PermittedUserId = parentPermission.PermittedUserId;
        //                notePermission.PermittedUserGroupId = parentPermission.PermittedUserGroupId;
        //                notePermission.InheritedFrom = parentPermission.Id;                       
        //                await Edit(notePermission);
        //                //bulk update
        //            }                   
        //        }
        //    }
        //    List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
        //    var subfolders = await _documentBusiness.GetAllChildByParentId(noteId, subfoldersList);
        //   //get all folder and permission in bulk
        //    foreach (var folder in subfolders)
        //    {
        //        // edit new permission for the subfolder 
        //        var parentPermission = await GetSingleById(PermissionId);
        //        //var parentPermissions = await GetNotePermissionData(noteId);
        //        var notePermissions = await GetNotePermissionData(folder.Id);
        //        if (folder.DisablePermissionInheritance == null || folder.DisablePermissionInheritance == false)
        //        {                    
        //            foreach (var permission in notePermissions.Where(x => x.InheritedFrom == PermissionId))
        //            {                        
        //                var notePermission = await GetSingleById(permission.Id);
        //                notePermission.PermissionType = parentPermission.PermissionType;
        //                notePermission.Access = parentPermission.Access;
        //                notePermission.AppliesTo = parentPermission.AppliesTo;
        //                notePermission.PermittedUserId = parentPermission.PermittedUserId;
        //                notePermission.PermittedUserGroupId = parentPermission.PermittedUserGroupId;
        //                notePermission.InheritedFrom = parentPermission.Id;
        //                await Edit(notePermission);
        //                await ManageChildPermissionsOnEdit(folder.Id, permission.Id);
        //            }
        //            var folderPermission = await GetSingle(x => x.InheritedFrom == PermissionId);
        //            if (folderPermission==null) 
        //            {
        //                await ManageChildPermissions(folder.ParentId);
        //                await ManageParentPermissions(folder.ParentId,PermissionId);
        //            }
        //        }
               
        //    }
        //}
        public async Task ManageChildPermissionsOnEdit(string noteId, string PermissionId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();            
            var allPermision = new List<DocumentPermissionViewModel>();            
            var allChildFoldersAndDocuments = await _documentBusiness.GetAllPermissionChildByParentId(noteId, new List<FolderViewModel>());
            var ids = allChildFoldersAndDocuments.Select(x => x.Id).ToList();
            ids.Add(noteId);
            var noteids = string.Join(",", ids);
            var notePermissions = await _documentBusiness.GetAllNotePermissionByParentId(noteids);
            var parentPermission = notePermissions.ToList().Where(x => x.Id == PermissionId).FirstOrDefault();
            var childnotePermissions = notePermissions.ToList().Where(x => x.InheritedFrom == PermissionId).ToList(); 
            foreach (var notePermission in childnotePermissions)
            {  
                notePermission.PermissionType = parentPermission.PermissionType;
                notePermission.Access = parentPermission.Access;
                notePermission.AppliesTo = parentPermission.AppliesTo;
                notePermission.PermittedUserId = parentPermission.PermittedUserId;
                notePermission.PermittedUserGroupId = parentPermission.PermittedUserGroupId;
                notePermission.InheritedFrom = parentPermission.Id;
                notePermission.LastUpdatedBy = _userContext.UserId;
                notePermission.LastUpdatedDate = DateTime.Now;
                allPermision.Add(notePermission);
            }
            if (allPermision.Count > 0)
            {
                await UpdateBulkPermission(allPermision);
            }
        }
        public async Task ManageParentPermissionsOnEdit(string noteId, string PermissionId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            var parents = await _documentBusiness.GetAllParentByNoteId(noteId);
            var allPermision = new List<DocumentPermissionViewModel>();
            var ids = parents.Select(x => x.Id).ToList();
            ids.Add(noteId);
            var noteIds = string.Join(",", ids);
            var notePermissions = await _documentBusiness.GetAllNotePermissionByParentId(noteIds);
            var childPermission = notePermissions.ToList().Where(x => x.Id == PermissionId).FirstOrDefault();
            var childnotePermissions = notePermissions.ToList().Where(x => x.InheritedFrom == PermissionId && x.IsInheritedFromChild==true).ToList();            
            foreach (var permission in childnotePermissions)
            {
                var notePermission = permission;
                notePermission.PermissionType = childPermission.PermissionType;
                notePermission.Access = DmsAccessEnum.ReadOnly;
                notePermission.AppliesTo = DmsAppliesToEnum.OnlyThisFolder;
                notePermission.PermittedUserId = childPermission.PermittedUserId;
                notePermission.PermittedUserGroupId = childPermission.PermittedUserGroupId;                
                notePermission.InheritedFrom = childPermission.Id;
                notePermission.LastUpdatedBy = _userContext.UserId;
                notePermission.LastUpdatedDate = DateTime.Now;
                allPermision.Add(notePermission);                
            }
            if (allPermision.Count > 0)
            {
                await UpdateBulkPermission(allPermision);
            }
        }
        public async Task DeleteChildPermissions(string noteId, IList<DocumentPermissionViewModel> parentnotePermissions, string PermissionId=null)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            var allPermision = new List<DocumentPermissionViewModel>();
            var allChildFoldersAndDocuments = await _documentBusiness.GetAllPermissionChildByParentId(noteId, new List<FolderViewModel>());
            var ids = allChildFoldersAndDocuments.Select(x => x.Id).ToList();
            ids.Add(noteId);
            var noteids = string.Join(",", ids);
            var notePermissions = await _documentBusiness.GetAllNotePermissionByParentId(noteids);
            if (PermissionId.IsNotNullAndNotEmpty())
            {
                var childnotePermissions = notePermissions.ToList().Where(x => x.InheritedFrom == PermissionId).ToList();
                foreach (var notePermission in childnotePermissions)
                {
                    notePermission.IsDeleted = true;
                    notePermission.LastUpdatedBy = _userContext.UserId;
                    notePermission.LastUpdatedDate = DateTime.Now;
                    allPermision.Add(notePermission);
                }
            }
            else
            {
                //delete existing parent permission
                foreach (var parentnotePermission in parentnotePermissions.Where(x => x.IsInheritedFromChild == true))
                {
                    if (notePermissions.Any(x => x.Id == parentnotePermission.InheritedFrom))
                    {
                        parentnotePermission.IsDeleted = true;
                        parentnotePermission.LastUpdatedBy = _userContext.UserId;
                        parentnotePermission.LastUpdatedDate = DateTime.Now;
                        allPermision.Add(parentnotePermission);
                    }

                }
                //delete child permission inherited from source
                var sourceInheritedPermission= notePermissions.ToList().Where(x => x.IsInherited == true && x.NoteId==noteId).ToList();
                var sourcePermissionInheritedFromIds = sourceInheritedPermission.Select(x => x.InheritedFrom).ToList();
                var childnotePermissions = notePermissions.ToList().Where(x => x.IsInherited == true && sourcePermissionInheritedFromIds.Contains(x.InheritedFrom)).ToList();
                foreach (var notePermission in childnotePermissions)
                {
                    notePermission.IsDeleted = true;
                    notePermission.LastUpdatedBy = _userContext.UserId;
                    notePermission.LastUpdatedDate = DateTime.Now;
                    allPermision.Add(notePermission);
                }
            }            
            if (allPermision.Count > 0)
            {
                await UpdateBulkPermission(allPermision);
            }
        }
        public async Task DeleteParentPermissions(string noteId, string PermissionId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            var parents = await _documentBusiness.GetAllParentByNoteId(noteId);
            var allPermision = new List<DocumentPermissionViewModel>();
            var ids = parents.Select(x => x.Id).ToList();
            ids.Add(noteId);
            var noteIds = string.Join(",", ids);
            var notePermissions = await _documentBusiness.GetAllNotePermissionByParentId(noteIds);            
            var childnotePermissions = notePermissions.ToList().Where(x => x.InheritedFrom == PermissionId && x.IsInheritedFromChild == true).ToList();
            foreach (var permission in childnotePermissions)
            {
                var notePermission = permission;
                notePermission.IsDeleted = true;
                notePermission.LastUpdatedBy = _userContext.UserId;
                notePermission.LastUpdatedDate = DateTime.Now;
                allPermision.Add(notePermission);
            }
            if (allPermision.Count > 0)
            {
                await UpdateBulkPermission(allPermision);
            }
        }
        //public async Task ManageParentPermissionsOnEdit(string noteId, string PermissionId)
        //{
        //    var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();

        //    List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
        //    var subfolders = await _documentBusiness.GetAllParentByChildId(noteId, subfoldersList);

        //    foreach (var folder in subfolders)
        //    {
        //        // edit new permission for the parent 
        //        var childPermission = await GetSingleById(PermissionId);              
        //        var parentPermissions = await GetNotePermissionData(folder.Id);
        //       // if (folder.DisablePermissionInheritance == null || folder.DisablePermissionInheritance == false)
        //       // {
        //            foreach (var permission in parentPermissions.Where(x => x.InheritedFrom == PermissionId && x.IsInheritedFromChild==true))
        //            {
        //                var notePermission = await GetSingleById(permission.Id);
        //                notePermission.PermissionType = childPermission.PermissionType;
        //                notePermission.Access = DmsAccessEnum.ReadOnly;//parentPermission.Access;
        //                notePermission.AppliesTo = DmsAppliesToEnum.OnlyThisFolder;
        //                notePermission.PermittedUserId = childPermission.PermittedUserId;
        //                notePermission.PermittedUserGroupId = childPermission.PermittedUserGroupId;
        //                notePermission.IsInheritedFromChild = true;
        //                notePermission.InheritedFrom = childPermission.Id;
        //            await Edit(notePermission);
        //            }
        //        //var newPermissionId = parentPermissions.Where(x => x.InheritedFrom == PermissionId && x.IsInheritedFromChild == true).Select(x => x.Id).FirstOrDefault();
        //       // }
        //        await ManageParentPermissionsOnEdit(folder.Id, PermissionId);
        //    }
        //}
        public async Task DeleteOldParentPermission(string noteId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            // get all inherited permission of folder and delete
            var notePermissions = await GetNotePermissionData(noteId);
            foreach (var permission in notePermissions)
            {
                if (permission.IsInherited == true)
                {
                    // Delete Permission
                    await Delete(permission.Id);
                }

            }
            // get all files and delete the inherited permissions
            //var documents = await _documentBusiness.GetAllFiles(noteId, null, null);
            //foreach (var doc in documents)
            //{
            //    var docPermissions = await GetNotePermissionData(doc.Id);                
            //    foreach (var permission in docPermissions)
            //    {
            //        if (permission.IsInherited == true)
            //        {
            //            await Delete(permission.Id);
            //            //_repository.RemoveRelationShip<NTS_NotePermission, R_NotePermission_Note, NTS_Note>(permission.Id, doc.Id, false);
            //            //_repository.Commit();
            //        }
            //    }
            //}
            //// get list of sub folders and delete inherited permission
            //List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            //var subfolders =await  _documentBusiness.GetAllChildByParentId(noteId, subfoldersList);
            ////var subfolders = _notebusiness.GetAllFolderByParentId(noteId, subfoldersList);
            //foreach (var folder in subfolders)
            //{
            //    await DeleteOldParentPermission(folder.Id);
            //}
        }
        //public async Task RemoveChildPermission(string noteId,string removedNoteId)
        //{
        //    var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
        //    // get all  permission of removed child
        //    var childPermissions = await GetNotePermissionData(noteId);
        //    // get list of direct parent folders or workspace and delete inherited child permission
        //    List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
        //    var subfolders = await _documentBusiness.GetAllParentByChildId(noteId, subfoldersList);           
        //    foreach (var folder in subfolders)
        //    {
        //        var notePermissions = await GetNotePermissionData(folder.Id);
        //        foreach (var permission in notePermissions)
        //        {
        //            // check if the permission inherited from child is present in parent or not
        //            if (permission.IsInheritedFromChild == true && childPermissions.Any(x=>x.Id== permission.InheritedFrom))
        //            {
        //                // Delete Permission
        //                await Delete(permission.Id);
        //            }
        //        }
        //        await RemoveChildPermission(folder.Id);
        //    }
        //}
        public async Task RemoveChildPermission(string noteId, string removedNoteId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            // get all  permission of removed child
            var childPermissions = await GetNotePermissionData(noteId);
            var notePermissions = await GetNotePermissionData(removedNoteId);
            foreach (var permission in childPermissions)
            {
                // check if the permission inherited from child is present in parent or not
                if (permission.IsInheritedFromChild == true && notePermissions.Any(x => x.Id == permission.InheritedFrom))
                {
                    // Delete Permission
                    await Delete(permission.Id);
                }
            }
            // get list of direct parent folders or workspace and delete inherited child permission
            List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            var subfolders = await _documentBusiness.GetAllParentByChildId(noteId, subfoldersList);
            foreach (var folder in subfolders)
            {
                //var notePermissions = await GetNotePermissionData(folder.Id);
             
                await RemoveChildPermission(folder.Id, removedNoteId);
            }
        }
        public async Task<IList<DocumentPermissionViewModel>> GetParentPermissionData(string noteId)
        {
            IList<DocumentPermissionViewModel> PermissionList = new List<DocumentPermissionViewModel>();
            List<FolderViewModel> list = new List<FolderViewModel>();
            var parentLIst=await GetAllParents(noteId, list);
            //var cypher = string.Concat(@"match (n:NTS_Note {Id: {noteId}})-[:R_Note_Parent_Note*1..]->(p:NTS_Note) return n.Id as Id, n.Subject as Name,p.Id as ParentId,p.Subject as ParentName, p.DisablePermissionInheritance as DisablePermissionInheritance");
            // var prms = new Dictionary<string, object> { { "noteId", noteId } };
            //var parentLIst =await _queryRepo.ExecuteQueryList<FolderViewModel>(cypher, null);
            if (parentLIst != null && parentLIst.Count() > 0)
            {
                foreach (var parent in parentLIst)
                {
                  
                    IList<DocumentPermissionViewModel> ParentPermissionList = new List<DocumentPermissionViewModel>();
                    ParentPermissionList =await GetNotePermissionData(parent.ParentId);
                    if (ParentPermissionList != null && ParentPermissionList.Count() > 0)
                    {
                        foreach (var permission in ParentPermissionList)
                        {                            
                            PermissionList.Add(permission);
                        }
                    }
                    if (parent.DisablePermissionInheritance.HasValue && parent.DisablePermissionInheritance.Value == true)
                    {
                        break;
                    }
                }
            }
            return PermissionList;
        }
        public async Task DisableParentPermissions(string noteId, string InheritanceStatus)
        {
            var _noteBusiness = _sp.GetService<INoteBusiness>();
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            var allPermision = new List<DocumentPermissionViewModel>();
            var alldocPermision = new List<DocumentPermissionViewModel>();
            var Note = await _noteBusiness.GetSingleById(noteId);
            //direct level child documents
            var documents = await _documentBusiness.GetAllFiles(noteId, null, null);
            var docids = documents.Select(x => x.Id).ToList();
            docids.Add(noteId);
            var noteids = string.Join(",", docids);
            var notePermissions = await _documentBusiness.GetAllNotePermissionByParentId(noteids);
            if (InheritanceStatus.ToSafeBool() == false)
            { 
                if (Note.ParentNoteId.IsNotNull())
                {                    
                    var parentPermissions =await GetNotePermissionData(Note.ParentNoteId);                                      
                    // Create or Edit the Permission for the documents
                    var notePermissionscheck = notePermissions.Where(x => x.NoteId == noteId).ToList();
                    foreach (var permission in parentPermissions.Where(x=>x.IsInheritedFromChild==false))
                    {
                        if (permission.AppliesTo != DmsAppliesToEnum.OnlyThisFolder)
                        {                            
                            if (!notePermissionscheck.Any(e => e.PermissionType == permission.PermissionType && e.Principal == permission.Principal && e.Access == permission.Access && e.AppliesTo == permission.AppliesTo))
                            {
                                var existpermission = permission;
                                //var permissionData = new DocumentPermissionViewModel
                                //{

                                //    PermissionType = existpermission.PermissionType,
                                //    Access = existpermission.Access,
                                //    AppliesTo = existpermission.AppliesTo,
                                //    IsInherited = true,
                                //    PermittedUserId = existpermission.PermittedUserId,
                                //    NoteId = noteId,
                                //    InheritedFrom = existpermission.InheritedFrom == null ? existpermission.Id : existpermission.InheritedFrom,
                                //    Isowner = existpermission.Isowner,
                                //};
                                //await Create(permissionData);
                                var permissionData = new DocumentPermissionViewModel
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    PermissionType = existpermission.PermissionType,
                                    Access = existpermission.Access,
                                    AppliesTo = existpermission.AppliesTo,
                                    IsInherited = true,
                                    PermittedUserId = existpermission.PermittedUserId,
                                    PermittedUserGroupId = existpermission.PermittedUserGroupId,
                                    NoteId = noteId,
                                    InheritedFrom = existpermission.IsInherited == true ? existpermission.InheritedFrom : existpermission.Id,
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
                    if (allPermision.Count > 0)
                    {
                        await CreateBulkPermission(allPermision);
                    }
                    // Create or Edit the Permission for direct level child documents
                    var parentPermissionsfordoc = await GetNotePermissionData(noteId);
                    foreach (var doc in documents)
                    {                        
                        var docPermissionscheck = notePermissions.Where(x => x.NoteId == doc.Id).ToList();
                        if (doc.DisablePermissionInheritance == null || doc.DisablePermissionInheritance == false)
                        {
                            foreach (var permission in parentPermissionsfordoc.Where(x => x.IsInheritedFromChild == false))
                            {
                                if (permission.AppliesTo != DmsAppliesToEnum.OnlyThisFolder)
                                {
                                    if (!docPermissionscheck.Any(e => e.PermissionType == permission.PermissionType && e.Principal == permission.Principal && e.Access == permission.Access && e.AppliesTo == permission.AppliesTo))
                                    {
                                        //var existpermission = await GetSingleById(permission.Id);
                                        var existpermission = permission;
                                        var permissionData = new DocumentPermissionViewModel
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            PermissionType = existpermission.PermissionType,
                                            Access = existpermission.Access,
                                            AppliesTo = existpermission.AppliesTo,
                                            IsInherited = true,
                                            PermittedUserId = existpermission.PermittedUserId,
                                            PermittedUserGroupId = existpermission.PermittedUserGroupId,
                                            NoteId = doc.Id,
                                            InheritedFrom = existpermission.IsInherited == true ? existpermission.InheritedFrom : existpermission.Id,
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
                                        alldocPermision.Add(permissionData);
                                    }
                                }
                            }
                        }
                    }
                    if (alldocPermision.Count>0)
                    {
                        await CreateBulkPermission(alldocPermision);
                    }
                    Note.DisablePermissionInheritance = false;
                    Note.LastUpdatedBy = _userContext.UserId;
                    Note.LastUpdatedDate = DateTime.Now;
                    await _noteBusiness.Edit(Note);
                }
                else
                {
                    Note.DisablePermissionInheritance = false;
                    Note.LastUpdatedBy = _userContext.UserId;
                    Note.LastUpdatedDate = DateTime.Now;
                    await _noteBusiness.Edit(Note);
                }
            }
            else
            {               
                var inheritedPermissions = notePermissions.Where(x => x.IsInherited == true).ToList();
                foreach (var permission in inheritedPermissions)
                {
                    permission.IsDeleted = true;
                    permission.LastUpdatedBy = _userContext.UserId;
                    permission.LastUpdatedDate = DateTime.Now;
                    allPermision.Add(permission);
                }
                if (allPermision.Count > 0)
                {
                    await UpdateBulkPermission(allPermision);
                }
                Note.DisablePermissionInheritance = true;
                Note.LastUpdatedBy = _userContext.UserId;
                Note.LastUpdatedDate = DateTime.Now;
                await _noteBusiness.Edit(Note);
              
            }

        }
        public async Task<IList<FolderViewModel>> GetAllParents(string noteId, List<FolderViewModel> parentList)
        {


            var list = await _documentManagementQueryBusiness.GetAllParents(noteId, parentList);
            if (list!=null && list.ParentId!=null) 
            {
                parentList.Add(list);
                await GetAllParents(list.ParentId, parentList);
            }
            return parentList;
        }

        public async Task<List<DMSCalenderViewModel>> GetCalenderDetails(string UserdId)
        {



            var resuly = await _documentManagementQueryBusiness.GetCalenderDetails(UserdId);
           // foreach (var item in resuly)
           // {
           //
           //     item.End =DateTime.Now;
           //     item.DueDate =DateTime.Now;
           //         
           //
           // }

            return resuly;


        }



        public async Task<List<NoteLinkShareViewModel>> GetDocumentLinksData(string id)
        {

            var result = await _documentManagementQueryBusiness.GetDocumentLinksData(id);

            return result;
        }


        public async Task<NoteLinkShareViewModel> GetNoteDocumentByKey(string Key)
        {


            var result = await _documentManagementQueryBusiness.GetNoteDocumentByKey(Key);

            return result;
        }

        public async Task<DMSDocumentViewModel> GetFileId(string Id)
        {


            var result = await _documentManagementQueryBusiness.GetFileId(Id);

            return result;
        }
        

        
        public async Task<bool> DeleteLink(string Id)
        {

            await _documentManagementQueryBusiness.DeleteLink(Id);
            return true;

        }
        public async Task<WorkspaceViewModel> GetLegalEntity(string parentId)
        {

            var queryData = await _documentManagementQueryBusiness.GetLegalEntity(parentId);
            var list = queryData;
            return list;
        }
        public async Task<bool> DeletePermissionByDocumentIds(string ids)
        {

            var result = await _documentManagementQueryBusiness.DeletePermissionByDocumentIds(ids);
            return true;
        }
    }
    }
