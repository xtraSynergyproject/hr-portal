using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.UI.ViewModel;
using Kendo.Mvc.UI;
using CMS.Business;
using Kendo.Mvc.Extensions;

namespace CMS.UI.Web.Areas.LMS.Controllers
{
    [Area("LMS")]
    public class LearningController : Controller
    {
        ILearningBusiness _learningBusiness;

        public LearningController(ILearningBusiness learningBusiness)
        {
            _learningBusiness = learningBusiness;
        }
        public IActionResult Index()
        {
            var model = new LearningPlanViewModel();
            return View(model);
        }

        public async Task<ActionResult> ReadData([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _learningBusiness.GetLearningPlanData();
            var json = Json(result.ToDataSourceResult(request));
            return json;
        }
    }
}
