using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    [CompanyNotRequired]
    public partial class NTS_ServiceTask : NodeBase
    {
        //public long ServiceTaskId { get; set; }
        public long ServiceId { get; set; }
        public long TaskId { get; set; }
        public NtsServiceTaskTypeEnum? ServiceTaskType { get; set; }
        public long ServiceTaskTemplateId { get; set; }
        //[NonSerialized]
        //private NTS_ServiceTaskTemplate _ServiceTaskTemplate;
         
    }
}
