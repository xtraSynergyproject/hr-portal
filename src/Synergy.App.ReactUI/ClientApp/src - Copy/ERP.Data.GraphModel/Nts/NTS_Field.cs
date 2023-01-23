
namespace ERP.Data.GraphModel
{
   
  
    public partial class NTS_Field : NodeBase
    {
        public string Name { get; set; }
        public string PartialViewName { get; set; }
        public string DefaultFieldWidth { get; set; }
     
        public bool IsSelectionMultiValued { get; set; }
        public bool IsDataSourceMultiValued { get; set; }
        
    }
    public class R_Field_DefaultDataType_ListOfValue : RelationshipBase
    {
        
    }
}
