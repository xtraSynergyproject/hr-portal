using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class TestViewModel : ViewModelBase
    {
        public bool Bool { get; set; }
        public string Url { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public DataActionEnum? DataActionEnum { get; set; }
        public int Int { get; set; }
        public long Long { get; set; }
        public double Double { get; set; }

    }
}
