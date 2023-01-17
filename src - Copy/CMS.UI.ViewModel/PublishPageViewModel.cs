using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
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
