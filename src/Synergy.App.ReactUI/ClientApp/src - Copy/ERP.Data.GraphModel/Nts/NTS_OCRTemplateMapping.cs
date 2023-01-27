
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERP.Data.GraphModel
{

    public partial class NTS_OCRTemplateMapping : NodeBase
    {
        public string OCRKeyword { get; set; }
        public string OCRUdfFieldName { get; set; }
        public int Coordinate1 { get; set; }
        public int Coordinate2 { get; set; }
        public int Coordinate3 { get; set; }
        public int Coordinate4 { get; set; }
    }
    public class R_OCRTemplateMapping_TemplateField : RelationshipBase
    {

    }
    

}
