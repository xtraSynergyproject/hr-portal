using Synergy.App.Business;
using Synergy.App.Common;
using CMS.UI.Utility;
using Synergy.App.ViewModel;
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

        public ActionResult TravelReimbursement(string EmpId)
        {
            var model = new TravelReimbursementViewModel();
            if (EmpId.IsNotNullAndNotEmpty())
            {
                string[] spl = EmpId.Split("?Userid=");
                model.Id = spl[0];
                model.UserId = spl[1];
            }
            return View(model);
        }


        public async Task<ActionResult> GetTravelReimbursementData(string Id = null, string Empuserid = null)
        {
            var userid = "";

            if (!string.IsNullOrEmpty(Id))
            {
                userid = Empuserid;
            }
            else { userid = _userContext.UserId; }

            var model = await _hrCoreBusiness.GetTravelReimbursementbyOwneruserId(userid);

            var j = Json(model);
            return j;
        }



        public ActionResult MedicalReimbursement(string EmpId)
        {
            var model = new MedicalReimbursementViewModel();
            if (EmpId.IsNotNullAndNotEmpty())
            {
                string[] spl = EmpId.Split("?Userid=");
                model.Id = spl[0];
                model.UserId = spl[1];
            }
            return View(model);
        }


        public async Task<ActionResult> GetMedicalReimbursementData(string Id = null, string Empuserid = null)
        {
            var userid = "";

            if (!string.IsNullOrEmpty(Id))
            {
                userid = Empuserid;
            }
            else { userid = _userContext.UserId; }

            var model = await _hrCoreBusiness.GetMedicalReimbursementbyOwneruserId(userid);

            var j = Json(model);
            return j;
        }

        public ActionResult EducationalReimbursement(string EmpId)
        {
            var model = new EducationalReimbursementViewModel();
            if (EmpId.IsNotNullAndNotEmpty())
            {
                string[] spl = EmpId.Split("?Userid=");
                model.Id = spl[0];
                model.UserId = spl[1];
            }
            return View(model);
        }

        public async Task<ActionResult> GetEducationalReimbursementData(string Id = null, string Empuserid = null)
        {
            var userid = "";

            if (!string.IsNullOrEmpty(Id))
            {
                userid = Empuserid;
            }
            else { userid = _userContext.UserId; }

            var model = await _hrCoreBusiness.GetEducationalReimbursementbyOwneruserId(userid);

            var j = Json(model);
            return j;
        }

        public ActionResult OtherReimbursement(string EmpId)
        {
            var model = new OtherReimbursementViewModel();
            if (EmpId.IsNotNullAndNotEmpty())
            {
                string[] spl = EmpId.Split("?Userid=");
                model.Id = spl[0];
                model.UserId = spl[1];
            }
            return View(model);
        }

        public async Task<ActionResult> GetOtherReimbursementData(string Id = null, string Empuserid = null)
        {
            var userid = "";

            if (!string.IsNullOrEmpty(Id))
            {
                userid = Empuserid;
            }
            else { userid = _userContext.UserId; }

            var model = await _hrCoreBusiness.GetOtherReimbursementbyOwneruserId(userid);

            var j = Json(model);
            return j;
        }


        public ActionResult PolicyDocument(string EmpId)
        {
            var model = new HRPolicyViewModel();
            if (EmpId.IsNotNullAndNotEmpty())
            {
                string[] spl = EmpId.Split("?Userid=");
                model.Id = spl[0];
                model.UserId = spl[1];
            }
            return View(model);
        }


        public async Task<ActionResult> GetPolicyDocuments(string Id = null, string Empuserid = null)
        {
            var userid = "";

            if (!string.IsNullOrEmpty(Id))
            {
                userid = Empuserid;
            }
            else { userid = _userContext.UserId; }

            var model = await _hrCoreBusiness.GetPolicyDocs(userid);

            var j = Json(model);
            return j;
        }


    }
}
