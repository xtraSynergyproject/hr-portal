
namespace ERP.Data.GraphModel
{
  
    public partial class NTS_TemplateFieldConfig : NodeBase
    {        
        public long FieldId { get; set; }
        //public FieldConfigTypeEnum ConfigType { get; set; } 
        public string Query { get; set; }
    }
    public class R_TemplateFieldConfig_TemplateField : RelationshipBase
    {

    }
}
