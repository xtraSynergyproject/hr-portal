using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel.Cpm
{
    public class CpmAccountsViewModel : ViewModelBase
    {
        [Required]
        [Display(Name= "Name")]
        public string Name{get;set;}
        [Required]
        public CpmAccountType? Type { get; set; }     
        public string ChildOf { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }
        [Display(Name = "Cash asset")]
        public bool CashAsset { get; set; }
        [Display(Name = "Sub Account")]
        public long? SubAccount { get; set; }
        public string SubAccountName { get; set; }



    }
}
