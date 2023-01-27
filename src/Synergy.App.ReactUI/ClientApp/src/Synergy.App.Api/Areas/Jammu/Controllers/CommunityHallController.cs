using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Synergy.App.Api.Areas.EGov.Models;
using Synergy.App.Api.Controllers;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.Jammu.Controllers
{
    [Route("jammu/communityhall")]
    [ApiController]
    public class CommunityHallController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly ISmartCityBusiness _smartCityBusiness;
        private readonly ILOVBusiness _lOVBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        public CommunityHallController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IServiceProvider serviceProvider,
            ISmartCityBusiness smartCityBusiness, ILOVBusiness lOVBusiness, IServiceBusiness serviceBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _smartCityBusiness = smartCityBusiness;
            _lOVBusiness=   lOVBusiness;
            _serviceBusiness = serviceBusiness;

        }
        [HttpGet]
        [Route("ReadCommunityHallList")]
        public async Task<IActionResult> ReadCommunityHallList(string type, DateTime? start = null, DateTime? end = null, string dateList = null)
        {
            string[] dates=null;
            if (dateList != null)
            {
                 dates = dateList.Split(',');

            }
            var data = await _smartCityBusiness.GetCommunityHallList(type, start, end, dates);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetJSCCommunityHallBooking")]
        public async Task<IActionResult> GetJSCCommunityHallBooking(string userId,string portalName,string hallId, string bookingById, string dtseltype, DateTime? fdate, DateTime? tdate, string multidate, string rate, string paymentMode)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            JSCCommunityHallBookingViewModel model = new JSCCommunityHallBookingViewModel();
            //model.Id = hallId;
            model.BookingById = bookingById;
            model.DateSelectionType = dtseltype;
            model.BookingFromDate = fdate;
            model.BookingToDate = tdate;
            model.MultipleDates = multidate;
            model.GrandTotal = Convert.ToDouble(rate);
            model.Name = _userContext.Name;
            model.Email = _userContext.Email;
            model.PaymentMode = paymentMode;
            //return View(model);

            var halldetails = await _smartCityBusiness.GetJSCCommunityHallDetailsById(hallId);
            var hallrate = halldetails.StandardRate;
            var commhalllist = new List<CommunityHallBooking>();
            var dateselecttype = await _lOVBusiness.GetSingleById(model.DateSelectionType);
            if (dateselecttype.Code == "JSC_DATE_RANGE")
            {
                var nodays = (model.BookingToDate.Value.Date - model.BookingFromDate.Value.Date).Days + 1;
                var tamt = nodays * hallrate;
                commhalllist.Add(new CommunityHallBooking
                {
                    CommunityHallId = hallId,
                    CommunityBookingFromDate = model.BookingFromDate.Value,
                    CommunityBookingToDate = model.BookingToDate.Value,
                    NoOfDays = nodays,
                    Rate = hallrate,
                    TotalAmount = tamt
                });
                model.GrandTotal = tamt;
            }


            if (dateselecttype.Code == "JSC_MULTIPLE_DATES")
            {
                var grandtotal = 0.0;
                var tamt = 0.0;
                var multipledate = model.MultipleDates.Split(",");
                foreach (var dt in multipledate)
                {
                    var nodays = (Convert.ToDateTime(dt) - Convert.ToDateTime(dt)).Days + 1;
                    tamt = nodays * hallrate;
                    commhalllist.Add(new CommunityHallBooking
                    {
                        CommunityHallId = hallId,
                        CommunityBookingFromDate = Convert.ToDateTime(dt),
                        CommunityBookingToDate = Convert.ToDateTime(dt),
                        NoOfDays = nodays,
                        Rate = hallrate,
                        TotalAmount = tamt
                    });
                    grandtotal = grandtotal + tamt;
                }
                model.GrandTotal = grandtotal;
            }


            model.CommunityHallBookingList = commhalllist;
            model.JSC_CommunityHallBooking = Newtonsoft.Json.JsonConvert.SerializeObject(model.CommunityHallBookingList);
            var serviceTempModel = new ServiceTemplateViewModel();
            serviceTempModel.DataAction = DataActionEnum.Create;
            serviceTempModel.ActiveUserId = _userContext.UserId;
            serviceTempModel.TemplateCode = "JSC_COMMUNITY_HALL_SERVICE";
            var serviceModel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
            serviceModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            serviceModel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
            serviceModel.DataAction = DataActionEnum.Create;
            //serviceModel.ServiceId = model.ServiceId;
            var result = await _serviceBusiness.ManageService(serviceModel);
            if (result.IsSuccess)
            {
                model.ServiceId = result.Item.ServiceId;
                model.ServiceNo = result.Item.ServiceNo;
                model.UdfNoteTableId = result.Item.UdfNoteTableId;
                return Ok(model);
            }
            return Ok(model);
        }

        [HttpPost]
        [Route("ManageJSCCommunityHallBooking")]
        public async Task<IActionResult> ManageJSCCommunityHallBooking(JSCCommunityHallBookingViewModel model)
        {
            await Authenticate(model.OwnerUserId, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();


            if (ModelState.IsValid)
            {
                var serviceTempModel = new ServiceTemplateViewModel();
                serviceTempModel.DataAction = DataActionEnum.Edit;
                serviceTempModel.SetUdfValue = true;
                serviceTempModel.ActiveUserId = _userContext.UserId;
                //serviceTempModel.TemplateCode = "JSC_COMMUNITY_HALL_SERVICE";
                serviceTempModel.ServiceId = model.ServiceId;
                var serviceModel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
                var rowData = serviceModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                rowData["Name"] = model.Name;
                rowData["Email"] = model.Email;
                rowData["Mobile"] = model.Mobile;
                rowData["PANNo"] = model.PANNo;
                rowData["Aadhar"] = model.Aadhar;
                rowData["IsJmcEmployeeId"] = model.IsJmcEmployeeId;
                serviceModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                serviceModel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                serviceModel.DataAction = DataActionEnum.Edit;

                var result = await _serviceBusiness.ManageService(serviceModel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true, data = result.Item });
                }
            }
            return Ok(new { success = false, error = ModelState.SerializeErrors() });
        }

        [HttpGet]
        [Route("GetJSCCommunityHallIdNameList")]
        public async Task<IActionResult> GetJSCCommunityHallIdNameList(string wardId)
        {
            var data = await _smartCityBusiness.GetJSCCommunityHallIdNameList(wardId);
            return Ok(data);
        }

        [HttpGet]
        [Route("SearchJSCCommunityHallList")]
        public async Task<IActionResult> SearchJSCCommunityHallList(string communityHallId, string wardId)
        {
            var data = await _smartCityBusiness.SearchJSCCommunityHallList(communityHallId, wardId);
            return Ok(data);
        }

    }
}
