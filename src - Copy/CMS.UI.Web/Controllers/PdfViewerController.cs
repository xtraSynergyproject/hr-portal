using CMS.Business;
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ActiveQueryBuilder.Core.Commands;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AutoMapper;
using CMS.Common.Utilities;
using Microsoft.AspNetCore.Identity;
using AutoMapper.Configuration;
using System.Collections;
using CMS.UI.Web;
using System.Reflection;
using System.Net.Http;
using System.Net;
using Syncfusion.EJ2.PdfViewer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Syncfusion.Presentation;
using Syncfusion.PresentationToPdfConverter;
using Syncfusion.Pdf;
using CMS.UI.Utility;

namespace CMS.UI.Web.Controllers
{
    public class PdfViewerController : ApplicationController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        //Initialize the memory cache object.
        public IMemoryCache _cache;
        private readonly IFileBusiness _fileBusiness;
        private readonly IServiceProvider _sp;

        public PdfViewerController(IHostingEnvironment hostingEnvironment, IMemoryCache cache, IFileBusiness fileBusiness, IServiceProvider sp)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = cache;
            _fileBusiness = fileBusiness;
            _sp = sp;
            Console.WriteLine("PdfViewerController initialized");
        }

        //public PdfViewerController(IFileBusiness fileBusiness)
        //{
        //    _fileBusiness = fileBusiness;
        //    Console.WriteLine("PdfViewerController initialized");
        //}

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/Load")]
        public IActionResult Load([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            // PdfRenderer pdfviewer = new PdfRenderer();
            MemoryStream stream = new MemoryStream();
            object jsonResult = new object();
            if (jsonObject != null && jsonObject.ContainsKey("document"))
            {
                var VersionNo = jsonObject["elementId"].ToString().Split('_')[1];
                FileViewModel fileType = new FileViewModel();
                byte[] bytes;
                if (VersionNo.IsNotNullAndNotEmpty())
                {
                    fileType = _fileBusiness.GetFileLogsDetailsByFileIdAndVersion(jsonObject["document"].ToString(), Convert.ToInt64(VersionNo)).Result;
                    bytes = _fileBusiness.DownloadMongoFileByte(fileType.MongoFileId).Result;
                }
                else 
                {
                    fileType = _fileBusiness.GetSingleById(jsonObject["document"].ToString()).Result;
                    bytes= _fileBusiness.GetFileByte(jsonObject["document"].ToString()).Result;
                }
               
                if (fileType != null && fileType.FileExtension.ToLower().Contains("ppt"))
                {
                   // var bytes1 = _fileBusiness.GetFileByte(jsonObject["document"].ToString()).Result;
                    stream = new MemoryStream(bytes);
                    var s2 = new MemoryStream();
                    using (IPresentation pptxDoc = Presentation.Open(stream))
                    {
                        //Initialize 'ChartToImageConverter' to convert charts in the slides, and this is optional
                        PdfDocument pdfDocument = PresentationToPdfConverter.Convert(pptxDoc);
                        pdfDocument.Save(s2);
                    }
                    jsonResult = pdfviewer.Load(s2, jsonObject);
                }
                else
                {
                 // var bytes = _fileBusiness.GetFileByte(jsonObject["document"].ToString()).Result;
                  stream = new MemoryStream(bytes);
                    jsonResult = pdfviewer.Load(stream, jsonObject);
                }
            }
            
            return Content(JsonConvert.SerializeObject(jsonResult));
        }
        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/ImportAnnotations")]
        public ActionResult ImportAnnotations([FromBody] Dictionary<string, string> jsonObject)
        {
            //PdfRenderer pdfviewer = new PdfRenderer();
            //string jsonResult = string.Empty;
            ////var jsonData = JsonConverter(jsonObject);
            //if (jsonObject != null && jsonObject.ContainsKey("fileName"))
            //{
            //    string documentPath = GetDocumentPath(jsonObject["fileName"]);
            //    if (!string.IsNullOrEmpty(documentPath))
            //    {
            //        jsonResult = System.IO.File.ReadAllText(documentPath);
            //    }
            //    else
            //    {
            //        return this.Content(jsonObject["document"] + " is not found");
            //    }
            //}
            //return Content(JsonConvert.SerializeObject(jsonResult));

            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            string jsonResult = string.Empty;
            object JsonResult;
            if (jsonObject != null && jsonObject.ContainsKey("fileName"))
            {
                string documentPath = GetDocumentPath(jsonObject["fileName"]);
                if (!string.IsNullOrEmpty(documentPath))
                {
                    jsonResult = System.IO.File.ReadAllText(documentPath);
                }
                else
                {
                    return this.Content(jsonObject["document"] + " is not found");
                }
            }
            else
            {
                var fileType = _fileBusiness.GetSingleById(jsonObject["document"].ToString()).Result;
                string extension = Path.GetExtension(jsonObject["importedData"]);
                if (extension != ".xfdf")
                {
                    JsonResult = pdfviewer.ImportAnnotation(jsonObject);
                    return Content(JsonConvert.SerializeObject(JsonResult));
                }
                else
                {
                    string documentPath = GetDocumentPath(jsonObject["importedData"]);
                    if (!string.IsNullOrEmpty(documentPath))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(documentPath);
                        jsonObject["importedData"] = Convert.ToBase64String(bytes);
                        JsonResult = pdfviewer.ImportAnnotation(jsonObject);
                        return Content(JsonConvert.SerializeObject(JsonResult));
                    }
                    else
                    {
                        return this.Content(jsonObject["document"] + " is not found");
                    }
                }
            }
            return Content(jsonResult);
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/RenderPdfPages")]
        public IActionResult RenderPdfPages([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object jsonResult = pdfviewer.GetPage(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/Unload")]
        public IActionResult Unload([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            pdfviewer.ClearCache(jsonObject);
            return this.Content("Document cache is cleared");
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/RenderThumbnailImages")]
        public IActionResult RenderThumbnailImages([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object result = pdfviewer.GetThumbnailImages(jsonObject);
            return Content(JsonConvert.SerializeObject(result));
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/Bookmarks")]
        public IActionResult Bookmarks([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object jsonResult = pdfviewer.GetBookmarks(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/Download")]
        public async Task<IActionResult> Download([FromBody] Dictionary<string, string> jsonObject)
        {
            #region noor Code
            ////PdfRenderer pdfviewer = new PdfRenderer(_cache);
            ////string documentBase = pdfviewer.GetDocumentAsBase64(jsonObject);
            ////return Content(documentBase);
            //PdfRenderer pdfviewer = new PdfRenderer();
            ////  var jsonData = JsonConverter(jsonObject);

            //var jsonResult = pdfviewer.GetAnnotationComments(jsonObject);

            ////jsonResult = jsonResult.Replace("data:application/json;base64,", "");

            ////var fileId = jsonData["elementId"].Trim().Split('_')[1].ToSafeLong();
            //var fileId = jsonObject["documentId"].ToString();

            //// var _fileBusiness = BusinessHelper.GetInstance<IFileBusiness>();

            //var doc = _fileBusiness.GetSingleById(fileId).Result;

            //if (doc != null)
            //{
            //    doc.AnnotationsText = jsonObject["stickyNotesAnnotation"].Trim();
            //    doc.LastUpdatedDate = DateTime.Now;
            //    doc.DataAction = DataActionEnum.Edit;
            //    _fileBusiness.Edit(doc);
            //}
            //return null;
            #endregion

            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            
            string documentBase = pdfviewer.GetDocumentAsBase64(jsonObject);
             var documentBase1 = documentBase.Replace("data:application/pdf;base64,", "").Trim();           
            var fileId = jsonObject["documentId"].ToString();
            var doc = _fileBusiness.GetSingleById(fileId).Result;
            byte[] bytes;
            bytes = Convert.FromBase64String(documentBase1.Trim());
            if (doc != null)
            {
                doc.ContentByte = bytes;
                //doc.AnnotationsText = jsonObject["stickyNotesAnnotation"].Trim().ToString();
                doc.LastUpdatedDate = DateTime.Now;
                doc.DataAction = DataActionEnum.Edit;
               await _fileBusiness.Edit(doc);
            }
            return null;
        }

        private string GetDocumentPath(string document)
        {
            string documentPath = string.Empty;
            if (!System.IO.File.Exists(document))
            {
                string basePath = _hostingEnvironment.WebRootPath;
                string dataPath = string.Empty;
                dataPath = basePath + @"/Data/";
                if (System.IO.File.Exists(dataPath + document))
                    documentPath = dataPath + document;
            }
            else
            {
                documentPath = document;
            }
            return @"D:\Downloads\JanusServerLoad.pdf";
        }
        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/ExportAnnotations")]
        public async Task<object> ExportAnnotations([FromBody] Dictionary<string, string> jsonObject)
        {
            #region noor Code
            //  PdfRenderer pdfviewer = new PdfRenderer();
            ////  var jsonData = JsonConverter(jsonObject);

            //  var jsonResult = pdfviewer.GetAnnotationComments(jsonObject);

            //  //jsonResult = jsonResult.Replace("data:application/json;base64,", "");

            //  //var fileId = jsonData["elementId"].Trim().Split('_')[1].ToSafeLong();
            //  var fileId = jsonObject["document"].ToString();

            // // var _fileBusiness = BusinessHelper.GetInstance<IFileBusiness>();

            //  var doc = _fileBusiness.GetSingleById(fileId).Result;

            //  if (doc != null)
            //  {
            //      doc.AnnotationsText = jsonObject["pdfAnnotation"].Trim();
            //      doc.LastUpdatedDate = DateTime.Now;
            //      doc.DataAction = DataActionEnum.Edit;
            //      _fileBusiness.Edit(doc);
            //  }
            #endregion
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            string jsonResult = pdfviewer.ExportAnnotation(jsonObject);
           // string jsonResult1 = jsonResult.Replace("data:application/json;base64,", "").Trim();


            var fileId = jsonObject["document"].ToString();

            //byte[] bytes;
            //bytes = Convert.FromBase64String(jsonResult1);

            var doc = _fileBusiness.GetSingleById(fileId).Result;
           
            if (doc != null)
            {     
                doc.AnnotationsText = jsonObject["pdfAnnotation"].Trim();
                doc.LastUpdatedDate = DateTime.Now;
                doc.DataAction = DataActionEnum.Edit;
                await  _fileBusiness.Edit(doc);
            }
            return null; 


            //var fileName = Path.GetFileName(doc.FileName);



            //_fileBusiness.Correct(new FileViewModel
            //{
            //    AttachmentType = AttachmentTypeEnum.File,
            //    FileName = fileName,
            //    FileExtension = doc.FileExtension,
            //    ContentType = doc.ContentType,
            //    ContentLength = doc.ContentLength,
            //    AnnotationsText = jsonData["pdfAnnotation"].Trim(),
            //    MongoFileId = doc.MongoFileId,
            //    Id = fileId
            //    //AttachmentDescription = desc
            //});

            ////return Content(JsonConvert.SerializeObject(jsonResult));
            //return null;

        }
        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("api/[controller]/RenderAnnotationComments")]
        public IActionResult RenderAnnotationComments([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object jsonResult = pdfviewer.GetAnnotationComments(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }
    }
}
