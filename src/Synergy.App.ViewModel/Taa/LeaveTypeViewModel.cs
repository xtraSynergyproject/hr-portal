using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class LeaveTypeViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool ValidateEntitlement { get; set; }
        public double DefaultEntitlement { get; set; }

    }
}
