
using ERP.Utility;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_TemplateFieldInfo : NodeBase
    {       
        //public long TemplateFieldId { get; set; }
       // public FieldInfoTypeEnum FieldInfoType { get; set; }
        public string LanguageCode { get; set; }
        public string Name { get; set; }
    }
    public class R_TemplateFieldInfo_TemplateField : RelationshipBase
    {

    }
}
