using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class CampaignListItemViewModel : ViewModelBase
    {
        public long EmailCampaignListId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string EmailCampaignListName { get; set; }
    }
}
