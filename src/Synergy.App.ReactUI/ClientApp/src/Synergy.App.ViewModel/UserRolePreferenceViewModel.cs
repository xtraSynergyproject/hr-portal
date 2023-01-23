using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class UserRolePreferenceViewModel: UserRolePreference
    {
        public string PreferencePortalName { get; set; }
        public string DefaultLandingPageName { get; set; }
    }
}
