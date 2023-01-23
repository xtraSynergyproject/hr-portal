using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CMS.UI.Web.Areas.Clk.Controllers
{

    [Area("Clk")]
    public class RegisterUserInDeviceController : Controller
    {

        private readonly IUserInfoBusiness _business;
        IUserContext _userContext;
        public RegisterUserInDeviceController(IUserInfoBusiness business, IUserContext userContext)
        {
            _business = business;
            _userContext = userContext;
        }
        
        public ActionResult RegisterUserInDevice()
        {
            var model = new UserInfoViewModel();
            return View(model);
        }


        public ActionResult Index()
        {
            var model = new UserInfoViewModel();
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetExcludePersonList(string deviceId, string searchParam)
        {

          
             var data = new List<UserInfoViewModel>();
            if (deviceId != null)
            {
               var data1 =await _business.GetExcludePersonList(deviceId, searchParam);
                return Json(data1);
            }
            return Json(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetIncludePersonList(string deviceId, string searchParam)
        {

          
            var data = new List<UserInfoViewModel>();
            if (deviceId != null)
            {
               var data1= await _business.GetIncludePersonList(deviceId, searchParam);
                return Json(data1);
            }
            return Json(data);
        }


        [HttpGet]
        public async Task<ActionResult> GetIdNameList()
        {
            var data1 = await _business.GetAllDevice();
            return Json(data1);
        }
        [HttpPost]
        public async Task<IActionResult> IncludePerson(string deviceId, string persons)
        {
            var userid = _userContext.UserId;
            await  _business.IncludePerson(deviceId, persons,userid);
            return Json(new { success = true });
        }
        [HttpPost]
        public ActionResult ExcludePerson(string deviceId, string persons)
        {

            var userid = _userContext.UserId;
            _business.ExcludePerson(deviceId, persons);
            return Json(new { success = true });
        }
    }
}
