using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class TemplateCategoryViewModel : TemplateCategory
    {
        public LayoutModeEnum? LayoutMode { get; set; }
        public string PopupCallbackMethod { get; set; }
        public NtsViewTypeEnum? ViewType { get; set; }
        public string ClassName { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public bool CopyInPage { get; set; }
        public bool CopyInForm { get; set; }
        public bool CopyInNote { get; set; }
        public bool CopyInTask { get; set; }
        public bool CopyInService { get; set; }
        public bool CopyInCustom { get; set; }
    }
}
