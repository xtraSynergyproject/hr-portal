using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Controllers
{
    public class TestModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime? NullableDate { get; set; }
    }
    public class TestController : Controller
    {
        // GET: /<controller>/  
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(TestModel model)
        {
            return View(model);
        }
        public IActionResult Json()
        {
            return Json(new TestModel
            {
                Id = 1,
                Date = DateTime.UtcNow,
                NullableDate = null,
            });
        }
    }
}
