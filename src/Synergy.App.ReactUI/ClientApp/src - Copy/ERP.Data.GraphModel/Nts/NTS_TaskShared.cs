using ERP.Utility;

namespace ERP.Data.GraphModel
{

    public partial class NTS_TaskShared : NodeBase
    {
        public AssignToTypeEnum? SharedType { get; set; }
        public ulong? SharedTo { get; set; }
        public ulong? TeamId { get; set; }
       
        public class R_TaskShared_Task : RelationshipBase
        {

        }
        public class R_TaskShared_TaskShared : RelationshipBase
        {

        }
    }
}
