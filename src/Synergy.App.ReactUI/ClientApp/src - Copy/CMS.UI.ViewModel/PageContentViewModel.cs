using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class PageContentViewModel : PageContent
    {
        public DataActionEnum DataAction { get; set; }

        public bool IsLatest { get; set; }
        public string PageContentId { get; set; }

    }
}
