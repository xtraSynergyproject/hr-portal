using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ActivityReportsViewModel : ViewModelBase
    {
        public string Lead { get; set; }

        public string EmpName { get; set; }

        [Display(Name = "Number of Calls")]
        public long? NoOfCallsCount { get; set; }
        [Display(Name = "Number of Meetings")]
        public long? NoOfMeetingCount { get; set; }
        [Display(Name = "Number of Emails")]
        public long? NoOfEmailCount { get; set; }
        [Display(Name = "Number of Mobile SMS")]
        public long? NoOfMobileSMSCount { get; set; }
        [Display(Name = "Number of Leads")]
        public long? NoOfLeadCount { get; set; }
        [Display(Name = "Number of Brokers")]
        public long? NoOfBrokerCount { get; set; }
        public string Leadstatus { get; set; }

        public long? PropertyConsultantId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        [DateCompare("StartDate")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        public string Target { get; set; }
    }
}

