using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common;
using CMS.Data.Model;

namespace CMS.UI.ViewModel
{
    public class DocumentPermissionViewModel: DocumentPermission
    {

        public string UserPermissionGroup { get; set; }
        public bool? DisablePermissionInheritance { get; set; }
        public string Principal { get; set; }
        public string WorkspaceId { get; set; }
        public string ParentId { get; set; }
        public string DocumentName { get; set; }
        public bool? IsDocument { get; set; }

        public string ReferenceId { get; set; }
        public bool IsOwner { get; set; }
       
        public bool IsSelfWorkspace { get; set; }
        public bool IsWorkspaceAdmin { get; set; }
        public FolderTypeEnum? FolderType { get; set; }
        public string InheritedFromId { get; set; }

        public bool CanManagePermission
        {
            get
            {
                if (IsSelfWorkspace)
                { 
                    return false;
                }
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
        public bool DisablePermittedNotification { get; set; }
        public string FileId { get; set; }
    }
}
