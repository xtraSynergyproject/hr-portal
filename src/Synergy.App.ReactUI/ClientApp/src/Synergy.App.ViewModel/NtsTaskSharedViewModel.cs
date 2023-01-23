using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class NtsTaskSharedViewModel : NtsTaskShared
    {
        public string Name { get; set; }
        public string PhotoId { get; set; }
        public string Type { get; set; }
        public bool IsSharingEnabled { get; set; }

        public string UserId { get; set; }
    }
}
