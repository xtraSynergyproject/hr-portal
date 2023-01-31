using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using CMS.Web.Api.Controllers;
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

namespace CMS.Web.Api.Areas.Nts.Controllers
{
    [Route("nts/command")]
    [ApiController]
    public class CommandController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        public CommandController(AuthSignInManager<ApplicationIdentityUser> customUserManager
             , IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        [HttpPost]
        [Route("ManageService")]
        public async Task<IActionResult> ManageService(ServiceTemplateViewModel model)
        {
            try
            {
                await Authenticate(model.RequestedByUserId, model.PortalName);
                var _business = _serviceProvider.GetService<IServiceBusiness>();
                var result = await _business.ManageService(model);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        [Route("ManageTask")]
        public async Task<IActionResult> ManageTask(TaskTemplateViewModel model)
        {
            try
            {
                await Authenticate(model.RequestedByUserId, model.PortalName);
                var _context = _serviceProvider.GetService<IUserContext>();
                var _business = _serviceProvider.GetService<ITaskBusiness>();
                var result = await _business.ManageTask(model);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        [Route("ManageNote")]
        public async Task<IActionResult> ManageNote(NoteTemplateViewModel model)
        {
            try
            {
                await Authenticate(model.RequestedByUserId, model.PortalName);
                var _business = _serviceProvider.GetService<INoteBusiness>();
                var result = await _business.ManageNote(model);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        [Route("PostServiceComment")]
        public async Task<IActionResult> PostServiceComment(NtsServiceCommentViewModel model)
        {
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _ntsServiceCommentBusiness = _serviceProvider.GetService<INtsServiceCommentBusiness>();
            try
            {
                //model.CommentedByUserId = _userContext.UserId;
                model.CommentedDate = DateTime.Now;
                var result = await _ntsServiceCommentBusiness.Create(model);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [Route("PostTaskComment")]
        public async Task<IActionResult> PostTaskComment(NtsTaskCommentViewModel model)
        {
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _ntsTaskCommentBusiness = _serviceProvider.GetService<INtsTaskCommentBusiness>();
            try
            {
                //model.CommentedByUserId = _userContext.UserId;
                model.CommentedDate = DateTime.Now;
                var result = await _ntsTaskCommentBusiness.Create(model);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [Route("PostNoteComment")]
        public async Task<IActionResult> PostNoteComment(NtsNoteCommentViewModel model)
        {
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _ntsNoteCommentBusiness = _serviceProvider.GetService<INtsNoteCommentBusiness>();
            try
            {
                //model.CommentedByUserId = _userContext.UserId;
                model.CommentedDate = DateTime.Now;
                var result = await _ntsNoteCommentBusiness.Create(model);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }

        }


    }
}
