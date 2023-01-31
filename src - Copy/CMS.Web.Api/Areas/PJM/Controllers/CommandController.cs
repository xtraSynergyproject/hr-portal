using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Web.Api.Areas.PJM.Controllers
{
    public class CommandController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
