using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class NtsGroupViewModel : NtsGroup
    {
        public List<string> TemplateIds { get; set; }
        public List<string> UserGroupIds { get; set; }
    }
}
