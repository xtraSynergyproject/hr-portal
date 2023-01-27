using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class FieldViewModel : ViewModelBase
    {
        public long FieldId { get; set; }
        public string Name { get; set; }
        public string PartialViewName { get; set; }
        public string DefaultDataTypeCode { get; set; }
        public string DefaultDataTypeName { get; set; }
        public bool IsSelectionMultiValued { get; set; }
        public bool IsDataSourceMultiValued { get; set; }
        public long? SequenceNo { get; set; }
    } 
}
