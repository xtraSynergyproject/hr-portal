using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.Model;
using Kendo.Mvc.UI;

namespace ERP.UI.ViewModel
{
    public class DocumentViewModel : FolderDocumentViewModel, ISchedulerEvent
    {
        //[Display(Name = "Document No")]
        [Display(Name = "Document No")]
        public string NoteNo { get; set; }
        public string ParentName { get; set; }
        public string Name { get; set; }
        [Display(Name = "Text", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string Text { get; set; }
        [Display(Name = "NoteStatus", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string NoteStatus { get; set; }
        public string VersionNo { get; set; }
        public string ApprovalStatusType { get; set; }
        public long? DocumentTypeDashboardId { get; set; }
        public string DocumentTypeName { get; set; }
        public string WorkspaceName { get; set; }
        [Display(Name = "Document Status")]
        public NtsActionEnum? DocumentStatus { get; set; }
        public string DocumentStatusCode { get; set; }
        public string DocumentStatusName { get; set; }
        [Display(Name = "Document Status")]
        public string StatusName { get; set; }
        public string DocumentText { get; set; }
        public string DocumentApprovalStatusName { get; set; }
        public string StageStatus { get; set; }
        public string Revision { get; set; }
        public string Discipline { get; set; }
        public string IssueCode { get; set; }
        public string Vendor { get; set; }
        public string ProjectFolder { get; set; }

        public string Mode { get; set; }
        public long? FilterStatusCount { get; set; }
        public long? PhotoId { get; set; }
        public ModuleEnum? ModuleName { get; set; }
        public long? UserId { get; set; }
        public NoteReferenceTypeEnum? TagToType { get; set; }
        public long? TagTo { get; set; }
        public long? TagViewId { get; set; }
        public string Type { get; set; }
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
        public bool IsAdmin { get; set; }
        public bool? IsArchived { get; set; }
        [Display(Name = "Department Name")]
        public long OrganizationId { get; set; }

        public string FolderName { get; set; }
        public string ServiceType { get; set; }
        public string ServiceName { get; set; }
        public string ServiceStatus { get; set; }
        public string ServiceStatusCode { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DueDate { get; set; }
        public string DueDateText { get { return DueDate.ToDefaultDateFormat(); } }
        //public long DocumentId { get; set; }
        //public long NoteId { get; set; }
        //public string DocumentName { get; set; }
        public long? ServiceId { get; set; }

        public long? ChangeRequestServiceId { get; set; }
        // public long? DocumentTypeDashboardId { get; set; }
        //public string DocumentTypeName { get; set; }
        // public long? WorkspaceId { get; set; }
        //public string WorkspaceName { get; set; }

        public string Company { get; set; }
        public string Department { get; set; }
        public string Direction { get; set; }
        public string Source { get; set; }
        public string DocumentFileName { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Shared Date")]
        public DateTime? SharedDate { get; set; }
        [Display(Name = "Last Updated By")]
        public string UpdatedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Last Updated Date")]
        public DateTime? UpdatedDate { get; set; }
        public long? NoteId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DocumentDate { get; set; }
        public long? DocumentSequence { get; set; }
        public int? DocumentCount { get; set; }
        [Display(Name = "Document Version")]
        public long? NoteVersionNo { get; set; }
        [Display(Name = "Document Description")]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ExpiryDate { get; set; }
        public string DocumentName { get; set; }
        public string[] Metadata { get; set; }
        public string DocumentNameLimit
        {
            get
            {
                return DocumentName.LimitTo(30);
            }
        }
        public long? DocumentId { get; set; }
        public long? ServiceIdForDashboard { get; set; }

        public string DateType { get; set; }
        public long? FolderParentId { get; set; }
        public string FileExtension { get; set; }
        public string MemberCount { get; set; }

        public string UploadType { get; set; }

        public bool? FullAccess { get; set; }
        public bool? CanView { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanDelete { get; set; }
        public bool? CanShare { get; set; }

        public string WorkflowName { get; set; }
        public string WorkflowNo { get; set; }
        public string WorkflowStatus { get; set; }

        public string Comments { get; set; }
        public string CommentedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public string CommentedDate { get; set; }

        public List<TemplateFieldValueViewModel> Controls { get; set; }
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
        public LockStatusEnum? LockStatus { get; set; }
        public string level { get; set; }
        public ICollection<string> Tagsfilter { get; set; }
        public ICollection<string> Statusfilter { get; set; }
        public ICollection<string> DocumentTypefilter { get; set; }
        public ICollection<string> DocumentApprovalfilter { get; set; }
        public string DocumentTypeId { get; set; }
        public string tag { get; set; }
        public AssignedTypeEnum? SharedTo { get; set; }
        public DocumentApprovalStatusEnum? DocumentApprovalStatus { get; set; }
        public DocumentApprovalStatuTypeEnum? DocumentApprovalStatusType { get; set; }
        public string TemplateMasterCode { get; set; }

        public string WorkflowServiceTemplateMasterCode { get; set; }
        public long? WorkflowServiceTemplateMasterId { get; set; }
        public string FileName { get; set; }
        public string FolderPath { get; set; }


        public long? PermittedUserId { get; set; }
        public string PermittedUserName { get; set; }
        public string[] HighlightText { get; set; }

        public long? WorkflowTemplateId { get; set; }
        public bool? EnableDocumentChangeRequest { get; set; }
        public bool? EnableLock { get; set; }

        public long? LegalEntityId { get; set; }
        public long? FileSize { get; set; }

        public string FileSizeDisplay
        {
            get
            {
                return Helper.ByteSizeWithSuffix(FileSize, 2);
            }
        }

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
        public bool CanMove
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
        public bool CanCopy
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
        public bool CanArchive
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
        public bool CanManagePermission
        {
            get
            {
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
        public string Title { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public string NtsId { get; set; }
        public string TemplateCategoryCode { get; set; }
        public string Code { get; set; }
        public string Label { get; set; }
        public int? RecurrId { get; set; }
        //public DateTime? DueDate { get; set; }
        public string TitleLimited { get { return Title.LimitTo(120); } }

        public string DefaultView { get; set; }
        public string TemplateOwner { get; set; }

        public int? Skip { get; set; }
        public int? Take { get; set; }
        public int? TotalCount { get; set; }
        public NtsModifiedStatusEnum? ModifiedStatus { get; set; }
    }




    public class DatesViewModel
    {
        public string Name { get; set; }
        public string Level { get; set; }
        public string DocCount { get; set; }
        public List<DocumentViewModel> DocumentList { get; set; }

        public DocumentApprovalStatusEnum? DocumentApprovalStatus { get; set; }
        public DocumentApprovalStatuTypeEnum? DocumentApprovalStatusType { get; set; }

    }
}
