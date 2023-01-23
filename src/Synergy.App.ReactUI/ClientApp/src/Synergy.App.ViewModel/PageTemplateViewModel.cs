using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class PageTemplateViewModel : PageTemplate
    {
        public string Json { get; set; }
        public string TableMetadataId { get; set; }
        public string ChartItems { get; set; }
        public Dictionary<string, string> Prms { get; set; }
    }
}
