using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class BankBranchViewModel : ViewModelBase
    {
        [Display(Name = "Branch Name")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Branch Code")]
        [Required]
        public string Code { get; set; }
        [Display(Name = "Swift Code")]
        public string SwiftCode { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }

        [Display(Name = "Bank Name")]
        public long? BankId { get; set; }
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
    }
}
