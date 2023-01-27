using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class LOVController : ApplicationController
    {
        private ILOVBusiness _LOVBusiness;
        private IPageBusiness _pageBusiness;
        private ITemplateBusiness _templateBusiness;
        private ICustomTemplateBusiness _customTemplateBusiness;
        private readonly IConfiguration _configuration;
        private IHostingEnvironment _hostingEnvironment;
        public LOVController(ILOVBusiness LOVBusiness, IPageBusiness pageBusiness, ITemplateBusiness templateBusiness, ICustomTemplateBusiness customTemplateBusiness, IConfiguration configuration,
            IHostingEnvironment hostingEnvironment)
        {
            _pageBusiness = pageBusiness;
            _LOVBusiness = LOVBusiness;
            _templateBusiness = templateBusiness;
            _customTemplateBusiness = customTemplateBusiness;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CustomIndex(string pageId)
        {
            CustomTemplateViewModel model = new CustomTemplateViewModel();
            var page = await _pageBusiness.GetSingleById(pageId);
            if (page!=null) 
            {
                var template = await _templateBusiness.GetSingleById(page.TemplateId);
                if (template!=null)
                {
                    if (template.TemplateType==TemplateTypeEnum.Custom) 
                    {
                        model = await _customTemplateBusiness.GetSingle(x=>x.TemplateId==template.Id);
                        model.DataAction = DataActionEnum.Create;
                    }
                }
                ViewBag.PageTitle = page.Title;
            }
            return View(model);
        }
        [HttpGet]
        public async Task<JsonResult> GetListOfValueList(string type)
        {
            var data = await _LOVBusiness.GetList(x => x.LOVType == type && x.Status != StatusEnum.Inactive);
            return Json(data.OrderBy(x => x.SequenceOrder));
        }


        [HttpGet]
        public async Task<JsonResult> GetListOfValueByid(string ID)
        {
            var data = await _LOVBusiness.GetList(x => x.Id == ID && x.Status != StatusEnum.Inactive);
            return Json(data.OrderBy(x => x.SequenceOrder));
        }
        [HttpGet]
        public async Task<JsonResult> GetLOVDetailsByid(string Id)
        {
            var data = await _LOVBusiness.GetSingle(x => x.Id == Id && x.Status != StatusEnum.Inactive);
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllListOfValue(string type)
        {
            var data = await _LOVBusiness.GetList(x => x.Status != StatusEnum.Inactive);
            return Json(data.OrderBy(x => x.SequenceOrder));
        }
        public async Task<ActionResult> ReadData([DataSourceRequest] DataSourceRequest request, string lovTypeId)
        {
            var model = await _LOVBusiness.GetList();
            if (lovTypeId.IsNotNullAndNotEmpty())
            {
                var lov = await _LOVBusiness.GetSingleById(lovTypeId);
                model = await _LOVBusiness.GetList(x => x.LOVType == lov.Id, y => y.Parent);
                model.ForEach(x =>
                x.ParentName = x.ParentId.IsNotNullAndNotEmpty() ? _LOVBusiness.GetSingleById(x.ParentId).Result.Name : null);
            }

            var dsResult = model.OrderBy(x => x.SequenceOrder).ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<ActionResult> ReadData1(string lovTypeId)
        {
            var model = await _LOVBusiness.GetList();
            if (lovTypeId.IsNotNullAndNotEmpty())
            {
                var lov = await _LOVBusiness.GetSingleById(lovTypeId);
                model = await _LOVBusiness.GetList(x => x.LOVType == lov.Id, y => y.Parent);
                model.ForEach(x =>
                x.ParentName = x.ParentId.IsNotNullAndNotEmpty() ? _LOVBusiness.GetSingleById(x.ParentId).Result.Name : null);
            }

            var dsResult = model.OrderBy(x => x.SequenceOrder);
            return Json(dsResult);
        }
        public IActionResult Create(string jobAdvertisementId)
        {
            return View("Manage", new LOVViewModel
            {
                DataAction = DataActionEnum.Create,

            });
        }
        public async Task<IActionResult> Edit(string Id)
        {
            var ListOfValue = await _LOVBusiness.GetSingleById(Id);

            if (ListOfValue != null)
            {

                ListOfValue.DataAction = DataActionEnum.Edit;
                return View("Manage", ListOfValue);
            }
            return View("Manage", new LOVViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(LOVViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _LOVBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _LOVBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("Manage", model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageLOV(LOVViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _LOVBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _LOVBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true});

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

        }
        public async Task<IActionResult> Delete(string id)
        {
            await _LOVBusiness.Delete(id);
            return View("Index", new LOVViewModel());
        }

        public async Task<ActionResult> GetLOVIdNameList(string lovType)
        {
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            var model = await _LOVBusiness.GetList(x => x.LOVType == lovType);

            list = model.OrderBy(x => x.SequenceOrder).ThenBy(x => x.Name).Select(x => new IdNameViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code
            }).ToList();
            
            return Json(list);
        }


        [HttpPost]
        public async Task<ActionResult> ImportLOVs(IList<IFormFile> file)
        {
            var baseurl = _hostingEnvironment.WebRootPath;
            if (file != null)
            {
                var physicalPath = "";
                //List<string> uploadedFiles = new List<string>();
                foreach (IFormFile postedFile in file)
                {
                    string path = string.Concat(baseurl,"\\");
                    string fileName = Path.GetFileName(postedFile.FileName);
                    physicalPath = string.Concat(path, fileName);
                    using (FileStream stream = new FileStream(physicalPath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);                                               
                    }
                }
                var result =await UploadLOVDetails(physicalPath);
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
                if (result.Count >= 0)
                {
                    return Json(new { success = true, operation = DataActionEnum.Create.ToString(), result = result });
                }
                else
                {
                    return Json(new { success = false, errors = result });
                }

            }

            return Content("");

        }
        public async Task<List<string>> UploadLOVDetails(string physicalPath)
        {
            CommandResult<LOVViewModel> result;
            try
            {
                var LovList =await  _LOVBusiness.GetList();
                //var userlist = _userbusiness.GetList();
                var errorList = new List<string>();
                if (System.IO.File.Exists(physicalPath))
                {
                    var i = 0;                   
                    using (TextFieldParser parser = new TextFieldParser(physicalPath))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        parser.ReadFields();
                        while (!parser.EndOfData)
                        {
                            bool IsSuccess = true;
                            i++;
                            var fields = parser.ReadFields().ToList();
                            var leadmodel = new LOVViewModel
                            {
                                LOVType = fields[0],
                                Name = fields[1],
                                Code = fields[2],
                                Description = fields[3],
                               // ParentId = fields[4],
                                Status = fields[5].ToEnum<StatusEnum>(),
                            };

                            // Check Parent Id is valid or not
                            if (fields[4].IsNotNullAndNotEmpty())
                            {
                                var country = LovList.FirstOrDefault(e => e.Code == fields[4] );
                                if (country == null)
                                {
                                    errorList.Add(string.Concat("Parent code does not exist: ", fields[4]));
                                    IsSuccess = false;
                                }
                                else
                                {
                                    var lov = await _LOVBusiness.GetSingle(x => x.Code == fields[4]);
                                    if (lov.IsNotNull())
                                    {
                                        leadmodel.ParentId = lov.Id;
                                    }
                                   
                                }
                            }
                            if (fields[0].IsNotNullAndNotEmpty())
                            {
                                var lov = LovList.FirstOrDefault(e => e.Code == leadmodel.LOVType && e.LOVType == "LOV_TYPE");
                                if (lov == null)
                                {
                                    errorList.Add(string.Concat("LOV Type Code does not exist: ", leadmodel.LOVType));
                                    IsSuccess = false;
                                }                               
                            }

                            if (IsSuccess)
                            {
                                result = await _LOVBusiness.Create(leadmodel);
                                if (result.IsSuccess)
                                {

                                }
                                else
                                {
                                    errorList.Add("Row " + i + " does not get inserted."+ result.Messages.Values.FirstOrDefault()+ " \n");
                                }
                            }
                            else
                            {
                                errorList.Add("Row " + i + " does not get inserted \n");
                            }
                        }
                        return errorList;
                    }
                }
                return errorList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult DownloadLOVSampleFile()
        {
            var baseurl = _hostingEnvironment.WebRootPath;
            string path = string.Concat(baseurl, "\\");
            string filePath =string.Concat(path, "LOVSampleFile.csv");
            //string fullName = Server.MapPath("~" + filePath);

            byte[] fileBytes = GetFile(filePath);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "LOVSampleFile.csv");
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        [HttpGet]
        public async Task<JsonResult> GetLOVDetailsByCode(string code)
        {
            var data = await _LOVBusiness.GetSingle(x => x.Code == code && x.Status != StatusEnum.Inactive);
            return Json(data);
        }

    }
}
