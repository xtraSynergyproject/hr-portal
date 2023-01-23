using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel.Rec
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string Mode { get; set; }

        public string ForgotPasswordOTP { get; set; }
        [StringLength(50)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [StringLength(50)]
        [Display(Name = "Re-type Password")]
        public string ConfirmPassword { get; set; }

    }
}
