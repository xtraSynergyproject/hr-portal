
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cms.UI.ViewModel
{
    public class FolderViewModel : FolderDocumentViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TemplateMasterCode { get; set; }        
        public int? Level { get; set; }
        public string FolderId { get; set; }        
        public string UniqueId { get; set; }
        // public string UniqueIdReference { get; set; }
        public string ParentUniqueId { get; set; }

        public string ToWorkspaceId { get; set; }
        public string selectedFiles { get; set; }
        public string ParentName { get; set; }
        public string SharedByUserId { get; set; }
        public string SharedByUserName { get; set; }
        public long? PreviousParentId { get; set; }
        public bool? IsAllowSubFolders { get; set; }
        public bool? IsArchived { get; set; }
        public bool? IsFolderFileArchived { get; set; }
        //public List<DocumentViewModel> DocumentsList { get; set; }
        public string FileId { get; set; }
      
        private string _Color;
        public string Color
        {
            get
            {
                if (_Color.IsNullOrEmpty())
                {
                    return Helper.RandomColor();
                }
                return _Color;
            }
            set { _Color = value; }
        }
        public string LastUpdatedDateDisplay
        {
            get
            {
                return LastUpdatedDate!=null ? LastUpdatedDate.ToDefaultDateFormat() : DateTime.Today.ToDefaultDateFormat();
            }
        }
        public bool IsAssignedWorkspace { get; set; }

        
        public string TemaplateMasterCatCode { get; set; }
        public string NoteId { get; set; }
        public string LegalEntityName { get; set; }

        public AssignedTypeEnum? SharedTo { get; set; }




        public string WorkspaceGroupUserId { get; set; }
        public string WorkspaceGroupId { get; set; }

        public DmsPermissionTypeEnum? WorkspaceGroupPermissionType { get; set; }
        public DmsAccessEnum? WorkspaceGroupAccess { get; set; }
        public string WorkspaceGroupInheritedFrom { get; set; }
        public DmsAppliesToEnum? WorkspaceGroupAppliesTo { get; set; }
        public bool? HasChildren { get; set; }

        public NtsModifiedStatusEnum? ModifiedStatus { get; set; }
        // public bool IsWorkspace { get; set; }




        public bool CanOpen
        {
            get
            {
                if (/*FolderType == FolderTypeEnum.LegalEntity ||*/ FolderType == FolderTypeEnum.Workspace)
                {
                    return false;
                }
                if (IsWorkspaceAdmin)
                {
                    return true;
                }
                //if (FolderType == FolderTypeEnum.Workspace && !IsAssignedWorkspace)
                //{
                //    return false;
                //}
                if (FolderType == FolderTypeEnum.Folder && !IsOwner && (PermissionType == null || PermissionType == DmsPermissionTypeEnum.Deny))
                {
                    return false;
                }
                return true;
            }
        }
        public bool ShowMenu
        {
            get
            {
                //if (FolderType == FolderTypeEnum.LegalEntity)
                //{
                //    return false;
                //}
                if (IsWorkspaceAdmin)
                {
                    return true;
                }
                if (FolderType == FolderTypeEnum.Workspace && !IsAssignedWorkspace)
                {
                    return false;
                }
                if (FolderType == FolderTypeEnum.Folder && !IsOwner && (PermissionType == null || PermissionType == DmsPermissionTypeEnum.Deny))
                {
                    return false;
                }
                return true;
            }
        }
        public bool CanCreateSubFolder
        {
            get
            {
                if ((IsOwner || IsSelfWorkspace || IsWorkspaceAdmin) && (FolderType==FolderTypeEnum.Folder || FolderType==FolderTypeEnum.Workspace))
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess || Access == DmsAccessEnum.Modify) && (FolderType == FolderTypeEnum.Folder || FolderType == FolderTypeEnum.Workspace))
                {
                    return true;
                }

                return false;
            }
        }
        public bool CanRename
        {
            get
            {
                if ((IsOwner || IsSelfWorkspace || IsWorkspaceAdmin) && FolderType==FolderTypeEnum.Folder)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess || Access == DmsAccessEnum.Modify) && FolderType == FolderTypeEnum.Folder)
                {
                    return true;
                }

                return false;
            }
        }
        public bool CanShare
        {
            get
            {
                //if (IsOwner || IsSelfWorkspace || IsWorkspaceAdmin)
                //{
                //    return true;
                //}
                //else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess))
                //{
                //    return true;
                //}

                return false;
            }
        }
        public bool CanMove
        {
            get
            {
                if (IsOwner || IsSelfWorkspace || IsWorkspaceAdmin)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess || Access == DmsAccessEnum.Modify))
                {
                    return true;
                }

                return false;
            }
        }
        public bool CanCopy
        {
            get
            {
                if (IsOwner || IsSelfWorkspace || IsWorkspaceAdmin)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess || Access == DmsAccessEnum.Modify))
                {
                    return true;
                }

                return false;
            }
        }
        public bool CanArchive
        {
            get
            {
                if (IsOwner || IsSelfWorkspace || IsWorkspaceAdmin)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess))
                {
                    return true;
                }

                return false;
            }
        }
        public bool CanDelete
        {
            get
            {
                if (IsOwner || IsSelfWorkspace || IsWorkspaceAdmin)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess))
                {
                    return true;
                }

                return false;
            }
        }
        public bool CanSeeDetail
        {
            get
            {
                if (IsOwner || IsSelfWorkspace || IsWorkspaceAdmin)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow)
                {
                    return true;
                }

                return false;
            }
        }
        public bool CanManagePermission
        {
            get
            {
                if (IsSelfWorkspace) { return false; }
                if (IsWorkspaceAdmin || (IsOwner && FolderType == FolderTypeEnum.Folder))
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess))
                {
                    return true;
                }

                return false;
            }
        }

        public string PropertyText
        {
            get
            {
                return string.Concat(Name, ",", Id, ",", ParentId, ",", FolderType, ",", DocCount);
            }
        }

        public long? SequenceNo { get; set; }

        public string DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentNo { get; set; }
       
        public string TagCode { get; set; }
        public string TagName { get; set; }

        public string TagCategoryName { get; set; }
        public DocumentApprovalStatusEnum? DocumentApprovalStatus { get; set; }
        public string DocumentsApprovalStatus { get; set; }
        public string DocumentApprovalStatusType { get; set; }
        public bool CanEditDocument
        {
            get
            {
                if (/*IsOwner || */IsSelfWorkspace || IsWorkspaceAdmin)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess || Access == DmsAccessEnum.Modify) && (AppliesTo == DmsAppliesToEnum.OnlyThisDocument || AppliesTo == DmsAppliesToEnum.ThisFolderAndFiles || AppliesTo == DmsAppliesToEnum.ThisFolderSubFoldersAndFiles))
                {
                    return true;
                }

                return false;
            }
        }
        public bool CanShareDocument
        {
            get
            {
                //if (IsOwner || IsSelfWorkspace || IsWorkspaceAdmin)
                //{
                //    return true;
                //}
                //else if(PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess) && (AppliesTo == DmsAppliesToEnum.ThisFolderAndFiles || AppliesTo == DmsAppliesToEnum.ThisFolderSubFoldersAndFiles))
                //{
                //    return true;
                //}

                return false;
            }
        }
        public bool CanMoveDocument
        {
            get
            {
                if (/*IsOwner || */IsSelfWorkspace || IsWorkspaceAdmin)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess || Access == DmsAccessEnum.Modify) && (AppliesTo == DmsAppliesToEnum.OnlyThisDocument || AppliesTo == DmsAppliesToEnum.ThisFolderAndFiles || AppliesTo == DmsAppliesToEnum.ThisFolderSubFoldersAndFiles))
                {
                    return true;
                }

                return false;
            }
        }
        public bool CanCopyDocument
        {
            get
            {
                if (/*IsOwner || */IsSelfWorkspace || IsWorkspaceAdmin)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess || Access == DmsAccessEnum.Modify) && (AppliesTo == DmsAppliesToEnum.OnlyThisDocument || AppliesTo == DmsAppliesToEnum.ThisFolderAndFiles || AppliesTo == DmsAppliesToEnum.ThisFolderSubFoldersAndFiles))
                {
                    return true;
                }

                return false;
            }
        }
        public bool CanArchiveDocument
        {
            get
            {
                if (/*IsOwner || */IsSelfWorkspace || IsWorkspaceAdmin)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess) && (AppliesTo == DmsAppliesToEnum.OnlyThisDocument || AppliesTo == DmsAppliesToEnum.ThisFolderAndFiles || AppliesTo == DmsAppliesToEnum.ThisFolderSubFoldersAndFiles))
                {
                    return true;
                }

                return false;
            }
        }
       
        public string ServiceId { get; set; }
        public bool? EnableLock { get; set; }
        public LockStatusEnum? LockStatus { get; set; }
        public string WorkflowServiceTemplateMasterId { get; set; }
        public string WorkflowTemplateId { get; set; }
        public string WorkflowName { get; set; }
        public string WorkflowCode { get; set; }
        public string WorkflowNo { get; set; }
        public string WorkflowStatus { get; set; }
        public string WorkflowServiceId { get; set; }
        public string WorkflowServiceStatus { get; set; }
        public string WorkflowServiceStatusName { get; set; }
        public string NoteStatus { get; set; }
        public string EnableDocumentChangeRequest { get; set; }
        public bool CanDeleteDocument
        {
            get
            {
                if (DocumentApprovalStatus == DocumentApprovalStatusEnum.ApprovalInProgress)
                {
                    return false;
                }
                if (/*IsOwner || */IsSelfWorkspace || IsWorkspaceAdmin)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess) && (AppliesTo == DmsAppliesToEnum.OnlyThisDocument || AppliesTo == DmsAppliesToEnum.ThisFolderAndFiles || AppliesTo == DmsAppliesToEnum.ThisFolderSubFoldersAndFiles))
                {
                    return true;
                }

                return false;
            }
        }
        public bool CanManagePermissionDocument
        {
            get
            {
                if (IsSelfWorkspace) { return false; }
                if (IsWorkspaceAdmin/* || (IsOwner)*/)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess))
                {
                    return true;
                }

                return false;
            }
        }
        public bool CanDocumentChange
        {
            get
            {
                if (DocumentApprovalStatus != DocumentApprovalStatusEnum.Approved)
                {
                    return false;
                }
                if (/*IsOwner || */IsSelfWorkspace || IsWorkspaceAdmin)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess) && (AppliesTo == DmsAppliesToEnum.OnlyThisDocument || AppliesTo == DmsAppliesToEnum.ThisFolderAndFiles || AppliesTo == DmsAppliesToEnum.ThisFolderSubFoldersAndFiles))
                {
                    return true;
                }

                return false;
            }

        }
        public bool CanCreateDocument
        {
            get
            {
                if ((IsOwner || IsSelfWorkspace || IsWorkspaceAdmin) || (PermissionType == 0 && (Access == DmsAccessEnum.FullAccess || Access == DmsAccessEnum.Modify) && (AppliesTo == DmsAppliesToEnum.ThisFolderAndFiles || AppliesTo == DmsAppliesToEnum.ThisFolderSubFoldersAndFiles)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
   
    public class FolderDocumentViewModel : ViewModelBase
    {

        public string TagIds { get; set; }
        public string TagId { get; set; }
        public string TagCategoryId { get; set; }
        public long DocCount { get; set; }
        public bool? DisablePermissionInheritance { get; set; }
        public FolderTypeEnum? FolderType { get; set; }
        public string ParentId { get; set; }

        public bool IsOwner { get; set; }
        public string OwnerUserId { get; set; }
        public bool IsSelfWorkspace { get; set; }
        public bool IsWorkspaceAdmin { get; set; }

        public string WorkspaceId { get; set; }

        public DmsPermissionTypeEnum? PermissionType { get; set; }
        public DmsAccessEnum? Access { get; set; }
        public string InheritedFrom { get; set; }
        public DmsAppliesToEnum? AppliesTo { get; set; }
        public string CreatedByUser { get; set; }
        public string UpdatedByUser { get; set; }
        public string FolderCode { get; set; }
        public long FileSize { get; set; }
    }
    public class UploadedFileResult
    {
        public bool uploaded { get; set; }
        public string fileUid { get; set; }
        public string Id { get; set; }
        public string FolderName { get; set; }
        public string ParentFolderName { get; set; }
        public string FileId { get; set; }
        public string RelativePath { get; set; }
        public List<string> Folders { get; set; }


    }
    [DataContract]
    public class ChunkMetaData1
    {
        [DataMember(Name = "uploadUid")]
        public string UploadUid { get; set; }
        [DataMember(Name = "fileName")]
        public string FileName { get; set; }
        [DataMember(Name = "contentType")]
        public string ContentType { get; set; }
        [DataMember(Name = "chunkIndex")]
        public long ChunkIndex { get; set; }
        [DataMember(Name = "totalChunks")]
        public long TotalChunks { get; set; }
        [DataMember(Name = "totalFileSize")]
        public long TotalFileSize { get; set; }
        [DataMember(Name = "relativePath")]
        public string RelativePath { get; set; }

    }

    public class FolderPermissionViewModel
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
        public long? ParentId { get; set; }


        public DmsPermissionTypeEnum? UserPermissionType { get; set; }
        public DmsAccessEnum? UserAccess { get; set; }
        public string UserInheritedFrom { get; set; }
        public DmsAppliesToEnum? UserAppliesTo { get; set; }

        public long? WorkspaceGroupUserId { get; set; }
        public long? WorkspaceGroupId { get; set; }



        public DmsPermissionTypeEnum? WorkspaceGroupPermissionType { get; set; }
        public DmsAccessEnum? WorkspaceGroupAccess { get; set; }
        public string WorkspaceGroupInheritedFrom { get; set; }
        public DmsAppliesToEnum? WorkspaceGroupAppliesTo { get; set; }



    }
    public class DocumentsPermissionViewModel
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
        public long? WorkspaceGroupUserId { get; set; }

        public DmsPermissionTypeEnum? UserPermissionType { get; set; }
        public DmsAccessEnum? UserAccess { get; set; }
        public string UserInheritedFrom { get; set; }
        public DmsAppliesToEnum? UserAppliesTo { get; set; }

        public DmsPermissionTypeEnum? WorkspaceGroupPermissionType { get; set; }
        public DmsAccessEnum? WorkspaceGroupAccess { get; set; }
        public string WorkspaceGroupInheritedFrom { get; set; }
        public DmsAppliesToEnum? WorkspaceGroupAppliesTo { get; set; }      
       
    }

}
