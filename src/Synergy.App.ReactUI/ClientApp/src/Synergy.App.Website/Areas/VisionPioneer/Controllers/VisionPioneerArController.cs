using Synergy.App.WebUtility;
using Microsoft.AspNetCore.Mvc;

namespace CMS.UI.Web.Areas.VisionPioneer.Controllers
{
    [Area("VisionPioneer")]
    public class VisionPioneerArController : ApplicationController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult WhoWeAre()
        {
            return View();
        }
        public IActionResult CompanyProfile()
        {
            var code = "VP_AR_COMP_PROF_PDF";
            return RedirectToAction("GetApplicationDocument", "Document", new { @area = "cms", @documentCode = code });
        }
        public IActionResult Assessment()
        {
            return View();
        }

        public IActionResult Careers()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult English()
        {
            return View();
        }
        public IActionResult Arabic()
        {
            return View();
        }

        public IActionResult RequestCallback()
        {
            return View();
        }
    }
}
