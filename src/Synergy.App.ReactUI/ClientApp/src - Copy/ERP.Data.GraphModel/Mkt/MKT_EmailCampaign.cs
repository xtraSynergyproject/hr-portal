using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
 
    public partial class MKT_EmailCampaign : NodeBase
    {        
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; } 
        public DateTime ScheduledTime { get; set; }
        public DocumentStatusEnum EmailCampaignStatus { get; set; }
        public EmailCampaignDeliveryStatusEnum EmailCampaignDeliveryStatus { get; set; }
    }
    public class R_EmailCampaign_EmailCampaignList : RelationshipBase
    {

    }
    public class R_EmailCampaign_EmailFrom_EmailConfig : RelationshipBase
    {

    }
}
