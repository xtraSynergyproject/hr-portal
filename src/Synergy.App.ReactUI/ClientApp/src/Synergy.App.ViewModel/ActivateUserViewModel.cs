using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class ActivateUserViewModel : ViewModelBase
    {
        public bool IsActivated { get; set; }
        public string Url { get; set; }
        public string Layout { get; set; }

    }
}
