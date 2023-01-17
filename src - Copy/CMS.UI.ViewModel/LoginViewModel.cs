using CMS.Common;
using CMS.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class LoginViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //[Required]

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
        public string PortalId { get; set; }
        public string LoginViewName { get; set; }
        public string Layout { get; set; }
        public string RenderMode { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string ConfirmPassword { get; set; }
        public string Portal { get; set; }
        public virtual bool SendWelcomeEmail { get; set; }
        public bool EnableTwoFactorAuth { get; set; }
        public string TwoFactorAuthOTP { get; set; }
        public TwoFactorAuthTypeEnum? TwoFactorAuthType { get; set; }
        public int Time { get; set; }
        public string AuthenticateViewName { get; set; }
        public string PhotoId { get; set; }
        public string UserId { get; set; }


    }
}
