using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Property.Controllers
{
    public class HomeTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
