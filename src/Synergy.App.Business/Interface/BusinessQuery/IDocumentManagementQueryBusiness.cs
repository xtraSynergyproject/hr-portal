using Synergy.App.ViewModel;
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
    public interface IDocumentManagementQueryBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        #region DMSDocumentBusiness

        // put dms doc business defns
        Task<List<FolderAndDocumentViewModel>> GetAllFolderAndDocumentByUserId(string userId);
        Task<List<WorkspaceViewModel>> GetDocuments(string userId, string search);
        Task<string> GetDocumentsQueryByParentFolderIdNew(string parentId, string UserId, string id, string udfs);
        Task<string> GetFoldersAndDocumentsQueryNew(DocumentQueryTypeEnum documentQueryType, string UserId, string parentId);
        //Task<string> GetFoldersAndDocumentsQueryNew(string UserId, string parentId, string udfs);
        Task<List<FolderViewModel>> GetAllByParent(string UserId, string parentId, string udfs);
        Task<List<FolderViewModel>> GetFirstLevelWorkspacesByUser(string UserId);
        Task<List<FolderViewModel>> GetAllChildWorkspaceAndFolder(string UserId, string parentId, string udfs);
        Task<List<FolderViewModel>> GetAllChildWorkspaceFolderAndDocument(string UserId, string parentId, string udfs);

        Task<List<FolderViewModel>> GetAllChildWorkspaceFolderAndFiles(string UserId, string parentId, string udfs);
        Task<List<FolderViewModel>> GetAllGeneralWorkspaceData();
        Task<List<FolderViewModel>> GetDocumentVersions(string noteId);
        Task<List<FolderViewModel>> GetAllChildbyParent(string parentId);
        Task<List<FolderViewModel>> CheckDocumentExist(string parentId);
        Task<List<string>> GetAllDocumentsDMSReportByRevesion(string userId);
        Task<IList<DocumentListViewModel>> DocumentSubmittedReportData(string documentIds, string discipline, string revesion);
        Task<IList<DocumentListViewModel>> DocumentSubmittedReportData(string documentIds, string discipline, string revesion, string udfs);
        Task<IList<DocumentListViewModel>> DocumentSubmittedReport(string documentIds, string discipline, string revesion, string udfs);

        Task<List<DocumentListViewModel>> DocumentReceived(string documentIds , string discipline, string revesion);
        Task<List<DocumentListViewModel>> DocumentReceivedData(string documentIds, string discipline, string revesion);
        Task<List<DocumentListViewModel>> DocumentReceivedReport(string documentIds, string discipline, string revesion);
        Task<List<DocumentListViewModel>> DocumentReceivedReportData(string documentIds, string discipline, string revesion);

        //Task<IList<DocumentListViewModel>> DocumentStage( string userId); DocumentStageReportData
        Task<List<DocumentListViewModel>> DocumentStage(IList<TemplateViewModel> templateList, string discipline, bool IsOverdue, string userId);
        Task<List<DocumentListViewModel>> DocumentStageData(IList<TemplateViewModel> templateList, string discipline, bool IsOverdue, string userId);
        Task<List<DocumentListViewModel>> DocumentStageReport(IList<TemplateViewModel> templateList, string discipline, bool IsOverdue, string userId);
        Task<List<DocumentListViewModel>> DocumentDataStageReportData(IList<TemplateViewModel> templateList, string discipline, bool IsOverdue, string userId);
        Task<List<DocumentListViewModel>> DocumentStageReportData(IList<TemplateViewModel> templateList, string discipline, bool IsOverdue, string userId);

        Task<List<DocumentListViewModel>> GetAllDocumentsDMSReport(string userId);

        Task<List<DocumentListViewModel>> DocumentReceivedCommentsReportData(List<TemplateViewModel> templateList, string documentIds, string discipline, string revesion);
        Task<string> ValidateRequestForInspection(string startstr);
        Task<string> ValidateRequestForInspectionHalul(string discipline);
        Task<string> ValidateCustomeNotNoRFI(string _firstStr, string _lastStr);
        Task<string> ValidateCustomeNotNoRFIHalul(string _lastStr, string discipline);
        Task<IList<NoteViewModel>> GetAllFiles(string ParentId, string code, string Id);
        Task<List<FolderViewModel>> GetAllParentByChildId(string ChildId, List<FolderViewModel> ParentList);
        Task<List<FolderViewModel>> GetAllChildByParentId(string ParentId, List<FolderViewModel> FolderList);
        Task<IList<FolderViewModel>> GetAllPermissionChildByParentId(string ParentId);
        Task<IList<FolderViewModel>> GetAllPermissionChildByParentIdData(string ParentId);

        Task<List<FolderViewModel>> GetAllPermittedChildByParentId(string UserId, string ParentId);
        Task<List<FolderViewModel>> GetAllPermittedChildByParentIdData(string UserId, string ParentId);

        Task<List<DocumentPermissionViewModel>> GetAllNotePermissionByParentId(string id);
        Task<List<FolderViewModel>> GetAllChildDocumentByParentId(string ParentId);
        Task<List<NoteTemplateViewModel>> GetAllPermittedDocumentOfLoggedInUser(string userId);
        Task<List<AttachmentViewModel>> GetUserAttachments(string userId, string portalId);
        Task<ServiceViewModel> GetWorflowDetailByDocument(string noteId, string udfs);
        Task<IList<FolderViewModel>> GetFolderByParent(string parentId, string noteId);
        Task<List<NoteLinkShareViewModel>> GetDocumentLinksData(long id);
        Task<string> IsUniqueGeneralDocumentByNo(string ParentId, string code, string Id);
        Task<List<NoteViewModel>> IsUniqueDocumentFolder(string ParentId, string code);
        Task<List<NoteViewModel>> IsUniqueGeneralDocument(string ParentId, string code);
        Task<IList<FolderViewModel>> GetAllParentByNoteId(string noteId);
        Task<IList<FolderViewModel>> GetAllDocuments();
        Task<FolderViewModel> UpdateTagsByDocumentIds(string ids);

        Task<FolderViewModel> DeleteNotesbyNoteIds(string ids);
        Task<FolderViewModel> ArchiveNotesbyNoteIds(string ids);
        Task<FolderViewModel> CheckMyWorkspaceExist(string UserId);

        Task<NoteTemplateViewModel> CheckEmployeeBook(string empId);
        Task<List<FolderViewModel>> GetAllChildFiles(string UserId, string parentId ,string udfs);
        Task<List<FolderViewModel>> GetAllPermittedWorkspaceFolderAndDocument(string UserId);
        Task<List<DocumentSearchViewModel>> GetAllWorkspaceFolderDocuments(DateTime? lastUpdatedDate);



            #endregion


        #region DocumentPermissionBusiness

        // put doc permission business defns
        Task<List<DocumentPermissionViewModel>> GetPermissionList(string noteId);
        Task<List<TemplateViewModel>> GetTemplateList(string parentId);
        Task<List<DMSDocumentViewModel>> GetArchive(string UserID);
        Task<List<DMSDocumentViewModel>> GetFolderByParent(string ParentId);
        Task<List<DMSDocumentViewModel>> GetBinDocumentData(string UserID);
        Task DeleteDocument(string NoteId);
        Task<List<WorkspaceViewModel>> ValidateSequenceOrder(WorkspaceViewModel model);
        Task<List<WorkspaceViewModel>> ValidateWorkspace(WorkspaceViewModel model);
        Task<List<WorkspaceViewModel>> GetWorkspaceList();
        Task<WorkspaceViewModel> GetWorkspaceEdit(string workspaceId);
        Task<List<WorkspaceViewModel>> DocumentTemplateList(string workspaceId);
        Task RestoreBinDocument(string NoteId);
        Task RestoreArchiveDocument(string Id, string TableMetadataid);
        Task<List<DocumentPermissionViewModel>> ViewPermissionList(string NoteId);
        Task DeleteWorkspace(string NoteId);
        Task<IList<DocumentPermissionViewModel>> GetNotePermissionDataForUser(string noteId);
        Task<IList<DocumentPermissionViewModel>> GetNotePermissionData(string noteId);
        Task<IList<DocumentPermissionViewModel>> GetNotePermissionDataExceptDeafultPermission(string noteId);
        Task<DocumentPermissionViewModel> GetNotePermissionDataPermissionId(string noteId, string permissionId);
        Task<FolderViewModel> GetAllParents(string noteId, List<FolderViewModel> parentList);
        Task<List<DMSCalenderViewModel>> GetCalenderDetails(string UserdId);
        Task<List<NoteLinkShareViewModel>> GetDocumentLinksData(string id);
        Task<NoteLinkShareViewModel> GetNoteDocumentByKey(string Key);
        Task DeleteLink(string Id);
        Task<WorkspaceViewModel> GetLegalEntity(string parentId);
        Task<TemplateViewModel> DeletePermissionByDocumentIds(string ids);
        Task CreateBulkPermission(List<DocumentPermissionViewModel> permissions);
        Task UpdateBulkPermission(List<DocumentPermissionViewModel> permissions);
        Task<DMSDocumentViewModel> GetFileId(string Id);
        Task<List<DMSDocumentViewModel>> getDocData(string NoteId, string UserId);
        Task<List<DMSTagViewModel>> GetDMSTagData();
        Task<List<IdNameViewModel>> GetDMSTagIdNameList();
        Task<DMSTagViewModel> GetDMSTagDetails(string tagId);
        Task DeleteDMSTag(string tagId);
        Task<List<DMSDocumentViewModel>> getDocumentPermission(string NoteId, string UserId);
        Task<List<dynamic>> GetAllDocumentUdfDataByTableName(string tableName, string ids);
        Task<List<DocumentSearchViewModel>> GetAllDocumentsWithUdf(string udfs);
        Task<List<DocumentSearchViewModel>> GetAllDocumentsWithUdf1(string udfs);
        Task<List<DocumentSearchViewModel>> GetRecentDocumentWithAttachment(string udfs);
        Task<List<DashboardDocumentViewModel>> GetTopPendingDocuments(string userId);
        Task<List<IdNameViewModel>> GetAllDocumentSummary(string userId);
        Task<List<IdNameViewModel>> GetAllDocumentAnalysis(string userId);
        Task<List<DashboardDocumentViewModel>> GetTopRecentActivities(string udfs, string userId);
        Task<List<DashboardDocumentViewModel>> GetTopRecentDocuments(string udfs, string userId);
        Task<int> GetDocumentTypeCount();
        Task<int> GetDocumentCount();
        Task <List<WorkspaceDMSTree>>GetWorkspacebyUser(string userId);

        Task<List<DMSDocument>> GetDocumentbyUser(string userId);
        #endregion
    }
}
