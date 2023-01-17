using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PayrollGroupViewModel : ViewModelBase
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
        public long? LegalEntityId { get; set; }

        //[Required]
        public List<long?> ElementList { get; set; }
    }
}
