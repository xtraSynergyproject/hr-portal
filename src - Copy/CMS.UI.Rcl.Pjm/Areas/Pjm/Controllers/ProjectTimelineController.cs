using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CMS.UI.ViewModel;
using CMS.UI.Utility;

namespace CMS.UI.Web.Areas.PJM.Controllers
{
    [Area("PJM")]
    public class ProjectTimelineController : ApplicationController
    {
        public IActionResult Index(int year, int month)
        {
            ViewBag.Dates = GetDates(2021, 4);
            return View();
        }

        public static List<DateTime> GetDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList(); // Load dates into a list
        }

        public async Task<IActionResult> Settings(string id, string nodes)
        {
            var list = new List<ComponentViewModel>();
            var model = JsonConvert.DeserializeObject<List<ComponentViewModel>>(nodes);
            ViewBag.CurrentNode = model.Where(x => x.Id == id).FirstOrDefault();
            var m = model.Where(x => x.Id != id).ToList();
            return View(m);
        }
    }
}
