using Synergy.App.ViewModel;
using Synergy.App.Business;
using Synergy.App.Business.Interface.DMS;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Api.Areas.DMS.Models;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using Syncfusion.EJ2.FileManager.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.CHR.Controllers
{
    [Route("chr/leave")]
    [ApiController]
    public class LeaveController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDMSDocumentBusiness _documentBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IHRCoreBusiness _hRCoreBusiness;


        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        public LeaveController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
         IServiceProvider serviceProvider, IDMSDocumentBusiness documentBusiness, INoteBusiness noteBusiness,
         IHRCoreBusiness hRCoreBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _documentBusiness = documentBusiness;
            _noteBusiness = noteBusiness;
            _hRCoreBusiness = hRCoreBusiness;
        }


        [HttpGet]
        [Route("ReadLeaveDetailData")]
        public async Task<ActionResult> ReadLeaveDetailData(string userId)
        {
            //userId = userId.IsNullOrEmpty() ? _userContext.UserId : userId;
            var model = await _hRCoreBusiness.GetLeaveDetail(userId);
            return Ok(model.OrderByDescending(x => x.CreatedDate));
            
        }

        [HttpGet]
        [Route("GetBusinessTripData")]
        public async Task<ActionResult> GetBusinessTripData(string portalName, string userId)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
           
            var model = await _hRCoreBusiness.GetBusinessTripbyOwneruserId(userId);

            return Ok(model);
        }

        [HttpGet]
        [Route("GetTimePermissionData")]
        public async Task<ActionResult> GetTimePermissionData(string portalName, string userId)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var model = await _hRCoreBusiness.GetTimePermissionDetailsList(userId);

            return Ok(model);
        }

        [HttpGet]
        [Route("GetTravelReimbursementData")]
        public async Task<ActionResult> GetTravelReimbursementData(string portalName, string userId)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var model = await _hRCoreBusiness.GetTravelReimbursementbyOwneruserId(userId);

            return Ok(model);
        }

        [HttpGet]
        [Route("GetMedicalReimbursementData")]
        public async Task<ActionResult> GetMedicalReimbursementData(string portalName, string userId)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var model = await _hRCoreBusiness.GetMedicalReimbursementbyOwneruserId(userId);

            return Ok(model);
        }

        [HttpGet]
        [Route("GetEducationalReimbursementData")]
        public async Task<ActionResult> GetEducationalReimbursementData(string portalName, string userId)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var model = await _hRCoreBusiness.GetEducationalReimbursementbyOwneruserId(userId);

            return Ok(model);
        }

        [HttpGet]
        [Route("GetOtherReimbursementData")]
        public async Task<ActionResult> GetOtherReimbursementData(string portalName, string userId)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var model = await _hRCoreBusiness.GetOtherReimbursementbyOwneruserId(userId);

            return Ok(model);
        }

        [HttpGet]
        [Route("GetPolicyDocuments")]
        public async Task<ActionResult> GetPolicyDocuments(string portalName, string userId)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var model = await _hRCoreBusiness.GetPolicyDocs(userId);

            return Ok(model);
        }

    }
}
