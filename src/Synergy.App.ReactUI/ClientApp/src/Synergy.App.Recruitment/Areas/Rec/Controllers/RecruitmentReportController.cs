using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Rcl.Rec.Areas.Rec.Controllers
{
    public class RecruitmentReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
