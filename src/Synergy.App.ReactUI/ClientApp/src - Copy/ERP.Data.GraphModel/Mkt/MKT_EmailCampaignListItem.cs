using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
 
    public partial class MKT_EmailCampaignListItem : NodeBase
    {        
        public string Name { get; set; }
        public string Email { get; set; } 
    }
    public class R_EmailCampaignListItem_EmailCampaignList : RelationshipBase
    {

    }
}
