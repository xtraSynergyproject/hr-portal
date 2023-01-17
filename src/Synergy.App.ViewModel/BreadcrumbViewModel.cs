using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class BreadcrumbViewModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }

        public bool IsNotClickable { get; set; }
        public bool IsClickDisabled { get; set; }
        // public string PageId { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string AreaName { get; set; }
        public string Url { get; set; }
        public string IconCss { get; set; }
        public string PageId { get; set; }
        public string RecordId { get; set; }
        public string RequestSource { get; set; }
        public string DataAction { get; set; }
        public string PageType { get; set; }
        public BreadcrumbLoadTypeEnum LoadType { get; set; }
        public string LoadContainer { get; set; }
    }

}
