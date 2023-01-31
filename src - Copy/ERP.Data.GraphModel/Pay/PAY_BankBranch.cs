
using ERP.Utility;

namespace ERP.Data.GraphModel
{

    public partial class PAY_BankBranch : NodeBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string SwiftCode { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
    }
    public class R_BankBranch_Bank : RelationshipBase
    {

    }
}
