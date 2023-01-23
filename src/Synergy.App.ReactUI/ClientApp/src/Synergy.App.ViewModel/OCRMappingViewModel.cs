using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class OCRMappingViewModel : OCRMapping
    {
         
        public LayoutModeEnum LayoutMode { get; set; }
        public string CallBackMethod { get; set; }
    }
}
