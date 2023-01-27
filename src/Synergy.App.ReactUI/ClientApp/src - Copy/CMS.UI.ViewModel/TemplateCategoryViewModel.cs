using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class TemplateCategoryViewModel : TemplateCategory
    {
        public LayoutModeEnum? LayoutMode { get; set; }
        public string PopupCallbackMethod { get; set; }
        public NtsViewTypeEnum? ViewType { get; set; }

    }
}
