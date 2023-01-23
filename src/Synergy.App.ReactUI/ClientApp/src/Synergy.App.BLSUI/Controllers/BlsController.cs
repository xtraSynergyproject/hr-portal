using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Core.Controllers
{
    public class BlsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
