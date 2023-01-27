using CMS.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class UserPagePreferenceController : ApplicationController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
