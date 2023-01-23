﻿using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Web.Api.Areas.Common.Controllers
{
    [Route("common/command")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public CommandController(AuthSignInManager<ApplicationIdentityUser> customUserManager
             , IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }
    }
}
