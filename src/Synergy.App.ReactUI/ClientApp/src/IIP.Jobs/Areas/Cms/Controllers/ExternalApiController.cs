using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.CMS.Controllers
{
    [Route("cms/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ExternalApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;

       
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public ExternalApiController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IConfiguration configuration
          )
        {
            _customUserManager = customUserManager;
            _configuration = configuration;
         

        }

       
        [HttpGet]
        [Route("GetPurchaseOrder")]
        public IActionResult GetPurchaseOrder()
        {
            try
            {
                //var list = await _lovBusiness.GetList(x => x.LOVType == lovType);

               // var result = list.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
                //return Ok(result);

                Console.WriteLine("GetPurchaseOrder!");
                using (var httpClient = new HttpClient())
                {
                    var url = _configuration.GetValue<string>("ExternalWebApiUrl");
                    var address = new Uri(url+"webservices/rest/PO_DATA_PKG/main/");
                    var json = GetJsonBody("'P_VENDOR_ID': '415'");
                    var j = JObject.Parse(json);
                    var data = new StringContent(j.ToString(), Encoding.UTF8, "application/json");
                    var val = GetAuthorization();
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
                  
                    var response = httpClient.PostAsync(address, data).Result;
                    //var result = JsonConvert.DeserializeObject<List<SYNERGY_Service>>(response.Content.ReadAsStringAsync().Result);
                    //var result = response.Content.ReadAsStringAsync().Result;


                    dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(response.Content.ReadAsStringAsync().Result);
                    //foreach (var enabledEndpoint in ((IEnumerable<dynamic>)result.P_PO_DATA_ITEM))
                    //{
                    //    Console.WriteLine($"{enabledEndpoint.name} is enabled");
                    //}
                    dynamic res1 = (ExpandoObject)result.OutputParameters;
                    dynamic res2 = (ExpandoObject)res1.P_PO_DATA;
                    var res3 = (IEnumerable<dynamic>)res2.P_PO_DATA_ITEM;
                    var dataRes = res3.Select(x => new IdNameViewModel { Id = x.PO_NUMBER, Name = x.PO_NUMBER }).ToList();
                    return Ok(dataRes);
                    //return Ok(result);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private string GetJsonBody(string input)
        {
            var json = @"{
                            'PO_DATA_PKG': {
                                '@xmlns': 'http://xmlns.oracle.com/apps/po/rest/PO_DATA_PKG/main',
                                'RESTHeader': {
                                    'xmlns': 'http://xmlns.oracle.com/apps/fnd/rest/header',
                                    'Responsibility': 'PURCHASING_SUPER_USER',
                                    'RespApplication': 'PO',
                                    'SecurityGroup': 'STANDARD',
                                    'NLSLanguage': 'AMERICAN',
                                    'Language': 'American English',
                                    'Org_Id': '204'
                                },
                                'InputParameters': {
                                    
                                    "+input+@"
                                }
                            }
                        }
                        ";
            return json;
        }
        private string GetAuthorization()
        {
            var username = _configuration.GetValue<string>("ExternalWebApiUrl-Username");
            var password = _configuration.GetValue<string>("ExternalWebApiUrl-Password");
            var plainTextBytes = Encoding.UTF8.GetBytes(username+":"+ password);
            string val = Convert.ToBase64String(plainTextBytes);
            return val;
        }
    }
}
