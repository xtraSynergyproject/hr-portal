using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CHR.Controllers
{
    [Area("CHR")]
    public class BusinessTripController : ApplicationController
    {        
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IUserContext _userContext;

        public BusinessTripController(IHRCoreBusiness hrCoreBusiness,IUserContext UserContext)
        {
            _hrCoreBusiness = hrCoreBusiness;
            _userContext = UserContext;
        }  
        
        public ActionResult Index(string EmpId)            
        {
            //string[] spl = EmpId.Split("UserId");

            var model = new BusinessTripViewModel();
            if (EmpId.IsNotNullAndNotEmpty())
            {
                string[] spl = EmpId.Split("?Userid=");
                model.Id = spl[0];
                model.UserId = spl[1];
            }
            return View(model);
        }

        public async Task<ActionResult> GetAllEmployee()
        {
            var model = await _hrCoreBusiness.GetAllEmployee();
            return Json(model);
        }

        public async Task<ActionResult> GetGridData(string Id=null,string Empuserid=null)
        {
            var userid = "";

            if (!string.IsNullOrEmpty(Id))
            {
                userid = Empuserid;
            }
            else { userid = _userContext.UserId; }

            var model = await _hrCoreBusiness.GetBusinessTripbyOwneruserId(userid);

            var j = Json(model);
            return j;
        }        

    }
}
