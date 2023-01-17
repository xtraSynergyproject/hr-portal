using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class ProcessDesignViewModel : ProcessDesign
    {
        public string Json { get; set; }
        public List<ComponentViewModel> Components { get; set; }            
        public PageViewModel Page { get; set; }
        public string PageId { get; set; }
        public string RecordId { get; set; }
        public string PortalName { get; set; }
        public bool EnableIndexPage { get; set; }
        public FlowYObject FlowYdata { get; set; }
    }
    public class FlowYObject
    {
        public string html { get; set; }
        public dynamic blocks { get; set; }
        public dynamic blockarr { get; set; }
    }
}
