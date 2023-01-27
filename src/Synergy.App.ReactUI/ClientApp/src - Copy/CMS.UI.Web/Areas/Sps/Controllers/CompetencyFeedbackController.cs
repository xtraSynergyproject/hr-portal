using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Sps.Controllers
{
    [Area("Sps")]
    public class CompetencyFeedbackController : Controller
    {
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IUserContext _userContext;
        private readonly ISuccessionPlanningBusiness _SuccessionPlanning;


        public CompetencyFeedbackController(IHRCoreBusiness hrCoreBusiness, IUserContext userContext, ISuccessionPlanningBusiness SuccessionPlanning)
        {
            _hrCoreBusiness = hrCoreBusiness;
            _userContext = userContext;
            _SuccessionPlanning = SuccessionPlanning;


        }
        public async Task<IActionResult> Index(string SubordinateId)
        {
            CompetencyFeedbackDetailsViewModel Model = new CompetencyFeedbackDetailsViewModel();

            if (SubordinateId.IsNotNull())
            {
                Model.Userid = SubordinateId;
                Model.ToppositiveCompetency = await _SuccessionPlanning.GetCompetencyTopName(SubordinateId);
                Model.AreaofDevelopment = await _SuccessionPlanning.GetAreDevelopmentCompetencyTopName(SubordinateId);

            }
           // else {
           //
           //
           //     List<CompetencyFeedbackUserViewModel> mo = new List<CompetencyFeedbackUserViewModel>();
           //
           //     mo.Add(new CompetencyFeedbackUserViewModel { CompetencyName = "Competen 1", Rating = 60 });
           //     Model.ToppositiveCompetency = mo;
           // }

            var position = await _hrCoreBusiness.GetPositionID(_userContext.UserId);
            ViewBag.PosId = position.IsNotNull() ? position.Id : "0";
            return View(Model);
        }


        [HttpGet]
        public async Task<ActionResult> GetFeedbackUserList(string subordinateId)
        {


            var data = new List<CompetencyFeedbackUserViewModel>();
            if (subordinateId != null)
            {
                var data1 = await _SuccessionPlanning.GetTopFeedbackUser(subordinateId);
                return Json(data1);
            }
            return Json(data);
        }

        public async Task<ActionResult> GetFeedbackChart(string subordinateId)
        {

            //List<CompetencyFeedbackUserViewModel> mo = new List<CompetencyFeedbackUserViewModel>();
            //mo.Add(new CompetencyFeedbackUserViewModel { CompetencyName = "Competen 1", SelfCount = 4,OhersCount=5 });
            //mo.Add(new CompetencyFeedbackUserViewModel { CompetencyName = "Competen 2", SelfCount = 3, OhersCount = 2 });


            var data =await _SuccessionPlanning.GetChartList(subordinateId);

            return Json(data);
        }
    }
}
