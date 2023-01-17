using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class RosterCopyViewModel : ViewModelBase
    {
        public DateTime CopyFromWeekStartDate { get; set; }
        [Display(Name = "Copy To Week")]
        public DateTime CopyToWeekStartDate { get; set; }
    }
}
