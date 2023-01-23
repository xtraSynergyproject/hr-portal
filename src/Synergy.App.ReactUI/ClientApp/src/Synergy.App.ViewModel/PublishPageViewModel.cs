using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class PublishPageViewModel : PagePublished
    {
        public PortalViewModel Portal { get; set; }
        public DataActionEnum DataAction { get; set; }       
        public List<PublishPageContentViewModel> Groups { get; set; }
        public List<PublishPageContentViewModel> Columns { get; set; }
        public List<PublishPageContentViewModel> Cells { get; set; }
        public List<PublishPageContentViewModel> Components { get; set; }
    }
}
