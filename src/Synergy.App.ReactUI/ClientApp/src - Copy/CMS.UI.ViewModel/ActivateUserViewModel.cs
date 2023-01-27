using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class ActivateUserViewModel : ViewModelBase
    {
        public bool IsActivated { get; set; }
        public string Url { get; set; }
        public string Layout { get; set; }

    }
}
