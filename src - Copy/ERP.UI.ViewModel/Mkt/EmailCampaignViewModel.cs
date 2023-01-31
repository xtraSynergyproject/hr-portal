using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class EmailCampaignViewModel : ViewModelBase
    {
        [Required]
        [Display(Name = "Campaign Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Email Subject")]
        public string Subject { get; set; }
        public string Body { get; set; }
        [Required]
        [Display(Name = "Scheduled Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateTimeFormat)]
        public DateTime ScheduledTime { get; set; }
        [Required]
        [Display(Name = "Email Campaign Status")]
        public DocumentStatusEnum EmailCampaignStatus { get; set; }
        [Required]
        [Display(Name = "Email Campaign List")]
        public long? EmailCampaignListId { get; set; }
        [Display(Name = "Email Campaign List")]
        public string EmailCampaignListName { get; set; }
        [Required]
        [Display(Name = "Email Send From")]
        public long? EmailConfigId { get; set; }
        [Display(Name = "Email Send From")]
        public string EmailConfigName { get; set; }
        [Display(Name = "Delivery Status")]
        public EmailCampaignDeliveryStatusEnum EmailCampaignDeliveryStatus { get; set; }
    }
}


