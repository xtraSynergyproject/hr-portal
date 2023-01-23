using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Synergy.App.Common;
using Synergy.App.DataModel;

namespace Synergy.App.ViewModel
{
    public class GrantAccessViewModel : GrantAccess
    {
        public string UserName { get; set; }
    }
}
