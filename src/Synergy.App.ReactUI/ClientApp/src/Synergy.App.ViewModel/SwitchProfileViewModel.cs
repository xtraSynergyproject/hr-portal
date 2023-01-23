using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
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