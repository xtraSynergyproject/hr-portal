using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class GrantAccessViewModel : ViewModelBase
    {        
        public long GrantedByUserId { get; set; }
        [Display(Name = "User")]
        [Required]
        public long GrantedToUserId { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Grant Status")]
        public GrantStatusEnum GrantStatus { get; set; }
        [Display (Name = "User Name")]
        public string UserName { get; set; }

    }
}


