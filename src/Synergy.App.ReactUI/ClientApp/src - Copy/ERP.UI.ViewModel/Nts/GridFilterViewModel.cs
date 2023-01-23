using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ERP.UI.ViewModel
{
    public class GridFilterViewModel
    {
        public string ConvertedValue { get; }
        public string Member { get; set; }
        public string MemberType { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public string Field { get; set; }
        public string FieldName { get { return Field ?? Member; } }
        public List<GridFilterViewModel> FilterDescriptors { get; set; }

    }
    public class DynamicUdfViewModel
    {
        public string FieldName { get; set; }
        public string LabelDisplayName { get; set; }
        public string PartialViewName { get; set; }
        public string DataSourceControllerName { get; set; } 
        public string DataSourceActionName { get; set; }
        public string DataSourceHtmlAttributesString { get; set; }
        public int SequenceNo { get; set; }
    }
}
