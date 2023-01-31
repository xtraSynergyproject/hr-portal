using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Synergy.App.DataModel;
using Synergy.App.Api.Areas.Cms.Models;
using System.Text;
using Microsoft.Extensions.Configuration;
using static Synergy.App.Common.ApplicationConstant;

namespace Synergy.App.Api.Areas.PortalAdmin.Controllers
{
    [Route("api/UserProfile")]
    [ApiController]
    public class UserProfileController : ApiController
    {
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IConfiguration _iConfiguration;
        private readonly IServiceProvider _serviceProvider;
        private IUserBusiness _userBusiness;
        public UserProfileController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IPageBusiness pageBusiness, IUserHierarchyPermissionBusiness userHierarchyPermissionBusiness, IPortalBusiness portalBusiness, ILegalEntityBusiness legalEntityBusiness, IServiceProvider serviceProvider, IUserPortalBusiness userPortalBusiness, ICandidateProfileBusiness candidateProfileBusiness,
            IUserBusiness userBusiness) : base(serviceProvider)
        {

            _userBusiness = userBusiness;
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        [Route("GetUserDetails")]
        public async Task<IActionResult> GetUserDetails(string Id)
        {
            var _business = _serviceProvider.GetService<IUserBusiness>();
            var data = await _business.GetUserIdNameList();
            var userDetail = (from d in data
                             where d.Id==Id
                             select d).ToList();
            return Ok(userDetail[0]);
        }


        [HttpPost]
        [Route("UploadAndCompareProfile")]
        public async Task<IActionResult> UploadAndCompareProfile(AttachmentSet model)
        {
            //var result = true;
            //upload functionalty to write
            
            //if uploaded then compare using return fileid

            //if compare success the true else false
            return Ok(true);
        }

        
        //protected async Task<ActionResult> UploadAttachment(AttachmentSet model)
        //{
        //    var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
        //    var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
        //    string fileId = "";
        //    //var result = "";
        //    try
        //    {
        //        var sb = new StringBuilder();
        //        //result = "1";
        //        sb.AppendFormat("Received image set {0}: ", model.Name);
        //        //result = "11";

        //        model.Images.ForEach(i =>
        //            sb.AppendFormat("Got image {0} of type {1} and size {2} bytes,", i.FileName, i.MimeType,
        //                i.StringData.Length)
        //            );
        //        //result = "111";

        //        //result = sb.ToString();

        //        var m = model.Images[0];

        //        fileId = Guid.NewGuid().ToString();
        //        string path = AppSettings.UploadPath(_iConfiguration);
        //        string fullpath = @"" + path + fileId + model.FileType;
        //        string contentType = model.Images[0].MimeType;
        //        //new FileExtensionContentTypeProvider().TryGetContentType(fullpath, out contentType);

        //        var fmodel = new FileViewModel
        //        {
        //            Id = fileId,
        //            DataAction = DataActionEnum.Create,
        //            AttachmentType = AttachmentTypeEnum.File.ToString(),
        //            FileName = m.FileName,
        //            FileExtension = model.FileType,
        //            ContentLength = 0,
        //            ContentType = contentType,
        //            //IsInPhysicalPath = false,
        //            ContentByte = Convert.FromBase64String(m.StringData),
        //            CreatedBy = model.UserId,
        //            ReferenceTypeId = model.NtsId,
        //            ReferenceTypeCode = model.NtsType
        //        };
        //        var result = await _fileBusiness.Create(fmodel);
        //        //if (model.IsNtsComments)
        //        //{
        //        //    LoadNtsComment(model.Comment, model.UserId, model.NtsId, fileId, model.NtsType);
        //        //}

        //        //}
        //    }
        //    catch (Exception e)
        //    {
        //        //result = e.ToString();
        //        return Ok(fileId);
        //    }


        //    return Ok(fileId);

        //}



    }
}
