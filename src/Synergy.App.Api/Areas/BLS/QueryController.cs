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

                foreach (var p in passportNumbers)
                {
                    if (p != "N5555555")
                    {
                        return Ok(new { success = false, error = "Given Passport Number " + p + " is Invalid" });
                    }
                }

                return Ok(new { success = true });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("PhotoFaceMatch")]
        public async Task<IActionResult> PhotoFaceMatch(PhotoFaceModel model)
        {
            var serviceBusiness = _serviceProvider.GetService<IBLSBusiness>();
            var fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            try
            {
                HttpClient client = new HttpClient();
                var request = new MultipartFormDataContent();
                var image = await serviceBusiness.GetAppointmentDetailsByServiceNo(model.appointmentNo);
                if (image != null && image.ImageId != null)
                {
                    var f1 = await fileBusiness.GetFileByte(image.ImageId);
                    //var f1 = await fileBusiness.GetFileByte(imageId);
                    var f2 = Convert.FromBase64String(model.imageString);//await fileBusiness.GetFileByte(imageId);
                    var image_data1 = new MemoryStream(f1);//File.OpenRead("obama1.jpg");
                    var image_data2 = new MemoryStream(f2); //File.OpenRead("obama2.jpg");
                    request.Add(new StreamContent(image_data1), "image1", Path.GetFileName("test-image6.jpg"));
                    request.Add(new StreamContent(image_data2), "image2", Path.GetFileName("test-image7.jpg"));
                    var output = await client.PostAsync("https://demo.aitalkx.com/deepstackai/v1/vision/face/match", request);
                    var jsonString = await output.Content.ReadAsStringAsync();
                    var result = JObject.Parse(jsonString);

                    var similiar = result.SelectToken("similarity");
                    if (similiar != null && double.Parse(similiar.ToString()) >= 0.7)
                    {
                        return Ok(new { success = true, similarity = similiar });
                    }
                }

                return Ok(new { success = false, similarity = 0 });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetAppointmentDetail")]
        public async Task<ActionResult> GetAppointmentDetail(string appointmentNo)
        {
            var serviceBusiness = _serviceProvider.GetService<IBLSBusiness>();
            var appointment = await serviceBusiness.GetAppointmentDetailsByServiceNo(appointmentNo);
            return Ok(appointment);
        }

        [HttpGet]
        [Route("GetApplicationDetail")]
        public async Task<ActionResult> GetApplicationDetail(string appointmentNo)
        {
            var blsBusiness = _serviceProvider.GetService<IBLSBusiness>();
            var application = await blsBusiness.GetVisaApplicationDetailsByServiceNo(appointmentNo);
            return Ok(application);
        }


        [HttpPost]
        [Route("UpdateApplicationStatus")]
        public async Task<ActionResult> UpdateApplicationStatus(BLSApplicationStatusViewModel model)
        {
            var serviceBusiness = _serviceProvider.GetService<IBLSBusiness>();
            var lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
            var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();

            var appStatus = await lovBusiness.GetSingle(x => x.Code == model.Status);

            await serviceBusiness.UpdateApplicationStatus(model.Id, appStatus.Id);

            dynamic exo = new System.Dynamic.ExpandoObject();
            ((IDictionary<String, Object>)exo).Add("StatusId", appStatus.Id);
            ((IDictionary<String, Object>)exo).Add("Comment", model.Comment);
            ((IDictionary<String, Object>)exo).Add("UpdatedById", model.UpdatedById);
            ((IDictionary<String, Object>)exo).Add("ClientIp", model.ClientIp);
            ((IDictionary<String, Object>)exo).Add("ClientMacId", model.ClientMacId);
            ((IDictionary<String, Object>)exo).Add("ParentId", model.Id);

            var Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);

            await _cmsBusiness.CreateForm(Json, null, "BLSAPPLICANTDETAILS");
            
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("GetPassportDetail")]
        public async Task<ActionResult> GetPassportDetail(string passportNo)
        {
            var serviceBusiness = _serviceProvider.GetService<IBLSBusiness>();
            var passport = await serviceBusiness.GetPassportDetail(passportNo);
            if(passport!=null)
            {
                passport.IssueDateT = passport.IssueDate.Value.ToDD_YYYY_MM_DD();
                passport.ExpiryDateT = passport.ExpiryDate.Value.ToDD_YYYY_MM_DD();
                passport.DateOfBirthT = passport.DateOfBirth.Value.ToDD_YYYY_MM_DD();
            }
            return Ok(passport);
        }
        
        [HttpGet]
        [Route("IntegratePassportDetail")]
        public async Task<ActionResult> IntegratePassportDetail(string passportNo,string countryId)
        {
            var serviceBusiness = _serviceProvider.GetService<IBLSBusiness>();
            var apiDetail = await serviceBusiness.IntegratePassportDetail(countryId);

            try
            {
               
                using (var httpClient = new HttpClient())
                {
                    if (apiDetail != null && passportNo.IsNotNullAndNotEmpty())
                    {
                        var url = apiDetail.Api + "?passportNo=" + passportNo;
                        var address = new Uri(url);
                        // var json = GetJsonBody(passportNo);
                        //var j = JObject.Parse(json);
                        //var data = new StringContent(j.ToString(), Encoding.UTF8, "application/json");


                        var response = httpClient.GetAsync(address).Result;
                        var result = JsonConvert.DeserializeObject<BLSApplicantViewModel>(response.Content.ReadAsStringAsync().Result);
                        //var result = response.Content.ReadAsStringAsync().Result;
                        return Ok(result);
                    }

                    
                    return Ok(new BLSApplicantViewModel());
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        private string GetJsonBody(string passportNo)
        {
            var json = @"{
                      'passportNo': '" + passportNo + @"'
                     }
                        ";
            return json;
        }


    }

    public class PhotoFaceModel
    {
        public string appointmentNo { get; set; }
        public string imageString { get; set; }
    }
}
