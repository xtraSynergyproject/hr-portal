using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.CMS.Controllers
{
    [Route("cms/NtsNote")]
    [ApiController]
    public class NtsNoteController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;


        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public NtsNoteController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
          IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        [Route("NtsNotePage")]
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

        [HttpGet]
        [Route("ReadNoteHomeData")]
        public async Task<IActionResult> ReadNoteHomeData(string userId, string moduleId, string noteStatus, string mode, string text)
        {
            await Authenticate(userId);
            var _business = _serviceProvider.GetService<INoteBusiness>();
            var _context = _serviceProvider.GetService<IUserContext>();
            if (userId.IsNullOrEmpty())
            {
                userId = _context.UserId;
            }
            //var result = new List<NoteViewModel>();//await _business.GetSearchResult(search).OrderByDescending(x => x.LastUpdatedDate);
            //var result = await _business.GetSearchResult(search);
            var result = await _business.GetSearchResult(new NoteSearchViewModel
            {
                ModuleId = moduleId,
                Mode = mode,
                UserId = userId,
                NoteStatus = noteStatus

            });
            result = result.Where(x => x.PortalId == _context.PortalId).ToList();

            if (text == "Today")
            {
                var res = result.Where(x => x.ExpiryDate <= DateTime.Now && x.NoteStatusCode != "COMPLETED" && x.NoteStatusCode != "CANCELED" && x.NoteStatusCode != "DRAFT");
                return Ok(res);
            }
            else if (text == "Week")
            {
                var res = result.Where(x => (x.ExpiryDate <= DateTime.Now.AddDays(7) && x.NoteStatusCode != "COMPLETED" && x.NoteStatusCode != "CANCELED" && x.NoteStatusCode != "DRAFT")).ToList();
                return Ok(res);
            }
            var data = result.OrderByDescending(x => x.LastUpdatedDate);
            if (data.Count() > 1000)
            {
                return Ok(data.Take(1000));
            }
            else
            {
                return Ok(data);
            }

        }

        [HttpGet]
        [Route("NoteBookHTML")]
        public async Task<IActionResult> NoteBookHTML(string noteId, string templateId)
        {
            var _business = _serviceProvider.GetService<INoteBusiness>();
            var model = await _business.GetBookDetails(noteId);
            return Ok(model);
        }


        [HttpPost]
        [Route("ManageMoveToParent")]
        public async Task<IActionResult> ManageMoveToParent(NoteViewModel model)
        {
            await Authenticate(model.OwnerUserId,model.PortalId);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _business = _serviceProvider.GetService<INoteBusiness>();

            var result = await _business.ManageMoveToParent(model);
            if (result.IsSuccess)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, error = result.Message });
        }

        [HttpGet]
        [Route("DeleteNote")]
        public async Task<IActionResult> DeleteNote(string id)
        {
            var _business = _serviceProvider.GetService<INoteBusiness>();
            await _business.Delete(id);
            return Ok(new { success = true });
        }


    }
}

