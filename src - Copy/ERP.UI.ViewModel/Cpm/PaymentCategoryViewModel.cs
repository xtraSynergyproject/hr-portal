using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace ERP.UI.ViewModel
{
    public class PaymentCategoryViewModel : ViewModelBase
    {
        [Required]
        public string Name { get; set; }
        [Display(Name = "Category Type")]
        public PaymenTypeEnum? CategoryTypeCode { get; set; }        
        [Display(Name = "Category has validity")]
        public bool CategoryValidity { get; set; }
        [Display(Name = "Apply Tax?")]
        public bool TaxApply { get; set; }
                                 
    }
}
