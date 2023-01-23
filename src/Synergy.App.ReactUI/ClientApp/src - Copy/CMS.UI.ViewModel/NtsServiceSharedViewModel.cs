using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class NtsServiceSharedViewModel : NtsServiceShared
    {
        public string Name { get; set; }
        public string PhotoId { get; set; }
        public string Type { get; set; }
        public bool IsSharingEnabled { get; set; }

        public string UserId { get; set; }
    }
}
