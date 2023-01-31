using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.Model;

namespace ERP.UI.ViewModel
{
    public class NoteSearchViewModel : SearchViewModelBase
    {
        //[Display(Name = "Note No")]
        [Display(Name = "NoteNo", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string NoteNo { get; set; }
        [Display(Name = "Text", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string Text { get; set; }
        [Display(Name = "NoteStatus", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string NoteStatus { get; set; }

        public string Mode { get; set; }
        public long? OwnerUserId { get; set; }
        public ModuleEnum? ModuleName { get; set; }
        public long? UserId { get; set; }
        public NoteReferenceTypeEnum? TagToType { get; set; }
        public long? TagTo { get; set; }
        public string Type { get; set; }
        public string ReturnUrl { get; set; }
        [Display(Name = "TemplateMasterId", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? TemplateMasterId { get; set; }
        [Display(Name = "CategoryLevel1", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? CategoryLevel1 { get; set; }
        [Display(Name = "CategoryLevel2", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? CategoryLevel2 { get; set; }
        [Display(Name = "CategoryLevel3", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? CategoryLevel3 { get; set; }
        [Display(Name = "CategoryLevel4", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? CategoryLevel4 { get; set; }
        [Display(Name = "CategoryLevel5", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? CategoryLevel5 { get; set; }
        public bool? IsAdmin { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }

        public string Company { get; set; }
        public string Department { get; set; }
        public string Direction { get; set; }
        public string Source { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string  CreatedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? CreatedDate { get; set; }
        public string  UpdatedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? UpdatedDate { get; set; }
        public long? NoteId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DocumentDate { get; set; }
        public long? DocumentSequence { get; set; }
        public int? DocumentCount { get; set; }
        public long? NoteVersionNo { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ExpiryDate { get; set; }
        public string DocumentName { get; set; }
        public long? DocumentId { get; set; }
        public long? ParentId { get; set; }
        public long? WorkspaceId { get; set; }

        public string ProjectName { get; set; }
        public string BinderCode { get; set; }
        public int? BinderDocSequence { get; set; }
        [Display(Name = "Name")]
        public string ContractorName { get; set; }
        [Display(Name = "Email Id")]
        public string ContractorEmail { get; set; }

        public string FieldName { get; set; }
        public DmsDocumentViewTypeEnum DocumentViewType { get; set; }

       
        public string Subject { get; set; }
        public string TagFilters { get; set; }
        public string DocumentApprovalFilters { get; set; }
        public string Documents { get; set; }
        public List<DocumentViewModel> DocumentTypeList { get; set; }
        public List<DocumentViewModel> DocumentList { get; set; }
        public List<DocumentViewModel> DocumentCreatorList { get; set; }
        public List<DatesViewModel> DocumentDatesList { get; set; }

        public AssignedTypeEnum? SharedTo { get; set; }
        public DocumentApprovalStatusEnum? DocumentApprovalStatus { get; set; }
        public DocumentApprovalStatuTypeEnum? DocumentApprovalStatusType { get; set; }


        public bool IsWorkspaceAdmin { get; set; }

        public DmsPermissionTypeEnum? PermissionType { get; set; }
        public DmsAccessEnum? Access { get; set; }
        public DmsAppliesToEnum? AppliesTo { get; set; }
        public bool IsOwner { get; set; }
        public bool IsSelfWorkspace { get; set; }

        public bool CanCreateDocument
        {
            get
            {
                if (IsOwner || IsSelfWorkspace || IsWorkspaceAdmin)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow && (Access == DmsAccessEnum.FullAccess|| Access==DmsAccessEnum.Modify) && (AppliesTo == DmsAppliesToEnum.ThisFolderAndFiles || AppliesTo == DmsAppliesToEnum.ThisFolderSubFoldersAndFiles))
                {
                    return true;
                }

                return false;
            }
        }

        public int TreeviewIndex { get; set; }
        public long? TagViewId { get; set; }
        public string TagIds { get; set; }
        [Display(Name = "Search Type")]
        public SearchTypeEnum? SearchType { get; set; }
        [Display(Name = "Applied To")]
        public AppliedToEnum? AppliedTo { get; set; }
        [Display(Name = "Has The Words")]
        public string HasWords { get; set; }
        [Display(Name = "Exclude the Words")]
        public string ExcludeWords { get; set; }
        public string RequestSource { get; set; }

        public string DefaultView { get; set; }
        public long? DocCount { get; set; }

    }
}
