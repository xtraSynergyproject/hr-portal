using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class PageTemplateViewModel : PageTemplate
    {
        public string Json { get; set; }
        public string TableMetadataId { get; set; }
        public string ChartItems { get; set; }
        public Dictionary<string, string> Prms { get; set; }
    }
}
