using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class NationalityViewModel : ViewModelBase
    {
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Nationality Name")]
        public string Name { get; set; }

        [Display(Name = "Nationality Name(In Arabic)")]
        public string NameLocal { get; set; }

        public bool IsNational { get; set; }

        public Nullable<long> SequenceNo { get; set; }

        public DateTime EffectiveAsOfDate { get; set; }

        [Display(Name = "Average Economy Ticket Cost")]
        public double? AverageEconomyTicketCost { get; set; }

        [Display(Name = "Average Business Ticket Cost")]
        public double? AverageBusinessTicketCost { get; set; }
        
    }
}
