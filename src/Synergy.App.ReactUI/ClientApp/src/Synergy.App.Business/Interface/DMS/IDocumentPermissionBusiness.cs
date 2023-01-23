using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
  public  interface IDocumentPermissionBusiness : IBusinessBase<DocumentPermissionViewModel, DocumentPermission>
    {
        Task<List<DocumentPermissionViewModel>> GetPermissionList(string noteId);        
        Task<List<TemplateViewModel>> GetTemplateList(string parentId);
        Task<List<DMSDocumentViewModel>> GetArchive(string UserID);

        Task<List<DMSDocumentViewModel>> GetBinDocumentData(string UserID);
        Task<CommandResult<WorkspaceViewModel>> ValidateSequenceOrder(WorkspaceViewModel model);
        Task<bool> DeleteDocument(string NoteId);

        Task<bool> RestoreBinDocument(string NoteId);
        Task<bool> RestoreArchiveDocument(string Id, string TableMetadataid);
        Task<List<WorkspaceViewModel>> GetWorkspaceList();
        Task<List<DocumentPermissionViewModel>> ViewPermissionList(string NoteId);
        Task<bool> DeleteWorkspace(string NoteId);
        Task<IList<DocumentPermissionViewModel>> GetNotePermissionData(string noteId);
        Task<IList<DocumentPermissionViewModel>> GetNotePermissionDataExceptDeafultPermission(string noteId);
        Task<DocumentPermissionViewModel> GetNotePermissionDataPermissionId(string noteId, string permissionId);
        Task<IList<DocumentPermissionViewModel>> GetNotePermissionDataForUser(string noteId);
        Task ManageInheritedPermission(string noteId, string parentId);
         Task ManageChildPermissions(string noteId);
        Task ManageParentPermissions(string noteId,string PermissionId=null);
         Task DeleteOldParentPermission(string noteId);
        Task RemoveChildPermission(string noteId,string removedNoteId);
        Task<List<WorkspaceViewModel>> DocumentTemplateList(string NoteId);
        Task ManageChildPermissionsOnEdit(string noteId, string PermissionId);
        Task ManageParentPermissionsOnEdit(string noteId, string PermissionId);
        Task DeleteInheritedPermissionFromChildOnPermissionDelete(string noteId, string PermissionId);
        Task DeleteInheritedPermissionFromParentOnPermissionDelete(string noteId, string PermissionId);
        Task DisableParentPermissions(string noteId, string InheritanceStatus);
        Task<List<DMSCalenderViewModel>> GetCalenderDetails(string UserdId);
        Task<CommandResult<WorkspaceViewModel>> ValidateWorkspace(WorkspaceViewModel model);
        Task<WorkspaceViewModel> GetWorkspaceEdit(string workspaceId);
        Task<List<DMSDocumentViewModel>> GetFolderByParent(string ParentId);

        Task<List<NoteLinkShareViewModel>> GetDocumentLinksData(string id);
        Task<NoteLinkShareViewModel> GetNoteDocumentByKey(string Key);

         Task<DMSDocumentViewModel> GetFileId(string Id);

        Task<bool> DeleteLink(string Id);
        Task<WorkspaceViewModel> GetLegalEntity(string parentId);
        Task SendNotification(DocumentPermissionViewModel model);
        Task CreateBulkPermission(List<DocumentPermissionViewModel> permissions);
        Task DeleteChildPermissions(string noteId, IList<DocumentPermissionViewModel> parentnotePermissions, string PermissionId=null);
        Task DeleteParentPermissions(string noteId, string PermissionId);
        Task<bool> DeletePermissionByDocumentIds(string ids);
    }
}
