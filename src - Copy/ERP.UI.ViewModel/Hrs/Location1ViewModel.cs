using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class Location1ViewModel : ViewModelBase
    {

        //public long LocationId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name  = "LocationName",ResourceType =typeof(ERP.Translation.Message))]
        public string Name { get; set; }

        [Display(Name = "Location Name(In Arabic)")]
        public string NameLocal { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
        [StringLength(2000)]
        public virtual string Address { get; set; }

        public long? SequenceNo { get; set; }

        public DateTime EffectiveAsOfDate { get; set; }

    }
}
