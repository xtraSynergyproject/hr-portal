using Synergy.App.DataModel;
using Synergy.App.Common;
using System;

namespace Synergy.App.ViewModel
{
    public class EndlessScrollListViewModel
    {
        public string Name { get; set; }
        public int PageSize { get; set; }
        public string Height { get; set; }
        public string CssClass { get; set; }
        public string UrlActionName { get; set; }
        public string UrlControlName { get; set; }
        public string UrlAreaName { get; set; }
        public string OrgId { get; set; }
        public string HomeType { get; set; }
        public string UserId { get; set; }
        public ModuleEnum? ModuleName { get; set; }
        public string Url { get; set; }
        public string PostId { get; set; }
    }
    public class EndlessScrollingRequest
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string UserId { get; set; }
        public string OrgId { get; set; }
        public string SearchParam { get; set; }
        public string HomeType { get; set; }
        public string LoggendInUserId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleId { get; set; }
        public string Url { get; set; }
        public string PostId { get; set; }

    }
}
