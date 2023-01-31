using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    [Serializable]
    public class TemplateMasterViewModel
        : ViewModelBase
    {

        // public long TemplateMasterId { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "NtsType", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public NtsTypeEnum NtsType { get; set; }

        [Display(Name = "Code", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string Code { get; set; }
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        [Display(Name = "TemplateTypeName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string TemplateTypeName { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "TemplateCategoryId", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public long? TemplateCategoryId { get; set; }
        [Display(Name = "TemplateCategoryName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string TemplateCategoryName { get; set; }
        public string TemplateCategoryCode { get; set; }
        [Display(Name = "TemplateCategoryNtsType", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public NtsTypeEnum? TemplateCategoryNtsType { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "Name", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        [Display(Name = "SequenceNo", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public long SequenceNo { get; set; }

        public NtsClassificationEnum? NtsClassification { get; set; }

        public bool IsSystemTemplate { get; set; }
        public bool IsSelfTemplate { get; set; }
        [Display(Name = "Allow One Per TagTo")]
        public bool? AllowOnePerTagTo { get; set; }
        [Display(Name = "Legal Entity")]
        public long LegalEntityId { get; set; }
        public List<long?> LegalEntities { get; set; }
        public string GroupCode { get; set; }

        public string GroupName { get; set; }

        public string NotificationSubject { get; set; }


        [Display(Name = "Template File")]
        public long? FileId { get; set; }
        public FileViewModel File { get; set; }
        public FileViewModel BannerFile { get; set; }

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

        //[Display(Name = "Workspace")]
        //public long? WorkspaceId { get; set; }

        [Display(Name = "Workspace")]
        public List<long> WorkspaceIds { get; set; }
        //[Display(Name = "Template")]
        //public List<long?> TemplateMasterIds { get; set; }

        public long? BannerId { get; set; }
        [Display(Name = "Parent Template")]
        public long? ParentTemplateMasterId { get; set; }
        [Display(Name = "Parent Template")]
        public string ParentTemplateMasterName { get; set; }
        public bool? IsTemplatePackage { get; set; }
        public long? TemplatePackageId { get; set; }
        public long? NoteId { get; set; }
        public long? TaskId { get; set; }
        public long? ServiceId { get; set; }

        public long? TemplateId { get; set; }
        public DocumentStatusEnum? DocumentStatus { get; set; }

        [Display(Name = "Excel Template File")]
        public long? ExcelTemplateIdFileId { get; set; }
        public FileViewModel ExcelTemplateIdFile { get; set; }

    }
}
