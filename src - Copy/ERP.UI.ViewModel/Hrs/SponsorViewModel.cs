using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class SponsorViewModel : ViewModelBase
    {
        [Required]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Sponsor Name")]
        public string Name { get; set; }

        public long? SequenceNo { get; set; }        
        public DateTime EffectiveAsOfDate { get; set; }
        [Display(Name = "Logo")]
        public long? LogoId { get; set; }
        public FileViewModel SelectedFile { get; set; }
    }
}
