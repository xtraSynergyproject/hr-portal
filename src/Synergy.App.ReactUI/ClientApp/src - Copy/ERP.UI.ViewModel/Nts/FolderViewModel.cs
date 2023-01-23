using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ERP.UI.ViewModel
{
    public class FolderViewModel : FolderDocumentViewModel
    {
        public string Name { get; set; }
        public int? Level { get; set; }
        public long? FolderId { get; set; }

        public string UniqueId { get; set; }
        // public string UniqueIdReference { get; set; }
        public string ParentUniqueId { get; set; }

        public long? ToWorkspaceId { get; set; }
        public string selectedFiles { get; set; }
        public string ParentName { get; set; }
        public long? SharedByUserId { get; set; }
        public string SharedByUserName { get; set; }
        public long? PreviousParentId { get; set; }
        public bool? IsAllowSubFolders { get; set; }
        public bool? IsArchived { get; set; }
        public bool? IsFolderFileArchived { get; set; }
        //public List<DocumentViewModel> DocumentsList { get; set; }
        public long? FileId { get; set; }
      
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

        public bool IsAssignedWorkspace { get; set; }

#pragma warning disable CS0108 // 'FolderViewModel.FolderCode' hides inherited member 'FolderDocumentViewModel.FolderCode'. Use the new keyword if hiding was intended.
        public string FolderCode { get; set; }
#pragma warning restore CS0108 // 'FolderViewModel.FolderCode' hides inherited member 'FolderDocumentViewModel.FolderCode'. Use the new keyword if hiding was intended.
        public string TemaplateMasterCatCode { get; set; }
        public long NoteId { get; set; }
        public string LegalEntityName { get; set; }

        public AssignedTypeEnum? SharedTo { get; set; }




        public long? WorkspaceGroupUserId { get; set; }
        public long? WorkspaceGroupId { get; set; }

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
                if (FolderType == FolderTypeEnum.LegalEntity || FolderType == FolderTypeEnum.Workspace)
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
                if (FolderType == FolderTypeEnum.LegalEntity)
                {
                    return false;
                }
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
        public bool CanRename
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
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess))
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
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess))
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

        public long? DocumentId { get; set; }
        public string DocumentName { get; set; }
       
        public string TagCode { get; set; }
        public string TagName { get; set; }

        public string TagCategoryName { get; set; }
    }

    public class FolderDocumentViewModel : ViewModelBase
    {

        public string TagIds { get; set; }
        public long? TagId { get; set; }
        public long? TagCategoryId { get; set; }
        public long DocCount { get; set; }
        public bool? DisablePermissionInheritance { get; set; }
        public FolderTypeEnum? FolderType { get; set; }
        public long? ParentId { get; set; }

        public bool IsOwner { get; set; }
        public long? OwnerUserId { get; set; }
        public bool IsSelfWorkspace { get; set; }
        public bool IsWorkspaceAdmin { get; set; }

        public long? WorkspaceId { get; set; }

        public DmsPermissionTypeEnum? PermissionType { get; set; }
        public DmsAccessEnum? Access { get; set; }
        public string InheritedFrom { get; set; }
        public DmsAppliesToEnum? AppliesTo { get; set; }
        public string CreatedByUser { get; set; }
        public string UpdatedByUser { get; set; }
        public string FolderCode { get; set; }
    }
    public class UploadedFileResult
    {
        public bool uploaded { get; set; }
        public string fileUid { get; set; }
        public long Id { get; set; }
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
