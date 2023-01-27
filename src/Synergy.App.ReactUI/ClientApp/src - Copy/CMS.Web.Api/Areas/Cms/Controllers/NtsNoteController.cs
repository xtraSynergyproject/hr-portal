using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
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
using CMS.Web.Api.Controllers;

namespace CMS.Web.Api.Areas.CMS.Controllers
{
    [Route("cms/NtsNote")]
    [ApiController]
    public class NtsNoteController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public NtsNoteController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        [HttpGet]
        [Route("NtsNotePage")]
        //[System.Web.Http.HttpGet]
        //[System.Web.Http.Route("api/NoteCard")]
        public async Task<IActionResult> NtsNotePage(string templateid, string dataAction, string noteId = null)
        {
            try
            {
                var _userContext = _serviceProvider.GetService<IUserContext>();
                var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
                var model = new NoteTemplateViewModel();
                model.ActiveUserId = _userContext.UserId;

                if (templateid.IsNotNullAndNotEmpty())
                {
                    model.TemplateId = templateid;
                }
                if (noteId.IsNullOrEmpty())
                {
                    model.DataAction = DataActionEnum.Create;
                }
                else
                {
                    model.NoteId = noteId;
                    if (dataAction.IsNullOrEmpty())
                    {
                        model.DataAction = DataActionEnum.Edit;
                    }
                    else
                    {
                        model.DataAction = dataAction.ToEnum<DataActionEnum>();
                    }
                }                
                var newmodel = await _noteBusiness.GetNoteDetails(model);
                newmodel.DataAction = model.DataAction;
                return Ok(newmodel);
                //string SerializedResponse = JsonConvert.SerializeObject(
                //         newmodel,
                //         new Newtonsoft.Json.Converters.StringEnumConverter()
                //    );
                //return Json(new { success = true, result = SerializedResponse });
            }
            catch (Exception)
            {
                throw;
                //return Json(new { success = false, errors = e.ToString() });
            }

        }
        [HttpPost]
        [Route("ManageNtsNote")]
        public async Task<IActionResult> ManageNtsNote([FromBody] NoteTemplateViewModel model)
        {
            try
            {
                var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
                if (model.DataAction == DataActionEnum.Create)
                {
                    var create = await _noteBusiness.ManageNote(model);
                    if (create.IsSuccess)
                    {
                        return Ok(create);
                    }
                    else
                    {
                        return NotFound(create.HtmlError);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var edit = await _noteBusiness.ManageNote(model);

                    if (edit.IsSuccess)
                    {
                        return Ok(edit);
                    }
                    else
                    {
                        return NotFound(edit.HtmlError);
                    }
                }
                return NotFound("Invalid action");
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("ReadNoteDashBoardGridData")]
        public async Task<IActionResult> ReadNoteDashBoardGridData(string templateCode)
        {
            try
            {
                var _userContext = _serviceProvider.GetService<IUserContext>();
                var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();                
                var result = await _noteBusiness.GetNoteDataListByTemplateCode(templateCode);
                return Ok(result);

            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
