using CMS.Common;
using CMS.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class SwitchProfileViewModel
    {
        [Required]
        [Display(Name = "Switch To User")]
        public string SwitchToUserId { get; set; }
        public LoggedInAsTypeEnum? LoggedInAsType { get; set; }

        [Display(Name = "Select Portal")]
        public string SwitchPortalId { get; set; }
    }
}