using CMS.Business;
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Web.Api.Controllers
{
    public class ApiController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        public ApiController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        internal async Task Authenticate(string userId, string portalName = "", string legalEntityCode = "")
        {
            var ub = _serviceProvider.GetService<IUserBusiness>();

            var user = await ub.ValidateUserById(userId);
            if (user != null)
            {
                HttpContext.Items["UserId"] = user.Id;
                HttpContext.Items["UserName"] = user.Name;
                HttpContext.Items["IsSystemAdmin"] = user.IsSystemAdmin;
                HttpContext.Items["Email"] = user.Email;
                HttpContext.Items["UserUniqueId"] = user.Email;
                HttpContext.Items["CompanyId"] = user.CompanyId;
                HttpContext.Items["CompanyCode"] = user.CompanyCode;
                HttpContext.Items["CompanyName"] = user.CompanyName;
                HttpContext.Items["JobTitle"] = user.JobTitle;
                HttpContext.Items["PhotoId"] = user.PhotoId;
                HttpContext.Items["UserRoleCodes"] = string.Join(",", user.UserRoles.Select(x => x.Code));
                HttpContext.Items["UserRoleIds"] = string.Join(",", user.UserRoles.Select(x => x.Id));
                HttpContext.Items["UserPortals"] = user.UserPortals;
                HttpContext.Items["PersonId"] = user.PersonId;
                HttpContext.Items["PositionId"] = user.PositionId;
                HttpContext.Items["DepartmentId"] = user.DepartmentId;
                if (portalName.IsNotNullAndNotEmpty())
                {
                    var portal = await ub.GetSingle<PortalViewModel, Portal>(x => x.Name == portalName);
                    if (portal != null)
                    {
                        HttpContext.Items["PortalId"] = portal.Id;
                    }
                }
                else
                {
                    var portal = await ub.GetSingle<PortalViewModel, Portal>(x => x.Name == "HR");
                    if (portal != null)
                    {
                        HttpContext.Items["PortalId"] = portal.Id;
                    }
                }
                if (legalEntityCode.IsNotNullAndNotEmpty())
                {
                    var le = await ub.GetSingle<LegalEntityViewModel, LegalEntity>(x => x.Code == legalEntityCode);
                    if (le != null)
                    {
                        HttpContext.Items["LegalEntityId"] = le.Id;
                        HttpContext.Items["LegalEntityCode"] = le.Code;
                    }
                }
                else
                {
                    HttpContext.Items["LegalEntityId"] = user.LegalEntityId;
                    HttpContext.Items["LegalEntityCode"] = user.LegalEntityCode;
                }

            }
        }
    }
}
