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
    public class EndlessScrollingRequest
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public long UserId { get; set; }
        public long OrgId { get; set; }
        public string SearchParam { get; set; }
        public string HomeType { get; set; }
        public long LoggendInUserId { get; set; }
        public ModuleEnum? ModuleName { get; set; }
        public string Url { get; set; }
        public long? PostId { get; set; }

    }
}
