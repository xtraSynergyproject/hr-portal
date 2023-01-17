using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class LocationViewModel : ViewModelBase
    {

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[StringLength(200)]
        [StringLength(Constant.NameStringLength1, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "Name", ResourceType = typeof(ERP.Translation.Hrs.Location))]
        public string Name { get; set; }

        [Display(Name = "NameLocal", ResourceType = typeof(ERP.Translation.Hrs.Location))]
        public string NameLocal { get; set; }

        [StringLength(Constant.LongStringLength, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "Description", ResourceType = typeof(ERP.Translation.Hrs.Location))]
        public string Description { get; set; }
        [StringLength(Constant.LongStringLength, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "Address", ResourceType = typeof(ERP.Translation.Hrs.Location))]
        public virtual string Address { get; set; }
        [Display(Name = "SequenceNo", ResourceType = typeof(ERP.Translation.Hrs.Location))]
        public long? SequenceNo { get; set; }
        [Display(Name = "EffectiveAsOfDate", ResourceType = typeof(ERP.Translation.Hrs.Location))]
        public DateTime EffectiveAsOfDate { get; set; }


    }
}
