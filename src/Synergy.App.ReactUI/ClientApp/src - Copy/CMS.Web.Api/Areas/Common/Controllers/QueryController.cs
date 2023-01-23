using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using CMS.Web.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace CMS.Web.Api.Areas.Common.Controllers
{
    [Route("common/query")]
    [ApiController]
    public class QueryController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;


        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
          IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        [HttpGet]
        [Route("GetFile")]
        public async Task<IActionResult> GetFile(string fileId)
        {
            try
            {
                var fileBusiness = _serviceProvider.GetService<IFileBusiness>();
                var image = await fileBusiness.GetFileByte(fileId);
                if (image != null)
                {
                    return File(image, "image/jpeg");
                }
                return new EmptyResult();
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("InsertAccessLog")]
        public async Task<IActionResult> InsertAccessLog(string userId, DateTime punchingTime, PunchingTypeEnum punchingType)
        {
            try
            {
                await Authenticate(userId);
                var _context = _serviceProvider.GetService<IUserContext>();
                var business = _serviceProvider.GetService<IHRCoreBusiness>();
                var result = await business.UpdateAccessLogDetail(userId, punchingTime, punchingType, DeviceTypeEnum.Mobile, null);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("GetSynergySchema")]
        public async Task<IActionResult> GetSynergySchema() 
        {
            var business = _serviceProvider.GetService<INoteBusiness>();
            var Result = await business.GetSyneryList();
            //var metaList = Result.Take(2).Select(x => x.Metadata);
            var cubeList = new List<CubeJsViewModel>();
            foreach (var item in Result)
            {                
                    var cube = new CubeJsViewModel();
                    var dimensionList = new List<DimensionsViewModel>();
                    cube.name = item.SchemaName;
                    cube.datasource = item.ElsasticDB ? "elasticsearch" : null;
                    var childs = await business.GetList(x => x.ParentNoteId == item.Id);
                    foreach (var child in childs)
                    {
                        dimensionList.Add(new DimensionsViewModel { name = child.NoteSubject, dataType = child.NoteDescription });
                    }
                    cube.dimensions = dimensionList;
                    cube.sql = item.Query.Replace("^", "'");
                    cubeList.Add(cube);                              
            }


            return Ok(cubeList);

        }
        [HttpGet]
        [Route("GetLastUpdatedSynergySchemaDate")]
        public async Task<IActionResult> GetLastUpdatedSynergySchemaDate()
        {
            var business = _serviceProvider.GetService<INoteBusiness>();
            var date = await business.GetLastUpdatedSynerySchema();
            var newDate= date.ToString("yyMMddHHmmssff");
            return Ok(newDate);
        }

    }
}
