

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class ReportSchedulerViewModel : ViewModelBase
    {

        public string ReportLOVCode { get; set; }
       
        [Display(Name = "Organization Name")]
        public long? OrganizationId { get; set; }
        [Display(Name = "Organization Name")]
        public string OrganizationName { get; set; }
        [Required]
        [Display(Name = "Report Frequency")]
        public ReportFrequencyEnum ReportFrequency { get; set; }
        [Required]
        [Display(Name = "Report Schedule Time")]
        public TimeSpan ReportScheduleTime { get; set; }
        [Display(Name = "Report Schedule Time")]
        public string ReportScheduleTimeGrid { get; set; }
        [Required]
        [Display(Name = "EmailIds")]
        public string EmailIds { get; set; }

        [Display(Name = "Email Subject")]
        public string EmailSubject { get; set; }

        [Display(Name = "Email Description")]
        public string EmailDescription { get; set; }
        public string EmailBody { get; set; }
        [Required]
        [Display(Name = "Legal Entity")]
        public long? LegalEntityId { get; set; }
        [Display(Name = "Legal Entity")]
        public string LegalEntityCode { get; set; }
        [Display(Name = "Legal Entity")]
        public string LegalEntityName { get; set; }
    }
}
