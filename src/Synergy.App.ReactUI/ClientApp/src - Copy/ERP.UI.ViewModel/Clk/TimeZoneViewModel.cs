using System;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class TimeZoneViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public TimeSpan UtcOffSet { get; set; }
    }
}
