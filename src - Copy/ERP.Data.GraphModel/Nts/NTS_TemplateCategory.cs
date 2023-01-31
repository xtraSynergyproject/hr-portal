using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{

    public partial class NTS_TemplateCategory : NodeBase
    {       
        public NtsTypeEnum NtsType { get; set; }
        public NtsClassificationEnum NtsClassification { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }       
    }
    public class R_TemplateCategory_Parent_TemplateCategory : RelationshipBase
    {

    }
}
