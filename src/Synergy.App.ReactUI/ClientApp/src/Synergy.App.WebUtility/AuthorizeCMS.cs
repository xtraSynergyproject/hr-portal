using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Synergy.App.WebUtility
{
   
    public class AuthorizeCMS : AuthorizationHandler<AuthorizeCMS>, IAuthorizationRequirement
    {
        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizeCMS requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.AuthorizationDecision))
            {
                context.Fail();
                return;
            }
            var userPortals = context.User.FindFirst(c => c.Type == ClaimTypes.AuthorizationDecision).Value.Split(',');
            if (userPortals.Any(x=>x=="CMS"))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
