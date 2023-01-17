﻿using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login Id")]
       // [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
