using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{    
    public class OCRTemplateMapping : ViewModelBase
    {
        public string OCRKeyword { get; set; }
        public string OCRUdfFieldName { get; set; }
        public int Coordinate1 { get; set; }
        public int Coordinate2 { get; set; }
        public int Coordinate3 { get; set; }
        public int Coordinate4 { get; set; }
        public long? TemplateId { get; set; }
        public long? TemplateFieldId { get; set; }

    }
}
