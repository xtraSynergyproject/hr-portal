using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class CustomTemplateViewModel : CustomTemplate
    {
        public PageViewModel Page { get; set; }
        public string PageId { get; set; }
        public string RecordId { get; set; }
        public string PortalName { get; set; }
        public string DashoardId { get; set; }
        public string WebsiteId { get; set; }
    }
}
