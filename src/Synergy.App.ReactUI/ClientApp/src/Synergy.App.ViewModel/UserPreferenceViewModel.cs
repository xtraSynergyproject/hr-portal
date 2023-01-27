using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class UserPreferenceViewModel: UserPreference
    {
        public string UserName { get; set; }
        public string PreferencePortalName { get; set; }
        public string DefaultLandingPageName { get; set; }
        public List<PageViewModel> PageList { get; set; }
    }
}
