using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class CostCenterSearchViewModel : DatedSearchViewModelBase
    {

        public long CostCenterId { get; set; }

        [StringLength(200)]
        [Display(Name = "CostCenter Code")]
        public string Code { get; set; }

        [Display(Name = "CostCenter Name")]
        public string Name { get; set; }
    }
}
