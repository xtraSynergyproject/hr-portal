using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{

    public partial class GEN_NotificationGroup : NodeBase
    {
        public string Subject { get; set; }
        public string Body { get; set; }
    }

}
