using Synergy.App.WebUtility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.PJM.Controllers
{
    [Area("PJM")]
    public class ProjectDiagramController : ApplicationController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
