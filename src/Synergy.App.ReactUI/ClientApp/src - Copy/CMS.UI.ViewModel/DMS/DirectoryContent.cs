using CMS.Common;
using CMS.Data.Model;
using Microsoft.AspNetCore.Http;
using Syncfusion.EJ2.FileManager.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class DirectoryContent
    {
        public FileManagerDirectoryContent[] data { get; set; }
        public bool showHiddenItems { get; set; }
        public string searchString { get; set; }
        public bool saseSensitive { get; set; }
        public IList<IFormFile> uploadFiles { get; set; }
        public string[] renameFiles { get; set; }
        public string targetPath { get; set; }
        public string parentId { get; set; }
        public string filterId { get; set; }
        public string filterPath { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public bool isFile { get; set; }
        public bool hasChild { get; set; }
        //public string URL { get; set; }
        //public string UrlValue { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime dateModified { get; set; }
        public string previousName { get; set; }
        public long size { get; set; }
        public string name { get; set; }
        public string[] names { get; set; }
        public string newName { get; set; }
        public string action { get; set; }
        public string path { get; set; }
        public FileManagerDirectoryContent targetData { get; set; }
        public AccessPermission permission { get; set; }
        public FolderTypeEnum FolderType { get; set; }
        public string TemplateCode { get; set; }
        public string Count { get; set; }
        public bool CanOpen { get; set; }
        public bool ShowMenu { get; set; }
        public bool CanCreateSubFolder { get; set; }
        public bool CanRename { get; set; }
        public bool CanShare { get; set; }
        public bool CanMove { get; set; }
        public bool CanCopy { get; set; }
        public bool CanArchive { get; set; }
        public bool CanDelete { get; set; }
        public bool CanSeeDetail { get; set; }
        public bool CanManagePermission { get; set; }
        public bool CanCreateWorkspace { get; set; }
        public bool CanCreateDocument { get; set; }
        //new properties added
        public bool CanEditDocument { get; set; }
        public string ServiceId { get; set; }
        public string WorkflowTemplateId { get; set; }
        public string WorkflowTemplateCode { get; set; }
        public string WorkflowServiceId { get; set; }
        public string WorkflowServiceStatus { get; set; }
        public string WorkflowServiceStatusName { get; set; }
        public bool CanShareDocument { get; set; }
        public string ModifiedStatus { get; set; }
        public string DocumentApprovalStatusType { get; set; }
        public string StatusName { get; set; }
        public bool CanDeleteDocument { get; set; }
        public string WorkspaceId { get; set; }
        public string NoteNo { get; set; }
        public string CreatedBy { get; set; }
        public long? SequenceOrder { get; set; }
        public bool IsSelfWorkspace { get; set; }
    }
    public class FileResponse
    {
        public DirectoryContent cwd { get; set; }
        public IEnumerable<DirectoryContent> files { get; set; }
        public ErrorDetails error { get; set; }
        public string errorsList { get; set; }
        public FileDetails details { get; set; }
    }

    public class FileExplorerViewModel
    {
        public string title { get; set; }
        public string key { get; set; }
        public bool lazy { get; set; }
        public bool expanded { get; set; }
        public bool active { get; set; }
        public bool folder { get; set; }
        public bool Workspace { get; set; }
        public bool Document { get; set; }
        public bool File { get; set; }
        public string FileId { get; set; }
        public string FileSize { get; set; }
        public string Count { get; set; }
        public long? Sequence { get; set; }
        public string NoteNo { get; set; }
        public string WorkflowServiceStatusName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool CanCreateWorkspace { get; set; }
        public bool CanManagePermission { get; set; }
        public bool CanSeeDetail { get; set; }
        public bool CanDelete { get; set; }
        public bool CanArchive { get; set; }
        public bool CanCopy { get; set; }
        public bool CanMove { get; set; }
        public bool CanShare { get; set; }
        public bool CanRename { get; set; }
        public bool CanCreateSubFolder { get; set; }
        public bool ShowMenu { get; set; }
        public bool CanOpen { get; set; }
        public string TemplateCode { get; set; }
        public string parentId { get; set; }
        public string ParentId { get; set; }
        public string WorkspaceId { get; set; }
        public FolderTypeEnum FolderType { get; set; }
        public string WorkflowTemplateCode { get; set; }
        public string DocumentApprovalStatusType { get; set; }
        public string WorkflowServiceId { get; set; }
        public string StatusName { get; set; }
        public bool CanEditDocument { get; set; }
        public bool CanCreateDocument { get; set; }
        public string WorkflowServiceStatus { get; set; }
        public string type { get; set; }
        public string namespaces { get; set; }
        public string methodName { get; set; }
        public string templateType { get; set; }
        public bool? checkbox { get; set; }
        public string FieldDataType { get; set; }
        public string ItemType { get; set; }
        public string ParentName { get; set; }
        public bool IsSelfWorkspace { get; set; }
        public List<FileExplorerViewModel> children { get; set; }
        public string NodeId { get; set; }
    }

}
