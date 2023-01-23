
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERP.Data.GraphModel
{

    public partial class NTS_TemplateField : NodeBase
    {
        public long FieldSerialNo { get; set; }


        public decimal? SequenceNo { get; set; }

        //public long? GroupTemplateFieldId { get; set; }
        //public long? GridTemplateFieldId { get; set; }       

     //   public string FieldPartialViewName { get; set; }
        public string FieldName { get; set; }

        public int FieldColumnWidth { get; set; }
        //public string FieldWidthCode { get; set; }


        public string LabelDisplayName { get; set; }
        public int LabelColumnWidth { get; set; }

        public HyperlinkTargetEnum HyperlinkTarget { get; set; }
        public int PopupWidth { get; set; }
        public int PopupHeight { get; set; }
        public string PopupTitle { get; set; }

        public string DataType { get; set; }
        public string DataFormatString { get; set; }

        public int NumberOfLines { get; set; }
        public long? DataLength { get; set; }
        public long? MaxFileSize { get; set; }

        public string DataTextField { get; set; }
        public string DataValueField { get; set; }

        public long? CharacterMinimumLength { get; set; }
        public long? CharacterMaximumLength { get; set; }
        public string MinimumValue { get; set; }
        public string MaximumValue { get; set; }
        public int? DecimalPrecision { get; set; }

        public string PlaceHolder { get; set; }
        public bool AutoBind { get; set; }
        public string DataSourceControllerName { get; set; }
        public string DataSourceActionName { get; set; }
        public AreaEnum? DataSourceAreaName { get; set; }
        public string Url { get; set; }
        public string DataSourceType { get; set; }
        public string DataSourceHtmlAttributesString { get; set; }


        public string LabelColorHexa { get; set; }
        public bool IsLabelItalic { get; set; }
        public bool IsLabelUnderlined { get; set; }
        public string ToolTip { get; set; }
        public string HelpInfo { get; set; }
        public string AdditionalInfo { get; set; }

        //public string RequiredTypeCode { get; set; }

        public string RequiredClientScript { get; set; }
        public string RequiredServerScript { get; set; }

        //public string VisibilityTypeCode { get; set; }

        public string VisibilityClientScript { get; set; }
        public string VisibilityServerScript { get; set; }



        //public string ReadonlyTypeCode { get; set; }

        public string ReadOnlyClientScript { get; set; }
        public string ReadOnlyServerScript { get; set; }


        //public string DefaultValueTypeCode { get; set; }

        public string DefaultCodeStatic { get; set; }
        public string DefaultValueStatic { get; set; }
        public string DefaultValueQuery { get; set; }

        public string DocumentReadyScript { get; set; }
        public string ClientOnChangeScript { get; set; }

        public string ClientValidationScript { get; set; }

        public string ServerValidationScript { get; set; }

        public string ClientDataBindScript { get; set; }

        public string FieldHtmlAttributesString { get; set; }

        public string LabelHtmlAttributesString { get; set; }

        //public ICollection<NtsUserTypeEnum> EditableBy { get; set; }
        //public ICollection<NtsActionEnum> EditableContext { get; set; }
        //public ICollection<NtsUserTypeEnum> ViewableBy { get; set; }
        //public ICollection<NtsActionEnum> ViewableContext { get; set; }

        public string DefaultDynamicMethodName { get; set; }

        public bool ShowInGrid { get; set; }
        public bool? EnableDropdownGrouping { get; set; }


        public int GridMinimumRows { get; set; }
        public int GridMaximumRows { get; set; }
        public int GridRequiredMinimumRows { get; set; }

        public bool GridCanAdd { get; set; }
        public string GridAddButtonName { get; set; }
        public bool GridCanEdit { get; set; }
        public string GridEditButtonName { get; set; }
        public bool GridCanView { get; set; }
        public string GridViewButtonName { get; set; }
        public bool GridCanDelete { get; set; }
        public string GridDeleteButtonName { get; set; }
        public bool GridShowDeleteConfirmation { get; set; }
        public string GridDeleteConfirmationMessage { get; set; }
        public bool GridAllowFilter { get; set; }
        public bool GridAllowSorting { get; set; }
        public bool GridAllowPaging { get; set; }
        public int GridPageSize { get; set; }
        public UdfValueTypeEnum GridDataBindingValueType { get; set; }


        public string GridDeleteSuccessMessage { get; set; }

        public string RelationshipName { get; set; }
        public NodeEnum RelationshipTargetNode { get; set; }

        public string ClientCallBackScript { get; set; }
        public string ControlStyle { get; set; }
        public string ContainerStyle { get; set; }
        public string ControlClass { get; set; }
        public string ConainerClass { get; set; }       
        public bool? IsPredefinedField { get; set; }
        public long? PermissionCode { get; set; }
        public bool? EnableScanUpload { get; set; }
        public bool? IsOCREnable { get; set; }
    }
    public class R_TemplateField_TemplatePackage : RelationshipBase
    {

    }
    public class R_TemplateField_Template : RelationshipBase
    {

    }
    public class R_TemplateField_Field : RelationshipBase
    {

    }
    public class R_TemplateField_Group_TemplateField : RelationshipBase
    {

    }
    //public class R_TemplateField_Parent_TemplateField : RelationshipBase
    //{

    //}
    public class R_TemplateField_Grid_TemplateField : RelationshipBase
    {

    }
    public class R_TemplateField_FieldWidth_ListOfValue : RelationshipBase
    {

    }
    public class R_TemplateField_RequiredType_ListOfValue : RelationshipBase
    {

    }
    public class R_TemplateField_VisibilityType_ListOfValue : RelationshipBase
    {

    }
    public class R_TemplateField_ReadonlyType_ListOfValue : RelationshipBase
    {

    }
    public class R_TemplateField_DefaultValueType_ListOfValue : RelationshipBase
    {

    }
    public class R_TemplateField_EditableBy_ListOfValue : RelationshipBase
    {

    }
    public class R_TemplateField_EditableContext_ListOfValue : RelationshipBase
    {

    }
    public class R_TemplateField_ViewableBy_ListOfValue : RelationshipBase
    {

    }
    public class R_TemplateField_ViewableContext_ListOfValue : RelationshipBase
    {

    }

    public class R_TemplateField_ViewableBy_ServiceTaskTemplate : RelationshipBase
    {

    }

}
