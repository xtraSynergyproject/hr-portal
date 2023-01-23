using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class AccrualViewModel : ViewModelBase
    {
        //[Required]
        [Display(Name = "Accrual (Month Year)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime AccrualDate { get; set; }

        public bool VacationAccrual { get; set; }
        public bool FlightTicketAccrual { get; set; }
        public bool LoanAccrual { get; set; }
        public bool EOSAccrual { get; set; }

    }
}
