using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace CMS.UI.ViewModel
{
    public class NoteSearchViewModel 
    {

        public string Id { get; set; }
        public bool ForAllCompany { get; set; }
        public virtual StatusEnum? Status { get; set; }
        public DataOperation? Operation { get; set; }
        public string NoteNo { get; set; }        
        public string Text { get; set; }       
        public string NoteStatus { get; set; }
        public string Mode { get; set; }
        public string OwnerUserId { get; set; }
        public ModuleEnum? ModuleName { get; set; }
        public string UserId { get; set; }
        public NoteReferenceTypeEnum? TagToType { get; set; }
        public string TagTo { get; set; }
       
        public string Type { get; set; }
        public string ReturnUrl { get; set; }
        
        public string TemplateMasterId { get; set; }
       
        public long? CategoryLevel1 { get; set; }
        
        public long? CategoryLevel2 { get; set; }
       
        public long? CategoryLevel3 { get; set; }
       
        public long? CategoryLevel4 { get; set; }
       
        public long? CategoryLevel5 { get; set; }
        public bool? IsAdmin { get; set; }
        
        public string OrganizationId { get; set; }

        public string Company { get; set; }
        public string Department { get; set; }
        public string Direction { get; set; }
        public string Source { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string  CreatedBy { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        public string  UpdatedBy { get; set; }
       
        public DateTime? UpdatedDate { get; set; }
        public string NoteId { get; set; }
       
        public DateTime? DocumentDate { get; set; }
        public long? DocumentSequence { get; set; }
        public int? DocumentCount { get; set; }
        public long? NoteVersionNo { get; set; }
        public string Description { get; set; }
        
        public DateTime? ExpiryDate { get; set; }
        public string DocumentName { get; set; }
        public long? DocumentId { get; set; }
        public long? ParentId { get; set; }
        public long? WorkspaceId { get; set; }

        public string ProjectName { get; set; }
        public string BinderCode { get; set; }
        public int? BinderDocSequence { get; set; }
        
        public string ContractorName { get; set; }
        public string ModuleCode { get; set; }

        public string ContractorEmail { get; set; }

        public string FieldName { get; set; }
        //public DmsDocumentViewTypeEnum DocumentViewType { get; set; }

       
        public string Subject { get; set; }
        public string TagFilters { get; set; }
        public string DocumentApprovalFilters { get; set; }
        public string Documents { get; set; }
        public List<DocumentViewModel> DocumentTypeList { get; set; }
        public List<DocumentViewModel> DocumentList { get; set; }
        public List<DocumentViewModel> DocumentCreatorList { get; set; }
        //public List<DatesViewModel> DocumentDatesList { get; set; }

        //public AssignedTypeEnum? SharedTo { get; set; }
        //public DocumentApprovalStatusEnum? DocumentApprovalStatus { get; set; }
        //public DocumentApprovalStatuTypeEnum? DocumentApprovalStatusType { get; set; }
        public string ModuleId { get; set; }

        public bool IsWorkspaceAdmin { get; set; }

        //public DmsPermissionTypeEnum? PermissionType { get; set; }
        //public DmsAccessEnum? Access { get; set; }
        //public DmsAppliesToEnum? AppliesTo { get; set; }
        public bool IsOwner { get; set; }
        public bool IsSelfWorkspace { get; set; }

        //public bool CanCreateDocument
        //{
        //    get
        //    {
        //        if (IsOwner || IsSelfWorkspace || IsWorkspaceAdmin)
        //        {
        //            return true;
        //        }
        //        else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess|| Access==DmsAccessEnum.Modify) && (AppliesTo == DmsAppliesToEnum.ThisFolderAndFiles || AppliesTo == DmsAppliesToEnum.ThisFolderSubFoldersAndFiles))
        //        {
        //            return true;
        //        }

        //        return false;
        //    }
        //}

        public int TreeviewIndex { get; set; }
        public long? TagViewId { get; set; }
        public string TagIds { get; set; }

        //public SearchTypeEnum? SearchType { get; set; }

        //public AppliedToEnum? AppliedTo { get; set; }
        public List<string> NoteStatusIds { get; set; }
        public List<string> NoteAssigneeIds { get; set; }
        public List<string> NoteOwnerIds { get; set; }

        public string HasWords { get; set; }
       
        public string ExcludeWords { get; set; }
        public string RequestSource { get; set; }

        public string DefaultView { get; set; }
        public long? DocCount { get; set; }
        public List<ModuleViewModel> ModuleList { get; set; }
        public string PortalNames { get; set; }
    }
}
