using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class NtsTaskIndexPageViewModel : TaskIndexPageTemplateViewModel
    {
        public string CategoryCode { get; set; }
        public string TemplateCode { get; set; }
        public string ModuleCode { get; set; }
        public string DisplayName { get; set; }
        public long TotalCount { get; set; }
        public string IconFileId { get; set; }
    }
}
