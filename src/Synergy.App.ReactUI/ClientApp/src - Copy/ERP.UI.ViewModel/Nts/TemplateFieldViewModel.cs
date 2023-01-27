using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    [Serializable]
    public class TemplateFieldViewModel : ViewModelBase
    {
        [Display(Name = "Sequence No")]
        public decimal? SequenceNo { get; set; }
        public long TemplateFieldId { get; set; }
        public long? TemplateMasterId { get; set; }

        public long TemplateId { get; set; }
        [Required]
        [Display(Name = "FieldId", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public long FieldId { get; set; }

        [Display(Name = "GroupTemplateFieldId", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public long? GroupTemplateFieldId { get; set; }

        [Display(Name = "Grid Template")]
        public long? GridTemplateFieldId { get; set; }

        public string FieldPartialViewName { get; set; }
        public NtsTypeEnum? TemplateMasterNtsType { get; set; }
        public string TemplateMasterLegalEntityCode { get; set; }

        [Required]
        [Display(Name = "FieldName", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string FieldName { get; set; }
        [Display(Name = "FieldColumnWidth", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public int FieldColumnWidth { get; set; }
        [Display(Name = "FieldWidthCode", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string FieldWidthCode { get; set; }
        [Required]
        [Display(Name = "LabelDisplayName", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string LabelDisplayName { get; set; }
        [Required]
        [Display(Name = "LabelColumnWidth", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public int LabelColumnWidth { get; set; }

        public int TemplateMaximumColumn { get; set; }

        public string DataType { get; set; }
        [Display(Name = "DataFormatString", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DataFormatString { get; set; }

        [Display(Name = "NumberOfLines", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public int NumberOfLines { get; set; }
        public long? DataLength { get; set; }

        [Display(Name = "DataTextField", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DataTextField { get; set; }
        [Display(Name = "DataValueField", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DataValueField { get; set; }

        [Display(Name = "CharacterMinimumLength", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public long? CharacterMinimumLength { get; set; }
        [Display(Name = "CharacterMaximumLength", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public long? CharacterMaximumLength { get; set; }
        [Display(Name = "MinimumValue", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string MinimumValue { get; set; }
        [Display(Name = "MaximumValue", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string MaximumValue { get; set; }
        [Display(Name = "DecimalPrecision", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public int? DecimalPrecision { get; set; }

        [Display(Name = "PlaceHolder", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string PlaceHolder { get; set; }
        [Display(Name = "AutoBind", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public bool AutoBind { get; set; }

        [Display(Name = "DataSourceControllerName", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DataSourceControllerName { get; set; }
        [Display(Name = "DataSourceActionName", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DataSourceActionName { get; set; }
        [Display(Name = "DataSourceAreaName", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public AreaEnum? DataSourceAreaName { get; set; }

        [Display(Name = "LabelColorHexa", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string LabelColorHexa { get; set; }
        [Display(Name = "IsLabelItalic", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public bool IsLabelItalic { get; set; }
        [Display(Name = "IsLabelUnderlined", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public bool IsLabelUnderlined { get; set; }
        [Display(Name = "ToolTip", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string ToolTip { get; set; }
        [Display(Name = "HelpInfo", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string HelpInfo { get; set; }
        [Display(Name = "AdditionalInfo", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string AdditionalInfo { get; set; }

        [Display(Name = "RequiredTypeCode", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string RequiredTypeCode { get; set; }
        [Display(Name = "RequiredClientScript", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string RequiredClientScript { get; set; }
        [Display(Name = "RequiredServerScript", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string RequiredServerScript { get; set; }


        [Display(Name = "VisibilityTypeCode", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string VisibilityTypeCode { get; set; }
        [Display(Name = "VisibilityClientScript", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string VisibilityClientScript { get; set; }
        [Display(Name = "VisibilityServerScript", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string VisibilityServerScript { get; set; }

        [Display(Name = "ReadonlyTypeCode", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string ReadonlyTypeCode { get; set; }
        [Display(Name = "ReadOnlyClientScript", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string ReadOnlyClientScript { get; set; }
        [Display(Name = "ReadOnlyServerScript", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string ReadOnlyServerScript { get; set; }


        [Display(Name = "DefaultValueTypeCode", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DefaultValueTypeCode { get; set; }
        [Display(Name = "DefaultValueStatic", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DefaultValueStatic { get; set; }
        [Display(Name = "DefaultCodeStatic", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DefaultCodeStatic { get; set; }
        [Display(Name = "DefaultValueQuery", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DefaultValueQuery { get; set; }

        [Display(Name = "DocumentReadyScript", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DocumentReadyScript { get; set; }
        [Display(Name = "ClientOnChangeScript", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string ClientOnChangeScript { get; set; }
        [Display(Name = "ClientValidationScript", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string ClientValidationScript { get; set; }
        [Display(Name = "ClientDataBindScript", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string ClientDataBindScript { get; set; }


        [Display(Name = "ServerValidationScript", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string ServerValidationScript { get; set; }

        [Display(Name = "FieldHtmlAttributesString", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string FieldHtmlAttributesString { get; set; }
        //
        [Display(Name = "DataSourceParameters", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DataSourceHtmlAttributesString { get; set; }

        [Display(Name = "LabelHtmlAttributesString", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string LabelHtmlAttributesString { get; set; }


        [Display(Name = "EditableBys", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public NtsUserTypeEnum EditableBys { get; set; }
        [Required]
        [Display(Name = "Editable By")]
        public ICollection<NtsUserTypeEnum> EditableBy { get; set; }
        public string[] Editable { get; set; }
        [Display(Name = "ViewableBys", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public NtsUserTypeEnum ViewableBys { get; set; }
        [Required]
        [Display(Name = "Viewable By")]
        public ICollection<NtsUserTypeEnum> ViewableBy { get; set; }
        public string[] Viewable { get; set; }
        [Required]
        [Display(Name = "EditableContext", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public ICollection<NtsActionEnum> EditableContext { get; set; }
        public NtsActionEnum EditableContexts { get; set; }
        public string[] EditableCont { get; set; }
        [Required]
        [Display(Name = "ViewableContext", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public ICollection<NtsActionEnum> ViewableContext { get; set; }
        public NtsActionEnum ViewableContexts { get; set; }
        public string[] ViewableCont { get; set; }

        [Display(Name = "GridMinimumRows", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public int GridMinimumRows { get; set; }
        [Display(Name = "GridMaximumRows", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public int GridMaximumRows { get; set; }
        [Display(Name = "GridRequiredMinimumRows", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public int GridRequiredMinimumRows { get; set; }
        [Display(Name = "ShowInGrid", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public bool ShowInGrid { get; set; }

        [Display(Name = "GridCanAdd", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public bool GridCanAdd { get; set; }
        [Display(Name = "GridAddButtonName", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string GridAddButtonName { get; set; }
        [Display(Name = "GridCanEdit", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public bool GridCanEdit { get; set; }
        [Display(Name = "GridEditButtonName", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string GridEditButtonName { get; set; }
        [Display(Name = "GridCanView", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public bool GridCanView { get; set; }
        [Display(Name = "GridViewButtonName", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string GridViewButtonName { get; set; }
        [Display(Name = "GridCanDelete", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public bool GridCanDelete { get; set; }
        [Display(Name = "GridDeleteButtonName", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string GridDeleteButtonName { get; set; }
        [Display(Name = "GridShowDeleteConfirmation", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public bool GridShowDeleteConfirmation { get; set; }
        [Display(Name = "GridDeleteConfirmationMessage", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string GridDeleteConfirmationMessage { get; set; }
        [Display(Name = "GridAllowFilter", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public bool GridAllowFilter { get; set; }
        [Display(Name = "GridAllowSorting", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public bool GridAllowSorting { get; set; }
        [Display(Name = "GridAllowPaging", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public bool GridAllowPaging { get; set; }
        [Display(Name = "GridPageSize", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public int GridPageSize { get; set; }



        [Display(Name = "GridDeleteSuccessMessage", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string GridDeleteSuccessMessage { get; set; }
        [Display(Name = "Default Dynamic Method Name")]
        public string DefaultDynamicMethodName { get; set; }
        [Display(Name = "Hyperlink Target")]
        public HyperlinkTargetEnum HyperlinkTarget { get; set; }
        [Display(Name = "Popup Width")]
        public int PopupWidth { get; set; }
        [Display(Name = "Popup Height")]
        public int PopupHeight { get; set; }
        [Display(Name = "Popup Title")]
        public string PopupTitle { get; set; }
        [Display(Name = "EnableDropdownGrouping", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public bool? EnableDropdownGrouping { get; set; }
        [Display(Name = "Relationship Name")]
        public string RelationshipName { get; set; }
        [Display(Name = "Relationship Target Node")]
        public NodeEnum RelationshipTargetNode { get; set; }
        [Display(Name = "Client Call Back Script")]
        public string ClientCallBackScript { get; set; }
        [Display(Name = "Control Style")]
        public string ControlStyle { get; set; }
        [Display(Name = "Container Style")]
        public string ContainerStyle { get; set; }
        [Display(Name = "Control Class")]
        public string ControlClass { get; set; }
        [Display(Name = "Container Class")]
        public string ConainerClass { get; set; }
        public TemplatePackageFieldViewModel TemplatePackageFieldConfig { get; set; }
        public bool? IsPredefinedField { get; set; }
        public long? TemplatePackageId { get; set; }

        public long? PermissionCode { get; set; }
        [Display(Name = "Enable Scan Upload")]
        public bool? EnableScanUpload { get; set; }
        public bool? IsOCREnable { get; set; }

        public IList<OCRTemplateMapping> OCRTemplateList { get; set; }
        [Display(Name = "Viewable By Step Tasks")]
        public List<long> ViewableByStepTasks { get; set; }
        public string ViewableByStepTaskStr { get; set; }
        public long[] ViewableByStepTaskData { get; set; }        
    }
}
