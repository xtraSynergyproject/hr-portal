using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class LeaveTypeViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool ValidateEntitlement { get; set; }
        public double DefaultEntitlement { get; set; }

    }
}
