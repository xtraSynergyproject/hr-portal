using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ActivityChartViewModel : ViewModelBase
    {
        public string Lead { get; set; }
        public long? UserId { get; set; }
        public long? PropertyConsultantId { get; set; }
        public string EmpName { get; set; }
        public DateTime ActivityDate { get; set; }
        [Display(Name = "Number of Calls")]
        public long? NoOfCallsCount { get; set; }
        [Display(Name = "Number of Meetings")]
        public long? NoOfMeetingCount { get; set; }
        [Display(Name = "Number of Leads")]
        public long? NoOfLeadCount { get; set; }
        [Display(Name = "Number of Brokers")]
        public long? NoOfBrokerCount { get; set; }
        [Display(Name = "Number of Emails")]
        public long? NoOfEmailCount { get; set; }
        [Display(Name = "Number of Mobile Messages")]
        public long? NoOfMobileSMSCount { get; set; }
        public string Leadstatus { get; set; }

        public string Dates { get; set; }



    }
}

