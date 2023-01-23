using CMS.Business;
using CMS.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IUserBusiness _userBusiness;
        private readonly IUserContext _userContext;
        private readonly IPortalBusiness _portalBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public AuthenticateController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IConfiguration configuration
            , IUserBusiness userBusiness
            , IUserContext userContext
            , IPortalBusiness portalBusiness)
        {
            _customUserManager = customUserManager;
            _configuration = configuration;
            _userBusiness = userBusiness;
            _userContext = userContext;
            _portalBusiness = portalBusiness;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            var user = await _userBusiness.ValidateLogin(model.Email.Trim(), model.Password.Trim());
            if (user != null)
            {
                
                if (user.Status==StatusEnum.Inactive)
                {
                    return Unauthorized();
                }
                var identity = new ApplicationIdentityUser
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
                    UserPortals = user.UserPortals,
                    LegalEntityId = user.LegalEntityId,
                    LegalEntityCode = user.LegalEntityCode,
                    PersonId = user.PersonId,
                    PositionId = user.PositionId,
                    DepartmentId = user.DepartmentId,
                };
                identity.MapClaims();
                var result = await _customUserManager.PasswordSignInAsync(identity, model.Password, true, lockoutOnFailure: false);

                if (result.Succeeded)
                {


                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(6),
                        claims: identity.Claims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
            }
            return Unauthorized();
        }


        [HttpPost]
        [Route("AuthenticateLogin")]
        public async Task<IActionResult> AuthenticateLogin(string username,string password)
        {

            var user = await _userBusiness.ValidateLogin(username.Trim(), password.Trim());
            if (user != null)
            {

                if (user.Status == StatusEnum.Inactive)
                {
                    return Unauthorized();
                }
                var identity = new ApplicationIdentityUser
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
                    UserPortals = user.UserPortals,
                    LegalEntityId = user.LegalEntityId,
                    LegalEntityCode = user.LegalEntityCode,
                    PersonId = user.PersonId,
                    PositionId = user.PositionId,
                    DepartmentId = user.DepartmentId,
                };
                identity.MapClaims();
                var result = await _customUserManager.PasswordSignInAsync(identity, password, true, lockoutOnFailure: false);

                if (result.Succeeded)
                {


                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(6),
                        claims: identity.Claims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(identity);
                }
                else
                {
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }

    }
}
