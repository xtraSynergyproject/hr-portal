using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class OCRMappingViewModel : OCRMapping
    {
         
        public LayoutModeEnum LayoutMode { get; set; }
        public string CallBackMethod { get; set; }
    }
}
