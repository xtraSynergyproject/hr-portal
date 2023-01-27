using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMS.Business;
using Microsoft.AspNetCore.Authorization;
using CMS.UI.ViewModel;
using CMS.Common;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Kendo.Mvc.UI;
using System.Data;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json.Linq;
using CMS.Data.Model;
using AutoMapper;
using System.Web;

namespace CMS.UI.Web.Controllers
{
    public class CmsFormController //: ApplicationController
    {

        private readonly ICmsBusiness _cmsFormBusiness;
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITempDataProvider _tempDataProvider;
        public CmsFormController(ICmsBusiness cmsFormBusiness
            , IRazorViewEngine razorViewEngine
            , IHttpContextAccessor contextAccessor
            , ITempDataProvider tempDataProvider)
        {
            _cmsFormBusiness = cmsFormBusiness;
            _razorViewEngine = razorViewEngine;
            _contextAccessor = contextAccessor;
            _tempDataProvider = tempDataProvider;
        }
       

    }
}
