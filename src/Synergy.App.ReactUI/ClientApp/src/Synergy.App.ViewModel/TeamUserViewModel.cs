using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class TeamUserViewModel : TeamUser
    {
        public DataActionEnum DataAction { get; set; }
        public string UserName { get; set; }


    }
}
