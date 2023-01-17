using Synergy.App.DataModel;
using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class ChangePassowrdViewModel
    {
        public string UserId { get; set; }
        public string CurrentPassword { get; set; }

        // [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()])(?!.*userName)(?!.*(.)\\1{2,}).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and must contain the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
    }
}
