using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ERP.UI.ViewModel
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
        public long OrgId { get; set; }
        public string HomeType { get; set; }
        public long UserId { get; set; }
        public ModuleEnum? ModuleName { get; set; }
        public string Url { get; set; }
        public long? PostId { get; set; }
    }
}
