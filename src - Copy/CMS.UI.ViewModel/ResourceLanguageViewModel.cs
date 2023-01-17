using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class ResourceLanguageViewModel : ResourceLanguage
    {
         
        public LayoutModeEnum LayoutMode { get; set; }
        public string CallBackMethod { get; set; }
    }
}
