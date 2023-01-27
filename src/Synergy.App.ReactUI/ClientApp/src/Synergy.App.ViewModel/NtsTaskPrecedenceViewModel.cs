using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class NtsTaskPrecedenceViewModel : NtsTaskPrecedence
    {
        public string[] PredecessorsIds { get; set; }
        public string PredecessorsName { get; set; }
        public string TaskNo { get; set; }
        // public string DurationText { get { return Duration.ToString(); } }
    }
}
