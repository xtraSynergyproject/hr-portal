using System;
using System.Collections.Generic;

namespace ERP.Data.GraphModel
{
    public partial class SAL_Currency : NodeBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string CodeAr { get; set; }
        public string NameAr { get; set; }
        public string NameBeforePoint { get; set; }
        public string NameBeforePointAr { get; set; }
        public string NameAfterPoint { get; set; }
        public string NameAfterPointAr { get; set; }

    }

    public class R_Currency_Country : RelationshipBase
    {

    }
}
