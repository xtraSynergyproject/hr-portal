using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;

namespace CMS.Web.Scheduler.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        private readonly IUserBusiness _userBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public LoginModel(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager, IUserBusiness userBusiness,
            AuthSignInManager<ApplicationIdentityUser> customUserManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _userBusiness = userBusiness;
            _customUserManager = customUserManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
            public string Layout { get; set; }
            public string PortalId { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null, string theme = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ReturnUrl = returnUrl;
            ViewData["Layout"] = "/Views/Shared/_LoginLayout.cshtml";
            if (theme.IsNotNullAndNotEmpty())
            {
                ViewData["Layout"] = $"~/Views/Shared/Themes/{theme}/_LoginLayout.cshtml";
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userBusiness.ValidateLogin(Input.Email, Input.Password);
            //var user = new UserViewModel
            //{
            //    Id=Guid.Empty.ToString(),
            //    Name="Admin",
            //    Email="admin@synergy.com",
            //    CompanyId= Guid.Empty.ToString(),
            //    JobTitle="Engineer",
            //    PhotoId= Guid.Empty.ToString()

            //};
            if (user != null)
            {
                if (user.Status == StatusEnum.Inactive)
                {
                    ModelState.AddModelError(string.Empty, "User is inactive. Please activate the user or contact administrator");
                    ViewData["Layout"] = Input.Layout;
                    return Page();
                }
                var id = new ApplicationIdentityUser
                {
                    Id = user.Id,
                    UserName = user.Name,
                    IsSystemAdmin = user.IsSystemAdmin,
                    Email = user.Email,
                    UserUniqueId = user.Email,
                    CompanyId = user.CompanyId,
                    CompanyCode = user.CompanyCode,
                    CompanyName = user.CompanyName,
                    JobTitle = user.JobTitle,
                    PhotoId = user.PhotoId,
                    UserRoleCodes = string.Join(",", user.UserRoles.Select(x => x.Code)),
                    UserRoleIds = string.Join(",", user.UserRoles.Select(x => x.Id)),
                    UserPortals=user.UserPortals
                //    CandidateId = user.CandidateId
                };
                var result = await _customUserManager.PasswordSignInAsync(id, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // SetSession(user);
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid User Name/Password");
                    ViewData["Layout"] = Input.Layout;
                    return Page();
                }


            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                ViewData["Layout"] = Input.Layout;
                return Page();
            }
        }
    }
}
