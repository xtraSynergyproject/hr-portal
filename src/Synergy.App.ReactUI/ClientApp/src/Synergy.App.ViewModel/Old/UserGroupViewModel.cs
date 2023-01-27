using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using Synergy.App.Common;

namespace Synergy.App.ViewModel
{
   public class UserGroupViewModel:UserGroup
    {

        public List<string> UserIds { get; set; }

        public IList<string> Portal { get; set; }
        public string PortalName { get; set; }

    }
}
