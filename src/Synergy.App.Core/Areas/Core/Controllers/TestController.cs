using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using System.ComponentModel.DataAnnotations;
using Synergy.App.Common;

namespace CMS.UI.Web.Controllers
{
    public class TestModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime? NullableDate { get; set; }
    }
    [Area("Core")]
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
            var k = ModelState;
            var asask = ModelState.Values;
            var zx = ModelState.Values.FirstOrDefault();
            var ka = ModelState.SerializeErrors();
            return View(model);
        }
        public IActionResult Json()
        {
            return Json(new TestModel
            {
                Date = DateTime.UtcNow,
                NullableDate = null,
            });
        }
    }
}
