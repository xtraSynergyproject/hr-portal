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
        public async Task<IActionResult> GetParcelByPropertyId(string mmid)
        {
            var res = await _smartCityBusiness.GetParcelByPropertyId(mmid);
            return Ok(res);
        }

        [HttpGet]
        [Route("SubmitGarbageCollected")]
        [HttpPost]
        public async Task<IActionResult> ManageGarbageCollection(string propertyid,string userId,string latitude,string longitude)
        {
            var data = await _smartCityBusiness.ManageSingleGarbageCollection(propertyid,userId, latitude, longitude);
            return Ok(new { success = data });
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

        
        [Route("AddPropertyToCollector")]
        [HttpPost]
        public async Task<IActionResult> AddPropertyToCollector(JSCCollectorPropertyViewModel model)
        {
            var result = await _smartCityBusiness.ManageGarbageCollectorProperty(model);
            if (result)
            {
                return Ok(new { success = result });
            }
            else
            {
                return Ok(new { success = result, error="Property already added to this collector" });
            }            
        }

    }
}
