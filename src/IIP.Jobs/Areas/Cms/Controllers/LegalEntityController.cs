using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Api.Controllers;
using System.Data;
using System.IO;

namespace Synergy.App.Api.Areas.Cms.Controllers
{
    [Route("cms/legalEntity")]
    [ApiController]
    public class LegalEntityController : ApiController
    {

        private readonly IServiceProvider _serviceProvider;
        private ILegalEntityBusiness _legalEntityBusiness;
        private readonly IDocumentBusiness _documentBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public LegalEntityController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider, ILegalEntityBusiness legalEntityBusiness, IDocumentBusiness documentBusiness, IHRCoreBusiness hrCoreBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _legalEntityBusiness = legalEntityBusiness;
            _documentBusiness = documentBusiness;
            _hrCoreBusiness = hrCoreBusiness;
        }

        [HttpGet]
        [Route("GetLegalEntityList")]
        public async Task<ActionResult> GetLegalEntityNameList()
        {
            var _legalEntityBusiness = _serviceProvider.GetService<ILegalEntityBusiness>();
            var data = await _legalEntityBusiness.GetList();

            return Ok(data);
        }

        [HttpGet]
        [Route("CreateLegalEntity")]
        public async Task<IActionResult> Create(string Id)
        {
            var model = new LegalEntityViewModel();
            if (Id.IsNullOrEmpty())
            {

                model = new LegalEntityViewModel
                {
                    DataAction = DataActionEnum.Create,
                    Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                    //GroupPortals=
                };
            }
            else
            {
                model = await _legalEntityBusiness.GetSingleById(Id);

                if (model != null)
                {

                    model.DataAction = DataActionEnum.Edit;

                }

            }
            return Ok(model);
        }

        //[HttpGet]
        //[Route("EditLegalEntity")]
        //public async Task<IActionResult> Edit(string Id)
        //{
        //    var module = await _legalEntityBusiness.GetSingleById(Id);

        //    if (module != null)
        //    {

        //        module.DataAction = DataActionEnum.Edit;
        //        return Ok( module);
        //    }
        //    return Ok( new LegalEntityViewModel());
        //}

        [HttpPost]
        [Route("ManageLegalEntity")]
        public async Task<IActionResult> Manage(LegalEntityViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _legalEntityBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return Ok(new { success = result.IsSuccess });

                    }
                    else
                    {
                        return Ok(new { success = false, error = result.Messages }); 
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _legalEntityBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return Ok(new { success = result.IsSuccess });

                    }
                    else
                    {
                        return Ok(new { success = false, error = result.Messages });
                    }
                }
            }

            return Ok( model);
        }

        [HttpGet]
        [Route("DeleteLegalEntity")]
        public async Task<IActionResult> Delete(string id)
        {
            await _legalEntityBusiness.Delete(id);
            return Ok();
        }

    }
}
