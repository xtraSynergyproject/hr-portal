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
    [Route("jammu/GarbageCollection")]
    [ApiController]
    public class GarbageCollectionController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly ISmartCityBusiness _smartCityBusiness;
        public GarbageCollectionController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IServiceProvider serviceProvider,
            ISmartCityBusiness smartCityBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _smartCityBusiness= smartCityBusiness;
        }
        [HttpGet]
        [Route("GetPropertyList")]
        public async Task<IActionResult> GetPropertyList(string userId,string wardId, string assetTypeId = null)
        {

            var _sctBusiness = _serviceProvider.GetService<ISmartCityBusiness>();
            var data = await _sctBusiness.GetJSCAssetParcelListByUser(userId);
            //var data = await _eGovernanceBusiness.GetJSCAssetsDataByConsumer(userId);
            if (!wardId.IsNullOrEmpty())
            {
                data = data.Where(x => x.ward_no == wardId).ToList();
            }
            //if (!assetTypeId.IsNullOrEmpty())
            //{
            //    data = data.Where(x => x.AssetTypeId == assetTypeId).ToList();
            //}
            return Ok(data);
            //List<PropertyViewModel> list = new List<PropertyViewModel>();
            //list.Add(new PropertyViewModel { Id = "612", Name = "DB Mall", Latitude= "23.23245587007467",Longitude="77.42997491826333",IsGarbageCollected=true,Polygon="" });
            //list.Add(new PropertyViewModel { Id = "469", Name = "Bhopal Haat", Latitude = "23.23387553784962", Longitude = "77.42874110219545", IsGarbageCollected = false, Polygon = "" });
            //list.Add(new PropertyViewModel { Id = "465", Name = "ASG EYE Hospital", Latitude = "23.234393121302855", Longitude = "77.43404114686966", IsGarbageCollected = false, Polygon = "" });
            //list.Add(new PropertyViewModel { Id = "467", Name = "City Hospital", Latitude = "23.230804501408052", Longitude = "77.43601525257829", IsGarbageCollected = false, Polygon = "" });
            //list.Add(new PropertyViewModel { Id = "5", Name = "Vishal Mega Mart", Latitude = "23.23719296651691", Longitude = "77.43294680566162", IsGarbageCollected = false, Polygon = "" });
            //list.Add(new PropertyViewModel { Id = "6", Name = "Sagar Gaire", Latitude = "23.22365655070939", Longitude = "77.43369782413772", IsGarbageCollected = false, Polygon = "" });
            //var property = (from d in list
            //                  where d.Id == localityId
            //                  select d).FirstOrDefault();
            //return Ok(property);
        }

        [HttpGet]
        [Route("GetParcelByPropertyId")]
        public async Task<IActionResult> GetParcelByPropertyId(string mmid, string userId=null, string latitude=null, string longitude=null)
        {
            var res = await _smartCityBusiness.GetParcelByPropertyId(mmid, userId, latitude, longitude);
            return Ok(res);
        }

        [HttpGet]
        [Route("SubmitGarbageCollected")]
        public async Task<IActionResult> ManageGarbageCollection(string propertyid,string userId,string latitude,string longitude)
        {
            var data = await _smartCityBusiness.ManageSingleGarbageCollection(propertyid,userId, latitude, longitude);
            if (data)
            {
                return Ok(data);
            }
            else
            {
                return Ok(new {success =false,error = "Garbage collected already"});
            }            
        }

        [HttpGet]
        [Route("ReadWardData")]
        public async Task<IActionResult> ReadWardData()
        {
            var data = await _smartCityBusiness.GetWardList();
            return Ok(data);
            //List<Ward> wardList = new List<Ward>();
            //wardList.Add(new Ward{Id="1", WardName="Ward 101"});
            //wardList.Add(new Ward{Id="2", WardName="Ward 102"});
            //wardList.Add(new Ward{Id="3", WardName="Ward 103"});
            //return Ok(wardList);
        }

        [HttpGet]
        [Route("GetLocalityByWard")]
        public async Task<IActionResult> GetLocalityByWard(string wardId)
        {
            List<Locality> localityList = new List<Locality>();
            localityList.Add(new Locality { Id = "2", LocalityName = "Property 102", WardId = "1" });
            localityList.Add(new Locality { Id = "3", LocalityName = "Property 103", WardId = "1" });
            localityList.Add(new Locality { Id = "4", LocalityName = "Property 201", WardId = "2" });
            localityList.Add(new Locality { Id = "1", LocalityName = "Property 101", WardId = "1" });
            localityList.Add(new Locality { Id = "5", LocalityName = "Property 202", WardId = "2" });
            localityList.Add(new Locality { Id = "6", LocalityName = "Property 301", WardId = "3" });
            localityList.Add(new Locality { Id = "7", LocalityName = "Property 302", WardId = "3" });

            return Ok(localityList);
        }

        [HttpGet]
        [Route("GetSubLocality")]
        public async Task<IActionResult> GetSubLocality(string wardId,string localityId)
        {
            List<SubLocality> subPropertyList = new List<SubLocality>();

            subPropertyList.Add(new SubLocality { Id = "1", SubLocalityName = "SubLocality 10101", localityId = "1" });
            subPropertyList.Add(new SubLocality { Id = "2", SubLocalityName = "SubLocality 10102", localityId = "1" });
            subPropertyList.Add(new SubLocality { Id = "3", SubLocalityName = "SubLocality 20101", localityId = "2" });
            subPropertyList.Add(new SubLocality { Id = "4", SubLocalityName = "SubLocality 20102", localityId = "2" });
            subPropertyList.Add(new SubLocality { Id = "5", SubLocalityName = "SubLocality 30101", localityId = "3" });
            subPropertyList.Add(new SubLocality { Id = "6", SubLocalityName = "SubLocality 30102", localityId = "3" });
            subPropertyList.Add(new SubLocality { Id = "7", SubLocalityName = "SubLocality 40201", localityId = "4" });
            subPropertyList.Add(new SubLocality { Id = "8", SubLocalityName = "SubLocality 40202", localityId = "4" });
            subPropertyList.Add(new SubLocality { Id = "9", SubLocalityName = "SubLocality 50201", localityId = "5" });
            subPropertyList.Add(new SubLocality { Id = "10", SubLocalityName = "SubLocality 50202", localityId = "5" });
            subPropertyList.Add(new SubLocality { Id = "11", SubLocalityName = "SubLocality 60301", localityId = "6" });
            subPropertyList.Add(new SubLocality { Id = "12", SubLocalityName = "SubLocality 60302", localityId = "6" });
            subPropertyList.Add(new SubLocality { Id = "13", SubLocalityName = "SubLocality 70301", localityId = "7" });
            subPropertyList.Add(new SubLocality { Id = "14", SubLocalityName = "SubLocality 70302", localityId = "7" });
            return Ok(subPropertyList);
        }

        [HttpGet]
        [Route("GetCollectorDetailsByUserId")]
        public async Task<IActionResult> GetCollectorDetailsByUserId(string userId)
        {
            var res = await _smartCityBusiness.GetCollectorDetailsByUserId(userId);
            return Ok(res);
        }


        [HttpPost]
        [Route("AddPropertyToCollector")]
        public async Task<IActionResult> AddPropertyToCollector(JSCCollectorPropertyViewModel model)
        {
            var parcel = await _smartCityBusiness.GetParcelByDDNNO(model.ParcelId);
           // var parcel = await _smartCityBusiness.IsParcelIdValid(model.ParcelId);
            if (!parcel.IsNotNull())
            {
                return Ok(new { success = false, error = "Invalid DDN No" });
            }
            model.WardNo = parcel.ward_no;

            var collector = await _smartCityBusiness.GetCollectorWithWardByCollectorId(model.CollectorId);

            var exist = collector.Where(x => x.WardNo == model.WardNo).FirstOrDefault();
            if (!exist.IsNotNull())
            {
                return Ok(new { success = false, error = "Collector not mapped with ward no of the given DDN " });
            }

            var result = await _smartCityBusiness.ManageGarbageCollectorProperty(model);
            if (result)
            {
                return Ok(new { success = result });
            }
            else
            {
                return Ok(new { success = result, error="Property already mapped" });
            }            
        }

        [HttpGet]
        [Route("GetGarbageCollectionDetailsByUser")]
        public async Task<IActionResult> GetGarbageCollectionDetailsByUser(string userId,DateTime? date=null, string mobileNo = null, string userName = null, string ddnNo = null,bool? isGarbageCollected=null)
        {
            var res = await _smartCityBusiness.GetGarbageCollectionDetailsByUserId(userId, date, mobileNo, userName, ddnNo);
            if (isGarbageCollected.IsNotNull())
            {
                res = res.Where(x => x.IsGarbageCollected == isGarbageCollected).ToList();
            }
            return Ok(res);
        }

        [HttpGet]
        [Route("GetGarbageCollectionDetailsByCitizen")]
        public async Task<IActionResult> GetGarbageCollectionDetailsByCitizen(string userId, DateTime? date = null, string ddnNo = null)
        {
            var res = await _smartCityBusiness.GetGarbageCollectionDetailsByCitizen(userId, date, ddnNo);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetParcelIdNameList")]
        public async Task<IActionResult> GetParcelIdNameList()
        {
            try
            {
                var _smartCityBusiness = _serviceProvider.GetService<ISmartCityBusiness>();
                var list = await _smartCityBusiness.GetParcelIdNameList();
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GarbageCollectionSummary")]
        public async Task<IActionResult> GarbageCollectionSummary(string userId)
        {
            var res = await _smartCityBusiness.GetGarbageCollectionDetailsByUserId(userId, DateTime.Now);
            var model = new JSCGarbageCollectionViewModel()
            {
                TotalProperty = res.Count,
                TotalCollectedProperty = res.Where(x => x.IsGarbageCollected == true).Count()
            };
            var bwg = await _smartCityBusiness.GetBWGDetailsByUserId(userId);
            model.TotalCollectedProperty = model.TotalCollectedProperty + bwg.Count();
            return Ok(model);
        }

        
        [HttpGet]
        [Route("CreateLodgeComplaint")]
        public async Task<IActionResult> CreateLodgeComplaint(string ddnNo,string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var result = await _smartCityBusiness.CreateLodgeComplaintService(ddnNo, userId);
            if (result.IsSuccess)
            {
                return Ok(new { success = result.IsSuccess, error = result.Message });
            }
            else
            {
                return Ok(new { success = result.IsSuccess, error = result.Message });
            }
        }

        [HttpGet]
        [Route("GetAutoListByUserId")]
        public async Task<IActionResult> GetAutoListByUserId(string userId)
        {
            var list = await _smartCityBusiness.GetAutoListByUserId(userId);
            return Ok(list);
        }

        [HttpGet]
        [Route("GetMSWAutoDetails")]
        public async Task<IActionResult> GetMSWAutoDetails(string id)
        {
            var data = await _smartCityBusiness.GetMSWAutoDetails(id);
            return Ok(data);
        }

        [HttpPost]
        [Route("CreateGVPData")]
        public async Task<IActionResult> CreateGVPData(JSCDailyBasedActivityViewModel model)
        {
            var result = await _smartCityBusiness.ManageDailyBasedActivity(model);
            return Ok(new { success = result.IsSuccess, error = result.Message });
        }

        [HttpGet]
        [Route("GetJSCGVPData")]
        public async Task<IActionResult> GetJSCGVPData(DateTime? date)
        {
            var data = await _smartCityBusiness.GetJSCGVPData(date);
            return Ok(data);
        }
        [HttpGet]
        [Route("GetJSCGVPDataById")]
        public async Task<IActionResult> GetJSCGVPDataById(string Id=null)
        {
            var list = await _smartCityBusiness.GetJSCGVPData(null);
            var data = list.Where(x => x.Id == Id).FirstOrDefault();
            return Ok(data);
        }


        //[HttpGet]
        //[Route("GetJSCTransferStationVehicle")]
        //public async Task<IActionResult> GetJSCTransferStationVehicle(string userId)
        //{
        //    //get transfer station

        //    // filter the vehicle by transfer station 
        //    var list = await _smartCityBusiness.GetJSCTransferStationVehicle(userId);
        //    //var data = list.Where(x => x.Id == Id).FirstOrDefault();
        //    return Ok(data);
        //}

        [HttpPost]
        [Route("CreateRefuseCompactor")]
        public async Task<IActionResult> CreateRefuseCompactor(JSCRefuseCompactorViewModel model)
        {
            var result = await _smartCityBusiness.ManageRefuseCompactor(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetRefuseCompactorData")]
        public async Task<IActionResult> GetRefuseCompactorData(DateTime? date=null)
        {
            var data = await _smartCityBusiness.GetRefuseCompactorData(date);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetPointList")]
        public async Task<IActionResult> GetPointList(string vehicleId)
        {
            var data = await _smartCityBusiness.GetPointList(vehicleId);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetDustbinData")]
        public async Task<IActionResult> GetDustbinData()
        {
            var data = await _smartCityBusiness.GetDustbinData();
            return Ok(data);
        }

        [HttpPost]
        [Route("MapPointAndVehicle")]
        public async Task<IActionResult> MapPointAndVehicle(JSCDailyBasedActivityViewModel model)
        {
            var result = await _smartCityBusiness.MapPointAndVehicle(model);
            return Ok(new { success = result.IsSuccess, error = result.Messages });
        }

        [HttpGet]
        [Route("GetPointAndVehicleMappingData")]
        public async Task<IActionResult> GetPointAndVehicleMappingData()
        {
            var data = await _smartCityBusiness.GetPointAndVehicleMappingData();
            return Ok(data);
        }

        [HttpGet]
        [Route("GetVehicleIdForLoggedInUser")]
        public async Task<string> GetVehicleIdForLoggedInUser(string userId)
        {
            var data = await _smartCityBusiness.GetVehicleIdForLoggedInUser(userId);
            return data;
        }
        
        [HttpGet]
        [Route("GetVehicleTypeForLoggedInUser")]
        public async Task<string> GetVehicleTypeForLoggedInUser(string userId)
        {
            var data = await _smartCityBusiness.GetVehicleTypeForLoggedInUser(userId);
            return data;
        }

        [HttpGet]
        [Route("GetJSCVehicleDetails")]
        public async Task<IActionResult> GetJSCVehicleDetails(string vehicleId, DateTime? startDate, DateTime? endDate)
        {
            var data = await _smartCityBusiness.GetJSCVehicleDetails(vehicleId, startDate, endDate);
            return Ok(data);
        }
    }
}
