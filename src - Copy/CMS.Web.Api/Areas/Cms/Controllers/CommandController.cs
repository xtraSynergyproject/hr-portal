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
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Web.Api.Areas.CMS.Controllers
{
    [Route("cms/command")]
    [ApiController]
    public class CommandController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;


        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public CommandController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
          IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        [HttpPost]
        [Route("ShareNote")]
        public async Task<ActionResult> ShareNote(NtsNoteSharedViewModel model)
        {
            var _ntsNoteSharedBusiness = _serviceProvider.GetService<INtsNoteSharedBusiness>();
            var _lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
            //NtsNoteSharedViewModel model = new NtsNoteSharedViewModel();
            //model.NoteSharedWithTypeId = noteSharedWithTypeId;
            if (model.NoteSharedWithTypeId == "1")
            {
                var lov = await _lovBusiness.GetSingle(x => x.Code == "SHARED_TYPE_USER" && x.LOVType == "SHARED_TYPE");
                model.NoteSharedWithTypeId = lov.Id;
            }
            else
            {
                var lov = await _lovBusiness.GetSingle(x => x.Code == "SHARED_TYPE_TEAM" && x.LOVType == "SHARED_TYPE");
                model.NoteSharedWithTypeId = lov.Id;
            }
            model.SharedDate = DateTime.Now;
            model.SharedByUserId = model.UserId;
            var result = await _ntsNoteSharedBusiness.Create(model);
            return Ok(new { success = result.IsSuccess, TaskId = result.Item.NtsNoteId });
        }

        [HttpPost]
        [Route("ShareService")]
        public async Task<ActionResult> ShareService(NtsServiceSharedViewModel model)
        {
            await Authenticate(model.UserId);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _ntsServiceSharedBusiness = _serviceProvider.GetService<INtsServiceSharedBusiness>();
            var _lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
            //NtsServiceSharedViewModel model = new NtsServiceSharedViewModel();
            //model.ServiceSharedWithTypeId = serviceSharedWithTypeId;
            if (model.ServiceSharedWithTypeId == "1")
            {
                var lov = await _lovBusiness.GetSingle(x => x.Code == "SHARED_TYPE_USER" && x.LOVType == "SHARED_TYPE");
                model.ServiceSharedWithTypeId = lov.Id;
            }
            else
            {
                var lov = await _lovBusiness.GetSingle(x => x.Code == "SHARED_TYPE_TEAM" && x.LOVType == "SHARED_TYPE");
                model.ServiceSharedWithTypeId = lov.Id;
            }
            model.SharedDate = DateTime.Now;
            model.SharedByUserId = model.UserId;
            var result = await _ntsServiceSharedBusiness.Create(model);
            return Ok(new { success = result.IsSuccess, TaskId = result.Item.NtsServiceId });
        }

        [HttpPost]
        [Route("ShareTask")]
        public async Task<ActionResult> ShareTask(NtsTaskSharedViewModel model)
        {
            var _ntsTaskSharedBusiness = _serviceProvider.GetService<INtsTaskSharedBusiness>();
            var _lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
            //NtsTaskSharedViewModel model = new NtsTaskSharedViewModel();
            //model.TaskSharedWithTypeId = taskSharedWithTypeId;
            if (model.TaskSharedWithTypeId == "1")
            {
                var lov = await _lovBusiness.GetSingle(x => x.Code == "SHARED_TYPE_USER" && x.LOVType == "SHARED_TYPE");
                model.TaskSharedWithTypeId = lov.Id;
            }
            else
            {
                var lov = await _lovBusiness.GetSingle(x => x.Code == "SHARED_TYPE_TEAM" && x.LOVType == "SHARED_TYPE");
                model.TaskSharedWithTypeId = lov.Id;
            }
            model.SharedDate = DateTime.Now;
            model.SharedByUserId = model.UserId;
            var result = await _ntsTaskSharedBusiness.Create(model);
            return Ok(new { success = result.IsSuccess, TaskId = result.Item.NtsTaskId });
        }

        [HttpPost]
        [Route("SaveServiceAttachment")]
        public async Task<ActionResult> SaveServiceAttachment(IList<IFormFile> files, string referenceTypeId, ReferenceTypeEnum referenceTypeCode)
        {
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            try
            {
                foreach (var file in files)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        ReferenceTypeId = referenceTypeId,
                        ReferenceTypeCode = referenceTypeCode,
                        FileExtension = Path.GetExtension(file.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Ok(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }



                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }


        [HttpPost]
        [Route("SaveTaskAttachment")]
        public async Task<ActionResult> SaveTaskAttachment(IList<IFormFile> files, string referenceTypeId, ReferenceTypeEnum referenceTypeCode)
        {
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            try
            {
                foreach (var file in files)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        ReferenceTypeId = referenceTypeId,
                        ReferenceTypeCode = referenceTypeCode,
                        FileExtension = Path.GetExtension(file.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Ok(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }



                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser(UserViewModel model)
        {
            var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
            var _userPortalBusiness = _serviceProvider.GetService<IUserPortalBusiness>();
            var result = await _userBusiness.Create(model);
            if (result.IsSuccess)
            {
                if (model.Portal.IsNotNull())
                {
                    foreach (var p in model.Portal)
                    {
                        var res = await _userPortalBusiness.Create(new UserPortalViewModel
                        {
                            UserId = result.Item.Id,
                            PortalId = p,
                        });
                    }
                }
                return Ok(new { success = result.IsSuccess, UserId = result.Item.Id });
            }
            else
            {
                return Ok(new { success = result.IsSuccess, error = result.ErrorText });
            }
        }
        [HttpPost]
        [Route("SendForgotPasswordOTP")]
        public async Task<IActionResult> SendForgotPasswordOTP(string email)
        {
            var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
            var _emailBusiness = _serviceProvider.GetService<IEmailBusiness>();
            var user = await _userBusiness.ConfirmEmailOTP(email);
            if (user != null)
            {
                try
                {
                    EmailViewModel emailModel = new EmailViewModel();
                    emailModel.To = user.Email;
                    emailModel.Subject = "Synergy User Temporary Forgot Password OTP";
                    emailModel.Body = "Your Temporary Forgot Password OTP is : " + user.ForgotPasswordOTP;
                    var resultemail = await _emailBusiness.SendMail(emailModel);
                    if (resultemail.IsSuccess)
                    {
                        return Ok(new { success = resultemail.IsSuccess, msg = "The temporary forgot password OTP has been sent to you" });

                    }
                    else
                    {
                        return Ok(new { success = resultemail.IsSuccess, error = resultemail.ErrorText });
                    }
                }
                catch (Exception)
                {
                    return Ok(new { success = false, error = "An error occured while processing your request. Please try again later or contact system administrator" });

                }
            }
            else
            {

                return Ok(new { success = false, error = "The given email address is invalid. Please enter valid email." });
            }
        }

        [HttpPost]
        [Route("ValidateForgotPasswordOTP")]
        public async Task<IActionResult> ValidateForgotPasswordOTP(string email, string otp)
        {
            var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
            var _emailBusiness = _serviceProvider.GetService<IEmailBusiness>();
            var user = await _userBusiness.GetSingleGlobal(x => x.Email == email);
            if (user != null)
            {

                if (user.ForgotPasswordOTP == otp)
                {
                    return Ok(new { success = true, UserId = user.Id });
                }
                else
                {
                    return Ok(new { success = false, error = "Invalid OTP. Please enter correct OTP." });

                }

            }
            else
            {
                return Ok(new { success = false, error = "Invalid email." });
            }
        }

        [HttpPost]
        [Route("ForgotPasswordUpdate")]
        public async Task<ActionResult> ForgotPasswordUpdate(ForgotViewModel model)
        {

            if (model != null)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    return Ok(new { success = false, error = "Password and Confirm password not matching" });
                }
                else
                {
                    var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
                    var _emailBusiness = _serviceProvider.GetService<IEmailBusiness>();
                    var user = await _userBusiness.GetSingleGlobal(x => x.Email == model.Email);
                    if (user != null)
                    {
                        var result = await _userBusiness.ConfirmPasswordChange(user.Email, model.Password);

                        if (result != null && result.ForgotPasswordOTP == null)
                        {
                            return Ok(new { success = true });

                        }
                        else
                        {
                            return Ok(new { success = false, error = "Please enter correct details" });
                        }
                    }
                    else
                    {
                        return Ok(new { success = false, error = "Invalid user" });
                    }
                }
            }

            return Ok(new { success = false, error = "Invalid data" });


        }
    }
}
