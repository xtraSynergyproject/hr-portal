
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class FolderAndDocumentViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string FolderCode { get; set; }
        public string WorkspaceId { get; set; }
        public bool IsSelfWorkspace { get; set; }
        public bool IsWorkspaceAdmin { get; set; }
        public bool IsAssignedWorkspace { get; set; }
        public FolderTypeEnum? FolderType { get; set; }
        public DmsPermissionTypeEnum? PermissionType { get; set; }
        public DmsAccessEnum? Access { get; set; }
        public string InheritedFrom { get; set; }
        public DmsAppliesToEnum? AppliesTo { get; set; }       
        public string OwnerUserId { get; set; }
        public bool IsOwner { get; set; }
        public string ParentId { get; set; }
        public string ParentName { get; set; }
        public bool? IsArchived { get; set; }
        public long DocCount { get; set; }
        public bool? DisablePermissionInheritance { get; set; }
        public string DocumentName { get; set; }
        public string Description { get; set; }
        public string NoteVersionNo { get; set; }
        public string LockStatus { get; set; }
        public string Title { get; set; }
        public string DocumentType { get; set; }
        public string TemplateMasterCode { get; set; }
        public string TemplateMasterId { get; set; }
        public string DocumentTypeId { get; set; }
        public string DocumentId { get; set; }
        public string FileName { get; set; }
        public string StatusName { get; set; }
        public string NoteStatus { get; set; }
        public string OwnerUser { get; set; } 
        public string CreatedByUser { get; set; }
        public string UpdatedByUser { get; set; }
        public string ServiceId { get; set; }
        public string WorkflowNo { get; set; }
        public string WorkflowName { get; set; }
        public string WorkflowStatus { get; set; }
        public string WorkflowTemplateId { get; set; }
        public long? SequenceNo { get; set; }
        public string NoteNo { get; set; }

        public bool CanOpen
        {
            get
            {
                if ( FolderType == FolderTypeEnum.Workspace)
                {
                    return false;
                }
                if (IsWorkspaceAdmin)
                {
                    return true;
                }                
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
       
    }  

}
