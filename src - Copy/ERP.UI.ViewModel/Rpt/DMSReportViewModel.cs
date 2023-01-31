using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class DMSReportViewModel
    {
        public string SelectedModules { get; set; }
        #region WorkSpace

        //[Display(Name = "Work space")]
        public long? WorkspaceId { get; set; }
        [Display(Name = "Work space")]
        public string WorkspaceName { get; set; }
        public bool IsWorkspaceNameColumnRequired { get; set; }
        [Display(Name = "Folder")]
        public string Folder { get; set; }
        [Display(Name = "Folder Path")]
        public string FolderPath { get; set; }
        public long? ParentFolderId { get; set; }
        public bool IsFolderPathColumnRequired { get; set; }

        //[Display(Name = "Legal Entity")]
        //public long? WorkspaceLegalEntityId { get; set; }
        //public string WorkspaceLegalEntity { get; set; }
        //public bool IsWorkspaceLegalEntityColumnRequired { get; set; }

        //[Display(Name = "Reference Type")]
        //public ReferenceTypeEnum? WorkspaceReferenceType { get; set; }
        //public string WorkspaceReferenceTypeName { get; set; }
        //public bool IsWorkSpaceReferenceTypeColumnRequired { get; set; }

        //[Display(Name = "Workspace Module")]
        //public ModuleEnum? WorkspaceModule { get; set; }
        //public string WorkspaceModuleName { get; set; }
        //public bool IsWorkspaceModuleColumnRequired { get; set; }

        //[Display(Name = "Start Date Between")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? WorkspaceStartDateFrom { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? WorkspaceStartDateTo { get; set; }
        //public bool IsWorkspaceStartDateColumnRequired { get; set; }

        #endregion

        #region Document
        [Display(Name = "Document")]
        public long? DocumentId { get; set; }
        public string DocumentName { get; set; }
        public bool IsDocumentColumnRequired { get; set; }

        [Display(Name = "Document Number")]
        public string DocumentNo { get; set; }
        public bool IsDocumentNoColumnRequired { get; set; }

        [Display(Name = "Document Description")]
        public string DocumentDescription { get; set; }
        public bool IsDocumentDescriptionColumnRequired { get; set; }

        [Display(Name = "Document Version")]
        public string DocumentVersion { get; set; }
        public bool IsDocumentVersionColumnRequired { get; set; }

        [Display(Name = "Document type")]
        public long? DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public bool IsDocumentTypeColumnRequired { get; set; }

        [Display(Name = "Document owner")]
        //public long? DocumentCreatedById { get; set; }
        public string DocumentCreatedByName { get; set; }
        public bool IsDocumentCreatedByColumnRequired { get; set; }

        //[Display(Name = "Created Date Between")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentCreatedDateFrom { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentCreatedDateTo { get; set; }
        [Display(Name = "Created date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DocumentCreatedDate { get; set; }
        public bool IsDocumentCreatedDateColumnRequired { get; set; }


        [Display(Name = "Last Updated by")]
        //public long? DocumentCreatedById { get; set; }
        public string DocumentLastUpdatedByName { get; set; }
        public bool IsDocumentLastUpdatedByColumnRequired { get; set; }

        [Display(Name = "Last Updated date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DocumentLastUpdatedDate { get; set; }
        public bool IsDocumentLastUpdatedDateColumnRequired { get; set; }

        [Display(Name = "Document status")]
        //public long? DocumentStatusId { get; set; }
        public NtsActionEnum? DocumentStatus { get; set; }
        public string DocumentStatusName { get; set; }
        public bool IsDocumentStatusColumnRequired { get; set; }

        [Display(Name = "Document workflow name")]
        public string DocumentWorkflowName { get; set; }
        public bool IsDocumentWorkflowNameColumnRequired { get; set; }

        [Display(Name = "Document approval status")]
        //public long? DocumentStatusId { get; set; }
        public DocumentApprovalStatusEnum? DocumentApprovalStatus { get; set; }
        public string DocumentApprovalStatusName { get; set; }
        public bool IsDocumentApprovalStatusColumnRequired { get; set; }

        [Display(Name = "Checkout by")]
        public long? DocumentCheckoutById { get; set; }
        public string DocumentCheckoutByName { get; set; }
        public bool IsDocumentCheckoutByColumnRequired { get; set; }

        [Display(Name = "Checkout Date Between")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DocumentCheckoutDateFrom { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DocumentCheckoutDateTo { get; set; }
        public bool IsDocumentCheckoutDateColumnRequired { get; set; }

        //[Display(Name = "Document locked status")]
        ////public long? DocumentLockedStatusId { get; set; }
        //public LockStatusEnum? DocumentLockStatus { get; set; }
        //public string DocumentLockedStatusName { get; set; }
        //public bool IsDocumentLockedStatusColumnRequired { get; set; }

        //[Display(Name = "Locked by")]
        //public long? DocumentLockedById { get; set; }
        //public string DocumentLockedByName { get; set; }
        //public bool IsDocumentLockedByColumnRequired { get; set; }

        //[Display(Name = "Locked Date Between")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentLockedDateFrom { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentLockedDateTo { get; set; }
        //public bool IsDocumentLockedDateColumnRequired { get; set; }

        [Display(Name = "Tags")]
        public string DocumentTags { get; set; }
        public ICollection<string> DocumentTagsName { get; set; }
        //public string DocumentTagsName { get; set; }
        public bool IsDocumentTagsColumnRequired { get; set; }

        [Display(Name = "Comments Count")]
        //public long? DocumentCreatedById { get; set; }
        public int? DocumentCommentsCount { get; set; }
        public bool IsDocumentCommentsCountColumnRequired { get; set; }

        [Display(Name = "Inline Comments Count")]
        //public long? DocumentCreatedById { get; set; }
        public int? DocumentInlineCommentsCount { get; set; }
        public bool IsDocumentInlineCommentsCountColumnRequired { get; set; }

        [Display(Name = "Permission Count")]
        //public long? DocumentCreatedById { get; set; }
        public int? DocumentPermissionCount { get; set; }
        public bool IsDocumentPermissionCountColumnRequired { get; set; }


        //public long? DocumentCreatedById { get; set; }
        //public List<string> DocumentFiles { get; set; }

        public string DocumentFileId { get; set; }
        [Display(Name = "Document Files")]
        public string DocumentFileNames { get; set; }

        public bool IsDocumentFilesColumnRequired { get; set; }

        #endregion

        // public List<DMSReportViewModel> ReportSource { get; set; }

        //#region Activity

        //[Display(Name = "Event Date Between")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentEventDateFrom { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentEventDateTo { get; set; }
        //public bool IsDocumentEventDateColumnRequired { get; set; }

        //[Display(Name = "Event")]
        //public DataOperationEvent? DocumentEvent { get; set; }
        //public string DocumentEventName { get; set; }
        //public bool IsDocumentEventColumnRequired { get; set; }

        //[Display(Name = "Event by")]
        //public long? DocumentEventById { get; set; }
        //public string DocumentEventByName { get; set; }
        //public bool IsDocumentEventByColumnRequired { get; set; }

        //#endregion

        //#region Workflow Request

        //[Display(Name = "WorkFlow status")]
        //public long? DocumentWfStatusId { get; set; }
        //public string DocumentWfStatusName { get; set; }
        //public bool IsDocumentWfStatusColumnRequired { get; set; }

        //[Display(Name = "WorkFlow Last Updated by")]
        //public long? DocumentWfUpdatedById { get; set; }
        //public string DocumentWfUpdatedByName { get; set; }
        //public bool IsDocumentWfUpdatedByColumnRequired { get; set; }

        //[Display(Name = "WorkFlow Last Updated Date Between")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentWfLastUpdatedDateFrom { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentWfLastUpdatedDateTo { get; set; }
        //public bool IsDocumentWfLastUpdatedDateColumnRequired { get; set; }

        //[Display(Name = "WorkFlow Start Date Between")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentWfStartDateFrom { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentWfStartDateTo { get; set; }
        //public bool IsDocumentWfStartDateColumnRequired { get; set; }

        //[Display(Name = "WorkFlow Due Date Between")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentWfDueDateFrom { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentWfDueDateTo { get; set; }
        //public bool IsDocumentWfDueDateColumnRequired { get; set; }

        //[Display(Name = "WorkFlow End Date Between")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentWfEndDateFrom { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentWfEndDateTo { get; set; }
        //public bool IsDocumentWfEndDateColumnRequired { get; set; }

        //[Display(Name = "WorkFlow End Date Between")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentWfCompletedDateFrom { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? DocumentWfCompletedDateTo { get; set; }
        //public bool IsDocumentWfCompletedDateColumnRequired { get; set; }

        //#endregion
    }

    public class AnalyzerCypherViewModel
    {
        //List<DMSReportViewModel> data = null;
        //var searchCondition = "";
        //var returnValues = "";
        //var param = new Dictionary<string, object>();
        //var udfList = new Dictionary<string, string>();
        public string SearchCondition { get; set; }
        public string ReturnValues { get; set; }
        public Dictionary<string,object> Parameters{ get; set; }
        public Dictionary<string,string> UDFList { get; set; }
    }
}
