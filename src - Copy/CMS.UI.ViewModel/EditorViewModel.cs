using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
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
