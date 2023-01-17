using Synergy.App.ViewModel;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
  public  interface IDMSDocumentBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<FolderViewModel>> GetFoldersAndDocumentsNew(string userId, DocumentQueryTypeEnum documentQueryType = DocumentQueryTypeEnum.Folder, string parentId = null, string id = null);
        Task<IList<FolderViewModel>> GetUserWorkspaceTreeDataNew(string userId,string parentId);
        Task<List<WorkspaceViewModel>> GetDocuments(string userId, string search);
        Task<IList<DocumentListViewModel>> DocumentStageReportData(string userId, string stageStatus, string discipline, bool IsOverdue, int skip = 0, int take = 0);
        Task<DataTable> DocumentReportDataWithFilter(string templateId, string noteNo, string projectNo, string docDesc);
        Task<DataTable> DocumentReportDetailDataWithFilter(string templateId, string noteNo);
        Task<IList<DocumentListViewModel>> DocumentReceivedCommentsReportData( string userId, string templateId, string discipline, string revesion, DateTime fromDate, DateTime toDate, int skip = 0, int take = 0);
        Task<IList<DocumentListViewModel>> DocumentSubmittedReportData(string userId, string stageStatus, string discipline, string revesion, DateTime? fromDate, DateTime? toDate, int skip = 0, int take = 0);
        Task<List<DocumentListViewModel>> DocumentReceivedReportData(string userId, string stageStatus, string discipline, string revesion, DateTime? fromDate, DateTime? toDate, int skip = 0, int take = 0);
        Task<bool> ValidateRequestForInspection(NoteTemplateViewModel viewModel, dynamic udf, Dictionary<string, string> errorList);
        Task<IList<NoteViewModel>> GetAllFiles(string ParentId, string code, string Id);
        Task<IList<FolderViewModel>> GetAllChildByParentId(string ParentId, List<FolderViewModel> FolderList);
        Task<IList<FolderViewModel>> GetAllPermissionChildByParentId(string ParentId, List<FolderViewModel> FolderList);        
        Task<IList<DocumentPermissionViewModel>> GetAllNotePermissionByParentId(string ids);
        Task<IList<FolderViewModel>> GetAllParentByChildId(string ChildId, List<FolderViewModel> ParentList);
        Task<IList<FolderViewModel>> GetAllChildDocumentByParentId(string ParentId, List<FolderViewModel> FolderList);
        Task<bool> ValidateRequestForInspectionHalul(NoteTemplateViewModel viewModel, dynamic udf, Dictionary<string, string> errorList);
        Task<CommandResult<ServiceTemplateViewModel>> CreateWorkflowService(TemplateViewModel viewModel, string docId, Dictionary<string, object> rowData);
        Task<List<FolderAndDocumentViewModel>> GetAllFolderAndDocumentByUserId(string userId);
        Task<CommandResult<NoteTemplateViewModel>> ManageUploadedFiles(NoteTemplateViewModel model);
        Task<CommandResult<NoteTemplateViewModel>> AddUploadedFiles(NoteTemplateViewModel model);
        Task<FolderViewModel> GetNoteWithItsPermission(string noteId);

        Task<List<AttachmentViewModel>> GetUserAttachments(string userId, string portalId);
         Task<ServiceViewModel> GetWorflowDetailByDocument(string noteId);
        Task<IList<FolderViewModel>> GetFolderByParent(string parentId, string noteId);
        Task<string> IsUniqueGeneralDocumentByNo(string ParentId, string code, string Id);
        Task<List<NoteLinkShareViewModel>> GetDocumentLinksData(long id);
        Task<List<FolderViewModel>> GetFirstLevelWorkspacesByUser(string UserId);
        Task<List<FolderViewModel>> GetAllGeneralWorkspaceData();
        Task<List<FolderViewModel>> GetDocumentVersions(string noteId);
        Task<List<FolderViewModel>> GetAllChildbyParent(string parentId);
        Task<List<FolderViewModel>> CheckDocumentExist(string parentId);
        Task<List<FolderViewModel>> GetAllByParent(string UserId, string parentId);
        Task<bool> IsUniqueGeneralDocument(string ParentId, string code, string Id);
        Task<bool> IsUniqueDocumentFolder(string ParentId, string code, string Id);
        Task<IList<FolderViewModel>> GetAllParentByNoteId(string noteId);
        Task<IList<FolderViewModel>> GetAllDocuments();
        Task<bool> UpdateTagsByDocumentIds(string ids);
        Task<bool> DeleteNotesbyNoteIds(string ids);
        Task<bool> ArchiveNotesbyNoteIds(string ids);
        Task<bool> CheckMyWorkspaceExist(string UserId);
        Task<bool> CheckEmployeeBook(NoteTemplateViewModel model);
        Task<List<FolderViewModel>> GetAllChildWorkspaceAndFolder(string UserId, string parentId);
        Task<List<FolderViewModel>> GetAllChildWorkspaceFolderAndDocument(string UserId, string parentId);
        Task<List<FolderViewModel>> GetAllChildWorkspaceFolderAndFiles(string UserId, string parentId);
        Task<List<FolderViewModel>> GetAllChildFiles(string UserId, string parentId);
        Task<List<FolderViewModel>> GetAllPermittedWorkspaceFolderAndDocument(string UserId);
        Task<List<UserHierarchyChartViewModel>> GetDMSBookHierarchy(string parentId, int levelUpto, string hierarchyId, string permissions);
        Task<IList<FolderViewModel>> GetAllPermittedChildByParentId(string UserId, string ParentId);
        Task<List<DMSTagViewModel>> GetDMSTagData();
        Task<List<IdNameViewModel>> GetDMSTagIdNameList();
        Task<DMSTagViewModel> GetDMSTagDetails(string tagId);
        Task DeleteDMSTag(string tagId);
        Task<string> CreateDocumentByUpload(string path, string fileName, string ext, string parentId, string userId, string templateId,string batchId, bool isSingleUpload = false);
        Task<IList<DocumentSearchViewModel>> GetAllWorkspaceFolderDocuments(DateTime? lastUpdatedDate);
        Task<List<dynamic>> GetAllDocumentUdfDataByTableName(string tableName, string ids);
        Task<string> GetWorkspaceByDocumentId(string documentId);
        Task<List<DocumentSearchViewModel>> GetAllDmsDocumentsWithUdf();
        Task<List<DocumentSearchViewModel>> GetAllDmsDocumentsWithUdf1();
        Task<List<DocumentSearchViewModel>> GetRecentDocumentWithAttachment();
        Task<List<DashboardDocumentViewModel>> GetTopPendingDocuments(string userId);
        Task<List<IdNameViewModel>> GetAllDocumentSummary(string userId);
        Task<List<IdNameViewModel>> GetAllDocumentAnalysis(string userId);
        Task<List<DashboardDocumentViewModel>> GetTopRecentActivities(string userId);
        Task<List<DashboardDocumentViewModel>> GetTopRecentDocuments(string userId);
        Task<int> GetDocumentTypeCount();
        Task<int> GetDocumentCount();
        Task<List<WorkspaceDMSTree>>GetWorkspacebyUser(string userID);
        Task<List<DMSDocument>> GetDocumentbyUser(string userID);
        Task<CommandResult<NoteTemplateViewModel>> CreateFolder(string FolderName, string ParentId);

    }
}
