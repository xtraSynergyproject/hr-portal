using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Nest;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Synergy.App.DataModel;
using System.Dynamic;

namespace Synergy.App.Api.Areas.BLS.Controllers
{
    [Route("bls/query")]
    [ApiController]
    public class QueryController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private IWebHostEnvironment Environment;

        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
        IServiceProvider serviceProvider, IWebHostEnvironment _environment) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Environment = _environment;
        }
        [HttpGet]
        [Route("ValidataPassportNumber")]
        public async Task<IActionResult> ValidataPassportNumber(string passportNumber)
        {
            try
            {
                string[] passportNumbers = passportNumber.Trim(',').Split(",").ToArray();
                
                foreach(var p in passportNumbers)
                {
                    if(p != "N5555555")
                    {
                        return Ok(new { success = false, error = "Given Passport Number " + p +" is Invalid" });
                    }
                }
                
                return Ok(new { success = true });               
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("PhotoFaceMatch")]
        public async Task<IActionResult> PhotoFaceMatch(string appointmentId,string imageId)
        {
            var serviceBusiness = _serviceProvider.GetService<IBLSBusiness>();
            var fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            try
            {
                HttpClient client = new HttpClient();
                var request = new MultipartFormDataContent();
                var image = await serviceBusiness.GetAppointmentDetailsByServiceId(appointmentId);
                if (image != null && image.ImageId != null)
                {
                    var f1 = await fileBusiness.GetFileByte(image.ImageId);
                    //var f1 = await fileBusiness.GetFileByte(imageId);
                    var f2 = await fileBusiness.GetFileByte(imageId);
                    var image_data1 = new MemoryStream(f1);//File.OpenRead("obama1.jpg");
                    var image_data2 = new MemoryStream(f2); //File.OpenRead("obama2.jpg");
                    request.Add(new StreamContent(image_data1), "image1", Path.GetFileName("test-image6.jpeg"));
                    request.Add(new StreamContent(image_data2), "image2", Path.GetFileName("test-image7.jpg"));
                    var output = await client.PostAsync("https://demo.aitalkx.com/deepstackai/v1/vision/face/match", request);
                    var jsonString = await output.Content.ReadAsStringAsync();
                    var result = JObject.Parse(jsonString);

                    var similiar = result.SelectToken("similarity");
                    if (similiar != null && double.Parse(similiar.ToString()) >= 0.7)
                    {
                        return Ok(new { success = true, similarity= similiar });
                    }
                }

                return Ok(new { success = false, similarity = 0});

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
