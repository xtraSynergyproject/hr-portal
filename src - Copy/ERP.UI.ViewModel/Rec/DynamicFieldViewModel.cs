

namespace ERP.UI.ViewModel
{
    public class DynamicFieldViewModel
    {
        public long FieldId { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string PartialViewName { get; set; }
        public string LabelDisplayName { get; set; }
        public string DefaultValueStatic { get; set; }
        public string Weightage { get; set; }
        public string Value { get; set; }
        public string DataSourceHtmlAttributesString { get; set; }
    }
}
