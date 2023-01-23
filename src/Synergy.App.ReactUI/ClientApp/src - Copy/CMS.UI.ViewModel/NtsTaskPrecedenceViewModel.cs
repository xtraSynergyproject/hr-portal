using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class NtsTaskPrecedenceViewModel : NtsTaskPrecedence
    {
        public string[] PredecessorsIds { get; set; }
        public string PredecessorsName { get; set; }
        public string TaskNo { get; set; }
        // public string DurationText { get { return Duration.ToString(); } }
    }
}
