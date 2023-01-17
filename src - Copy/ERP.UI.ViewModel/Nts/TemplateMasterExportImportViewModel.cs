using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    [Serializable]
    public class TemplateMasterExportImportViewModel : ViewModelBase
    {
        [Display(Name = "Parent")]
        public long? ParentId { get; set; }

        [Display(Name = "Legal Entity")]
        public long? LegalEntityId { get; set; }
        [Display(Name = "Legal Entity")]
        public List<long?> LegalEntities { get; set; }
        [Display(Name = "Nts Type")]
        public NtsTypeEnum? NtsType { get; set; }

        [Display(Name = "Template")]
        public List<long> TemplateMasterIds { get; set; }
        [Display(Name = "Import Template")]
        public long FileId { get; set; }
        public FileViewModel SelectedFile { get; set; }

        [Display(Name = "Template Type Name")]
        public string TemplateTypeName { get; set; }
        [Display(Name = "Template Name")]
        public string TemplateName { get; set; }
        [Display(Name = "New Template Name")]
        public string NewTemplateName { get; set; }
        [Display(Name = "Parent Template Name")]
        public string ParentTemplateName { get; set; }
        [Display(Name = "Template Master Code")]
        public string TemplateMasterCode { get; set; }
        [Display(Name = "New Template Master Code")]
        public string NewTemplateMasterCode { get; set; }

        [UIHint("ImportActionEditor")]
        [Display(Name = "Import Action")]
        public string ImportAction { get; set; }
        public ImportActionType? ImportActionType { get; set; }
        public string Templates { get; set; }

        public string Subject { get; set; }
        public string NewSubject { get; set; }

        public string Description { get; set; }
        public string NewDescription { get; set; }

        public long? ServiceTaskTemplateId { get; set; }

        [UIHint("ApprovalHierarchyEditor")]
        [Display(Name = "Approval Hierarchy")]
        public long? ApprovalHierarchyId { get; set; }
        public string ApprovalHierarchy { get; set; }
    }
}
