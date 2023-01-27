using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class SalHistoryViewModel : ViewModelBase
    {
        public string Lead { get; set; }
        public string Project { get; set; }
        //public string Unit { get; set; }
        public string PropertyCosultant { get; set; }
        //public long Calls { get; set; }
        //public long Meetings { get; set; }
        //public long Emails { get; set; }
        //public long MobileSMS { get; set; }
        public string Activity { get; set; }
        public DateTime Date { get; set; }
        public string LeadStatus { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

        public long PositionId { get; set; }
    }
}

