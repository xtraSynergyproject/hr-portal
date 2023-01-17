using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{

    public partial class NTS_NtsCategory : NodeBase
    {        
        //public NtsTypeEnum NtsType { get; set; }
        //public long NtsId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? Level { get; set; }
       // public long? ParentId { get; set; }
       // public string NtsCategoryCode { get; set; }
       // public string TemplateCode { get; set; }
       // public string TemplateCategoryCode { get; set; }

    }
    public class R_NtsCategory_Parent_NtsCategory : RelationshipBase
    {

    }
}
