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
    public class SearchViewModelBase
    {
        public long Id { get; set; }
        public bool ForAllCompany { get; set; }

        [Display(Name = "Status")]
        public virtual StatusEnum? Status { get; set; }
        
        public DataOperation? Operation { get; set; }
    }
}
