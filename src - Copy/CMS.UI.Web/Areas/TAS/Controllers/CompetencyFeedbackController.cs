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

namespace CMS.UI.Web.Areas.TAS.Controllers
{
    [Area("TAS")]
    public class CompetencyFeedbackController : Controller
    {

        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IUserContext _userContext;

        public CompetencyFeedbackController(IHRCoreBusiness hrCoreBusiness, IUserContext userContext)
        {
            _hrCoreBusiness = hrCoreBusiness;
            _userContext = userContext;


        }
        public async Task<IActionResult> Index()
        {


            var position = await _hrCoreBusiness.GetPositionID(_userContext.UserId);
            ViewBag.PosId = position.IsNotNull() ? position.Id : "0";
            return View();
        }



    }
}
