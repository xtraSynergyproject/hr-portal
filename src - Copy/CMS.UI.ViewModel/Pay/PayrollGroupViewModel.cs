using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class PayrollGroupViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }

        [Display(Name = "Start Day")]
        public int StartDay { get; set; }

        public bool IsStartDayPreviousMonth { get; set; }
        [Display(Name = "End Day")]
        public int EndDay { get; set; }

        [Display(Name = "Cut Off Start Day")]
        public int CutOffStartDay { get; set; }
        public bool IsCutOffStartDayPreviousMonth { get; set; }
        [Display(Name = "Cut Off End Day")]
        public int CutOffEndDay { get; set; }

        public PayrollIntervalEnum? PayrollInterval { get; set; }

        [Required]
        [Display(Name = "Legal Entity")]
        public string LegalEntityId { get; set; }

       
        public string ElementList { get; set; }
        public DateTime? PayrollStartDate { get; set; }
        public DateTime? PayrollEndDate { get; set; }

    }
}
