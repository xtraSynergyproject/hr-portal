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

namespace Synergy.App.Api.Areas.Common.Controllers
{
    [Route("common/query")]
    [ApiController]
    public class QueryController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        //private readonly IConfiguration _configuration;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private IWebHostEnvironment Environment;

        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
          IServiceProvider serviceProvider, IWebHostEnvironment _environment) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Environment = _environment;
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
        [HttpGet]
        [Route("GetRoipLastDate")]
        public async Task<IActionResult> GetRoipLastDate()
        {
            List<string> tables = new List<string>();
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _configuration = _serviceProvider.GetService<IConfiguration>();
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var query = ApplicationConstant.BusinessAnalytics.MaxDateQuery;
            query = query.Replace("#FILTERCOLUMN#", "date_value");
            var queryContent = new StringContent(query, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "testroip/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, queryContent);
                if (response.IsSuccessStatusCode)
                {                    
                    var _jsondata = await response.Content.ReadAsStringAsync();
                    var _dataToken = JToken.Parse(_jsondata);
                    var _responsedata = _dataToken.SelectToken("aggregations");
                    var _maxdateToken = _responsedata.SelectToken("max_date");
                    var _dateToken = _maxdateToken.Last();
                    var _date = _dateToken.Last();
                    var date = _date.Value<DateTime>();
                    var _newDate = date.AddDays(1);                    
                    var url1 = eldbUrl + "roiptables/_search?size=1000";
                    var address1 = new Uri(url1);
                    var response1 = await httpClient.GetAsync(address1);
                    if (response1.IsSuccessStatusCode)
                    {
                        var _jsondata1 = await response1.Content.ReadAsStringAsync();
                        var _dataToken1 = JToken.Parse(_jsondata1);
                        if (_dataToken1.IsNotNull())
                        {
                            var hits = _dataToken1.SelectToken("hits");
                            if (hits.IsNotNull())
                            {                                
                                var _hits = hits.SelectToken("hits");
                                foreach (var hit in _hits)
                                {                                   
                                    var source = hit.SelectToken("_source");
                                    if (source.IsNotNull())
                                    {
                                        var _table_deepvlg = source.SelectToken("tables_in_deepvlg");
                                        if (_table_deepvlg.IsNotNull())
                                        {
                                            var _table = _table_deepvlg.Value<string>();
                                            tables.Add(_table);
                                        }
                                        
                                    }



                                }
                            }
                        }
                    }
                    if (date == DateTime.Now.Date || (date.AddDays(1) == DateTime.Now.Date && date.AddDays(1).AddHours(2) < DateTime.Now))
                    {
                        var newDate1 = date.ToString("yy-MM-dd");
                        return Ok(newDate1.Replace("-", "_"));
                    }                    
                    var dateStr= await GetRoipDate(tables, _newDate);
                    return Ok(dateStr);

                }               

            }            
            return Ok();
        }

        private async Task<string> GetRoipDate(List<string> tables,DateTime date)
        {

            if(date==DateTime.Now.Date || date > DateTime.Now.Date)
            {
                var newDate1 = DateTime.Now.ToString("yy-MM-dd");
                return newDate1.Replace("-", "_");
            }            
            var newDate = date.ToString("yy-MM-dd");
            var dateStr = newDate.Replace("-", "_");
            if (tables.Where(x => x == dateStr).Any())
            {
                return dateStr;
            }
            else
            {
               return await GetRoipDate(tables,date.AddDays(1));
            }
            
         
        }

        [HttpGet]
        [Route("GetMigrationScripts")]
        public async Task<IActionResult> GetRemainingMigrationScripts(string lastMigrationScriptName)
        {
            try
            {
                // Fetching all the migration names
                var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
                List<string> migrationNames = await _cmsBusiness.GetAllMigrationsList();

                // Fetching path to access files
                string contentPath = this.Environment.WebRootPath;
                DirectoryInfo info = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, "MigrationScript"));

                List<FileInfo> f = info.GetFiles().ToList();

                var index = migrationNames.IndexOf(lastMigrationScriptName);

                if (index != -1)
                {
                    // Fetching remaining migrations
                    List<FileInfo> reqd = new List<FileInfo>();
                    for (var x = index + 1; x < migrationNames.Count; x++)
                    {
                        var str = migrationNames[x] + ".sql";
                        int idx = f.FindIndex(obj => obj.Name == str);
                        if (idx != -1)
                        {
                            reqd.Add(f[idx]);
                        }
                    }

                    // Reading file data
                    string scriptData = "";
                    foreach (FileInfo file in reqd)
                    {
                        var data = System.IO.File.ReadAllText(file.FullName);
                        scriptData = string.Concat(scriptData, data);

                    }

                    if (scriptData.IsNullOrEmpty()) {
                        return Ok("No Pending Migration");
                    }

                    return Ok(scriptData);
                }
                else
                {
                    return Ok("Migration not present");
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetTemplatesList")]
        public async Task<IActionResult> GetTemplatesListByTemplateType(string templateType) 
        {
            var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
            TemplateTypeEnum templateTypeName = (TemplateTypeEnum)Enum.Parse(typeof(TemplateTypeEnum), templateType, true);
            int i = (int)(object)(templateTypeName);
            List<TemplateViewModel> list = await _cmsBusiness.GetTemplateListByTemplateType(i);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetTemplateData")]
        public async Task<IActionResult> GetTemplateDataById(string id) 
        {
            var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
            var templateData = await _cmsBusiness.GetTemplateCompleteDataById(id);
            return Ok(templateData);
        }
    }
}
