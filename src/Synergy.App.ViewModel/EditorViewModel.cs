using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class EditorViewModel : Editor
    {

        public string DisplayField { get; set; }
        public string HtmlField { get; set; }
        public string JsonField { get; set; }
        public string CallBackMethod { get; set; }
        public string JsonData { get; set; }
    }
}
