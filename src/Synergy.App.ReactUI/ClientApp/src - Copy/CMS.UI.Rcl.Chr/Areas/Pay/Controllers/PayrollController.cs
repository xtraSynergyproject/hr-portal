using CMS.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Pay.Controllers
{
    [Area("Pay")]
    public class PayrollController : ApplicationController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TestReport()
        {
            return View();
        }
    }
}
