using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class PageContentViewModel : PageContent
    {
        public DataActionEnum DataAction { get; set; }

        public bool IsLatest { get; set; }
        public string PageContentId { get; set; }

    }
}
