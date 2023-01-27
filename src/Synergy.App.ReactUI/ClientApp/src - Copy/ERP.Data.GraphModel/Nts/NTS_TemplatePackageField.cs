using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{
    public class NTS_TemplatePackageField : NTS_TemplatePackageFieldConfig
    {
       
    }
    public class R_TemplatePackageField_TemplatePackage : RelationshipBase
    {

    }
    public class R_TemplatePackageField_TemplateField : RelationshipBase
    {

    }
    public class NTS_TemplatePackageFieldConfig : NodeBase
    {
        public bool SequenceNoAllow { get; set; }
        public bool TemplateFieldIdAllow { get; set; }
        public bool TemplateMasterIdAllow { get; set; }

        public bool TemplateIdAllow { get; set; }
        public bool FieldIdAllow { get; set; }

        public bool GroupTemplateFieldIdAllow { get; set; }

        public bool GridTemplateFieldIdAllow { get; set; }

        public bool FieldPartialViewNameAllow { get; set; }
        public bool TemplateMasterNtsTypeAllow { get; set; }
        public bool TemplateMasterLegalEntityCodeAllow { get; set; }

        public bool FieldNameAllow { get; set; }
        public bool FieldColumnWidthAllow { get; set; }
        public bool FieldWidthCodeAllow { get; set; }
        public bool LabelDisplayNameAllow { get; set; }
        public bool LabelColumnWidthAllow { get; set; }

        public bool TemplateMaximumColumnAllow { get; set; }

        public bool DataTypeAllow { get; set; }
        public bool DataFormatboolAllow { get; set; }

        public bool NumberOfLinesAllow { get; set; }
        public bool DataLengthAllow { get; set; }

        public bool DataTextFieldAllow { get; set; }
        public bool DataValueFieldAllow { get; set; }
        public bool DataSourceHtmlAttributesStringAllow { get; set; }

        public bool CharacterMinimumLengthAllow { get; set; }
        public bool CharacterMaximumLengthAllow { get; set; }
        public bool MinimumValueAllow { get; set; }
        public bool MaximumValueAllow { get; set; }
        public bool? DecimalPrecisionAllow { get; set; }
        public bool DataFormatStringAllow { get; set; }
        public bool PlaceHolderAllow { get; set; }
        public bool AutoBindAllow { get; set; }

        public bool DataSourceControllerNameAllow { get; set; }
        public bool DataSourceActionNameAllow { get; set; }
        public bool DataSourceAreaNameAllow { get; set; }

        public bool LabelColorHexaAllow { get; set; }
        public bool IsLabelItalicAllow { get; set; }
        public bool IsLabelUnderlinedAllow { get; set; }
        public bool ToolTipAllow { get; set; }
        public bool HelpInfoAllow { get; set; }
        public bool AdditionalInfoAllow { get; set; }

        public bool RequiredTypeCodeAllow { get; set; }
        public bool RequiredClientScriptAllow { get; set; }
        public bool RequiredServerScriptAllow { get; set; }


        public bool VisibilityTypeCodeAllow { get; set; }
        public bool VisibilityClientScriptAllow { get; set; }
        public bool VisibilityServerScriptAllow { get; set; }

        public bool ReadonlyTypeCodeAllow { get; set; }
        public bool ReadOnlyClientScriptAllow { get; set; }
        public bool ReadOnlyServerScriptAllow { get; set; }


        public bool DefaultValueTypeCodeAllow { get; set; }
        public bool DefaultValueStaticAllow { get; set; }
        public bool DefaultCodeStaticAllow { get; set; }
        public bool DefaultValueQueryAllow { get; set; }

        public bool DocumentReadyScriptAllow { get; set; }
        public bool ClientOnChangeScriptAllow { get; set; }
        public bool ClientValidationScriptAllow { get; set; }
        public bool ClientDataBindScriptAllow { get; set; }


        public bool ServerValidationScriptAllow { get; set; }

        public bool FieldHtmlAttributesStringAllow { get; set; }
        public bool LabelHtmlAttributesStringAllow { get; set; }
        public bool FieldHtmlAttributesboolAllow { get; set; }
        public bool DataSourceHtmlAttributesboolAllow { get; set; }

        public bool LabelHtmlAttributesboolAllow { get; set; }


        public bool EditableBysAllow { get; set; }
        public bool EditableByAllow { get; set; }
        public bool EditableAllow { get; set; }
        public bool ViewableBysAllow { get; set; }
        public bool ViewableByAllow { get; set; }
        public bool ViewableAllow { get; set; }
        public bool EditableContextAllow { get; set; }
        public bool EditableContextsAllow { get; set; }
        public bool EditableContAllow { get; set; }
        public bool ViewableContextAllow { get; set; }
        public bool ViewableContextsAllow { get; set; }
        public bool ViewableContAllow { get; set; }

        public bool GridMinimumRowsAllow { get; set; }
        public bool GridMaximumRowsAllow { get; set; }
        public bool GridRequiredMinimumRowsAllow { get; set; }

        public bool ShowInGridAllow { get; set; }

        public bool GridCanAddAllow { get; set; }
        public bool GridAddButtonNameAllow { get; set; }
        public bool GridCanEditAllow { get; set; }
        public bool GridEditButtonNameAllow { get; set; }
        public bool GridCanViewAllow { get; set; }
        public bool GridViewButtonNameAllow { get; set; }
        public bool GridCanDeleteAllow { get; set; }
        public bool GridDeleteButtonNameAllow { get; set; }
        public bool GridShowDeleteConfirmationAllow { get; set; }
        public bool GridDeleteConfirmationMessageAllow { get; set; }
        public bool GridAllowFilterAllow { get; set; }
        public bool GridAllowSortingAllow { get; set; }
        public bool GridAllowPagingAllow { get; set; }
        public bool GridPageSizeAllow { get; set; }



        public bool GridDeleteSuccessMessageAllow { get; set; }
        public bool DefaultDynamicMethodNameAllow { get; set; }

        public bool HyperlinkTargetAllow { get; set; }
        public bool PopupWidthAllow { get; set; }
        public bool PopupHeightAllow { get; set; }
        public bool PopupTitleAllow { get; set; }
        public bool? EnableDropdownGroupingAllow { get; set; }

        public bool RelationshipNameAllow { get; set; }
        public bool RelationshipTargetNodeAllow { get; set; }
        public bool ClientCallBackScriptAllow { get; set; }
        public bool ControlStyleAllow { get; set; }
        public bool ContainerStyleAllow { get; set; }
        public bool ControlClassAllow { get; set; }
        public bool ConainerClassAllow { get; set; }
    }


}
