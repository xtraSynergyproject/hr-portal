using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class TemplateFieldValueViewModel : ViewModelBase
    {

        public long TemplateId { get; set; }

        [Display(Name = "FieldId", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public long FieldId { get; set; }
        public long FieldSerialNo { get; set; }

        public long? GroupTemplateFieldId { get; set; }
        public long TemplateFieldId { get; set; }

        public long? FieldValueId { get; set; }

        public string FieldPartialViewName { get; set; }
        //[Display(Name = "Field Name")]

        public string FieldName { get; set; }
        [Display(Name = "FieldColumnWidth", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public int FieldColumnWidth { get; set; }
        public string FieldWidthCode { get; set; }

        public string GroupName { get; set; }

        [Display(Name = "LabelDisplayName", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string LabelDisplayName { get; set; }
        public HyperlinkTargetEnum HyperlinkTarget { get; set; }
        private int _PopupHeight;
        public int PopupHeight
        {
            get { return _PopupHeight == 0 ? 400 : _PopupHeight; }
            set { _PopupHeight = value; }
        }
        private int _PopupWidth;
        public int PopupWidth
        {
            get { return _PopupWidth == 0 ? 300 : _PopupWidth; }
            set { _PopupWidth = value; }
        }

        public string PopupTitle { get; set; }

        [Display(Name = "LabelColumnWidth", ResourceType = typeof(ERP.Translation.Nts.Field))]
        //[Display(Name = "Label Column Width")]
        public int LabelColumnWidth { get; set; }

        [Display(Name = "TemplateColumnCount", ResourceType = typeof(ERP.Translation.Nts.Field))]
        //[Display(Name = "TemplateColumn Count")]
        //  public int TemplateColumnCount { get { return 12 / TemplateMaximumColumn; } }
        public int TemplateColumnCount { get { return 12 / TemplateMaximumColumn; } }

        private int _TemplateMaximumColumn;
        public int TemplateMaximumColumn
        {
            get { return _TemplateMaximumColumn == 0 ? Constant.Nts.DefaultTemplateMaximumColumn : _TemplateMaximumColumn; }
            set { _TemplateMaximumColumn = value; }
        }
        [Display(Name = "SequenceNo", ResourceType = typeof(ERP.Translation.Nts.Field))]
        //[Display(Name = "Sequence No")]
        public long? SequenceNo { get; set; }

        public string Code { get; set; }
        public string Value { get; set; }

        public string DataType { get; set; }

        [Display(Name = "DataFormatString", ResourceType = typeof(ERP.Translation.Nts.Field))]
        //[Display(Name = "Data Format String")]
        public string DataFormatString { get; set; }

        [Display(Name = "NumberOfLines", ResourceType = typeof(ERP.Translation.Nts.Field))]
        //[Display(Name = "Number Of Lines")]
        public int NumberOfLines { get; set; }
        public long? DataLength { get; set; }

        //[Display(Name = "Data Text Field")]
        [Display(Name = "DataTextField", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DataTextField { get; set; }
        //[Display(Name = "Data Value Field")]
        [Display(Name = "DataValueField", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DataValueField { get; set; }

        //[Display(Name = "Character Minimum Length")]
        [Display(Name = "CharacterMinimumLength", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public long? CharacterMinimumLength { get; set; }
        //[Display(Name = "Character Maximum Length")]
        [Display(Name = "CharacterMaximumLength", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public long? CharacterMaximumLength { get; set; }
        [Display(Name = "MinimumValue", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string MinimumValue { get; set; }
        [Display(Name = "MaximumValue", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string MaximumValue { get; set; }
        [Display(Name = "DecimalPrecision", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public int DecimalPrecision { get; set; }

        public string PlaceHolder { get; set; }
        public bool AutoBind { get; set; }
        //public string DataSourceTypeCode { get; set; }
        //public string DataSourceQueryTypeCode { get; set; }
        //public string DataSourceQuery { get; set; }
        //[Display(Name = "DataSource Controller Name")]
        [Display(Name = "DataSourceControllerName", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DataSourceControllerName { get; set; }
        [Display(Name = "DataSourceActionName", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DataSourceActionName { get; set; }
        [Display(Name = "DataSourceAreaName", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DataSourceAreaName { get; set; }

        [Display(Name = "DefaultValue", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DefaultValue { get; set; }
        //public string DefaultValueQueryTypeCode { get; set; }
        //public string DefaultValueQuery { get; set; }

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

        public bool ShowRequiredAsterisk
        {
            get
            {
                return RequiredTypeCode == "REQUIRED"
                && DisplayMode == FieldDisplayModeEnum.Editable
                && IsVisible;
            }
        }

        public string RequiredTypeCode { get; set; }
        public string RequiredClientScript { get; set; }
        public string RequiredServerScript { get; set; }

        public long? GridTemplateFieldId { get; set; }
        public FieldDisplayModeEnum DisplayMode { get; set; }
        public bool IsVisible { get; set; }
        public Dictionary<string, Type> GridColumns { get; set; }


        public string VisibilityTypeCode { get; set; }
        public string VisibilityClientScript { get; set; }
        public string VisibilityServerScript { get; set; }


        public string ReadonlyTypeCode { get; set; }
        public string ReadOnlyClientScript { get; set; }
        public string ReadOnlyServerScript { get; set; }


        public string DefaultValueTypeCode { get; set; }
        public string DefaultValueStatic { get; set; }
        public string DefaultCodeStatic { get; set; }
        public string DefaultValueQuery { get; set; }

        public string ServerValidationScript { get; set; }

        [Display(Name = "DocumentReadyScript", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DocumentReadyScript { get; set; }
        public string ClientVisibilityScript { get; set; }
        public string ClientReadOnlyScript { get; set; }
        public string ClientOnChangeScript { get; set; }
     
        public string ClientValidationScript { get; set; }
        public string ClientDataBindScript { get; set; }

        //public string ServerVisibilityScript { get; set; }
        //public string ServerValidationScript { get; set; }
        [Display(Name = "FieldHtmlAttributesString", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string FieldHtmlAttributesString { get; set; }
        public Dictionary<string, object> FieldHtmlAttributes
        {
            get
            {
                var dict = new Dictionary<string, object>();
                if (FieldHtmlAttributesString.IsNotNullAndNotEmpty())
                {
                    dict = FieldHtmlAttributesString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
              .Select(part => part.Split('='))
              .ToDictionary(split => Convert.ToString(split[0]).Trim(), split => (object)Convert.ToString(split[1]).Trim());
                }
                InjectDefaultFieldAttributes(dict);
                return dict;

            }
        }
        private void InjectDefaultFieldAttributes(Dictionary<string, object> dict)
        {
            dict.AppendClass(string.Concat("form-control ", FieldWidthCode));
            if (this.DisplayMode == FieldDisplayModeEnum.Readonly || this.ReadonlyTypeCode == "READ_ONLY")
            {
                switch (FieldPartialViewName)
                {
                    case "NTS_CheckBox":
                        dict.Add("disabled", "disabled");
                        break;
                    default:
                        dict.Add("readonly", "readonly");
                        break;
                }

            }
            if (this.NumberOfLines > 0)
            {
                dict.Add("rows", this.NumberOfLines);
            }
            if (this.ToolTip.IsNotNullAndNotEmpty())
            {
                dict.Add("title", this.ToolTip);
            }
        }
        [Display(Name = "DataSourceHtmlAttributesString", ResourceType = typeof(ERP.Translation.Nts.Field))]
        public string DataSourceHtmlAttributesString { get; set; }

        public dynamic AreaName
        {
            get
            {
                return new { area = DataSourceAreaName };
            }
        }
        //public Dictionary<string, object> DataSourceHtmlAttributes
        //{
        //    get
        //    {


        //        var dict = new Dictionary<string, object>();
        //        dict.Add("lovTypeCode", "PERSON_TYPE");

        //        if (DataSourceHtmlAttributesString.IsNotNullAndNotEmpty())
        //        {
        //            dict = DataSourceHtmlAttributesString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //      .Select(part => part.Split('='))
        //      .ToDictionary(split => Convert.ToString(split[0]).Trim(), split => (object)Convert.ToString(split[1]).Trim());
        //        }

        //        //  InjectDefaultDatasourceAttributes(dict);
        //        // var o = Helper.ConvertToDynamicObject(dict);
        //        return dict;
        //    }
        //}

        private void InjectDefaultDatasourceAttributes(Dictionary<string, object> dict)
        {
            if (this.DataSourceAreaName.IsNotNullAndNotEmpty())
            {
                dict.Add("area", this.DataSourceAreaName);
            }
        }

        public string LabelHtmlAttributesString { get; set; }
        public Dictionary<string, object> LabelHtmlAttributes
        {
            get
            {
                var dict = new Dictionary<string, object>();
                if (LabelHtmlAttributesString.IsNotNullAndNotEmpty())
                {
                    dict = LabelHtmlAttributesString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(part => part.Split('='))
                   .ToDictionary(split => Convert.ToString(split[0]).Trim(), split => (object)Convert.ToString(split[1]).Trim());
                }

                InjectDefaultLabelAttributes(dict);
                return dict;

            }
        }
        public string AdditionalInfo { get; set; }
        private void InjectDefaultLabelAttributes(Dictionary<string, object> dict)
        {
            dict.AppendClass("control-label");
        }

        public List<NtsUserTypeEnum> EditableBy { get; set; }
        public List<NtsActionEnum> EditableContext { get; set; }
        public List<NtsUserTypeEnum> ViewableBy { get; set; }
        public List<NtsActionEnum> ViewableContext { get; set; }

        public string GridEditButtonName { get; set; }
        public string GridAddButtonName { get; set; }
        public string GridDeleteButtonName { get; set; }

        public string DefaultDynamicMethodName { get; set; }

        public bool? EnableDropdownGrouping { get; set; }

        public string RelationshipName { get; set; }
        public NodeEnum? RelationshipTargetNode { get; set; }
        public List<IdNameViewModel> FieldDropDownItems { get; set; }
        public string ClientCallBackScript { get; set; }
        public string ControlStyle { get; set; }
        public string ContainerStyle { get; set; }
        public string ControlClass { get; set; }
        public string ConainerClass { get; set; }
        public bool GridCanAdd { get; set; }

        public NtsTypeEnum? TemplateMasterNtsType { get; set; }
        public long? NtsTypeId { get; set; }
        public long? NtsTypeVersionId { get; set; }

        public NtsUserTypeEnum? TemplateUserType { get; set; } //set runtime
        public NtsActionEnum? TemplateAction { get; set; }//set runtime

        public DataTable GridTable { get; set; }

        public long? RowId { get; set; }

        public long? PermissionCode { get; set; }
        public bool? EnableScanUpload { get; set; }
        public bool? IsOCREnable { get; set; }
        public IList<OCRTemplateMapping> OCRTemplateList { get; set; }
        public string OCRTemplate { get; set; }

        public long? ServiceId { get; set; }
    }
}
