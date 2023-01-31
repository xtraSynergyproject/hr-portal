using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
namespace ERP.UI.ViewModel
{
    public class EmailCampaignSearchViewModel : SearchViewModelBase
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime ScheduledTime { get; set; }
        public DocumentStatusEnum EmailCampaignStatus { get; set; }
        public long? EmailCampaignListId { get; set; }
        public string EmailCampaignListName { get; set; }
        public long? EmailConfigId { get; set; }
        public string EmailConfigName { get; set; }
        public EmailCampaignDeliveryStatusEnum EmailCampaignDeliveryStatus { get; set; }
    }
}

